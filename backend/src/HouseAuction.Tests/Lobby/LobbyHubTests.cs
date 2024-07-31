using HouseAuction.Lobby;
using HouseAuction.Tests._Shared;
using HouseAuction.Tests._Shared.TestData;
using Microsoft.AspNetCore.SignalR.Client;

namespace HouseAuction.Tests.Lobby
{
    [Collection(HubClientFixture.CollectionName)]
    public class LobbyHubTests(HubClientFixture.Provider hubClientFixtureProvider)
    {
        private readonly HubClientFixture.Provider _hubClientFixtureProvider = hubClientFixtureProvider;

        [Fact]
        public async Task CanCreateLobby()
        {
            var connection = _hubClientFixtureProvider.GetHubConnection<LobbyHub>(LobbyHub.Route);

            string createdGameId = null;
            connection.On<string>(nameof(ILobbyNotifications.OnLobbyCreated), gameId =>
            {
                createdGameId = gameId;
            });

            await connection.StartAsync();
            Assert.True(connection.State == HubConnectionState.Connected);

            await connection.InvokeAsync<string>(nameof(LobbyHub.CreateLobby), Gamers.Sample[0]);

            await WaitUntil.IsNotNull(() => createdGameId);
            Assert.NotNull(createdGameId);
        }
    }
}
