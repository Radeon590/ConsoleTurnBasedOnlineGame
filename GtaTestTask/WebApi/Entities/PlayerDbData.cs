using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.WebApi.Entities
{
    internal class PlayerDbData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Coins { get; set; }

        public PlayerDbData(string username, string password, int coins) 
        {
            Username = username;
            Password = password;
            Coins = coins;
        }
    }
}
