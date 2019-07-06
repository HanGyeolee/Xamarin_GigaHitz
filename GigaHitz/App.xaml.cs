using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using GigaHitz.DataBase;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GigaHitz
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.MainPage())
            {
                BackgroundColor = Color.Transparent
            };
            //MainPage = new Views.MainPage();
        }

        protected override void OnStart()
        {
            ExperimentalFeatures.Enable(ExperimentalFeatures.ShareFileRequest);
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
