using System;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
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

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            ///*
            splash = new SplashView
            {
                IsEnabled = false
            };
            Absolute.Children.Add(splash, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            // Start Coroutine to Fadeout and delete
            StartCoroutine(FadeOut);
            //*/
        }

        ///*
        // fadeout alpha 1 to 0
        async void FadeOut()
        {
            Thread.Sleep(2500);
            //FadeOut and remove View
            await splash.FadeTo(0, 400, Easing.Linear);
            Device.BeginInvokeOnMainThread(delegate
            {
                Absolute.Children.Remove(splash);
            });
        }
        //*/

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
