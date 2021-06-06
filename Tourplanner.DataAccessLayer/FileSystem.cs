using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class FileSystem : IDataAccess
    {
        private string _filePath;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("FileSystem.cs");

        public FileSystem()
        {
            try
            {
                _filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + ConfigurationManager.AppSettings.Get("FileFolder");

                if (!Directory.Exists(_filePath))
                {
                    Directory.CreateDirectory(_filePath);
                }

                _filePath += ConfigurationManager.AppSettings.Get("DataJSON");
                if (!File.Exists(_filePath))
                {
                    File.Create(_filePath);
                    JSONSampleData jsonSampleData = new JSONSampleData(); 
                    File.WriteAllText(_filePath, jsonSampleData.sample);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to create filesystem object \n Details: {e}");
            }

        }

        public List<TourItem> GetItems()
        {
            log.Info($"Try to get Data from file system");
            try
            {
                
                string jsonDataString = File.ReadAllText(_filePath);
                List<TourItem> tourItems = JsonConvert.DeserializeObject<List<TourItem>>(jsonDataString);
                foreach (var tourItem in tourItems)
                {
                    if (!File.Exists(tourItem.RouteImagePath))
                    {
                        tourItem.RouteImagePath = " ";
                    }
                    if (tourItem.TourName is null)
                    {
                        tourItem.TourName = " ";
                    }
                    if (tourItem.Start is null)
                    {
                        tourItem.Start = " ";
                    }
                    if (tourItem.Destination is null)
                    {
                        tourItem.Destination = " ";
                    }
                    if (tourItem.TourDescription is null)
                    {
                        tourItem.TourDescription = " ";
                    }
                    if (tourItem.RouteInformation is null)
                    {
                        tourItem.RouteInformation = " ";
                    }
                    if (tourItem.RouteImagePath is null)
                    {
                        tourItem.RouteImagePath = " ";
                    }

                    if (tourItem.Log is null)
                    {
                        tourItem.Log = new ObservableCollection<LogItem>();
                    }

                    foreach (var logItem in tourItem.Log)
                    {
                        if (logItem.Notice is null)
                        {
                            logItem.Notice = " ";
                        }
                        if (logItem.Rating is null)
                        {
                            logItem.Rating = " ";
                        }
                        if (logItem.Weather is null)
                        {
                            logItem.Weather = " ";
                        }
                    }
                }
                return tourItems;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to get Data from file system \n Details: {e}");
                return new List<TourItem>();
            }
        }

        private void WriteDataIntoFile(List<TourItem> tourItems)
        {
            try
            {
                log.Info($"Try to Write Data to File System");
                File.WriteAllText(_filePath, JsonConvert.SerializeObject(tourItems));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to Write Data to File System \n Details: {e}");
            }
        }

        public TourItem AddItems()
        {
            try
            {
                log.Info($"Try to Add Item to File System");
                List<TourItem> tourItems = GetItems();
                Random r = new Random();
                int randomId = r.Next(111111111,999999999);
                tourItems.Add(new TourItem() {TourName = "New Tour", TourId = randomId, Log = new ObservableCollection<LogItem>(){new LogItem(){Date = DateTime.Now, TourId = randomId, LogId = 1}}});
                WriteDataIntoFile(tourItems);
                return tourItems[^1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to Write Data to File System \n Details: {e}");
                return new TourItem();
            }

        }

        public void DeleteItems(TourItem deleteTourItem)
        {
            try
            {
                List<TourItem> tourItems = GetItems();
                log.Info($"Try to delete a tour");

                for (int i = 0; i < tourItems.Count; i++)
                {
                    if (tourItems[i].TourId == deleteTourItem.TourId)
                    {
                        tourItems.RemoveAt(i);
                        break;
                    }
                }
            
                WriteDataIntoFile(tourItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to delete a tour from File System \n Details: {e}");
            }
            
        }

        public void DeleteLogItems(LogItem deleteLogItem)
        {
            try
            {
                List<TourItem> tourItems = GetItems();
                log.Info($"Try to delete tour log");

                for (int i = 0; i < tourItems.Count; i++)
                {
                    for (int j = 0; j < tourItems[i].Log.Count; j++)
                    {
                        if (tourItems[i].Log[j].LogId == deleteLogItem.LogId)
                        {
                            tourItems[i].Log.RemoveAt(j);
                            break;
                        }
                    }
                }
            
                WriteDataIntoFile(tourItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to delete tour log \n Details: {e}");
            }
            
        }

        public void AlterLogItems(LogItem alterItem)
        {
            try
            {
                List<TourItem> tourItems = GetItems();
                log.Info($"Try to alter tour log");

                for (int i = 0; i < tourItems.Count; i++)
                {
                    for (int j = 0; j < tourItems[i].Log.Count; j++)
                    {
                        if (tourItems[i].Log[j].LogId == alterItem.LogId && tourItems[i].TourId == alterItem.TourId)
                        {
                            tourItems[i].Log[j].Notice = alterItem.Notice;
                            tourItems[i].Log[j].Date = alterItem.Date;
                            tourItems[i].Log[j].Distance = alterItem.Distance;
                            tourItems[i].Log[j].Weather = alterItem.Weather;
                            tourItems[i].Log[j].Rating = alterItem.Rating;
                            tourItems[i].Log[j].ElevationGain = alterItem.ElevationGain;
                            tourItems[i].Log[j].DurationTime = alterItem.DurationTime;
                            tourItems[i].Log[j].IntakeCalories = alterItem.IntakeCalories;
                            tourItems[i].Log[j].SleepTime = alterItem.SleepTime;
                            tourItems[i].Log[j].StepCounter = alterItem.StepCounter;

                            break;
                        }
                    }
                }
            
                WriteDataIntoFile(tourItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to alter tour log \n Details: {e}");
            }
        }

        public void AlterTourDetails(TourItem alterTourItem)
        {
            try
            {
                List<TourItem> tourItems = GetItems();
                log.Info($"Try to alter touritem");

                /*tourItems.RemoveAt(tourItems.IndexOf(alterTourItem));
                tourItems.Add(alterTourItem);*/
                for (int i = 0; i < tourItems.Count; i++)
                {
                    if (tourItems[i].TourId == alterTourItem.TourId)
                    {
                        tourItems[i] = alterTourItem;
                    }
                }

                WriteDataIntoFile(tourItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to alter touritem \n Details: {e}");
            }
        }

        public void AddLogItems(LogItem addLogItem)
        {
            try
            {
                List<TourItem> tourItems = GetItems();
                log.Info($"Try to add Log item in FS");

                for (int i = 0; i < tourItems.Count; i++)
                {
                    if (tourItems[i].TourId == addLogItem.TourId)
                    {
                        addLogItem.LogId = tourItems[i].Log.Count + 1;
                        tourItems[i].Log.Add(addLogItem);
                    }
                }

                WriteDataIntoFile(tourItems);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to add Log item in FS \n Details: {e}");
            }
        }
    }
}