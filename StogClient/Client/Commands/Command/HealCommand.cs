using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StogClient.Server;
using StogClient.WebApi;
using StogShared.Entities;

namespace StogClient.Client.Commands.Command
{
    internal class HealCommand : Command
    {
        public override string CommandString => "heal";

        public HealCommand(GtaClient client, GameServerData server) : base(client, server)
        {
        }

        public override async Task<string> Execute()
        {
            return await StogServerApiConnector.Heal(_server, _client.Player.Username);
        }
    }
}
