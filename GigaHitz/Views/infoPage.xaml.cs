using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Collections.Generic;
using GigaHitz.Renderer;

namespace GigaHitz.Views
{
    public partial class infoPage : ContentPage
    {
        Interfaces.IAdBannerController controller;

        public infoPage()
        {
            InitializeComponent();
            controller = DependencyService.Get<Interfaces.IAdBannerController>();

            //배너를 만들고 난 이후에 광고 주소 로드.
            if (Device.RuntimePlatform.Equals(Device.Android))
                controller.Load("ca-app-pub-8979507455037422/1167316328");
            else if (Device.RuntimePlatform.Equals(Device.iOS))
                controller.Load("ca-app-pub-8979507455037422/8443171112");
            AdBanner.Size = AdBanner.Sizes.SmartBannerPortrait;

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            var tapGR = new TapGestureRecognizer();

            List<string> recipients = new List<string>
            {
                "gigahitz.app@gmail.com"
            };
            tapGR.Tapped += (sender, e) =>
            {
                //Clipboard.SetTextAsync("gigahitz.app@gmail.com");
                try
                {
                    var message = new EmailMessage
                    {
                        Subject = "피드백",
                        To = recipients
                    };
                    Email.ComposeAsync(message);
                }
                catch(FeatureNotSupportedException)
                {
                    //Email is not supported on this device
                }
                catch(Exception)
                {
                    //Some other exception occurred
                }
            };

            email.GestureRecognizers.Add(tapGR);
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            await Navigation.PopAsync(false);
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopAsync(false);
            return true;
        }
    }
}
