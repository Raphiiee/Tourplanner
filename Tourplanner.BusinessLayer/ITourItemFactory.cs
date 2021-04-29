using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ITourItemFactory
    {
        IEnumerable<TourItem> GetItems();
        IEnumerable<TourItem> Search(string itemName);
        IEnumerable<TourItem> AddTourItem();
        void DeleteTourItem(TourItem deleteTourItem);
        void DeleteLogItem(LogItem deleteLogItem);
        void AlterTourDetails(TourItem alterTourItem);
        void GetImage(TourItem tourItem);
        void CleanUpImages(ObservableCollection<TourItem> tourItems);
        void PrintSpecificTourReport(TourItem tourItem);
        void PrintSumerizeTourReport(ObservableCollection<TourItem> tourItems);
    }
}