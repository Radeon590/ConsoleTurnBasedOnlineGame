using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StogShared;
using StogShared.Entities.GameWorld;

namespace StogServerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ActionsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly WorldState _worldState;
    
    public ActionsController(ILogger<ActionsController> logger, WorldState worldState)
    {
        _logger = logger;
        _worldState = worldState;
    }
    
    [HttpPatch]
    [Route("Attack")]
    [Authorize]
    public IResult Attack(string sourcePlayerUsername, string targetPlayerUsername, int damage)
    {
        var sourcePlayer = _worldState.Players.Single(p => p.Username == sourcePlayerUsername);
        var player = _worldState.Players.Single(p => p.Username == targetPlayerUsername);
        string actionResult = $"{sourcePlayerUsername} attacks {targetPlayerUsername}";
        if (player.Health <= 0)
        {
            actionResult += $" but {targetPlayerUsername} is already dead";
        }
        else
        {
            player.Health -= damage;
        }

        _worldState.AddLastAction(actionResult);
        sourcePlayer.CleanLastActions();
        return Results.Text(actionResult);
    }
    
    [HttpPatch]
    [Route("Move")]
    [Authorize]
    public IResult Move(string username, float x, float y)
    {
        Vector2 vector = new Vector2(x, y);
        _logger.LogInformation($"{username} moves {JsonConvert.SerializeObject(vector)}");
        var player = _worldState.Players.Single(p => p.Username == username);
        _logger.LogInformation($"player: {JsonConvert.SerializeObject(player)}");
        if (player == null)
        {
            throw new Exception($"Player {username} not found");
        }

        player.Position += vector;
        if (player.Position.X == WorldConstants.MapBorderX)
        {
            player.Position = player.Position with { X = 0 };
        }
        else if (player.Position.X == -1)
        {
            player.Position = player.Position with { X = WorldConstants.MapBorderX - 1 };
        }
        else if (player.Position.Y == WorldConstants.MapBorderY)
        {
            player.Position = player.Position with { Y = 0 };
        }
        else if (player.Position.Y == -1)
        {
            player.Position = player.Position with { Y = WorldConstants.MapBorderY - 1 };
        }

        string actionResult = $"{username} moved";
        _worldState.AddLastAction(actionResult);
        player.CleanLastActions();
        return Results.Text(actionResult);
    }
    
    [HttpPatch]
    [Route("Heal")]
    [Authorize]
    public IResult Heal(string targetPlayerUsername)
    {
        var player = _worldState.Players.Single(p => p.Username == targetPlayerUsername);
        string actionResult;
        if (player.Coins >= 100)
        {
            player.Health = WorldConstants.MaxHP;
            player.Coins -= 100;
            actionResult = $"{targetPlayerUsername} healed";
        }
        else
        {
            actionResult = $"{targetPlayerUsername} tried to heal but have not enough coins";
        }
        
        
        _worldState.AddLastAction(actionResult);
        player.CleanLastActions();
        return Results.Text(actionResult);
    }
}