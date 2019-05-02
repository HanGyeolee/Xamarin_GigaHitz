using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using GigaHitz.PermissionApi;
using GigaHitz.DataBase;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

//TODO 대대적인 수정이 필요 iOS에서는 아예 작동이 안되는 문제가 발생
namespace GigaHitz.Views.RecordContent
{
    public partial class ListPage : ContentPage
    {
        Interfaces.IAudioPlayer player;
        ViewModel.RecordViewModel selectedItem;
        IPermission permission;
        //PermissionStatus status_STR;

        double MaxTime;
        string path;
        bool changeValue, cts, playerReady;

        Point StartP, LastP, PastP;
        double buf;

        public ObservableCollection<ViewModel.RecordViewModel> record { get; set; }

        public ListPage()
        {
            InitializeComponent();

            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsEnabled = false;

            playerReady = false;

            record = new ObservableCollection<ViewModel.RecordViewModel>();
            player = DependencyService.Get<Interfaces.IAudioPlayer>();
            permission = DependencyService.Get<IPermission>();

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            LV.RowHeight = 60;
            changeValue = false;
            cts = false;

            path = CreateDirectory();
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            //status_STR = permission.CheckPermissionAsync(Permission.Storage).Result;
            Load();
        }

        string CreateDirectory()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            directory = Path.Combine(directory, "Records");
            return directory;
        }

        void Load()
        {
            string title, time, day, creationTime;
            string[] filePath;
            filePath = Directory.GetFiles(path, "*.m4a");
            
            foreach (string namepath in filePath)
            {
                title = namepath.Substring(path.Length + 1);

                var dt = File.GetCreationTime(namepath);
                creationTime = dt.ToString("u");
                day = dt.ToString("yyyy. MM. dd");

                if (playerReady = player.Prepare(namepath))
                    time = double2String(player.GetDurationTime(), "{0:00}:{1:00}");
                else
                    time = "00:00";

                record.Add(new ViewModel.RecordViewModel { Title = title, Time = time, Day = day, filePath = namepath, creationTime = creationTime });
            }
            //sorting items
            record = new ObservableCollection<ViewModel.RecordViewModel>(record.OrderByDescending((arg) => arg.creationTime));

            LV.ItemsSource = record;
            playerReady = player.Release();
            /*
            if (status_STR.Equals(PermissionStatus.Granted))
            {
            }
            else
            {
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    if (status_STR.Equals(PermissionStatus.Unknown))
                    {
                        permission.RequestPermissionsAsync(new Permission[] { Permission.Storage }).ContinueWith(async delegate
                        {
                            status_STR = await permission.CheckPermissionAsync(Permission.Storage);
                        });
                    }
                    else if (!status_STR.Equals(PermissionStatus.Granted))
                        DisplayAlert("죄송해요!", "외부 디렉토리 저장 권한이 필요해요...", "그렇군요");
                    //"申し訳ありません！","外部ディレクトリの保存許可が必要です...","なるほど"
                    //"Sorry!", "External directory storage permission is required...", "I see"
                }
                else
                {
                    if (status_STR.Equals(PermissionStatus.Denied))
                    {
                        DisplayAlert("죄송해요!", "외부 디렉토리 저장 권한이 필요해요...", "그렇군요");
                        //"申し訳ありません！","外部ディレクトリの保存許可が必要です...","なるほど"
                        //"Sorry!", "External directory storage permission is required...", "I see"
                        permission.RequestPermissionsAsync(new Permission[] { Permission.Storage }).ContinueWith(async delegate
                        {
                            status_STR = await permission.CheckPermissionAsync(Permission.Storage);
                        });
                    }
                }
            }
            //*/
        }

        void SetPlay(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = e.SelectedItem as ViewModel.RecordViewModel;

            //record start
            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsEnabled = false;

            player.Stop();
            if (playerReady = player.Prepare(selectedItem.filePath))
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
            switch(args.Type)
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
                    if(Math.Abs(StartP.X - LastP.X) > 1)
                        player.SeekTo(slider.Value * MaxTime);
                    break;
            }
        }

        // 재생버튼과 정지버튼은 상호 보이지 않게 한다.

        async void Btn_Home(object sender, EventArgs s)
        {
            cts = false;
            playerReady = player.Release();
            await Navigation.PopToRootAsync(false);
        }

        void Btn_Play(object sender, EventArgs s)
        {
            cts = true;
            if (playerReady)
            {
                player.Start();

                var task = new Task(Update);
                task.Start();

                play.IsVisible = false;
                play.IsEnabled = false;
                pause.IsVisible = true;
                pause.IsEnabled = true;
            }
        }

        void Btn_Pause(object sender, EventArgs s)
        {
            cts = false;

            player.Stop();

            //record start
            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsEnabled = false;
        }

        async void Btn_Option(object sender, EventArgs s)
        {
            if (selectedItem != null)
            {
                string action = await DisplayActionSheet("동작", "취소", "제거", "공유");
                //"動作","キャンセル","削除","共有"
                //"Action", "Cancel", "Delete", "Share"
                if (action == null)
                    return;
                else if (action.Equals("제거")) //"削除"
                    await Delete();
                else if (action.Equals("공유")) //"共有"
                    await StaticDatas.Share.Share(selectedItem.filePath);
            }
        }

        async Task<bool> Delete()
        {
            var delete = await DisplayAlert("삭제하시겠어요?", "되돌릴 수 없어요!", "네", "아니요");
            //"削除しますか","元に戻すことはできません","はい","いいえ"
            //"Do you want to delete?", "Can not be undone", "Yes", "No"

            if (delete)
            {
                cts = false;

                player.Stop();
                playerReady = player.Release();

                //record start
                play.IsVisible = true;
                play.IsEnabled = true;
                pause.IsVisible = false;
                pause.IsEnabled = false;

                File.Delete(selectedItem.filePath);
                record.Remove(selectedItem);

                Device.BeginInvokeOnMainThread(delegate
                {
                    duration.Text = "00:00  ";
                    current.Text = "  00:00";
                    slider.Value = 0;
                });

                selectedItem = null;
                return true;
            }
            return false;
        }

        void OnRefreshing(object sender, EventArgs s)
        {
            var list = (sender as Xamarin.Forms.ListView);
            //put your refreshing logic here

            record.Clear();

            Device.BeginInvokeOnMainThread(Load);

            //make sure to end the refresh state
            list.IsRefreshing = false;
        }

        // current Text change with current time
        void Update()
        {
            while (cts)
            {
                if (!changeValue)
                {
                    double time = player.GetCurrentTime();
                    string CurrentT = double2String(time, "  {0:00}:{1:00}");

                    Device.BeginInvokeOnMainThread(delegate
                    {
                        slider.Value = time / MaxTime;
                        current.Text = CurrentT;
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
            cts = false;
            playerReady = player.Release();
            await Navigation.PopAsync(false);
        }

        protected override bool OnBackButtonPressed()
        {
            cts = false;
            playerReady = player.Release();
            Navigation.PopAsync(false);
            return true;
        }
    }
}
