using HouseAuction.Lobby;

namespace HouseAuction.Tests.Lobby._Shared
{
    public class TestLobbyReceiver : ILobbyReceiver
    {
        public List<string> GamersJoined { get; private set; } = [];

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
    }
}
