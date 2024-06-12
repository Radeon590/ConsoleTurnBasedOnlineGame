// init servers
using GtaTestTask.Client;
using GtaTestTask.Launcher;
using GtaTestTask.Server;
using GtaTestTask.WebApi;

for (int i = 0; i < 3; i++)
{
    GtaServer.StartNewServer($"{i}");
}

// test server and users
var testServer = GtaWebApi.Endpoints.ReadServer(0);
for (int i = 1; i < 3; i++)
{
    new GtaClient(testServer, new PlayerAuthData($"player{i}", $"password{i}", "jwt"));
}
// launch 
var client = new GtaLauncher().LaunchClient();