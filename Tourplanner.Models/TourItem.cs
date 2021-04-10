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

    }
}
