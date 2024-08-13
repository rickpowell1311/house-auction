using HouseAuction.Bidding;
using HouseAuction.Lobby;
using TypedSignalR.Client;

namespace HouseAuction
{
    [Receiver]
    public interface IHouseAuctionReceiver : ILobbyReceiver, IBiddingReceiver
    {
        Task NotifyError(string message);
    }
}
