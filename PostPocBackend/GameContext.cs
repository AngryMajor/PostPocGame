using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel
{
    public class GameContext : IGameActionContext
    {
        public Dictionary<string, object> dict => throw new NotImplementedException();

        public GameWorld World => throw new NotImplementedException();
    }
}
