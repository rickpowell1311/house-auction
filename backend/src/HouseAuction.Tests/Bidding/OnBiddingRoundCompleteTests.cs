using HouseAuction.Bidding;
using HouseAuction.Bidding.Domain;
using HouseAuction.Tests._Shared;
using HouseAuction.Tests._Shared.TestData;

namespace HouseAuction.Tests.Bidding
{
    [Collection(HouseAuctionServicesFixture.CollectionName)]
    public class OnBiddingRoundCompleteTests(HouseAuctionServicesFixture.Provider servicesFixtureProvider)
    {
        private readonly HouseAuctionServicesFixture.Provider _servicesFixtureProvider = servicesFixtureProvider;

        [Fact]
        public async Task CompleteBiddingRound_HandsAreUpdatedAndBiddingRoundHasFinished()
        {
            using var context = Context();

            var biddingPhase = new BiddingPhase(
                GameId.Generate(),
                Gamers.Sample.Take(4).ToList());

            context.BiddingPhases.Add(biddingPhase);
            await context.SaveChangesAsync();

            var plays = new List<int?> { 2, 3, 5, 7, null, null, null };
            var biddingRound = await context.BiddingRounds.FindAsync(
                biddingPhase.BiddingRounds
                    .OrderBy(x => x.RoundNumber)
                    .First().Id);

            for (int i = 0; i < plays.Count; i++)
            {
                var player = biddingPhase.PlayerCycle.CurrentPlayer;
                var play = plays[i];

                if (play.HasValue)
                {
                    biddingRound.Bid(player, play.Value);
                }
                else
                {
                    biddingRound.Pass(player);
                }

                await context.SaveChangesAsync();
            }

            using var rehydrationContext = Context();

            var rehydratedBiddingPhase = await rehydrationContext.BiddingPhases
                .FindAsync(biddingPhase.GameId);

            Assert.NotNull(rehydratedBiddingPhase);
            Assert.Equal(
                rehydratedBiddingPhase.Hands.Count(), 
                rehydratedBiddingPhase.Hands.DistinctBy(x => x.Coins).Count());
            Assert.Single(rehydratedBiddingPhase.BiddingRounds
                .Where(x => x.HasFinished));
        }

        private BiddingContext Context()
        {
            return _servicesFixtureProvider.GetService<BiddingContext>();
        }
    }
}
