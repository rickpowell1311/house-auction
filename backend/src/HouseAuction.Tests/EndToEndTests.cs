using HouseAuction.Lobby;
using HouseAuction.Tests._Shared;
using HouseAuction.Tests._Shared.TestData;
using HouseAuction.Tests.Lobby._Shared;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;

namespace HouseAuction.Tests
{
    [Collection(HubClientFixture.CollectionName)]
    public class EndToEndTests(HubClientFixture.Provider hubClientFixtureProvider)
    {
        private readonly HubClientFixture.Provider _hubClientFixtureProvider = hubClientFixtureProvider;

        [Fact]
        public async Task CanCompleteGame()
        {
            var sample = Gamers.Sample.Take(3);
            var players = new List<(string Name, HubConnection Connection, IHouseAuctionHub Hub)>();

            foreach (var sampleName in sample)
            {
                var (connection, hub) = await PlayerContext();
                players.Add((sampleName, connection, hub));
            }

            var creator = players[0];

            var createLobbyResponse = await creator.Hub.CreateLobby(
                new HouseAuction.Lobby.Requests.CreateLobby.CreateLobbyRequest
            {
                Name = creator.Name
            });
            await creator.Hub.ReadyUp(new HouseAuction.Lobby.Requests.ReadyUp.ReadyUpRequest
            {
                GameId = createLobbyResponse.GameId
            });

            var others = players.Skip(1).ToList();

            foreach (var other in others)
            {
                await other.Hub.JoinLobby(new HouseAuction.Lobby.Requests.JoinLobby.JoinLobbyRequest
                {
                    GameId = createLobbyResponse.GameId,
                    Name = other.Name
                });

                await other.Hub.ReadyUp(new HouseAuction.Lobby.Requests.ReadyUp.ReadyUpRequest
                {
                    GameId = createLobbyResponse.GameId
                });
            }

            await creator.Hub.StartGame(new HouseAuction.Lobby.Requests.StartGame.StartGameRequest
            {
                GameId = createLobbyResponse.GameId
            });

            var biddingPhase = await creator.Hub.GetBiddingPhase(new HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseRequest
            {
                GameId = createLobbyResponse.GameId
            });
        }

        private async Task<(HubConnection connection, IHouseAuctionHub hub)> PlayerContext()
        {
            var connection = await _hubClientFixtureProvider
                .StartHubConnection<HouseAuctionHub>(HouseAuctionHub.Route);
            var hub = connection.CreateHubProxy<IHouseAuctionHub>();

            return (connection, hub);
        }
    }
}
