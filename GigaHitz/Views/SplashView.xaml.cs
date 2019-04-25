using Xamarin.Forms;
using GigaHitz.DataBase;

namespace GigaHitz.Views
{
    public partial class SplashView : ContentView
    {
        public SplashView()
        {
            InitializeComponent();

            var av = new AppVersion();
            version.Text = av.Name + ":" + av.Numb;
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
