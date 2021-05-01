using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class FileSystem : IDataAccess
    {
        private string filePath;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("FileSystem.cs");

        public FileSystem()
        {
            filePath = "...";
        }

        public List<TourItem> GetItems()
        {
            log.Info($"Try to get Data from file system");
            // get tour items from file system
            return new List<TourItem>()
            {
                new TourItem() {TourId = 1, TourName = "Tour 1", Destination = "Wien",       Start = "Hollabrunn", RouteType = RouteTypeEnum.Fastest, Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(637535674210000000)}}},
                new TourItem() {TourId = 2, TourName = "Tour 2", Destination = "Salzburg",   Start = "Hollabrunn", RouteType = RouteTypeEnum.Shortest, Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1212, Notice = "ABCD", Date = new DateTime(637220314210000000)}}},
                new TourItem() {TourId = 3, TourName = "Tour 3", Destination = "Vorarlberg", Start = "Hollabrunn", RouteType = RouteTypeEnum.Pedestrian, Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1213, Notice = "ABCD", Date = new DateTime(636920314210000000)}}},
                new TourItem() {TourId = 4, TourName = "Apfel",  Destination = "Grado",      Start = "Hollabrunn", RouteType = RouteTypeEnum.Bicycle, Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1214, Notice = "ABCD", Date = new DateTime(636620314210000000)}}},
                new TourItem() {TourId = 5, TourName = "Appel",  Destination = "Korneuburg", Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1215, Notice = "ABCD", Date = new DateTime(636320314210000000)}}},
                new TourItem() {TourId = 6, TourName = "Banane", Destination = "Stockerau",  Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1216, Notice = "ABCD", Date = new DateTime(636020314210000000)}}}
            };
        }

        public List<TourItem> AddItems()
        {
            log.Info($"Try to Add Item to File System");
            // needs to implemented
            return new List<TourItem>()
            {
                new TourItem() {TourName = "New Tour", Log = new ObservableCollection<LogItem>(){new LogItem(){Date = DateTime.Now}}}
            };
        }

        public void DeleteItems(TourItem deleteTourItem)
        {
            log.Info($"Try to delete a tour");
            // Delete Tour ITEM from fileSystem
        }

        public void DeleteLogItems(LogItem deleteLogItem)
        {
            log.Info($"Try to delete tour log");
            // Delete Log ITEM from fileSystem
        }

        public void AlterTourDetails(TourItem alterTourItem)
        {
            log.Info($"Try to alter touritem");
            // Alter Log Item in FileSystem
        }
    }
}