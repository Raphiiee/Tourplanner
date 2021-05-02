using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows.Documents;
using Npgsql;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class Database : IDataAccess
    {
        private string _connetionString = ConfigurationManager.AppSettings.Get("DBConnection");
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Database.cs");
        private static Database _instance;
        private readonly NpgsqlConnection _conn;

        private Database()
        {
            log.Info($"Try to connect to database");
            _conn = new NpgsqlConnection(_connetionString);
        }

        public static Database Instance()
        {
            if (_instance is null)
            {
                _instance = new Database();
            }

            return _instance;
        }

        public List<TourItem> GetItems()
        {
            log.Info($"Try to get tour data from database");

            string sql = "SELECT * FROM tours ORDER BY tourid";
            List<TourItem> tours = new List<TourItem>();  

            _conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, _conn);
            cmd.Prepare();

            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                TourItem temp = new TourItem(){
                    TourId = reader.GetInt32(0),
                    TourName = reader.GetString(1),
                    Start = reader.GetString(2),
                    Destination = reader.GetString(3),
                    TourDescription = reader.GetString(4),
                    RouteInformation = reader.GetString(5),
                    RouteImagePath = reader.GetString(6),
                    TourDistance = reader.GetFloat(7),
                    FuelUsed = reader.GetFloat(8),
                    RouteType = (RouteTypeEnum) reader.GetFloat(9),
                    Log = new ObservableCollection<LogItem>(){}
                };

                tours.Add(temp);
            }
            _conn.Close();

            sql = "SELECT * FROM logs ORDER BY logid";
            List<LogItem> logs = new List<LogItem>();  

            _conn.Open();
            cmd = new NpgsqlCommand(sql, _conn);
            cmd.Prepare();

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                LogItem temps = new LogItem(){
                    TourId = reader.GetInt32(1),
                    Date = (DateTime) reader.GetTimeStamp(2),
                    DurationTime = reader.GetInt32(3),
                    Distance = reader.GetInt32(4),
                    ElevationGain = reader.GetInt32(5),
                    SleepTime = reader.GetInt32(6),
                    StepCounter = reader.GetInt32(7),
                    IntakeCalories = reader.GetInt32(8),
                    Weather = reader.GetString(9),
                    Rating = reader.GetString(10),
                    Notice = reader.GetString(11),
                };

                logs.Add(temps);
            }
            _conn.Close();

            foreach (var logItem in logs)
            {
                tours[logItem.TourId - 1].Log = new ObservableCollection<LogItem>(){logItem};
            }

            return tours;
            // return new List<TourItem>()
            // {
            //     new TourItem() {TourId = 1, TourName = "Tour 1", Destination = "Wien",       Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/1998)}}},
            //     new TourItem() {TourId = 2, TourName = "Tour 1", Destination = "Wien",       Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/1998)}}},
            //     new TourItem() {TourId = 3, TourName = "Tour 2", Destination = "Salzburg",   Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/1999)}}},
            //     new TourItem() {TourId = 4, TourName = "Tour 3", Destination = "Vorarlberg", Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/2000)}}},
            //     new TourItem() {TourId = 5, TourName = "Apfel",  Destination = "Grado",      Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/2001)}}},
            //     new TourItem() {TourId = 6, TourName = "Appel",  Destination = "Korneuburg", Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/2002)}}},
            //     new TourItem() {TourId = 7, TourName = "Banane", Destination = "Stockerau",  Start = "Hollabrunn", Log = new ObservableCollection<LogItem>(){new LogItem(){Distance = 1112, Notice = "ABCD", Date = new DateTime(31/03/2003)}}}
            // };
        }

        public List<TourItem> AddItems()
        {
            log.Info($"Try to add items to database");
            return new List<TourItem>()
            {
                new TourItem() {TourName = "New Tour"}
            };
        }

        public void DeleteItems(TourItem deleteTourItem)
        {
            log.Info($"Try to delete tour item from database");
            // Delete Tour ITEM from Database
        }

        public void DeleteLogItems(LogItem deleteLogItem)
        {
            log.Info($"Try to delete log item from database");
            // Delete Log ITEM from fileSystem
        }

        public void AlterTourDetails(TourItem alterTourItem)
        {
            log.Info($"Try to delete alter tour item from database");
            // Alter Tour ITem in Database
        }
    }
}