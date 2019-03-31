using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GigaHitz.Views
{
    public partial class SplashView : ContentView
    {
        public SplashView()
        {
            InitializeComponent();
        }

        public ProgressBar Progress
        {
            get
            {
                return progress;
            }
        }
    }
}
