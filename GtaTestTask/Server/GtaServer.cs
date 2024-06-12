using GtaTestTask.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Server
{
    internal class GtaServer
    {
        public string Name { get; private set; }

        public GtaServer(string serverName) 
        {
            Name = serverName;
            GtaWebApi.Endpoints.ConnectServer(this);
        }
    }
}
