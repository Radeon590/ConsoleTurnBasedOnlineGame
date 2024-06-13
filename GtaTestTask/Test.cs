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
                new GtaClient(_server, new PlayerAuthData($"player{i}", $"password{i}", "jwt"));
            }
        }

        public void ShowUi()
        {
            PlayerAuthData playerAuthData = new PlayerAuthData("player3", "password3", "jwt");
            var client = new GtaClient(_server, playerAuthData);
            // move one enemy to main player
            WorldState worldState = _server.ReadCurrentWorldState();
            Player player1 = worldState.Players[0];
            worldState.Players.RemoveAt(0);

            float newX;
            if (client.Player.Position.X >= WorldBorder.X - 1)
            {
                newX = client.Player.Position.X - 1;
            }
            else
            {
                newX = client.Player.Position.X + 1;
            }
            worldState.Players.Add(new Player(player1.Username, new Vector2(newX, client.Player.Position.Y), player1.Health));
            _server.UpdateWorldState(worldState);
            //
            client.ShowUi();
        }
    }
}
