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
        public WorldState WorldState;

        // TODO: во многом бесполезно, т.к. нельзя менять из-за struct. Возможно, лучше заменить на класс
        public Player Player 
        {
            get
            {
                return WorldState.Players.Where(p => p.Username == Username).First();
            }
        }
        
        protected readonly GameServerData GameServerData;
        protected PlayerAuthData LauncherAuthData;

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
            GameServerData.Jwt = connectionResult.Jwt;
        }

        private async Task StartGameLoop()
        {
            while (true)
            {
                await ShowUi();
                Console.WriteLine("Please press any key");
                Console.ReadKey();
                await UpdateWorldState();
            }
        }

        private async Task UpdateWorldState()
        {
            WorldState = await StogServerApiConnector.GetWorldState(GameServerData.ServerConnectionString, GameServerData.Jwt);
        }
    }
}
