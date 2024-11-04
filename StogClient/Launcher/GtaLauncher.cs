using Newtonsoft.Json;
using StogClient.Client;
using StogClient.Server;
using StogClient.WebApi;
using StogShared;

namespace StogClient.Launcher
{
    internal class GtaLauncher
    {
        public HttpClient Client { get; set; }
        
        public async Task<GtaClient> LaunchClient() 
        {
            Console.Clear();
            Console.WriteLine("start launcher");
            Console.WriteLine("authorize or registrate? (a or r)");
            PlayerAuthData playerAuthData;
            while (true)
            {
                string command = Console.ReadLine();
                if (command == "a")
                {
                    playerAuthData = await Authorize();
                }
                else if (command == "r")
                {
                    playerAuthData = await Registrate();
                }
                else
                {
                    Console.WriteLine("wrong command");
                    continue;
                }
                Console.WriteLine("succesfully authorized. Press any key to continue");
                Console.ReadKey();
                break;
            }
            //
            Console.Clear();
            List<GameServerData> gtaServers = await StogLauncherApiConnector.Endpoints.ReadServers();
            Console.WriteLine("select server");
            foreach (var server in gtaServers)
            {
                Console.WriteLine($"{server.ServerName}");
            }
            while (true)
            {
                string serverName = Console.ReadLine();
                GameServerData? server = gtaServers.FirstOrDefault(s => s.ServerName == serverName);
                if (server == null)
                {
                    Console.WriteLine("wrong server name");
                    continue;
                }
                var client = new GtaClient(server.Value, playerAuthData);
                Console.WriteLine("GtaClient succesfully launched. Press any key to continue");
                Console.WriteLine(JsonConvert.SerializeObject(server.Value));
                Console.ReadKey();
                return client;
            }
        }

        private async Task<PlayerAuthData> Authorize()
        {
            while (true)
            {
                Console.WriteLine("authorization");
                string username = ReadLine("username");
                string password = ReadLine("password");
                try
                {
                    string jwt =  await StogLauncherApiConnector.Endpoints.Authorize(username, password);
                    return new PlayerAuthData(username, password, jwt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

        private async Task<PlayerAuthData> Registrate()
        {
            while (true)
            {
                Console.WriteLine("registration");
                string username = ReadLine("username");
                string password = ReadLine("password");
                try
                {
                    string jwt = await StogLauncherApiConnector.Endpoints.Registrate(username, password);
                    return new PlayerAuthData(username, password, jwt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

        private string ReadLine(string lineName)
        {
            string? result = null;
            while (result is null)
            {
                Console.WriteLine($"enter {lineName}");
                result = Console.ReadLine();
            }

            return result;
        }
    }
}
