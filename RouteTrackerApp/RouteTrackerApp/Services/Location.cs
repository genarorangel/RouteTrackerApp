using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using RouteTrackerApp.Logic;
using RouteTrackerApp.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RouteTrackerApp.Services
{
    public class Location
    {

        public Location()
        {
        }

        public void setRunningStateLocationService(bool isRunning)
        {
            if (isRunning)
            {
                Application.Current.Properties["locationServiceIsRunning"] = true;
            }
            else
            {
                Application.Current.Properties["locationServiceIsRunning"] = false;
            }
        }
        public bool getRunningStateLocationService()
        {
            bool locationServiceIsRunning;
            if (Application.Current.Properties.ContainsKey("locationServiceIsRunning"))
            {
                locationServiceIsRunning = Convert.ToBoolean(Application.Current.Properties["locationServiceIsRunning"]);
            }
            else
            {
                locationServiceIsRunning = false;
                CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;
            }
            return locationServiceIsRunning;
        }

        public async Task Run(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                System.Diagnostics.Debug.WriteLine(getRunningStateLocationService());
                if (getRunningStateLocationService())
                {
                    token.ThrowIfCancellationRequested();
                    CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
                }
                return;
            }, token);
        }

        private void Current_PositionChanged(object sender, PositionEventArgs e)
        {
            var message = new LocationMessage
            {
                position = e.Position,
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send(message, "Location");
            });
        }
    }
}
