using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client.Commands.Command
{
    internal abstract class Command
    {
        protected readonly GtaClient _client;

        public abstract string CommandString
        {
            get;
        }
        
        protected Command(GtaClient client)
        {
            _client = client;
        }

        public abstract void Execute();
    }
}
