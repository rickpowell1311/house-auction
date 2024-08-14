using HouseAuction.Bidding.Reactions;

namespace HouseAuction.Bidding
{
    public interface IBiddingReceiver
    {
        Task OnPlayerTurnFinished(OnPlayerTurnFinished reaction);
    }
}
