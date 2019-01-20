﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GigaHitz.Views
{
    public partial class etcPage : ContentPage
    {
        public etcPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            await Navigation.PopAsync(false);
        }

        //when Click the button, Navigate MetroPage
        async void Btn_MetroPage(object sender, EventArgs s)
        {
            await Navigation.PushAsync(new etcContent.MetronomePage(), false);
        }

        //when Click the button, Navigate PracticePage
        async void Btn_PracticePage(object sender, EventArgs s)
        {
            await Navigation.PushAsync(new etcContent.PracticePage(), false);
        }

        //when Click the button, Navigate PianoPage
        async void Btn_PianoPage(object sender, EventArgs s)
        {
            await Navigation.PushAsync(new etcContent.PianoPage(), false);
        }
    }
}