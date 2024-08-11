using HouseAuction.Bidding.Domain;
using HouseAuction.Tests._Shared.TestData;

namespace HouseAuction.Tests.Bidding.Domain
{
    public class BiddingRoundTests
    {
        [Theory, MemberData(nameof(RoundEndingScenarios))]
        public void RoundEndsWhenExpected(
            List<string> players, 
            IEnumerable<(string Player, Play Play)> plays)
        {
            var biddingRound = new BiddingRound(
                new PlayerCycle(players
                    .Select((x, i) => new { Player = x, Order = i })
                    .ToDictionary(x => x.Order, x => x.Player)));
            
            foreach (var (player, play) in plays)
            {
                biddingRound.MakePlay(player, play);
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
                var plays = gamers.Take(2).Select(x => (x, Play.Pass));

                return [gamers, plays];
            }
        }

        public static object[] RoundEndingScenario2
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<(string, Play)>
                {
                    (gamers[0], Play.Pass),
                    (gamers[1], Play.Bid(1)),
                    (gamers[2], Play.Pass)
                };

                return [gamers, plays];
            }
        }

        public static object[] RoundEndingScenario3
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<(string, Play)>
                {
                    (gamers[0], Play.Bid(1)),
                    (gamers[1], Play.Bid(2)),
                    (gamers[2], Play.Pass),
                    (gamers[0], Play.Bid(3)),
                    (gamers[1], Play.Pass)
                };

                return [gamers, plays];
            }
        }

        public static object[] RoundEndingScenario4
        {
            get
            {
                var gamers = Gamers.Sample.Select(x => x).Take(3).ToList();
                var plays = new List<(string, Play)>
                {
                    (gamers[0], Play.Bid(1)),
                    (gamers[1], Play.Bid(2)),
                    (gamers[2], Play.Pass),
                    (gamers[0], Play.Bid(3)),
                    (gamers[1], Play.Bid(4)),
                    (gamers[0], Play.Pass)
                };

                return [gamers, plays];
            }
        }
    }
}
