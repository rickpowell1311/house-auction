namespace HouseAuction.Bidding.Domain
{
    public class Deck
    {
        public int DealSizePerRound { get; private set; }

        private List<int> _properties;
        public IEnumerable<int> Properties => _properties;

        public static Deck ForNumberOfPlayers(int numberOfPlayers)
        {
            var numberOfCards = numberOfPlayers == 4 ? 28 : 30;

            var cards = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            var shuffledCards = cards.OrderBy(x => new Random().NextDouble()).ToList();
            var properties = new List<int>(shuffledCards.Take(numberOfCards));

            return new Deck(numberOfPlayers) { _properties = properties };
        }

        private Deck(int dealSize)
        {
            DealSizePerRound = dealSize;
        }

        public List<int> ForRound(int round)
        {
            if (round * DealSizePerRound >= _properties.Count)
            {
                throw new InvalidOperationException("Deck does not have any more properties to deal");
            }

            return _properties.Skip(round * DealSizePerRound).Take(DealSizePerRound).ToList();
        }
    }
}
