using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel.CardSystem
{
    public class CardDataBase<T> where T : IGameCard
    {

        ICardBuilder<T> cardBulider;

        public CardDataBase(string sourcePath, ICardBuilder<T> cardBulider) {
            this.cardBulider = cardBulider;
        }

        public IEnumerable<T> GetStartingDeck()
        {
            List<T> cardList = new List<T>();

            cardList.Add(cardBulider.New("test1", "this is a test card")
                .WithActivation(new CardActivation { 
                    new GameWorld.CreateSettelmentAction(new Dictionary<string, object> { }) 
                })
                .WithActivation(new CardActivation {
                    new GameWorld.CreateSettelmentAction(new Dictionary<string, object> { })
                })
            );

            cardList.Add(cardBulider.New("test2", "this is a test card"));
            cardList.Add(cardBulider.New("test3", "this is a test card"));
            cardList.Add(cardBulider.New("test4", "this is a test card"));


            return cardList;
        }

        public T GetCard(string name) {
            throw new NotImplementedException();
        }

    }

    public abstract class ICardBuilder<p> where p : IGameCard{
        public abstract ICardBuilder<p> New(string name, string descriptoin);
        public abstract ICardBuilder<p> WithActivation(List<GameAction> actions);

        protected abstract p get();

        public static implicit operator p (ICardBuilder<p> target) => target.get();

    }
}
