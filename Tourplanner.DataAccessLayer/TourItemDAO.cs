using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Windows.Media.Imaging;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class TourItemDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("TourItemDAO.cs");
        private IDataAccess dataAccess;

        public TourItemDAO(int option=0)
        {
            if (option == 0)
            {
                dataAccess = Database.Instance();
            }
            else if (option == 1)
            {
                dataAccess = Database.Instance(ConfigurationManager.AppSettings.Get("DBTest"));
            }
            else
            {
                dataAccess = new FileSystem();
            }
        }
        
        public List<TourItem> GetItems()
        {
            return dataAccess.GetItems();
        }
        public TourItem AddItems()
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

        public void AddLogItems(LogItem addLogItem)
        {
            dataAccess.AddLogItems(addLogItem);
        }
        public void AlterLogItems(LogItem logItem)
        {
            dataAccess.AlterLogItems(logItem);
        }

        public void AlterTourDetails(TourItem alterTourItem)
        {
            dataAccess.AlterTourDetails(alterTourItem);
        }

        public string GetTourData(TourItem tourItem)
        {
            log.Info($"Try to get tour data");
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

        public void GetTourMapImage(TourItem tourItem)
        {
            log.Info($"Try to get tour image");
            string apiKey = ConfigurationManager.AppSettings.Get("MQApiKey");
            string url = $"https://www.mapquestapi.com/staticmap/v5/map?key={apiKey}&start={tourItem.Start}&end={tourItem.Destination}&session={tourItem.RouteSessionID}&size=800,400";
            string filename = Path.GetRandomFileName() + ".png";
            string imagePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + 
                               ConfigurationManager.AppSettings.Get("MapFolder");
            
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            imagePath = imagePath + filename;
            WebClient mapWebClient = new WebClient();
            mapWebClient.DownloadFile(url,imagePath);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(imagePath);
            image.EndInit();

            tourItem.RouteImage = image;
            tourItem.RouteImagePath = imagePath;
        }

        public void CleanUpImages(ObservableCollection<TourItem> tourItems)
        {
            log.Info($"Try to delete unwanted images");
            string mapFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                               ConfigurationManager.AppSettings.Get("MapFolder");
            DirectoryInfo directory = new DirectoryInfo(mapFolderPath);

            try
            {
                foreach (var file in directory.GetFiles("*.png"))
                {
                    bool delete = true;
                    foreach (var tour in tourItems)
                    {
                        if (tour.RouteImagePath != null && tour.RouteImagePath.Contains(file.Name) )
                        {
                            delete = false;
                            break;
                        }
                    }

                    if (delete)
                    {
                        File.Delete(file.FullName);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            
        }
    }
}