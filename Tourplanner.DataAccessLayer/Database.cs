using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class Database : IDataAccess
    {
        private string connetionString;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Database.cs");

        public Database()
        {
            log.Info($"Try to connect to database");
            connetionString = "...";
        }
        public List<TourItem> GetItems()
        {
            log.Info($"Try to get tour data from database");
            // get media items from file system
            return new List<TourItem>()
            {
                new TourItem() {TourId = 1, TourName = "Tour 1", Destination = "Wien",       Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/1998)}}},
                new TourItem() {TourId = 2, TourName = "Tour 1", Destination = "Wien",       Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/1998)}}},
                new TourItem() {TourId = 3, TourName = "Tour 2", Destination = "Salzburg",   Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/1999)}}},
                new TourItem() {TourId = 4, TourName = "Tour 3", Destination = "Vorarlberg", Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/2000)}}},
                new TourItem() {TourId = 5, TourName = "Apfel",  Destination = "Grado",      Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/2001)}}},
                new TourItem() {TourId = 6, TourName = "Appel",  Destination = "Korneuburg", Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/2002)}}},
                new TourItem() {TourId = 7, TourName = "Banane", Destination = "Stockerau",  Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/2003)}}}
            };
        }

        public List<TourItem> AddItems()
        {
            log.Info($"Try to add items to database");
            return new List<TourItem>()
            {
                new TourItem() {TourName = "New Tour"}
            };
        }

        public void DeleteItems(TourItem deleteTourItem)
        {
            log.Info($"Try to delete tour item from database");
            // Delete Tour ITEM from Database
        }

        public void DeleteLogItems(LogItem deleteLogItem)
        {
            log.Info($"Try to delete log item from database");
            // Delete Log ITEM from fileSystem
        }

        public void AlterTourDetails(TourItem alterTourItem)
        {
            log.Info($"Try to delete alter tour item from database");
            // Alter Tour ITem in Database
        }
    }
}