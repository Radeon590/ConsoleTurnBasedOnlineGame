using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StogClient.Server;
using StogShared.Entities;

namespace StogClient.Client.Commands.Command
{
    internal abstract class Command
    {
        protected readonly GtaClient _client;
        protected readonly GameServerData _server;

        public abstract string CommandString
        {
            get;
        }
        
        protected Command(GtaClient client, GameServerData server)
        {
            _client = client;
            _server = server;
        }

        /// <summary>
        /// executes this command
        /// </summary>
        /// <returns>execution result in string format</returns>
        public abstract Task<string> Execute();
    }
}
