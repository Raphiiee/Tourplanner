using System.Collections.Generic;
using System.Linq;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public class TourItemFactoryImpl : ITourItemFactory
    {
        private TourItemDAO tourItemDAO = new TourItemDAO();

        public IEnumerable<TourItem> GetItems()
        {
            return tourItemDAO.GetItems();
        }

        public IEnumerable<TourItem> Search(string itemName)
        {
            IEnumerable<TourItem> items = GetItems();

            return items.Where(x => x.TourName.ToLower().Contains(itemName.ToLower()));
        }

        public IEnumerable<TourItem> AddTourItem()
        {
            return tourItemDAO.AddItems();
        }

        public void DeleteTourItem(TourItem deleteTourItem)
        {
            tourItemDAO.DeleteItems(deleteTourItem);
        }

        public void DeleteLogItem(LogItem deleteLogItem)
        {
            tourItemDAO.DeleteLogItems(deleteLogItem);
        }

        public void AlterTourDetails(TourItem alterTourItem)
        {
            tourItemDAO.AlterTourDetails(alterTourItem);
        }
    }
}