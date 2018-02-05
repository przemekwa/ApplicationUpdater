namespace ApplicationUpdater
{
    public interface IUpdateProcess
    {
        void Update(UpdateModel updateModel);
        void CreateReport(UpdateModel updateModel);
        void UndoProcess(UpdateModel updateModel);
    }
}