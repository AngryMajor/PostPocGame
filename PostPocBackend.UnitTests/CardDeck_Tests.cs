using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PostPocModel.UnitTests
{
    public class CardDeck_Tests
    {

        CardDeck<MockCard> target;
        List<MockCard> storageStructure;
        List<MockCard> DiscardStructure;


        [SetUp]
        public void Setup()
        {
            storageStructure = new List<MockCard>();
            DiscardStructure = new List<MockCard>();
            int Random_seed = 20;

            target = new CardDeck<MockCard>(storageStructure, DiscardStructure, Random_seed);
        }

        [Test]
        public void CountCards_success()
        {
            MockCard card1 = new MockCard();
            target.AddCard(card1);
            Assert.IsTrue(target.CountCards == 1);

            target.AddCard(card1);
            Assert.IsTrue(target.CountCards == 2);

            target.AddCard(card1);
            Assert.IsTrue(target.CountCards == 3);
        }
        [Test]
        public void AddCard_NullCard_noAction()
        {
            MockCard cardNull = null;
            target.AddCard(cardNull);
            Assert.IsTrue(target.CountCards == 0);
            Assert.IsNull(target.DrawCard());
        }
        [Test]
        public void DrawCard_AddCard1DrawCard1_success()
        {
            MockCard card1 = new MockCard();
            target.AddCard(card1);

            MockCard card = target.DrawCard();
            Assert.IsTrue(card == card1);
        }
        [Test]
        public void DrawCard_AddCard2DrawCard2_success()
        {
            MockCard card1 = new MockCard();
            MockCard card2 = new MockCard();
            MockCard[] drawnArray = new MockCard[2];


            target.AddCard(new MockCard[] { card1, card2 });


            drawnArray[0] = target.DrawCard();
            drawnArray[1] = target.DrawCard();

            Assert.IsNotNull(drawnArray[0]);
            Assert.IsNotNull(drawnArray[1]);

            Assert.IsTrue(drawnArray[0] == card1 || drawnArray[1] == card1);
            Assert.IsTrue(drawnArray[0] == card2 || drawnArray[1] == card2);

        }
        [Test]
        public void DrawCard_deckEmpty()
        {
            Assert.IsNull(target.DrawCard());
        }
        [Test]
        public void DrawCard_AddCard1DrawCard2_deckEmpty()
        {
            MockCard card1 = new MockCard();
            target.AddCard(card1);
            target.DrawCard();
            Assert.IsNull(target.DrawCard());
        }
        [Test]
        public void DiscardCard_DiscardCard1_success()
        {
            Assert.IsTrue(DiscardStructure.Count == 0);

            MockCard card1 = new MockCard();
            target.Discard(card1);

            Assert.IsTrue(target.CountDiscard == 1);
            Assert.Contains(card1, DiscardStructure);
        }
        [Test]
        public void DiscardCard_DiscardCard2_success()
        {
            Assert.IsTrue(DiscardStructure.Count == 0);

            MockCard card1 = new MockCard();
            MockCard card2 = new MockCard();

            target.Discard(card1);
            target.Discard(card2);

            Assert.IsTrue(target.CountDiscard == 2);
            Assert.Contains(card1, DiscardStructure);
            Assert.Contains(card2, DiscardStructure);

        }
        [Test]
        public void DiscardCard_NullCard_noAction()
        {
            MockCard cardNull = null;
            target.Discard(cardNull);
            Assert.IsTrue(target.CountDiscard == 0);
        }
        [Test]
        public void CountDiscard_success()
        {
            MockCard card1 = new MockCard();
            target.Discard(card1);
            Assert.IsTrue(target.CountDiscard == 1);

            target.Discard(card1);
            Assert.IsTrue(target.CountDiscard == 2);

            target.Discard(card1);
            Assert.IsTrue(target.CountDiscard == 3);
        }
        [Test]
        public void Shuffle_success()
        {
            MockCard card1 = new MockCard("a");
            MockCard card2 = new MockCard("b");
            MockCard card3 = new MockCard("c");
            MockCard card4 = new MockCard("d");
            MockCard card5 = new MockCard("e");


            target.AddCard(new MockCard[] { card1, card2, card3, card4, card5 });

            List<MockCard> List1 = new List<MockCard>(storageStructure);
            List<MockCard> List2;
            List<MockCard> List3;

            target.Shuffle();
            List2 = new List<MockCard> { target.DrawCard(), target.DrawCard(), target.DrawCard(), target.DrawCard(), target.DrawCard() };
            foreach (MockCard card in List2)
                target.AddCard(card);
            
            target.Shuffle();
            List3 = new List<MockCard> { target.DrawCard(), target.DrawCard(), target.DrawCard(), target.DrawCard(), target.DrawCard() };

            Assert.IsFalse(List1.SequenceEqual(List2));
            Assert.IsFalse(List2.SequenceEqual(List3));
            Assert.IsFalse(List1.SequenceEqual(List3));

        }
        [Test]
        public void Reshuffle_Discard2_success()
        {
            Assert.IsTrue(DiscardStructure.Count == 0);

            MockCard card1 = new MockCard();
            MockCard card2 = new MockCard();

            target.Discard(card1);
            target.Discard(card2);
            target.Reshuffle();

            Assert.IsTrue(target.CountCards == 2);
            Assert.Contains(card1, storageStructure);
            Assert.Contains(card2, storageStructure);

        } 
    }


public class MockCard : ICardDeckable {

        private string name;

        public MockCard() { }

        public MockCard(string name) {
            this.name = name;
        }

    }
}