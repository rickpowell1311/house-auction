using HouseAuction.Bidding.Domain.Events;
using Onwrd.EntityFrameworkCore;
using System.Linq;

namespace HouseAuction.Bidding.Domain
{
    public class BiddingRound : EventRaiser
    {
        public Guid Id { get; private set; }

        public int RoundNumber { get; private set; }

        public BiddingPhase BiddingPhase { get; private set; }


        private readonly ICollection<Play> _plays;

        public IReadOnlyCollection<Play> Plays => _plays.ToList();
        
        public bool HasFinished =>
            (_plays.Players().Count() == BiddingPhase.PlayerCycle.Players.Count && _plays.PlayersWhoPassed().Count() >= (BiddingPhase.PlayerCycle.Players.Count - 1))
            || (_plays.Count == BiddingPhase.PlayerCycle.Players.Count - 1 && _plays.All(x => x.IsPass));

        public BiddingRound(int roundNumber, BiddingPhase biddingPhase)
        {
            Id = Guid.NewGuid();
            BiddingPhase = biddingPhase;

            _plays = [];
        }

        private BiddingRound(Guid id, int roundNumber)
        {
            Id = id;
            RoundNumber = roundNumber;

            _plays = [];
        }

        public void Pass(string player)
        {
            var order = Plays.Select(x => x.Order).DefaultIfEmpty(-1).Max() + 1;
            var play = Play.Pass(player, order);

            MakePlay(play);

            var highestBid = Plays.HighestBid(player);
            var finishingPosition = BiddingPhase.PlayerCycle.Players.Count - Plays.PlayersWhoPassed().Count();

            RaiseEvent(new PlayerFinishedBidding
            {
                Player = player,
                BidAmount = highestBid ?? 0,
                BiddingRoundId = Id,
                FinishingPosition = finishingPosition,
            });

            var remainingPlayers = BiddingPhase.PlayerCycle.Players.Values
                .Except(Plays.PlayersWhoPassed())
                .ToList();

            if (remainingPlayers.Count == 1)
            {
                var winner = remainingPlayers.Single();
                var winningBid = Plays.HighestBid(winner);

                RaiseEvent(new PlayerFinishedBidding
                {
                    Player = winner,
                    BidAmount = winningBid ?? 0,
                    FinishingPosition = 0,
                    BiddingRoundId = Id
                });
            }
        }

        public void Bid(string player, int amount)
        {
            var order = Plays.Select(x => x.Order).DefaultIfEmpty(-1).Max() + 1;
            var play = Play.Bid(player, order, amount);

            MakePlay(play);
        }

        private void MakePlay(Play play)
        {
            if (HasFinished)
            {
                throw new InvalidOperationException("Round has finished");
            }

            if (BiddingPhase.PlayerCycle.CurrentPlayer != play.Player)
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
                BiddingPhase.PlayerCycle.Next();

                while (Plays.PlayersWhoPassed().Contains(BiddingPhase.PlayerCycle.CurrentPlayer))
                {
                    BiddingPhase.PlayerCycle.Next();
                }
            }
            else
            {
                RaiseEvent(new BiddingRoundComplete
                {
                    BiddingRoundId = Id
                });
            }
        }
    }
}
