using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace PostPocModel.CardSystem
{
    public class Deck<T> where T : AbstractCard
    {
        private List<T> _discardPile = new List<T>();
        private Random rand;

        public ImmutableList<T> DiscardPile { get { return _discardPile.ToImmutableList(); } }

        private List<T> cards;

        public Deck(IEnumerable<T> startingCards, int randSeed) {
            cards = new List<T>(startingCards);
            rand = new Random(randSeed);
        }

        public Hand<T> CreateHand(int startingSize) {
            if (startingSize < 0)
                throw new ArgumentException("tryed creating hand with invalid staring size: " + startingSize);
            if (startingSize > cards.Count)
                throw new IndexOutOfRangeException("tryed creating hand with staring size: " + startingSize + "but deck size is: " + cards.Count);

            return new Hand<T>(this,startingSize);
        }

        public void DiscardCard(T card) {
            if (card == null)
                return;

            _discardPile.Add(card);
        }

        protected T TakeTopCard() {
            int index = rand.Next(0,cards.Count-1);

            T target = cards[index];
            cards.RemoveAt(index);
            return target;
        }

        public class Hand<T> where T : AbstractCard
        {
            public ImmutableList<T> Cards { get { return myCards.ToImmutableList(); } }

            private Deck<T> myDeck;
            private List<T> myCards = new List<T>();

            public Hand(Deck<T> deck, int startingSize) {
                myDeck = deck;
                DrawCard(startingSize);
            }

            public int Count { get { return myCards.Count; } }

            public void DrawCard(int numCards = 1)
            {
                if (numCards < 0)
                    throw new ArgumentException("Tryed to draw invalid number of cards: " + numCards);

                if (myDeck.cards.Count == 0)
                    return;

                if (myDeck.cards.Count < numCards)
                    DrawCard(myDeck.cards.Count);
                else
                    for (int i = 0; i < numCards; i++)
                    {
                        myCards.Add(myDeck.TakeTopCard());
                    }
            }

            public T Card(int index)
            {
                if (index < 0 || myCards.Count <= index)
                    throw new IndexOutOfRangeException();

                return myCards[index];
            }

            public bool CanPlay(int index)
            {
                if (index < 0 || myCards.Count <= index)
                    throw new IndexOutOfRangeException();

                CardContext context = new CardContext(); //TODO: take context as argument for playing cards

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

                T target = Card(index);
                RemoveAt(index);
                if (target.DiscardOnPlay == true)
                    myDeck.DiscardCard(target);
                target.Play();
                return true;
            }

        }
    }
}
