using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using RouteTrackerApp.Messages;
using Android.Content;
using XamarinForms.LocationService.Droid.Services;

namespace RouteTrackerApp.Droid
{
    [Activity(Label = "RouteTrackerApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Intent serviceIntent;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            CurrentPlatform.Init();

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            string dataBaseName = "RouteTracker_db.sqlte";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fullPath = Path.Combine(folderPath, dataBaseName);

            LoadApplication(new App(fullPath));

            serviceIntent = new Intent(this, typeof(AndroidLocationService));
            SetServiceMethods();

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void SetServiceMethods()
        {
            MessagingCenter.Subscribe<StartServiceMessage>(this, "ServiceStarted", message => {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    StartForegroundService(serviceIntent);
                }
                else
                {
                    StartService(serviceIntent);
                }
            });

            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message => {
                StopService(serviceIntent);
            });
        }
    }
}