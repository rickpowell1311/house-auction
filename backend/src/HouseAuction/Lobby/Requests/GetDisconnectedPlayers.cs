using Tapper;

namespace HouseAuction.Lobby.Requests
{
    public static class GetDisconnectedPlayers
    {
        [TranspilationSource]
        public class GetDisconnectedPlayersRequest
        {
            public string GameId { get; set; }
        }

        [TranspilationSource]
        public class GetDisconnectedPlayersResponse
        {
            public List<string> Players { get; set; }

            public GetDisconnectedPlayersResponse()
            {
                Players = [];
            }
        }
    }
}
