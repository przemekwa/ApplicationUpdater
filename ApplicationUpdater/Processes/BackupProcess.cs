using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class BackupProcess : ProcessBase, IProcess<UpdateModel>
    {
        public ProcesEventResult Process(UpdateModel model)
        {
            var backupDirectory = Directory.CreateDirectory(Path.Combine(model.BackupDirectory, "old-application"));

            CopyAll(new DirectoryInfo(model.IntepubDirectory), backupDirectory);

            UpdateProcess($"backup wykonany pomyślnie");

            return null;
        }

        public void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            
            foreach (FileInfo fi in source.GetFiles())
            {
                UpdateProcess($"Backup pliku {fi.Name}");

                fi.CopyTo(Path.Combine(target.FullName, fi.Name), false);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
