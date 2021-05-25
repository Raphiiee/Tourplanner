using System.Collections.Generic;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public interface IDataAccess
    {
        public List<TourItem> GetItems();
        public TourItem AddItems();
        public void DeleteItems(TourItem deleteTourItem);
        public void DeleteLogItems(LogItem deleteLogItem);
        public void AddLogItems(LogItem addLogItem);
        public void AlterLogItems(LogItem alterItem);
        public void AlterTourDetails(TourItem alterTourItem);
    }
}