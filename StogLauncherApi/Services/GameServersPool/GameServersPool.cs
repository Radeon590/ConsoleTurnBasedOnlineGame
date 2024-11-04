using StogLauncherApi.Entities;
using StogShared;

namespace StogLauncherApi.Services.GameServersPool;

public class GameServersPool
{
    public List<GameServerData> GameServers { get; set; } = new List<GameServerData>();
}