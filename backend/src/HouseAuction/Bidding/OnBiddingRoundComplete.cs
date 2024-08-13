using HouseAuction.Bidding.Domain;
using HouseAuction.Bidding.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Onwrd.EntityFrameworkCore;

namespace HouseAuction.Bidding
{
    public class OnBiddingRoundComplete : IOnwardProcessor<BiddingRoundComplete>
    {
        private readonly BiddingContext _context;

        public OnBiddingRoundComplete(BiddingContext context)
        {
            _context = context;
        }

        public async Task Process(BiddingRoundComplete @event, EventMetadata eventMetadata, CancellationToken cancellationToken = default)
        {
            var biddingRound = await _context.BiddingRounds.FindAsync([@event.BiddingRoundId], cancellationToken)
                ?? throw new InvalidOperationException($"Bidding round '{@event.BiddingRoundId}' not found");

            var purchases = new List<(string Player, int Property, int Amount, bool IsBiddingRoundWinner)>();
            var properties = new Stack<int>(biddingRound.BiddingPhase.Deck
                .ForRound(biddingRound.BiddingPhase.CurrentRound.RoundNumber - 1)
                .OrderByDescending(x => x));

            var notPlayed = biddingRound.BiddingPhase.PlayerCycle.Players.Values
                .Where(x => !biddingRound.Plays.Select(y => y.Player).Distinct().Contains(x))
                .FirstOrDefault();

            if (notPlayed != null)
            {
                /*  Exceptional case. Everyone passed and one person didn't play at all.
                    *  Person who didn't play is the winner */
                purchases.Add((notPlayed, properties.Pop(), 0, true));
            }

            foreach (var play in biddingRound.Plays.OrderBy(x => x.Order))
            {
                if (purchases.Select(x => x.Player).Contains(play.Player))
                {
                    continue;
                }

                if (play.IsPass)
                {
                    var amount = biddingRound.Plays
                        .Where(x => x.Player == play.Player)
                        .Select(x => x.Amount ?? 0)
                        .Max();

                    purchases.Add((play.Player, properties.Pop(), amount, false));
                }
            }

            var winner = biddingRound.BiddingPhase.PlayerCycle.Players.Values
                .Single(x => !purchases.Select(y => y.Player).Contains(x));
            var winningAmount = biddingRound.Plays
                .Where(x => x.Player == winner)
                .Select(x => x.Amount ?? 0)
                .Max();

            purchases.Add((winner, properties.Pop(), winningAmount, true));

            var players = purchases.Select(x => x.Player).ToList();
            var hands = await _context.Hands
                .Where(x => players.Contains(x.Player) && x.BiddingPhaseId == biddingRound.BiddingPhase.GameId)
                .ToListAsync(cancellationToken);

            foreach (var hand in hands)
            {
                var (_, property, amount, isBiddingRoundWinner) = purchases.Find(x => x.Player == hand.Player);
                hand.BuyProperty(property, amount, isBiddingRoundWinner);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
