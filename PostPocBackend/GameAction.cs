using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocBackend
{
    public abstract class GameAction
    {
        public GameAction(string[] Args) { 
            
        }

        /// <summary>
        /// returns an object with Do method to execute action. Returns null if context given prohibits the action to be done
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract IGameActionDoable GetDoable(IGameActionContext context);


        protected abstract class Doable : IGameActionDoable {
            public abstract void Do(IGameActionContext context);
        }
    }

    public interface IGameActionDoable {
        public void Do(IGameActionContext context);
    }
    public interface IGameActionContext {
        public Dictionary<String, Object> dict { get; }
    }

    public class TestAction : GameAction
    {
        public TestAction(string[] Args) : base(Args) { }

        public override IGameActionDoable GetDoable(IGameActionContext context)
        {
            throw new NotImplementedException();
        }

        private class Doable : IGameActionDoable {
            public void Do(IGameActionContext context) {
                throw new NotImplementedException();
            }
        }
    }
}
