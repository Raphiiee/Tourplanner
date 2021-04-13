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
            // get tour items from file system
            return new List<TourItem>()
            {
                new TourItem() {TourId = 1, TourName = "Tour 1", Destination = "Wien",       Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = "1112", Notice = "ABCD", Date = new DateTime(637535674210000000)}}},
                new TourItem() {TourId = 2, TourName = "Tour 2", Destination = "Salzburg",   Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = "0012", Notice = "ABCD", Date = new DateTime(637220314210000000)}}},
                new TourItem() {TourId = 3, TourName = "Tour 3", Destination = "München",    Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = "0013", Notice = "ABCD", Date = new DateTime(636920314210000000)}}},
                new TourItem() {TourId = 4, TourName = "Apfel",  Destination = "Grado",      Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = "0014", Notice = "ABCD", Date = new DateTime(636620314210000000)}}},
                new TourItem() {TourId = 5, TourName = "Appel",  Destination = "Korneuburg", Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = "0015", Notice = "ABCD", Date = new DateTime(636320314210000000)}}},
                new TourItem() {TourId = 6, TourName = "Banane", Destination = "Stockerau",  Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = "0016", Notice = "ABCD", Date = new DateTime(636020314210000000)}}}
            };
        }

        public List<TourItem> AddItems()
        {
            // needs to implemented
            return new List<TourItem>()
            {
                new TourItem() {TourName = "New Tour", Log = new ObservableCollection<LogItem>(){new LogItem(){Date = DateTime.Now}}}
            };
        }

        public void DeleteItems(TourItem deleteTourItem)
        {
            // Delete Tour ITEM from fileSystem
        }

        public void DeleteLogItems(LogItem deleteLogItem)
        {
            // Delete Log ITEM from fileSystem
        }
    }
}