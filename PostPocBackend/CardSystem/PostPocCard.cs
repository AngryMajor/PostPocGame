using System;
using System.Collections.Generic;
using System.Text;

namespace PostPocModel.CardSystem
{
    public class PostPocCard : AbstractCard
    {

        public static PostPocCardBuilder New(string name) {
            return new PostPocCardBuilder(name);
        }
        
        private Predicate<CardContext> canPlayPredicate;
        private string name;
        private string desctiption;

        private PostPocCard(string name, string Description, Predicate<CardContext> canPlayPredicate, bool DiscardOnPlay) {
            this.canPlayPredicate = canPlayPredicate;
            this.DiscardOnPlay = DiscardOnPlay;

            this.name = name;
            this.desctiption = Description;
        }

        public override bool CanPlay(CardContext context)
        {
            return canPlayPredicate(context);
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

        public class PostPocCardBuilder
        {

            protected string name;
            protected string description = "";
            protected Predicate<CardContext> canPlayPredicate = (x) => true;
            protected bool discardOnPlay = false;


            public PostPocCardBuilder(string name) { }

            public PostPocCardBuilder WithDescription(string Description) {
                this.description = Description;
                return this;
            }

            public PostPocCardBuilder WithPlayPredicate(Predicate<CardContext> canPlayPredicate) {
                this.canPlayPredicate = canPlayPredicate;
                return this;
            }

            public PostPocCardBuilder WithDiscardOnPlay(bool value) {
                this.discardOnPlay = value;
                return this;
            }

            public static implicit operator PostPocCard(PostPocCardBuilder b) { return new PostPocCard(b.name, b.description, b.canPlayPredicate, b.discardOnPlay); }
        }
    }
}
