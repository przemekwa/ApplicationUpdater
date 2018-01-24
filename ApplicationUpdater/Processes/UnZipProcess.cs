using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class UnzipProcess: ProcessBase, IProcess<UpdateModel>
    {
        public ProcesEventResult Process(UpdateModel model)
        {
            var unZipDirectory = Path.Combine(model.BackupDirectory.FullName, Consts.DirectoriesNames.NewApplication);

            Directory.CreateDirectory(unZipDirectory);

            UpdateProcess($"Wypakowywanie {model.PathToZipFile.FullName} do {unZipDirectory}");

            ZipFile.ExtractToDirectory(model.PathToZipFile.FullName, unZipDirectory);

            model.UnZipDirectory = new DirectoryInfo(unZipDirectory);

            model.NewApplicationDirectory = new DirectoryInfo(Path.Combine(model.UnZipDirectory.FullName, "app"));

            return new ProcesEventResult
            {
                Result = true
            };
        }
    }
}
