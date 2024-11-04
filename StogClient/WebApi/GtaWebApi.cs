using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StogClient.Server;
using StogClient.WebApi.Entities;

namespace StogClient.WebApi
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
        private TemporaryDb _db = new TemporaryDb();

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

        public GtaServer ReadServer(int id)
        {
            return _servers[id];
        }

        #endregion

        #region Auth

        public string Authorize(string username, string password)
        {
            if (_db.PlayerDbDatas.Where(p => p.Username == username && p.Password == password).Count() == 1)
            {
                return "jwt";
            }
            throw new Exception("user not found");
        }

        public string Registrate(string username, string password)
        {
            if (_db.PlayerDbDatas.Where(p => p.Username == username && p.Password == password).Count() == 0)
            {
                _db.PlayerDbDatas.Add(new Entities.PlayerDbData(username, password, 500));
                return "jwt";
            }
            throw new Exception("user already exists");
        }

        public void IsAuthorized(string jwt)
        {

        }

        #endregion

        #region User

        public PlayerDbData ReadUser(string username)
        {
            PlayerDbData? player = _db.PlayerDbDatas.Where(p => p.Username == username).FirstOrDefault();
            if (player == null)
            {
                throw new Exception("player not found");
            }
            return player;
        }

        public void UpdateUser(PlayerDbData playerDbData)
        {
            // нет необходимости что-то делать, т.к. при ReadUser уже передается ссылочный объект. в реальном апи надо было бы менять
        }

        #endregion
    }
}
