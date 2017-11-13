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
            var inetpubFiles = Directory.GetFiles(model.IntepubDirectory, "*.*", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f));

            var newAppDirectory = Path.Combine(model.BackupDirectory, "new-application", "app\\");

            var newAppFiles = Directory.GetFiles(newAppDirectory, "*.*", SearchOption.AllDirectories)
                .Select(f => new FileInfo(f));

            if (!inetpubFiles.Any() || !newAppFiles.Any())
            {
                throw new Exception("Brak plików aby poównać.");
            }

            foreach (var inetpubFile in inetpubFiles)
            {
                var fileNameToCheck = inetpubFile.FullName.Replace(model.IntepubDirectory, "");


                var file = newAppFiles.SingleOrDefault(s => s.FullName.Replace(newAppDirectory, "") == fileNameToCheck);

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
