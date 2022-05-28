using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel
{
    public class GameContext : IGameActionContext
    {
        public GameContext(GameWorld target)
        {
            this.World = target;
        }

        private Dictionary<string, object> _dict = new Dictionary<string, object>();
        public Dictionary<string, object> dict { get { return _dict; } }

        public GameWorld World { get; private set; }
    }
}
