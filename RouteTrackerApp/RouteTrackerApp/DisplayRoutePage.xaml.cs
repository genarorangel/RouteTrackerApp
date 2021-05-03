using RouteTrackerApp.Model;
using RouteTrackerApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RouteTrackerApp.Logic;
using Xamarin.Forms.Maps;

namespace RouteTrackerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayRoutePage : ContentPage
    {
        List<Plugin.Geolocator.Abstractions.Position> positions = new List<Plugin.Geolocator.Abstractions.Position>();
        Servico servico = new Servico();
        private Pin stopPin = new Pin();
        private Pin startPin = new Pin();
        private Pin backPin = new Pin();
        private int serviceNumber;
        public Pin StopPin { get => stopPin; set => stopPin = value; }
        public Pin StartPin { get => startPin; set => startPin = value; }
        public Pin BackPin { get => backPin; set => backPin = value; }
        public int ServiceNumber { get => serviceNumber; set => serviceNumber = value; }


        public DisplayRoutePage(int Number)
        {
            serviceNumber = Number;
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            RouteMap.IsVisible = false;
            await GetServiceAsync();
            StartPin.Address = servico.LOCAL_SAIDA;
            StartPin.Position = await MapLogic.GetMapPositionFromAdressAsync(servico.LOCAL_SAIDA);
            StopPin.Address = servico.LOCAL_CHEGADA;
            StopPin.Position = await MapLogic.GetMapPositionFromAdressAsync(servico.LOCAL_CHEGADA);
            StartPin.Label = "Local de Partida";
            StopPin.Label = "Local do Serviço";
            RouteMap.Pins.Add(StartPin);
            RouteMap.Pins.Add(StopPin);

            if (servico.VOLTA == "Sim")
            {
                BackPin.Address = servico.LOCAL_VOLTA;
                BackPin.Position = await MapLogic.GetMapPositionFromAdressAsync(servico.LOCAL_VOLTA);
                BackPin.Label = "Local da Volta";
                RouteMap.Pins.Add(BackPin);
            }

            Polyline expectedpolyline = await GetPolylineAsync();
            expectedpolyline.StrokeColor = Color.DodgerBlue;
            Polyline polyline = GetDecodedPolyline(servico.Rota);
            RouteMap.MapElements.Add(expectedpolyline);
            RouteMap.MapElements.Add(polyline);
            MapSpan span = MapLogic.GetMapSpan(StartPin.Position);
            RouteMap.MoveToRegion(span);
            LoadingIndicator.IsVisible = false;
            RouteMap.IsVisible = true;
        }

        private async Task<Polyline> GetPolylineAsync()
        {
            var startPoint = LocationLogic.LocationStringBuilder(await LocationLogic.GetLocationFromAddress(servico.LOCAL_SAIDA));
            var stopPoint = LocationLogic.LocationStringBuilder(await LocationLogic.GetLocationFromAddress(servico.LOCAL_CHEGADA));
            var polyline = await GoogleAPIRequest.GetPolyline(startPoint, stopPoint);
            string encodedPolyline = polyline.routes.FirstOrDefault().overview_polyline.points;
            return GetDecodedPolyline(encodedPolyline);
        }

        private async Task GetServiceAsync()
        {
            servico = (await App.client.GetTable<Servico>().Where(u => u.NUMERO == serviceNumber).ToListAsync()).FirstOrDefault();
        }

        private Polyline GetDecodedPolyline(string encodedPolyline)
        {
           var points = GooglePolylineLogic.GooglePoints.Decode(encodedPolyline).ToList();
           return  MapLogic.CreatPolylineFromPosition(points);

        }
    }
}