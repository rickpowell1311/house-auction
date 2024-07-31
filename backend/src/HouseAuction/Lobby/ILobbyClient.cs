namespace HouseAuction.Lobby
{
    public interface ILobbyClient
    {
        Task OnLobbyCreated(string gameId);

        Task OnLobbyMembersChanged(List<string> members);

        Task OnGameBegun(string gameId);
    }
}
