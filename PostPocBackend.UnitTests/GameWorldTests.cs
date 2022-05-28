using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PostPocModel.UnitTests
{
    class GameWorldTests
    {

        GameWorld target;
        mockSettelmentBuilder settelmentBuilder = new mockSettelmentBuilder();

        [SetUp]
        public void Setup() {
            target = new GameWorld(settelmentBuilder);
        }

        [Test]
        public void AddSettelemntAction() {
            Dictionary<string, object> args = new Dictionary<string, object>();

            GameWorld.CreateSettelmentAction createSettelmentAction = new GameWorld.CreateSettelmentAction(args);
            MockContext context = new MockContext(target);
            context.dict.Add("settelmentName","test");

            Assert.AreEqual(0, target.SetelmentDict.Count);

            createSettelmentAction.GetDoable(context).Do(context);

            Assert.AreEqual(1, target.SetelmentDict.Count);
            Assert.NotNull(target.SetelmentDict["test"]);
        }

        [Test]
        public void AddSettelemntAction_MissingSettelmentName()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();

            GameWorld.CreateSettelmentAction createSettelmentAction = new GameWorld.CreateSettelmentAction(args);
            MockContext context = new MockContext(target);

            
            Assert.IsNull(createSettelmentAction.GetDoable(context));

        }

        public class MockContext : IGameActionContext
        {
            public MockContext(GameWorld target) {
                this.World = target; 
            }

            private Dictionary<string, object> _dict = new Dictionary<string, object>();
            public Dictionary<string, object> dict { get { return _dict; } }

            public GameWorld World { get; private set; }
        }


        public class mockSettelmentBuilder : ISettelmentBuilder
        {
            public AbstractSettelment Build(string name)
            {
                return new mockSettelment();
            }
        }

        public class mockSettelment : AbstractSettelment { 
        
        }
    }
}
