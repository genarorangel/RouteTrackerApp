using System;
using System.Collections.Generic;
using System.Text;

namespace RouteTrackerApp.Model
{
    public class EncodedPolyine
    {
      

        public class OverviewPolyline
        {
            public string points { get; set; }
        }

        public class Route
        {
            public OverviewPolyline overview_polyline { get; set; }
        }

        public class Example
        {
            public IList<Route> routes { get; set; }
        }
    }
}
