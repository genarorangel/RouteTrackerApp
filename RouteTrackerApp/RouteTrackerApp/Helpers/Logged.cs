using RouteTrackerApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RouteTrackerApp.Helpers
{
    class Logged
        //Claase utilizada para armazenar a pessoa que realizou o login
    {
        private static Pessoa pessoaLogged;
        public static void SetLogged(Pessoa pessoa)
        {
          pessoaLogged = pessoa;
        }

        public static Pessoa GetLogged()
        {
            return pessoaLogged;
        }
    }
}
