using Tapper;

namespace HouseAuction.Lobby.Requests
{
    public static class Reconnect
    {
        [TranspilationSource]
        public class ReconnectRequest
        {
            public string GameId { get; set; }

            public string Gamer { get; set; }
        }
    }
}
