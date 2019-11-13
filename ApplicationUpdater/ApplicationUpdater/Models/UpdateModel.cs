using CommandLine;
using System.IO;

namespace ApplicationUpdater
{
    public class UpdateModel
    {
        public string CurrentProcessName { get; set; }

        public DirectoryInfo UnZipDirectory { get; set; }
        public DirectoryInfo NewApplicationDirectory { get; set; }
        public DirectoryInfo OldApplicationDirectory { get; set; }
       
        public UserParams UserParams { get; set; }

        public UpdateModel()
        {
            UserParams = new UserParams();
        }
    }

    public class UserParams
    {
        [Option('z', "zipFile", Required = true, HelpText = "Path to zip file with application.")]
        public FileInfo PathToZipFile { get; internal set; }

        [Option('b', "backup", Required = true, HelpText = "Path to backup directory.")]
        public DirectoryInfo BackupDirectory { get; set; }

        [Option('i', "inetpub", Required = true, HelpText = "Path to inteput directory.")]
        public DirectoryInfo IntepubDirectory { get; set; }

        [Option('v', "appversion", Required = true, HelpText = "Version of application.")]
        public string Version { get; internal set; }

        [Option('u', "undo", Required = false, HelpText = "Is undo process enabled?")]
        public bool IsUndoProcess { get; internal set; }

        [Option('s', "strategy", Required = true, HelpText = "Choose strategy: Selgros | Orlen | Mini")]
        public string Strategy { get; internal set; }

        public override string ToString()
        {
            return $" Strategy:         {Strategy}\n Path to zip file: {PathToZipFile}\n BackupDirectory:  {BackupDirectory} \n IntepubDirectory: {IntepubDirectory} \n New version:      {Version} \n Is undo process:  {IsUndoProcess} ";
        }
    }
}