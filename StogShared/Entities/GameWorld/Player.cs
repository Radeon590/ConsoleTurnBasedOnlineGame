using System.Numerics;

namespace StogShared.Entities.GameWorld
{
    public class Player
    {
        public string Username => DbData.Username;
        public Vector2 Position {  get; set; }

        public int Coins
        {
            get => DbData.Coins;
            set => DbData.Coins = value;
        }
        public int Health
        {
            get => DbData.Health; 
            set => DbData.Health = value;
        }
        public DbPlayer DbData { get; set; }

        public Player(DbPlayer dbData)
        {
            DbData = dbData;
            Position = new Vector2(0, 0);
        }
    }
}
