namespace HouseAuction.Bidding.Domain
{
    public class Hand
    {
        public string Player { get; }

        private List<int> _properties;
        public IEnumerable<int> Properties => _properties;

        public int Coins { get; private set; }

        public Hand(string player, int numberOfPlayers)
        {
            Coins = numberOfPlayers switch
            {
                3 => 28,
                4 => 21,
                5 => 16,
                6 => 14,
                _ => throw new ArgumentOutOfRangeException(nameof(numberOfPlayers))
            };

            Player = player;

            _properties = [];
        }

        public void BuyProperty(int property, Play play, bool isBiddingRoundWinner)
        {
            _properties.Add(property);

            Coins -= isBiddingRoundWinner ? play.Amount : ((play.Amount / 2) + (play.Amount % 2));
        }
    }
}
