﻿namespace ApplicationUpdater
{
    public interface IUpdateProcess
    {
        void Update(UpdateModel updateModel);
        void CreateReport(UpdateModel updateModel);
        void Undo(UpdateModel updateModel);
    }
}