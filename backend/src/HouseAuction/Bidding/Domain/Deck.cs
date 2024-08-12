﻿namespace HouseAuction.Bidding.Domain
{
    public class Deck
    {
        public int DealSize { get; private set; }

        public int Dealt { get; private set; }

        public List<int> Properties { get; private set; }

        public bool AllCardsDealt => Dealt >= Properties.Count;

        public List<int> CurrentDeal => Properties.Skip(Dealt).Take(DealSize).ToList();

        public Deck(int numberOfPlayers)
        {
            var numberOfCards = numberOfPlayers == 4 ? 28 : 30;

            DealSize = numberOfPlayers;

            var cards = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            var shuffledCards = cards.OrderBy(x => new Random().NextDouble()).ToList();

            Properties = new List<int>(shuffledCards.Take(numberOfCards));
        }

        public void DealNext()
        {
            if (AllCardsDealt)
            {
                throw new InvalidOperationException("Deck has no cards left to deal");
            }

            Dealt++;
        }
    }
}
