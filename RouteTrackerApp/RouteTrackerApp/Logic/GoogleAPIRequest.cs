using Newtonsoft.Json;
using RouteTrackerApp.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RouteTrackerApp.Logic
{
    public static class GoogleAPIRequest
    {
        //Essa classe utiliza um cliente HTTP e o plugin Newtonsoft.Json para desserializar a resposta em Json para a classe Route.Information criada
        private static string uri;
        private static string key = "PUT_YOUR_GOOGLE_KEY";
        public static string Key { get => key; set => key = value; }
        private static  int  radius = 200000;

        public async static Task<Routes.RouteInformation> GetRouteInfo(string startLocation, string stopLocation)
        {
            //Essa função utiliza coordenadas de partida e chegada para retornar informações da rota
            using (HttpClient client = new HttpClient())
            {
                uri = $"https://maps.googleapis.com/maps/api/distancematrix/json?&origins={startLocation}&destinations={stopLocation}&language=pt-BR&departure_time=now&key={key}";
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var routeinfo = JsonConvert.DeserializeObject<Routes.RouteInformation>(json);
                return routeinfo;
            }
        }

        public async static Task<Places.PlacesPredictions> GetPlaces(string startLocation, string addressToGo)
        {
            //Essa função utiliza coordenadas de partida e chegada para retornar informações da rota
         
            using (HttpClient client = new HttpClient())
            {
                uri = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={addressToGo}=&location={startLocation}&radius={radius}&language=pt_BR&key={key}";
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var placesinfo = JsonConvert.DeserializeObject<Places.PlacesPredictions>(json);
                return placesinfo;
            }
        }

        public async static Task<EncodedPolyine.Example> GetPolyline(string startLocation, string stopLocation)
        {
            using (HttpClient client = new HttpClient())
            {
                uri = $"https://maps.googleapis.com/maps/api/directions/json?origin={startLocation}&destination={stopLocation}&key={key}";
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                var polyline = JsonConvert.DeserializeObject<EncodedPolyine.Example>(json);
                return polyline;
            }

        }

    }
}

