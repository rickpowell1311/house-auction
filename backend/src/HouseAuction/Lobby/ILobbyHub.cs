using HouseAuction.Lobby.Requests;

namespace HouseAuction.Lobby
{
    public interface ILobbyHub
    {
        Task<GetMyName.GetMyNameResponse> GetMyName(GetMyName.GetMyNameRequest request);

        Task<CreateLobby.CreateLobbyResponse> CreateLobby(CreateLobby.CreateLobbyRequest request);

        Task<FetchLobby.FetchLobbyResponse> FetchLobby(FetchLobby.FetchLobbyRequest request);

        Task JoinLobby(JoinLobby.JoinLobbyRequest request);

        Task ReadyUp(ReadyUp.ReadyUpRequest request);
    }
}
