using HouseAuction.Lobby;
using HouseAuction.Lobby.Requests;
using Microsoft.AspNetCore.SignalR;

namespace HouseAuction
{
    public class HouseAuctionHub(ILobbyHub lobbyHub) : Hub<IHouseAuctionReceiver>, IHouseAuctionHub
    {
        public const string Route = "/house-auction";

        private readonly ILobbyHub _lobbyHub = lobbyHub;

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

        public async Task ReadyUp(ReadyUp.ReadyUpRequest request)
        {
            await _lobbyHub.ReadyUp(request);
        }

        public async Task StartGame(StartGame.Request request)
        {
            await _lobbyHub.StartGame(request);
        }
    }
}
