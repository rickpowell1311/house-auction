namespace HouseAuction.Bidding.Domain
{
    public class PlayerCycle
    {
        public Guid Id { get; private set; }

        public string BiddingPhaseId { get; private set; }

        private readonly Dictionary<int, string> _players;
        public IReadOnlyDictionary<int, string> Players => _players;

        public int CurrentPlayerIndex { get; private set; }

        public string CurrentPlayer => _players[CurrentPlayerIndex];

        public PlayerCycle(string biddingPhaseId, IEnumerable<string> players)
        {
            Id = Guid.NewGuid();
            BiddingPhaseId = biddingPhaseId;

            var shuffledOrder = players
                .OrderBy(x => new Random().NextDouble())
                .Select((x, i) => (x, i))
                .ToDictionary(y => y.i, y => y.x);

            _players = shuffledOrder;
        }

        private PlayerCycle(Guid id, string biddingPhaseId, int currentPlayerIndex)
        {
            Id = id;
            BiddingPhaseId = biddingPhaseId;
            CurrentPlayerIndex = currentPlayerIndex;
        }

        public void Next()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % _players.Count;
        }
    }
}
