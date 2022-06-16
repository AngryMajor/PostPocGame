using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PostPocModel.ResourceSystem;

namespace PostPocModel.UnitTests.IntegrationTests
{
    class ResourceSystemTests
    {
        Stockpile currStockpile;
        ResourceBank<Resource> currBank;

        [SetUp]
        public void Setup() {
            currStockpile = new Stockpile();
            currBank = new ResourceBank<Resource>();
        }

        [Test]
        public void CreateResource() {

            new Resource(currBank, "NoTag");

            new Resource(currBank, "asdf", new HashSet<string>() { "big", "chunky" });
            Assert.NotNull(currBank.Get("asdf"));

        }

        [Test]
        public void GetResourcesFromBank()
        {
            new Resource(currBank,"asdf", new HashSet<string>() { "big", "chunky" });

            Assert.NotNull(currBank.Get("asdf"));
            Assert.AreEqual(1, currBank.GetByTag("big").Count);

            new Resource(currBank,"other", new HashSet<string>() { "big" });
            Assert.AreEqual(2, currBank.GetByTag("big").Count);

        }

        [Test]
        public void GetResourcesFail() {
            Assert.IsNull( currBank.Get("asdf"));
            Assert.IsEmpty(currBank.GetByTag("big"));
        }

        [Test]
        public void DepositResource() {
            new Resource(currBank, "asdf");

            currStockpile.Adjust(currBank.Get("asdf"), 100);
            Assert.AreEqual(100, currStockpile.GetLevel(currBank.Get("asdf")));

        }

        [Test]
        public void WithdrawResource() {
            new Resource(currBank, "asdf");

            currStockpile.Adjust(currBank.Get("asdf"), 100);
            currStockpile.Adjust(currBank.Get("asdf"), -100);
            Assert.AreEqual(0, currStockpile.GetLevel(currBank.Get("asdf")));

        }

        [Test]
        public void CreateNeed()
        {
            new Resource(currBank, "asdf");

            currStockpile.Adjust(currBank.Get("asdf"), -100);
            Assert.AreEqual(-100, currStockpile.GetLevel(currBank.Get("asdf")));
        }

        [Test]
        public void CheckLevels() {
            new Resource(currBank, "asdf");

            currStockpile.Adjust(currBank.Get("asdf"), 100);

            Assert.AreEqual(100, currStockpile.GetLevel(currBank.Get("asdf")));
        }

    }
}
