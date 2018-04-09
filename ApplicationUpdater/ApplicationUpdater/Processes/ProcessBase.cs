
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected virtual void UpdateProcess(string msg, bool isNewLine = true)
        {
            var p = new ConsoleWriteProcess
            {
                Msg = $"[{this.Name}] {msg}" ,
                NewLine = isNewLine
            };

            ProcessEvent(p, new EventArgs { });
        }

        protected virtual void CopyAll(string rootsourcePath, DirectoryInfo source, DirectoryInfo target, bool overrideFile, string msgFormat, IEnumerable<string> excludePath = null)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                //[TODO] Musi to byc bardziej zaawansowane. Wildcards
                if (excludePath != null && excludePath.Any(s => fi.FullName.Contains(s)))
                {
                    continue;
                }

                fi.CopyTo(Path.Combine(target.FullName, fi.Name), overrideFile);

                UpdateProcess(string.Format(msgFormat,   fi.FullName.Replace(rootsourcePath, string.Empty)));
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(rootsourcePath, diSourceSubDir, nextTargetSubDir, overrideFile, msgFormat, excludePath);
            }
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
