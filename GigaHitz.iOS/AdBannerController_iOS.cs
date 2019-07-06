using System;
using Xamarin.Forms;
using Google.MobileAds;
using GigaHitz.Interfaces;
using UIKit;

[assembly: Dependency(typeof(GigaHitz.iOS.AdBannerController_iOS))]
namespace GigaHitz.iOS
{
    public class AdBannerController_iOS : IAdBannerController
    {
        string AdUnitId;

        public void GetAdView<T>(ref T adView)
        {
            (adView as BannerView).AdUnitID = AdUnitId;
        }

        public void Load(string UId)
        {
            AdUnitId = UId;
        }
    }
}
