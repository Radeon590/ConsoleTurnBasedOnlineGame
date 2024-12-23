namespace StogShared.Entities.GameWorld
{
    public class WorldState
    {
        public List<Player> Players { get; set; }
        public int CurrentPlayer { get; set; }

        public WorldState() 
        {
            Players = new List<Player>();
            CurrentPlayer = 0;
        }

        public WorldState(List<Player> players)
        {
            Players = players;
        }
    }
}
