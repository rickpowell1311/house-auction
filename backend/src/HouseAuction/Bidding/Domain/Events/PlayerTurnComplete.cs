namespace HouseAuction.Bidding.Domain.Events
{
    public class PlayerTurnComplete
    {
        public Guid BiddingRoundId { get; set; }

        public string Player { get; set; }

        public bool HasPassed { get; set; }

        public int? BidAmount { get; set; }
    }
}
