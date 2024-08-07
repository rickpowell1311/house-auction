using HouseAuction.Lobby;
using HouseAuction.Lobby.Reactions;

namespace HouseAuction.Tests.Lobby._Shared
{
    public class TestLobbyReceiver : ILobbyReceiver
    {
        public List<OnLobbyMembersChanged.OnLobbyMembersChangedReaction> LobbyMembersChanges { get; } = [];

        public List<OnGameReadinessChanged.OnGameReadinessChangedReaction> GameReadinessChanges { get; } = [];

        public List<OnGameStarted.OnGameStartedReaction> GamesStarted { get; } = [];

        public Task OnGameReadinessChanged(OnGameReadinessChanged.OnGameReadinessChangedReaction reaction)
        {
            GameReadinessChanges.Add(reaction);

            return Task.CompletedTask;
        }

        public Task OnGameStarted(OnGameStarted.OnGameStartedReaction reaction)
        {
            GamesStarted.Add(reaction);

            return Task.CompletedTask;
        }

        public Task OnLobbyMembersChanged(OnLobbyMembersChanged.OnLobbyMembersChangedReaction reaction)
        {
            LobbyMembersChanges.Add(reaction);

            return Task.CompletedTask;
        }
    }
}
