namespace HouseAuction.Bidding.Domain
{
    public class PlayerCycle
    {
        private readonly Dictionary<int, string> _players;
        public IReadOnlyDictionary<int, string> Players => _players;

        public int CurrentPlayerIndex { get; private set; }

        public string CurrentPlayer => _players[CurrentPlayerIndex];

        public PlayerCycle(Dictionary<int, string> players)
        {
            _players = players;
        }

        public void Next()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % _players.Count;
        }
    }
}
