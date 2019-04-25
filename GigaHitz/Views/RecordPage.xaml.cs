using System;
using System.Text.RegularExpressions;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GigaHitz.DataBase;
using GigaHitz.PermissionApi;
using Xamarin.Forms;

namespace GigaHitz.Views
{
    public partial class RecordPage : ContentPage
    {
        Interfaces.IAudioRecorder recorder;
        IPermission permission;
        LocalDB dB;

        PermissionStatus status_MIC;
        string fileName, path, filePath;

        DateTime dt, nt;

        bool cts;

        public RecordPage()
        {
            InitializeComponent();

            //edit.Keyboard = Keyboard.Text;
            dB = new LocalDB();
            recorder = DependencyService.Get<Interfaces.IAudioRecorder>();
            permission = DependencyService.Get<IPermission>();

            ////status bar
            On<iOS>().SetUseSafeArea(true);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);

            path = CreateDirectory();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            fileName = string.Format("Records{0}", DateTime.Now.ToString("MMddHHmmss"));

            nt = dt = DateTime.Now;
            cts = false;

            edit.Text = fileName;

            edit.Unfocused += delegate {
                bool noname = false;
                var NewTextValue = edit.Text;

                if (Device.RuntimePlatform.Equals(Device.Android))              // 아직 안드로이드만 한글을 지원한다.
                {
                    if (noname = Regex.IsMatch(NewTextValue, "^[0-9a-zA-Z가-힣\x20]{1,17}$"))
                    {
                        fileName = NewTextValue;
                    }
                    else
                    {
                        DisplayAlert("죄송해요!", "파일명은 한글, 영문, 숫자만 가능해요...", "알겠어요");
                    }
                }
                else //ios                                                      // 아이폰에도 한글지원이 가능한 지 찾아보자.
                {
                    if (noname = Regex.IsMatch(NewTextValue, "^[0-9a-zA-Z\x20]{1,17}$"))
                    {
                        fileName = NewTextValue;
                    }
                    else
                    {
                        DisplayAlert("죄송해요!", "파일명은 영문, 숫자만 가능해요...", "알겠어요");
                    }
                }

                if(!noname) // 이름이 잘 못 되었을 경우, 리셋시킨다.
                {
                    fileName = string.Format("Records{0}", DateTime.Now.ToString("MMddHHmmss"));
                    Device.BeginInvokeOnMainThread(delegate
                    {
                        edit.Text = fileName;
                    });
                }
            };

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

        void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Device.RuntimePlatform.Equals(Device.Android))              // 아직 안드로이드만 한글을 지원한다.
            {
                if (!Regex.IsMatch(e.NewTextValue, "^[0-9a-zA-Z가-힣ㄱ-ㅎ\x20]{0,17}$"))
                {
                    Device.BeginInvokeOnMainThread(delegate
                    {
                        (sender as Xamarin.Forms.Entry).Text = e.OldTextValue;
                    });
                }
            }
            else //ios                                                      // 아이폰에도 한글지원이 가능한 지 찾아보자.
            {
                if (!Regex.IsMatch(e.NewTextValue, "^[0-9a-zA-Z\x20]{0,17}$"))
                {
                    Device.BeginInvokeOnMainThread(delegate
                    {
                        (sender as Xamarin.Forms.Entry).Text = e.OldTextValue;
                    });
                }
            }
        }

        void TextCompleted(object sender, EventArgs e)
        {
            (sender as Xamarin.Forms.Entry).Unfocus();
        }

        async void Btn_Option(object sender, EventArgs s)
        {
            recorder.Release();
            cts = false;

            //why dont stop?
            fileName = string.Format("Records{0}", DateTime.Now.ToString("MMddHHmmss"));

            Device.BeginInvokeOnMainThread(delegate
            {
                duration.Text = double2String(0, "{0:00}:{1:00}  ");
                edit.Text = fileName;
            });

            //record start
            record.IsVisible = true;
            record.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsEnabled = false;
            edit.IsEnabled = true;

            await Navigation.PushAsync(new RecordContent.OptionPage(), false);
        }

        async void Btn_List(object sender, EventArgs s)
        {
            recorder.Release();
            cts = false;

            //why dont stop?
            fileName = string.Format("Records{0}", DateTime.Now.ToString("MMddHHmmss"));

            Device.BeginInvokeOnMainThread(delegate
            {
                duration.Text = double2String(0, "{0:00}:{1:00}  ");
                edit.Text = fileName;
            });

            //record start
            record.IsVisible = true;
            record.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsEnabled = false;
            edit.IsEnabled = true;

            await Navigation.PushAsync(new RecordContent.ListPage(), false);
        }

        async void Btn_Record(object sender, EventArgs s)
        {
            if (fileName.Length > 0)
                filePath = Path.Combine(path, fileName + ".m4a");
            Device.BeginInvokeOnMainThread(delegate
            {
                edit.Text = fileName;
            });
            if (status_MIC.Equals(PermissionStatus.Granted))
            {
                if (dB.IsExist()) // 존재 할 떄
                {
                    int kbps;
                    float rate;
                    kbps = int.Parse(dB.ReadIndexOf(0));
                    //dB.Read(out kbps);
                    recorder.SetBitRate(kbps);
                    rate = float.Parse(dB.ReadIndexOf(1));
                    //dB.Read(out rate);
                    recorder.SetSampleRate(rate);
                }
                else
                {
                    recorder.SetBitRate(256);
                    recorder.SetSampleRate(44100f);
                }

                if (recorder.Setting(filePath))
                {
                    cts = true;

                    recorder.Recording();

                    var task = new Task(Update);
                    task.Start();

                    record.IsVisible = false;
                    record.IsEnabled = false;
                    pause.IsVisible = true;
                    pause.IsEnabled = true;
                    edit.IsEnabled = false;
                }
                /*else
                {
                    await DisplayAlert("죄송해요!", "파일명을 한글로 저장할 수 없어요...", "그렇군요");
                    //"申し訳ありません！","ファイル名を日本語で保存することができません...","なるほど"
                    //"Sorry!", "The file name can not be saved in a language other than English.", "I see"
                }
                //*/
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
            fileName = string.Format("Records{0}", DateTime.Now.ToString("MMddHHmmss"));

            Device.BeginInvokeOnMainThread(delegate
            {
                duration.Text = double2String(0, "{0:00}:{1:00}  ");
                edit.Text = fileName;
            });

            //record start
            record.IsVisible = true;
            record.IsEnabled = true;
            pause.IsVisible = false;
            pause.IsEnabled = false;
            edit.IsEnabled = true;
        }

        void Update()
        {
            TimeSpan tmp;

            dt = DateTime.Now;
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

        async void Btn_Back(object sender, EventArgs s)
        {
            recorder.Release();
            cts = false;
            await Navigation.PopAsync(false);
        }

        protected override bool OnBackButtonPressed()
        {
            recorder.Release();
            cts = false;
            Navigation.PopAsync(false);
            return true;
        }
    }
}
