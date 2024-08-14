using HouseAuction.Bidding;
using HouseAuction.Bidding.Reactions;

namespace HouseAuction.Tests.Bidding._Shared
{
    public class TestBiddingReceiver : IBiddingReceiver
    {
        public OnPlayerTurnFinished LatestPlayerTurnFinished { get; private set; }

        public Task OnPlayerTurnFinished(OnPlayerTurnFinished reaction)
        {
            LatestPlayerTurnFinished = reaction;

            return Task.CompletedTask;
        }
    }
}
