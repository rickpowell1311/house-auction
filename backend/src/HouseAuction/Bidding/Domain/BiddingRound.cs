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
            RoundNumber = roundNumber;
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
            BuyNextAvailableProperty(player);
            HandleEndOfTurn();
        }

        public void Bid(string player, int amount)
        {
            var order = Plays.Select(x => x.Order).DefaultIfEmpty(-1).Max() + 1;
            var play = Play.Bid(player, order, amount);

            MakePlay(play);
            HandleEndOfTurn();
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
        }

        private void HandleEndOfTurn()
        {
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
                var winner = BiddingPhase.PlayerCycle.Players.Values
                    .Except(Plays.PlayersWhoPassed())
                    .Single();

                BuyNextAvailableProperty(winner);

                BiddingPhase.PlayerCycle.SetCurrentPlayer(winner);

                RaiseEvent(new BiddingRoundComplete
                {
                    BiddingRoundId = Id
                });
            }
        }

        private void BuyNextAvailableProperty(string player)
        {
            var highestBid = Plays.HighestBid(player);
            var finishingPosition =
                BiddingPhase.PlayerCycle.Players.Count - Plays.PlayersWhoPassed().Count();

            var propertyToPurchase = BiddingPhase.Deck
                .ForRound(RoundNumber)
                .OrderByDescending(x => x)
                .ToList()
                [finishingPosition];

            var hand = BiddingPhase.Hands.Single(x => x.Player == player);
            hand.BuyProperty(propertyToPurchase, highestBid ?? 0, finishingPosition == 0);
        }
    }
}
