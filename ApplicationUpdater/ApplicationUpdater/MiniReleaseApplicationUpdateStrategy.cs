﻿using ApplicationUpdater.Processes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ApplicationUpdater
{
    public class MiniReleaseApplicationUpdateStrategy : IUpdateProcess
    {
        public event EventHandler UpdateEvent; 
        public event EventHandler ConfirmEvent;
        public event EventHandler ResultEvetnt;


        public IProcess<UpdateModel> SetOnLineProcess { get; set; }

        private IEnvironmentManager environmentManager;
        public IConfigurationRoot ConfigurationRoot { get; set; }

        public MiniReleaseApplicationUpdateStrategy(IConfigurationRoot configurationRoot, IEnvironmentManager environmentManager)
        {
            ConfigurationRoot = configurationRoot;
            this.environmentManager = environmentManager;
        }

        public void DeleteFiles(UpdateModel updateModel)
        {
            var process = new DeleteFilesProcess(ConfigurationRoot);

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);
        }

        public void SetOnline(UpdateModel updateModel)
        {
            var process = new SetOnLineProcess( );

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);
        }

        public void SetOffline(UpdateModel updateModel)
        {
            var process = new SetOffLineProcess();

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);
        }

        public void CheckVersion(UpdateModel updateModel)
        {
            var process = new CheckVersionProcess(ConfigurationRoot, environmentManager);

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);
        }

         public void MiniReleaseCheckVersion(UpdateModel updateModel)
        {
            var process = new MiniReleaseCheckVersionProcess(ConfigurationRoot, environmentManager);

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);
        }

        public void MiniReleaseCopyFiles(UpdateModel updateModel)
        {
            var process = new MiniReleaseCopyFilesProcess(ConfigurationRoot);

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);
        }

        public void CreateReport(UpdateModel updateModel)
        {
            return;
        }

        public void MakeBackup(UpdateModel updateModel)
        {
            var process = new BackupProcess(ConfigurationRoot);

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);
        }

        public void Unzip(UpdateModel updateModel)
        {
            var process = new UnzipProcess();

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);
        }

        private void ProcessEvent(object sender, EventArgs e)
        {
            UpdateEvent(sender, new EventArgs());
        }

        public void VerifyCopy(UpdateModel updateModel)
        {
           
        }

        public void EditWebConfig(UpdateModel updateModel)
        {
            var process = new EditWebConfigProcess(ConfigurationRoot);

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);

        }

        public void PrepareEnviroment(UpdateModel updateModel)
        {
            var process = new PrepareEnviroment();

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);

        }

        public void UndoProcess(UpdateModel updateModel)
        {
            var process = new UndoProcess();

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);

        }

        public void StartUpdateProcess(UpdateModel updateModel)
        {
            var process = new StartUpdateProcess(ConfigurationRoot, environmentManager);

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);
        }

        public IEnumerable<Action<UpdateModel>> GetProcess(UpdateModel updateModel)
        {
            if (updateModel.UserParams.IsUndoProcess)
            {
                return new List<Action<UpdateModel>>
                {
                    SetOffline,
                    UndoProcess,
                    SetOnline
                };
            }

            return new List<Action<UpdateModel>>
            {
                StartUpdateProcess,
                PrepareEnviroment,
                Unzip,
                MiniReleaseCheckVersion,
                MakeBackup,
                MiniReleaseCopyFiles
            };
        }
    }
}
