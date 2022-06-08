using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel
{
    public class Game : IPlayableGame
    {

        public int NumStartingCardSlots { get; private set; }

        public event Action TurnEndEvent;

        public void EndTurn()
        {
            throw new NotImplementedException();
        }


    }
}
