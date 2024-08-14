namespace HouseAuction.Bidding.Domain
{
    public class Hand
    {
        public static Dictionary<int, int> StartingCoinsByPlayerCount => new()
        {
            { 3, 28 },
            { 4, 21 },
            { 5, 16 },
            { 6, 14 }
        };

        public string BiddingPhaseId { get; private set; }

        public string Player { get; private set; }

        public IReadOnlyList<int> Properties { get; private set; }

        public int Coins { get; private set; }

        public static Hand ForPlayer(
            string biddingPhaseId, 
            string player, 
            int numberOfPlayers)
        {
            if (!StartingCoinsByPlayerCount.ContainsKey(numberOfPlayers))
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfPlayers));
            }

            return new Hand(
                biddingPhaseId, 
                player, 
                StartingCoinsByPlayerCount[numberOfPlayers]);
        }

        private Hand(string biddingPhaseId, string player, int coins)
        {
            BiddingPhaseId = biddingPhaseId;
            Player = player;
            Coins = coins;

            Properties = [];
        }

        public void BuyProperty(int property, int? amount, bool isBiddingRoundWinner)
        {
            Properties = Properties.Append(property).ToList();

            var coinsBid = amount ?? 0;

            Coins -= isBiddingRoundWinner ? coinsBid : ((coinsBid / 2) + (coinsBid % 2));
        }
    }
}
