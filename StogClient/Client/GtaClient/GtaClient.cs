using Newtonsoft.Json;
using StogClient.WebApi;
using StogClient.Launcher;
using StogShared.Entities;
using StogShared.Entities.GameWorld;

namespace StogClient.Client
{
    internal partial class GtaClient
    {
        public string Username => LauncherAuthData.Username;
        public WorldState WorldState
        {
            get; set;
        }

        protected Player? _player;

        // TODO: во многом бесполезно, т.к. нельзя менять из-за struct. Возможно, лучше заменить на класс
        public Player Player 
        {
            get
            {
                if (_player == null)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(WorldState));
                    Console.WriteLine(Username);
                    _player = WorldState.Players.Where(p => p.Username == Username).First();
                }
                return _player;
            }
            set { _player = value; }
        }
        
        protected readonly GameServerData GameServerData;
        protected PlayerAuthData LauncherAuthData;
        protected string ServerJwt;

        public GtaClient(GameServerData serverData, PlayerAuthData playerLauncherAuthData) 
        {
            GameServerData = serverData;
            LauncherAuthData = playerLauncherAuthData;
        }

        public async Task StartGame()
        {
            await ConnectToServer();
            await StartGameLoop();
        }

        private async Task ConnectToServer()
        {
            var connectionResult = await StogServerApiConnector.ConnectToGameServer(GameServerData.ServerConnectionString, LauncherAuthData.Jwt);
            WorldState = connectionResult.WorldState;
            ServerJwt = connectionResult.Jwt;
        }

        private async Task StartGameLoop()
        {
            while (true)
            {
                ShowUi();
                Console.WriteLine("Please press any key");
                Console.ReadKey();
                await UpdateWorldState();
            }
        }

        private async Task UpdateWorldState()
        {
            WorldState = await StogServerApiConnector.GetWorldState(GameServerData.ServerConnectionString, ServerJwt);
        }
    }
}
