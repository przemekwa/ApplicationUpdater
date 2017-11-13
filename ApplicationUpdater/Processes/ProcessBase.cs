using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public abstract class ProcessBase
    {
        public event EventHandler ProcessEvent;

        public void UpdateProcess(string msg)
        {
            ProcessEvent(msg, new EventArgs { });
        }
    }
}
