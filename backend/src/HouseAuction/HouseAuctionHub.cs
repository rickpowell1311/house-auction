using HouseAuction.Lobby;
using HouseAuction.Lobby.Domain;
using Microsoft.AspNetCore.SignalR;

namespace HouseAuction
{
    public class HouseAuctionHub(ILobbyHub lobbyHub) : Hub<IHouseAuctionReceiver>, IHouseAuctionHub
    {
        public const string Route = "/house-auction";

        private readonly ILobbyHub _lobbyHub = lobbyHub;

        public async Task<string> CreateLobby(string name)
        {
            return await _lobbyHub.CreateLobby(name);
        }

        public async Task<List<string>> FetchLobby(string gameId)
        {
            return await _lobbyHub.FetchLobby(gameId);
        }

        public async Task<string> GetMyName(string gameId)
        {
            return await _lobbyHub.GetMyName(gameId);
        }

        public async Task JoinLobby(string gameId, string name)
        {
            await _lobbyHub.JoinLobby(gameId, name);
        }

        public async Task ReadyUp(string gameId, string name)
        {
            await _lobbyHub.ReadyUp(gameId, name);
        }
    }
}
