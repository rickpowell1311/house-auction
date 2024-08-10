using Tapper;

namespace HouseAuction.Lobby.Requests
{
    public static class ReadyUp
    {
        [TranspilationSource]
        public class ReadyUpRequest
        {
            public string GameId { get; set; }
        }
    }
}
