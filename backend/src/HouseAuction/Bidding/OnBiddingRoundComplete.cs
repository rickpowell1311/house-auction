using HouseAuction.Bidding.Domain.Events;
using Onwrd.EntityFrameworkCore;

namespace HouseAuction.Bidding
{
    public class OnBiddingRoundComplete : IOnwardProcessor<BiddingRoundComplete>
    {
        public Task Process(BiddingRoundComplete @event, EventMetadata eventMetadata, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
