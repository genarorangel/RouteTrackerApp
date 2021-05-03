using RouteTrackerApp.Model;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RouteTrackerApp.Logic
{
    public class TesteLogic
    {
        public async static Task InsertTeste()
        {
            int index = 1;
            Teste teste = new Teste()
            {
                idx = index
            };
            try
            {
                await App.client.GetTable<Teste>().InsertAsync(teste);
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", e.Message, "Ok");
            }
        }
    }
}
