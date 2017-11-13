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
        public IUpdateProcess SelgrosApplicationUpdateStrategy { get; private set; }

        public ILogger Logger { get; private set; }

        public IISAplicationUpdater(IUpdateProcess selgrosApplicationUpdateStrategy, ILogger logger)
        {
            SelgrosApplicationUpdateStrategy = selgrosApplicationUpdateStrategy;
            Logger = logger;
        }

        public void Update(UpdateModel updateModel)
        {
            try
            {
                SelgrosApplicationUpdateStrategy.Unzip(updateModel);

                SelgrosApplicationUpdateStrategy.CheckVersion(updateModel);

                SelgrosApplicationUpdateStrategy.MakeBackup(updateModel);



                SelgrosApplicationUpdateStrategy.CopyFiles(updateModel);

                SelgrosApplicationUpdateStrategy.VerifyCopy(updateModel);
            }
            finally
            {
                SelgrosApplicationUpdateStrategy.CreateReport(updateModel);
            }
        }

      
    }
}
