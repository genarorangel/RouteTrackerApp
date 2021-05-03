using System;
using System.Collections.Generic;
using System.Text;

namespace RouteTrackerApp.Model
{
    public class Servico
    {
        //Classe espelho da tabela Pessoa no banco de dados do serviço
        public string id { get; set; }
        public int NUMERO { get; set; }
        public string DATAHORA_SAIDA { get; set; }
        public string DATAHORA_CHEGADA { get; set; }
        public string LOCAL_SAIDA { get; set; }
        public string LOCAL_CHEGADA { get; set; }
        public string VOLTA { get; set; }
        public double KM_TOTAL { get; set; }
        public double KM_CALCULADO { get; set; }
        public int QRA { get; set; }
        public string LOCAL_VOLTA { get; set; }
        public string DURACAO_ESPERADA { get; set; }
        public string DURACAO_CALCULADA { get; set; }
        public string Rota { get; set; }
        public string KM_ESPERADO { get; set;}
        public string KM_ATE_SERVICO { get; set; }
    }
}
