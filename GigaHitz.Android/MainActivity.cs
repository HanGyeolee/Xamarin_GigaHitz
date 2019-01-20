using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using GigaHitz.Droid.Api;

namespace GigaHitz.Droid
{
    [Activity(Label = "GigaHitz", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::CarouselView.FormsPlugin.Android.CarouselViewRenderer.Init();
            global::GigaHitz.Droid.Share_Android.Init(this);
            global::GigaHitz.Droid.Api.PermissionRequest.Init(this);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {   
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}