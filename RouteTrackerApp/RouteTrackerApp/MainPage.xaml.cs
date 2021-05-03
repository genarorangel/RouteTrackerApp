using RouteTrackerApp.Logic;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using RouteTrackerApp.Helpers;

namespace RouteTrackerApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            //Inicializa página principal para
            InitializeComponent();

            //Inicializa a página com os dados do último usuário
            QraEntry.Text = KeepLoginAndPassword.UserQRA;
            Password.Text = KeepLoginAndPassword.UserPassword;
        }


        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            //Quando o botão de login é pressionado, valida a entrada de acordo com as entradas de QRA e senha
            await LoginLogic.CheckLoginAndPassword(QraEntry.Text, Password.Text);
        }                       

    }
}

