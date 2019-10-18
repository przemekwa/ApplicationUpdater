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

            foreach (var fileInfo in dirList
                .Where(s=>s.Exists)
                .SelectMany(s=>s.GetFiles())
                .Where(s=>s.Exists))
            {
                try
                {
                    fileInfo.Delete();
                }
                catch (System.Exception e)
                {
                    UpdateProcess($"ERROR on delete file {fileInfo.FullName}", true, false);
                    continue;
                }

                UpdateProcess($"Delete file {fileInfo.FullName}", false, true);
            }


            return GetProcesEventResult("Successful");
        }
    }
}
