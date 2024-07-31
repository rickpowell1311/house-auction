using HouseAuction.Lobby;
using HouseAuction.Tests._Shared.TestData;
using HouseAuction.Tests.Lobby._Shared;

namespace HouseAuction.Tests.Lobby
{
    [Collection(HouseAuctionServicesFixture.CollectionName)]
    public class LobbyContextTests(HouseAuctionServicesFixture.Provider servicesFixtureProvider)
    {
        private readonly HouseAuctionServicesFixture.Provider _servicesFixtureProvider = servicesFixtureProvider;

        [Fact]
        public async Task CanCreateAndRehydrateLobby()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0]);

            var context = _servicesFixtureProvider.GetService<LobbyContext>();
            context.Lobbies.Add(lobby);

            await context.SaveChangesAsync();

            var rehydratedLobby = await context.Lobbies.FindAsync(lobby.GameId);

            Assert.NotNull(rehydratedLobby);
        }
    }
}
