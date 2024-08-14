using HouseAuction.Lobby;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HouseAuction.Infrastructure.Identity
{
    public class UserContextFilter : IHubFilter
    {
        private readonly UserContext _userContext;
        private readonly LobbyContext _lobbyContext;
        private readonly TestMode _testModeOptions;

        public UserContextFilter(UserContext userContext, LobbyContext lobbyContext, IOptions<TestMode> testModeOptions)
        {
            _userContext = userContext;
            _lobbyContext = lobbyContext;
            _testModeOptions = testModeOptions.Value;
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

            if (_testModeOptions.Enabled)
            {
                _userContext.Games.Add(new UserContext.Game
                {
                    GameId = "123",
                    Player = "Alice",
                    PlayerGroupName = $"123-Alice"
                });
            }
            
            return await next(invocationContext);
        }
    }
}
