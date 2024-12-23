using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using StogClient.Client.Commands.Command;
using StogClient.Server;
using StogShared.Entities;

namespace StogClient.Client.Commands
{
    internal class CommandsBuilder
    {
        private Commands _commands;
        private GtaClient _client;
        private GameServerData _server;

        public CommandsBuilder(GtaClient client, GameServerData server)
        {
            _client = client;
            _server = server;
            _commands = new Commands();
        }

        public void AddMove(Vector2 vector)
        {
            _commands.Add(new MoveCommand(_client, _server, vector));
        }

        public void AddAttack(string targetPlayer, int damage)
        {
            _commands.Add(new AttackCommand(_client, _server, targetPlayer, damage));
        }

        public void AddHeal()
        {
            _commands.Add(new HealCommand(_client, _server));
        }

        public Commands Build()
        {
            return _commands;
        }
    }
}
