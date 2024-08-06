using HouseAuction.Lobby;
using HouseAuction.Lobby.Requests;
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
        private TestLobbyReceiver _client;

        public async Task InitializeAsync()
        {
            _connection = await _hubClientFixtureProvider.StartHubConnection<HouseAuctionHub>(HouseAuctionHub.Route);
            _hub = _connection.CreateHubProxy<ILobbyHub>();
            _client = new TestLobbyReceiver();
            _connection.Register<ILobbyReceiver>(_client);
        }

        [Fact]
        public async Task CanCreateLobby()
        {
            await _hub.CreateLobby(new CreateLobby.CreateLobbyRequest { Name = Gamers.Sample[0] });
        }

        [Fact]
        public async Task CanFetchLobby()
        {
            var createLobbyResponse = await _hub.CreateLobby(new CreateLobby.CreateLobbyRequest { Name = Gamers.Sample[0] });
            var result = await _hub.FetchLobby(new FetchLobby.FetchLobbyRequest { GameId = createLobbyResponse.GameId });

            Assert.Single(result.Gamers);
        }

        [Fact]
        public async Task CanJoinLobby()
        {
            var firstGamer = Gamers.Sample[0];
            var secondGamer = Gamers.Sample[1];

            var createLobbyResponse = await _hub.CreateLobby(new CreateLobby.CreateLobbyRequest { Name = firstGamer });

            var secondClient = new TestLobbyReceiver();
            _connection.Register<ILobbyReceiver>(secondClient);

            await _hub.JoinLobby(new JoinLobby.JoinLobbyRequest { GameId = createLobbyResponse.GameId, Name = secondGamer });

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

            var createLobbyResponse = await _hub.CreateLobby(new CreateLobby.CreateLobbyRequest { Name = lobbyCreator });

            foreach (var (gamer, index) in otherGamers.Select((x, i) => (gamer: x, index: i)))
            {
                await _hub.JoinLobby(new JoinLobby.JoinLobbyRequest { GameId = createLobbyResponse.GameId, Name = gamer });
                await WaitFor.Condition(() => _client.GamersJoined.Count >= index + 2);
            }

            foreach (var gamer in gamers)
            {
                await _hub.ReadyUp(new ReadyUp.ReadyUpRequest { GameId = createLobbyResponse.GameId, Name = gamer });
            }

            await WaitFor.Condition(() => _client.GamesBegun.Count >= 1);

            var game = _client.GamesBegun.SingleOrDefault(x => x == createLobbyResponse.GameId);
            Assert.NotNull(game);
        }

        public async Task DisposeAsync()
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
        }
    }
}
