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
            // SQL query here
            return new List<TourItem>()
            {
                new TourItem() {Name = "Tour 1"},
                new TourItem() {Name = "Tour 2"},
                new TourItem() {Name = "Tour 3"},
                new TourItem() {Name = "Tour 4"},
                new TourItem() {Name = "Tour 5"}
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