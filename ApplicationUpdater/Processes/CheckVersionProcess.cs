using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class CheckVersionProcess: ProcessBase, IProcess<UpdateModel>
    {
        public ProcesEventResult Process(UpdateModel model)
        {
            var inetpubFiles = Directory.GetFiles(model.IntepubDirectory.FullName, "*.*", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f));

            var newAppDirectory = model.UnZipDirectory.FullName;

            var newAppFiles = Directory.GetFiles(newAppDirectory, "*.*", SearchOption.AllDirectories)
                .Select(s=>new FileInfo(s));

            if (!inetpubFiles.Any() || !newAppFiles.Any())
            {
                throw new Exception("Brak plików aby poównać.");
            }

            foreach (var inetpubFile in inetpubFiles)
            {
                var fileNameToCheck = inetpubFile.FullName.Replace(model.IntepubDirectory.FullName, string.Empty);

                var file = newAppFiles.SingleOrDefault(s => s.FullName.Replace(model.UnZipDirectory.FullName + "\\app\\", "").Equals(fileNameToCheck, StringComparison.CurrentCultureIgnoreCase));

                if (file == null)
                {
                    UpdateProcess($"UWAGA! Brak pliku w nowej aplikacji {inetpubFile.FullName}");
                    continue;
                }

                if (inetpubFile.CreationTime >= file.CreationTime)
                {
                    UpdateProcess($"UWAGA! W katalogu docelowym znaduje się nowszy plik {inetpubFile.FullName}");
                }
            }


            UpdateProcess("Proces sprawdzania wersji plików zakończony pomyślnie.");


            return null;

        }
    }
}
