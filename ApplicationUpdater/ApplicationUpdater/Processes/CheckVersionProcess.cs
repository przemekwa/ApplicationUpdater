using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ApplicationUpdater.Processes
{
    public class CheckVersionProcess: ProcessBase, IProcess<UpdateModel>
    {
        private IEnvironmentManager environmentManager;
        public IEnumerable<string> ExcludeDir { get; set; }

        public CheckVersionProcess(IConfigurationRoot configurationRoot, IEnvironmentManager environmentManager) : base(configurationRoot, "Checking the files")
        {
            this.environmentManager = environmentManager;
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            ExcludeDir = GetExcludeFiles(model.UserParams.IntepubDirectory.FullName);

            var inetpubFiles = Directory.GetFiles(model.UserParams.IntepubDirectory.FullName, "*.*", SearchOption.AllDirectories)
                .Where(s=> ExcludeDir.Any(d=> s.Contains(d)) == false)
                .Select(f => new FileInfo(f));

            var newAppDirectory = model.UnZipDirectory.FullName;

            var newAppFiles = Directory.GetFiles(newAppDirectory, "*.*", SearchOption.AllDirectories)
                .Select(s=>new FileInfo(s));

            if (!inetpubFiles.Any() || !newAppFiles.Any())
            {
                throw new Exception("No files to compare.");
            }

            var error = false;

            foreach (var inetpubFile in inetpubFiles)
            {
                var fileNameToCheck = inetpubFile.FullName.Replace(model.UserParams.IntepubDirectory.FullName, string.Empty);

                var file = newAppFiles.SingleOrDefault(s => s.FullName.Replace(model.UnZipDirectory.FullName + "\\app", "").Equals(fileNameToCheck, StringComparison.CurrentCultureIgnoreCase));

                if (file == null)
                {
                    UpdateProcess($"No file in the new application {fileNameToCheck}");
                    error = true;
                    continue;
                }

                if (inetpubFile.CreationTime >= file.CreationTime)
                {
                    UpdateProcess($"A newer file is loaded in the destination directory {inetpubFile.FullName}");
                    error = true;
                }
            }

            if (error && Confirm("Errors occurred while checking files. Do you want to continue?") == false)
            {
                environmentManager.Exit(0);
            }

            return GetProcesEventResult(Consts.ProcesEventResult.Successful);

        }
    }
}
