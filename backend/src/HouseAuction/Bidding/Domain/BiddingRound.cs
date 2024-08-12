namespace HouseAuction.Bidding.Domain
{
    public class BiddingRound
    {
        public int RoundNumber { get; }

        public string GameId { get; }

        public PlayerCycle PlayerCycle { get; }

        private readonly Dictionary<string, Play> _plays;

        public IReadOnlyDictionary<string, Play> Plays => _plays;
        
        public bool HasFinished => 
            (Plays.Count == PlayerCycle.Players.Count && Plays.Values.Where(x => !x.IsPass).Count() <= 1)
            || (Plays.Count == PlayerCycle.Players.Count - 1 && Plays.Values.All(x => x.IsPass));

        public BiddingRound(int roundNumber, string gameId, PlayerCycle playerCycle)
        {
            GameId = gameId;
            PlayerCycle = playerCycle;

            _plays = [];
        }

        public void MakePlay(string player, Play play)
        {
            if (HasFinished)
            {
                throw new InvalidOperationException("Round has finished");
            }

            if (PlayerCycle.CurrentPlayer != player)
            {
                throw new InvalidOperationException($"It's not {player}'s turn");
            }

            var highestBid = Plays.Values
                .Select(x => x.Amount)
                .DefaultIfEmpty(0)
                .Max();

            if (!play.IsPass && play.Amount <= highestBid)
            {
                throw new InvalidOperationException($"Bid must be higher than {highestBid}");
            }

            _plays[player] = play;

            if (!HasFinished)
            {
                PlayerCycle.Next();

                var nextPlayer = PlayerCycle.CurrentPlayer;
                while (Plays.ContainsKey(nextPlayer) && Plays[nextPlayer].IsPass)
                {
                    PlayerCycle.Next();
                    nextPlayer = PlayerCycle.CurrentPlayer;
                }
            }
        }
    }
}
