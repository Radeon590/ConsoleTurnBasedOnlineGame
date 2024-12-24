namespace StogShared.Entities.GameWorld
{
    public class WorldState
    {
        public List<Player> Players { get; set; }
        public Player? CurrentPlayer { get; set; }

        public WorldState() 
        {
            Players = new List<Player>();
            CurrentPlayer = null;
        }

        public WorldState(List<Player> players)
        {
            Players = players;
        }

        public void AddLastAction(string action)
        {
            foreach (var player in Players)
            {
                player.LastActions.Add(action);
            }
        }
    }
}
