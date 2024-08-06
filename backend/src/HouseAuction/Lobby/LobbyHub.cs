
using HouseAuction.Lobby.Domain;
using HouseAuction.Lobby.Requests;
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

        public async Task<CreateLobby.CreateLobbyResponse> CreateLobby(CreateLobby.CreateLobbyRequest request)
        {
            var lobby = Lobby.Domain.Lobby.Create(
                request.Name, 
                _callingHubContext.Hub.Context.ConnectionId);

            _context.Lobbies.Add(lobby);
            await _context.SaveChangesAsync();

            await _callingHubContext.Hub.Groups.AddToGroupAsync(
                _callingHubContext.Hub.Context.ConnectionId, 
                lobby.GameId);

            return new CreateLobby.CreateLobbyResponse { GameId = lobby.GameId };
        }

        public async Task<FetchLobby.FetchLobbyResponse> FetchLobby(FetchLobby.FetchLobbyRequest request)
        {
            var lobby = await _context.Lobbies.FindAsync(request.GameId);

            if (lobby == null || !lobby.Gamers.Any(x => x.ConnectionId == _callingHubContext.Hub.Context.ConnectionId))
            {
                throw new HubException($"Game with Id {request.GameId} not found");
            }

            return new FetchLobby.FetchLobbyResponse
            {
                Gamers = lobby.Gamers.Select(x => x.Name).ToList()
            };
        }

        public async Task<GetMyName.GetMyNameResponse> GetMyName(GetMyName.GetMyNameRequest request)
        {
            var lobby = await _context.Lobbies.FindAsync(request.GameId)
                ?? throw new HubException($"Game with Id {request.GameId} not found");

            var gamer = lobby.Gamers
                .FirstOrDefault(x => x.ConnectionId == _callingHubContext.Hub.Context.ConnectionId);

            return new Requests.GetMyName.GetMyNameResponse
            {
                Name = gamer?.Name
            };
        }

        public async Task JoinLobby(JoinLobby.JoinLobbyRequest request)
        {
            var lobby = await _context.Lobbies.FindAsync(request.GameId);

            if (lobby == null)
            {
                throw new HubException($"Game with Id {request.GameId} not found");
            }

            var lobbyJoinResult = lobby.Join(request.Name, _callingHubContext.Hub.Context.ConnectionId);

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

        public async Task ReadyUp(ReadyUp.ReadyUpRequest request)
        {
            var lobby = await _context.Lobbies.FindAsync(request.GameId);

            if (lobby == null)
            {
                throw new HubException($"Game with Id {request.GameId} not found");
            }

            if (!lobby.TryReadyUp(request.Name, out var error))
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
