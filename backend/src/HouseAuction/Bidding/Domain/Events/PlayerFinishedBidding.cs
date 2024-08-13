namespace HouseAuction.Bidding.Domain.Events
{
    public class PlayerFinishedBidding
    {
        public Guid BiddingRoundId { get; set; }

        public string Player { get; set; }

        public int BidAmount { get; set; }

        public int FinishingPosition { get; set; }
    }
}
