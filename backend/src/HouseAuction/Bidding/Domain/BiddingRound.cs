using System.Linq;

namespace HouseAuction.Bidding.Domain
{
    public class BiddingRound
    {
        public int RoundNumber { get; }

        public string BiddingPhaseId { get; }

        public PlayerCycle PlayerCycle { get; }


        private readonly List<Play> _plays;

        public IEnumerable<Play> Plays => _plays;
        
        public bool HasFinished =>
            (_plays.Players().Count() == PlayerCycle.Players.Count && _plays.PlayersWhoPassed().Count() >= (PlayerCycle.Players.Count - 1))
            || (_plays.Count == PlayerCycle.Players.Count - 1 && _plays.All(x => x.IsPass));

        public BiddingRound(int roundNumber, string biddingPhaseId, PlayerCycle playerCycle)
        {
            BiddingPhaseId = biddingPhaseId;
            PlayerCycle = playerCycle;

            _plays = [];
        }

        public void MakePlay(Play play)
        {
            if (HasFinished)
            {
                throw new InvalidOperationException("Round has finished");
            }

            if (PlayerCycle.CurrentPlayer != play.Player)
            {
                throw new InvalidOperationException($"It's not {play.Player}'s turn");
            }

            var highestBid = _plays
                .Select(x => x.Amount)
                .DefaultIfEmpty(0)
                .Max();

            if (!play.IsPass && play.Amount <= highestBid)
            {
                throw new InvalidOperationException($"Bid must be higher than {highestBid}");
            }

            _plays.Add(play);

            if (!HasFinished)
            {
                PlayerCycle.Next();

                while (Plays.PlayersWhoPassed().Contains(PlayerCycle.CurrentPlayer))
                {
                    PlayerCycle.Next();
                }
            }
        }
    }
}
