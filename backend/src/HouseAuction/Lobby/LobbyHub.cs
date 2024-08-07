
using HouseAuction.Lobby.Domain;
using HouseAuction.Lobby.Reactions;
using HouseAuction.Lobby.Requests;
using Microsoft.AspNetCore.SignalR;

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
            var lobby = Domain.Lobby.Create(
                request.Name, 
                _callingHubContext.Hub.Context.ConnectionId);

            _context.Lobbies.Add(lobby);
            await _context.SaveChangesAsync();

            await _callingHubContext.Hub.Groups.AddToGroupAsync(
                _callingHubContext.Hub.Context.ConnectionId, 
                lobby.GameId);

            await _callingHubContext.Hub.Groups.AddToGroupAsync(
                _callingHubContext.Hub.Context.ConnectionId,
                request.Name);

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
                Gamers = lobby.Gamers.Select(x => new FetchLobby.FetchLobbyResponseGamer
                {
                    Name = x.Name,
                    IsMe = x.ConnectionId == _callingHubContext.Hub.Context.ConnectionId,
                    IsReady = x.IsReady
                }).ToList()
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

            await _callingHubContext.Hub.Groups.AddToGroupAsync(
                _callingHubContext.Hub.Context.ConnectionId,
                request.Name);

            await NotifyMembersChanged(lobby);
        }

        public async Task ReadyUp(ReadyUp.ReadyUpRequest request)
        {
            var lobby = await _context.Lobbies.FindAsync(request.GameId);

            if (lobby == null)
            {
                throw new HubException($"Game with Id {request.GameId} not found");
            }

            var gameIsReady = lobby.IsReadyToStartGame;

            if (!lobby.TryReadyUp(request.Name, out var error))
            {
                throw new HubException(error);
            }

            await _context.SaveChangesAsync();

            await NotifyMembersChanged(lobby);

            if (gameIsReady != lobby.IsReadyToStartGame)
            {
                await _callingHubContext.Hub
                    .Clients
                    .Group(lobby.GameId)
                    .AsReceiver<ILobbyReceiver>()
                    .OnGameReadinessChanged(new OnGameReadinessChanged.OnGameReadinessChangedReaction
                    {
                        GameId = lobby.GameId,
                        IsReadyToStart = lobby.IsReadyToStartGame
                    });
            }
        }

        private async Task NotifyMembersChanged(Domain.Lobby lobby)
        {
            foreach (var gamer in lobby.Gamers)
            {
                var reaction = new OnLobbyMembersChanged.OnLobbyMembersChangedReaction
                {
                    Gamers = lobby.Gamers.Select(x => new OnLobbyMembersChanged.OnLobbyMembersChangedReactionGamer
                    {
                        IsMe = x.Name == gamer.Name,
                        IsReady = x.IsReady,
                        Name = x.Name
                    }).ToList()
                };

                await _callingHubContext.Hub.Clients
                    .Group(gamer.Name)
                    .AsReceiver<ILobbyReceiver>()
                    .OnLobbyMembersChanged(reaction);
            }
        }
    }
}
