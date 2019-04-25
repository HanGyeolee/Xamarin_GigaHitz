using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using GigaHitz.DataBase;

namespace GigaHitz.Views
{
    public partial class NoticePage : ContentPage
    {
        public ObservableCollection<ImgSource> ImgUrl { get; set; }
        private Task check, getData;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationTokenSource cts_get = new CancellationTokenSource();
        private CancellationTokenSource cts_data = new CancellationTokenSource();

        public NoticePage()
        {
            InitializeComponent();

            ImgUrl = new ObservableCollection<ImgSource>();

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            if (StaticDatas.Urls == null)
            {
                if (StaticDatas.PreUrls.Count > 0)
                {
                    for (int j = 0; j < StaticDatas.PreUrls.Count; j++)
                    {
                        ImgUrl.Add(new ImgSource { BackSource = new Uri(StaticDatas.PreUrls[j]) });
                    }
                    CV.ItemsSource = ImgUrl;
                }
            }
            else
            {
                if (StaticDatas.PreUrls.Count > 0)
                {
                    for (int j = 0; j < StaticDatas.PreUrls.Count ; j++)
                    {
                        ImgUrl.Add(new ImgSource { BackSource = new Uri(StaticDatas.PreUrls[j]) });
                        if(j < StaticDatas.Urls.Count)
                            ImgUrl[j].Source = new Uri(StaticDatas.Urls[j]);
                    }
                    CV.ItemsSource = ImgUrl;
                }
            }

            check = new Task(async delegate
            {
                if (StaticDatas.Urls == null)
                {
                    if (await StaticDatas.InitializeUrls(cts_data.Token) == -1)
                        Device.BeginInvokeOnMainThread(async delegate
                        {
                            await DisplayAlert("네트워크", "네트워크 연결이 필요합니다.", "그렇군요...");
                        });
                    else
                    {
                        CheckData();
                    }
                }
                else
                {
                    if (await StaticDatas.RetireUrls(cts_data.Token) == -1)
                        Device.BeginInvokeOnMainThread(async delegate
                        {
                            await DisplayAlert("네트워크", "네트워크 연결이 필요합니다.", "그렇군요...");
                        });
                    else
                    {
                        CheckData();
                    }
                }
            }, cts.Token);

            getData = new Task(async delegate
               {
                   int tmp = 0;
                   while (true)
                   {
                       await Task.Delay(50);
                       if (StaticDatas.Urls != null)
                           break;
                   }

                   while (true)
                   {
                       int count = StaticDatas.Urls.Count;
                       int gap = count - tmp;

                       //오류 검사의 경우 if if
                       if (gap > 0)
                       {
                           GetFirebaseData(tmp, gap);
                           tmp = count;
                       }
                       if (count == 10)
                           break;

                       await Task.Delay(4000);
                   }
               }, cts_get.Token);

            check.Start(); // check poster
            getData.Start(); // download poster, after preload poster image
        }

        void CheckData()
        {
            var n = StaticDatas.Num;
            if (StaticDatas.NumPosterOpened != StaticDatas.Urls.Count)            //제대로 다운받지 못했을 때
            {
                if (StaticDatas.InitializeUrls(cts_data.Token).Result == -1)
                {
                    Device.BeginInvokeOnMainThread(async delegate
                    {
                        await DisplayAlert("네트워크", "네트워크 연결이 필요합니다.", "그렇군요...");
                    });
                }
            }
            else if (n != StaticDatas.CheckPosterNum() && StaticDatas.RetireUrls(cts_data.Token).Result == -1)
                Device.BeginInvokeOnMainThread(async delegate
                {
                    await DisplayAlert("네트워크", "네트워크 연결이 필요합니다.", "그렇군요...");
                });
        }

        void GetFirebaseData(int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (index + i > 9) break;
                ImgUrl[index + i].Source = new Uri(StaticDatas.Urls[index + i]);
            }
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            if (cts_data != null)
                cts_data.Cancel();
            if (cts_get != null)
                cts_get.Cancel();
            if (cts != null)
                cts.Cancel();
            await Navigation.PopAsync(false);
        }

        protected override bool OnBackButtonPressed()
        {
            if (cts_data != null)
                cts_data.Cancel();
            if (cts_get != null)
                cts_get.Cancel();
            if (cts != null)
                cts.Cancel();
            Navigation.PopAsync(false);
            return true;
        }
    }

    public class ImgSource
    {
        public Uri BackSource { get; set; }
        public Uri Source { get; set; }
    }
}
