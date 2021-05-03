using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RouteTrackerApp
{
    public partial class App : Application
    {
        public static string Databaselocation = string.Empty;

        public static MobileServiceClient client = new MobileServiceClient("https://routetracker.azurewebsites.net");

        public static Task MobileService { get; internal set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        public App(string databaseLocation)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            Databaselocation = databaseLocation;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
