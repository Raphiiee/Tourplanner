using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
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
            string tourDataJson = tourItemDAO.GetTourData(alterTourItem);

            JObject jsonObject = JObject.Parse(tourDataJson);

            alterTourItem.TourDistance = (float) jsonObject["route"]?["distance"];
            alterTourItem.FuelUsed = (float) jsonObject["route"]?["fuelUsed"];
            alterTourItem.RouteSessionID = (string) jsonObject["route"]?["sessionId"];

            int errorCode = (int) jsonObject["route"]?["routeError"]?["errorCode"];
            string errorMessage = (string) jsonObject["route"]?["routeError"]?["message"];

            alterTourItem.RouteInformation = $"Directions of Route From {alterTourItem.Start} to {alterTourItem.Destination}\n" +
                                             $"Tour distance: {alterTourItem.TourDistance}km\n" +
                                             $"Tour Time: {jsonObject["route"]?["formattedTime"]}\n---------------\n";
            
            foreach (var maneuversSource in (JArray) jsonObject["route"]?["legs"]?[0]?["maneuvers"])
            {
                alterTourItem.RouteInformation += $"{(int)maneuversSource["index"]+1}. {maneuversSource["narrative"]}; \n" +
                                                  $"\tDirection distance {maneuversSource["distance"]}km; \n" +
                                                  $"\tTime for Direction {maneuversSource["formattedTime"]}\n\n";
            }

            tourItemDAO.GetTourMapImage(alterTourItem);

            tourItemDAO.AlterTourDetails(alterTourItem);
        }

        public void GetImage(TourItem tourItem)
        {
            tourItemDAO.GetTourMapImage(tourItem);
        }

        public void CleanUpImages(ObservableCollection<TourItem> tourItems)
        {
            tourItemDAO.CleanUpImages(tourItems);
        }
    }
}