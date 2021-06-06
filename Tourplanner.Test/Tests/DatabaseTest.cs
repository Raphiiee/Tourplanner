using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;

namespace Tourplanner.Test.Tests
{
    [TestFixture]
    public class DatabaseTest
    {
        private Database _database;
        private TourItem _testTourItem;
        private List<TourItem> _itemList;
        private LogItem _newLogItem = new LogItem(){Date = DateTime.Now, LogId = 0, TourId = 0, ElevationGain = 0, Notice = "", Rating = "", SleepTime = 0, StepCounter = 0, IntakeCalories = 0, Distance = 0, DurationTime = 0, Weather = ""};
        private string test;
        [SetUp]
        public void Setup()
        {

            test = ConfigurationManager.AppSettings.Get("DBTest");
            _database  = Database.Instance(test);
            _itemList = _database.GetItems();
            _testTourItem = _itemList[^1];
        }

        [Test]
        public void LoadTest()
        {
            Assert.Greater(_itemList.Count, 1);
        }

        [Test]
        public void AddItemTest()
        {
            int numberOfToursBevorAdd = _itemList.Count;
            TourItem itemListAfterAdd = _database.AddItems();
            _itemList.Add(itemListAfterAdd);
            int numberOfToursAfterAdd = _itemList.Count;

            Assert.Greater(numberOfToursAfterAdd, numberOfToursBevorAdd);
            _testTourItem = itemListAfterAdd;
        }

        [Test]
        public void AddLogTest()
        {
            _newLogItem.LogId = 0;
            _newLogItem.TourId = _testTourItem.TourId;
            _database.AddLogItems(_newLogItem);

            Assert.Greater(_newLogItem.LogId, 0);
        }

        [Test]
        public void DeleteLogTest()
        {
            int logCountBevorDelete = _itemList[^1].Log.Count;
            _database.DeleteLogItems(_newLogItem);
            _itemList = _database.GetItems();

            Assert.AreEqual(logCountBevorDelete,_itemList[^1].Log.Count);
        }

        [Test]
        public void DeleteItemTest()
        {
            int countToursBeforeDeletion = _itemList.Count;
            _database.DeleteItems(_testTourItem);
            _itemList = _database.GetItems();
            int countToursAfterDeletion = _itemList.Count;

            Assert.AreNotEqual(countToursBeforeDeletion, countToursAfterDeletion);
        }

        [Test]
        public void AlterLogItemTest()
        {
            string Test = "Test!";
            _newLogItem.Notice = Test;
            _database.AlterLogItems(_newLogItem);
            _itemList = _database.GetItems();
            string ActualNotice = "";

            foreach (var tourItem in _itemList)
            {
                if (tourItem.TourId == _newLogItem.TourId)
                {
                    foreach (var logItem in tourItem.Log)
                    {
                        if (logItem.LogId == _newLogItem.LogId)
                        {
                            ActualNotice = logItem.Notice;
                            break;
                        }
                    }
                }
            } 

            Assert.AreEqual(Test, ActualNotice);
        }

        [Test]
        public void AlterTourItemTest()
        {
            string Test = "Test!";
            _testTourItem.TourDescription = Test;
            _database.AlterTourDetails(_testTourItem);
            _itemList = _database.GetItems();
            string ActualTourDescription = "";

            foreach (var tourItem in _itemList)
            {
                if (tourItem.TourId == _testTourItem.TourId)
                {
                    ActualTourDescription = tourItem.TourDescription;
                    break;
                }
            }

            Assert.AreEqual(Test, ActualTourDescription);
        }
    }
}