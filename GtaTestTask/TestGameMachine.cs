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
using GtaTestTask.BotsClient;
using Newtonsoft.Json;

namespace GtaTestTask
{
    internal class TestGameMachine
    {
        private GtaServer _server;
        private GtaClient _playerClient;
        private List<BotGtaClient> _botClients = new List<BotGtaClient>();

        public TestGameMachine() 
        {
            SetUpServers();
            SetUpClients();
            SetUpBotClients();
            StartGame();
        }

        private void SetUpServers()
        {
            for (int i = 0; i < 3; i++)
            {
                GtaServer.StartNewServer($"{i}");
            }

            _server = GtaWebApi.Endpoints.ReadServer(0);
        }

        private void SetUpBotClients()
        {
            for (int i = 1; i < 3; i++)
            {
                _botClients.Add(new BotGtaClient(_playerClient, _server, new PlayerAuthData($"player{i}", $"password{i}", "jwt")));
            }
        }

        public void SetUpClients()
        {
            _playerClient = new GtaLauncher().LaunchClient();
        }

        public void StartGame()
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
                bot.MakeMove();
            }
        }
    }
}
