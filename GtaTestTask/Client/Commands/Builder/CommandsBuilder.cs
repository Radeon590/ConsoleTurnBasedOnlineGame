using GtaTestTask.Client.Commands.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client.Commands
{
    internal class CommandsBuilder
    {
        private Commands _commands;
        private GtaClient _client;

        public CommandsBuilder(GtaClient client)
        {
            _client = client;
            _commands = new Commands();
        }

        public void AddMove(Vector2 vector)
        {
            _commands.Add(new MoveCommand(_client, vector));
        }

        public void AddAttack(string targetPlayer, int damage)
        {
            _commands.Add(new AttackCommand(_client, targetPlayer, damage));
        }

        public void AddHeal()
        {
            _commands.Add(new HealCommand(_client));
        }

        public Commands Build()
        {
            return _commands;
        }
    }
}
