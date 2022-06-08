using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel.CardSystem
{
    public class PostPocCard : AbstractCard
    {
        
        private Predicate<CardContext> canPlayCheck;
        private string name;
        private string desctiption;

        public PostPocCard(string name, string Description, Predicate<CardContext> canPlayCheck, bool DiscardOnPlay) {
            this.canPlayCheck = canPlayCheck;
            this.DiscardOnPlay = DiscardOnPlay;

            this.name = name;
            this.desctiption = Description;
        }

        public override bool CanPlay(CardContext context)
        {
            return canPlayCheck(context);
        }

        public override void Play()
        {
            
        }

        public override string ToString()
        {
            return name +": "+ desctiption;
        }

        public void SetDiscardOnPlay(bool value) {
            DiscardOnPlay = value;
        }
    }
}
