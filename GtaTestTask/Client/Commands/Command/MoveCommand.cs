using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client.Commands.Command
{
    internal class MoveCommand : ICommand
    {
        public string CommandString => $"move {_moveSide}";

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

        GtaClient ICommand.Client => _client;

        private readonly GtaClient _client;
        private Vector2 _vector;

        public MoveCommand(GtaClient client, Vector2 vector)
        {
            _client = client;
            _vector = vector;
        }

        public void Execute()
        {
            _client.Move(_vector);
        }
    }
}
