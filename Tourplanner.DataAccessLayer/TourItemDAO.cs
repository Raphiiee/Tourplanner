using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class TourItemDAO
    {
        private IDataAccess dataAccess;

        public TourItemDAO()
        {
            // Check which datasource to use
            // dataAccess = new Database();
             dataAccess = new FileSystem();
        }
        
        public List<TourItem> GetItems()
        {
            return dataAccess.GetItems();
        }
        public List<TourItem> AddItems()
        {
            return dataAccess.AddItems();
        }
        public void DeleteItems(TourItem deleteTourItem)
        {
            dataAccess.DeleteItems(deleteTourItem);
        }

        public void DeleteLogItems(LogItem deleteLogItem)
        {
            dataAccess.DeleteLogItems(deleteLogItem);
        }

        public void AlterTourDetails(TourItem alterTourItem)
        {
            dataAccess.AlterTourDetails(alterTourItem);
        }

        public string GetTourData(TourItem tourItem)
        {
            string apiKey = ConfigurationManager.AppSettings.Get("MQApiKey");
            string routeType = tourItem.RouteType.ToString();
            string url = $"https://www.mapquestapi.com/directions/v2/route?key={apiKey}&from={tourItem.Start}&to={tourItem.Destination}&routeType={routeType}&locale=de_DE&unit=k";

            var request = WebRequest.Create(url);
            request.Method = "GET";

            using var webResponse = request.GetResponse();
            using var webStream = webResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            string data = reader.ReadToEnd();

            Console.WriteLine(data);

            return data;
        }
    }
}