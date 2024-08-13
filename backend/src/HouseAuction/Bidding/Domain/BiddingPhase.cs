namespace HouseAuction.Bidding.Domain
{
    public class BiddingPhase
    {
        public string GameId { get; private set; }

        private readonly ICollection<BiddingRound> _rounds;

        public IEnumerable<BiddingRound> BiddingRounds => _rounds;

        private readonly ICollection<Hand> _hands;
        public IEnumerable<Hand> Hands => _hands;

        public Deck Deck { get; private set; }

        public PlayerCycle PlayerCycle { get; private set; }

        public bool HasFinished => BiddingRounds.All(x => x.HasFinished);

        public BiddingPhase(string gameId, List<string> players)
        {
            Deck = Deck.ForNumberOfPlayers(players.Count);
            PlayerCycle = new PlayerCycle(gameId, players);
            GameId = gameId;

            _rounds = [];
            for (var i = 0; i < Deck.Properties.Count() / Deck.DealSizePerRound; i++)
            {
                _rounds.Add(new BiddingRound(i, this));
            }

            _hands = [];
            for (var i = 0; i < players.Count; i++)
            {
                _hands.Add(Hand.ForPlayer(
                    gameId, 
                    players[i],
                    players.Count));
            }
        }

        private BiddingPhase(string gameId)
        {
            GameId = gameId;

            _rounds = [];
            _hands = [];
        }
    }
}
