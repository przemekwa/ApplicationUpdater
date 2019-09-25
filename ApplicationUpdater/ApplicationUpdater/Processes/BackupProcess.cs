using Microsoft.Extensions.Configuration;
using System.IO;

namespace ApplicationUpdater.Processes
{
    public class BackupProcess : ProcessBase, IProcess<UpdateModel>
    {
        public BackupProcess(IConfigurationRoot configurationRoot) : base(configurationRoot, "Backup application files")
        {
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            var backupDirectory = Directory.CreateDirectory(Path.Combine(model.UserParams.BackupDirectory.FullName, Consts.DirectoriesNames.OldApplication));

            CopyAll(model.UserParams.IntepubDirectory.FullName,
                new DirectoryInfo(model.UserParams.IntepubDirectory.FullName), 
                backupDirectory, 
                false, 
                "Backing up: {0}",
                this.GetExcludeFiles(model.UserParams.IntepubDirectory.FullName));
            
            return GetProcesEventResult("Successful");
        }
    }
}
