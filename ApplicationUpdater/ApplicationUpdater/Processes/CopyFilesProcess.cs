﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class CopyFilesProcess : ProcessBase
    {
        public CopyFilesProcess() : base("Update application")
        {
        }

        public ProcesEventResult Process(UpdateModel model)
        {
            if (Confirm("Do you want to update application?") == false)
            {
                return ProcesEventResult.STOP;
            }

            CopyAll(model.NewApplicationDirectory.FullName, model.NewApplicationDirectory, model.UserParams.IntepubDirectory, true, "Update file: {0}");

            return null;
        }
    }
}
