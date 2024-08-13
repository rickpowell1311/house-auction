using Google.Cloud.Diagnostics.Common;
using HouseAuction.Bidding.Domain;
using HouseAuction.Tests._Shared.TestData;

namespace HouseAuction.Tests.Bidding.Domain
{
    public class BiddingRoundTests
    {
        [Theory, MemberData(nameof(RoundEndingScenarios))]
        public void RoundEndsWhenExpected(
            List<string> players, 
            List<(int playerNumber, int? amount, bool isPass)> plays)
        {
            var biddingPhase = new BiddingPhase(GameId.Generate(), players);

            var biddingRound = new BiddingRound(0, biddingPhase);

            foreach (var (playerNumber, amount, isPass) in plays)
            {
                var player = biddingPhase.PlayerCycle.Players[playerNumber];

                if (isPass)
                {
                    biddingRound.Pass(player);
                }
                else
                {
                    biddingRound.Bid(player, amount.Value);
                }
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

        [Theory, MemberData(nameof(InvalidBidScenarios))]
        public void InvalidBidThrowsException(
            List<string> players, 
            List<(int playerNumber, int? amount, bool isPass)> plays)
        {
            var biddingPhase = new BiddingPhase(GameId.Generate(), players);

            var biddingRound = new BiddingRound(0, biddingPhase);

            foreach (var (playerNumber, amount, isPass, order) in plays.Select((x, i) => (x.playerNumber, x.amount, x.isPass, i)))
            {
                var player = biddingPhase.PlayerCycle.Players[playerNumber];

                if (order == plays.Count - 1)
                {
                    Assert.Throws<InvalidOperationException>(() =>
                    {
                        if (isPass)
                        {
                            biddingRound.Pass(player);
                        }
                        else
                        {
                            biddingRound.Bid(player, amount.Value);
                        }
                    });
                }
                else if (isPass)
                {
                    biddingRound.Pass(player);
                }
                else
                {
                    biddingRound.Bid(player, amount.Value);
                }
            }
        }

        public static IEnumerable<object[]> InvalidBidScenarios =>
            new List<object[]>
            {
                InvalidBidScenario1,
                InvalidBidScenario2,
                InvalidBidScenario3
            };

        public static object[] InvalidBidScenario1
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<(int playerNumber, int? amount, bool isPass)>
                {
                    (0, 1, false),
                    (1, 1, false)
                };

                return [gamers, plays];
            }
        }

        public static object[] InvalidBidScenario2
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<(int playerNumber, int? amount, bool isPass)>
                {
                    (0, 1, false),
                    (0, 2, false)
                };

                return [gamers, plays];
            }
        }

        public static object[] InvalidBidScenario3
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<(int playerNumber, int? amount, bool isPass)>
                {
                    (0, null, true),
                    (1, 2, false),
                    (2, 3, false),
                    (0, null, true)
                };

                return [gamers, plays];
            }
        }
    }
}
