using ApplicationUpdater.Processes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater
{
    public class SelgrosApplicationUpdateStrategy : IUpdateProcess
    {
        public event EventHandler UpdateEvent; 
        public event EventHandler ConfirmEvent;
        public event EventHandler ResultEvetnt;


        public IProcess<UpdateModel> SetOnLineProcess { get; set; }


        public IConfigurationRoot ConfigurationRoot { get; set; }

        public SelgrosApplicationUpdateStrategy(IConfigurationRoot configurationRoot)
        {
            ConfigurationRoot = configurationRoot;
        }

        public void SetOnline(UpdateModel updateModel)
        {
            var process = new SetOnLineProcess( );

            process.ProcessEvent += ProcessEvent;
            process.ConfirmEvent += ConfirmEvent;

            process.Process(updateModel);
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
            var checkVersionEvent = new CheckVersionProcess(ConfigurationRoot);

            checkVersionEvent.ProcessEvent += ProcessEvent;
            checkVersionEvent.ConfirmEvent += ConfirmEvent;

            checkVersionEvent.Process(updateModel);
        }

        public void CopyFiles(UpdateModel updateModel)
        {
            var copyfilesProcess = new CopyFilesProcess();

            copyfilesProcess.ProcessEvent += ProcessEvent;
            copyfilesProcess.ConfirmEvent += ConfirmEvent;

            copyfilesProcess.Process(updateModel);
        }

        public void CreateReport(UpdateModel updateModel)
        {
            return;
        }

        public void MakeBackup(UpdateModel updateModel)
        {
            var backupProcess = new BackupProcess(ConfigurationRoot);

            backupProcess.ProcessEvent += ProcessEvent;
            backupProcess.ConfirmEvent += ConfirmEvent;

            backupProcess.Process(updateModel);
        }

        public void Unzip(UpdateModel updateModel)
        {
            var unZipEvent = new UnzipProcess();

            unZipEvent.ProcessEvent += ProcessEvent;
            unZipEvent.ConfirmEvent += ConfirmEvent;

            var result = unZipEvent.Process(updateModel);
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
            var editWebConfig = new EditWebConfigProcess();

            editWebConfig.ProcessEvent += ProcessEvent;
            editWebConfig.ConfirmEvent += ConfirmEvent;

            editWebConfig.Process(updateModel);

        }

        public void PrepareEnviroment(UpdateModel updateModel)
        {
            var prepare = new PrepareEnviroment();

            prepare.ProcessEvent += ProcessEvent;
            prepare.ConfirmEvent += ConfirmEvent;

            prepare.Process(updateModel);

        }

        public void UndoProcess(UpdateModel updateModel)
        {
            var undoProcess = new UndoProcess();

            undoProcess.ProcessEvent += ProcessEvent;
            undoProcess.ConfirmEvent += ConfirmEvent;

            undoProcess.Process(updateModel);

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
                PrepareEnviroment,
                SetOffline,
                Unzip,
                CheckVersion,
                MakeBackup,
                CopyFiles,
                VerifyCopy,
                EditWebConfig,
                SetOnline
            };
        }
    }
}
