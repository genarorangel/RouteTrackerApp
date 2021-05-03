using System;
using System.Collections.Generic;
using System.Text;

namespace RouteTrackerApp.Logic
{
    public class TimeLogic
    {
        public static int DaTimeNowinSeconds(DateTime dateTime)
        {

            TimeSpan span = dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return (int)span.TotalSeconds; 
        }
    }
}
