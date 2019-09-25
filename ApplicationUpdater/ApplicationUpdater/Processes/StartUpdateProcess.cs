using Microsoft.Extensions.Configuration;
using System.IO;

namespace ApplicationUpdater.Processes
{
    public class StartUpdateProcess : ProcessBase, IProcess<UpdateModel>
    {
        private IEnvironmentManager environmentManager;

        public StartUpdateProcess(IConfigurationRoot configurationRoot, IEnvironmentManager environmentManager) : base(configurationRoot, "Start update")
        {
            this.environmentManager = environmentManager;
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            if (Confirm("Do you want to start update?") == false)
            {
                environmentManager.Exit(0);
            }

            return GetProcesEventResult(Consts.ProcesEventResult.Successful);
        }
    }
}
