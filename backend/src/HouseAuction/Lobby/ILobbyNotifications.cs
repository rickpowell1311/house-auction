namespace HouseAuction.Lobby
{
    public interface ILobbyNotifications
    {
        Task OnLobbyCreated(string gameId);

        Task OnGamerJoined(string name);
    }
}
