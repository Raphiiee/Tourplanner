using System.Collections.Generic;
using NUnit.Framework;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;

namespace Tourplanner.Test
{
    public class FileSystemTests
    {
        private FileSystem fileSystem;   
        [SetUp]
        public void Setup()
        {
            fileSystem = new FileSystem();
        }

        [Test]
        public void LoadTest()
        {
            List<TourItem> itemList = fileSystem.GetItems();
            Assert.AreEqual(6,itemList.Count);
        }

        [Test]
        public void AddItemTest()
        {
            // After implementation Test here
        }

        [Test]
        public void DeleteItemTest()
        {
            // After implementation Test here
        }
    }
}