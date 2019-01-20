using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GigaHitz.Views
{
    public partial class MainPage : ContentPage
    {
        SplashView splash;
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);

            splash = new SplashView
            {
                IsEnabled = false
            };
            Absolute.Children.Add(splash, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            // Start Coroutine to Fadeout and delete
            StartCoroutine(FadeOut);
        }

        // fadeout alpha 1 to 0
        void FadeOut()  
        {
            Thread.Sleep(2000);
            //FadeOut and remove View
            splash.FadeTo(0, 300, Easing.Linear).ContinueWith(delegate
            {
                splash.IsVisible = false; // only iOS
                splash = null;
                Absolute.Children.Remove(splash); // only Android
            });
        }

        //when Click the button, Navigate NoticePage
        async void Btn_NoticePage(object sender, EventArgs s)
        {
            await Navigation.PushAsync(new NoticePage(), false);
        }

        //when Click the button, Navigate etcPage
        async void Btn_etcPage(object sender, EventArgs s)
        {
            await Navigation.PushAsync(new etcPage(), false);
        }

        //when Click the button, Navigate RecordPage
        async void Btn_RecordPage(object sender, EventArgs s)
        {
            await Navigation.PushAsync(new RecordPage(), false);
        }

        //when Click the button, Navigate infoPage
        async void Btn_infoPage(object sender, EventArgs s)
        {
            await Navigation.PushAsync(new infoPage(), false);
        }

        void StartCoroutine(Action action)
        {
            var task = new Task(action);
            task.Start();
        }
    }
}
