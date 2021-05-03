using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xamarin.Forms.Maps;

namespace RouteTrackerApp.Logic
{
    public class MapLogic
    {
        //public static Xamarin.Forms.Maps.Position FirstPosition, LastPosition;
        public static Polyline CreatPolylineFromPosition(List<Plugin.Geolocator.Abstractions.Position> PositionsList)
        {
            //Cria uma linha representando a rota a partir de uma lista de posições e indica a posição incial e final da rota
            Polyline newPolyline = new Polyline();
            newPolyline.StrokeColor = Color.Red;
            newPolyline.StrokeWidth = 10;
            List<Xamarin.Forms.Maps.Position> positions = PositionsList.ConvertAll(p => new Xamarin.Forms.Maps.Position(p.Latitude, p.Longitude));
            positions.ForEach(x => newPolyline.Geopath.Add(x));
            SetPolyline(newPolyline);
            return newPolyline;
        }

        public static MapSpan GetMapSpan(Xamarin.Forms.Maps.Position position)
        {
            //Cria um MapSpan adequado de acordo com a posição fornecida
            Xamarin.Forms.Maps.Position center = position;
            MapSpan span = new MapSpan(center, 0.1, 0.1);
            return span;
        }

        private static Polyline polyline;

        public static Polyline GetPolyline()
        {
            return polyline;
        }
        public static void SetPolyline(Polyline newPolyline)
        {
            polyline = newPolyline;
        }

        public static async System.Threading.Tasks.Task<Xamarin.Forms.Maps.Position> GetMapPositionFromAdressAsync(string adress)
        {
            var position = await LocationLogic.GetLocationFromAddress(adress);
            return new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
        }
    }
}
