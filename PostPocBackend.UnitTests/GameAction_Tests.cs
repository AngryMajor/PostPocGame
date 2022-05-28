using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel.UnitTests
{
    class GameAction_Tests
    {

        [SetUp]
        public void Setup()
        {
            testTarget = new TestAction(new Dictionary<string, object>());
        }

        #region TestAction

        TestAction testTarget;

        [Test]
        public void TestAction_GetDoable_correctContext() {
            IGameActionContext context = new MockContext();
            context.dict.Add("test", true);

            Assert.IsNotNull(testTarget.GetDoable(context));
        }
        [Test]
        public void TestAction_GetDoable_incorrectContext()
        {
            IGameActionContext context = new MockContext();
            context.dict.Add("test", false);

            Assert.IsNull(testTarget.GetDoable(context));
        }
        [Test]
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
            Assert.Contains("testsRun", context.dict.Keys);
            Assert.AreEqual(1, context.dict["testsRun"]);
        }

        public class MockContext : IGameActionContext
        {
            private Dictionary<string, object> _dict = new Dictionary<string, object>();
            public Dictionary<string, object> dict { get { return _dict; } }

            public GameWorld World => throw new NotImplementedException();
        }

        #endregion

    }
}
