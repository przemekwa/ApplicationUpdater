using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.ProcessEvetns
{
    public class UnZipEvent: ProcessBase, IProcessEvent<UpdateModel>
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

            return new ProcesEventResult
            {
                Result = true
            };
        }
    }
}
