using HouseAuction.Bidding.Domain;
using HouseAuction.Tests._Shared.TestData;

namespace HouseAuction.Tests.Bidding.Domain
{
    public class BiddingRoundTests
    {
        [Theory, MemberData(nameof(RoundEndingScenarios))]
        public void RoundEndsWhenExpected(
            List<string> players, 
            IEnumerable<(int playerNumber, int? amount, bool isPass)> plays)
        {
            var biddingPhase = new BiddingPhase(GameId.Generate(), players);

            var biddingRound = new BiddingRound(0, biddingPhase);

            foreach (var (playerNumber, amount, isPass) in plays)
            {
                var player = biddingPhase.PlayerCycle.Players[playerNumber];
                var play = isPass ? Play.Pass(player) : Play.Bid(player, amount.Value);

                biddingRound.MakePlay(play);
            }

            Assert.True(biddingRound.HasFinished);
        }

        public static IEnumerable<object[]> RoundEndingScenarios =>
            new List<object[]>
            {
                RoundEndingScenario1,
                RoundEndingScenario2,
                RoundEndingScenario3,
                RoundEndingScenario4
            };

        public static object[] RoundEndingScenario1
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<(int playerNumber, int? amount, bool isPass)>
                {
                    (0, null, true),
                    (1, null, true)
                };

                return [gamers, plays];
            }
        }

        public static object[] RoundEndingScenario2
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<(int playerNumber, int? amount, bool isPass)>
                {
                    (0, null, true),
                    (1, 1, false),
                    (2, null, true),
                };

                return [gamers, plays];
            }
        }

        public static object[] RoundEndingScenario3
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<(int playerNumber, int? amount, bool isPass)>
                {
                    (0, 1, false),
                    (1, 2, false),
                    (2, null, true),
                    (0, 3, false),
                    (1, 4, false),
                    (0, null, true)
                };

                return [gamers, plays];
            }
        }

        public static object[] RoundEndingScenario4
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<(int playerNumber, int? amount, bool isPass)>
                {
                    (0, 1, false),
                    (1, 2, false),
                    (2, null, true),
                    (0, 3, false),
                    (1, 4, false),
                    (0, null, true)
                };

                return [gamers, plays];
            }
        }
    }
}
