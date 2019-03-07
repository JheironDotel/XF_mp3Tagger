using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;

namespace mp3TaggerMusic.Droid
{
    [Activity(Label = "mp3TaggerMusic", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //string[] allPermisions = new[] {
        //    Android.Manifest.Permission.Internet,
        //    Android.Manifest.Permission.AccessFineLocation,
        //    Android.Manifest.Permission.AccessNetworkState,
        //    Android.Manifest.Permission.WriteExternalStorage,
        //    Android.Manifest.Permission.ReadExternalStorage
        //};       

        //const int requestId = 0;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            var forceXamlOnlyTypeLoading = new[]
            {
                typeof(UXDivers.Effects.Effects),
                typeof(UXDivers.Effects.Droid.CircleEffect)
            };

            base.OnCreate(bundle);

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);

            //RequestPermissions(allPermisions, requestId);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

