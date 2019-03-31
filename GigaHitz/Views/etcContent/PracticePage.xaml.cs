using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;

namespace GigaHitz.Views.etcContent
{
    public partial class PracticePage : ContentPage
    {
        Interfaces.IAudioPlayer player;
        private readonly string[] vs = { "UpScale-C~G~C", "UpScale-CEGEC", "UpScale-CEGCGEC", "UpScale-Speedy", "UpScale-AllParts",
                                         "DownScale-C~G~C", "DownScale-CEGEC", "DownScale-CEGCGEC", "DownScale-Speedy"};

        double MaxTime;
        bool changeValue, playerReady;

        Point StartP, LastP, PastP;
        double buf;

        public ObservableCollection<ViewModel.RecordViewModel> record { get; set; }
        CancellationTokenSource cts;

        public PracticePage()
        {
            InitializeComponent();

            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsVisible = false;

            playerReady = false;

            record = new ObservableCollection<ViewModel.RecordViewModel>();
            player = DependencyService.Get<Interfaces.IAudioPlayer>();

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            LV.RowHeight = 60;
            changeValue = false;

            Load();
        }

        void Load()
        {
            string fileName, time;
            int i = 1;

            while(i < 9)
            {
                fileName = "Scale" + i.ToString();

                if (playerReady = player.Prepare(fileName, 2))
                    time = double2String(player.GetDurationTime(), "{0:00}:{1:00}");
                else
                    time = "00:00";

                record.Add(new ViewModel.RecordViewModel { Title = vs[i - 1], Time = time, Day = " ", filePath = fileName });

                i++;
            }
            LV.ItemsSource = record;
            playerReady = player.Release();
        }

        void SetPlay(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as ViewModel.RecordViewModel;

            //record start
            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsVisible = false;

            player.Stop();
            if (playerReady = player.Prepare(item.filePath, 2))
            {
                MaxTime = player.GetDurationTime();

                player.Finished(delegate
                {
                    SetPlay(sender, e);
                });

                Device.BeginInvokeOnMainThread(delegate
                {
                    duration.Text = double2String(MaxTime, "{0:00}:{1:00}  ");
                    current.Text = "  00:00";
                    slider.Value = 0;
                });
            }
        }

        void OnTouchEffectAction(object sender, TouchApi.TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchApi.TouchActionType.Pressed:
                    changeValue = true;
                    StartP = LastP = PastP = args.Location;
                    break;
                case TouchApi.TouchActionType.Moved:
                    LastP = args.Location;

                    buf = (LastP.X - PastP.X) / slider.Width;
                    if (buf > 0.5) buf = 0.5;
                    else if (buf < -0.5) buf = -0.5;

                    Device.BeginInvokeOnMainThread(delegate
                    {
                        slider.Value += buf;
                        current.Text = double2String(slider.Value * MaxTime, "  {0:00}:{1:00}");
                    });

                    PastP = LastP;
                    break;
                case TouchApi.TouchActionType.Released:
                    // seek 재생
                    changeValue = false;
                    if (Math.Abs(StartP.X - LastP.X) > 1)
                        player.SeekTo(slider.Value * MaxTime);
                    break;
            }
        }

        // 재생버튼과 정지버튼은 상호 보이지 않게 한다.

        async void Btn_Home(object sender, EventArgs s)
        {
            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }
            playerReady = player.Release();
            await Navigation.PopToRootAsync(false);
        }

        void Btn_Play(object sender, EventArgs s)
        {
            cts = new CancellationTokenSource();
            if (playerReady)
            {
                player.Start();

                var task = new Task(Update, cts.Token);
                task.Start();

                play.IsVisible = false;
                play.IsEnabled = false;
                pause.IsVisible = true;
                pause.IsVisible = true;
            }
        }

        void Btn_Pause(object sender, EventArgs s)
        {
            player.Stop();

            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }

            //record start
            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsVisible = false;
        }

        // current Text change with current time
        void Update()
        {
            while (true)
            {
                if (!changeValue)
                {
                    double time = player.GetCurrentTime();
                    string CurrentT = double2String(time, "  {0:00}:{1:00}");

                    Device.BeginInvokeOnMainThread(delegate
                    {
                        current.Text = CurrentT;
                        slider.Value = time / MaxTime;
                    });
                }
                Thread.Sleep(250);
            }
        }

        string double2String(double tmp, string format)
        {
            string get;
            if (tmp < 3600)
                get = string.Format(format,
                    tmp / 60,
                    tmp % 60);
            else
                get = string.Format("{0:0}.{1:00}:{2:00}",
                    tmp / 3600,
                    (tmp % 3600) / 60,
                    tmp % 60);
            return get;
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }
            playerReady = player.Release();
            await Navigation.PopAsync(false);
        }

        protected override bool OnBackButtonPressed()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }
            playerReady = player.Release();
            Navigation.PopAsync(false);
            return true;
        }
    }
}
