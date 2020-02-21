using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ApplicationUpdater.Processes
{
    public class UndoProcess : ProcessBase, IProcess<UpdateModel>
    {
        public UndoProcess() : base("Undo update")
        {
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            SetLastUpdatePath(model);

            if (Confirm($"Do you want to undo the application to the version from the catalog { model.OldApplicationDirectory}?") == false)
            {
                return GetProcesEventResult(Consts.ProcesEventResult.Skip);
            }

            CopyFromOldApplication(model);

            return GetProcesEventResult(Consts.ProcesEventResult.Successful);
        }

        private void CopyFromOldApplication(UpdateModel model)
        {
            var pathToOldApplication = new DirectoryInfo(Path.Combine(model.OldApplicationDirectory.FullName,  Consts.DirectoriesNames.OldApplication));

            if (pathToOldApplication.Exists == false)
            {
                throw new Exception($"There is no substitution application in the catalog {pathToOldApplication}");
            }

            CopyAll(0,100, pathToOldApplication.FullName, pathToOldApplication, model.UserParams.IntepubDirectory, true, "Copy file: {0}");
        }

        private void SetLastUpdatePath(UpdateModel model)
        {
            var dateTime = DateTime.Now;

            var rootDir = GetBackupPath(model, dateTime);

            while (model.OldApplicationDirectory == null || model.OldApplicationDirectory.Exists == false)
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

            string index = files.OrderBy(s=>int.Parse(s.Substring(s.LastIndexOf('\\')+1))).Last().Substring(files.Last().Length - 1);

            return int.Parse(index);
        }

        private DirectoryInfo GetAppDir(DirectoryInfo rootDir, int index)
        {
            return new DirectoryInfo(Path.Combine(rootDir.FullName, index.ToString()));
        }

        private DirectoryInfo GetBackupPath(UpdateModel updateModel, DateTime dateTime)
        {
            return new DirectoryInfo(Path.Combine(updateModel.UserParams.BackupDirectory.FullName, dateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)));
        }
    }
}
