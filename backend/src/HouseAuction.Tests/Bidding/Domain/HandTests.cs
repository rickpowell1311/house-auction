using HouseAuction.Bidding.Domain;

namespace HouseAuction.Tests.Bidding.Domain
{
    public class HandTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 2)]
        [InlineData(5, 3)]
        [InlineData(6, 3)]
        public void BuyProperty_WhenNotWinner_DeductsExpectedCoins(int coins, int expectedDeduction)
        {
            var hand = Hand.ForPlayer(Guid.NewGuid().ToString(), "player", 3);

            var startingCoins = hand.Coins;
            hand.BuyProperty(1, coins, false);
            var endingCoins = hand.Coins;

            Assert.Equal(expectedDeduction, startingCoins - endingCoins);
        }
    }
}
