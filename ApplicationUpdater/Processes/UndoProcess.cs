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
            SetLastUpdatePath(model);

            if (Confirm($"Czy chcesz cofnąć aplikację do versji z katalogu {model.OldApplicationDirectory}?") == false)
            {
                return null;
            }

            CopyFromOldApplication(model);

            return null;
        }

        private void CopyFromOldApplication(UpdateModel model)
        {
            var pathToOldApplication = new DirectoryInfo(Path.Combine(model.OldApplicationDirectory.FullName,  Consts.DirectoriesNames.OldApplication));

            if (pathToOldApplication.Exists == false)
            {
                throw new Exception($"w katalogu {pathToOldApplication} nie ma aplikacji do podmiany");
            }

            CopyAll(pathToOldApplication, model.IntepubDirectory, true, "Undo {0}");
        }

        private void SetLastUpdatePath(UpdateModel model)
        {
            var dateTime = DateTime.Now;

            var rootDir = GetBackupPath(model, dateTime);

            model.OldApplicationDirectory = new DirectoryInfo("X:/");

            while (model.OldApplicationDirectory.Exists == false)
            {
                while (rootDir.Exists == false)
                {
                    dateTime = dateTime.AddDays(-1);

                    rootDir = GetBackupPath(model, dateTime);
                }

                int index = GetLastIndex(rootDir);

                if (index < 0)
                {
                    continue;
                }

                var appDir = GetAppDir(rootDir, index);

                if (appDir.Exists == false)
                {
                    continue;
                }

                model.OldApplicationDirectory = appDir;
            }
        }

        private int GetLastIndex(DirectoryInfo rootDir)
        {
            var files = Directory.GetDirectories(rootDir.FullName);

            if (files.Length == 0)
            {
                return -1;
            }

            string index = files.Last().Substring(files.Last().Length - 1);

            return int.Parse(index);
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
