namespace HouseAuction.Bidding.Domain
{
    public class Play
    {
        public int Amount { get; private set; }

        public bool IsPass { get; private set; }

        private Play(int amount, bool isPass)
        {
            Amount = amount;
            IsPass = isPass;
        }

        public static Play Pass(string player) => new(0, true);

        public static Play Bid(int amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Cannot bid zero or less");
            };

            return new(amount, false);
        }
    }
}
