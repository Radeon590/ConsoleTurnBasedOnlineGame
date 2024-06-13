using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client.Commands.Command
{
    internal interface ICommand
    {
        protected GtaClient Client
        {
            get;
        }

        public string CommandString
        {
            get;
        }

        public void Execute();
    }
}
