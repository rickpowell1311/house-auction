using HouseAuction.Lobby.Reactions;

namespace HouseAuction.Lobby
{
    public interface ILobbyReceiver
    {
        Task OnLobbyMembersChanged(OnLobbyMembersChanged.OnLobbyMembersChangedReaction reaction);

        Task OnGameReadinessChanged(OnGameReadinessChanged.OnGameReadinessChangedReaction reaction);
    }
}
