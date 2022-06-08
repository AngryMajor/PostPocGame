using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel.CardSystem
{
    public abstract class AbstractCard
    {
        public bool DiscardOnPlay { get; protected set; }

        public abstract bool CanPlay(CardContext context);
        public abstract void Play();

    }
}
