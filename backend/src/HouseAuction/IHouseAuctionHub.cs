using HouseAuction.Bidding;
using HouseAuction.Lobby;
using TypedSignalR.Client;

namespace HouseAuction
{
    [Hub]
    public interface IHouseAuctionHub : ILobbyHub, IBiddingHub
    {
    }
}
