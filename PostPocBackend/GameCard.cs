using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocBackend
{
    public class GameCard : ICardDeckable
    {
        public object CountActivations { get; }


        public GameCard(string name, List<List<GameAction>> activations) {
            throw new NotImplementedException();

        }

        public Activation Activations(int index) {
            throw new NotImplementedException();
        }


        public class Activation
        {
            public object CountActions { get; }

            public Activatable GetActivatable(IGameActionContext context) {
                throw new NotImplementedException();
            }

            public class Activatable {

                public void Activate(IGameActionContext context) {
                    throw new NotImplementedException();
                }

            }
        }
    }
}
