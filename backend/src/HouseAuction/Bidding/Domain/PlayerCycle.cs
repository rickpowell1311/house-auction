namespace HouseAuction.Bidding.Domain
{
    public class PlayerCycle
    {
        private readonly Dictionary<int, string> _players;

        private int _currentPlayerIndex = 0;

        public int TotalPlayers => _players.Count;

        public PlayerCycle(Dictionary<int, string> players)
        {
            _players = players;
        }

        public string CurrentPlayer => _players[_currentPlayerIndex];

        public void Next()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        }
    }
}
