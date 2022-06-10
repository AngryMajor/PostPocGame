﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Moq;
using PostPocModel.CardSystem;

namespace PostPocModel.UnitTests.CardSystem
{
    public class HandTests
    {
        private Deck<MockCard> deck;
        private Deck<MockCard>.Hand target;

        [SetUp]
        public void Setup() {

            List<MockCard> cards = new List<MockCard>() {
                new MockCard(true),
                new MockCard(true),
                new MockCard(true),
                new MockCard(true),
                new MockCard(true),
                new MockCard(true)
            };
            deck = new Deck<MockCard>(cards, 1);
            target = deck.CreateHand(0);
        }


        [Test]
        public void DrawCard_success_enoughCards() {
            Assert.AreEqual(0, target.Count);
            target.DrawCard(0);
            target.DrawCard();
            Assert.AreEqual(1, target.Count);
            target.DrawCard(2);
            Assert.AreEqual(3, target.Count);
            target.DrawCard(3);
            Assert.AreEqual(6, target.Count);
        }

        [Test]
        public void DrawCard_fail_NegativeNumberGiven()
        {
            Assert.Throws<ArgumentException>(delegate () { target.DrawCard(-1); });
        }

        [Test]
        public void DrawCard_Fail_TotalyEmptyDeck()
        {
            List<MockCard> cards = new List<MockCard>();
            deck = new Deck<MockCard>(cards, 1);//create empty deck
            target = deck.CreateHand(0);

            target.DrawCard();
            Assert.AreEqual(0, target.Count);

            target.DrawCard(2);
            Assert.AreEqual(0, target.Count);

            target.DrawCard(3);
            Assert.AreEqual(0, target.Count);
        }

        [Test]
        public void DrawCard_PartialSuccess_TooFewCardsInDeckTotal()
        {
            target.DrawCard(7);//target deck has 6 cards
            Assert.AreEqual(6, target.Count);
        }

        [Test]
        public void DrawCard_Success_InsuficentDrawSufficentTotal()
        {
            deck.DiscardCard(new MockCard(true));
            target.DrawCard(7);//6 cards starting in main deck
            Assert.AreEqual(7, target.Count);

            target.TryPlay(0);
            target.TryPlay(0);
            target.DrawCard(2);
            Assert.AreEqual(7, target.Count);

            target.TryPlay(0);
            target.TryPlay(0);
            target.TryPlay(0);
            target.DrawCard(3);
            Assert.AreEqual(7, target.Count);

        }
        [Test]
        public void DrawCard_PartialSuccess_insuficentDrawInsuficentTotal()
        {
            deck.DiscardCard(new MockCard(true));
            target.DrawCard(8);
            Assert.AreEqual(7, target.Count);
        }

        [Test]
        public void CanPlayCard_success() {
            target.DrawCard(3);

            Assert.AreEqual(true, target.CanPlay(0));
            Assert.AreEqual(true, target.CanPlay(1));
            target.Card(2).canBePlayed = false;
            Assert.AreEqual(false, target.CanPlay(2));
        }

        [Test]
        public void CanPlayCard_IndexOutOfRange()
        {
            target.DrawCard(3);

            Assert.Throws<IndexOutOfRangeException>(delegate () { target.CanPlay(-1); });
            Assert.Throws<IndexOutOfRangeException>(delegate () { target.CanPlay(3); });

        }

        [Test]
        public void GetCard_success()
        {
            target.DrawCard(3);

            Assert.NotNull(target.Card(0));
            Assert.NotNull(target.Card(1));
            Assert.NotNull(target.Card(2));
        }

        [Test]
        public void GetCard_IndexOutOfRange()
        {
            target.DrawCard(3);

            Assert.Throws<IndexOutOfRangeException>(delegate () { target.Card(-1); });
            Assert.Throws<IndexOutOfRangeException>(delegate () { target.Card(3); });

        }

        [Test]
        public void TryPlay_Destroy_success()
        {
            target.DrawCard(3);
            MockCard card1 = (MockCard)target.Card(0);
            MockCard card2 = (MockCard)target.Card(1);
            MockCard card3 = (MockCard)target.Card(2);


            Assert.NotNull(target.TryPlay(2));
            Assert.AreEqual(2, target.Count);

            Assert.IsFalse(card1.beenPlayed);
            Assert.IsFalse(card2.beenPlayed);
            Assert.IsTrue(card3.beenPlayed);


            Assert.NotNull(target.TryPlay(1));
            Assert.AreEqual(1, target.Count);

            Assert.IsFalse(card1.beenPlayed);
            Assert.IsTrue(card2.beenPlayed);
            Assert.IsTrue(card3.beenPlayed);

            Assert.NotNull(target.TryPlay(0));
            Assert.AreEqual(0, target.Count);

            Assert.IsTrue(card1.beenPlayed);
            Assert.IsTrue(card2.beenPlayed);
            Assert.IsTrue(card3.beenPlayed);

        }
        [Test]
        public void TryPlay_Discard()
        {
            target.DrawCard(3);

            MockCard card = (MockCard)target.Card(2);
            card.setDiscardOnPlay = true;

            Assert.NotNull(target.TryPlay(2));
            Assert.AreEqual(2, target.Count);
            Assert.Contains(card, deck.DiscardPile);

        }

        [Test]
        public void TryPlay_IndexOutOfRange()
        {
            target.DrawCard(3);

            Assert.Throws<IndexOutOfRangeException>(delegate () { target.TryPlay(-1); });
            Assert.Throws<IndexOutOfRangeException>(delegate () { target.TryPlay(3); });

        }

        [Test]
        public void TryPlay_NotAllowedPlay()
        {
            target.DrawCard(1);
            ((MockCard)target.Card(0)).canBePlayed = false;
            Assert.IsFalse(target.TryPlay(0));
        }

        private class MockCard : AbstractCard
        {
            public bool canBePlayed = true;
            public bool beenPlayed = false;
            public bool setDiscardOnPlay { get { return DiscardOnPlay; } set { DiscardOnPlay = value; } }

            public MockCard(bool discardOnPlay) {
                this.DiscardOnPlay = discardOnPlay;
            }

            public override bool CanPlay(CardContext context)
            {
                return canBePlayed;
            }

            public override void Play()
            {
                beenPlayed = true;
            }
        }

        private class MockContext : CardContext { }

    }
}
