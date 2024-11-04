using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StogClient.Server;

namespace StogClient.Client.Commands.Command
{
    internal abstract class Command
    {
        protected readonly GtaClient _client;
        protected readonly GtaServer _server;

        public abstract string CommandString
        {
            get;
        }
        
        protected Command(GtaClient client, GtaServer server)
        {
            _client = client;
            _server = server;
        }

        public abstract void Execute();
    }
}
