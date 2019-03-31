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
        private Task check;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationTokenSource cts_data = new CancellationTokenSource();

        public NoticePage()
        {
            InitializeComponent();

            ImgUrl = new ObservableCollection<ImgSource>();

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            check = new Task(async delegate
            {
                if (StaticDatas.Uris == null)
                {
                    if (await StaticDatas.InitializeUris(cts_data.Token) == -1)
                        Device.BeginInvokeOnMainThread(async delegate
                        {
                            await DisplayAlert("네트워크", "네트워크 연결이 필요합니다.", "그렇군요...");
                        });
                    else
                    {
                        CheckData();
                        GetFirebaseData();
                    }
                }
                else
                {
                    if (!await StaticDatas.RetireUris())
                        Device.BeginInvokeOnMainThread(async delegate
                        {
                            await DisplayAlert("네트워크", "네트워크 연결이 필요합니다.", "그렇군요...");
                        });
                    else
                    {
                        CheckData();
                        GetFirebaseData();
                    }
                }
            }, cts.Token);

            check.Start();
        }

        void CheckData()
        {
            var n = StaticDatas.Num;
            if (StaticDatas.NumPosterOpened != StaticDatas.Uris.Count)            //제대로 다운받지 못했을 때
            {
                if (StaticDatas.InitializeUris(cts_data.Token).Result == -1)
                    Device.BeginInvokeOnMainThread(async delegate
                    {
                        await DisplayAlert("네트워크", "네트워크 연결이 필요합니다.", "그렇군요...");
                    });
            }
            else if (n != StaticDatas.CheckPosterNum()) // 서버에서 포스터 갯수가 달라질 때
            {
                if (!StaticDatas.RetireUris().Result)
                    Device.BeginInvokeOnMainThread(async delegate
                    {
                        await DisplayAlert("네트워크", "네트워크 연결이 필요합니다.", "그렇군요...");
                    });
            }
        }

        void GetFirebaseData()
        {
            //get url at firebase
            Device.BeginInvokeOnMainThread(delegate
            {
                if (StaticDatas.Uris.Count > 0)
                {
                    for (int j = 0; j < StaticDatas.Uris.Count; j++)
                    {
                        ImgUrl.Add(new ImgSource { Source = new Uri(StaticDatas.Uris[j]) });
                    }
                    CV.ItemsSource = ImgUrl;
                }
            });
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            if (cts_data != null)
                cts_data.Cancel();
            if (cts != null)
                cts.Cancel();
            await Navigation.PopAsync(false);
        }

        async void Btn_NoticeContent(object sender, EventArgs s)
        {
            await Navigation.PushAsync(new NoticeContent.NoticeContentPage(), false);
        }

        protected override bool OnBackButtonPressed()
        {
            if (cts_data != null)
                cts_data.Cancel();
            if (cts != null)
                cts.Cancel();
            Navigation.PopAsync(false);
            return true;
        }
    }

    public class ImgSource
    {
        public Uri Source { get; set; }
    }
}
