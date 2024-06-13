using GtaTestTask.Client;
using GtaTestTask.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Server
{
    internal class GtaServer
    {
        public string Name { get; private set; }

        private List<GtaClient> _connectedClients = new List<GtaClient>();
        private GtaClient? _host = null;
        private int? _minPing = null;

        public GtaServer(string serverName) 
        {
            Name = serverName;
            GtaWebApi.Endpoints.ConnectServer(this);
        }

        public static void StartNewServer(string serverName)
        {
            var server = new GtaServer(serverName);
        }

        public (WorldState, bool) ConnectPlayer(GtaClient client, int ping)
        {
            _connectedClients.Add(client);
            //
            Random random = new Random();
            Vector2 spawnPos = new Vector2(random.Next(0, WorldConstants.MapBorderX), random.Next(0, WorldConstants.MapBorderY));
            int health = 10;
            Player newPlayer = new Player(client.Username, spawnPos, health);
            //
            WorldState worldState;
            bool isHost = false;
            //
            if (_host == null)
            {
                _minPing = ping;
                _host = client;
                //
                worldState = new WorldState(new List<Player>() { newPlayer });
                isHost = true;
            }
            else
            {
                if (ping < _minPing)
                {
                    client.WorldState = _host.WorldState;
                    //
                    _minPing = ping;
                    _host = client;
                    isHost = true;
                }
                worldState = _host.WorldState;
                worldState.Players.Add(newPlayer);
            }

            return (worldState, isHost);
        }

        public WorldState ReadCurrentWorldState()
        {
            return _host!.WorldState;
        }

        public void UpdateWorldState(WorldState updatedWorldState)
        {
            _host!.WorldState = updatedWorldState;
        }
    }
}
