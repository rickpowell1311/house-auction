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

            await Clients.Caller.OnLobbyCreated(lobby.GameId);
        }

        public async Task JoinLobby(string gameId, string name)
        {
            var lobby = await _context.Lobbies.FindAsync(gameId);

            if (lobby == null)
            {
                throw new HubException($"Game with Id {gameId} not found");
            }

            if (!lobby.TryJoin(name, out var error))
            {
                throw new HubException(error);
            }

            await _context.SaveChangesAsync();

            await Clients.All.OnGamerJoined(name);
        }
    }
}
