using System;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace GigaHitz.Views.etcContent
{
    public partial class MetronomePage : ContentPage
    {
        Interfaces.ISoundEffect soundEffect;

        private int note = 4, time = 4, bpm = 0; 
        double sleep_half;

        bool playing, toggle;
        uint count;

        public MetronomePage()
        {
            InitializeComponent();

            soundEffect = DependencyService.Get<Interfaces.ISoundEffect>();

            soundEffect.Initialize(4);
            soundEffect.AddSystemSound("Tik");
            soundEffect.AddSystemSound("Tok");
            soundEffect.AddSystemSound("Tok");
            soundEffect.AddSystemSound("Tok");

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }

        void Calculate()
        {
            var cal = (60.0 / bpm) * (4.0 / note);
            sleep_half = (500 * cal);
        }

        void TextCompleted(object sender, EventArgs e)
        {
            playing = false;

            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsVisible = false;

            var tmp = Convert.ToInt32((sender as Editor).Text);
            if (tmp == 0) return;   //Text = null

            if (tmp > 240) tmp = 240;
            else if (tmp < 30) tmp = 30;

            bpm = tmp;

            Device.BeginInvokeOnMainThread(delegate
            {
                (sender as Editor).Text = bpm.ToString();
            });

            Calculate();
        }

        async void Btn_Option(object sender, ClickedEventArgs s)
        {
            playing = false;

            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsVisible = false;

            var action = await DisplayActionSheet("박자", "취소", null, "4/4", "3/4");
            if (action.Equals("4/4"))
            {
                time = 4;
                Device.BeginInvokeOnMainThread(delegate
                {
                    beats.Text = " 4 / 4 ";
                });
            }
            else if (action.Equals("3/4"))
            {
                time = 3;
                Device.BeginInvokeOnMainThread(delegate
                {
                    beats.Text = " 3 / 4 ";
                });
            }
        }

        async void Btn_Back(object sender, ClickedEventArgs s)
        {
            playing = false;
            await Navigation.PopAsync(false);
        }

        void Btn_Play(object sender, ClickedEventArgs s)
        {
            if (bpm > 25)
            {
                count = 0;
                playing = true;
                toggle = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(sleep_half), delegate
                {
                    if (playing)
                    {
                        if (toggle)
                            soundEffect.Play((int)(count % time));
                        else
                            soundEffect.Stop((int)(count++ % time));
                        toggle = !toggle;
                        return true;
                    }
                    return false;
                });

                play.IsVisible = false;
                play.IsEnabled = false;
                pause.IsVisible = true;
                pause.IsVisible = true;
            }
        }

        void Btn_Pause(object sender, ClickedEventArgs s)
        {
            playing = false;

            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsVisible = false;
        }
    }
}
