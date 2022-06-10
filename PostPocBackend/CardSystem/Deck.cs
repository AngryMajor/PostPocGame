using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace PostPocModel.CardSystem
{
    public class Deck<CardType> where CardType : AbstractCard

    {
        private List<CardType> _discardPile = new List<CardType>();
        private Random rand;

        public ImmutableList<CardType> DiscardPile { get { return _discardPile.ToImmutableList(); } }

        private List<CardType> cards;
        public bool isCardToDraw { get { return cards.Count > 0; } }
        public int TotalCardCount { get { return cards.Count + DiscardPile.Count; } }

        public Deck(IEnumerable<CardType> startingCards, int randSeed) {
            cards = new List<CardType>(startingCards);
            rand = new Random(randSeed);
        }

        public Hand CreateHand(int startingSize) {
            if (startingSize < 0)
                throw new ArgumentException("tryed creating hand with invalid staring size: " + startingSize);
            if (startingSize > cards.Count)
                throw new IndexOutOfRangeException("tryed creating hand with staring size: " + startingSize + "but deck size is: " + cards.Count);

            return new Hand(this,startingSize);
        }

        public void DiscardCard(CardType card) {
            if (card == null)
                return;

            _discardPile.Add(card);
        }

        protected CardType TakeTopCard() {
            int index = rand.Next(0,cards.Count-1);

            CardType target = cards[index];
            cards.RemoveAt(index);
            return target;
        }

        protected void Reshuffle() {
            foreach (CardType card in _discardPile)
                cards.Add(card);
            _discardPile.Clear();
        }

        public class Hand
        {
            public ImmutableList<CardType> Cards { get { return myCards.ToImmutableList(); } }

            private Deck<CardType> myDeck;
            private List<CardType> myCards = new List<CardType>();

            public Hand(Deck<CardType> deck, int startingSize) {
                myDeck = deck;
                DrawCard(startingSize);
            }

            public int Count { get { return myCards.Count; } }

            public void DrawCard(int numCards = 1)
            {
                if (numCards < 0) throw new ArgumentException("Tryed to draw invalid number of cards: " + numCards);
                if (numCards == 0) return;

                if (myDeck.TotalCardCount < numCards)
                    DrawCard(myDeck.TotalCardCount);
                else
                {
                    for (int i = 0; i < numCards; i++)
                    {
                        if (myDeck.isCardToDraw)
                            myCards.Add(myDeck.TakeTopCard());
                        else
                        {
                            myDeck.Reshuffle();
                        myCards.Add(myDeck.TakeTopCard());
                    }
                    }
                }
                
                    
            }

            public CardType Card(int index)
            {
                if (index < 0 || myCards.Count <= index)
                    throw new IndexOutOfRangeException();

                return myCards[index];
            }

            public bool CanPlay(int index)
            {
                if (index < 0 || myCards.Count <= index)
                    throw new IndexOutOfRangeException();

                CardContext context = new CardContext();//TODO: create context system

                return Card(index).CanPlay(context);
            }

            private void RemoveAt(int index) {
                myCards.RemoveAt(index);
            }

            public bool TryPlay(int index)
            {
                if (index < 0 || myCards.Count <= index)
                    throw new IndexOutOfRangeException();

                if (CanPlay(index) == false)
                    return false;

                CardType target = Card(index);
                RemoveAt(index);
                if (target.DiscardOnPlay == true)
                    myDeck.DiscardCard(target);
                target.Play();
                return true;
            }

        }
    }
}
