using GtaTestTask.Client.Commands;
using GtaTestTask.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client
{
    internal partial class GtaClient
    {
        private CommandsBuilder _availableCommandsBuilder;

        private void ResetAvailableCommandsBuilder()
        {
            _availableCommandsBuilder = new CommandsBuilder(this);
            _availableCommandsBuilder.AddMove(new Vector2(-1, 0));
            _availableCommandsBuilder.AddMove(new Vector2(1, 0));
            _availableCommandsBuilder.AddMove(new Vector2(0, -1));
            _availableCommandsBuilder.AddMove(new Vector2(0, 1));
            _availableCommandsBuilder.AddHeal();
        }

        private void ReadCommand()
        {
            Console.WriteLine();
            Console.WriteLine("enter command");
            string? commandString = Console.ReadLine();
            if (commandString != null) 
            {
                var command = _availableCommandsBuilder.Build().Where(c => c.CommandString == commandString).FirstOrDefault();
                if (command != null) 
                {
                    command.Execute();
                }
                else 
                {
                    Console.WriteLine("command not available");
                }
            }
        }

        // TODO: на левом краю карты боты иногда пропадают
        public void Move(Vector2 vector)
        {
            Player.Position = Player.Position + vector;
            if (Player.Position.X == WorldConstants.MapBorderX)
            {
                Player.Position = new Vector2(0, Player.Position.Y);
            }
            else if (Player.Position.X == -1)
            {
                Player.Position = new Vector2(WorldConstants.MapBorderX - 1, Player.Position.Y);
            }
            else if (Player.Position.Y == WorldConstants.MapBorderY)
            {
                Player.Position = new Vector2(Player.Position.X, 0);
            }
            else if (Player.Position.Y == -1)
            {
                Player.Position = new Vector2(Player.Position.X, WorldConstants.MapBorderY - 1);
            }
            _server.UpdateWorldState(WorldState);
            Console.WriteLine($"{Username} moved");
        }

        public void Attack(string targetPlayer, int damage)
        {
            var player = WorldState.Players.Where(p => p.Username == targetPlayer).First();
            if (player.Health <= 0)
            {
                Console.WriteLine($"{targetPlayer} is already dead");
            }
            else
            {
                player.Health -= damage;
                _server.UpdateWorldState(WorldState);
                Console.WriteLine($"{Username} attacks {targetPlayer}");
            }
        }

        public void Heal()
        {
            var playerDbData = GtaWebApi.Endpoints.ReadUser(Username);
            if (playerDbData.Coins >= 100)
            {
                Player.Health = WorldConstants.MaxHP;
                _server.UpdateWorldState(WorldState);
                playerDbData.Coins -= 100;
                GtaWebApi.Endpoints.UpdateUser(playerDbData);
                Console.WriteLine($"{Username} healed");
            }
            else
            {
                Console.WriteLine($"{Username} not enough coins");
            }
        }
    }
}
