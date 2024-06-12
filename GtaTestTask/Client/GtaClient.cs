using GtaTestTask.Launcher;
using GtaTestTask.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client
{
    internal class GtaClient
    {
        public string Username => _authData.Username;
        public WorldState WorldState
        {
            get; set;
        }

        private bool _isHost;

        private readonly GtaServer _server;
        private PlayerAuthData _authData;

        public GtaClient(GtaServer server, PlayerAuthData playerAuthData) 
        {
            _server = server;
            _authData = playerAuthData;
            var connectionResult = _server.ConnectPlayer(this, new Random().Next(1, 250));
            WorldState = connectionResult.Item1;
            _isHost = connectionResult.Item2;
        }

        public void ShowUi()
        {
            while (true)
            {

            }
        }
    }
}
