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
            _connection = await _hubClientFixtureProvider.StartHubConnection<HouseAuctionHub>(HouseAuctionHub.Route);
            _hub = _connection.CreateHubProxy<ILobbyHub>();
            _client = new TestLobbyClient();
            _connection.Register<ILobbyClient>(_client);
        }

        [Fact]
        public async Task CanCreateLobby()
        {
            await _hub.CreateLobby(Gamers.Sample[0]);

            await WaitFor.Condition(() => _client.LobbiesCreated.Count == 1);
            Assert.Single(_client.LobbiesCreated);
        }

        [Fact]
        public async Task CanJoinLobby()
        {
            var firstGamer = Gamers.Sample[0];
            var secondGamer = Gamers.Sample[1];

            await _hub.CreateLobby(firstGamer);

            await WaitFor.Condition(() => _client.LobbiesCreated.Count == 1);
            var gameId = _client.LobbiesCreated.Single();

            var secondClient = new TestLobbyClient();
            _connection.Register<ILobbyClient>(secondClient);

            await _hub.JoinLobby(gameId, secondGamer);

            await WaitFor.Condition(() => _client.GamersJoined.Count == 2 && secondClient.GamersJoined.Count == 2);
            Assert.Equal(2, _client.GamersJoined.Count);
            Assert.Equal(2, secondClient.GamersJoined.Count);
        }

        [Fact]
        public async Task CanStartGame()
        {
            var gamers = Gamers.Sample.Take(HouseAuction.Lobby.Domain.Lobby.MinGamers).ToList();
            var lobbyCreator = gamers[0];
            var otherGamers = gamers.Skip(1).ToList();

            await _hub.CreateLobby(lobbyCreator);

            await WaitFor.Condition(() => _client.LobbiesCreated.Count == 1);
            var gameId = _client.LobbiesCreated.Single();

            foreach (var (gamer, index) in otherGamers.Select((x, i) => (gamer: x, index: i)))
            {
                await _hub.JoinLobby(gameId, gamer);
                await WaitFor.Condition(() => _client.GamersJoined.Count >= index + 2);
            }

            foreach (var gamer in gamers)
            {
                await _hub.ReadyUp(gameId, gamer);
            }

            await WaitFor.Condition(() => _client.GamesBegun.Count >= 1);

            var game = _client.GamesBegun.SingleOrDefault(x => x == gameId);
            Assert.NotNull(game);
        }

        public async Task DisposeAsync()
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
        }
    }
}
