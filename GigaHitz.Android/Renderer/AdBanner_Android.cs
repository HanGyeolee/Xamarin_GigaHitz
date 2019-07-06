using System;
using Android.Content;
using Android.Gms.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GigaHitz.Renderer;
using GigaHitz.Droid.Renderer;

[assembly: ExportRenderer(typeof(AdBanner), typeof(AdBanner_Android))]
namespace GigaHitz.Droid.Renderer
{
    public class AdBanner_Android : ViewRenderer
    {
        readonly Context context;
        readonly Interfaces.IAdBannerController controller;
        readonly AdRequest.Builder builder;
        AdView adView;

        public AdBanner_Android(Context _context) : base(_context)
        {
            context = _context;
            controller = DependencyService.Get<Interfaces.IAdBannerController>();
            builder = new AdRequest.Builder();
            adView = new AdView(context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if(e.OldElement == null)
            {
                switch((Element as AdBanner).Size)
                {
                    case AdBanner.Sizes.StandardBanner:
                        adView.AdSize = AdSize.Banner;
                        break;
                    case AdBanner.Sizes.LargeBanner:
                        adView.AdSize = AdSize.LargeBanner;
                        break;
                    case AdBanner.Sizes.MediumRectangle:
                        adView.AdSize = AdSize.MediumRectangle;
                        break;
                    case AdBanner.Sizes.FullBanner:
                        adView.AdSize = AdSize.FullBanner;
                        break;
                    case AdBanner.Sizes.LeaderBoard:
                        adView.AdSize = AdSize.Leaderboard;
                        break;
                    case AdBanner.Sizes.SmartBannerPortrait:
                        adView.AdSize = AdSize.SmartBanner;
                        break;
                    case AdBanner.Sizes.SmartBannerLandScape:
                        adView.AdSize = AdSize.SmartBanner;
                        break;
                    default:
                        adView.AdSize = AdSize.Banner;
                        break;
                }

                controller.GetAdView(ref adView);
                (adView as AdView).LoadAd(builder.Build());
                //Load("ca-app-pub-8979507455037422/9611104515");
                SetNativeControl(adView);
            }
        }

        void Load(string UId)
        {
            adView.AdUnitId = UId;
            var requestbuilder = new AdRequest.Builder();
            adView.LoadAd(requestbuilder.Build());
        }
    }
}
