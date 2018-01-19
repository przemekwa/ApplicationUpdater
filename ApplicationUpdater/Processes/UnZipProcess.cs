using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class UnZipProcess: ProcessBase, IProcess<UpdateModel>
    {
        public ProcesEventResult Process(UpdateModel model)
        {
            var unZipDirectory = Path.Combine(model.BackupDirectory.FullName, Consts.DirectoriesNames.NewApplication);

            UpdateProcess($"Tworzenie katalogu do wypakowania {unZipDirectory}");

            Directory.CreateDirectory(unZipDirectory);

            UpdateProcess($"Wypakowywanie");

            ZipFile.ExtractToDirectory(model.PathToZipFile.FullName, unZipDirectory);

            UpdateProcess($"Koniec wypakowywania");
            
            model.UnZipDirectory = new DirectoryInfo(unZipDirectory);

            model.NewApplicationDirectory = new DirectoryInfo(Path.Combine(model.UnZipDirectory.FullName, "app"));

            return new ProcesEventResult
            {
                Result = true
            };
        }
    }
}
