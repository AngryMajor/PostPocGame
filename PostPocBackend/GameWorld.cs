using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using System.Text;

namespace PostPocModel
{
    public class GameWorld
    {
        protected Dictionary<string, AbstractSettelment> _setelmentDict = new Dictionary<string, AbstractSettelment>();
        public ImmutableDictionary<string, AbstractSettelment> SetelmentDict { get { return _setelmentDict.ToImmutableDictionary(); } }

        private ISettelmentBuilder SettelmentBuilder;

        public GameWorld(ISettelmentBuilder builder) {
            this.SettelmentBuilder = builder;
        }

        protected void CreateSettelment(string settelmentName) {
            _setelmentDict.Add(settelmentName, SettelmentBuilder.Build(settelmentName));
        }

        public class CreateSettelmentAction : GameAction
        {
            public CreateSettelmentAction(Dictionary<string, object> args) : base(args) { }

            protected override bool ValidContext(IGameActionContext context)
            {
                return context.World != null && context.dict.ContainsKey("settelmentName");
            }

            protected override void Do(IGameActionContext context)
            {
                context.World.CreateSettelment((string)context.dict["settelmentName"]);
            }

           
        }
    }

    public abstract class AbstractSettelment { }

    public interface ISettelmentBuilder {
        public AbstractSettelment Build(String name);
    }

}
