using RouteTrackerApp.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace RouteTrackerApp.Logic
{
    class RotaLogic
    {
        //private List<Plugin.Geolocator.Abstractions.Position> positions = new List<Plugin.Geolocator.Abstractions.Position>();
        public static async Task InsertRota(List<Plugin.Geolocator.Abstractions.Position> routePostions)
        {
            routePostions.ForEach(async x =>
             {
                 Rota rota = new Rota()
                 {
                     NUMERO = ServiceLogic.ServiceNumber,
                     LATITUDE = x.Latitude.ToString(),
                     LONGITUDE = x.Longitude.ToString(),
                 };
                 try
                 {
                     await App.client.GetTable<Rota>().InsertAsync(rota);
                 }
                 catch (Exception e)
                 {
                     await Application.Current.MainPage.DisplayAlert("Erro", e.Message, "Ok");

                 }
             });
            /*Rota rota = new Rota()
            {
                IDX = idx,
                NUMERO = ServiceLogic.ServiceNumber,
                LATITUDE = routePostions.FirstOrDefault().Latitude.ToString(),
                LONGITUDE = routePostions.FirstOrDefault().Longitude.ToString(),
                DESTINO = 'N',
                VOLTA = 'N',
            };
            try
            {
                await App.client.GetTable<Rota>().InsertAsync(rota);
                idx++;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", e.Message, "Ok");

            }*/

        }
    }
}
