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

            var countAll = CountAll(new DirectoryInfo(model.UserParams.IntepubDirectory.FullName), this.GetExcludeFiles(model.UserParams.IntepubDirectory.FullName));

            CopyAll(0, countAll, model.UserParams.IntepubDirectory.FullName,
                new DirectoryInfo(model.UserParams.IntepubDirectory.FullName), 
                backupDirectory, 
                false, 
                "",
                this.GetExcludeFiles(model.UserParams.IntepubDirectory.FullName));
            
            return GetProcesEventResult("Successful");
        }
    }
}
