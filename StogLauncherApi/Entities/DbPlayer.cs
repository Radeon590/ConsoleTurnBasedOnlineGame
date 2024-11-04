namespace StogLauncherApi.Entities;

public class DbPlayer
{
    public uint Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public DbPlayer(string username, string password)
    {
        Username = username;
        Password = password;
    }
}