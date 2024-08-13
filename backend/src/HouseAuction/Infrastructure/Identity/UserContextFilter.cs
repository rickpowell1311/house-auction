using HouseAuction.Infrastructure.HubContext;
using HouseAuction.Lobby;
using HouseAuction.Messages.Lobby;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HouseAuction.Infrastructure.Identity
{
    public class UserContextFilter : IHubFilter
    {
        private readonly UserContext _userContext;
        private readonly LobbyContext _lobbyContext;

        public UserContextFilter(UserContext userContext, LobbyContext lobbyContext)
        {
            _userContext = userContext;
            _lobbyContext = lobbyContext;
        }

        public async ValueTask<object> InvokeMethodAsync(
            HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            _userContext.ConnectionId = invocationContext.Context.ConnectionId;

            var lobbies = await _lobbyContext.Lobbies
                .Where(x => x.Gamers.Any(g => g.ConnectionId == _userContext.ConnectionId))
                .ToListAsync();

            if (lobbies.Any())
            {
                _userContext.Games = lobbies
                    .Select(x =>
                    {
                        var gamer = x.Gamers.FirstOrDefault(x => x.ConnectionId == invocationContext.Context.ConnectionId);

                        return new UserContext.Game
                        {
                            GameId = x.GameId,
                            Player = gamer.Name,
                            PlayerGroupName = gamer.GroupName
                        };
                    })
                    .ToList();
            }

            return await next(invocationContext);
        }
    }
}
