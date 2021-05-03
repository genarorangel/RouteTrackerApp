using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace RouteTrackerApp.Logic
{
    class PermissionsLogic
    {
        //Classe com a lógica de requisição de permissão para utilizar localização no sistema 
        //Lógica baseada na disponibilizada pelo criador do Plugin.Permissions

        public static bool IsLocationAccessPermitted;

        public async static Task GetPermissions()
        {
            try
            {
                PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse))
                    {
                        await Application.Current.MainPage.DisplayAlert("Premissão de localização", "Preciso acessar sua localização", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);
                    if (results.ContainsKey(Permission.LocationWhenInUse))
                        status = results[Permission.LocationWhenInUse];
                }

                if (status == PermissionStatus.Granted)
                {
                    IsLocationAccessPermitted = true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await Application.Current.MainPage.DisplayAlert("Localização negada", "Permissão para acessar localização negada", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }   
}
