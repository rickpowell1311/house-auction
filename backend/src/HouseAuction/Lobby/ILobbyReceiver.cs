using TypedSignalR.Client;

namespace HouseAuction.Lobby
{
    public interface ILobbyReceiver
    {
        Task OnLobbyMembersChanged(List<string> members);

        Task OnGameBegun(string gameId);
    }
}
