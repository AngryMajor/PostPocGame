using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel.UnitTests
{
    class GameCard_Tests
    {

        GameCard target;

        [SetUp]
        public void Setup() {
            target = target = new GameCard("Name", new List<List<GameAction>> {
                 new List<GameAction>{ new TestAction(new Dictionary<string, object>()) }
                ,new List<GameAction> { new TestAction(new Dictionary<string, object>())
                    ,new TestAction(new Dictionary<string, object>()) }
            });
        }

        [Test]
        public void CreateCards_success()
        { 
            //testing the setup target
            Assert.AreEqual(2, target.CountActivations);
            Assert.AreEqual(1, target.Activations(0).CountActions);
            Assert.AreEqual(2, target.Activations(1).CountActions);

            //testing alternat GameCard
            GameAction test = new TestAction(new Dictionary<string, object>());
            test = new TestAction(new Dictionary<string, object>());
            target = new GameCard("Name", new List<List<GameAction>> {
                 new List<GameAction>{ test }
                ,new List<GameAction> { test,test }
                ,new List<GameAction> { test,test }

            });

            Assert.AreEqual(3, target.CountActivations);
            Assert.AreEqual(2, target.Activations(2).CountActions);
        }
        [Test]
        public void Play_1effect_success() {
            IGameActionContext context = new MockContext();
            context.dict.Add("test", true);

            target.Activations(0).GetActivatable(context)?.Activate(context);

            Assert.AreEqual(1, context.dict["testsRun"]);
        }
        [Test]
        public void Play_2effect_success()
        {
            IGameActionContext context = new MockContext();
            context.dict.Add("test", true);

            target.Activations(1).GetActivatable(context)?.Activate(context);

            Assert.AreEqual(2, context.dict["testsRun"]);
        }
        [Test]
        public void Play_1effect_incorrectContext()
        {
            IGameActionContext context = new MockContext();
            context.dict.Add("test", false);

            Assert.IsNull(target.Activations(0).GetActivatable(context));
        }
        [Test]
        public void Play_1effect_nullContext()
        {
            Assert.IsNull(target.Activations(0).GetActivatable(null));
        }


        public class MockContext : IGameActionContext
        {
            private Dictionary<string, object> _dict = new Dictionary<string, object>();
            public Dictionary<string, object> dict { get { return _dict; } }

            public GameWorld World => throw new NotImplementedException();
        }

    }
}
