using System;

namespace Tourplanner.Models
{
    public class LogItem
    {
        public int TourId { get; set; }
        public int LogId { get; set; }
        public DateTime Date { get; set; }
        public int DurationTime { get; set; }
        public int Distance { get; set; }
        public int ElevationGain { get; set; }
        public int SleepTime { get; set; }
        public int StepCounter { get; set; }
        public int IntakeCalories { get; set; }
        public string Weather { get; set; }
        public string Rating { get; set; }
        public string Notice { get; set; }
    }
}