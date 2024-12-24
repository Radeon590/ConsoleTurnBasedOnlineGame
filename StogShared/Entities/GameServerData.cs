namespace StogShared.Entities;

public class GameServerData
{
    public string ServerName { get; set; }
    public string ServerConnectionString { get; set; }
    public string? Jwt { get; set; }
}