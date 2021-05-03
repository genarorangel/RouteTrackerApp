using RouteTrackerApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using MapKit;
using UIKit;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using RouteTrackerApp.Messages;

namespace RouteTrackerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeMap : ContentPage
    {
        private bool returning;
        private Pin stopPin = new Pin();
        private Pin startPin = new Pin();
        private Pin backPin = new Pin();
        private double totalDistance;
        private List<Plugin.Geolocator.Abstractions.Position> positions = new List<Plugin.Geolocator.Abstractions.Position>();
        private bool stopped;

        public bool Returning { get => returning; set => returning = value; }
        public Pin StopPin { get => stopPin; set => stopPin = value; }
        public Pin StartPin { get => startPin; set => startPin = value; }
        public Pin BackPin { get => backPin; set => backPin = value; }
        public double TotalDistance { get => totalDistance; set => totalDistance = value; }
        public List<Plugin.Geolocator.Abstractions.Position> Positions { get => positions; set => positions = value; }
        public bool Stopped { get => stopped; set => stopped = value; }
        public EmployeeMap()
        {
            InitializeComponent();
            MessagesSubscriptions();
            StartServiceAsync();

            Returning = false;
            ServiceLogic.BackLocationAddress = "Não houve volta.";
            StopButton.IsEnabled = false;
            StopButton.IsVisible = true;
            EndButton.IsEnabled = false;
            EndButton.IsVisible = false;
            ComeBackButton.IsEnabled = false;
            ComeBackButton.IsVisible = false;

            //LocationLogic.GetGeolocator(PermissionsLogic.IsLocationAccessPermitted);

        }
        protected async override void OnAppearing()
        {
            //Quando se inicia a página do mapa, adiciona-se os pins de início e fim da rota 

            StartPin.Address = ServiceLogic.StartAddress;
            StartPin.Position = new Xamarin.Forms.Maps.Position(ServiceLogic.StartPosition.Latitude, ServiceLogic.StartPosition.Longitude);
            StopPin.Address = ServiceLogic.StopAddress;
            StopPin.Position = new Xamarin.Forms.Maps.Position(ServiceLogic.StopPosition.Latitude, ServiceLogic.StopPosition.Longitude);
            StartPin.Label = "Local de Partida";
            StopPin.Label = "Local do Serviço";
            RoutesMap.Pins.Add(StartPin);
            RoutesMap.Pins.Add(StopPin);

            MapSpan span = MapLogic.GetMapSpan(StopPin.Position);
            RoutesMap.MoveToRegion(span);
            RoutesMap.IsShowingUser = true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            RoutesMap.Pins.Clear();
            RoutesMap.MapElements.Clear();
        }

        private async void StopButton_Clicked(object sender, EventArgs e)
        {
            //Similar à partida, atualiza o status dos botões e variáveis pertinentes, cria a string com latitude e longitude de parada,
            //e salva a data e hora da chegada
            await StopServiceAsync();
            Stopped = true;
            ServiceLogic.DistanceUntilService = (Math.Round(totalDistance, 3).ToString() + " km").Replace(".",",");
            ComeBackButton.IsVisible = true;
            ComeBackButton.IsEnabled = true;
            EndButton.IsVisible = true;
            StopButton.IsEnabled = false;
            ServiceLogic.StopDatetime = DateTime.Now;
            EndButton.IsEnabled = true;
        }

        private async void ComeBackButton_Clicked(object sender, EventArgs e)
        {
            //Se houver volta após o término do serviço, inicia umanova etapa da corrida
            await StartServiceAsync();
            ComeBackButton.IsEnabled = false;
            Returning = true;
        }

        private async void EndButton_Clicked(object sender, EventArgs e)
        {
         
            if (Returning)
            {
                await StopServiceAsync();
                //Se houve volta, busca as informações da rota de volta
                var BackLocation = await LocationLogic.GetLocation().ConfigureAwait(false);
                ServiceLogic.BackLocationAddress = await LocationLogic.GetAddressFromLocation(BackLocation);
                BackPin.Address = ServiceLogic.BackLocationAddress;
                BackPin.Label = "Local de retorno";
                BackPin.Position = new Xamarin.Forms.Maps.Position(BackLocation.Latitude, BackLocation.Longitude);
            }


            //Insere os dados de serviço no banco de dados
            string poly = GooglePolylineLogic.GooglePoints.Encode(Positions);
            await ServiceLogic.InsertService(TotalDistance, Returning, poly);
            await Navigation.PushAsync(new EmployeePage());

        }

        private async Task StartServiceAsync()
        {
            if (!CrossGeolocator.Current.IsListening)
                _ = await LocationLogic.GetGeolocator(PermissionsLogic.IsLocationAccessPermitted);
            var message = new StartServiceMessage();
            MessagingCenter.Send(message, "ServiceStarted");
        }

        private async Task StopServiceAsync()
        {
            if (CrossGeolocator.Current.IsListening)
                await CrossGeolocator.Current.StopListeningAsync();
            var message = new StopServiceMessage();
            MessagingCenter.Send(message, "ServiceStopped");
        }

        private void MessagesSubscriptions()
        {
            MessagingCenter.Subscribe<LocationMessage>(this, "Location", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var position = message.position;
                    GPSAccuracy.Text = "Precisão: " + position.Accuracy.ToString()+" m";
                    if (position.Accuracy <= 20)
                    {
                        if (position.CalculateDistance(ServiceLogic.StopPosition, GeolocatorUtils.DistanceUnits.Kilometers) < 0.05 && !Stopped)
                            StopButton.IsEnabled = true;

                        var speedKm_h = position.Speed * 3.6;
                        SpeedLabel.Text = "Velocidade: "+(speedKm_h.ToString() + "km/h");
                        if (Positions.Count > 0)
                        {

                            Polyline polyline = MapLogic.CreatPolylineFromPosition(Positions);
                            RoutesMap.MapElements.Add(polyline);
                            TotalDistance += Positions.LastOrDefault().CalculateDistance(position, GeolocatorUtils.DistanceUnits.Kilometers);
                            TotalKms.Text = "Distância percorrida: " + TotalDistance.ToString() + " km";
                        }
                        if (position != Positions.LastOrDefault())
                            Positions.Add(position);
                        MapSpan span = MapLogic.GetMapSpan(new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude));
                        RoutesMap.MoveToRegion(span);
                    }
                });
            });
            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStarted", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Atenção", "Serviço de localização iniciado", "OK");
                });
            });
            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Atenção", "Serviço de localização encerrado", "OK");
                });
            });
            MessagingCenter.Subscribe<LocationErrorMessage>(this, "LocationError", message =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Falha!", "Serviço de localização falhou", "OK");
                });
            });
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await StopServiceAsync();
            await Navigation.PushAsync(new EmployeePage());
        }
    }
}