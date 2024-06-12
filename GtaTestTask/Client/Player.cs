using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Client
{
    internal struct Player
    {
        public string Username { get; set; }
        public Vector2 Position {  get; set; }
        public int Health { get; set; }

        public Player(string username, Vector2 spawnPos, int health) 
        {
            Username = username;
            Position = spawnPos;
            Health = health;
        }
    }
}
