using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUpdater.Processes
{
    public class ProcessConfirmation
    {
        private string _question;

        public string Question
        {
            get {
                return  $"{_question} [Y]es, [N]o, Cancel[Exit application]? " ; }
            set { _question = value; }
        }


        public ConsoleKey Key { get; set; }
    }
}
