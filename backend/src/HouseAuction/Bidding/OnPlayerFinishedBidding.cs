using HouseAuction.Bidding.Domain.Events;
using Onwrd.EntityFrameworkCore;

namespace HouseAuction.Bidding
{
    public class OnPlayerFinishedBidding : IOnwardProcessor<PlayerFinishedBidding>
    {
        private readonly BiddingContext _context;

        public OnPlayerFinishedBidding(BiddingContext context)
        {
            _context = context;
        }

        public async Task Process(PlayerFinishedBidding @event, EventMetadata eventMetadata, CancellationToken cancellationToken = default)
        {
            var biddingRound = await _context.BiddingRounds.FindAsync([@event.BiddingRoundId], cancellationToken)
                ?? throw new ArgumentException($"Bidding round '{@event.BiddingRoundId}' not found");

            try
            {
                var hand = await _context.Hands.FindAsync([biddingRound.BiddingPhase.GameId, @event.Player], cancellationToken)
                ?? throw new ArgumentException($"Hand not found for player {@event.Player} in game {biddingRound.BiddingPhase.GameId}");

                var properties = biddingRound.BiddingPhase.Deck
                    .ForRound(biddingRound.RoundNumber)
                    .OrderByDescending(x => x)
                    .ToList();

                var boughtProperty = properties[@event.FinishingPosition];

                hand.BuyProperty(boughtProperty, @event.BidAmount, @event.FinishingPosition == 0);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
