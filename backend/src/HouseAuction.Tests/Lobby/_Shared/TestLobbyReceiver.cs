using HouseAuction.Lobby;
using HouseAuction.Lobby.Reactions;

namespace HouseAuction.Tests.Lobby._Shared
{
    public class TestLobbyReceiver : ILobbyReceiver
    {
        public List<OnLobbyMembersChanged.OnLobbyMembersChangedReaction> LobbyMembersChanges { get; private set; } = [];

        public List<OnGameReadinessChanged.OnGameReadinessChangedReaction> GameReadinessChanges { get; } = [];

        public Task OnGameReadinessChanged(OnGameReadinessChanged.OnGameReadinessChangedReaction reaction)
        {
            GameReadinessChanges.Add(reaction);

            return Task.CompletedTask;
        }

        public Task OnLobbyMembersChanged(OnLobbyMembersChanged.OnLobbyMembersChangedReaction reaction)
        {
            LobbyMembersChanges.Add(reaction);

            return Task.CompletedTask;
        }
    }
}
