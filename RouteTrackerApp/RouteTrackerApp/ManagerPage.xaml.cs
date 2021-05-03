using RouteTrackerApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RouteTrackerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManagerPage : ContentPage
    {
        private static List<string> serviceNumberList = new List<string>();
        public ManagerPage()
        {
            InitializeComponent();

        }

        private async Task getServiceNumerList()
        {
            List<Servico> serviceList = await App.client.GetTable<Servico>().Take(1000).OrderByDescending(u => u.DATAHORA_SAIDA).ToListAsync();
            serviceList.ForEach(x =>
            {
                serviceNumberList.Add(x.NUMERO.ToString());  
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await getServiceNumerList();
            listview.ItemsSource = serviceNumberList;
        }
        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            listview.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                listview.ItemsSource = serviceNumberList;
            else
                listview.ItemsSource = serviceNumberList.Where(i => i.Contains(e.NewTextValue));

            listview.EndRefresh();
        }

        private async void listview_ItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new ServicePage(e.SelectedItem.ToString()));
        }
    }
}