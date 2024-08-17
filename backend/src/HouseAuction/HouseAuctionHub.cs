using HouseAuction.Bidding;
using HouseAuction.Bidding.Requests;
using HouseAuction.Lobby;
using HouseAuction.Lobby.Requests;
using Microsoft.AspNetCore.SignalR;

namespace HouseAuction
{
    public class HouseAuctionHub(ILobbyHub lobbyHub, IBiddingHub biddingHub) : Hub<IHouseAuctionReceiver>, IHouseAuctionHub
    {
        public const string Route = "/house-auction";

        private readonly ILobbyHub _lobbyHub = lobbyHub;
        private readonly IBiddingHub _biddingHub = biddingHub;

        public async Task OnDisconnectedAsync()
        {
            await _lobbyHub.OnDisconnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await OnDisconnectedAsync();
        }

        public async Task<CreateLobby.CreateLobbyResponse> CreateLobby(CreateLobby.CreateLobbyRequest request)
        {
            return await _lobbyHub.CreateLobby(request);
        }

        public async Task<FetchLobby.FetchLobbyResponse> FetchLobby(FetchLobby.FetchLobbyRequest request)
        {
            return await _lobbyHub.FetchLobby(request);
        }

        public async Task JoinLobby(JoinLobby.JoinLobbyRequest request)
        {
            await _lobbyHub.JoinLobby(request);
        }

        public async Task<GetDisconnectedPlayers.GetDisconnectedPlayersResponse> GetDisconnectedPlayers(
            GetDisconnectedPlayers.GetDisconnectedPlayersRequest request)
        {
            return await _lobbyHub.GetDisconnectedPlayers(request);
        }

        public async Task Reconnect(Reconnect.ReconnectRequest request)
        {
            await _lobbyHub.Reconnect(request);
        }

        public async Task ReadyUp(ReadyUp.ReadyUpRequest request)
        {
            await _lobbyHub.ReadyUp(request);
        }

        public async Task StartGame(StartGame.StartGameRequest request)
        {
            await _lobbyHub.StartGame(request);
        }

        public async Task<GetBiddingPhase.GetBiddingPhaseResponse> GetBiddingPhase(GetBiddingPhase.GetBiddingPhaseRequest request)
        {
            return await _biddingHub.GetBiddingPhase(request);
        }

        public async Task Bid(Bid.BidRequest request)
        {
            await _biddingHub.Bid(request);
        }

        public async Task Pass(Pass.PassRequest request)
        {
            await _biddingHub.Pass(request);
        }
    }
}
