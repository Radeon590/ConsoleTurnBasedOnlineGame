﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using StogClient.Server;
using StogClient.WebApi;
using StogShared.Entities;

namespace StogClient.Client.Commands.Command
{
    internal class MoveCommand : Command
    {
        public override string CommandString => $"move {_moveSide}";

        private string _moveSide
        {
            get
            {
                if (_vector == new Vector2(-1, 0))
                {
                    return "left";
                }
                if (_vector == new Vector2(1, 0))
                {
                    return "right";
                }
                if ( _vector == new Vector2(0, -1))
                {
                    return "up";
                }
                if (_vector == new Vector2(0, 1))
                {
                    return "down";
                }
                return $"({_vector.X}, {_vector.Y})";
            }
        }
        
        private Vector2 _vector;

        public MoveCommand(GtaClient client, GameServerData server, Vector2 vector) : base(client, server)
        {
            _vector = vector;
        }

        public override async Task<string> Execute()
        {
            return await StogServerApiConnector.Move(_server, _client.Player.Username, _vector);
        }
    }
}
