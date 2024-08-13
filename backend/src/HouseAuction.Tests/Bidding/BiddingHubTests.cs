using HouseAuction.Bidding.Domain;
using HouseAuction.Bidding;
using HouseAuction.Tests._Shared;
using HouseAuction.Tests._Shared.TestData;
using HouseAuction.Infrastructure.Identity;

namespace HouseAuction.Tests.Bidding
{
    [Collection(HouseAuctionServicesFixture.CollectionName)]
    public class BiddingHubTests(HouseAuctionServicesFixture.Provider servicesFixtureProvider)
    {
        private readonly HouseAuctionServicesFixture.Provider _servicesFixtureProvider = servicesFixtureProvider;

        [Fact]
        public async Task CanFetchBiddingPhase()
        {
            using var context = Context();
            var biddingPhase = await CreateBiddingPhase(context);

            var userContext = new UserContext
            {
                ConnectionId = Guid.NewGuid().ToString(),
                Games = new List<UserContext.Game>
                {
                    new UserContext.Game
                    {
                        GameId = biddingPhase.GameId,
                        Player = biddingPhase.PlayerCycle.CurrentPlayer,
                        PlayerGroupName = $"{biddingPhase.GameId}-{biddingPhase.PlayerCycle.CurrentPlayer}"
                    }
                }
            };

            var biddingHub = new BiddingHub(userContext, Context());

            var result = await biddingHub.GetBiddingPhase(new HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseRequest
            {
                GameId = biddingPhase.GameId
            });
        }

        private async Task<BiddingPhase> CreateBiddingPhase(
            BiddingContext context)
        {
            var biddingPhase = new BiddingPhase(
                GameId.Generate(),
                Gamers.Sample.Take(3).ToList());

            context.BiddingPhases.Add(biddingPhase);

            await context.SaveChangesAsync();

            return biddingPhase;
        }

        private BiddingContext Context()
        {
            return _servicesFixtureProvider.GetService<BiddingContext>();
        }
    }
}
