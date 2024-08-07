using HouseAuction.Lobby;
using HouseAuction.Lobby.Requests;
using HouseAuction.Tests._Shared;
using HouseAuction.Tests._Shared.TestData;
using HouseAuction.Tests.Lobby._Shared;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;
using Xunit;
using static HouseAuction.Lobby.Requests.CreateLobby;

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
            await _hub.CreateLobby(new CreateLobbyRequest { Name = Gamers.Sample[0] });
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

            await WaitFor.Condition(() => 
                _client.LobbyMembersChanges.Any()
                && _client.LobbyMembersChanges.Last().Gamers.Count == 2 
                && secondClient.LobbyMembersChanges.Any()
                && secondClient.LobbyMembersChanges.Last().Gamers.Count == 2);

            Assert.Equal(2, _client.LobbyMembersChanges.Last().Gamers.Count);
            Assert.Equal(2, secondClient.LobbyMembersChanges.Last().Gamers.Count);
        }


        [Fact]
        public async Task CanFetchLobby()
        {
            var first = Gamers.Sample[0];
            var second = Gamers.Sample[1];

            var createLobbyResponse = await _hub.CreateLobby(new CreateLobbyRequest { Name = first });

            // Ready up a second gamer, but not the first
            var secondConnection = await _hubClientFixtureProvider.StartHubConnection<HouseAuctionHub>(HouseAuctionHub.Route);
            var secondHub = secondConnection.CreateHubProxy<ILobbyHub>();
            var secondClient = new TestLobbyReceiver();
            secondConnection.Register<ILobbyReceiver>(secondClient);
            await secondHub.JoinLobby(new JoinLobby.JoinLobbyRequest { GameId = createLobbyResponse.GameId, Name = second });
            await secondHub.ReadyUp(new ReadyUp.ReadyUpRequest { GameId = createLobbyResponse.GameId, Name = second });


            var firstClientResult = await _hub.FetchLobby(new FetchLobby.FetchLobbyRequest { GameId = createLobbyResponse.GameId });
            var firstClientNotReadyGamers = firstClientResult.Gamers.Where(x => !x.IsReady).ToList();
            Assert.Single(firstClientNotReadyGamers);
            Assert.Equal(first, firstClientNotReadyGamers.Single().Name);
            Assert.True(firstClientNotReadyGamers.Single().IsMe);

            var secondClientResult = await secondHub.FetchLobby(new FetchLobby.FetchLobbyRequest { GameId = createLobbyResponse.GameId });
            var secondClientNotReadyGamers = secondClientResult.Gamers.Where(x => !x.IsReady).ToList();
            Assert.Single(secondClientNotReadyGamers);
            Assert.Equal(first, secondClientNotReadyGamers.Single().Name);
            Assert.False(secondClientNotReadyGamers.Single().IsMe);
        }

        [Fact]
        public async Task CanReadyUp()
        {
            var first = Gamers.Sample[0];
            var second = Gamers.Sample[1];

            var createLobbyResponse = await _hub.CreateLobby(new CreateLobbyRequest { Name = first });

            // Ready up a second gamer, but not the first
            var secondConnection = await _hubClientFixtureProvider.StartHubConnection<HouseAuctionHub>(HouseAuctionHub.Route);
            var secondHub = secondConnection.CreateHubProxy<ILobbyHub>();
            var secondClient = new TestLobbyReceiver();
            secondConnection.Register<ILobbyReceiver>(secondClient);
            await secondHub.JoinLobby(new JoinLobby.JoinLobbyRequest { GameId = createLobbyResponse.GameId, Name = second });
            await secondHub.ReadyUp(new ReadyUp.ReadyUpRequest { GameId = createLobbyResponse.GameId, Name = second });

            var result = await _hub.FetchLobby(new FetchLobby.FetchLobbyRequest { GameId = createLobbyResponse.GameId });

            var ready = result.Gamers.Where(x => x.IsReady).ToList();
            var notReady = result.Gamers.Where(x => !x.IsReady).ToList();

            Assert.Single(ready);
            Assert.Equal(second, ready.Single().Name);
            Assert.False(ready.Single().IsMe);

            Assert.Single(notReady);
            Assert.Equal(first, notReady.Single().Name);
            Assert.True(notReady.Single().IsMe);

            await WaitFor.Condition(() => 
                _client.LobbyMembersChanges.Any()
                && _client.LobbyMembersChanges.Last().Gamers.Any(x => x.IsReady));

            var firstClientGamersReady = _client.LobbyMembersChanges.Last().Gamers.Where(x => x.IsReady).ToList();

            Assert.Single(firstClientGamersReady);
            Assert.Equal(second, firstClientGamersReady.Single().Name);
            Assert.False(firstClientGamersReady.Single().IsMe);

            await WaitFor.Condition(() => 
                secondClient.LobbyMembersChanges.Any()
                && secondClient.LobbyMembersChanges.Last().Gamers.Any(x => x.IsReady));

            var secondClientGamersReady = secondClient.LobbyMembersChanges.Last().Gamers.Where(x => x.IsReady).ToList();

            Assert.Single(secondClientGamersReady);
            Assert.Equal(second, secondClientGamersReady.Single().Name);
            Assert.True(secondClientGamersReady.Single().IsMe);
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
                await WaitFor.Condition(() => 
                    _client.LobbyMembersChanges.Any()
                    && _client.LobbyMembersChanges.Last().Gamers.Count >= index + 2);
            }

            foreach (var gamer in gamers)
            {
                await _hub.ReadyUp(new ReadyUp.ReadyUpRequest { GameId = createLobbyResponse.GameId, Name = gamer });
            }

            await WaitFor.Condition(() => _client.GameReadinessChanges.Count >= 1);
            Assert.Single(_client.GameReadinessChanges);

            var game = _client.GameReadinessChanges
                .Single(x => x.GameId == createLobbyResponse.GameId);

            Assert.True(game.IsReadyToStart);

            await _hub.StartGame(new StartGame.Request
            {
                Name = lobbyCreator,
                GameId = createLobbyResponse.GameId
            });
        }

        public async Task DisposeAsync()
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
        }
    }
}
