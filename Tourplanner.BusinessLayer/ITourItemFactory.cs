using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ITourItemFactory
    {
        IEnumerable<TourItem> GetItems();
        List<TourItem> Search(string itemName);
        TourItem AddTourItem();
        void DeleteTourItem(TourItem deleteTourItem);
        void DeleteLogItem(LogItem deleteLogItem);
        void AddLogItem(LogItem addLogItem, TourItem selectedTourItem);
        void AlterLogItem(TourItem selectedTourItem);
        void AlterTourDetails(TourItem alterTourItem);
        void GetImage(TourItem tourItem);
        void CleanUpImages(ObservableCollection<TourItem> tourItems);
        void PrintSpecificTourReport(TourItem tourItem);
        void PrintSumerizeTourReport(ObservableCollection<TourItem> tourItems);
        void SaveTourDataJson(ObservableCollection<TourItem> tourItems);
        void LoadTourDataJson(ObservableCollection<TourItem> tourItems);
        void ChangeDataSource(int source);
    }
}