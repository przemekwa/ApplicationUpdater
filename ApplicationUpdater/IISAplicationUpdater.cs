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
        public IUpdateStrategy UpdateStrategy { get; private set; }
        public ILogger Logger { get; private set; }

        public IISAplicationUpdater(IUpdateStrategy updateStrategy, ILogger logger)
        {
            UpdateStrategy = updateStrategy;
            Logger = logger;
        }

        public void Update(UpdateModel updateModel)
        {
            try
            {
                UpdateStrategy.Unzip(updateModel);

                UpdateStrategy.CheckVersion(updateModel);

                UpdateStrategy.MakeBackup(updateModel);

                UpdateStrategy.CopyFiles(updateModel);

                UpdateStrategy.VerifyCopy(updateModel);
            }
            finally
            {
                UpdateStrategy.CreateReport(updateModel);
            }
        }
    }
}
