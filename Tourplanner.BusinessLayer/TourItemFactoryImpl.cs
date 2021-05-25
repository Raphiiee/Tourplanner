using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
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
            tourItemDAO = new TourItemDAO(source);
        }

        public IEnumerable<TourItem> GetItems()
        {
            return tourItemDAO.GetItems();
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
            
            log.Info($"Found {foundItems.Count()} for the search term {itemName}");

            return foundItems;
        }

        public TourItem AddTourItem()
        {
            return tourItemDAO.AddItems();
        }

        public void DeleteTourItem(TourItem deleteTourItem)
        {
            tourItemDAO.DeleteItems(deleteTourItem);
        }

        public void AddLogItem(LogItem addLogItem, TourItem selectedTourItem)
        {
            addLogItem.Date = DateTime.Now; 
            addLogItem.TourId = selectedTourItem.TourId;
            tourItemDAO.AddLogItems(addLogItem);
        }

        public void AlterLogItem(TourItem selectedTourItem)
        {
            foreach (var logItem in selectedTourItem.Log)
            {
                tourItemDAO.AlterLogItems(logItem);
            }
        }

        public void DeleteLogItem(LogItem deleteLogItem)
        {
            tourItemDAO.DeleteLogItems(deleteLogItem);
        }

        public void AlterTourDetails(TourItem alterTourItem)
        {
            try
            {
                string tourDataJson = tourItemDAO.GetTourData(alterTourItem);
                
                log.Info($"Try to Parse JSON from Tour Data");
                JObject jsonObject = JObject.Parse(tourDataJson);
                LoadJsonData(alterTourItem,jsonObject);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error(e);
            }
            
            tourItemDAO.GetTourMapImage(alterTourItem);
            tourItemDAO.AlterTourDetails(alterTourItem);
        }

        public void LoadJsonData(TourItem alterTourItem, JObject jsonObject)
        {
            string infoCode = "";
            string infoMessage = "";

            int errorCode = (int) jsonObject["route"]?["routeError"]?["errorCode"];
            if (errorCode > 0)
            {
                infoCode = (string) jsonObject["info"]?["statuscode"];
                infoMessage = (string) jsonObject["info"]?["messages"]?[0];
                throw new ArgumentException($"Tour not Valid Error Code: {errorCode}; Info Code: {infoCode}; Info Messsage: {infoMessage}");
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

        public void GetImage(TourItem tourItem)
        {
            tourItemDAO.GetTourMapImage(tourItem);
        }

        public void CleanUpImages(ObservableCollection<TourItem> tourItems)
        {
            tourItemDAO.CleanUpImages(tourItems);
        }

        public void PrintSpecificTourReport(TourItem tourItem)
        {
            log.Info($"Generating PDF '{tourItem.TourName}.pdf' from Tour Data");
            string documentName = $"{tourItem.TourName}.pdf";
            var writer = new PdfWriter(documentName);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4, false);
            Image tourMapImage = new Image(ImageDataFactory.Create(tourItem.RouteImagePath));
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

        public void PrintSumerizeTourReport(ObservableCollection<TourItem> tourItems)
        {
            log.Info($"Generating Summerize Tour Report");
            string documentName = "Summerize Report.pdf";
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

        private void MakeReport(PdfDocument pdf, Document document, string documentName)
        {
            log.Info($"Make PDF");
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + $"\\{documentName}";
            document.Close();

            try
            {
                log.Info($"Try to open Generated PDF");
                var p = new Process {StartInfo = new ProcessStartInfo(path) {UseShellExecute = true}};
                p.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error(e);
            }
        }
    }
}