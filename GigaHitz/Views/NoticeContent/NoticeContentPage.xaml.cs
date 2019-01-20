using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GigaHitz.Views.NoticeContent
{
    public partial class NoticeContentPage : ContentPage
    {
        public NoticeContentPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            await Navigation.PopToRootAsync(false);
        }
    }
}
