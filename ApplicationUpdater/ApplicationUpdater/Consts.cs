using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater
{
    public static class Consts
    {
        public static class DirectoriesNames
        {
            public const string OldApplication = "old-application";
            public const string NewApplication = "new-application";
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
                Console.WriteLine(@"       \__ / |    |__/ /~~\  |  |___ |  \              ");
                Console.WriteLine();
            }
        }
    }
}
