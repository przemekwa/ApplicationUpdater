using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class UndoProcess : ProcessBase, IProcess<UpdateModel>
    {
        public ProcesEventResult Process(UpdateModel model)
        {
            var dateTime = DateTime.Now;

            var rootDir = GetBackupPath(model, dateTime);

            while (rootDir.Exists == false)
            {
                dateTime = dateTime.AddDays(-1);

                rootDir = GetBackupPath(model, dateTime);
            }

            int index = GetLastIndex

            var appDir = GetAppDir(rootDir, index);

            while (appDir.Exists == false)
            {
                index--;
                appDir = GetAppDir(rootDir, index);
            }


            return null;
        }

        private DirectoryInfo GetAppDir(DirectoryInfo rootDir, int index)
        {
            return new DirectoryInfo(Path.Combine(rootDir.FullName, index.ToString()));
        }

        private DirectoryInfo GetBackupPath(UpdateModel updateModel, DateTime dateTime)
        {
            return new DirectoryInfo(Path.Combine(updateModel.BackupDirectory.FullName, dateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)));
        }
    }
}
