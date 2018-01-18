using System;
using System.IO;

namespace ApplicationUpdater
{
    public class UpdateModel
    {
        public FileInfo PathToZipFile { get; internal set; }
        public DirectoryInfo BackupDirectory { get; internal set; }
        public DirectoryInfo IntepubDirectory { get; internal set; }
        public DirectoryInfo UnZipDirectory { get; internal set; }
    }
}