using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RouteTrackerApp.Model
{
    public class Pessoa
    {
        //Classe espelho da tabela Pessoa no banco de dados do serviço
        public string id { get; set; }
        public int QRA { get; set; }
        public string NOME { get; set; }
        public string CPF { get; set; }
        public string TELEFONE { get; set; }
        public string FUNCAO { get; set; }
        public int SENHA { get; set; }

    }
}
