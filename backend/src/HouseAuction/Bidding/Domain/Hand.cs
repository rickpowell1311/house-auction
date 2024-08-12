namespace HouseAuction.Bidding.Domain
{
    public class Hand
    {
        public string BiddingPhaseId { get; }

        public string Player { get; }

        private List<int> _properties;
        public IEnumerable<int> Properties => _properties;

        public int Coins { get; private set; }

        public Hand(string biddingPhaseId, string player, int numberOfPlayers)
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

        public Hand(string biddingPhaseId, string player, int coins, List<int> properties)
        {
            BiddingPhaseId = biddingPhaseId;
            Player = player;
            Coins = coins;
            _properties = properties;
        }

        public void BuyProperty(int property, Play play, bool isBiddingRoundWinner)
        {
            _properties.Add(property);

            var coinsBid = play.Amount ?? 0;

            Coins -= isBiddingRoundWinner ? coinsBid : ((coinsBid / 2) + (coinsBid % 2));
        }
    }
}
