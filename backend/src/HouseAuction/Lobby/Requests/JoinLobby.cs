using Tapper;

namespace HouseAuction.Lobby.Requests
{
    public static class JoinLobby
    {
        [TranspilationSource]
        public class JoinLobbyRequest
        {
            public string Name { get; set; }

            public string GameId { get; set; }
        }
    }
}
