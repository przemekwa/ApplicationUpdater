using NLog;
using System;
using System.Collections.Generic;
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
                Validate(args);

                iISAplicationUpdater.Update(new UpdateModel
                {

                });
            }
            catch (Exception e)
            {
                logger.Error(e);

                Console.WriteLine($"Wystąpił błąd podczas aktualizacji: {e.Message}");
            }

            Console.WriteLine("Naciśnij dowolny klawisz aby kontynuować...");
            Console.ReadKey();
        }

        private static void Validate(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
