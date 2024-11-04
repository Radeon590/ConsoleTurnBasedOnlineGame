using StogClient.WebApi;
using StogClient.WebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using StogClient.Launcher;
using StogClient.Server;

namespace StogClient.Client
{
    internal partial class GtaClient
    {
        public string Username => _authData.Username;
        public WorldState WorldState
        {
            get; set;
        }

        protected Player? _player;

        // TODO: во многом бесполезно, т.к. нельзя менять из-за struct. Возможно, лучше заменить на класс
        public Player Player 
        {
            get
            {
                if (_player == null)
                {
                    _player = WorldState.Players.Where(p => p.Username == Username).First();
                }
                return _player;
            }
            set { _player = value; }
        }

        protected bool _isHost;

        protected readonly GtaServer _server;
        protected PlayerAuthData _authData;

        public GtaClient(GtaServer server, PlayerAuthData playerAuthData) 
        {
            _server = server;
            _authData = playerAuthData;
            var connectionResult = _server.ConnectPlayer(this, new Random().Next(1, 250));
            WorldState = connectionResult.Item1;
            _isHost = connectionResult.Item2;
        }

        
    }
}
