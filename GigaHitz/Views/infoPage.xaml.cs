using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GigaHitz.Views
{
    public partial class infoPage : ContentPage
    {
        public infoPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            await Navigation.PopAsync(false);
        }
    }
}
