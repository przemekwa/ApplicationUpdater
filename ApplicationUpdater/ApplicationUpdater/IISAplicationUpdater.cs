
using System;
using System.Collections.Generic;

namespace ApplicationUpdater
{
    public class IISAplicationUpdater
    {
        public IUpdateProcess UpdateProcess { get; private set; }

        public IISAplicationUpdater(IUpdateProcess selgrosApplicationUpdateStrategy)
        {
            UpdateProcess = selgrosApplicationUpdateStrategy;
        }

        private void ExecuteProcess(IEnumerable<Action<UpdateModel>> actions, UpdateModel updateModel)
        {
            foreach (var action in actions)
            {
                action.Invoke(updateModel);
                Console.WriteLine();
            }
        }

        public void Update(UpdateModel updateModel)
        {
           ExecuteProcess(UpdateProcess.GetProcess(updateModel), updateModel);
        }
    }
}
