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
                return GetProcesEventResult("No files to delete");
            }

            foreach (var item in dirList.SelectMany(s=>s.GetFiles()))
            {
                try
                {
                    item.Delete();
                }
                catch (System.Exception e)
                {
                    UpdateProcess($"ERROR on delete file {item.FullName}", true, false);
                    continue;
                }

                UpdateProcess($"Delete file {item.FullName}", false, true);
            }


            return GetProcesEventResult("Successful");
        }
    }
}
