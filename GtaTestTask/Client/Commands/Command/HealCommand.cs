using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GtaTestTask.Server;

namespace GtaTestTask.Client.Commands.Command
{
    internal class HealCommand : Command
    {
        public override string CommandString => "heal";

        public HealCommand(GtaClient client, GtaServer server) : base(client, server)
        {
        }

        public override void Execute()
        {
            _server.Heal(_client.Username);
        }
    }
}
