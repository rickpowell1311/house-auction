using HouseAuction.Lobby;

namespace HouseAuction.Tests.Lobby._Shared
{
    public class TestLobbyClient : ILobbyClient
    {
        public List<string> GamersJoined { get; private set; } = [];

        public List<string> LobbiesCreated { get; } = [];

        public List<string> GamesBegun { get; } = [];

        public Task OnGameBegun(string gameId)
        {
            GamesBegun.Add(gameId);

            return Task.CompletedTask;
        }

        public Task OnLobbyMembersChanged(List<string> members)
        {
            GamersJoined = members;

            return Task.CompletedTask;
        }

        public Task OnLobbyCreated(string gameId)
        {
            LobbiesCreated.Add(gameId);

            return Task.CompletedTask;
        }
    }
}
