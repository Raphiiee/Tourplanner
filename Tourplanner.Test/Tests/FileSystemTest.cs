using System.Collections.Generic;
using NUnit.Framework;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;

namespace Tourplanner.Test.Tests
{
    public class FileSystemTests
    {
        private FileSystem fileSystem;
        private List<TourItem> _itemList;
        private LogItem _newLogItem;

        [SetUp]
        public void Setup()
        {
            fileSystem = new FileSystem();
            _itemList = fileSystem.GetItems();
        }

        [Test]
        public void LoadTest()
        {
            List<TourItem> itemList = fileSystem.GetItems();
            Assert.AreEqual(8,itemList.Count);
        }

        [Test]
        public void AddItemTest()
        {
            int TourCountBevorNewItem = _itemList.Count;
            _itemList.Add(fileSystem.AddItems());
            int TourCountAfterNewItem = _itemList.Count;

            Assert.Greater(TourCountAfterNewItem, TourCountBevorNewItem);

        }

        [Test]
        public void AddLogTest()
        {
            int TourLogCountBevorNewLog = _itemList[^1].Log.Count;
            _newLogItem = new LogItem() {TourId = _itemList[^1].TourId};
            fileSystem.AddLogItems(_newLogItem);
            _itemList = fileSystem.GetItems();
            int TourLogCountAfterNewLog = _itemList[^1].Log.Count;

            Assert.Greater(TourLogCountAfterNewLog, TourLogCountBevorNewLog);
        }

        [Test]
        public void AlterLogItemTest()
        {
            string notice = "Test!";
            string currentNotice = "";
            _newLogItem.Notice = notice;
            fileSystem.AlterLogItems(_newLogItem);
            _itemList = fileSystem.GetItems();

            foreach (var tourItem in _itemList)
            {
                if (tourItem.TourId == _newLogItem.TourId)
                {
                    foreach (var logItem in tourItem.Log)
                    {
                        if (logItem.LogId == _newLogItem.LogId)
                        {
                            currentNotice = _newLogItem.Notice;
                            break;
                        }
                    }
                    break;
                }
            }

            Assert.AreEqual(notice, currentNotice);
        }

        [Test]
        public void AlterTourItemTest()
        {
            string tourName = "Test Tour!";
            string currentTourName = "";
            _itemList[^1].TourName = tourName;
            fileSystem.AlterTourDetails(_itemList[^1]);
            _itemList = fileSystem.GetItems();

            foreach (var tourItem in _itemList)
            {
                if (tourItem.TourId == _itemList[^1].TourId)
                {
                    currentTourName = tourItem.TourName;
                    break;
                }
            }

            Assert.AreEqual(tourName, currentTourName);

        }

        [Test]
        public void DeleteLogItemTest()
        {
            int LogCountBevorLogDelete = _itemList[^1].Log.Count;
            fileSystem.DeleteLogItems(_newLogItem);
            _itemList = fileSystem.GetItems();
            int LogCountAfterLogDelete = _itemList[^1].Log.Count;

            Assert.AreNotEqual(LogCountBevorLogDelete, LogCountAfterLogDelete);
        }

        [Test]
        public void DeleteTourItemTest()
        {
            int TourCountBevorTourDelete = _itemList.Count;
            fileSystem.DeleteItems(_itemList[^1]);
            _itemList = fileSystem.GetItems();
            int TourCountAfterTourDelete = _itemList.Count;

            Assert.AreNotEqual(TourCountBevorTourDelete, TourCountAfterTourDelete);
        }
    }
}