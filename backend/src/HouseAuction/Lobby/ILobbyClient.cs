namespace HouseAuction.Lobby
{
    public interface ILobbyClient
    {
        Task OnLobbyCreated(string gameId);

        Task OnGamerJoined(string name);
    }
}
