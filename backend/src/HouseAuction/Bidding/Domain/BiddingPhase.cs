﻿namespace HouseAuction.Bidding.Domain
{
    public class BiddingPhase
    {
        public string GameId { get; private set; }

        private readonly List<BiddingRound> _rounds;

        public IEnumerable<BiddingRound> BiddingRounds => _rounds;

        public Deck Deck { get; private set; }

        public PlayerCycle PlayerCycle { get; private set; }

        public BiddingRound CurrentRound => BiddingRounds
            .OrderBy(x => x.RoundNumber)
            .FirstOrDefault(x => !x.HasFinished);

        public List<int> CurrentDeal => Deck.ForRound(CurrentRound.RoundNumber);

        public bool HasFinished => BiddingRounds.All(x => x.HasFinished);

        public BiddingPhase(string gameId, List<string> players)
        {
            Deck = Deck.ForNumberOfPlayers(players.Count);
            PlayerCycle = new PlayerCycle(gameId, players);
            GameId = gameId;

            _rounds = [];
            for (var i = 0; i < Deck.Properties.Count() / Deck.DealSize; i++)
            {
                _rounds.Add(new BiddingRound(i, this));
            }
        }

        private BiddingPhase(string gameId)
        {
            GameId = gameId;
        }
    }
}
