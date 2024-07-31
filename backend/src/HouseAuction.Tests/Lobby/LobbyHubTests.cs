using HouseAuction.Lobby;
using HouseAuction.Tests._Shared;
using HouseAuction.Tests._Shared.TestData;
using HouseAuction.Tests.Lobby._Shared;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;

namespace HouseAuction.Tests.Lobby
{
    [Collection(HubClientFixture.CollectionName)]
    public class LobbyHubTests(HubClientFixture.Provider hubClientFixtureProvider) : IAsyncLifetime
    {
        private readonly HubClientFixture.Provider _hubClientFixtureProvider = hubClientFixtureProvider;
        private HubConnection _connection;
        private ILobbyHub _hub;
        private TestLobbyClient _client;

        public async Task InitializeAsync()
        {
            _connection = await _hubClientFixtureProvider.StartHubConnection<LobbyHub>(LobbyHub.Route);
            _hub = _connection.CreateHubProxy<ILobbyHub>();
            _client = new TestLobbyClient();
            _connection.Register<ILobbyClient>(_client);
        }

        [Fact]
        public async Task CanCreateLobby()
        {
            await _hub.CreateLobby(Gamers.Sample[0]);

            Assert.Single(_client.LobbiesCreated);
        }

        public async Task DisposeAsync()
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
        }
    }
}
