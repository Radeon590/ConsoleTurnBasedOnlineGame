using GtaTestTask.Client;
using GtaTestTask.Launcher;
using GtaTestTask.Server;
using GtaTestTask.WebApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GtaTestTask
{
    internal class Test
    {
        private GtaServer _server;
        private GtaClient _playerClient;
        private List<GtaClient> _botClients = new List<GtaClient>();

        public Test() 
        {
            for (int i = 0; i < 3; i++)
            {
                GtaServer.StartNewServer($"{i}");
            }

            // pass server and users for testing
            _server = GtaWebApi.Endpoints.ReadServer(0);
            for (int i = 1; i < 3; i++)
            {
                _botClients.Add(new GtaClient(_server, new PlayerAuthData($"player{i}", $"password{i}", "jwt")));
            }

            //
            SetUpClients();
            MoveEnemiesToPlayer();
            Start();
        }

        public void SetUpClients()
        {
            PlayerAuthData playerAuthData = new PlayerAuthData("player3", "password3", "jwt");
            _playerClient = new GtaClient(_server, playerAuthData);
            
        }

        private void MoveEnemiesToPlayer()
        {
            MoveEnemyToPlayer();
            MoveEnemyToPlayer();
        }

        private void MoveEnemyToPlayer()
        {
            // move enemies to main player
            WorldState worldState = _server.ReadCurrentWorldState();
            Player player1 = worldState.Players[0];
            worldState.Players.RemoveAt(0);

            float newX;
            if (_playerClient.Player.Position.X >= WorldConstants.MapBorderX - 1)
            {
                newX = _playerClient.Player.Position.X - 1;
            }
            else
            {
                newX = _playerClient.Player.Position.X + 1;
            }
            worldState.Players.Add(new Player(player1.Username, new Vector2(newX, _playerClient.Player.Position.Y), player1.Health));
            _server.UpdateWorldState(worldState);
        }

        public void Start()
        {
            while (true)
            {
                _playerClient.ShowUi();
                MoveBots();
                Console.WriteLine("Please press any key");
                Console.ReadKey();
            }
        }

        public void MoveBots()
        {
            foreach(var bot in _botClients)
            {
                if (Vector2.Distance(bot.Player.Position, _playerClient.Player.Position) <= 1)
                {
                    bot.Attack(_playerClient.Username, 1);
                }
                //
                int moveSide = new Random().Next(0, 4);
                Vector2 moveVector = Vector2.Zero;
                switch (moveSide)
                {
                    case 0:
                        moveVector = new Vector2(-1, 0);
                        break;
                    case 1:
                        moveVector = new Vector2(1, 0);
                        break;
                    case 2:
                        moveVector = new Vector2(0, -1);
                        break;
                    case 3:
                        moveVector = new Vector2(0, 1);
                        break;
                    default: break;
                }
                bot.Move(moveVector);
            }
        }
    }
}
