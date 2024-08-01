namespace HouseAuction.Lobby
{
    public interface ILobbyHub
    {
        Task CreateLobby(string name);

        Task JoinLobby(string gameId, string name);

        Task ReadyUp(string gameId, string name);
    }
}
