namespace ApplicationUpdater
{
    public interface IUpdateStrategy
    {
        void Unzip(UpdateModel updateModel);
        void CheckVersion(UpdateModel updateModel);
        void MakeBackup(UpdateModel updateModel);
        void CopyFiles(UpdateModel updateModel);
        void VerifyCopy(UpdateModel updateModel);
        void CreateReport(UpdateModel updateModel);
    }
}