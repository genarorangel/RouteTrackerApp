using RouteTrackerApp.Helpers;
using RouteTrackerApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RouteTrackerApp.Logic
{
    public class LoginLogic
    {            
      //Essa classe contém a lógica para realizar o login do usuário no sistema           
        public async static Task CheckLoginAndPassword(string Qra, string Password)
        {
            //Essa função faz a correspondência dos dados de QRA e senha com o banco de dados para validar a entrada do usuário

            //Os dados de QRA e senha foram erroneamente setados como int no banco de dados de teste 
            //Um banco de dados definivo será construído com QRA e senha com o tipo varchar
            int qra = int.Parse(Qra);
            int password = int.Parse(Password);

            //Realiza uma Querry utilizando LINQ para buscar o usuário correspondente no banco de dados através de uma aplicação Web que conecta ao banco de dados
            try
            {
                Pessoa user = (await App.client.GetTable<Pessoa>().Where(u => u.QRA == qra).ToListAsync()).FirstOrDefault();
                //Se houve correspondência
                if (user != null)
                {
                    //Verifica se a senha corresponde à senha designada ao usuário no banco de dados
                    if (user.SENHA == password)
                    {
                        //Se a senha for correta, salva no programa qual usuário está loggado e salva as credenciais para o próximo login
                        Logged.SetLogged(user);
                        KeepLoginAndPassword.UserQRA = user.QRA.ToString();
                        KeepLoginAndPassword.UserPassword = user.SENHA.ToString();

                        //Verifica a função do usuário para ser encaminhado à página correta
                        if (user.FUNCAO == "Adm")
                            await Application.Current.MainPage.Navigation.PushAsync(new ManagerPage());
                        else
                            await Application.Current.MainPage.Navigation.PushAsync(new EmployeePage());
                    }
                    else
                        //Se o usuário for correto, mas a senha não tiver correspondência:
                        await Application.Current.MainPage.DisplayAlert("Erro", "Senha inválida", "OK");
                }
                else
                    //Se o usuário digitado não for encontrado:
                    await Application.Current.MainPage.DisplayAlert("Erro", "Usuário não encontrado", "OK");
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", e.Message, "OK");
            }

        }
    }

}
