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
            if (context == null || context.dict.ContainsKey("test") == false || ((bool)context.dict["test"]) == false)
                return null;

            else
                return new Doable();
        }

        private class Doable : IGameActionDoable {
            public void Do(IGameActionContext context) {
                if (context.dict.ContainsKey("testsRun") == false)
                    context.dict.Add("testsRun", 0);

                context.dict["testsRun"] = (int)context.dict["testsRun"] + 1;
            }
        }
    }
}
