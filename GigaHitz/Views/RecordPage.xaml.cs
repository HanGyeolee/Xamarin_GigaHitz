using System;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GigaHitz.PermissionApi;
using Xamarin.Forms;

namespace GigaHitz.Views
{
    public partial class RecordPage : ContentPage
    {
        Interfaces.IAudioRecorder recorder;
        IPermission permission;

        PermissionStatus status_MIC;
        string fileName, path, filePath;

        DateTime dt, nt;

        bool cts;

        public RecordPage()
        {
            InitializeComponent();

            edit.Keyboard = Keyboard.Plain;

            recorder = DependencyService.Get<Interfaces.IAudioRecorder>();
            permission = DependencyService.Get<IPermission>();

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            path = CreateDirectory();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            fileName = string.Format("Record-{0}", DateTime.Now.ToString("MMddHHmmss"));
            filePath = Path.Combine(path, fileName + ".m4a");

            edit.Text = fileName;

            Device.StartTimer(TimeSpan.FromSeconds(1), delegate
            {
                dt = DateTime.Now;
                return true;
            });

            status_MIC = permission.CheckPermissionAsync(Permission.Microphone).Result;
        }

        string CreateDirectory()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            directory = Path.Combine(directory, "Records");
            return directory;
        }

        void NameChanged(object sender, TextChangedEventArgs e)
        {
            fileName = e.NewTextValue;
            filePath = Path.Combine(path, fileName + ".m4a");
        }

        async void Btn_Back(object sender, EventArgs s)
        {
            recorder.Release();
            await Navigation.PopAsync(false);
        }

        async void Btn_Option(object sender, EventArgs s)
        {
            recorder.Release();
            await Navigation.PushAsync(new RecordPage(), false);
        }

        async void Btn_List(object sender, EventArgs s)
        {
            recorder.Release();
            await Navigation.PushAsync(new RecordContent.ListPage(), false);
        }

        async void Btn_Record(object sender, EventArgs s)
        {
            if (status_MIC.Equals(PermissionStatus.Granted))
            {
                if (recorder.Setting(filePath))
                {
                    cts = true;

                    recorder.Recording();

                    var task = new Task(Update);
                    task.Start();

                    record.IsVisible = false;
                    record.IsEnabled = false;
                    pause.IsVisible = true;
                    pause.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("죄송해요!", "파일명을 한글로 저장할 수 없어요...", "그렇군요");
                    //"申し訳ありません！","ファイル名を日本語で保存することができません...","なるほど"
                    //"Sorry!", "The file name can not be saved in a language other than English.", "I see"
                }
            }
            else
            {
#region RequestPermission
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    if (status_MIC.Equals(PermissionStatus.Unknown))
                    {
                        await permission.RequestPermissionsAsync(new Permission[] { Permission.Microphone }).ContinueWith(async delegate
                        {
                            status_MIC = await permission.CheckPermissionAsync(Permission.Microphone);
                        });
                    }
                    //else if (!status_MIC.Equals(PermissionStatus.Granted))
                    //    await DisplayAlert("죄송해요!", "마이크 권한이 필요해요...", "그렇군요");
                    //"申し訳ありません！","マイクの許可が必要です...","なるほど"
                    //"Sorry!", "Microphone permission is required...", "I see"
                }
                else
                {
                    if (!status_MIC.Equals(PermissionStatus.Granted))
                    {
                        //await DisplayAlert("죄송해요!", "마이크 권한이 필요해요...", "그렇군요");
                        //"申し訳ありません！","マイクの許可が必要です...","なるほど"
                        //"Sorry!", "Microphone permission is required...", "I see"
                        await permission.RequestPermissionsAsync(new Permission[] { Permission.Microphone }).ContinueWith(async delegate
                        {
                            status_MIC = await permission.CheckPermissionAsync(Permission.Microphone);
                        });
                    }
                }
#endregion
            }
        }

        void Btn_Pause(object sender, EventArgs s)
        {
            recorder.Stop();
            recorder.Reset();

            cts = false;

            //why dont stop?
            fileName = string.Format("Record-{0}", DateTime.Now.ToString("MMddHHmmss"));

            Device.BeginInvokeOnMainThread(delegate
            {
                duration.Text = double2String(0, "{0:00}:{1:00}  ");
                edit.Text = fileName;
            });

            //record start
            record.IsVisible = true;
            record.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsVisible = false;
        }

        void Update()
        {
            TimeSpan tmp;

            nt = dt;
            while (cts)
            {
                tmp = dt.Subtract(nt);

                string time = double2String(tmp.TotalSeconds, "{0:00}:{1:00}  ");

                Device.BeginInvokeOnMainThread(delegate
                {
                    duration.Text = time;
                });
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
                    tmp / 3600 ,
                    (tmp % 3600) / 60, 
                    tmp % 60);
            return get;
        }
    }
}
