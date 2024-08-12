using System.Runtime.CompilerServices;

namespace HouseAuction.Bidding.Domain
{
    public class Play
    {
        public string Player { get; private set; }

        public int? Amount { get; private set; }

        public bool IsPass { get; private set; }

        private Play(string player, int? amount, bool isPass)
        {
            Player = player;
            Amount = amount;
            IsPass = isPass;
        }

        public static Play Pass(string player) => new(player, null, true);

        public static Play Bid(string player, int amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Cannot bid zero or less");
            };

            return new(player, amount, false);
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
    }
}
