using System;
using System.Collections.Generic;
using System.Text;
using PostPocModel.ResourceSystem;
using NUnit.Framework;
using Moq;

namespace PostPocModel.UnitTests.UnitTests.ResourceSystem
{
    public class StockPileTests
    {

        Stockpile target;
        Mock<IResource> mockResource1;
        Mock<IResource> mockResource2;
        Mock<IResource> mockResource3;
        Mock<IResource> mockResource4;



        [SetUp]
        public void Setup() {
            mockResource1 = new Mock<IResource>();
            mockResource2 = new Mock<IResource>();
            mockResource3 = new Mock<IResource>();
            mockResource4 = new Mock<IResource>();


            target = new Stockpile();
        }

        [Test]
        public void Adjust_AddResource_success() {
            target.Adjust(mockResource1.Object, 100);
            Assert.AreEqual(100, target.GetLevel(mockResource1.Object));
            target.Adjust(mockResource2.Object, 10);
            Assert.AreEqual(10, target.GetLevel(mockResource2.Object));
            target.Adjust(mockResource3.Object, -100);
            Assert.AreEqual(-100, target.GetLevel(mockResource3.Object));
            target.Adjust(mockResource4.Object, 0);
            Assert.AreEqual(0, target.GetLevel(mockResource4.Object));
        }

        [Test]
        public void Adjust_fail_nullResource()
        {
            Assert.Throws<ArgumentNullException>(delegate () { target.Adjust(null, 10); });
        }

        [Test]
        public void AdjustResource_success()
        {
            target.Adjust(mockResource1.Object, 100);
            Assert.AreEqual(100, target.GetLevel(mockResource1.Object));

            target.Adjust(mockResource1.Object, 100);
            Assert.AreEqual(200, target.GetLevel(mockResource1.Object));

            target.Adjust(mockResource1.Object, -51);
            Assert.AreEqual(149, target.GetLevel(mockResource1.Object));

            target.Adjust(mockResource1.Object, -150);
            Assert.AreEqual(-1, target.GetLevel(mockResource1.Object));

        }

        [Test]
        public void GetLevel_fail_nullResource()
        {
            Assert.Throws<ArgumentNullException>(delegate () { target.GetLevel(null); });
        }

    }

}
