using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using Npgsql;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class Database : IDataAccess
    {
        private string _connetionString = ConfigurationManager.AppSettings.Get("DBConnection");
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Database.cs");
        private static Database _instance;
        private NpgsqlConnection _conn;

        private Database()
        {
            log.Info($"Try to connect to database");
            if (!(_connetionString is null))
            {
                Connect();
            }
        }

        public static Database Instance(string test="")
        {
            if (test.Length > 1)
            {
                _instance = new Database();
                _instance._connetionString = test;
                _instance.Connect();
            }
            if (_instance is null)
            {
                _instance = new Database();
            }

            return _instance;
        }

        private void Connect()
        {
            _conn = new NpgsqlConnection(_connetionString);
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
                    Log = new ObservableCollection<LogItem>(){},
                };

                if (temp.RouteImagePath.Contains(".png"))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(temp.RouteImagePath);
                    image.EndInit();
                    temp.RouteImage = image;
                }

                tours.Add(temp);
            }
            _conn.Close();

            sql = "SELECT * FROM logs ORDER BY logid";

            _conn.Open();
            cmd = new NpgsqlCommand(sql, _conn);
            cmd.Prepare();

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                LogItem temps = new LogItem(){
                    LogId = reader.GetInt32(0),
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

                foreach (var tourItem in tours)
                {
                    if (tourItem.TourId == temps.TourId)
                    {
                        tourItem.Log.Add(temps);
                        break;
                    }
                }
                
            }
            _conn.Close();

            return tours;
        }

        public List<TourItem> AddItems()
        {
            log.Info($"Try to add a new tour to database");
            string sql = "SELECT add_new_tour()";
            int tourId = 0;

            _conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, _conn);
            cmd.Prepare();

            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tourId = reader.GetInt32(0);
            }
            _conn.Close();
            
            return new List<TourItem>()
            {
                new TourItem() {TourName = "New Tour", TourId = tourId}
            };
        }

        public void DeleteItems(TourItem deleteTourItem)
        {
            // Delete all Logs from tour
            foreach (var logItem in deleteTourItem.Log)
            {
                DeleteLogItems(logItem);
            }

            // delete Tour
            log.Info($"Try to delete tour item from database");
            string sqlDelteTour = "DELETE FROM tours WHERE tourid=@tid";

            _conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sqlDelteTour, _conn);
            cmd.Parameters.AddWithValue("tid", deleteTourItem.TourId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            _conn.Close();
        }

        public void DeleteLogItems(LogItem deleteLogItem)
        {
            log.Info($"Try to delete logid {deleteLogItem.LogId} item from database");

            string sqlDelteLog = "DELETE FROM logs WHERE logid=@lid AND tourid=@tid";

            _conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sqlDelteLog, _conn);
            cmd.Parameters.AddWithValue("lid", deleteLogItem.LogId);
            cmd.Parameters.AddWithValue("tid", deleteLogItem.TourId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            _conn.Close();
        }

        public void AddLogItems(LogItem addLogItem)
        {
            log.Info($"Try to add a new tour log to database");
            string sql = "SELECT add_new_log(@tid)";
            int logId = 0;

            _conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("tid", addLogItem.TourId);
            cmd.Prepare();

            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                logId = reader.GetInt32(0);
            }

            addLogItem.LogId = logId;
            _conn.Close();
        }

        public void AlterLogItems(LogItem alterItem)
        {
            string sqlAlterLogs = "UPDATE logs SET datee=@date, durationtime=@dtime, distance=@distance, elevationgain=@elevation," +
                                    " sleeptime=@stime, stepcounter=@counter, intakecalories=@calorie, weather=@weather, rating=@rating, notices=@notice" +
                                  " WHERE logid=@lid AND tourid=@tid";

            _conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sqlAlterLogs, _conn);
            cmd.Parameters.AddWithValue("date", alterItem.Date);
            cmd.Parameters.AddWithValue("dtime", alterItem.DurationTime);
            cmd.Parameters.AddWithValue("distance", alterItem.Distance);
            cmd.Parameters.AddWithValue("elevation", alterItem.ElevationGain);
            cmd.Parameters.AddWithValue("stime", alterItem.SleepTime);
            cmd.Parameters.AddWithValue("counter", alterItem.StepCounter);
            cmd.Parameters.AddWithValue("calorie", alterItem.IntakeCalories);
            cmd.Parameters.AddWithValue("weather", alterItem.Weather);
            cmd.Parameters.AddWithValue("rating", alterItem.Rating);
            cmd.Parameters.AddWithValue("notice", alterItem.Notice);
            cmd.Parameters.AddWithValue("lid", alterItem.LogId);
            cmd.Parameters.AddWithValue("tid", alterItem.TourId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            _conn.Close();
            
        }

        public void AlterTourDetails(TourItem alterTourItem)
        {
            log.Info($"Try to delete alter tour item from database");

            string sqlAlterTour = "UPDATE tours SET tourname=@tname, startp=@start, destination=@dest, tourdescription=@tdesc, " +
                                        "routeinformation=@rinf, routeimagepath=@img, tourdistance=@tdist, fuelused=@fuel, routetype=@rtype" + 
                                  " WHERE tourid=@tid";

            _conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sqlAlterTour, _conn);
            cmd.Parameters.AddWithValue("tname", alterTourItem.TourName);
            cmd.Parameters.AddWithValue("start", alterTourItem.Start);
            cmd.Parameters.AddWithValue("dest", alterTourItem.Destination);
            cmd.Parameters.AddWithValue("tdesc", alterTourItem.TourDescription);
            cmd.Parameters.AddWithValue("rinf", alterTourItem.RouteInformation);
            cmd.Parameters.AddWithValue("img", alterTourItem.RouteImagePath);
            cmd.Parameters.AddWithValue("tdist", alterTourItem.TourDistance);
            cmd.Parameters.AddWithValue("fuel", alterTourItem.FuelUsed);
            cmd.Parameters.AddWithValue("rtype", (int) alterTourItem.RouteType);
            cmd.Parameters.AddWithValue("tid", alterTourItem.TourId);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            _conn.Close();
        }
    }
}