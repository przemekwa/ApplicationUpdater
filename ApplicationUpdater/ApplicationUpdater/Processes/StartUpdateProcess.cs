using Microsoft.Extensions.Configuration;
using System.IO;

namespace ApplicationUpdater.Processes
{
    public class StartUpdateProcess : ProcessBase, IProcess<UpdateModel>
    {
        public StartUpdateProcess(IConfigurationRoot configurationRoot) : base(configurationRoot, "Start update")
        {
        }

        public ProcesEventResult Process(UpdateModel model)
        {
           return GetProcesEventResult(Consts.ProcesEventResult.Successful);
        }
    }
}
