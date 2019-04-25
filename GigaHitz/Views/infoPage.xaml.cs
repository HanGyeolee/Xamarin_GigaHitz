using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GigaHitz.Views
{
    public partial class infoPage : ContentPage
    {
        public infoPage()
        {
            InitializeComponent();

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            var tapGR = new TapGestureRecognizer();
            tapGR.Tapped += (sender, e) =>
            {
                Clipboard.SetTextAsync("gigahitz.app@gmail.com");
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
