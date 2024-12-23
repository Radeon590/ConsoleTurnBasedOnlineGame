using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StogClient.ApiConnectors.Base;
using StogClient.Server;
using StogShared;
using StogShared.Entities;

namespace StogClient.WebApi
{
    internal class StogLauncherApiConnector : StogApiConnector
    {
        protected override string BaseUrl => "http://localhost:5153";
        
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
            var result = await HttpClient.GetAsync($"{BaseUrl}/Authorization/Authorize?username={username}&password={password}");
            if (result.IsSuccessStatusCode)
            {
                return await SetUpJwtToken(result);
            }
            throw new Exception($"{result.StatusCode} {await result.Content.ReadAsStringAsync()}");
        }

        public async Task<string> Registrate(string username, string password)
        {
            var result = await HttpClient.PostAsync($"{BaseUrl}/Authorization/Register?username={username}&password={password}", null);
            if (result.IsSuccessStatusCode)
            {
                return await SetUpJwtToken(result);
            }
            throw new Exception($"{result.StatusCode} {await result.Content.ReadAsStringAsync()}");
        }

        private async Task<string> SetUpJwtToken(HttpResponseMessage httpResponseMessage)
        {
            var resultJwt = await httpResponseMessage.Content.ReadAsStringAsync();
            resultJwt = resultJwt.Replace("\"", "");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + resultJwt);
            return resultJwt;
        }

        #endregion
    }
}
