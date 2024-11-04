// init servers
using StogClient;
using StogClient.Client;
using StogClient.Launcher;
using StogClient.Server;
using StogClient.WebApi;
using Newtonsoft.Json;
using System.Numerics;

//tests
var test = new TestGameMachine();
test.StartGame();

// launch
//var client = new GtaLauncher().LaunchClient();