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
            var backupDirectory = Directory.CreateDirectory(Path.Combine(model.BackupDirectory.FullName, Consts.DirectoriesNames.OldApplication));

            CopyAll(new DirectoryInfo(model.IntepubDirectory.FullName), backupDirectory, false, "Tworzenie kopi zapasowej pliku: {0}" );

            return null;
        }
    }
}
