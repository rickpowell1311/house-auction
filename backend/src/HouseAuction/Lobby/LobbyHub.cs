
using Google.Api;
using HouseAuction.Lobby.Domain;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace HouseAuction.Lobby
{
    public class LobbyHub : ILobbyHub
    {
        private readonly CallingHubContext _callingHubContext;
        private readonly LobbyContext _context;

        public LobbyHub(CallingHubContext callingHubContext, LobbyContext context)
        {
            _callingHubContext = callingHubContext;
            _context = context;
        }

        public async Task<string> CreateLobby(string name)
        {
            var lobby = Lobby.Domain.Lobby.Create(
                name, 
                _callingHubContext.Hub.Context.ConnectionId);

            _context.Lobbies.Add(lobby);
            await _context.SaveChangesAsync();

            await _callingHubContext.Hub.Groups.AddToGroupAsync(
                _callingHubContext.Hub.Context.ConnectionId, 
                lobby.GameId);

            return lobby.GameId;
        }

        public async Task<List<string>> FetchLobby(string gameId)
        {
            var lobby = await _context.Lobbies.FindAsync(gameId);

            if (lobby == null || !lobby.Gamers.Any(x => x.ConnectionId == _callingHubContext.Hub.Context.ConnectionId))
            {
                throw new HubException($"Game with Id {gameId} not found");
            }

            return lobby.Gamers.Select(x => x.Name).ToList();
        }

        public async Task<string> GetMyName(string gameId)
        {
            var lobby = await _context.Lobbies.FindAsync(gameId)
                ?? throw new HubException($"Game with Id {gameId} not found");

            var gamer = lobby.Gamers.FirstOrDefault(x => x.ConnectionId == _callingHubContext.Hub.Context.ConnectionId);

            return gamer?.Name;
        }

        public async Task JoinLobby(string gameId, string name)
        {
            var lobby = await _context.Lobbies.FindAsync(gameId);

            if (lobby == null)
            {
                throw new HubException($"Game with Id {gameId} not found");
            }

            var lobbyJoinResult = lobby.Join(name, _callingHubContext.Hub.Context.ConnectionId);

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

            await _callingHubContext.Hub.Groups.AddToGroupAsync(
                _callingHubContext.Hub.Context.ConnectionId, 
                lobby.GameId);

            await _callingHubContext.Hub
                .Clients
                .Group(lobby.GameId)
                .AsReceiver<ILobbyReceiver>()
                .OnLobbyMembersChanged(lobby.Gamers.Select(x => x.Name).ToList());
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
                await _callingHubContext.Hub
                    .Clients
                    .Group(lobby.GameId)
                    .AsReceiver<ILobbyReceiver>()
                    .OnGameBegun(lobby.GameId);
            }
        }
    }
}
