using ApplicationUpdater.Processes;
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
        public event EventHandler ConfirmEvent;
        public event EventHandler ResultEvetnt;

        public void SetOnline(UpdateModel updateModel)
        {
            var process = new SetOnLineProcess();

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
            var checkVersionEvent = new CheckVersionProcess();

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
            var backupProcess = new BackupProcess();

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

        public void Update(UpdateModel updateModel)
        {
            if (updateModel.IsUndoProcess)
            {
                ExecuteProcess(new List<Action<UpdateModel>>
                {
                    SetOffline,
                    UndoProcess,
                    SetOnline
                }, updateModel);

                return;
            }

            ExecuteProcess(new List<Action<UpdateModel>>
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
            }, updateModel);
        }

        private void ExecuteProcess(IEnumerable<Action<UpdateModel>> actions, UpdateModel updateModel)
        {
            foreach (var action in actions)
            {
                action.Invoke(updateModel);
            }
        }
    }
}
