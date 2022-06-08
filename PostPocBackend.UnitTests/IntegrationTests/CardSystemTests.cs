using NUnit.Framework;
using System;
using System.Collections.Generic;
using Moq;
using PostPocModel.CardSystem;

namespace PostPocModel.UnitTests.IntegrationTests
{
    public class CardSystemTests
    {
        private const int StartingHandSize = 7;

        private Deck<PostPocCard> currDeck;
        private Deck<PostPocCard>.Hand<PostPocCard> currHand;

        private List<PostPocCard> StartingCards;

        private bool CardShouldPlay;
        private bool CardShouldDiscard;

        [SetUp]
        public void SetUp() {
            CardShouldPlay = true;

            StartingCards = new List<PostPocCard>() {
                new PostPocCard("First","First",x=>CardShouldPlay,true),
                new PostPocCard("First","First",x=>CardShouldPlay,false),
                new PostPocCard("First","First",x=>CardShouldPlay,false),
                new PostPocCard("First","First",x=>CardShouldPlay,false),
                new PostPocCard("First","First",x=>CardShouldPlay,false),
                new PostPocCard("First","First",x=>CardShouldPlay,false),
                new PostPocCard("First","First",x=>CardShouldPlay,false),
                new PostPocCard("First","First",x=>CardShouldPlay,false),
                new PostPocCard("First","First",x=>CardShouldPlay,false),
                new PostPocCard("First","First",x=>CardShouldPlay,false)
            };
            currDeck = new Deck<PostPocCard>(StartingCards,1);
            currHand = currDeck.CreateHand(StartingHandSize);
        }

        [Test]
        public void DrawCards()
        {
            Assert.AreEqual(StartingHandSize, currHand.Count);
            currHand.DrawCard();
            Assert.AreEqual(StartingHandSize + 1, currHand.Count);
            currHand.DrawCard(2);
            Assert.AreEqual(StartingHandSize + 3, currHand.Count);

            bool listsAreSame = true;
            int i = 0;
            while (listsAreSame && i < currHand.Count) {
                if (currHand.Card(i) != StartingCards[i])
                    listsAreSame = false;
                i++;
            }
            Assert.IsFalse(listsAreSame);
        }

        [Test]
        public void PlayCards_discard()
        {
            int targetCardIndex = 0;
            Assert.AreEqual(StartingHandSize, currHand.Count);
            PostPocCard targetCard = currHand.Card(targetCardIndex);
            targetCard.SetDiscardOnPlay(true);
            currHand.TryPlay(targetCardIndex);

            Assert.AreEqual(StartingHandSize-1, currHand.Count);
            Assert.Contains(targetCard, currDeck.DiscardPile);
        }

        [Test]
        public void PlayCards_destroy()
        {
            int targetCardIndex = 1;
            Assert.AreEqual(StartingHandSize, currHand.Count);
            PostPocCard targetCard = currHand.Card(targetCardIndex);
            targetCard.SetDiscardOnPlay(false);
            currHand.TryPlay(targetCardIndex);

            Assert.AreEqual(StartingHandSize - 1, currHand.Count);
            Assert.IsFalse(currDeck.DiscardPile.Contains(targetCard));
        }

        [Test]
        public void PlayCards_CantPlay()
        {
            CardShouldPlay = false;
            int targetCardIndex = 2;
            Assert.AreEqual(StartingHandSize, currHand.Count);
            PostPocCard targetCard = currHand.Card(targetCardIndex);
            currHand.TryPlay(targetCardIndex);

            Assert.AreEqual(StartingHandSize, currHand.Count);
            Assert.IsFalse(currDeck.DiscardPile.Contains(targetCard));
        }

        [Test]
        public void GetListOfCards() {
            Assert.NotNull( currHand.Cards);
            Assert.AreEqual(StartingHandSize, currHand.Cards.Count);
        }
    }
}
