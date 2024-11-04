using System.Numerics;
using StogClient.Client;
using StogClient.Launcher;
using StogClient.Server;
using StogShared;

namespace StogClient.BotsClient;

internal class BotGtaClient : GtaClient
{
    private GtaClient _targetPlayerClient;
    
    public BotGtaClient(GtaClient targetPlayerClient, GameServerData server, PlayerAuthData playerAuthData) : base(server, playerAuthData)
    {
        _targetPlayerClient = targetPlayerClient;
    }

    public void MakeMove()
    {
        if (Player.Health > 0)
        {
            if (Vector2.Distance(Player.Position, _targetPlayerClient.Player.Position) <= 1)
            {
                Attack(1);
            }
            //
            int moveSide = new Random().Next(0, 4);
            Vector2 moveVector = Vector2.Zero;
            switch (moveSide)
            {
                case 0:
                    moveVector = new Vector2(-1, 0);
                    break;
                case 1:
                    moveVector = new Vector2(1, 0);
                    break;
                case 2:
                    moveVector = new Vector2(0, -1);
                    break;
                case 3:
                    moveVector = new Vector2(0, 1);
                    break;
                default: break;
            }
            Move(moveVector);
        }
    }

    public void Attack(int damage)
    {
        _server.Attack(Player.Username, _targetPlayerClient.Username, damage);
    }

    public void Move(Vector2 vector)
    {
        _server.Move(Player.Username, vector);
    }
}