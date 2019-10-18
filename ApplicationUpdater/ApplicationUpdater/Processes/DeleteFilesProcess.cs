using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace ApplicationUpdater.Processes
{
    public class DeleteFilesProcess : ProcessBase, IProcess<UpdateModel>
    {
        public DeleteFilesProcess(IConfigurationRoot configurationRoot) : base(configurationRoot, "Delete application files from config")
        {
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            var dirList = this.ConfigurationRoot
                  .GetSection("DeleteFilesProcess.DirectoryList")?
                  .GetChildren()?
                  .Select(s=>new DirectoryInfo(Path.Combine(model.UserParams.IntepubDirectory.FullName, s.Value)));
                  
            if (dirList == null || dirList.Any() == false)
            {
                return GetProcesEventResult("No directories to delete");
            }

            foreach (var directoryInfo in dirList
                .Where(s=>s.Exists))
            {
                try
                {
                    directoryInfo.Delete(true);
                }
                catch (System.Exception e)
                {
                    UpdateProcess($"ERROR on delete direcotry {directoryInfo.FullName}", true, false);
                    continue;
                }

                UpdateProcess($"Delete file {directoryInfo.FullName}", false, true);
            }


            return GetProcesEventResult("Successful");
        }
    }
}
