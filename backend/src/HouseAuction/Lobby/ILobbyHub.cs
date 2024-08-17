using HouseAuction.Lobby.Requests;

namespace HouseAuction.Lobby
{
    public interface ILobbyHub
    {
        Task<CreateLobby.CreateLobbyResponse> CreateLobby(CreateLobby.CreateLobbyRequest request);

        Task<FetchLobby.FetchLobbyResponse> FetchLobby(FetchLobby.FetchLobbyRequest request);

        Task JoinLobby(JoinLobby.JoinLobbyRequest request);

        Task ReadyUp(ReadyUp.ReadyUpRequest request);

        Task StartGame(StartGame.StartGameRequest request);

        Task<GetDisconnectedPlayers.GetDisconnectedPlayersResponse> GetDisconnectedPlayers(
            GetDisconnectedPlayers.GetDisconnectedPlayersRequest request);

        Task OnDisconnectedAsync();
    }
}
