using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater
{
    public class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetLogger("ApplicationUpdater");

            try
            {
                ConsoleEvent("Aktualizacja aplikacji", null);

                var updateModel = GetUpdateModel(args);

                ConsoleEvent("Przygotowywanie modelu", null);

                PrepareEnviroment(updateModel);

                var selgrosApplicationUpdateStrategy = new SelgrosApplicationUpdateStrategy(logger);

                selgrosApplicationUpdateStrategy.UpdateEvent += ConsoleEvent;

                var iISAplicationUpdater = new IISAplicationUpdater(selgrosApplicationUpdateStrategy, logger);

                iISAplicationUpdater.Update(updateModel);
            }
            catch (Exception e)
            {
                logger.Error(e);

                Console.WriteLine($"Wystąpił błąd podczas aktualizacji: {e.Message}");
            }

            Console.WriteLine("Naciśnij dowolny klawisz aby kontynuować...");
            Console.ReadKey();
        }

        public static string GetStopWatchString(DateTime ts)
        {
            return $"{ts.Hour:00}:{ts.Minute:00}:{ts.Second:00}:{(ts.Millisecond / 10):00}";
        }

        private static void ConsoleEvent(object sender, EventArgs e)
        {
            Console.WriteLine($"{GetStopWatchString(DateTime.Now)}...{(string)sender}.");
        }

        private static void PrepareEnviroment(UpdateModel updateModel)
        {
            var index = 0;

            var backupDirectoryPath = GetBackupPath(updateModel, index);
            
            while (Directory.Exists(backupDirectoryPath))
            {
                backupDirectoryPath = GetBackupPath(updateModel, ++index);
            }

            Directory.CreateDirectory(backupDirectoryPath);

            updateModel.BackupDirectory = new DirectoryInfo(backupDirectoryPath);
        }

        private static string GetBackupPath(UpdateModel updateModel, int index)
        {
            return Path.Combine(updateModel.BackupDirectory.FullName, DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), index.ToString());
        }

        private static UpdateModel GetUpdateModel(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("Brak odpowiednich parametrów");
            }

            var updateModel = new UpdateModel
            {
                PathToZipFile = new FileInfo( GetParam(args, 0, "PathToZipFile")),
                BackupDirectory = new DirectoryInfo(GetParam(args, 1, "BackupDirectory")),
                IntepubDirectory = new DirectoryInfo(GetParam(args, 2, "IntepubDirectory"))
            };

            return updateModel;
        }

        private static string GetParam(string[] args, int index, string name)
        {
            if (string.IsNullOrEmpty(args[index]))
            {
                throw new ArgumentException(name);
            }

            return args[index];
        }
    }
}
