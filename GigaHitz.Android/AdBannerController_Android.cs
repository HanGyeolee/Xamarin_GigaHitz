using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Gms.Ads;
using GigaHitz.Interfaces;

[assembly: Dependency(typeof(GigaHitz.Droid.AdBannerController_Android))]
namespace GigaHitz.Droid
{
    public class AdBannerController_Android : IAdBannerController
    {
        string AdUnitId;

        public void GetAdView<T>(ref T adView)
        {
            (adView as AdView).AdUnitId = AdUnitId;
        }

        public void Load(string UId)
        {
            AdUnitId = UId;
        }
    }
}
