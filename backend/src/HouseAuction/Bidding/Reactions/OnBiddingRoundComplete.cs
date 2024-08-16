using Tapper;

namespace HouseAuction.Bidding.Reactions
{
    [TranspilationSource]
    public class OnBiddingRoundComplete
    {
        public int CoinsRemaining { get; set; }

        public OnBiddingRoundCompleteNextRound NextRound { get; set; }


        [TranspilationSource]
        public class OnBiddingRoundCompleteNextRound
        {
            public List<int> Properties { get; set; }

            public bool IsLastRound { get; set; }
        }
    }
}
