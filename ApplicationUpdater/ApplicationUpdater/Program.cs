using ApplicationUpdater.Processes;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationUpdater
{
    class Program
    {
        [DllImport("libc")]
        public static extern uint getuid();

        public static ProgressBar progressBar { get; set; }

        static void Main(string[] args)
        {
            Consts.Header.WriteHeader();
            Console.CursorVisible = false;

            var updateModel = new UpdateModel();

            var result = Parser.Default.ParseArguments<UserParams>(args)
                    .WithParsed(o =>
                    {
                        updateModel.UserParams = o;
                    })
                    .WithNotParsed(errorList =>
                    {
                        Environment.Exit(100);
                    });

            var di = new Di(null, ConsoleEvent, GetConfirmation, RezultEvent);

            di.Build(updateModel.UserParams.Strategy);

            try
            {
#if !DEBUG
                RequireAdministrator();
#endif
                Console.WriteLine(updateModel.UserParams.ToString());
                Console.WriteLine();

                var iISAplicationUpdater = di.GetService<IISAplicationUpdater>();

                iISAplicationUpdater.Update(updateModel);

                Console.WriteLine("The application has been updated", null);
            }
            catch (Exception e)
            {
                ConsoleEvent(new ConsoleWriteProcess { Msg = $"[ERROR] {e.Message}", NewLine = true }, null);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.CursorVisible = true;
        }

        public static string GetStopWatchString(DateTime ts)
        {
            return $"{ts.Hour:00}:{ts.Minute:00}:{ts.Second:00}:{(ts.Millisecond / 10):000}";
        }

        private static void RezultEvent(object sender, EventArgs e)
        {
            var cwp = new ConsoleWriteProcess
            {
                Msg = $"{((ProcesEventResult)sender).Result}",
                NewLine = true
            };

            ConsoleEvent(cwp, null);
        }

        private static void ConsoleEvent(object sender, EventArgs e)
        {
            Console.CursorVisible = false;
            var consoleWriteProcess = sender as ConsoleWriteProcess;

            if (consoleWriteProcess.NewLine)
            {
                Console.WriteLine($"{GetStopWatchString(DateTime.Now)}   {consoleWriteProcess.Msg}");
            }
            else if (consoleWriteProcess.OneLineMode)
            {
                var top = Console.CursorTop - 1;

                Console.SetCursorPosition(0, top);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, top);

                var header = GetStopWatchString(DateTime.Now);

                var line = $"{header}   {consoleWriteProcess.Msg}";

                if (line.Length >= (Console.WindowWidth - 1))
                {
                    line = line.Substring(0, (Console.WindowWidth - 2));
                }

                Console.WriteLine(line);
            }
            else if (consoleWriteProcess.ShowProgress)
            {
                if (progressBar == null)
                {
                    progressBar = new ProgressBar(consoleWriteProcess.Msg);
                }
                
                progressBar.Report(consoleWriteProcess.StepNumberProgress / consoleWriteProcess.MaxProgress);
                
                if (consoleWriteProcess.StepNumberProgress == consoleWriteProcess.MaxProgress)
                {
                    Thread.Sleep(200);
                    progressBar.Dispose();
                    progressBar = null;
                    Console.WriteLine();
                }
            }
            else
            {
                Console.Write($"{GetStopWatchString(DateTime.Now)}   {consoleWriteProcess.Msg}");
            }

            Console.CursorVisible = true;
        }

        private static void GetConfirmation(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            var pc = (ProcessConfirmation)sender;

            Console.Write($"{ GetStopWatchString(DateTime.Now)}   {pc.Question}");

            var allowKeys = new List<ConsoleKey>
            {
                ConsoleKey.Y,
                ConsoleKey.N,
                ConsoleKey.C
            };

            var key = ConsoleKey.Clear;

            while (allowKeys.Contains(key) == false)
            {
                key = Console.ReadKey(true).Key;
            }

            if (key == ConsoleKey.C)
            {
                Environment.Exit(0);
            }

            pc.Key = key;

            Console.WriteLine(pc.Key.ToString());

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static DirectoryInfo GetInetpubDirectory(string[] args)
        {
            var param = GetParam(args, 3, "IntepubDirectory");

            if (param[^1] == '/' || param[^1] == '\\')
            {
                param = param[0..^1];
            }

            return new DirectoryInfo(param);
        }

        private static string GetParam(string[] args, int index, string name)
        {
            if (string.IsNullOrEmpty(args[index]))
            {
                throw new ArgumentException(name);
            }

            return args[index];
        }

        public static void RequireAdministrator()
        {
            try
            {
                string name = System.AppDomain.CurrentDomain.FriendlyName;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    using WindowsIdentity identity = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
                    {
                        throw new InvalidOperationException($"Application must be run as administrator. Right click the {name} file and select 'run as administrator'.");
                    }
                }
                else if (getuid() != 0)
                {
                    throw new InvalidOperationException($"Application must be run as root/sudo. From terminal, run the executable as 'sudo {name}'");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error when determining administrator");
            }
        }
    }
}
