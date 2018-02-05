using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class CheckVersionProcess: ProcessBase, IProcess<UpdateModel>
    {
        public ProcesEventResult Process(UpdateModel model)
        {
            var inetpubFiles = Directory.GetFiles(model.IntepubDirectory.FullName, "*.*", SearchOption.AllDirectories)
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
                var fileNameToCheck = inetpubFile.FullName.Replace(model.IntepubDirectory.FullName, string.Empty);

                var file = newAppFiles.SingleOrDefault(s => s.FullName.Replace(model.UnZipDirectory.FullName + "\\app\\", "").Equals(fileNameToCheck, StringComparison.CurrentCultureIgnoreCase));

                if (file == null)
                {
                    UpdateProcess($"No file in the new application {inetpubFile.FullName}");
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
                Environment.Exit(0);
            }

            return null;

        }
    }
}
