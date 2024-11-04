using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StogClient.ApiConnectors.Base;
using StogClient.Server;
using StogClient.WebApi.Entities;
using StogShared;

namespace StogClient.WebApi
{
    internal class StogLauncherApiConnector : StogApiConnector
    {
        #region Singleton

        private static StogLauncherApiConnector? s_endpoints;

        public static StogLauncherApiConnector Endpoints
        {
            get
            {
                if (s_endpoints is null)
                {
                    s_endpoints = new StogLauncherApiConnector();
                }
                return s_endpoints;
            }
        }

        #endregion

        private List<GtaServer> _servers = new List<GtaServer>();

        public StogLauncherApiConnector()
        {
            
        }

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

        public async Task<List<GameServerData>> ReadServers() 
        {
            var result = await HttpClient.GetAsync($"{BaseUrl}/GameServers/ReadAll");
            if (result.IsSuccessStatusCode)
            {
                var deserializedResult = await result.Content.ReadFromJsonAsync<List<GameServerData>>();
                if (deserializedResult != null)
                {
                    return deserializedResult;
                }
                else
                {
                    throw new Exception("Cant read servers list");
                }
            }
            throw new Exception($"StatusCode: {result.StatusCode} Exception: {await result.Content.ReadAsStringAsync()}");
        }

        public GtaServer ReadServer(int id)
        {
            return _servers[id];
        }

        #endregion

        #region Auth

        public async Task<string> Authorize(string username, string password)
        {
            var result = await HttpClient.GetAsync($"{BaseUrl}/Authorize?username={username}&password={password}");
            if (result.IsSuccessStatusCode)
            {
                return await SetUpJwtToken(result);
            }
            throw new Exception(await result.Content.ReadAsStringAsync());
        }

        public async Task<string> Registrate(string username, string password)
        {
            var result = await HttpClient.PostAsync($"{BaseUrl}/Register?username={username}&password={password}", null);
            if (result.IsSuccessStatusCode)
            {
                return await SetUpJwtToken(result);
            }
            throw new Exception(await result.Content.ReadAsStringAsync());
        }

        private async Task<string> SetUpJwtToken(HttpResponseMessage httpResponseMessage)
        {
            var resultJwt = await httpResponseMessage.Content.ReadAsStringAsync();
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + resultJwt);
            return resultJwt;
        }

        #endregion

        #region User

        public PlayerDbData ReadUser(string username)
        {
            /*PlayerDbData? player = _db.PlayerDbDatas.Where(p => p.Username == username).FirstOrDefault();
            if (player == null)
            {
                throw new Exception("player not found");
            }
            return player;*/
            return null;
        }

        public void UpdateUser(PlayerDbData playerDbData)
        {
            // нет необходимости что-то делать, т.к. при ReadUser уже передается ссылочный объект. в реальном апи надо было бы менять
        }

        #endregion
    }
}
