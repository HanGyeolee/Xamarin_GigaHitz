using Xamarin.Forms;
using GigaHitz.DataBase;

namespace GigaHitz.Views
{
    public partial class SplashView : ContentView
    {
        public SplashView()
        {
            InitializeComponent();

            version.Text = AppVersion.Name + ":" + AppVersion.Numb;
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
