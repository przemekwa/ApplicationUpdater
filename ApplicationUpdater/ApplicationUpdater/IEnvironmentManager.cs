using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationUpdater
{
    public interface IEnvironmentManager
    {
        void Exit(int exitCode);
    }
}
