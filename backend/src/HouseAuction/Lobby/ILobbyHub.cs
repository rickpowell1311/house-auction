using TypedSignalR.Client;

namespace HouseAuction.Lobby
{
    public interface ILobbyHub
    {
        Task<string> GetMyName(string gameId);

        Task<string> CreateLobby(string name);

        Task<List<string>> FetchLobby(string gameId);

        Task JoinLobby(string gameId, string name);

        Task ReadyUp(string gameId, string name);
    }
}
