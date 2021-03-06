﻿using System;
using System.Globalization;
using System.IO;

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

            return GetProcesEventResult(Consts.ProcesEventResult.Successful);
        }

        private string GetBackupPath(UpdateModel updateModel, int index)
        {
            return Path.Combine(updateModel.UserParams.BackupDirectory.FullName, DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), index.ToString());
        }
    }
}
