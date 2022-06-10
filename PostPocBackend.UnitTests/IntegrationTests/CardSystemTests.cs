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
        private Deck<PostPocCard>.Hand currHand;

        private List<PostPocCard> StartingCards;

        private bool CardShouldPlay;
        private bool CardShouldDiscard;

        [SetUp]
        public void SetUp() {
            CardShouldPlay = true;

            StartingCards = new List<PostPocCard>() {
                new PostPocCard("First","First",x=>CardShouldPlay,true),
                new PostPocCard("First","First",x=>CardShouldPlay,true),
                new PostPocCard("First","First",x=>CardShouldPlay,true),
                new PostPocCard("First","First",x=>CardShouldPlay,true),
                new PostPocCard("First","First",x=>CardShouldPlay,true),
                new PostPocCard("First","First",x=>CardShouldPlay,true),
                new PostPocCard("First","First",x=>CardShouldPlay,true),
                new PostPocCard("First","First",x=>CardShouldPlay,true),
                new PostPocCard("First","First",x=>CardShouldPlay,true),
                new PostPocCard("First","First",x=>CardShouldPlay,true)
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
            Assert.IsFalse(targetCard == currHand.Card(targetCardIndex));


            targetCard = currHand.Card(targetCardIndex);
            targetCard.SetDiscardOnPlay(true);
            currHand.TryPlay(targetCardIndex);

            Assert.AreEqual(StartingHandSize - 2, currHand.Count);
            Assert.Contains(targetCard, currDeck.DiscardPile);
            Assert.IsFalse(targetCard == currHand.Card(targetCardIndex));

            targetCard = currHand.Card(targetCardIndex);
            targetCard.SetDiscardOnPlay(true);
            currHand.TryPlay(targetCardIndex);

            Assert.AreEqual(StartingHandSize - 3, currHand.Count);
            Assert.Contains(targetCard, currDeck.DiscardPile);
            Assert.IsFalse(targetCard == currHand.Card(targetCardIndex));

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
            Assert.IsFalse(targetCard == currHand.Card(targetCardIndex));

            targetCard = currHand.Card(targetCardIndex);
            targetCard.SetDiscardOnPlay(false);
            currHand.TryPlay(targetCardIndex);

            Assert.AreEqual(StartingHandSize - 2, currHand.Count);
            Assert.IsFalse(currDeck.DiscardPile.Contains(targetCard));
            Assert.IsFalse(targetCard == currHand.Card(targetCardIndex));


            targetCard = currHand.Card(targetCardIndex);
            targetCard.SetDiscardOnPlay(false);
            currHand.TryPlay(targetCardIndex);

            Assert.AreEqual(StartingHandSize - 3, currHand.Count);
            Assert.IsFalse(currDeck.DiscardPile.Contains(targetCard));
            Assert.IsFalse(targetCard == currHand.Card(targetCardIndex));

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

        [Test]
        public void DrawCard_DiscardReshuffled() {
            Assert.AreEqual(7, currHand.Count);
            Assert.IsEmpty(currDeck.DiscardPile);

            currHand.TryPlay(0);
            currHand.TryPlay(0);
            currHand.TryPlay(0);
            currHand.TryPlay(0);

            Assert.AreEqual(4,currDeck.DiscardPile.Count);

            currHand.DrawCard(4);

            Assert.IsEmpty(currDeck.DiscardPile);
            Assert.AreEqual(7, currHand.Count);
        }
    }
}
