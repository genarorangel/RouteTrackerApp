using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RouteTrackerApp.Logic
{
    public static class LocationLogic
    {
        //Esta classe implementa a lógica referente a localização do usuário
        

        public async static Task<IGeolocator> GetGeolocator(bool LocationPermission)
        {
            //Se há acesso a localização, inicia o serviço de geolocalização e começa a escutar a localização do usuário 
            if (LocationPermission)
            {
                var locator = CrossGeolocator.Current;
                await locator.StartListeningAsync(TimeSpan.FromSeconds(3), 20, true, new ListenerSettings
                {
                    ActivityType = ActivityType.AutomotiveNavigation,
                    AllowBackgroundUpdates = true,
                    DeferLocationUpdates = true,
                    DeferralDistanceMeters = 1,
                    DeferralTime = TimeSpan.FromSeconds(1),
                    ListenForSignificantChanges = true,
                    PauseLocationUpdatesAutomatically = false,

                });
                locator.DesiredAccuracy = 5;
                return locator;
            }
            else
                return null;
        }

        public async static Task<Position> GetLocation()
        {
            //Retorna a localização atual do usuário
            //baseado em https://jamesmontemagno.github.io/GeolocatorPlugin/CurrentLocation.html
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 5;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, false);

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }

            if (position == null)
                return null;

            return position;
        }

        public async static Task<Position> GetLocationFromAddress(string Address)
        {
            //Essa função obtém uma localização inserindo um endereço
            try
            {
                var locator = CrossGeolocator.Current;
                var locations = await locator.GetPositionsForAddressAsync(Address);
                Position location = locations.FirstOrDefault();
                return location;

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
                await Application.Current.MainPage.DisplayAlert("Erro", fnsEx.Message, "OK");
                return null;
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                return null;
            }

        }

        public async static Task<string> GetAddressFromLocation(Position position)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                var addresses = await locator.GetAddressesForPositionAsync(position);
                var address = addresses.FirstOrDefault();
                string addressString = (address.Thoroughfare + ", " + address.SubThoroughfare + "- " + address.SubLocality + ", "
                    + address.SubAdminArea + "- " + address.AdminArea + ", " + address.PostalCode + ", " + address.CountryName); 
                return addressString;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", e.Message, "OK");
                return null;
            }
        }

        public static string LocationStringBuilder(Position position)
        {
            //Cria uma string com Latitude e Longitude referentes à localização do usuário
            string Latitude = position.Latitude.ToString().Replace(',', '.');
            string Longitude = position.Longitude.ToString().Replace(',', '.');
            return (Latitude + "," + Longitude);
        }



    }
}
