using System;
using System.IO;
using System.Linq;

namespace ApplicationUpdater.Processes
{
    public class SetOffLineProcess : ProcessBase, IProcess<UpdateModel>
    {
        private const string offLineFileName = "_app_offline.htm";
        private const string searchPattern = "Trwa aktualizacja danych";

        public SetOffLineProcess() : base("Set off line")
        {
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            if (Confirm("Do you want to go OFFLINE mode?") == false)
            {
                return GetProcesEventResult("SKIP");
            }

            var fileOffLine = model.UserParams.IntepubDirectory
                .GetFiles()
                .SingleOrDefault(s => s.Name == $"{offLineFileName}");

            if (fileOffLine == null || fileOffLine.Exists == false)
            {
                return ProcesEventResult.ERROR;
            }

            File.Copy(fileOffLine.FullName, Path.Combine(fileOffLine.DirectoryName, "app_offline.htm"));
            File.Delete(fileOffLine.FullName);

            UpdateProcess($"Switched application into OFFLINE mode");

            return GetProcesEventResult("Successful");

        }

        //private void UpdateDateTimeInFile(FileInfo file)
        //{
        //    var lines = File.ReadAllLines(file.FullName);

        //    var index = lines.Select((s, i) => new { i, s })
        //        .Where(t => t.s.Contains(searchPattern))
        //        .Select(t => t.i)
        //        .First();

        //    var dateTimeValue = DateTime.Now.AddMinutes(3).ToString("HH:mm");

        //    lines[index] = $"Trwa aktualizacja danych. Do godz. {dateTimeValue} NPG bedzie niedostepny. <br/><br/> W naglych przypadkach prosze o kontakt pod numerem: 695 877 795";

        //    File.WriteAllLines(Path.Combine(file.DirectoryName, "app_offline.htm"), lines);
           
        //}
    }
}
