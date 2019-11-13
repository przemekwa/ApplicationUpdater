using System;
using System.Reflection;

namespace ApplicationUpdater
{
    public static class Consts
    {
        public static class DirectoriesNames
        {
            public const string OldApplication = "old-application";
            public const string NewApplication = "new-application";
        }

        public static class ProcesEventResult
        {
            public const string Successful = "Successful";
            public const string Skip = "Skip";
            public const string Error = "Error";
        }


        public static class Header
        {
            public static void WriteHeader()
            {
                Console.WriteLine(@"       __   __          __       ___    __             ");
                Console.WriteLine(@"  /\  |__) |__) |    | /  `  /\   |  | /  \ |\ |       ");
                Console.WriteLine(@" /~~\ |    |    |___ | \__, /~~\  |  | \__/ | \|       ");
                Console.WriteLine(@"              __   __       ___  ___  __               ");
                Console.WriteLine(@"       |   | |__) |  \  /\   |  |__  |__)              ");
                Console.WriteLine($@"       \__ / |    |__/ /~~\  |  |___ |  \              v{Assembly.GetEntryAssembly().GetName().Version}");
                Console.WriteLine();
            }
        }
    }
}
