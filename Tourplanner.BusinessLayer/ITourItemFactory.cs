using System.Collections.Generic;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ITourItemFactory
    {
        IEnumerable<TourItem> GetItems();
        IEnumerable<TourItem> Search(string itemName);
        IEnumerable<TourItem> AddTourItem();
        void DeleteTourItem(TourItem deleteTourItem);
    }
}