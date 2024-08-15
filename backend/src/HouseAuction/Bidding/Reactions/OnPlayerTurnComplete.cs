using Tapper;

namespace HouseAuction.Bidding.Reactions
{
    [TranspilationSource]
    public class OnPlayerTurnComplete
    {
        public string Player { get; set; }

        public string NextPlayer { get; set; }

        public OnPlayerTurnFinishedResult Result { get; set; }


        [TranspilationSource]
        public class OnPlayerTurnFinishedResult
        {
            public int? Bid { get; set; }

            public bool Passed { get; set; }
        }
    }
}
