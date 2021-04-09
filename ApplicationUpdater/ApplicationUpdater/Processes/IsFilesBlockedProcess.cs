using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApplicationUpdater.Processes
{
    public class IsFilesBlockedProcess : ProcessBase, IProcess<UpdateModel>
    {
        private readonly IEnvironmentManager environmentManager;

        public IsFilesBlockedProcess(IConfigurationRoot configurationRoot,IEnvironmentManager environmentManager) : base(configurationRoot, "Checking files for blocking")
        {
            this.environmentManager = environmentManager;
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            var excludePath = this.GetExcludeFiles(model.UserParams.IntepubDirectory.FullName);
            
            var result = CheckPath(model.UserParams.IntepubDirectory, excludePath);

            foreach (var item in result)
            {
                UpdateProcess($"This file is blocked {item.Name}", true, false);
            }

            if (result.Any() && Confirm("Some file(s) are blocked. Do you want to continue?") == false)
            {
                environmentManager.Exit(0);
            }

            return GetProcesEventResult(Consts.ProcesEventResult.Successful);
        }

        public List<FileInfo> CheckPath(DirectoryInfo pathToDirectory, IEnumerable<string> excludePath)
        {
            var blockedFileList = new List<FileInfo>();

            foreach (FileInfo fi in pathToDirectory.GetFiles())
            {
                if (excludePath != null && excludePath.Any(s => fi.FullName.Contains(s)))
                {
                    continue;
                }

                if (IsFileLocked(fi))
                {
                    blockedFileList.Add(fi);
                }
            }

            foreach (DirectoryInfo diSourceSubDir in pathToDirectory.GetDirectories())
            {
                blockedFileList.AddRange(CheckPath(diSourceSubDir, excludePath));
            }

            return blockedFileList;
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                stream.Close();
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }
    }
}
