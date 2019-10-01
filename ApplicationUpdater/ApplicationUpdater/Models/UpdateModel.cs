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
        public FileInfo PathToZipFile { get; internal set; }
        public DirectoryInfo BackupDirectory { get; set; }
        public DirectoryInfo IntepubDirectory { get; set; }
        public string Version { get; internal set; }
        public bool IsUndoProcess { get; internal set; }
        public string Strategy { get; internal set; }

        public override string ToString()
        {
            return $" Strategy:         {Strategy}\n Path to zip file: {PathToZipFile}\n BackupDirectory:  {BackupDirectory} \n IntepubDirectory: {IntepubDirectory} \n New version:      {Version} \n Is undo process:  {IsUndoProcess} ";
        }

    }
}