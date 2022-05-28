using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel
{
    public abstract class GameAction
    {
        public GameAction(Dictionary<string,object> Args) { 
            
        }

        /// <summary>
        /// returns an object with Do method to execute action. Returns null if context given prohibits the action to be done
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IGameActionDoable GetDoable(IGameActionContext context) {
            if (context == null || ValidContext(context) == false)
                return null;
            else
                return new Doable(this);
        }

        protected abstract bool ValidContext(IGameActionContext context);
        protected abstract void Do(IGameActionContext context);

        protected class Doable : IGameActionDoable {

            GameAction myAction;

            public Doable(GameAction action) {
                myAction = action;
            }

            public void Do(IGameActionContext context) {
                myAction.Do(context);
            }
        }
    }

    public interface IGameActionDoable {
        public void Do(IGameActionContext context);
    }
    public interface IGameActionContext {
        public Dictionary<String, Object> dict { get; }
        public GameWorld World { get; }
    }


    public class TestAction : GameAction
    {
        public TestAction(Dictionary<string,object> Args) : base(Args) { }

        protected override void Do(IGameActionContext context)
        {
            if (context.dict.ContainsKey("testsRun") == false)
                context.dict.Add("testsRun", 0);

            context.dict["testsRun"] = (int)context.dict["testsRun"] + 1;
        }

        protected override bool ValidContext(IGameActionContext context)
        {
            return context.dict.ContainsKey("test") && ((bool)context.dict["test"]) == true;
        }
    }
}
