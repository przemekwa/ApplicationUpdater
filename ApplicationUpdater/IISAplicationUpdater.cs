using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater
{
    public class IISAplicationUpdater
    {
        public void Update(UpdateModel updateModel, IUpdateStrategy updateStrategy)
        {
            try
            {
                updateStrategy.Unzip(updateModel);

                updateStrategy.CheckVersion(updateModel);

                updateStrategy.MakeBackup(updateModel);

                updateStrategy.CopyFiles(updateModel);

                updateStrategy.VerifyCopy(updateModel);
            }
            catch (Exception e)
            {
                
            }
            finally
            {
                updateModel.CreateReport(updateModel);
            }


        }
    }
}
