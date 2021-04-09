using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ApplicationUpdater.Processes
{

    public class UnzipProcess: ProcessBase, IProcess<UpdateModel>
    {
        public UnzipProcess() : base("Unzipping")
        {
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            var unZipDirectory = Path.Combine(model.UserParams.BackupDirectory.FullName, Consts.DirectoriesNames.NewApplication);

            Directory.CreateDirectory(unZipDirectory);

            UpdateProcess($"Unzip {model.UserParams.PathToZipFile.FullName} to {unZipDirectory}");

            UnZip(model, unZipDirectory);

            model.UnZipDirectory = new DirectoryInfo(unZipDirectory);

            model.NewApplicationDirectory = new DirectoryInfo(Path.Combine(model.UnZipDirectory.FullName, "app"));

            return GetProcesEventResult(Consts.ProcesEventResult.Successful);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/24941741/zip-entry-name-ends-in-directory-separator-character-but-contains-data
        /// </summary>
        /// <param name="model"></param>
        /// <param name="unZipDirectory"></param>
        private void UnZip(UpdateModel model, string unZipDirectory)
        {
            var archive = ArchiveFactory.Open(model.UserParams.PathToZipFile.FullName);

            var maxProgress = archive.Entries.ToList().Count;
            var count = 0;

            foreach (var entry in archive.Entries)
            {
                if (entry.IsDirectory == false)
                {
                    entry.WriteToDirectory(unZipDirectory, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                }

                UpdateProcessWithPgoressBar(++count, maxProgress, "", false);
            }
        }
    }
}
