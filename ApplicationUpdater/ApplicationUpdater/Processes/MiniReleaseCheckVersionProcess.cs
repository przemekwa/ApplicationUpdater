using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ApplicationUpdater.Processes
{
    public class MiniReleaseCheckVersionProcess : ProcessBase, IProcess<UpdateModel>
    {
        private IEnvironmentManager environmentManager;
        public IEnumerable<string> ExcludeDir { get; set; }

        public MiniReleaseCheckVersionProcess(IConfigurationRoot configurationRoot, IEnvironmentManager environmentManager) : base(configurationRoot, "Checking the files")
        {
            this.environmentManager = environmentManager;
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            ExcludeDir = GetExcludeFiles(model.UserParams.IntepubDirectory.FullName);

            var inetpubFiles = Directory.GetFiles(model.UserParams.IntepubDirectory.FullName, "*.*", SearchOption.AllDirectories)
                .Where(s => ExcludeDir.Any(d => s.Contains(d)) == false)
                .Select(f => new
                {
                    File = new FileInfo(f),
                    RelativePath = f.Replace(model.UserParams.IntepubDirectory.FullName, "")
                });


            var newAppFiles = this.ConfigurationRoot
                 .GetSection("MiniRelease.FileList")?
                 .GetChildren()?
                 .Select(s => new
                 {
                     File = new FileInfo(Path.Combine(model.NewApplicationDirectory.FullName, s.Value)),
                     RelativePath = $"\\{s.Value}"
                 });

            if (!inetpubFiles.Any() || !newAppFiles.Any())
            {
                throw new Exception("No files to compare.");
            }

            var error = false;

            foreach (var newfile in newAppFiles)
            {

                var inetpubFile = inetpubFiles.SingleOrDefault(s => s.RelativePath.Equals(newfile.RelativePath, StringComparison.InvariantCultureIgnoreCase));

                if (inetpubFile == null)
                {
                    UpdateProcess($"No file in the inetpub dir {newfile.RelativePath}");
                    continue;
                }

                if (inetpubFile.File.CreationTime >= newfile.File.CreationTime)
                {
                    UpdateProcess($"A newer file is loaded in the destination directory {newfile.File.FullName}");
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
