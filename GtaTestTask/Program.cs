// init servers
using GtaTestTask.Server;
using GtaTestTask.WebApi;

GtaServer server0 = new GtaServer("0");
GtaServer server1 = new GtaServer("1");
GtaServer server2 = new GtaServer("2");

foreach (var item in GtaWebApi.Endpoints.ReadServers())
{
    Console.WriteLine(item.Name);
}