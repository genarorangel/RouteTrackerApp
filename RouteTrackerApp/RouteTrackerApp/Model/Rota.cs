using System;
using System.Collections.Generic;
using System.Text;

namespace RouteTrackerApp.Model
{
    public class Rota
    {
        //classe espelho da tabela rotas do banco de dados
        public string id { get; set; }
        public int NUMERO { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
    }
}
