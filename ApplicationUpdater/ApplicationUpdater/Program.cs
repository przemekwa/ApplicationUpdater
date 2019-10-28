﻿using ApplicationUpdater.Processes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace ApplicationUpdater
{
    class Program
    {
        [DllImport("libc")]
        public static extern uint getuid();

        static void Main(string[] args)
        {
            Consts.Header.WriteHeader();
            Console.CursorVisible = false;

            var updateModel = GetUpdateModel(args);

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
                Console.WriteLine($"An error occurred during the update: {e.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.CursorVisible = true;
        }

        public static string GetStopWatchString(DateTime ts)
        {
            return $"{ts.Hour:00}:{ts.Minute:00}:{ts.Second:00}:{(ts.Millisecond / 10):00}";
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
                var top = Console.CursorTop-1;
                
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

        private static UpdateModel GetUpdateModel(string[] args)
        {
            if (args.Length != 6)
            {
                throw new ArgumentException("No suitable parameters. Details on https://github.com/przemekwa/ApplicationUpdater");
            }

            var updateModel = new UpdateModel
            {
                UserParams = new UserParams
                {
                    Strategy = GetParam(args, 0, "Strategy"),
                    PathToZipFile = new FileInfo(GetParam(args, 1, "PathToZipFile")),
                    BackupDirectory = new DirectoryInfo(GetParam(args, 2, "BackupDirectory")),
                    IntepubDirectory = GetInetpubDirectory(args),
                    Version = GetParam(args, 4, "Version"),
                    IsUndoProcess = bool.Parse(GetParam(args, 5, "IsUndoProcess")),
                }
            };

            return updateModel;
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
