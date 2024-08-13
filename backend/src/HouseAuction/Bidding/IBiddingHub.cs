using HouseAuction.Bidding.Requests;

namespace HouseAuction.Bidding
{
    public interface IBiddingHub
    {
        Task<GetBiddingPhase.GetBiddingPhaseResponse> GetBiddingPhase(GetBiddingPhase.GetBiddingPhaseRequest request);
    }
}
