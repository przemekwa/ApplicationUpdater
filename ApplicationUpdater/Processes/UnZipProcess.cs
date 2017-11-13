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
            var unZipDirectory = Path.Combine(model.BackupDirectory, "new-application");

            UpdateProcess($"Tworzenie katalogu do wypakowania {unZipDirectory}");

            //Directory.CreateDirectory(unZipDirectory);

            UpdateProcess($"Wypakowywanie");

            //ZipFile.ExtractToDirectory(model.PathToZipFile, unZipDirectory);

            UpdateProcess($"Koniec wypakowywania");

            model.BackupDirectory = "D:\\13-11-2017\\5\\";
            model.UnZipDirectory = unZipDirectory;

            return new ProcesEventResult
            {
                Result = true
            };
        }
    }
}
