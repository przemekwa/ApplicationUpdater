﻿using ApplicationUpdater.ProcessEvetns;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater
{
    public class SelgrosApplicationUpdateStrategy : IUpdateProcess
    {
        public ILogger Logger { get; private set; }

        public SelgrosApplicationUpdateStrategy(ILogger logger)
        {
            Logger = logger;
        }

        public event EventHandler UpdateEvent;

        public void CheckVersion(UpdateModel updateModel)
        {
            var checkVersionEvent = new CheckVersionEvent();

            checkVersionEvent.ProcessEvent += ProcessEvent;

            checkVersionEvent.Process(updateModel);
        }

        public void CopyFiles(UpdateModel updateModel)
        {
            throw new NotImplementedException();
        }

        public void CreateReport(UpdateModel updateModel)
        {
            return;
        }

        public void MakeBackup(UpdateModel updateModel)
        {
            throw new NotImplementedException();
        }

        public void Unzip(UpdateModel updateModel)
        {
            var unZipEvent = new UnZipEvent();

            unZipEvent.ProcessEvent += ProcessEvent;

            var result = unZipEvent.Process(updateModel);

            if (!result.Result)
            {
                throw new Exception("Błąd w unZip");
            }
        }

        private void ProcessEvent(object sender, EventArgs e)
        {
            
            UpdateEvent(sender, new EventArgs());
        }

        public void VerifyCopy(UpdateModel updateModel)
        {
            throw new NotImplementedException();
        }
    }
}