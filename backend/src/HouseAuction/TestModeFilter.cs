using HouseAuction.Bidding;
using HouseAuction.Bidding.Requests;
using HouseAuction.Infrastructure.Identity;
using HouseAuction.Lobby;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace HouseAuction
{
    public class TestModeFilter : IHubFilter
    {
        private readonly UserContext _userContext;
        private readonly LobbyContext _lobbyContext;
        private readonly BiddingContext _biddingContext;
        private readonly AutoPlay _autoPlay;
        private readonly TestMode _testModeOptions;

        public TestModeFilter(
            UserContext userContext,
            LobbyContext lobbyContext,
            BiddingContext biddingContext,
            AutoPlay autoPlay,
            IOptions<TestMode> testModeOptions)
        {
            _userContext = userContext;
            _lobbyContext = lobbyContext;
            _biddingContext = biddingContext;
            _autoPlay = autoPlay;
            _testModeOptions = testModeOptions.Value;
        }

        public async ValueTask<object> InvokeMethodAsync(
            HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            if (!_testModeOptions.Enabled)
            {
                return await next(invocationContext);
            }

            _userContext.ConnectionId = invocationContext.Context.ConnectionId;

            var biddingPhase = await _biddingContext.BiddingPhases
                .FindAsync("123");

            var userContextGame = new UserContext.Game
            {
                GameId = "123",
                Player = biddingPhase.PlayerCycle.CurrentPlayer
            };

            _userContext.Games.Add(userContextGame);

            await invocationContext.Hub.Groups.AddToGroupAsync(
                invocationContext.Context.ConnectionId,
                userContextGame.GameId);

            await invocationContext.Hub.Groups.AddPlayerAsIndividualGroup(
                invocationContext.Context.ConnectionId,
                userContextGame.GameId,
                userContextGame.Player);

            var result = await next(invocationContext);

            if (invocationContext.HubMethodName == nameof(IHouseAuctionHub.Bid))
            {
                var request = (Bid.BidRequest)invocationContext.HubMethodArguments[0];
                await _autoPlay.Run(_userContext, request.GameId);
            }

            if (invocationContext.HubMethodName == nameof(IHouseAuctionHub.Pass))
            {
                var request = (Pass.PassRequest)invocationContext.HubMethodArguments[0];
                await _autoPlay.Run(_userContext, request.GameId);
            }

            return result;
        }
    }
}
