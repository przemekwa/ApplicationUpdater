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

            var iISAplicationUpdater = new IISAplicationUpdater(new SelgrosApplicationUpdateStrategy(logger), logger);

            try
            {
                var updateModel= GetUpdateModel(args);

                PrepareEnviroment(updateModel);

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

        private static void PrepareEnviroment(UpdateModel updateModel)
        {
            var index = 0;

            var backupDirectoryPath = GetBackupPath(updateModel, index);
            
            while (Directory.Exists(backupDirectoryPath))
            {
                backupDirectoryPath = GetBackupPath(updateModel, ++index);
            }

            Directory.CreateDirectory(backupDirectoryPath);
        }

        private static string GetBackupPath(UpdateModel updateModel, int index)
        {
            return Path.Combine(updateModel.BackupDirectory, DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture), index.ToString());
        }

        private static UpdateModel GetUpdateModel(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Brak odpowiednich parametrów");
            }

            var updateModel = new UpdateModel
            {
                PathToZipFile = GetParam(args, 0, "PathToZipFile"),
                BackupDirectory = GetParam(args, 1, "BackupDirectory")
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
