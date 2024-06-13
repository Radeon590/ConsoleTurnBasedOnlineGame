// init servers
using GtaTestTask;
using GtaTestTask.Client;
using GtaTestTask.Launcher;
using GtaTestTask.Server;
using GtaTestTask.WebApi;
using Newtonsoft.Json;
using System.Numerics;

//tests
var test = new Test();
test.Start();

// launch
//var client = new GtaLauncher().LaunchClient();