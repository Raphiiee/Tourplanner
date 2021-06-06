using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;
using Path = System.IO.Path;

namespace Tourplanner.BusinessLayer
{
    public class TourItemFactoryImpl : ITourItemFactory
    {
        private TourItemDAO tourItemDAO = new TourItemDAO();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("TourItemFactoryImpl.cs");

        public void ChangeDataSource(int source)
        {
            try
            {
                log.Info($"Try to change DataSource");
                tourItemDAO = new TourItemDAO(source);
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to Change DataSource \n Details: {e}");
                Console.WriteLine(e);
            }
        }

        public IEnumerable<TourItem> GetItems()
        {
            try
            {
                log.Info($"Try to get Items");
                return tourItemDAO.GetItems();
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to get Items \n Details: {e}");
                Console.WriteLine(e);
                return new List<TourItem>();
            }

        }

        public List<TourItem> Search(string itemName)
        {
            IEnumerable<TourItem> allItems = GetItems();
            if (itemName is null || itemName == "")
            {
                return (List<TourItem>) allItems;
            }
            itemName = itemName.ToLower();
            List<TourItem> foundItems = new List<TourItem>();

            try
            {
                log.Info($"Try to find Tour Items");
                foreach (var tourItem in allItems)
                {
                    if (tourItem.Destination.ToLower().Contains(itemName)
                        || tourItem.Start.ToLower().Contains(itemName)
                        || tourItem.TourDescription.ToLower().Contains(itemName)
                        || tourItem.RouteInformation.ToLower().Contains(itemName)
                        || tourItem.TourName.ToLower().Contains(itemName))
                    {
                        foundItems.Add(tourItem);
                        continue;
                    }

                    if (!(tourItem.Log is null))
                    {
                        foreach (var logItem in tourItem.Log)
                        { 
                            if (logItem.Weather.ToLower().Contains(itemName)
                              || logItem.Notice.ToLower().Contains(itemName)
                              || logItem.Rating.ToLower().Contains(itemName))
                            {
                                foundItems.Add(tourItem);
                            }

                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to Tour find Items \n Details: {e}");
                Console.WriteLine(e);
                throw;
            }
            
            
            log.Info($"Found {foundItems.Count()} for the search term {itemName}");

            return foundItems;
        }

        public TourItem AddTourItem()
        {
            try
            {
                log.Info($"Try to add Tour Item");
                return tourItemDAO.AddItems();
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to add Tour Items \n Details: {e}");
                Console.WriteLine(e);
                return new TourItem();
            }
        }

        public void DeleteTourItem(TourItem deleteTourItem)
        {
            try
            {
                log.Info($"Try to delete Tour Item");
                tourItemDAO.DeleteItems(deleteTourItem);
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to delete Tour Item \n Details: {e}");
                Console.WriteLine(e);
            }
        }

        public void AddLogItem(LogItem addLogItem, TourItem selectedTourItem)
        {
            try
            {
                log.Info($"Try to add Log Item");
                addLogItem.Date = DateTime.Now; 
                addLogItem.TourId = selectedTourItem.TourId;
                tourItemDAO.AddLogItems(addLogItem);
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to add Log Item \n Details: {e}");
                Console.WriteLine(e);
            }
        }

        public void AlterLogItem(TourItem selectedTourItem)
        {
            try
            {
                log.Info($"Try to alter Log Items");
                foreach (var logItem in selectedTourItem.Log)
                {
                    tourItemDAO.AlterLogItems(logItem);
                }
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to alter Log Items \n Details: {e}");
                Console.WriteLine(e);
            }

        }

        public void DeleteLogItem(LogItem deleteLogItem)
        {
            try
            {
                log.Info($"Try to Delete Log Item");
                tourItemDAO.DeleteLogItems(deleteLogItem);
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to Delete Log Item \n Details: {e}");
                Console.WriteLine(e);
            }
        }

        public void AlterTourDetails(TourItem alterTourItem)
        {
            try
            {
                string tourDataJson = tourItemDAO.GetTourData(alterTourItem);
                
                log.Info($"Try to alter Tour Data");
                JObject jsonObject = JObject.Parse(tourDataJson);
                LoadJsonData(alterTourItem,jsonObject);

                tourItemDAO.GetTourMapImage(alterTourItem);
                tourItemDAO.AlterTourDetails(alterTourItem);
                
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to alter Log Item \n Details: {e}");
                Console.WriteLine(e);
            }
        }

        public void LoadJsonData(TourItem alterTourItem, JObject jsonObject)
        {
            try
            {
                log.Info($"Try to Parse JSON from Tour Data");
                string infoCode = "";
                string infoMessage = "";

                int errorCode = (int) jsonObject["route"]?["routeError"]?["errorCode"];
                if (errorCode > 0)
                {
                    infoCode = (string) jsonObject["info"]?["statuscode"];
                    infoMessage = (string) jsonObject["info"]?["messages"]?[0];
                    log.Info($"Tour not Valid Error Code: {errorCode}; Info Code: {infoCode}; Info Messsage: {infoMessage}");
                    alterTourItem.Destination = "Not a Valid Tour";
                    alterTourItem.Start = "Not a Valid Tour";
                    alterTourItem.TourDescription = $"{infoMessage}";
                    return;
                }

                alterTourItem.TourDistance = (float) jsonObject["route"]?["distance"];
                alterTourItem.FuelUsed = (float) jsonObject["route"]?["fuelUsed"];
                alterTourItem.RouteSessionID = (string) jsonObject["route"]?["sessionId"];


                alterTourItem.RouteInformation = $"Directions of Route From {alterTourItem.Start} to {alterTourItem.Destination}\n" +
                                                 $"Tour distance: {alterTourItem.TourDistance}km\n" +
                                                 $"Tour Time: {jsonObject["route"]?["formattedTime"]}\n---------------\n";

                JToken maneuverArray = (JArray) jsonObject["route"]?["legs"]?[0]?["maneuvers"];
                if (maneuverArray != null)
                {
                    foreach (var maneuversSource in maneuverArray)
                    {
                        alterTourItem.RouteInformation +=
                            $"{(int) maneuversSource["index"] + 1}. {maneuversSource["narrative"]}; \n" +
                            $"\tDirection distance {maneuversSource["distance"]}km; \n" +
                            $"\tTime for Direction {maneuversSource["formattedTime"]}\n\n";
                    }
                }

            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to Parse JSON \n Details: {e}");
                Console.WriteLine(e);
            }

        }

        public void GetImage(TourItem tourItem)
        {
            try
            {
                log.Info($"Try to Get Image");
                tourItemDAO.GetTourMapImage(tourItem);
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to Get Image \n Details: {e}");
                Console.WriteLine(e);
            }
        }

        public void CleanUpImages(ObservableCollection<TourItem> tourItems)
        {
            try
            {
                log.Info($"Try to clean Image");
                tourItemDAO.CleanUpImages(tourItems);
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to clean Images \n Details: {e}");
                Console.WriteLine(e);
            }
        }

        public string SaveFileDialog()
        {
            try
            {
                log.Info($"Try to open Save File Dialog");
                var dialog = new SaveFileDialog();
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                dialog.FileName = "TourData"; // Default file name
                dialog.DefaultExt = ".pdf"; // Default file extension
                dialog.Filter = "Tour PDF (.pdf)|*.pdf"; // Filter files by extension

                // Show save file dialog box
                bool? result = dialog.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    return dialog.FileName;
                }
            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to open Save File Dialog \n Details: {e}");
                Console.WriteLine(e);
            }

            return "report.pdf";
        }

        public void PrintSpecificTourReport(TourItem tourItem)
        {
            try
            {
                log.Info($"Generating PDF from Tour Data");
                string documentName = SaveFileDialog();
                // C:\YourAmazingData\AmazingTour.pdf
                var writer = new PdfWriter(documentName);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, PageSize.A4, false);
                Image tourMapImage = new Image(ImageDataFactory.Create(tourItem.RouteImagePath));
                // C:\YourAmazingData\map1.png
                Table table = new Table(UnitValue.CreatePercentArray(10)).UseAllAvailableWidth();
                
                table.AddCell("Date");
                table.AddCell("Duration in Minutes");
                table.AddCell("Distance in km");
                table.AddCell("Elevation Gain in m");
                table.AddCell("Sleep Time in Minutes");
                table.AddCell("Step Counter");
                table.AddCell("Intake Calories");
                table.AddCell("Weather");
                table.AddCell("Rating");
                table.AddCell("Notice");

                foreach (var logItem in tourItem.Log)
                {
                    table.AddCell($"{logItem.Date}");
                    table.AddCell($"{logItem.DurationTime}");
                    table.AddCell($"{logItem.Distance}");
                    table.AddCell($"{logItem.ElevationGain}");
                    table.AddCell($"{logItem.SleepTime}");
                    table.AddCell($"{logItem.StepCounter}");
                    table.AddCell($"{logItem.IntakeCalories}");
                    table.AddCell($"{logItem.Weather}");
                    table.AddCell($"{logItem.Rating}");
                    table.AddCell($"{logItem.Notice}");
                }

                document.Add(new Paragraph($"{tourItem.TourName}").SetFontSize(25).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph($"Tour Facts").SetFontSize(20));
                document.Add(new Paragraph($"Your Tour started in {tourItem.Start} and ended in {tourItem.Destination}"));
                document.Add(new Paragraph($"Your Tour Description {tourItem.TourDescription}"));
                document.Add(new Paragraph($"Your Tour was {tourItem.TourDistance}km long"));
                document.Add(new Paragraph($"Your Tour was a {tourItem.RouteType} Route Type"));
                document.Add(new Paragraph($"Your Tour co2 emissons were {tourItem.FuelUsed * 2.64}kg"));
                document.Add(new Paragraph($"Tour Map").SetFontSize(20));
                document.Add(tourMapImage);
                document.Add(new Paragraph($"Tour Directions").SetFontSize(20));
                document.Add(new Paragraph($"{tourItem.RouteInformation}"));
                document.Add(new Paragraph($"Tour Logs").SetFontSize(20));
                document.Add(table);

                MakeReport(pdf, document, documentName);

            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to save data in document object \n Details: {e}");
                Console.WriteLine(e);
            }

        }

        public void PrintSumerizeTourReport(ObservableCollection<TourItem> tourItems)
        {
            try
            {
                log.Info($"Generating Summerize Tour Report");
                string documentName = SaveFileDialog();
                var writer = new PdfWriter(documentName);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, PageSize.A4, false);
                float totalKilometersDriven = 0, totalFuelConsumption = 0, totalKilometersWalked = 0, totalKilometersBicycled = 0, totalTourKilometers = 0;
                int toursDriven = 0, toursWalked = 0, toursBicycled = 0, elevaitionGainUp = 0, elevaitionGainDown = 0, durationTime = 0, sleepTime = 0, stepCounter = 0, intakeCalories = 0;
                string tourNames = "";

                foreach (var item in tourItems)
                {
                    if (item.RouteType is RouteTypeEnum.Fastest or RouteTypeEnum.Shortest)
                    {
                        totalKilometersDriven += item.TourDistance;
                        totalFuelConsumption += item.FuelUsed;
                        toursDriven++;
                    }
                    else if (item.RouteType is RouteTypeEnum.Pedestrian)
                    {
                        totalKilometersWalked += item.TourDistance;
                        toursWalked++;
                    }
                    else if (item.RouteType is RouteTypeEnum.Bicycle)
                    {
                        totalKilometersBicycled += item.TourDistance;
                        toursBicycled++;
                    }
                    tourNames += $"{item.TourName}\n";
                    totalTourKilometers += item.TourDistance;
                    foreach (var logItem in item.Log)
                    {
                        if (logItem.ElevationGain > 0)
                        {
                            elevaitionGainUp += logItem.ElevationGain;
                        }
                        else
                        {
                            elevaitionGainDown -= logItem.ElevationGain;
                        }

                        durationTime = logItem.DurationTime;
                        sleepTime = logItem.SleepTime;
                        stepCounter = logItem.StepCounter;
                        intakeCalories = logItem.IntakeCalories;

                    }
                }

                document.Add(new Paragraph("Summerize Tour Report!").SetFontSize(25).SetTextAlignment(TextAlignment.CENTER));
                document.Add(new Paragraph($"Tour Facts").SetFontSize(20));
                document.Add(new Paragraph($"{totalKilometersDriven} Kilometers driven (Tours {toursDriven})"));
                document.Add(new Paragraph($"{totalKilometersWalked} Kilometers walked (Tours {toursWalked})"));
                document.Add(new Paragraph($"{totalKilometersBicycled} Kilometers bicycled (Tours {toursBicycled})"));
                document.Add(new Paragraph($"Your Total co2 emissions were {totalFuelConsumption * 2.64}kg"));
                document.Add(new Paragraph($"You Walked {elevaitionGainUp}m up"));
                document.Add(new Paragraph($"You Walked {elevaitionGainDown}m down"));
                document.Add(new Paragraph($"Total Tour Duration: {durationTime} minutes"));
                document.Add(new Paragraph($"Total Tour Distance: {totalTourKilometers}km"));
                document.Add(new Paragraph($"Total Tour Sleep time: {sleepTime} minutes"));
                document.Add(new Paragraph($"Total Tour Step Counter: {stepCounter} Steps"));
                document.Add(new Paragraph($"Total Tour Intake Calories: {intakeCalories} Calories"));
                document.Add(new Paragraph($"Tours you made").SetFontSize(20));
                document.Add(new Paragraph($"{tourNames}"));

                MakeReport(pdf, document, documentName);

            }
            catch (Exception e)
            {
                log.Error($"Error occoured after trying to save data in document object \n Details: {e}");
                Console.WriteLine(e);
            }
        }

        private void MakeReport(PdfDocument pdf, Document document, string documentName)
        {
            try
            {
                log.Info($"Make PDF");
                string path = documentName;
                document.Close();

                log.Info($"Try to open Generated PDF");
                var p = new Process {StartInfo = new ProcessStartInfo(path) {UseShellExecute = true}};
                p.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to save pdf \n Details: {e}");
            }
        }

        public void SaveTourDataJson(ObservableCollection<TourItem> tourItems)
        {
            try
            {
                log.Info($"Try to Save Tour Data JSON");
                tourItemDAO.SaveTourDataJson(tourItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to Save Tour Data JSON \n Details: {e}");
            }
        }

        public void LoadTourDataJson(ObservableCollection<TourItem> tourItems)
        {
            try
            {
                log.Info($"Try to load Tour Data JSON");
                tourItemDAO.LoadTourDataJson(tourItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error($"Error occoured after trying to load Tour Data JSON \n Details: {e}");
            }
        }
    }
}