using System.Collections.Generic;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public interface IDataAccess
    {
        public List<TourItem> GetItems();
        public List<TourItem> AddItems();
        public List<TourItem> DeleteItems();
    }
}