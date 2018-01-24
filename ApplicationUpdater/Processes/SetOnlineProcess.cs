using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class SetOnLineProcess : ProcessBase, IProcess<UpdateModel>
    {
        private const string offLineFileName = "app_offline.htm";

        public ProcesEventResult Process(UpdateModel model)
        {
            if (Confirm("Do you want to go ONLINE mode?") == false)
            {
                return null;
            }

            var file = model.IntepubDirectory
                .GetFiles()
                .SingleOrDefault(s => s.Name == $"{offLineFileName}");

            File.Copy(file.FullName, Path.Combine(file.DirectoryName, $"_{offLineFileName}"));
            File.Delete(file.FullName);

            return null;
        }
    }
}
