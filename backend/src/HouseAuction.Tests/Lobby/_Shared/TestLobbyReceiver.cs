using HouseAuction.Lobby;
using HouseAuction.Lobby.Reactions;

namespace HouseAuction.Tests.Lobby._Shared
{
    public class TestLobbyReceiver : ILobbyReceiver
    {
        public List<OnLobbyMembersChanged.OnLobbyMembersChangedReaction> LobbyMembersChanges { get; private set; } = [];

        public List<OnGameBegun.OnGameBegunReaction> GamesBegun { get; } = [];

        public Task OnGameBegun(OnGameBegun.OnGameBegunReaction reaction)
        {
            GamesBegun.Add(reaction);

            return Task.CompletedTask;
        }

        public Task OnLobbyMembersChanged(OnLobbyMembersChanged.OnLobbyMembersChangedReaction reaction)
        {
            LobbyMembersChanges.Add(reaction);

            return Task.CompletedTask;
        }
    }
}
