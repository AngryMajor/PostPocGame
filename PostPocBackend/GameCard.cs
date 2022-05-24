using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocBackend
{
    public class GameCard : ICardDeckable
    {
        public object CountActivations { get { return activations.Count; } }

        private List<Activation> activations = new List<Activation>();

        public GameCard(string name, List<List<GameAction>> activations) {

            foreach (List<GameAction> actionList in activations)
                this.activations.Add(new Activation(actionList));

        }

        public Activation Activations(int index) {
            return activations[index];
        }


        public class Activation
        {
            public object CountActions { get { return actions.Count; } }

            protected List<GameAction> actions;

            public Activation(List<GameAction> actionList) {
                actions = actionList;
            }

            public Activatable GetActivatable(IGameActionContext context) {
                if (context == null || context.dict.ContainsKey("test") == false || (bool)context.dict["test"] == false)
                    return null;

                return new Activatable(this);
            }

            public class Activatable {

                Activation parent;

                public Activatable(Activation parent) {
                    this.parent = parent;
                }

                public void Activate(IGameActionContext context) {
                    foreach (GameAction action in parent.actions)
                        action.GetDoable(context)?.Do(context);
                }

            }
        }
    }
}
