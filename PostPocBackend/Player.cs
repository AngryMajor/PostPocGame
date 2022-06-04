using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace PostPocModel
{
    public class Player
    {

        protected List<IPlayableCard> _hand;
        public ImmutableList<IPlayableCard> Hand { get { return _hand.ToImmutableList(); } }
        protected IPlayableDeck Deck { get; set; }
        protected IPlayableGame Game { get; set; }
        protected int HandSizeLimit { get; private set; }

        public Player(IPlayableGame game, IPlayableDeck deck, int handSizeLimit = 7) {
            this.Deck = deck;
            this._hand = new List<IPlayableCard>();
            this.Game = game;
            this.HandSizeLimit = handSizeLimit;

            game.TurnEndEvent += OnEndOfTurn;

            DrawToHandLimit();
        }

        public void Draw()
        {
            _hand.Add(Deck.Draw());
        }
  
        public void Play(int cardIndex, int effectIndex) {
            if (cardIndex < 0 || cardIndex >= Hand.Count)
                throw new IndexOutOfRangeException("Card Index is out of bounds");

            Play(Hand[cardIndex], effectIndex);
        }

        public void Play(IPlayableCard card, int effectIndex) {
            if (card.IsPlayable(this.Game).Contains(effectIndex) == false)
                throw new IndexOutOfRangeException("Effect Index is Unplable");

            card.Play(effectIndex, this.Game);

            if(Hand.Contains(card))
                Remove(card);
        }


        protected void Remove(int cardIndex)
        {
            _hand.RemoveAt(cardIndex);
        }
        protected void Remove(IPlayableCard card)
        {
            _hand.Remove(card);
        }

        protected void OnEndOfTurn() {
            DrawToHandLimit();
        }

        private void DrawToHandLimit() {
            int numToDraw = HandSizeLimit - Hand.Count;
            for (int i = 0; i < numToDraw; i++)
            {
                Draw();
            }
        }

    }

    public interface IPlayableGame {
        event Action TurnEndEvent;
        public void EndTurn();
    }

    public interface IPlayableDeck {
        public IPlayableCard Draw();
        public void Discard(IPlayableCard card);
    }

    public interface IPlayableCard {
        public int NumEffects { get;}
        public void AsignDeck(IPlayableDeck deck);
        public List<int> IsPlayable(IPlayableGame game);
        public void Play(int effectIndex, IPlayableGame game);
    }
}
