using NUnit.Framework;
using System;
using System.Collections.Generic;
using Moq;

namespace PostPocModel.CardSystem.UnitTests
{
    public class Player_Tests {

        public Player target;
        private Mock<IPlayableDeck> deck = new Mock<IPlayableDeck>();
        private Mock<IPlayableGame> game = new Mock<IPlayableGame>();

        private List<Mock<IPlayableCard>> cards;

        [OneTimeSetUp]
        public void setuponce() {
            game.Setup(p => p.EndTurn()).Raises(p => p.TurnEndEvent += null);
            
        }

        [SetUp]
        public void SetUp() 
        {
            cards = new List<Mock<IPlayableCard>>();
            for (int i = 0; i < 10; i++)
            {
                cards.Add(new Mock<IPlayableCard>());
                cards[i].Setup(p => p.IsPlayable(It.IsAny<IPlayableGame>())).Returns(new List<int>() { 0,1});
            }
            deck.SetupSequence(p => p.Draw()).Returns(cards[0].Object)
                                            .Returns(cards[1].Object)
                                            .Returns(cards[2].Object)
                                            .Returns(cards[3].Object)
                                            .Returns(cards[4].Object)
                                            .Returns(cards[5].Object)
                                            .Returns(cards[6].Object)
                                            .Returns(cards[7].Object)
                                            .Returns(cards[8].Object)
                                            .Returns(cards[9].Object);

            target = new Player(game.Object, deck.Object);
        }

        [Test]
        public void Draw_success() {
            Assert.AreEqual(7, target.Hand.Count);
            target.Draw();
            Assert.AreEqual(8, target.Hand.Count);

        }

        [Test]
        public void Constructor_DrawToHandSize() {
            Assert.AreEqual(7, target.Hand.Count);
            target = new Player(game.Object, deck.Object, 4);
            Assert.AreEqual(4, target.Hand.Count);
        }

        [Test]
        public void OnEndOfTurn_DrawToHandSize() {
            target.Play(0,0);
            Assert.AreEqual(6, target.Hand.Count);
            game.Object.EndTurn();
            Assert.AreEqual(7, target.Hand.Count);

        }

        [Test]
        public void Discard_CardIndex_success() {
            Mock<IPlayableCard> mockCard = new Mock<IPlayableCard>();
            deck.Setup(p => p.Draw()).Returns(mockCard.Object);
            target.Draw();
            Assert.IsTrue(target.Hand.Contains(mockCard.Object));
            target.Play(target.Hand.Count-1,0);
            Assert.IsFalse(target.Hand.Contains(mockCard.Object));
        }

        [Test]
        public void Discard_Card_success() {
            Mock<IPlayableCard> mockCard = new Mock<IPlayableCard>();
            deck.Setup(p => p.Draw()).Returns(mockCard.Object);
            target.Draw();
            Assert.IsTrue(target.Hand.Contains(mockCard.Object));
            target.Play(mockCard.Object,0);
            Assert.IsFalse(target.Hand.Contains(mockCard.Object));
        }

        [Test]
        public void PlayCard_CardIndex_success() {
            target.Play(0,0);
            cards[0].Verify(p => p.Play(0,game.Object), Times.Once);
            Assert.IsFalse(target.Hand.Contains(cards[0].Object));

            target.Play(2, 1);
            cards[3].Verify(p => p.Play(1, game.Object), Times.Once);
            Assert.IsFalse(target.Hand.Contains(cards[3].Object));

        }

        [Test]
        public void PlayCard_CardIndex_InvalidEffectIndex() {
            cards[0].Setup(p => p.NumEffects).Returns(1);
            Assert.Throws<IndexOutOfRangeException>(() => target.Play(0, 2));
        }

        [Test]
        public void PlayCard_CardIndex_InvalidCardIndex() {
            Assert.Throws<IndexOutOfRangeException>(() => target.Play(11, 2));
        }

        [Test]
        public void PlayCard_Card_success() {
            target.Play(cards[0].Object, 0);
            cards[0].Verify(p => p.Play(0, game.Object), Times.Once);
            Assert.IsFalse(target.Hand.Contains(cards[0].Object));

            target.Play(cards[3].Object, 1);
            cards[3].Verify(p => p.Play(1, game.Object), Times.Once);
            Assert.IsFalse(target.Hand.Contains(cards[3].Object));
        }

        [Test]
        public void PlayCard_Card_InvalidEffectIndex() {
            cards[0].Setup(p => p.NumEffects).Returns(1);
            Assert.Throws<IndexOutOfRangeException>(() => target.Play(cards[0].Object, 2));
        }

        [Test]
        public void PlayCard_CardEffectUnplayable() {
            cards[0].Setup(p => p.IsPlayable(It.IsAny<IPlayableGame>())).Returns(new List<int>());

            Assert.Throws<IndexOutOfRangeException>(() => target.Play(cards[0].Object, 2));
        }

    }


}