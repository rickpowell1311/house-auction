namespace HouseAuction.Lobby
{
    public interface ILobby
    {
        Task OnGamerJoined(string name);
    }
}
