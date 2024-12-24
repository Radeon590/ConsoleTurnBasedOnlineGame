using System.Numerics;
using StogClient.Client;
using StogClient.WebApi;
using StogShared;

namespace StogClient.Server;

internal partial class GtaServer
{
    // TODO: на левом краю карты боты иногда пропадают
    public void Move(string username, Vector2 vector)
    {
        var worldState = ReadCurrentWorldState();
        var player = worldState.Players.Single(p => p.Username == username);
        if (player == null)
        {
            throw new Exception($"Player {username} not found");
        }

        player.Position = player.Position + vector;
        if (player.Position.X == WorldConstants.MapBorderX)
        {
            player.Position = new Vector2(0, player.Position.Y);
        }
        else if (player.Position.X == -1)
        {
            player.Position = new Vector2(WorldConstants.MapBorderX - 1, player.Position.Y);
        }
        else if (player.Position.Y == WorldConstants.MapBorderY)
        {
            player.Position = new Vector2(player.Position.X, 0);
        }
        else if (player.Position.Y == -1)
        {
            player.Position = new Vector2(player.Position.X, WorldConstants.MapBorderY - 1);
        }

        UpdateWorldState(worldState);
        Console.WriteLine($"{username} moved");
    }

    public void Attack(string sourcePlayerUsername, string targetPlayerUsername, int damage)
    {
        var worldState = ReadCurrentWorldState();
        var player = worldState.Players.Single(p => p.Username == targetPlayerUsername);
        if (player.Health <= 0)
        {
            Console.WriteLine($"{targetPlayerUsername} is already dead");
        }
        else
        {
            player.Health -= damage;
            UpdateWorldState(worldState);
            Console.WriteLine($"{sourcePlayerUsername} attacks {targetPlayerUsername}");
        }
    }
    
    /*public void Heal(string targetPlayerUsername)
    {
        var worldState = ReadCurrentWorldState();
        var player = worldState.Players.Single(p => p.Username == targetPlayerUsername);
        var playerDbData = StogLauncherApiConnector.Endpoints.ReadUser(targetPlayerUsername);
        if (playerDbData.Coins >= 100)
        {
            player.Health = WorldConstants.MaxHP;
            UpdateWorldState(worldState);
            playerDbData.Coins -= 100;
            StogLauncherApiConnector.Endpoints.UpdateUser(playerDbData);
            Console.WriteLine($"{targetPlayerUsername} healed");
        }
        else
        {
            Console.WriteLine($"{targetPlayerUsername} not enough coins");
        }
    }*/
}