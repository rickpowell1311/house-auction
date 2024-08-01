using HouseAuction.Lobby.Domain;
using Microsoft.AspNetCore.SignalR;

namespace HouseAuction.Lobby
{
    public class LobbyHub(LobbyContext context) : Hub<ILobbyClient>, ILobbyHub
    {
        public const string Route = "/lobby";

        private readonly LobbyContext _context = context;

        public async Task CreateLobby(string name)
        {
            var lobby = Domain.Lobby.Create(name);

            _context.Lobbies.Add(lobby);
            await _context.SaveChangesAsync();

            await Groups.AddToGroupAsync(name, lobby.GameId);

            await Clients.Caller.OnLobbyCreated(lobby.GameId);
        }

        public async Task JoinLobby(string gameId, string name)
        {
            var lobby = await _context.Lobbies.FindAsync(gameId);

            if (lobby == null)
            {
                throw new HubException($"Game with Id {gameId} not found");
            }

            var lobbyJoinResult = lobby.Join(name);

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

            await Groups.AddToGroupAsync(name, lobby.GameId);
            await Clients.Group(lobby.GameId).OnLobbyMembersChanged(lobby.Gamers.Select(x => x.Name).ToList());
        }

        public async Task BeginGame(string gameId)
        {
            var lobby = await _context.Lobbies.FindAsync(gameId);

            if (lobby == null)
            {
                throw new HubException($"Game with Id {gameId} not found");
            }

            if (!lobby.TryBeginGame(out var error))
            {
               throw new HubException(error);
            }

            await _context.SaveChangesAsync();

            await Clients.Group(lobby.GameId).OnGameBegun(gameId);
        }
    }
}
