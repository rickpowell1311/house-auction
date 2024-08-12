namespace HouseAuction.Bidding.Domain
{
    public class BiddingPhase
    {
        public string GameId { get; }

        private readonly List<BiddingRound> _rounds;

        public IEnumerable<BiddingRound> BiddingRounds => _rounds;

        public Deck Deck { get; private set; }

        public PlayerCycle PlayerCycle { get; private set; }

        public bool HasFinished => BiddingRounds.Count() * PlayerCycle.Players.Count >= 28
            && BiddingRounds.All(x => x.HasFinished);

        public BiddingPhase(string gameId, List<string> players)
        {
            _rounds = new List<BiddingRound>();

            Deck = new Deck(players.Count);
            GameId = gameId;
        }

        public void NextRound()
        {
            var nextRound = _rounds.Select(x => x.RoundNumber).DefaultIfEmpty(-1).Max() + 1;

            _rounds.Add(new BiddingRound(nextRound, GameId, PlayerCycle));

            Deck.DealNext();
        }
    }
}
