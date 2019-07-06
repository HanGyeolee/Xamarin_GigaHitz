using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GigaHitz.Views.etcContent
{
    public partial class LoadingView : ContentView
    {
        public LoadingView()
        {
            InitializeComponent();
            StartCoroutine(Circling);
        }

        async void Circling()
        {
            double r = 30;
            for (int i = 0; i < 60; i++)
            {
                await Task.Delay(100);
                Device.BeginInvokeOnMainThread(delegate
                {
                    loading.Rotation += r; // 30
                });
            }
        }

        void StartCoroutine(Action action)
        {
            var task = new Task(action);
            task.Start();
        }
    }
}
