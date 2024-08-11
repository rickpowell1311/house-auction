namespace HouseAuction.Bidding.Domain
{
    public class PlayerCycle
    {
        private readonly Dictionary<int, string> _players;
        public IReadOnlyDictionary<int, string> Players => _players;

        public int CurrentPlayerIndex { get; private set; }

        public string CurrentPlayer => _players[CurrentPlayerIndex];

        public PlayerCycle(IEnumerable<string> players)
        {
            var shuffledOrder = players
                .OrderBy(x => new Random().NextDouble())
                .Select((x, i) => (x, i))
                .ToDictionary(y => y.i, y => y.x);

            _players = shuffledOrder;
        }

        public void Next()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % _players.Count;
        }
    }
}
