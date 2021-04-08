using System;
using System.Collections.Generic;
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
                new TourItem() {Name = "Tour 1", Destination = "Wien", Start = "Hollabrunn", Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/1998)},
                new TourItem() {Name = "Tour 2", Destination = "Salzburg", Start = "Hollabrunn", Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/1999)},
                new TourItem() {Name = "Tour 3", Destination = "München", Start = "Hollabrunn", Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/2000)},
                new TourItem() {Name = "Apfel", Destination = "Grado", Start = "Hollabrunn", Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/2001)},
                new TourItem() {Name = "Appel", Destination = "Korneuburg", Start = "Hollabrunn", Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/2002)},
                new TourItem() {Name = "Banane", Destination = "Stockerau", Start = "Hollabrunn", Distance = "1112", Notice = "ABCD", Date = new DateTime(31/03/2003)}
            };
        }

        public List<TourItem> AddItems()
        {
            return new List<TourItem>()
            {
                new TourItem() {Name = "New Tour"}
            };
        }

        public void DeleteItems(TourItem deleteTourItem)
        {
            // Delete Tour ITEM from fileSystem
        }
    }
}