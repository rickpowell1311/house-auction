namespace HouseAuction.Bidding.Domain
{
    public class Hand
    {
        public string BiddingPhaseId { get; private set; }

        public string Player { get; private set; }

        private List<int> _properties;
        public IEnumerable<int> Properties => _properties;

        public int Coins { get; private set; }

        public static Hand ForPlayer(
            string biddingPhaseId, 
            string player, 
            int numberOfPlayers)
        {
            var coins = numberOfPlayers switch
            {
                3 => 28,
                4 => 21,
                5 => 16,
                6 => 14,
                _ => throw new ArgumentOutOfRangeException(nameof(numberOfPlayers))
            };

            return new Hand(biddingPhaseId, player, coins);
        }

        private Hand(string biddingPhaseId, string player, int coins)
        {
            BiddingPhaseId = biddingPhaseId;
            Player = player;
            Coins = coins;
        }

        public void BuyProperty(int property, Play play, bool isBiddingRoundWinner)
        {
            _properties.Add(property);

            var coinsBid = play.Amount ?? 0;

            Coins -= isBiddingRoundWinner ? coinsBid : ((coinsBid / 2) + (coinsBid % 2));
        }
    }
}
