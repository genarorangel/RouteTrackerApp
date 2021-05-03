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
    public partial class ServicePage : ContentPage
    {
        private int intServiceNumber;
        private string serviceNumber;
        private Servico servico = new Servico();
        public string ServiceNumber { get => serviceNumber;  set => serviceNumber = value; }
        public int IntServiceNumber { get => intServiceNumber; set => intServiceNumber = value; }

        public ServicePage(string currentServiceNumber)
        {
            ServiceNumber = currentServiceNumber;
            IntServiceNumber = int.Parse(ServiceNumber);
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ServiceNumberLabel.Text = ServiceNumber;
            servico = await GetServiceAsync();
            ServiceNumberLabel.Text = "Numero do serviço: " + servico.NUMERO;
            StartingDateTimeLabel.Text = "Data/hora de saída: " + servico.DATAHORA_SAIDA;
            StartingPointLabel.Text = "Local de partida: " + servico.LOCAL_SAIDA;
            EndingDateTimeLabel.Text = "Data/hora de chegada: " + servico.DATAHORA_CHEGADA;
            EndingPointLabel.Text =  "Local do serviço: " + servico.LOCAL_CHEGADA;
            ReturningLabel.Text = "Houve retorno? " + servico.VOLTA;
            ReturngingPointLabel.Text = "Local de retorno: " + servico.LOCAL_VOLTA;
            ExpectedKmLabel.Text = "Distância esperada até o local do serviço: " + servico.KM_ESPERADO;
            RealKMLabel.Text = "Distância percorrida até local do serviço: " + servico.KM_ATE_SERVICO;
            KmTotalLabel.Text = "Distância total percorrida: " + Math.Round(servico.KM_TOTAL,3).ToString().ToString().Replace(".",",") + " km";
            ExceededKmLabel.Text = "Distância excedida: " + Math.Round(servico.KM_CALCULADO, 3).ToString().ToString().Replace(".", ",") + " km"; 
            ExpectedDuration.Text = "Duração experada: " + servico.DURACAO_ESPERADA;
            RealDuration.Text = "Duração aferida: " + servico.DURACAO_CALCULADA;
        }

        private async Task<Servico> GetServiceAsync()
        {
            var service = (await App.client.GetTable<Servico>().Where(u => u.NUMERO == IntServiceNumber).ToListAsync()).FirstOrDefault();
            return service;
        }

        private async void routeButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DisplayRoutePage(intServiceNumber));
        }
    }
}