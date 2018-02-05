using System;
using System.Collections.Generic;

namespace ApplicationUpdater
{
    public interface IUpdateProcess
    {
        event EventHandler UpdateEvent;
        event EventHandler ConfirmEvent;
        event EventHandler ResultEvetnt;
        IEnumerable<Action<UpdateModel>> GetProcess(UpdateModel updateModel);
    }
}