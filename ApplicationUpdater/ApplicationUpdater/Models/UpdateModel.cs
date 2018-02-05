using System;
using System.IO;

namespace ApplicationUpdater
{
    public class UpdateModel
    {
        public string CurrentProcessName { get; set; }

        public DirectoryInfo UnZipDirectory { get; internal set; }
        public DirectoryInfo NewApplicationDirectory { get; internal set; }
        public DirectoryInfo OldApplicationDirectory { get; internal set; }
       
        public UserParams UserParams { get; set; }

        public UpdateModel()
        {
            UserParams = new UserParams();
        }
    }

    public class UserParams
    {
        public FileInfo PathToZipFile { get; internal set; }
        public DirectoryInfo BackupDirectory { get; internal set; }
        public DirectoryInfo IntepubDirectory { get; internal set; }
        public string Version { get; internal set; }
        public bool IsUndoProcess { get; internal set; }

        public override string ToString()
        {
            return $" Path to zip file: {PathToZipFile} \n BackupDirectory:  {BackupDirectory} \n IntepubDirectory: {IntepubDirectory} \n New version:      {Version} \n Is undo process:  {IsUndoProcess} ";
        }

    }
}