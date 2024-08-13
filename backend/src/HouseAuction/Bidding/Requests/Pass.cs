using Tapper;

namespace HouseAuction.Bidding.Requests
{
    public static class Pass
    {
        [TranspilationSource]
        public class PassRequest
        {
            public string GameId { get; set; }
        }
    }
}
