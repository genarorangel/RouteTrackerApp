using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RouteTrackerApp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RouteTrackerApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationPage : ContentPage
    {
		public List<Position> Positions = new List<Position>();
		double TotalDistance = 0;
		public LocationPage()
        {
			
			InitializeComponent();

        }

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await PermissionsLogic.GetPermissions();
			IGeolocator locator = await LocationLogic.GetGeolocator(PermissionsLogic.IsLocationAccessPermitted);
			Position position = await locator.GetPositionAsync();
			Positions.Add(position);
			LocationsMap.IsShowingUser = true;
			MoveMap(position);
			CrossGeolocator.Current.PositionChanged += Locator_PositionChanged;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			CrossGeolocator.Current.StopListeningAsync();
			CrossGeolocator.Current.PositionChanged -= Locator_PositionChanged;
		}

		private void Locator_PositionChanged(object sender, PositionEventArgs e)
		{
			MoveMap(e.Position);
			TotalDistance = TotalDistance + Positions.LastOrDefault().CalculateDistance(e.Position, GeolocatorUtils.DistanceUnits.Kilometers);
			Positions.Add(e.Position);
		}


		private void MoveMap(Position position)
		{
			var center = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
			var span = new Xamarin.Forms.Maps.MapSpan(center, 0.1, 0.1);
			LocationsMap.MoveToRegion(span);
		}

	}
}