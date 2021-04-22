using System.Collections.Generic;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public interface IDataAccess
    {
        public List<TourItem> GetItems();
        public List<TourItem> AddItems();
        public void DeleteItems(TourItem deleteTourItem);
        public void DeleteLogItems(LogItem deleteLogItem);
        public void AlterTourDetails(TourItem alterTourItem);
    }
}