using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using CarouselView.FormsPlugin;

namespace GigaHitz.Droid
{
    [Activity(Label = "GigaHitz", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //proguard true, not link
            FitWindowsFrameLayout buf_0;
            CarouselView.FormsPlugin.Android.CarouselViewRenderer buf_1;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::CarouselView.FormsPlugin.Android.CarouselViewRenderer.Init();
            global::GigaHitz.Droid.Share_Android.Init(this);
            global::GigaHitz.Droid.Api.PermissionRequest.Init(this);

            buf_0 = null;
            buf_1 = null;

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {   
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}