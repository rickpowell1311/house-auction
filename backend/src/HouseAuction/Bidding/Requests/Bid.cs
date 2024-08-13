using Tapper;

namespace HouseAuction.Bidding.Requests
{
    public static class Bid
    {
        [TranspilationSource]
        public class BidRequest
        {
            public string GameId { get; set; }

            public int Amount { get; set; }
        }
    }
}
