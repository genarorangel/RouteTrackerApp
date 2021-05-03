using dotMorten.Xamarin.Forms;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RouteTrackerApp.Helpers;
using RouteTrackerApp.Logic;
using RouteTrackerApp.Model;
using SearchPlaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RouteTrackerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeePage : ContentPage
    {
        //Página destinada ao socorrista e tem como objetivo monitorar e gravar corretamente as rotas no banco de dados
        List<Plugin.Geolocator.Abstractions.Position> Positions = new List<Plugin.Geolocator.Abstractions.Position>();
        private Routes.RouteInformation routeInformation { get; set; }
        private Pessoa socorrista = new Pessoa();

        public EmployeePage()
        {
            InitializeComponent();
            //Busca a pessoa que logou no sistema
            socorrista = Logged.GetLogged();
            LabelEmployee.Text = "QRA: "+ socorrista.QRA.ToString();
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string address = SuggestBox.Text;
            if (!CrossGeolocator.Current.IsListening)
            {
                IGeolocator locator = await LocationLogic.GetGeolocator(PermissionsLogic.IsLocationAccessPermitted);
            }
            var location = await LocationLogic.GetLocation();
            string PositionNow = LocationLogic.LocationStringBuilder(location);
            var places = await GoogleAPIRequest.GetPlaces(PositionNow, address);
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;
                var PlacesList = places.predictions.ToList();
                List<string> AddresList = new List<string>();
                PlacesList.ForEach(l => AddresList.Add(l.description));
                SuggestBox.ItemsSource = AddresList;
            }
        }


        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SuggestBox.Text = args.SelectedItem.ToString();
        }


        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                DisplayAlert("Atenção", "Por favor verifique se o endereço está correto.", "OK");
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
                DisplayAlert("Falha!", "Por favor selecione um endereço válido.", "OK");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await PermissionsLogic.GetPermissions();
        }

        private async void StartButton_Clicked(object sender, EventArgs e)
        {
            //Quando o usuário dá a partida, o sistema verifica o número de serviço, que dev conter 7 algarismos
            //Essa lógica pode ser realizada em uma classe separada assim como o login
            if (string.IsNullOrEmpty(ServiceEntry.Text))
                await DisplayAlert("Falha!", "Entre com o número do serviço", "OK");
            else if (ServiceEntry.Text.Length != 7)
                await DisplayAlert("Falha!", "Número do serviço deve conter 7 algarismos", "OK");
            else
            {
                //Então o sistema busca no banco de dados se o serviço já foi adcionado
                //isRunning = true;
                var ServiceMatch = await App.client.GetTable<Servico>().Where(u => u.NUMERO == int.Parse(ServiceEntry.Text)).ToListAsync();
                if (ServiceMatch.Count > 0)
                    await DisplayAlert("Falha!", "Serviço já foi adcionado", "OK").ConfigureAwait(false);
                else
                {
                    //O número de serviço também foi setado erroneamente como int e será nvarchar no banco definitivo
                    ServiceLogic.ServiceNumber = int.Parse(ServiceEntry.Text);
                    ServiceLogic.StartPosition = await LocationLogic.GetLocation();
                    ServiceLogic.StopAddress = SuggestBox.Text;
                    ServiceLogic.StopPosition = await LocationLogic.GetLocationFromAddress(ServiceLogic.StopAddress);
                    ServiceLogic.StartDatetime = DateTime.Now;
                    string StartPositionString = LocationLogic.LocationStringBuilder(ServiceLogic.StartPosition);
                    Routes.CurrentRoute = routeInformation = await GoogleAPIRequest.GetRouteInfo(StartPositionString,
                        ServiceLogic.StopAddress);
                    ServiceLogic.StartAddress = routeInformation.origin_addresses.FirstOrDefault();
                    await Navigation.PushAsync(new EmployeeMap()).ConfigureAwait(false);

                }
            }

        }

    }
}