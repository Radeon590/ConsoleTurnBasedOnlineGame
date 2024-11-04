using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StogClient.BotsClient;
using StogClient.Client;
using StogClient.Launcher;
using StogClient.Server;
using StogClient.WebApi;
using StogShared;

namespace StogClient
{
    internal class TestGameMachine
    {
        private GtaServer _server;
        private GtaClient _playerClient;
        private List<BotGtaClient> _botClients = new List<BotGtaClient>();

        public async Task StartMachine()
        {
            SetUpServers();
            await SetUpClients();
            SetUpBotClients();
            StartGame();
        }

        private void SetUpServers()
        {
            for (int i = 0; i < 3; i++)
            {
                GtaServer.StartNewServer($"{i}");
            }

            _server = StogLauncherApiConnector.Endpoints.ReadServer(0);
        }

        private void SetUpBotClients()
        {
            for (int i = 1; i < 3; i++)
            {
                //TODO: bots wont work
                _botClients.Add(new BotGtaClient(_playerClient, new GameServerData(), new PlayerAuthData($"player{i}", $"password{i}", "jwt")));
            }
        }

        public async Task SetUpClients()
        {
            _playerClient = await new GtaLauncher().LaunchClient();
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
