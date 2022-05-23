using System;
using System.Collections;
using System.Collections.Generic;

namespace PostPocBackend
{
    public class CardDeck<T> where T : class,ICardDeckable
    {
        public int CountCards { get; set; }
        public int CountDiscard { get; set; }

        public CardDeck() 
        {

        }

        public CardDeck( ICollection<T> storageStructure, ICollection<T> discardStructure, int random_seed)
        {

        }

        public T DrawCard()
        {
            throw new NotImplementedException();
        }

        public void AddCard(ICardDeckable[] cardDeckables)
        {
            foreach (ICardDeckable card in cardDeckables)
                AddCard(card);
        }

        public void AddCard(ICardDeckable card1)
        {
            throw new NotImplementedException();
        }

        public void Discard(ICardDeckable card1)
        {
            throw new NotImplementedException();
        }

        public void Reshuffle()
        {
            throw new NotImplementedException();
        }

        public void Shuffle()
        {
            throw new NotImplementedException();
        }
    }

    public interface ICardDeckable { }

}
