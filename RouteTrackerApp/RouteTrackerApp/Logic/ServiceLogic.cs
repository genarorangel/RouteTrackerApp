using Plugin.Geolocator.Abstractions;
using RouteTrackerApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Linq;
using Xamarin.Forms;

namespace RouteTrackerApp.Logic
{
    public class ServiceLogic
    {
        //Classe que realiza cálculos necessários e insere o Serviço no banco de dados através de um cliente de aplicação Web que se comunica com o banco 
        public static Position StartPosition, StopPosition;
        public static string StartAddress, StopAddress, BackLocationAddress, ExpectedDuration, DistanceUntilService;
        public static DateTime StartDatetime, StopDatetime;
        public static int ServiceNumber;
        public static double distanceUntilService;
        private static double KmTotal;
        private static double excededKM; 
        private const int KmMax = 40;
        public static async Task InsertService(double kmTotal, bool hadReturn, string polyline)
        {
            KmTotal = kmTotal;
            excededKM = KmCalculation(KmTotal);
            string ExpectedDuration = Routes.CurrentRoute.rows.First().elements.First().duration_in_traffic.text;
            string ExpectedDistance = Routes.CurrentRoute.rows.First().elements.First().distance.text;
            TimeSpan durationTime = StopDatetime.Subtract(StartDatetime);
            string realDuration = (durationTime.Hours.ToString() + " hora(s) e " + durationTime.Minutes.ToString() + " minuto(s)");
            string didReturn = CheckIfReturn(hadReturn);
            string StartDateTime = StartDatetime.ToString("yyyy-MM-dd HH:mm:ss");
            string StopDateTime = StopDatetime.ToString("yyyy-MM-dd HH:mm:ss");
            Servico servico = new Servico()
            {
                NUMERO = ServiceNumber,
                DATAHORA_CHEGADA = StopDateTime,
                DATAHORA_SAIDA = StartDateTime,
                DURACAO_CALCULADA = realDuration,
                DURACAO_ESPERADA = ExpectedDuration,
                LOCAL_CHEGADA = StopAddress,
                LOCAL_SAIDA = StartAddress,
                LOCAL_VOLTA = BackLocationAddress,
                QRA = Helpers.Logged.GetLogged().QRA,
                VOLTA = didReturn,
                KM_TOTAL = KmTotal,
                KM_CALCULADO = excededKM, 
                Rota = polyline,
                KM_ESPERADO = ExpectedDistance,
                KM_ATE_SERVICO = DistanceUntilService,
            };
            try
            {
                await App.client.GetTable<Servico>().InsertAsync(servico);
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", e.Message, "Ok");
            }

        }

        private  static string CheckIfReturn(bool Volta)
        {
            if (Volta)
                return "Sim";
            else
                return "Não";
        }

        private static double KmCalculation(double Kms)
        {
            if (Kms > KmMax)
            {
                return KmTotal - KmMax;
            }
            else
            {
                return 0.0;
            }
        }



    }
}
