using System;
using System.Collections.Generic;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class Database : IDataAccess
    {
        private string connetionString;

        public Database()
        {
            connetionString = "...";
        }
        public List<TourItem> GetItems()
        {
            // get media items from file system
            return new List<TourItem>()
            {
                new TourItem() {TourId = 1, TourName = "Tour 1", Destination = "Wien",       Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/1998)}}},
                new TourItem() {TourId = 1, TourName = "Tour 1", Destination = "Wien",       Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/1998)}}},
                new TourItem() {TourId = 1, TourName = "Tour 2", Destination = "Salzburg",   Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/1999)}}},
                new TourItem() {TourId = 1, TourName = "Tour 3", Destination = "München",    Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/2000)}}},
                new TourItem() {TourId = 1, TourName = "Apfel",  Destination = "Grado",      Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/2001)}}},
                new TourItem() {TourId = 1, TourName = "Appel",  Destination = "Korneuburg", Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/2002)}}},
                new TourItem() {TourId = 1, TourName = "Banane", Destination = "Stockerau",  Start = "Hollabrunn", Log = new List<LogItem>(){new LogItem(){Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/2003)}}}
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
            // Delete Tour ITEM from Database
        }
    }
}