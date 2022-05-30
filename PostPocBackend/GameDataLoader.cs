using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel
{
    public abstract class GameDataLoader
    {
        public abstract IEnumerable<IGameCard> GetGameCards();
    }

    public class HardCodeGameDataLoader : GameDataLoader
    {
        public override IEnumerable<IGameCard> GetGameCards()
        {
            List<IGameCard> cardList = new List<IGameCard> {
                new GameCard("Create Settelment","this card creates a settelemnt", new List<List<GameAction>>
                { new List<GameAction>
                {
                    new ViewControler.GetPlayerInputAction(new Dictionary<string, object>{ {"Message","Name This Settelment"},{"InputField","SettelmentName"} }),
                    new GameWorld.CreateSettelmentAction(new Dictionary<string, object>{ })
                }
                })
            };

            return cardList;
        }
    }
}
