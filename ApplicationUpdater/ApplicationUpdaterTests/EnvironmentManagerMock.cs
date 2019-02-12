using System;
using System.Collections.Generic;
using System.Text;
using ApplicationUpdater;

namespace ApplicationUpdaterTests
{
    class EnvironmentManagerMock : IEnvironmentManager
    {
        private int counter;

        public int Counter
        {
            get { return counter; }
            private set { counter = value; }
        }

        public EnvironmentManagerMock()
        {
            counter = 0;
        }

        public void Exit(int exitCode)
        {
            counter++;
        }
    }
}
