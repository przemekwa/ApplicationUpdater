using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationUpdater
{
    public class EnvironmentManager : IEnvironmentManager
    {
        public void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }
}
