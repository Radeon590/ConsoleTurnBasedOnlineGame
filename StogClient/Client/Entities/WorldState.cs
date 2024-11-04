using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogClient.Client
{
    internal class WorldState
    {
        public List<Player> Players { get; set; }

        public WorldState() 
        {
            Players = new List<Player>();
        }

        public WorldState(List<Player> players)
        {
            Players = players;
        }
    }
}
