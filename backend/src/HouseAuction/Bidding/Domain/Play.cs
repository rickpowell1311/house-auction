using System.Runtime.CompilerServices;

namespace HouseAuction.Bidding.Domain
{
    public class Play
    {
        public string Player { get; private set; }

        public int Order { get; private set; }

        public int? Amount { get; private set; }

        public bool IsPass { get; private set; }

        private Play(string player, int order, int? amount, bool isPass)
        {
            Player = player;
            Order = order;
            Amount = amount;
            IsPass = isPass;
        }

        public static Play Pass(string player, int order) => new(player, order, null, true);

        public static Play Bid(string player, int order, int amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Cannot bid zero or less");
            };

            return new(player, order, amount, false);
        }
    }

    public static class PlaysExtensions
    {
        public static IEnumerable<string> Players(this IEnumerable<Play> plays)
        {
            return plays
                .Select(x => x.Player)
                .Distinct();
        }

        public static IEnumerable<string> PlayersWhoPassed(this IEnumerable<Play> plays)
        {
            return plays
                .Where(x => x.IsPass)
                .Select(x => x.Player)
                .Distinct();
        }

        public static int HighestBid(
            this IEnumerable<Play> plays,
            string player)
        {
            return plays.GroupBy(x => x.Player)
                .Where(x => x.Key == player)
                .SelectMany(x => x.ToList().Select(y => y.Amount ?? 0))
                .DefaultIfEmpty(0)
                .Max();
        }
    }
}
