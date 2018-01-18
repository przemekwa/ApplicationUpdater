using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public abstract class ProcessBase
    {
        public event EventHandler ProcessEvent;

        protected virtual void UpdateProcess(string msg)
        {
            ProcessEvent(msg, new EventArgs { });
        }


        protected virtual void CopyAll(DirectoryInfo source, DirectoryInfo target, bool overrideFile, string msgFormat)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                UpdateProcess(string.Format(msgFormat, fi.Name));

                fi.CopyTo(Path.Combine(target.FullName, fi.Name), overrideFile);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir, overrideFile, msgFormat);
            }
        }
    }
}
