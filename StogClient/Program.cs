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
await test.StartMachine();

// launch
//var client = new GtaLauncher().LaunchClient();