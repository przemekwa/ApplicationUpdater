using System;

namespace ApplicationUpdater
{
    public class UpdateModel
    {
        public string PathToZipFile { get; internal set; }
        public string BackupDirectory { get; internal set; }
        public string IntepubDirectory { get; internal set; }
        public string UnZipDirectory { get; internal set; }
    }
}