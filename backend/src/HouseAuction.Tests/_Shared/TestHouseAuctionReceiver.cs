using HouseAuction.Bidding.Reactions;
using HouseAuction.Lobby.Reactions;
using HouseAuction.Tests.Bidding._Shared;
using HouseAuction.Tests.Lobby._Shared;

namespace HouseAuction.Tests._Shared
{
    public class TestHouseAuctionReceiver : IHouseAuctionReceiver
    {
        public TestLobbyReceiver TestLobbyReceiver { get; }
        public TestBiddingReceiver TestBiddingReceiver { get; }

        public TestHouseAuctionReceiver()
        {
            TestLobbyReceiver = new TestLobbyReceiver();
            TestBiddingReceiver = new TestBiddingReceiver();
        }

        public Task NotifyError(string message)
        {
            return Task.CompletedTask;
        }

        public async Task OnGameReadinessChanged(OnGameReadinessChanged.OnGameReadinessChangedReaction reaction)
        {
            await TestLobbyReceiver.OnGameReadinessChanged(reaction);
        }

        public async Task OnGameStarted(OnGameStarted.OnGameStartedReaction reaction)
        {
            await TestLobbyReceiver.OnGameStarted(reaction);
        }

        public async Task OnLobbyMembersChanged(
            OnLobbyMembersChanged.OnLobbyMembersChangedReaction reaction)
        {
            await TestLobbyReceiver.OnLobbyMembersChanged(reaction);
        }

        public async Task OnPlayerTurnComplete(OnPlayerTurnComplete reaction)
        {
            await TestBiddingReceiver.OnPlayerTurnComplete(reaction);
        }

        public async Task OnBiddingRoundComplete(OnBiddingRoundComplete reaction)
        {
            await TestBiddingReceiver.OnBiddingRoundComplete(reaction);
        }
    }
}
