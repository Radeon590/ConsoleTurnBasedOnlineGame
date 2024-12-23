using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StogClient.Server;
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

        public override void Execute()
        {
            //_server.Attack(_client.Username, _targetPlayer, _damage);
        }
    }
}
