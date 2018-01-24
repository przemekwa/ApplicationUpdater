﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class SetOfflineProcess : ProcessBase, IProcess<UpdateModel>
    {
        private const string offLineFileName = "_app_offline.htm";
        private const string searchPattern = "Trwa aktualizacja danych";

        public ProcesEventResult Process(UpdateModel model)
        {
            if (Confirm("Czy chcesz przejść w stan off line aplikacji?") == false)
            {
                return null;
            }

            var file = model.IntepubDirectory
                .GetFiles()
                .SingleOrDefault(s => s.Name == $"{offLineFileName}");

            if (file == null ||file.Exists == false)
            {
                var file2 = model.IntepubDirectory
                .GetFiles()
                .SingleOrDefault(s => s.Name == $"app_offline.htm");

                if (file2 == null || file2.Exists == false)
                {
                    throw new Exception("Nie znaleziono pliku");
                }
                UpdateDateTimeInFile(file2);

                return null;
            }

            UpdateDateTimeInFile(file);
            File.Delete(file.FullName);

            UpdateProcess($"Przełączona apliakcję w trym offline");

            return null;

        }

        private void UpdateDateTimeInFile(FileInfo file)
        {
            var lines = File.ReadAllLines(file.FullName);

            var index = lines.Select((s, i) => new { i, s })
                .Where(t => t.s.Contains(searchPattern))
                .Select(t => t.i)
                .First();

            var dateTimeValue = DateTime.Now.AddHours(1).ToString("HH:mm");

            lines[index] = $"Trwa aktualizacja danych. Do godz. {dateTimeValue} NPG bedzie niedostepny. <br/><br/> W naglych przypadkach prosze o kontakt pod numerem: 695 877 795";

            File.WriteAllLines(Path.Combine(file.DirectoryName, "app_offline.htm"), lines);
           
        }
    }
}
