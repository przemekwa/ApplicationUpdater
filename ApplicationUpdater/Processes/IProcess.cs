using System;

namespace ApplicationUpdater
{
    public interface IProcess<T> 
    {
        ProcesEventResult Process(T model);
        event EventHandler ProcessEvent;
        event EventHandler ConfirmEvent;
    }
}