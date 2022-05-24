using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PostPocBackend
{
    public class CardDeck<T> where T : class,ICardDeckable
    {
        public int CountCards { get { return StorageStructure.Count; } }
        public int CountDiscard { get { return DiscardStructure.Count; } }

        private List<T> StorageStructure;
        private List<T> DiscardStructure;
        private Random rand;


        public CardDeck() 
        {
            StorageStructure = new List<T>();
            DiscardStructure = new List<T>();
            rand = new Random();
        }

        public CardDeck(List<T> storageStructure, List<T> discardStructure, int random_seed)
        {
            StorageStructure = storageStructure;
            DiscardStructure = discardStructure;
            rand = new Random(random_seed);
        }

        public T DrawCard()
        {
            if (StorageStructure.Count == 0)
                return null;

            T topCard = ((List<T>)StorageStructure)[0];
            StorageStructure.RemoveAt(0);
            return topCard;
        }

        public void AddCard(T[] cardDeckables)
        {
            foreach (T card in cardDeckables)
                AddCard(card);
        }

        public void AddCard(T card)
        {
            if (card == null)
                return;

            StorageStructure.Add(card);
        }

        public void Discard(T card)
        {
            if (card == null)
                return;

            DiscardStructure.Add(card);
        }

        public void Reshuffle()
        {
            foreach (T card in DiscardStructure)
                AddCard(card);
            Shuffle();
        }

        public void Shuffle()
        {
            StorageStructure = StorageStructure.OrderBy(a => rand.Next()).ToList<T>();
        }
    }

    public interface ICardDeckable { }

}
