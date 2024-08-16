using HouseAuction.Bidding;
using HouseAuction.Lobby;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HouseAuction.Infrastructure.Identity
{
    public class UserContextFilter : IHubFilter
    {
        private readonly UserContext _userContext;
        private readonly LobbyContext _lobbyContext;
        private readonly BiddingContext _biddingContext;

        public UserContextFilter(
            UserContext userContext, 
            LobbyContext lobbyContext, 
            BiddingContext biddingContext)
        {
            _userContext = userContext;
            _lobbyContext = lobbyContext;
            _biddingContext = biddingContext;
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
                            Player = gamer.Name
                        };
                    })
                    .ToList();
            }
            
            return await next(invocationContext);
        }
    }
}
