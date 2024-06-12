using GtaTestTask.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.WebApi
{
    internal class GtaWebApi
    {
        #region Singleton

        private static GtaWebApi? s_endpoints;

        public static GtaWebApi Endpoints
        {
            get
            {
                if (s_endpoints is null)
                {
                    s_endpoints = new GtaWebApi();
                }
                return s_endpoints;
            }
        }

        #endregion

        private List<GtaServer> _servers = new List<GtaServer>();

        public GtaWebApi() { }

        #region Server

        public void ConnectServer(GtaServer server)
        {
            if (_servers.Contains(server)) 
            {
                throw new Exception("server already connected");
            }
            _servers.Add(server);
        }

        public void DisconnectServer(GtaServer server)
        {
            if (!_servers.Contains(server)) 
            {
                throw new Exception("server not connected");
            }
            _servers.Remove(server);
        }

        public List<GtaServer> ReadServers() 
        {
            return _servers;
        }

        #endregion

        public void Authorize()
        {

        }
    }
}
