using System;
using System.Collections.Generic;

namespace ApplicationUpdater
{
    public interface IUpdateProcess
    {
        IEnumerable<Action<UpdateModel>> GetProcess(UpdateModel updateModel);
    }
}