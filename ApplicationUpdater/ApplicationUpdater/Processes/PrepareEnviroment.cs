using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class PrepareEnviroment : ProcessBase, IProcess<UpdateModel>
    {
        public PrepareEnviroment() : base("Prepare enviroment")
        {
        }

      

        public ProcesEventResult Process(UpdateModel model)
        {
            var index = 0;

            var backupDirectoryPath = GetBackupPath(model, index);

            while (Directory.Exists(backupDirectoryPath))
            {
                backupDirectoryPath = GetBackupPath(model, ++index);
            }

            Directory.CreateDirectory(backupDirectoryPath);

            model.UserParams.BackupDirectory = new DirectoryInfo(backupDirectoryPath);

            return null;
        }

        private string GetBackupPath(UpdateModel updateModel, int index)
        {
            return Path.Combine(updateModel.UserParams.BackupDirectory.FullName, DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), index.ToString());
        }
    }
}
