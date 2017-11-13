namespace ApplicationUpdater
{
    public interface IUpdateProcess
    {
        void Unzip(UpdateModel updateModel);
        void CheckVersion(UpdateModel updateModel);
        void MakeBackup(UpdateModel updateModel);
        void CopyFiles(UpdateModel updateModel);
        void VerifyCopy(UpdateModel updateModel);
        void CreateReport(UpdateModel updateModel);

        event System.EventHandler UpdateEvent;
    }
}