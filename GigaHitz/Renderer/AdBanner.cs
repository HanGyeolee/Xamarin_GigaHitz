using System;
using Xamarin.Forms;

namespace GigaHitz.Renderer
{
    public class AdBanner : View
    {
        public enum Sizes {  StandardBanner, LargeBanner, MediumRectangle, FullBanner, LeaderBoard, SmartBannerPortrait, SmartBannerLandScape}
        public Sizes Size { get; set; }
        public AdBanner()
        {
            this.AnchorX = 0.5;
            this.AnchorY = 0.5;
            this.BackgroundColor = Color.Transparent;
        }
    }
}
