using Tapper;

namespace HouseAuction.Bidding.Requests
{
    public static class GetBiddingPhase
    {
        [TranspilationSource]
        public class GetBiddingPhaseRequest
        {
            public string GameId { get; set; }
        }

        [TranspilationSource]
        public class GetBiddingPhaseResponse
        {
            public bool IsFinished => Deck.TotalProperties <=
                Players.Me.BoughtProperties.Count + Players.Others.Sum(x => x.NumberOfBoughtProperties);

            public GetBiddingPhaseDeckResponse Deck { get; set; }

            public GetBiddingPhasePlayersResponse Players { get; set; }

            public int Round { get; set; }

            public int TotalRounds { get; set; }
        }

        [TranspilationSource]
        public class GetBiddingPhaseDeckResponse
        {
            public List<int> PropertiesOnTheTable { get; set; }

            public int TotalProperties { get; set; }

            public GetBiddingPhaseDeckResponse()
            {
                PropertiesOnTheTable = [];
            }
        }

        [TranspilationSource]
        public class GetBiddingPhasePlayersResponse
        {
            public string LatestWinner { get; set; }

            public GetBiddingPhaseMeResponse Me { get; set; }

            public List<GetBiddingPhaseOtherPersonResponse> Others { get; set; }

            public GetBiddingPhasePlayersResponse()
            {
                Others = [];
            }
        }

        [TranspilationSource]
        public class GetBiddingPhaseMeResponse
        {
            public string Name { get; set; }

            public int Order { get; set; }

            public bool IsTurn { get; set; }

            public List<int> BoughtProperties { get; set; }

            public int Coins { get; set; }

            public GetBiddingPhaseBidResponse Bid { get; set; }

            public GetBiddingPhaseMeResponse()
            {
                BoughtProperties = [];
            }
        }

        [TranspilationSource]
        public class GetBiddingPhaseOtherPersonResponse
        {
            public string Name { get; set; }

            public int Order { get; set; }

            public bool IsTurn { get; set; }

            public int NumberOfBoughtProperties { get; set; }

            public GetBiddingPhaseBidResponse Bid { get; set; }
        }

        [TranspilationSource]
        public class GetBiddingPhaseBidResponse
        {
            public int? Amount { get; set; }

            public bool HasPassed { get; set; }
        }
    }
}
