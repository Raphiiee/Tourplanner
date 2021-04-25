using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tourplanner.Models
{
    public class TourItem
    {
        public ObservableCollection<LogItem> Log { get; set; }
        public string TourName { get; set; }
        public int TourId { get; set; }
        public string Start { get; set; }
        public string Destination { get; set; }
        public string TourDescription { get; set; }
        public string RouteInformation { get; set; }
        public string RouteImage { get; set; }
        public float TourDistance { get; set; }
        public float FuelUsed { get; set; }
        public RouteTypeEnum RouteType { get; set; }

    }
}
