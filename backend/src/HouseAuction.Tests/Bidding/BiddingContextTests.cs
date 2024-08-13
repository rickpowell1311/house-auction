using HouseAuction.Tests._Shared.TestData;
using HouseAuction.Bidding.Domain;
using HouseAuction.Bidding;
using HouseAuction.Tests._Shared;

namespace HouseAuction.Tests.Bidding
{
    [Collection(HouseAuctionServicesFixture.CollectionName)]
    public class BiddingContextTests(HouseAuctionServicesFixture.Provider servicesFixtureProvider)
    {
        private readonly HouseAuctionServicesFixture.Provider _servicesFixtureProvider = servicesFixtureProvider;

        [Fact]
        public async Task CanCreateAndRehydrateBiddingPhase()
        {
            using var context = Context();
            await CreateBiddingPhase(context);
        }

        [Fact]
        public async Task CanCreateAndRehyrdrateBiddingRound()
        {
            using var context = Context();
            var biddingPhase = await CreateBiddingPhase(context);

            Assert.NotEmpty(biddingPhase.BiddingRounds);

            var biddingRound = await GetFirstBiddingRound(context, biddingPhase);

            Assert.NotNull(biddingRound);
        }

        [Fact]
        public async Task CanCreateAndRehydratePlay()
        {
            using var context = Context();
            var biddingPhase = await CreateBiddingPhase(context);
            var biddingRound = await GetFirstBiddingRound(context, biddingPhase);

            var player = biddingPhase.PlayerCycle.CurrentPlayer;
            biddingRound.Bid(player, 1);

            await context.SaveChangesAsync();

            await context.Entry(biddingRound).ReloadAsync();
            Assert.Single(biddingRound.Plays);
        }

        [Fact]
        public async Task CanCreateAndRehydrateHand()
        {
            using var context = Context();
            var biddingPhase = await CreateBiddingPhase(context);
            var biddingRound = await GetFirstBiddingRound(context, biddingPhase);

            var player = biddingPhase.PlayerCycle.CurrentPlayer;
            var hand = await context.Hands.FindAsync([biddingPhase.Hands.First().BiddingPhaseId, biddingPhase.Hands.First().Player]);
            hand.BuyProperty(1, 1, false);
            await context.SaveChangesAsync();

            using var rehydrationContext = Context();
            var rehydratedHand = await rehydrationContext.Hands.FindAsync([hand.BiddingPhaseId, hand.Player]);

            Assert.NotNull(rehydratedHand);
            Assert.NotEmpty(rehydratedHand.Properties);
            Assert.Equal(hand.Coins, rehydratedHand.Coins);
        }

        private async Task<BiddingPhase> CreateBiddingPhase(
            BiddingContext context)
        {
            var biddingPhase = new BiddingPhase(
                GameId.Generate(),
                Gamers.Sample.Take(3).ToList());

            context.BiddingPhases.Add(biddingPhase);

            await context.SaveChangesAsync();

            using var rehydrationContext = Context();
            var rehydratedBiddingPhase = await rehydrationContext.BiddingPhases
                .FindAsync(biddingPhase.GameId);

            Assert.NotNull(rehydratedBiddingPhase.PlayerCycle);
            Assert.NotNull(rehydratedBiddingPhase.Deck);
            Assert.NotEmpty(rehydratedBiddingPhase.Deck.Properties);
            Assert.NotEmpty(rehydratedBiddingPhase.BiddingRounds);
            Assert.NotEmpty(rehydratedBiddingPhase.Hands);

            return biddingPhase;
        }

        private async Task<BiddingRound> GetFirstBiddingRound(BiddingContext context, BiddingPhase biddingPhase)
        {
            var biddingRound = await context.BiddingRounds.FindAsync(biddingPhase.BiddingRounds.First().Id);

            Assert.NotNull(biddingRound);

            return biddingRound;
        }

        private BiddingContext Context()
        {
            return _servicesFixtureProvider.GetService<BiddingContext>();
        }
    }
}
