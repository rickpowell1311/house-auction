namespace HouseAuction.Bidding.Domain
{
    public class BiddingPhase
    {
        public string GameId { get; private set; }

        private readonly List<BiddingRound> _rounds;

        public IEnumerable<BiddingRound> BiddingRounds => _rounds;

        public Deck Deck { get; private set; }

        public PlayerCycle PlayerCycle { get; private set; }

        public bool HasFinished => BiddingRounds.Count() * PlayerCycle.Players.Count >= 28
            && BiddingRounds.All(x => x.HasFinished);

        public BiddingPhase(string gameId, List<string> players)
        {
            Deck = new Deck(players.Count);
            PlayerCycle = new PlayerCycle(gameId, players);
            GameId = gameId;

            _rounds = new List<BiddingRound>();
            NextRound();
        }

        private BiddingPhase(string gameId)
        {
            GameId = gameId;
        }

        public void NextRound()
        {
            var nextRound = _rounds.Select(x => x.RoundNumber).DefaultIfEmpty(-1).Max() + 1;

            _rounds.Add(new BiddingRound(nextRound, this));

            Deck.DealNext();
        }
    }
}
