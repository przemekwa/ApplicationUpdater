﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class CopyFilesProcess : ProcessBase, IProcess<UpdateModel>
    {
        public ProcesEventResult Process(UpdateModel model)
        {
            CopyAll(new DirectoryInfo(Path.Combine(model.BackupDirectory.FullName, "app\\")), new DirectoryInfo( model.IntepubDirectory.FullName), true, "Updateing files: {0}");

            return null;
        }
    }
}
