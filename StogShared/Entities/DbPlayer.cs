namespace StogShared.Entities;

public class DbPlayer
{
    public uint Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Coins { get; set; }
    public int Health { get; set; }

    public DbPlayer(string username, string password)
    {
        Username = username;
        Password = password;
        Coins = 1000;
        Health = 100;
    }
}