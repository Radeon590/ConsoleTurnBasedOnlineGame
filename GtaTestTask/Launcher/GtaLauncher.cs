using GtaTestTask.Client;
using GtaTestTask.Server;
using GtaTestTask.WebApi;
using GtaTestTask.WebApi.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaTestTask.Launcher
{
    internal class GtaLauncher
    {
        public GtaClient LaunchClient() 
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
                    playerAuthData = Authorize();
                }
                else if (command == "r")
                {
                    playerAuthData = Registrate();
                }
                else
                {
                    Console.WriteLine("wrong command");
                    continue;
                }
                Console.WriteLine("succesfully authorized");
                break;
            }
            //
            Console.Clear();
            List<GtaServer> gtaServers = GtaWebApi.Endpoints.ReadServers();
            Console.WriteLine("select server");
            foreach (var server in gtaServers)
            {
                Console.WriteLine($"{server.Name}");
            }
            while (true)
            {
                string serverName = Console.ReadLine();
                GtaServer? server = gtaServers.Where(s => s.Name == serverName).FirstOrDefault();
                if (server == null)
                {
                    Console.WriteLine("wrong server name");
                    continue;
                }
                return new GtaClient(server, playerAuthData);
            }
        }

        private PlayerAuthData Authorize()
        {
            while (true)
            {
                Console.WriteLine("authorization");
                string username = ReadLine("username");
                string password = ReadLine("password");
                try
                {
                    string jwt =  GtaWebApi.Endpoints.Authorize(username, password);
                    return new PlayerAuthData(username, password, jwt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

        private PlayerAuthData Registrate()
        {
            while (true)
            {
                Console.WriteLine("registration");
                string username = ReadLine("username");
                string password = ReadLine("password");
                try
                {
                    string jwt = GtaWebApi.Endpoints.Registrate(username, password);
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
