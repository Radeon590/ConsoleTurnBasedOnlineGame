using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client.Commands.Command
{
    internal class AttackCommand : Command
    {
        public override string CommandString => $"attack {_targetPlayer}";
        
        private string _targetPlayer;
        private int _damage;

        public AttackCommand(GtaClient client, string targetPlayer, int damage) : base(client)
        {
            _targetPlayer = targetPlayer;
            _damage = damage;
        }

        public override void Execute()
        {
            _client.Attack(_targetPlayer, _damage);
        }
    }
}
