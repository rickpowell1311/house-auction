using Tapper;

namespace HouseAuction.Lobby.Requests
{
    public static class GetMyName
    {
        [TranspilationSource]
        public class GetMyNameRequest
        {
            public string GameId { get; set; }
        }

        [TranspilationSource]
        public class GetMyNameResponse
        {
            public string Name { get; set; }
        }
    }
}
