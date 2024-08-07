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
            public List<FetchLobbyResponseGamer> Gamers { get; set; }
        }

        [TranspilationSource]
        public class FetchLobbyResponseGamer
        {
            public string Name { get; set; }

            public bool IsMe { get; set; }

            public bool IsCreator { get; set; }

            public bool IsReady { get; set; }
        }
    }
}
