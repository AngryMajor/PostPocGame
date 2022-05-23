using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocBackend.UnitTests
{
    class GameAction_Tests
    {

        [SetUp]
        public void Setup()
        {
            testTarget = new TestAction(new string[0]);
        }

        #region TestAction

        TestAction testTarget;



        [Test]
        public void TestAction_GetDoable_correctContext() {
            IGameActionContext context = new MockContext();
            context.dict.Add("test", true);

            Assert.IsNotNull(testTarget.GetDoable(context));
        }

        public void TestAction_GetDoable_incorrectContext()
        {
            IGameActionContext context = new MockContext();
            context.dict.Add("test", false);

            Assert.IsNull(testTarget.GetDoable(context));
        }

        public void TestAction_GetDoable_nullContext()
        {
            Assert.IsNull(testTarget.GetDoable(null));
        }

        [Test]
        public void TestAction_Do_correctContext()
        {
            IGameActionContext context = new MockContext();
            context.dict.Add("test", true);


            testTarget.GetDoable(context)?.Do(context);
            Assert.Contains(test, context.dict);
            Assert.AreEqual(false, context.dict["test"]);

        }

        public class MockContext : IGameActionContext
        {
            private Dictionary<string, object> _dict = new Dictionary<string, object>();
            public Dictionary<string, object> dict { get { return _dict; } }
        }

        #endregion

    }
}
