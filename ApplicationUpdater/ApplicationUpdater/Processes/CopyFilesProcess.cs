using System.IO;
using System.Linq;

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

            var result = CopyAll(
                0, 
                countAll,
                model.NewApplicationDirectory.FullName,
                model.NewApplicationDirectory,
               new DirectoryInfo(model.UserParams.IntepubDirectory.FullName),
               true,
               "",
               null);

            foreach (var errorMsg in result.Item2)
            {
                UpdateProcess(errorMsg);
            }

            return result.Item2.Any() ? GetProcesEventResult(Consts.ProcesEventResult.Error): GetProcesEventResult(Consts.ProcesEventResult.Successful);
        }
    }
}
