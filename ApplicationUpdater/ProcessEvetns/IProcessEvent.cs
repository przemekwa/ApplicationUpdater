using System;

namespace ApplicationUpdater
{
    public interface IProcessEvent<T> 
    {
        ProcesEventResult Process(T model);
        event EventHandler ProcessEvent;
    }
}