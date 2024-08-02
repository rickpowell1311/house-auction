using HouseAuction.Lobby;
using HouseAuction.Lobby.Domain;
using Microsoft.AspNetCore.SignalR;

namespace HouseAuction
{
    public class HouseAuctionHub(LobbyContext context) : Hub<IHouseAuctionReceiver>, IHouseAuctionHub
    {
        public const string Route = "/house-auction";

        private readonly LobbyContext _context = context;

        public async Task<string> CreateLobby(string name)
        {
            var lobby = Lobby.Domain.Lobby.Create(name, Context.ConnectionId);

            _context.Lobbies.Add(lobby);
            await _context.SaveChangesAsync();

            await Groups.AddToGroupAsync(Context.ConnectionId, lobby.GameId);

            return lobby.GameId;
        }

        public async Task<List<string>> FetchLobby(string gameId)
        {
            var lobby = await _context.Lobbies.FindAsync(gameId);

            if (lobby == null || !lobby.Gamers.Any(x => x.ConnectionId == Context.ConnectionId))
            {
                throw new HubException($"Game with Id {gameId} not found");
            }

            return lobby.Gamers.Select(x => x.Name).ToList();
        }

        public async Task JoinLobby(string gameId, string name)
        {
            var lobby = await _context.Lobbies.FindAsync(gameId);

            if (lobby == null)
            {
                throw new HubException($"Game with Id {gameId} not found");
            }

            var lobbyJoinResult = lobby.Join(name, Context.ConnectionId);

            switch (lobbyJoinResult.Type)
            {
                case LobbyJoinResult.JoinResultType.Error:
                    throw new HubException(lobbyJoinResult.ErrorMessage);
                case LobbyJoinResult.JoinResultType.Reconnection:
                    // TODO:
                    throw new NotImplementedException("Reconnection not implemented yet");
                default:
                    break;
            }

            await _context.SaveChangesAsync();

            await Groups.AddToGroupAsync(Context.ConnectionId, lobby.GameId);
            await Clients.Group(lobby.GameId).OnLobbyMembersChanged(lobby.Gamers.Select(x => x.Name).ToList());
        }

        public async Task ReadyUp(string gameId, string name)
        {
            var lobby = await _context.Lobbies.FindAsync(gameId);

            if (lobby == null)
            {
                throw new HubException($"Game with Id {gameId} not found");
            }

            if (!lobby.TryReadyUp(name, out var error))
            {
                throw new HubException(error);
            }

            await _context.SaveChangesAsync();

            if (lobby.HasGameStarted)
            {
                await Clients.Group(lobby.GameId).OnGameBegun(lobby.GameId);
            }
        }
    }
}
