using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Launcher
{
    internal class PlayerAuthData
    {
        public string Username {  get; set; }
        public string Password { get; set; }
        public string Jwt { get; set; }

        public PlayerAuthData(string username, string password, string jwt)
        {
            Username = username;
            Password = password;
            Jwt = jwt;
        }
    }
}
