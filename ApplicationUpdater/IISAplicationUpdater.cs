using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater
{
    public class IISAplicationUpdater
    {
        public IUpdateProcess UpdateProcess { get; private set; }

        public ILogger Logger { get; private set; }

        public IISAplicationUpdater(IUpdateProcess selgrosApplicationUpdateStrategy, ILogger logger)
        {
            UpdateProcess = selgrosApplicationUpdateStrategy;
            Logger = logger;
        }

        public void Update(UpdateModel updateModel)
        {
            try
            {
                UpdateProcess.Update(updateModel);
            }
            finally
                {
                UpdateProcess.CreateReport(updateModel);
            }
        }
    }
}
