using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
            // Handle when your app starts
            var a = StaticDatas.Load().Result;
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
