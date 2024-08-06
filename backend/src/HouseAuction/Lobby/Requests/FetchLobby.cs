using Tapper;

namespace HouseAuction.Lobby.Requests
{
    public static class FetchLobby
    {
        [TranspilationSource]
        public class FetchLobbyRequest
        {
            public string GameId { get; set; }
        }

        [TranspilationSource]
        public class FetchLobbyResponse
        {
            public List<string> Gamers { get; set; }
        }
    }
}
