using System.IO;

namespace ApplicationUpdater.Processes
{
    public class CopyFilesProcess : ProcessBase
    {
        public CopyFilesProcess() : base("Update application")
        {
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            if (Confirm("Do you want to update application?") == false)
            {
                return GetProcesEventResult(Consts.ProcesEventResult.Skip);
            }
            var countAll = CountAll(new DirectoryInfo(model.NewApplicationDirectory.FullName));

           


            CopyAll(
                0, 
                countAll,
                model.NewApplicationDirectory.FullName,
                model.NewApplicationDirectory,
               new DirectoryInfo(model.UserParams.IntepubDirectory.FullName),
               true,
               "",
               null);

            return GetProcesEventResult(Consts.ProcesEventResult.Successful);
        }
    }
}
