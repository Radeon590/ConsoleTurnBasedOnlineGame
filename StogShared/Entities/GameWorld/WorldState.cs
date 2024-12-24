namespace StogShared.Entities.GameWorld
{
    public class WorldState
    {
        public List<Player> Players { get; set; }
        public string? CurrentPlayerUsername { get; set; }

        public Player? CurrentPlayer
        {
            get => Players.SingleOrDefault(p => p.Username == CurrentPlayerUsername);
            set => CurrentPlayerUsername = value?.Username;
        }

        public WorldState() 
        {
            Players = new List<Player>();
            CurrentPlayerUsername = null;
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

        public void NextPlayer()
        {
            int nextPlayerIndex = Players.IndexOf(CurrentPlayer) + 1;
            if (nextPlayerIndex >= Players.Count)
            {
                nextPlayerIndex = 0;
            }
            CurrentPlayer = Players[nextPlayerIndex];
        }
    }
}
