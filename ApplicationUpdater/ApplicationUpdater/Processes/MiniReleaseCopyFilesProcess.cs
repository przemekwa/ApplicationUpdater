using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace ApplicationUpdater.Processes
{
    public class MiniReleaseCopyFilesProcess : ProcessBase
    {
        public MiniReleaseCopyFilesProcess(IConfigurationRoot configurationRoot)
            : base(configurationRoot, "Update application")
        {
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            if (Confirm("Do you want to start mini-realese?") == false)
            {
                return GetProcesEventResult(Consts.ProcesEventResult.Skip);
            }

            var fileList = this.ConfigurationRoot
                  .GetSection("MiniRelease.FileList")?
                  .GetChildren()?
                  .Select(s => new
                  {
                      File = new FileInfo(Path.Combine(model.NewApplicationDirectory.FullName, s.Value)),
                      RelativePath = s.Value
                  });


            if (fileList == null || fileList.Any() == false)
            {
                return GetProcesEventResult("No file in app.config to to mini-realese");
            }

            if (fileList.Any(s => s.File.Exists == false))
            {
                return GetProcesEventResult("Not all file exist in new-application");
            }

            if (Confirm($"Copy this file(s) \n {fileList.Select(s => s.File.Name).Aggregate((next, current) => { return $"{current}\n {next}"; })}") == false)
            {
                return GetProcesEventResult(Consts.ProcesEventResult.Skip);
            }

            foreach (var item in fileList)
            {
                var pathCopyTo = Path.Combine(model.UserParams.IntepubDirectory.FullName, item.RelativePath);

                item.File.CopyTo(pathCopyTo, true);

                UpdateProcess($"Update file { item.RelativePath }", true);
            }

            return GetProcesEventResult(Consts.ProcesEventResult.Successful);
        }
    }
}
