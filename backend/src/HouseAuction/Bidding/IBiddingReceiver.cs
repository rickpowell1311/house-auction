using HouseAuction.Bidding.Reactions;

namespace HouseAuction.Bidding
{
    public interface IBiddingReceiver
    {
        Task OnPlayerTurnComplete(OnPlayerTurnComplete reaction);

        Task OnBiddingRoundComplete(OnBiddingRoundComplete reaction);
    }
}
