using HouseAuction.Lobby;

namespace HouseAuction.Tests.Lobby._Shared
{
    public class TestLobbyClient : ILobbyClient
    {
        public List<string> GamersJoined { get; } = [];

        public List<string> LobbiesCreated { get; } = [];

        public Task OnGamerJoined(string name)
        {
            GamersJoined.Add(name);

            return Task.CompletedTask;
        }

        public Task OnLobbyCreated(string gameId)
        {
            LobbiesCreated.Add(gameId);

            return Task.CompletedTask;
        }
    }
}
