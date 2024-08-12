using HouseAuction.Lobby;
using HouseAuction.Lobby.Domain;
using HouseAuction.Tests._Shared.TestData;
using HouseAuction.Tests._Shared;

namespace HouseAuction.Tests.Lobby
{
    [Collection(HouseAuctionServicesFixture.CollectionName)]
    public class LobbyContextTests(HouseAuctionServicesFixture.Provider servicesFixtureProvider)
    {
        private readonly HouseAuctionServicesFixture.Provider _servicesFixtureProvider = servicesFixtureProvider;

        [Fact]
        public async Task CanCreateAndRehydrateLobby()
        {
            var lobby = HouseAuction.Lobby.Domain.Lobby.Create(Gamers.Sample[0], Guid.NewGuid().ToString());

            var context = _servicesFixtureProvider.GetService<LobbyContext>();
            context.Lobbies.Add(lobby);

            await context.SaveChangesAsync();

            var rehydratedLobby = await context.Lobbies.FindAsync(lobby.GameId);
            Assert.NotNull(rehydratedLobby);

            var joinResult = rehydratedLobby.Join(Gamers.Sample[1], Guid.NewGuid().ToString());
            Assert.Equal(LobbyJoinResult.JoinResultType.Success, joinResult.Type);
            await context.SaveChangesAsync();
        }
    }
}
