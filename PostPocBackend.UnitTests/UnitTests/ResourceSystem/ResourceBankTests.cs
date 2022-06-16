using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Moq;
using PostPocModel.ResourceSystem;

namespace PostPocModel.UnitTests.UnitTests.ResourceSystem
{
    public class ResourceBankTests
    {

        ResourceBank<MockResource> target;

        [SetUp]
        public void Setup() {
            target = new ResourceBank<MockResource>();
        }

        [Test]
        public void AddResource_success() {
            MockResource mock = new MockResource();
            mock.Name = "mock";
            target.AddNewResource(mock);

            Assert.AreEqual(mock, target.Get("mock"));
        }

        [Test]
        public void AddResource_Fail_DuplicatResource()
        {
            MockResource mock = new MockResource();
            mock.Name = "mock";
            target.AddNewResource(mock);
            Assert.Throws<ArgumentException>(delegate() { target.AddNewResource(mock); });

            MockResource mock2 = new MockResource();
            mock2.Name = "mock";
            Assert.Throws<ArgumentException>(delegate () { target.AddNewResource(mock2); });
        }

        [Test]
        public void AddResource_Fail_NullResource()
        {
            Assert.Throws<ArgumentNullException>(delegate () { target.AddNewResource(null); });
        }

        [Test]
        public void AddResource_Fail_BadName()
        {
            MockResource mock = new MockResource();
            mock.Name = "";
            Assert.Throws<ArgumentException>(delegate () { target.AddNewResource(mock); });

            mock = new MockResource();
            mock.Name = " ";
            Assert.Throws<ArgumentException>(delegate () { target.AddNewResource(mock); });

            mock = new MockResource();
            mock.Name = "   ";
            Assert.Throws<ArgumentException>(delegate () { target.AddNewResource(mock); });
        }

        [Test]
        public void GetResource_NotFound()
        {
            Assert.IsNull ( target.Get("asdf"));
        }

        [Test]
        public void GetResourceByTag_Success()
        {
            MockResource mock = new MockResource();
            mock.Name = "mock";
            mock.Tags = new HashSet<string>() { "tag"};
            target.AddNewResource(mock);

            Assert.Contains(mock, target.GetByTag("Tag"));

            MockResource mock2 = new MockResource();
            mock2.Name = "mock2";
            mock2.Tags = new HashSet<string>() { "tag" };
            target.AddNewResource(mock2);

            Assert.Contains(mock2, target.GetByTag("tag"));
            Assert.AreEqual(2, target.GetByTag("Tag").Count);

        }

        [Test]
        public void GetResourceByTag_NoResourcesWithTag()
        {
            MockResource mock = new MockResource();
            mock.Name = "mock";
            mock.Tags = new HashSet<string>() { "Notag" };
            target.AddNewResource(mock);

            Assert.IsEmpty(target.GetByTag("Tag"));
        }

        [Test]
        public void GetResourceByTag_Fail_BadTag()
        {

            Assert.Throws<ArgumentException>(delegate() { target.GetByTag(""); });
            Assert.Throws<ArgumentException>(delegate () { target.GetByTag(" "); });
            Assert.Throws<ArgumentException>(delegate () { target.GetByTag("    "); });

        }

        public class MockResource : IResource
        {
            public string Name { get; set; }

            public ISet<string> Tags { get; set; }

        }

    }

}
