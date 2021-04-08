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
                new TourItem() {Name = "Tour 1"},
                new TourItem() {Name = "Tour 2"},
                new TourItem() {Name = "Tour 3"},
                new TourItem() {Name = "Apfel"},
                new TourItem() {Name = "Appel"},
                new TourItem() {Name = "Banane"}
            };
        }

        public List<TourItem> AddItems()
        {
            return new List<TourItem>()
            {
                new TourItem() {Name = "New Tour"}
            };
        }

        public List<TourItem> DeleteItems()
        {
            throw new System.NotImplementedException();
        }
    }
}