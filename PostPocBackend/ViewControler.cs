using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel
{

    public static class ViewControler
    {
        public static event Func<string,string> InputPopupEvent;


        /// <summary>
        /// Action for card to get user text input, Note a card can not have multipul player input actions with the sanem inputfiled name
        /// </summary>
        public class GetPlayerInputAction : GameAction
        {
            private string _message;
            private string _InputField;

            public GetPlayerInputAction(Dictionary<string,object> args) : base(args){
                Helper.CheckForMissingArgument("Message", args);
                Helper.CheckForMissingArgument("InputField", args);

                _message = (string)args["Message"];
                _InputField = (string)args["InputField"];
            }

            protected override void Do(IGameActionContext context)
            {                
                string userInput = InputPopupEvent(_message);
                context.dict.Add(_InputField, userInput);
            }

            protected override bool ValidContext(IGameActionContext context)
            {
                return InputPopupEvent != null;
            }
        }

    }
}
