using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StogLauncherApi.Entities;
using StogLauncherApi.Services.GameServersPool;

namespace StogLauncherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GameServersController : ControllerBase
{
    private readonly GameServersPool _gameServersPool;
    
    public GameServersController(GameServersPool gameServersPool)
    {
        _gameServersPool = gameServersPool;
    }

    [HttpPost("Connect")]
    public void ConnectGameServer([FromBody] GameServerData gameServerData)
    {
        _gameServersPool.GameServers.Add(gameServerData);
    }

    [HttpGet("ReadAll")]
    [Authorize]
    public IResult ReadAllGameServers()
    {
        return Results.Json(_gameServersPool.GameServers);
    }
    
    [HttpGet("Read")]
    [Authorize]
    public IResult ReadGameServer(string serverName)
    {
        return Results.Json(_gameServersPool.GameServers.Single(gs => gs.ServerName == serverName));
    }

    [HttpPut("Update")]
    public void UpdateGameServer([FromBody] GameServerData gameServerData)
    {
        var pooledGameServer = _gameServersPool.GameServers.Single(s => s.ServerName == gameServerData.ServerName);
        var index = _gameServersPool.GameServers.IndexOf(pooledGameServer);
        _gameServersPool.GameServers[index] = gameServerData;
    }
    
    [HttpDelete("Disconnect")]
    public void DisconnectGameServer(string serverName)
    {
        var pooledGameServer = _gameServersPool.GameServers.Single(s => s.ServerName == serverName);
        _gameServersPool.GameServers.Remove(pooledGameServer);
    }
}