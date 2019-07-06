using System;
using Google.MobileAds;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using GigaHitz.Renderer;
using GigaHitz.iOS.Renderer;
using UIKit;

[assembly: ExportRenderer(typeof(AdBanner), typeof(AdBanner_iOS))]
namespace GigaHitz.iOS.Renderer
{
    public class AdBanner_iOS : ViewRenderer
    {
        readonly Interfaces.IAdBannerController controller;
        readonly Request request;
        BannerView adView;

        public AdBanner_iOS()
        {
            controller = DependencyService.Get<Interfaces.IAdBannerController>();
            adView = new BannerView();
            request = Request.GetDefaultRequest();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                switch ((Element as AdBanner).Size)
                {
                    case AdBanner.Sizes.StandardBanner:
                        adView.AdSize = AdSizeCons.Banner;
                        //adView = new BannerView(AdSizeCons.Banner, new CGPoint(0, 0));
                        break;
                    case AdBanner.Sizes.LargeBanner:
                        adView.AdSize = AdSizeCons.LargeBanner;
                        //adView = new BannerView(AdSizeCons.Banner, new CGPoint(0, 0));
                        break;
                    case AdBanner.Sizes.MediumRectangle:
                        adView.AdSize = AdSizeCons.MediumRectangle;
                        //adView = new BannerView(AdSizeCons.Banner, new CGPoint(0, 0));
                        break;
                    case AdBanner.Sizes.FullBanner:
                        adView.AdSize = AdSizeCons.FullBanner;
                        //adView = new BannerView(AdSizeCons.Banner, new CGPoint(0, 0));
                        break;
                    case AdBanner.Sizes.LeaderBoard:
                        adView.AdSize = AdSizeCons.Leaderboard;
                        //adView = new BannerView(AdSizeCons.Banner, new CGPoint(0, 0));
                        break;
                    case AdBanner.Sizes.SmartBannerPortrait:
                        adView.AdSize = AdSizeCons.SmartBannerPortrait;
                        //adView = new BannerView(AdSizeCons.Banner, new CGPoint(0, 0));
                        break;
                    case AdBanner.Sizes.SmartBannerLandScape:
                        adView.AdSize = AdSizeCons.SmartBannerLandscape;
                        break;
                    default:
                        adView.AdSize = AdSizeCons.Banner;
                        //adView = new BannerView(AdSizeCons.Banner, new CGPoint(0, 0));
                        break;
                }

                controller.GetAdView(ref adView);
                (adView as BannerView).RootViewController = UIApplication.SharedApplication.Windows[0].RootViewController;
                (adView as BannerView).LoadRequest(request);
                //Load("ca-app-pub-8979507455037422/1646227850");
                SetNativeControl(adView);
            }
        }

        public void Load(string UId)
        {
            adView.AdUnitID = UId;
            adView.RootViewController = UIApplication.SharedApplication.Windows[0].RootViewController;
            var request = Request.GetDefaultRequest();
            adView.LoadRequest(request);
        }
    }
}
