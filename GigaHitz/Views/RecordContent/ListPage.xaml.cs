using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GigaHitz.PermissionApi;
using Xamarin.Forms;

namespace GigaHitz.Views.RecordContent
{
    public partial class ListPage : ContentPage
    {
        Interfaces.IAudioPlayer player;
        ViewModel.RecordViewModel selectedItem;
        IPermission permission;
        PermissionStatus status_STR;

        double MaxTime;
        string path;
        bool changeValue;

        Point StartP, LastP, PastP;
        double buf;

        public ObservableCollection<ViewModel.RecordViewModel> record { get; set; }
        CancellationTokenSource cts;

        public ListPage()
        {
            InitializeComponent();

            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsVisible = false;

            record = new ObservableCollection<ViewModel.RecordViewModel>();
            player = DependencyService.Get<Interfaces.IAudioPlayer>();
            permission = DependencyService.Get<IPermission>();

            LV.RowHeight = 60;
            changeValue = false;

            // 슬라이드 설정을 해준다.
            // 여기서 슬라이드는 터치이펙트를 가지고 있다.

            path = CreateDirectory();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);

            status_STR = permission.CheckPermissionAsync(Permission.Storage).Result;
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
            string title, time, day;
            var filePath = Directory.GetFiles(path, "*.m4a");

            if (status_STR.Equals(PermissionStatus.Granted))
            {
                foreach (string namepath in filePath)
                {
                    title = namepath.Substring(path.Length + 1);

                    var dt = File.GetCreationTime(namepath);
                    day = dt.ToString("yyyy. MM. dd");

                    player.Prepare(namepath);
                    time = double2String(player.GetDurationTime(), "{0:00}:{1:00}");

                    record.Add(new ViewModel.RecordViewModel { Title = title, Time = time, Day = day, filePath = namepath });
                }
                LV.ItemsSource = record;
                player.Release();
            }
            else
            {
                permission.RequestPermissionsAsync(new Permission[] { Permission.Storage });
                status_STR = permission.CheckPermissionAsync(Permission.Storage).Result;
            }
        }

        void SetPlay(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = e.SelectedItem as ViewModel.RecordViewModel;

            //record start
            play.IsVisible = true;
            play.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsVisible = false;

            player.Stop();
            if (player.Prepare(selectedItem.filePath))
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
            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }
            player.Release();
            await Navigation.PopToRootAsync(false);
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }
            player.Release();
            await Navigation.PopAsync(false);
        }

        void Btn_Play(object sender, EventArgs s)
        {
            cts = new CancellationTokenSource();
            if (player != null)
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

        async void Btn_Option(object sender, EventArgs s)
        {
            if (selectedItem != null)
            {
                var action = await DisplayActionSheet("동작", "취소", "제거", "공유");
                if (action.Equals("제거"))
                    await Delete();
                else if (action.Equals("공유"))
                    DependencyService.Get<Interfaces.IShare>().Share(selectedItem.filePath);
            }
        }

        async Task<bool> Delete()
        {
            var delete = await DisplayAlert("삭제하시겠어요?", "되돌릴 수 없어요!", "네", "아니요");

            if (delete)
            {
                File.Delete(selectedItem.filePath);
                record.Remove(selectedItem);

                return true;
            }

            return false;
        }

        void OnRefreshing(object sender, EventArgs s)
        {
            var list = (sender as ListView);
            //put your refreshing logic here

            record.Clear();

            Load();

            //make sure to end the refresh state
            list.IsRefreshing = false;
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
    }
}
