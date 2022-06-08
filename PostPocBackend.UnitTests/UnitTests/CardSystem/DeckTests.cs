using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Moq;
using PostPocModel.CardSystem;

namespace PostPocModel.UnitTests.CardSystem
{
    public class DeckTests
    {
        private Deck<MockCard> target;

        [SetUp]
        public void Setup() {

            List<MockCard> cards = new List<MockCard>() {
                new MockCard(),
                new MockCard(),
                new MockCard(),
                new MockCard(),
                new MockCard(),
                new MockCard()
            };
            target = new Deck<MockCard>(cards, 1);
        }

        [Test]
        public void CreateHand_success() {
            Deck<MockCard>.Hand<MockCard> hand1 = target.CreateHand(0);
            Deck<MockCard>.Hand<MockCard> hand2 = target.CreateHand(1);
            Deck<MockCard>.Hand<MockCard> hand3 = target.CreateHand(2);
            Deck<MockCard>.Hand<MockCard> hand4 = target.CreateHand(3);

            Assert.AreEqual(0, hand1.Count);
            Assert.AreEqual(1, hand2.Count);
            Assert.AreEqual(2, hand3.Count);
            Assert.AreEqual(3, hand4.Count);

        }

        [Test]
        public void CreateHand_NegativeStartingSize()
        {
            Assert.Throws<ArgumentException>(delegate() { target.CreateHand(-1); });
        }

        [Test]
        public void CreateHand_NotEnoughCards()
        {
            Assert.Throws<IndexOutOfRangeException>(delegate () { target.CreateHand(7); });
        }

        [Test]
        public void DiscardCard_success() {
            MockCard card = new MockCard();
            target.DiscardCard(card);
            Assert.Contains(card, target.DiscardPile);
        }

        [Test]
        public void DiscardCard_NullCard()
        {
            target.DiscardCard(null);
            Assert.IsEmpty(target.DiscardPile);
        }

        [Test]
        public void DrawCard() { 
        
        }

        private class MockCard : AbstractCard
        {
            public override bool CanPlay(CardContext context)
            {
                return true;
            }

            public override void Play()
            {
                throw new NotImplementedException();
            }
        }
    }
}
