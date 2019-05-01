using System;
using System.Diagnostics;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using GigaHitz.DataBase;

namespace GigaHitz.Views
{
    public partial class MainPage : ContentPage
    {
        SplashView splash;
        double ProgressValue;

        public MainPage()
        {
            InitializeComponent();

            ////status bar
            //On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            ///*
            splash = new SplashView();
            Absolute.Children.Add(splash, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            // Start Coroutine to Fadeout and delete
            //*/

            StartCoroutine(Check);
        }

        // Check Update
        void Check()
        {
            string UpdatedVersion;
            int UpdatedBuildVersion, ret;

            // inserting reference is current Version 
            ret = StaticDatas.CheckVersion(out UpdatedVersion, out UpdatedBuildVersion);
            if (ret != -1)
            {
                if (ret == 0) // 업데이트 버전이 현재 버젼보다 높을 경우
                {
                    var str = string.Format("최신 업데이트(v{0}:{1})가 있습니다.\n업데이트하시겠습니까?", UpdatedVersion, UpdatedBuildVersion);

                    Device.BeginInvokeOnMainThread(async delegate
                    {
                        if (await DisplayAlert("업데이트", str, "업데이트", "나중에"))
                        {
                            if (Device.RuntimePlatform.Equals(Device.Android)) // google play store
                                await Browser.OpenAsync("https://play.google.com/store/apps/details?id=com.moment.GigaHitz", BrowserLaunchMode.External);
                            else                                              // One Store
                                await Browser.OpenAsync("https://play.google.com/store/apps/details?id=com.moment.GigaHitz", BrowserLaunchMode.External);
                        }
                        else
                            StartCoroutine(FadeOut);
                    });
                }
                else // 현재 버전이 업데이트 버전보다 높을 경우 // 테스트 중
                {
                    Device.BeginInvokeOnMainThread(async delegate
                    {
                        await DisplayAlert("업데이트", "현재 테스트 중인 업데이트입니다.", "넵");
                        StartCoroutine(FadeOut);
                    });
                }
            }
            else // 현재 버전이 업데이트 버전과 동일하거나 인터넷 오류인 경우
                StartCoroutine(FadeOut);
        }

        ///*
        // fadeout alpha 1 to 0
        async void FadeOut()
        {
            //Progress
            var indicator = new Progress<double>(ReportProgress);
            StaticDatas.Init(indicator);

            LocalDB dB = new LocalDB();
            if (!dB.IsExist())
            {
                var s = StaticDatas.CheckNotice();
                if (s.Length > 3)
                {
                    Device.BeginInvokeOnMainThread(async delegate
                    {
                        var b = await DisplayAlert("공지 사항", s, "다시보지 않기", "넵");
                        dB.AddItem(256);       //kbps
                        dB.AddItem(44100f);   //rate
                        dB.AddItem(s);
                        dB.AddItem(b); // true = dont pop again, false = pass
                        dB.Write();
                    });
                }
            }
            else
            {
                int k;
                float r;
                bool boolean;

                dB.Read(out k, 0);
                dB.Read(out r, 1);
                var s_tmp = dB.ReadIndexOf(2);
                dB.Read(out boolean, 3);

                var s = StaticDatas.CheckNotice();
                if (s.Length > 3) // if not, notice not exist.
                {
                    if (!s.Equals(s_tmp)) // another notice
                    {
                        Device.BeginInvokeOnMainThread(async delegate
                        {
                            var b = await DisplayAlert("공지 사항", s, "다시보지 않기", "넵");
                            dB.AddItem(k);
                            dB.AddItem(r);
                            dB.AddItem(s);
                            dB.AddItem(b); // true = dont pop again, false = pass
                            dB.Write();
                        });
                    }
                    else if (!boolean) // same notice // check boolean
                    {
                        Device.BeginInvokeOnMainThread(async delegate
                        {
                            var b = await DisplayAlert("공지 사항", s, "다시보지 않기", "넵");
                            dB.AddItem(k);
                            dB.AddItem(r);
                            dB.AddItem(s);
                            dB.AddItem(b); // true = dont pop again, false = pass
                            dB.Write();
                        });
                    }
                }
            }

            //FadeOut and remove View
            await splash.FadeTo(0, 500, Easing.Linear);

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

        protected override bool OnBackButtonPressed()
        {
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
            return base.OnBackButtonPressed();
        }

        internal void ReportProgress(double value)
        {
            ProgressValue = value;
            Device.BeginInvokeOnMainThread(async delegate
            {
                await splash.Progress.ProgressTo(ProgressValue / 100.0, 500, Easing.Linear);
            });
        }

        void StartCoroutine(Action action)
        {
            var task = new Task(action);
            task.Start();
        }
    }
}
