using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GigaHitz.DataBase;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace GigaHitz.Views.etcContent
{
    public partial class MetronomePage : ContentPage
    {
        private int note = 4, time = 4, bpm = 120;
        double sleep_half;

        bool playing, toggle;
        uint count;

        public MetronomePage()
        {
            InitializeComponent();

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            edit.Text = bpm.ToString();

            Calculate();

            //this event have to be call at Paused.
            edit.Focused += delegate {
                Device.BeginInvokeOnMainThread(delegate
                {
                    edit.Text = "";
                });
            };

            edit.Unfocused += delegate {
                if (Regex.IsMatch(edit.Text, "^[1-9][0-9]{0,2}$"))
                {
                    int tmp = 0;

                    try
                    {
                        tmp = Convert.ToInt32(edit.Text);
                    }
                    catch(Exception)
                    {
                        tmp = 120;
                    }

                    if (tmp > 240) tmp = 240;
                    else if (tmp < 30) tmp = 30;

                    bpm = tmp;
                }
                else
                {
                    DisplayAlert("죄송해요!", "정확한 BPM을 기입해 주세요...", "알겠어요");
                }

                Device.BeginInvokeOnMainThread(delegate
                {
                    edit.Text = bpm.ToString();
                });

                Calculate();
            };
        }

        void Calculate()
        {
            var cal = (60.0 / bpm) * (4.0 / note);
            sleep_half = (500 * cal);
        }

        void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(e.NewTextValue, "^[1-9]?[0-9]{0,2}$"))
            {
                Device.BeginInvokeOnMainThread(delegate
                {
                    (sender as Xamarin.Forms.Entry).Text = e.OldTextValue;
                });
            }
        }

        void TextCompleted(object sender, EventArgs e)
        {
            (sender as Xamarin.Forms.Entry).Unfocus();
        }

        async void Btn_Option(object sender, EventArgs s)
        {
            playing = false;

            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsEnabled = false;
            edit.IsEnabled = true;

            var action = await DisplayActionSheet("박자", "취소", null, "4/4", "3/4", "6/8");
            if (action == null)
                return;
            else if (action.Equals("4/4"))
            {
                time = 4;
                note = 4;
                Device.BeginInvokeOnMainThread(delegate
                {
                    beats.Text = " 4 / 4 ";
                });
            }
            else if (action.Equals("3/4"))
            {
                time = 3;
                note = 4;
                Device.BeginInvokeOnMainThread(delegate
                {
                    beats.Text = " 3 / 4 ";
                });
            }
            else if (action.Equals("6/8"))
            {
                time = 6;
                note = 8;
                Device.BeginInvokeOnMainThread(delegate
                {
                    beats.Text = " 6 / 8 ";
                });
            }
        }

        void Btn_Play(object sender, EventArgs s)
        {
            count = 0;
            playing = true;
            toggle = true;
            Device.StartTimer(TimeSpan.FromMilliseconds(sleep_half), delegate
            {
                if (playing)
                {
                    if (toggle)
                        StaticDatas.SoundEffect.Play((int)(count % time));
                    else
                        StaticDatas.SoundEffect.Stop((int)(count++ % time));
                    toggle = !toggle;
                    return true;
                }
                return false;
            });

            play.IsVisible = false;
            play.IsEnabled = false;
            pause.IsVisible = true;
            pause.IsEnabled = true;
            edit.IsEnabled = false;
        }

        void Btn_Pause(object sender, EventArgs s)
        {
            playing = false;

            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsEnabled = false;
            edit.IsEnabled = true;
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            playing = false;
            await Navigation.PopAsync(false);
        }

        protected override bool OnBackButtonPressed()
        {
            playing = false;
            Navigation.PopAsync(false);
            return true;
        }
    }
}
