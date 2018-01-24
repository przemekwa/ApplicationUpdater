using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class CopyFilesProcess : ProcessBase, IProcess<UpdateModel>
    {
        public ProcesEventResult Process(UpdateModel model)
        {
            CopyAll(model.NewApplicationDirectory, model.IntepubDirectory, true, "Aktualizacja pliku: {0}");

            return null;
        }
    }
}
