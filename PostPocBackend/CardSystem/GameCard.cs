﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel.CardSystem
{
    public class GameCard : IGameCard
    {

        public string Name { get; private set; }
        public string Description { get; private set; }
        public object CountActivations { get { return activations.Count; } }

        private List<Activation> activations = new List<Activation>();

        public GameCard(string name,string description, List<List<GameAction>> activations) {
            this.Name = name;
            this.Description = description;

            if(activations != null)
                foreach (List<GameAction> actionList in activations)
                    this.activations.Add(new Activation(actionList));

        }

        public Activation Activations(int index) {
            return activations[index];
        }

        public override string ToString() {
            return this.Name;
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

        public class Builder : ICardBuilder<GameCard>
        {
            protected string name;
            protected string description;
            protected List<List<GameAction>> activations = new List<List<GameAction>>();

            public override ICardBuilder<GameCard> New(string name, string descriptoin)
            {
                this.name = name;
                this.description = descriptoin;
                return this;
            }

            public override ICardBuilder<GameCard> WithActivation(List<GameAction> actions)
            {
                activations.Add(actions);
                return this;
            }

            protected override GameCard get()
            {
                return new GameCard(name, description,activations);
            }

        }
    }

    public class CardActivation : List<GameAction> { }

    public abstract class IGameCard : ICardDeckable
    { }
}
