using HouseAuction.Bidding.Domain;
using HouseAuction.Bidding.Requests;
using HouseAuction.Lobby;
using HouseAuction.Tests._Shared;
using HouseAuction.Tests._Shared.TestData;
using Microsoft.AspNetCore.SignalR.Client;
using TypedSignalR.Client;
using static HouseAuction.Lobby.Requests.CreateLobby;

namespace HouseAuction.Tests
{
    [Collection(HubClientFixture.CollectionName)]
    public class EndToEndTests(HubClientFixture.Provider hubClientFixtureProvider)
    {
        private readonly HubClientFixture.Provider _hubClientFixtureProvider = hubClientFixtureProvider;

        [Fact]
        public async Task CanCompleteGame()
        {
            var sample = Gamers.Sample.Take(6);
            var creator = sample.First();
            var others = sample.Except([creator]);
            var players = new Dictionary<string, 
                (HubConnection Connection, IHouseAuctionHub Hub, TestHouseAuctionReceiver Receiver)>();

            foreach (var sampleName in sample)
            {
                var (connection, hub, receiver) = await PlayerContext();
                players[sampleName] = (connection, hub, receiver);
            }

            var createLobbyResponse = await players[creator].Hub.CreateLobby(
                new HouseAuction.Lobby.Requests.CreateLobby.CreateLobbyRequest
            {
                Name = creator
            });
            await players[creator].Hub.ReadyUp(new HouseAuction.Lobby.Requests.ReadyUp.ReadyUpRequest
            {
                GameId = createLobbyResponse.GameId
            });

            foreach (var other in others)
            {
                await players[other].Hub.JoinLobby(new HouseAuction.Lobby.Requests.JoinLobby.JoinLobbyRequest
                {
                    GameId = createLobbyResponse.GameId,
                    Name = other
                });

                await players[other].Hub.ReadyUp(new HouseAuction.Lobby.Requests.ReadyUp.ReadyUpRequest
                {
                    GameId = createLobbyResponse.GameId
                });
            }

            await players[creator].Hub.StartGame(new HouseAuction.Lobby.Requests.StartGame.StartGameRequest
            {
                GameId = createLobbyResponse.GameId
            });

            var biddingPhase = await players[creator].Hub.GetBiddingPhase(new HouseAuction.Bidding.Requests.GetBiddingPhase.GetBiddingPhaseRequest
            {
                GameId = createLobbyResponse.GameId
            });

            var startingPlayer = biddingPhase.Players.Me.IsTurn
                ? biddingPhase.Players.Me.Name
                : biddingPhase.Players.Others.Single(x => x.IsTurn).Name;

            await CompleteBiddingRound(
                biddingPhase, 
                players, 
                createLobbyResponse.GameId, 
                startingPlayer);

            // TODO: Add more functionality as it becomes available
        }

        private async Task CompleteBiddingRound(
            GetBiddingPhase.GetBiddingPhaseResponse biddingPhase,
            Dictionary<string, (HubConnection Connection, IHouseAuctionHub Hub, TestHouseAuctionReceiver Receiver)> players,
            string gameId,
            string startingPlayer)
        {
            var playerBanks = biddingPhase.Players
                .Others
                .Select(x => x.Name)
                .Concat([biddingPhase.Players.Me.Name])
                .ToDictionary(x => x, _ => Hand.StartingCoinsByPlayerCount[biddingPhase.Players.Others.Count + 1]);
            var playerPasses = new List<string>();

            var highestBid = 0;
            var chanceOfPassing = 0.3d;
            var activePlayer = startingPlayer;

            while (playerPasses.Count() < playerBanks.Count - 1)
            {
                var pass = new Random().NextDouble() < chanceOfPassing
                    || highestBid >= playerBanks[activePlayer]
                    || playerPasses.Contains(activePlayer);

                if (pass)
                {
                    await players[activePlayer].Hub.Pass(new Pass.PassRequest
                    {
                        GameId = gameId
                    });

                    playerPasses.Add(activePlayer);
                }
                else
                {
                    await players[activePlayer].Hub.Bid(new Bid.BidRequest
                    {
                        Amount = highestBid++,
                        GameId = gameId
                    });

                    playerBanks[activePlayer] -= highestBid;
                }

                await WaitFor.Condition(() => players[activePlayer]
                    .Receiver
                    .TestBiddingReceiver
                    .LatestPlayerTurnFinished?
                    .Player == activePlayer);

                activePlayer = players[activePlayer]
                    .Receiver
                    .TestBiddingReceiver
                    .LatestPlayerTurnFinished
                    .NextPlayer;
            }
        }

        private async Task<(HubConnection connection, IHouseAuctionHub hub, TestHouseAuctionReceiver receiver)> PlayerContext()
        {
            var connection = await _hubClientFixtureProvider
                .StartHubConnection<HouseAuctionHub>(HouseAuctionHub.Route);
            var hub = connection.CreateHubProxy<IHouseAuctionHub>();
            var receiver = new TestHouseAuctionReceiver();
            connection.Register<IHouseAuctionReceiver>(receiver);

            return (connection, hub, receiver);
        }
    }
}
