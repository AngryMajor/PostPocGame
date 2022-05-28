using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace PostPocModel.UnitTests
{
    public class ViewControlerActionTests
    {
        [Test]
        public void GetPlayerInputAction_createAndRun() { 
            
            ViewControler.InputPopupEvent += x =>  "TestReturnString";

            Dictionary<string,object> args = new Dictionary<string, object>();
            args.Add("Message", "Test");
            args.Add("InputField", "Test");

            ViewControler.GetPlayerInputAction action = new ViewControler.GetPlayerInputAction(args);
            MockContext context = new MockContext();

            action.GetDoable(context).Do(context);

            Assert.Contains("Test", context.dict.Keys);
            Assert.AreEqual("TestReturnString", context.dict["Test"]);

            //tear down
            ViewControler.InputPopupEvent -= x => "TestReturnString";

        }

        [Test]
        public void GetPlayerInputAction_MissingParams()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("Message", "Test");

            Assert.Throws(typeof(MissingFieldException), () => new ViewControler.GetPlayerInputAction(args));

        }


        public class MockContext : IGameActionContext
        {
            private Dictionary<string, object> _dict = new Dictionary<string, object>();
            public Dictionary<string, object> dict { get { return _dict; } }

            public GameWorld World => throw new NotImplementedException();
        }
    }
}
