using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater
{
    public class SelgrosApplicationUpdateStrategy : IUpdateStrategy
    {
        public ILogger Logger { get; private set; }

        public SelgrosApplicationUpdateStrategy(ILogger logger)
        {
            Logger = logger;
        }

        public void CheckVersion(UpdateModel updateModel)
        {
            throw new NotImplementedException();
        }

        public void CopyFiles(UpdateModel updateModel)
        {
            throw new NotImplementedException();
        }

        public void CreateReport(UpdateModel updateModel)
        {
            throw new NotImplementedException();
        }

        public void MakeBackup(UpdateModel updateModel)
        {
            throw new NotImplementedException();
        }

        public void Unzip(UpdateModel updateModel)
        {
            throw new NotImplementedException();
        }

        public void VerifyCopy(UpdateModel updateModel)
        {
            throw new NotImplementedException();
        }
    }
}
