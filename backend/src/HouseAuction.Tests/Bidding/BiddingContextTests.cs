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

            Assert.Single(biddingPhase.BiddingRounds);

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
            biddingRound.MakePlay(Play.Bid(player, 1));

            await context.SaveChangesAsync();

            var rehydrated = await context.BiddingRounds.FindAsync(biddingRound.Id);
            Assert.Single(rehydrated.Plays);
        }

        private async Task<BiddingPhase> CreateBiddingPhase(
            BiddingContext context)
        {
            var biddingPhase = new BiddingPhase(
                GameId.Generate(),
                Gamers.Sample.Take(3).ToList());

            context.BiddingPhases.Add(biddingPhase);

            await context.SaveChangesAsync();

            var rehydratedBiddingPhase = await context.BiddingPhases.FindAsync(biddingPhase.GameId);
            Assert.NotNull(rehydratedBiddingPhase);
            Assert.NotNull(rehydratedBiddingPhase.PlayerCycle);
            Assert.NotNull(rehydratedBiddingPhase.Deck);
            Assert.NotEmpty(rehydratedBiddingPhase.BiddingRounds);

            return rehydratedBiddingPhase;
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
