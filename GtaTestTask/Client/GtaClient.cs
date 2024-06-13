using GtaTestTask.Launcher;
using GtaTestTask.Server;
using GtaTestTask.WebApi;
using GtaTestTask.WebApi.Entities;
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
        public string Username => _authData.Username;
        public WorldState WorldState
        {
            get; set;
        }

        private Player? _player;

        public Player Player 
        {
            get
            {
                if (_player == null)
                {
                    _player = WorldState.Players.Where(p => p.Username == Username).First();
                }
                return _player.Value;
            }
            set { _player = value; }
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

        private void ReadCommand()
        {
            
        }
    }
}
