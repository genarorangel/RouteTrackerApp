using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RouteTrackerApp.Helpers
{
    class KeepLoginAndPassword
    {
        //Essa classe utiliza o Plugin.Settings para ser possível guardar e acessar os dados de 
        //QRA e senha para autopreenchimento das entradas utilizadas para logar no sistema
        private static ISettings AppSettings =>
        CrossSettings.Current;

        public static string UserQRA
        {
            get => AppSettings.GetValueOrDefault(nameof(UserQRA), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserQRA), value);
        }

        public static string UserPassword
        {
            get => AppSettings.GetValueOrDefault(nameof(UserPassword), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserPassword), value);
        }

    }
}
