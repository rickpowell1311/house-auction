using Tapper;

namespace HouseAuction.Lobby.Requests
{
    public static class CreateLobby
    {
        [TranspilationSource]
        public class CreateLobbyRequest
        {
            public string Name { get; set; }
        }

        [TranspilationSource]
        public class CreateLobbyResponse
        {
            public string GameId { get; set; }
        }
    }
}

