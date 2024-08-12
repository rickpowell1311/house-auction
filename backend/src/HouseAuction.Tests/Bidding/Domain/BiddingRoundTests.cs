using HouseAuction.Bidding.Domain;
using HouseAuction.Tests._Shared.TestData;

namespace HouseAuction.Tests.Bidding.Domain
{
    public class BiddingRoundTests
    {
        [Theory, MemberData(nameof(RoundEndingScenarios))]
        public void RoundEndsWhenExpected(
            List<string> players, 
            IEnumerable<Play> plays)
        {
            var biddingRound = new BiddingRound(0, "XGHLK", new PlayerCycle(players
                .Select((x, i) => new { Player = x, Order = i })
                .ToDictionary(x => x.Order, x => x.Player), 0));
            
            foreach (var play in plays)
            {
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
                var plays = gamers.Take(2).Select(Play.Pass);

                return [gamers, plays];
            }
        }

        public static object[] RoundEndingScenario2
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<Play>
                {
                    Play.Pass(gamers[0]),
                    Play.Bid(gamers[1], 1),
                    Play.Pass(gamers[2])
                };

                return [gamers, plays];
            }
        }

        public static object[] RoundEndingScenario3
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<Play>
                {
                    Play.Bid(gamers[0], 1),
                    Play.Bid(gamers[1], 2),
                    Play.Pass(gamers[2]),
                    Play.Bid(gamers[0], 3),
                    Play.Pass(gamers[1])
                };

                return [gamers, plays];
            }
        }

        public static object[] RoundEndingScenario4
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<Play>
                {
                    Play.Bid(gamers[0], 1),
                    Play.Bid(gamers[1], 2),
                    Play.Pass(gamers[2]),
                    Play.Bid(gamers[0], 3),
                    Play.Bid(gamers[1], 4),
                    Play.Pass(gamers[0])
                };

                return [gamers, plays];
            }
        }
    }
}
