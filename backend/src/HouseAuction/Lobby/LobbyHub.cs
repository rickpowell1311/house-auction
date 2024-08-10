
using HouseAuction.Lobby.Domain;
using HouseAuction.Lobby.Reactions;
using HouseAuction.Lobby.Requests;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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

            var gamer = lobby.Gamers.Single(x => x.Name == request.Name);

            await _callingHubContext.Hub.Groups.AddToGroupAsync(
                _callingHubContext.Hub.Context.ConnectionId,
                gamer.GroupName);

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
                    IsCreator = lobby.Creator.Name == x.Name,
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

            var gamer = lobby.Gamers.Single(x => x.Name == request.Name);

            await _callingHubContext.Hub.Groups.AddToGroupAsync(
                _callingHubContext.Hub.Context.ConnectionId,
                gamer.GroupName);

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

            if (!lobby.TryReadyUp(_callingHubContext.Hub.Context.ConnectionId, out var error))
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
                        IsReadyToStart = lobby.IsReadyToStartGame
                    });
            }
        }

        public async Task StartGame(StartGame.StartGameRequest request)
        {
            var lobby = await _context.Lobbies.FindAsync(request.GameId);

            if (lobby == null)
            {
                throw new HubException($"Game with Id {request.GameId} not found");
            }

            if (!lobby.TryBeginGame(_callingHubContext.Hub.Context.ConnectionId, out var error))
            {
                throw new HubException(error);
            }

            await _context.SaveChangesAsync();

            await _callingHubContext.Hub.Clients
                .Group(lobby.GameId)
                .AsReceiver<ILobbyReceiver>()
                .OnGameStarted(new OnGameStarted.OnGameStartedReaction());
        }

        public async Task OnDisconnectedAsync()
        {
            var lobbies = await _context.Lobbies
                .Where(x => x.Gamers
                    .Any(y => y.ConnectionId == _callingHubContext.Hub.Context.ConnectionId))
                .ToListAsync();

            foreach (var lobby in lobbies)
            {
                lobby.Disconnect(_callingHubContext.Hub.Context.ConnectionId);
            }

            await _context.SaveChangesAsync();

            foreach (var lobby in lobbies)
            {
                await NotifyMembersChanged(lobby);
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
                        IsCreator = lobby.Creator.Name == x.Name,
                        IsReady = x.IsReady,
                        Name = x.Name
                    }).ToList()
                };

                await _callingHubContext.Hub.Clients
                    .Group(gamer.GroupName)
                    .AsReceiver<ILobbyReceiver>()
                    .OnLobbyMembersChanged(reaction);
            }
        }
    }
}
