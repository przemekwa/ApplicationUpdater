
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ApplicationUpdater.Processes
{
    public abstract class ProcessBase
    {
        public event EventHandler ProcessEvent;
        public event EventHandler ConfirmEvent;
        public IConfigurationRoot ConfigurationRoot { get; set; }

        public string Name { get; set; }


        public ProcesEventResult GetProcesEventResult(string msg)
        {
            return new ProcesEventResult
            {
                Result = $"[{this.Name}] {msg}"
            };
        }

        protected ProcessBase(IConfigurationRoot configurationRoot, string name)
        {
            ConfigurationRoot = configurationRoot;
            Name = name;
        }

        public ProcessBase(string name)
        {
            this.Name = name;
        }

        protected virtual bool Confirm(string question)
        {
            var processConfirmation = new ProcessConfirmation
            {
                Question = $"[{this.Name}] {question}"
            };

            ConfirmEvent(processConfirmation, new EventArgs());

            return processConfirmation.Key == ConsoleKey.Y;
        }

        protected virtual void UpdateProcess(string msg, bool isNewLine = true, bool oneLine = false)
        {
            var p = new ConsoleWriteProcess
            {
                Msg = $"[{this.Name}] {msg}",
                NewLine = isNewLine,
                OneLineMode = oneLine
            };

            ProcessEvent(p, new EventArgs { });
        }  
        
        protected virtual void UpdateProcessWithPgoressBar(double stepNumberProgress, double maxProgress,   string msg, bool isNewLine = true, bool oneLine = false)
        {
            var p = new ConsoleWriteProcess
            {
                Msg = $"[{this.Name}] " ,
                NewLine = isNewLine,
                OneLineMode = oneLine,
                SetNewProgress = false,
                ShowProgress = true,
                StepNumberProgress = stepNumberProgress,
                MaxProgress = maxProgress
            };

            ProcessEvent(p, new EventArgs { });
        }

        protected virtual double CopyAll(double count, double maxProgress, string rootsourcePath, DirectoryInfo source, DirectoryInfo target, bool overrideFile, string msgFormat, IEnumerable<string> excludePath = null)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                if (excludePath != null && excludePath.Any(s => fi.FullName.Contains(s)))
                {
                    continue;
                }

                fi.CopyTo(Path.Combine(target.FullName, fi.Name), overrideFile);

                UpdateProcessWithPgoressBar(++count, maxProgress, "", false);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                count = CopyAll(count, maxProgress, rootsourcePath, diSourceSubDir, nextTargetSubDir, overrideFile, msgFormat, excludePath);
            }

            return count;
        }

        protected virtual int CountAll(DirectoryInfo source, IEnumerable<string> excludePath = null)
        {
            var count = 0;

            foreach (FileInfo fi in source.GetFiles())
            {
                if (excludePath != null && excludePath.Any(s => fi.FullName.Contains(s)))
                {
                    continue;
                }
                count++;
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                count+= CountAll(diSourceSubDir, excludePath);
            }

            return count;
        }

            public IEnumerable<string> GetExcludeFiles(string intepubDirectoryPath)
        {
            return ConfigurationRoot
                  .GetSection("CheckVersionProcess.ExcludeDirectories")
                  .GetChildren()
                  .Select(x => Path.Combine(intepubDirectoryPath, x.Value))
                  .ToList();
        }
    }
}
