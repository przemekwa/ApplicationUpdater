using System;
using System.IO;
using System.Linq;

namespace ApplicationUpdater.Processes
{
    public class SetOffLineProcess : ProcessBase, IProcess<UpdateModel>
    {
        private const string offLineFileName = "_app_offline.htm";
        private const string searchPattern = "Trwa aktualizacja danych";

        public SetOffLineProcess() : base("Set off line")
        {
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            if (Confirm("Do you want to go OFFLINE mode?") == false)
            {
                return GetProcesEventResult("SKIP");
            }

            var fileOffLine = model.UserParams.IntepubDirectory
                .GetFiles()
                .SingleOrDefault(s => s.Name == $"{offLineFileName}");

            if (fileOffLine == null || fileOffLine.Exists == false)
            {
                return ProcesEventResult.ERROR;
            }

            File.Copy(fileOffLine.FullName, Path.Combine(fileOffLine.DirectoryName, "app_offline.htm"));
            File.SetAttributes(fileOffLine.FullName, FileAttributes.Normal);
            File.Delete(fileOffLine.FullName);

            UpdateProcess($"Switched application into OFFLINE mode");

            return GetProcesEventResult("Successful");
        }
    }
}
