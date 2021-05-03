using System;
using System.Collections.Generic;
using System.Text;

namespace RouteTrackerApp.Model
{
    public class Routes
    {
        //Essa classe foi gerada inserindo a resposta do API Google Maps distancematrix no site https://www.jsonutils.com/

        private static RouteInformation currentRoute = new RouteInformation();

        public static RouteInformation CurrentRoute { get => currentRoute; set => currentRoute = value; }

        public class Distance
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class Duration
        {
            public string text { get; set; }
            public int value { get; set; }
        }
        public class DurationInTraffic
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class Element
        {
            public Distance distance { get; set; }
            public Duration duration { get; set; }
            public DurationInTraffic duration_in_traffic { get; set; }
            public string status { get; set; }
        }

        public class Row
        {
            public IList<Element> elements { get; set; }
        }

        public class RouteInformation
        {
            public IList<string> destination_addresses { get; set; }
            public IList<string> origin_addresses { get; set; }
            public IList<Row> rows { get; set; }
        }


    }
}
