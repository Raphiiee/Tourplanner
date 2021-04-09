using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class FileSystem : IDataAccess
    {
        private string filePath;

        public FileSystem()
        {
            filePath = "...";
        }

        public List<TourItem> GetItems()
        {
            // get media items from file system
            return new List<TourItem>()
            {
                new TourItem() {TourId = 1, TourName = "Tour 1", Destination = "Wien",       Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "1112", Notice = "ABCD", Date = new DateTime(2021, 12, 12, 0, 0, 0)}}},
                new TourItem() {TourId = 2, TourName = "Tour 2", Destination = "Salzburg",   Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "0012", Notice = "ABCD", Date = new DateTime(2020, 12, 12, 0, 0, 0)}}},
                new TourItem() {TourId = 3, TourName = "Tour 3", Destination = "München",    Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "0013", Notice = "ABCD", Date = new DateTime(2019, 12, 12, 0, 0, 0)}}},
                new TourItem() {TourId = 4, TourName = "Apfel",  Destination = "Grado",      Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "0014", Notice = "ABCD", Date = new DateTime(2018, 12, 12, 0, 0, 0)}}},
                new TourItem() {TourId = 5, TourName = "Appel",  Destination = "Korneuburg", Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "0015", Notice = "ABCD", Date = new DateTime(2017, 12, 12, 0, 0, 0)}}},
                new TourItem() {TourId = 6, TourName = "Banane", Destination = "Stockerau",  Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "0016", Notice = "ABCD", Date = new DateTime(2016, 12, 12, 0, 0, 0)}}}
            };
        }

        public List<TourItem> AddItems()
        {
            return new List<TourItem>()
            {
                new TourItem() {TourName = "New Tour"}
            };
        }

        public void DeleteItems(TourItem deleteTourItem)
        {
            // Delete Tour ITEM from fileSystem
        }
    }
}