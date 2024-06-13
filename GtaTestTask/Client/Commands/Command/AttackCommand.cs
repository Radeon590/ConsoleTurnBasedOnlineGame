using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client.Commands.Command
{
    internal class AttackCommand : ICommand
    {
        public string CommandString => $"attack {_targetPlayer}";

        GtaClient ICommand.Client => _client;

        private GtaClient _client;
        private string _targetPlayer;
        private int _damage;

        public AttackCommand(GtaClient client, string targetPlayer, int damage)
        {
            _client = client;
            _targetPlayer = targetPlayer;
            _damage = damage;
        }

        public void Execute()
        {
            _client.Attack(_targetPlayer, _damage);
        }
    }
}
