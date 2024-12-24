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
    internal class AttackCommand : Command
    {
        public override string CommandString => $"attack {_targetPlayer}";
        
        private string _targetPlayer;
        private int _damage;

        public AttackCommand(GtaClient client, GameServerData server, string targetPlayer, int damage) : base(client, server)
        {
            _targetPlayer = targetPlayer;
            _damage = damage;
        }

        public override async Task<string> Execute()
        {
            return await StogServerApiConnector.Attack(_server, _client.Player.Username, _targetPlayer, _damage);
        }
    }
}
