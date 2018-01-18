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
            var backupDirectory = Directory.CreateDirectory(Path.Combine(model.BackupDirectory.FullName, "old-application"));

            CopyAll(new DirectoryInfo(model.IntepubDirectory.FullName), backupDirectory, false, "Backup file: {0}" );

            UpdateProcess($"Backup successful.");

            return null;
        }
    }
}
