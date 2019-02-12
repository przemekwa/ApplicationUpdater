using ApplicationUpdater.Processes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ApplicationUpdater
{
    public class OrlenApplicationUpdateStrategy : IUpdateProcess
    {
        public event EventHandler UpdateEvent; 
        public event EventHandler ConfirmEvent;
        public event EventHandler ResultEvetnt;


        public IProcess<UpdateModel> SetOnLineProcess { get; set; }

        public IEnvironmentManager environmentManager;


        public IConfigurationRoot ConfigurationRoot { get; set; }

        public OrlenApplicationUpdateStrategy(IConfigurationRoot configurationRoot, IEnvironmentManager environmentManager)
        {
            ConfigurationRoot = configurationRoot;
            this.environmentManager = environmentManager;
        }

       

        public void CheckVersion(UpdateModel updateModel)
        {
            var process = new CheckVersionProcess(ConfigurationRoot, environmentManager);

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            ResultEvetnt(process.Process(updateModel), null);
        }

        public void CopyFiles(UpdateModel updateModel)
        {
            var process = new CopyFilesProcess();

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
            var process = new EditWebConfigProcess();

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

        public IEnumerable<Action<UpdateModel>> GetProcess(UpdateModel updateModel)
        {
            if (updateModel.UserParams.IsUndoProcess)
            {
                return new List<Action<UpdateModel>>
                {
                   
                    UndoProcess
                   
                };
            }

            return new List<Action<UpdateModel>>
            {
                PrepareEnviroment,
                Unzip,
                CheckVersion,
                MakeBackup,
                CopyFiles,
                VerifyCopy,
                EditWebConfig,
            };
        }
    }
}
