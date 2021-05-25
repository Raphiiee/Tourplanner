using System.Collections.Generic;
using NUnit.Framework;
using Tourplanner.BusinessLayer;
using Tourplanner.Models;

namespace Tourplanner.Test.Tests
{
    [TestFixture]
    public class SearchTest
    {
        private TourItemFactoryImpl _tourItemFactory;

        [SetUp]
        public void Setup()
        {
            _tourItemFactory = new TourItemFactoryImpl();
            _tourItemFactory.ChangeDataSource(1);
        }

        [Test]
        public void DBFindNothingTest()
        {
            List<TourItem> foundList =  _tourItemFactory.Search("aseljdjhaslvbjkashfjkesh");

            Assert.AreEqual(0, foundList.Count);
        }

        [Test]
        public void DBFindTourSpecificTourDescription()
        {
            List<TourItem> foundList =  _tourItemFactory.Search("not");

            Assert.AreEqual(1, foundList.Count);
        }

        [Test]
        public void DBFindTourSpecificTourLog()
        {
            List<TourItem> foundList =  _tourItemFactory.Search("write like an elephant");

            Assert.AreEqual(1, foundList.Count);
        }

        [Test]
        public void FileFindNothingTest()
        {
            _tourItemFactory.ChangeDataSource(2);
            List<TourItem> foundList =  _tourItemFactory.Search("aseljdjhaslvbjkashfjkesh");

            Assert.AreEqual(0, foundList.Count);
        }

        [Test]
        public void FileFindTourSpecificTourDescription()
        {
            _tourItemFactory.ChangeDataSource(2);
            List<TourItem> foundList =  _tourItemFactory.Search("not");

            Assert.AreEqual(1, foundList.Count);
        }

        [Test]
        public void FileFindTourSpecificTourLog()
        {
            _tourItemFactory.ChangeDataSource(2);
            List<TourItem> foundList =  _tourItemFactory.Search("write like an elephant");

            Assert.AreEqual(1, foundList.Count);
        }
    }
}