using HouseAuction.Lobby.Reactions;

namespace HouseAuction.Lobby
{
    public interface ILobbyReceiver
    {
        Task OnLobbyMembersChanged(OnLobbyMembersChanged.OnLobbyMembersChangedReaction reaction);

        Task OnGameBegun(OnGameBegun.OnGameBegunReaction reaction);
    }
}
