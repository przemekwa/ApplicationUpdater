
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

       

        public IISAplicationUpdater(IUpdateProcess selgrosApplicationUpdateStrategy)
        {
            UpdateProcess = selgrosApplicationUpdateStrategy;
           
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
