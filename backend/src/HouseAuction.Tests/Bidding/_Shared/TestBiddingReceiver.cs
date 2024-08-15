using HouseAuction.Bidding;
using HouseAuction.Bidding.Reactions;

namespace HouseAuction.Tests.Bidding._Shared
{
    public class TestBiddingReceiver : IBiddingReceiver
    {
        public OnPlayerTurnComplete LatestPlayerTurnFinished { get; private set; }

        public Task OnPlayerTurnComplete(OnPlayerTurnComplete reaction)
        {
            LatestPlayerTurnFinished = reaction;

            return Task.CompletedTask;
        }
    }
}
