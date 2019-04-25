using System;
using System.Net;
using System.Net.NetworkInformation;
using Xamarin.Forms.PlatformConfiguration;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using GigaHitz.Interfaces;
using Xamarin.Forms;

namespace GigaHitz.DataBase
{
    public class AppVersion
    {
        public string Name { get; set; } = "2.1.5";
        public int Numb { get; set; } = 17;
    }

    public static class StaticDatas
    {
        public static IPianoSound PianoSound { get; private set; } = DependencyService.Get<IPianoSound>();
        public static ISoundEffect SoundEffect { get; private set; } = DependencyService.Get<ISoundEffect>();
        public static IShare Share { get; private set; } = DependencyService.Get<Interfaces.IShare>();
        public static List<string> PreUrls { get; private set; }
        public static List<string> Urls { get; private set; }
        public static short Num { get; private set; } = 0;
        public static short NumPosterOpened { get; private set; } = 10;

        readonly static int sleep = 2;

        public static void Init(IProgress<double> progress)
        {
            soundLoad(); // metronome sound load
            progress.Report(25);
            Load().Start();
            progress.Report(50); 
            CheckPosterNum(); // get poster number from firebase database
            progress.Report(75);
            InitializePreUrls();
            progress.Report(100);
        }

        // Initialize Image
        public static async Task<short> InitializeUrls(CancellationToken token)
        {
            if (Num > 0)
            {
                Urls = null;
                var i = Num;

                Urls = new List<string>(NumPosterOpened);
                while (i > Num - NumPosterOpened)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    catch (Exception)
                    {
                        return 1; // cancel
                    }
                    await Task.Delay(250);
                    if (NetworkInterface.GetIsNetworkAvailable())
                        //get Unshorten URI of poster from shorten URI
                        Urls.Add(GetPosterUrl(i--));
                    else
                        return -1; //Task.FromResult<bool>(false);
                }
            }

            return 0; // 정상종료 //Task.FromResult<bool>(true);
        }

        /*
        // Initialize Image
        public static Task<bool> InitializeUrls(ref IProgress<double> progress)
        {
            if (Num > 0)
            {
                Urls = null;
                var i = Num;
                if (Num <= 10)
                {
                    Urls = new List<string>(Num);
                    while (i > 0)
                    {
                        progress.Report((1 - (i / (double)Num)) * 80 + 20);
                        if (NetworkInterface.GetIsNetworkAvailable())
                            //get Unshorten URI of poster from shorten URI
                            Urls.Add(GetPosterUrl(i--));
                        else
                            return Task.FromResult<bool>(false);
                    }
                }
                else
                {
                    int j = 0;
                    Urls = new List<string>(10);
                    while (i > Num - 10)
                    {
                        progress.Report(j++ / 10.0 * 80 + 20);
                        if (NetworkInterface.GetIsNetworkAvailable())
                            //get Unshorten URI of poster from shorten URI
                            Urls.Add(GetPosterUrl(i--));
                        else
                            return Task.FromResult<bool>(false);
                    }
                }
            }
            return Task.FromResult<bool>(true);
        }
        //*/

        // Retire Image Uri
        public static async Task<short> RetireUrls(CancellationToken token)
        {
            if (Num > 0)
            {
                var i = Num;
                var buf = Urls.Capacity;
                // if buf == Num, doesn't work
                if (i <= 10)
                {
                    for(var j = 1; j <= buf - Num; j++) // if buf > Num, delete last of buf
                        Urls.RemoveAt(buf - j);

                    while (i > buf) // if Num > buf
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                        }
                        catch (Exception)
                        {
                            return 1; // cancel
                        }
                        await Task.Delay(250);

                        if (NetworkInterface.GetIsNetworkAvailable())
                            //get Unshorten URI of poster from shorten URI
                            Urls.Insert(Num - i, GetPosterUrl(i--));
                        else
                            return -1;
                    }
                }
                else
                {
                    while (i > 10)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                        }
                        catch (Exception)
                        {
                            return 1; // cancel
                        }
                        await Task.Delay(250);

                        if (NetworkInterface.GetIsNetworkAvailable())
                        {
                            //Remove item at last
                            Urls.RemoveAt(9);
                            //get Unshorten URI of poster from shorten URI
                            //Insert item at first
                            Urls.Insert(Num - i, GetPosterUrl(i--));
                        }
                        else
                            return -1;
                    }
                }
            }
            return 0;
        }

        // get poster number from firebase database
        public static short CheckPosterNum()
        {
            Num = 0;
            var client = new WebClient();
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                    //TODO
                    Num = Convert.ToInt16(client.DownloadString("https://gigahitz-41617.firebaseio.com/Config/PosterNum.json"));
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                Num = 10;
            }
            return Num;
        }

        public static string CheckNotice()
        {
            string not;
            var client = new WebClient();
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                    //TODO
                    not = client.DownloadString("https://gigahitz-41617.firebaseio.com/Config/Notice.json");
                else
                    throw new Exception();

                not = not.Split('\"')[1];
            }
            catch (Exception)
            {
                not = "0";
            }

            return not;
        }

        //get shorten uri
        private static string GetPosterUrl(int index)
        {
            //TODO
            var response = WebRequest.Create("https://gigahitz.page.link/" + index.ToString()).GetResponse();

            string uri;
            try
            {
                //unshortener method
                uri = response.ResponseUri.AbsoluteUri;
            }
            catch(Exception)
            {
                response.Close();
                return "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/Chess_xxt45.svg/1024px-Chess_xxt45.svg.png";
            }

            response.Close();
            return uri;
        }

        // Initialize Image
        public static bool InitializePreUrls()
        {
            if (Num > 0)
            {
                PreUrls = null;
                var i = Num;

                PreUrls = new List<string>(NumPosterOpened);
                while (i > Num - NumPosterOpened)
                {
                    if (NetworkInterface.GetIsNetworkAvailable())
                        //get Unshorten URI of poster from shorten URI
                        PreUrls.Add(GetPrePosterUrl(i--));
                    else
                        PreUrls.Add("https://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/Chess_xxt45.svg/1024px-Chess_xxt45.svg.png");
                }
            }
            return true; // 정상종료 
        }

        private static string GetPrePosterUrl(int index)
        {
            //TODO
            var response = WebRequest.Create("https://gigahitz.page.link/pre" + index.ToString()).GetResponse();

            string uri;
            try
            {
                //unshortener method
                uri = response.ResponseUri.AbsoluteUri;
            }
            catch (Exception)
            {
                response.Close();
                return "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/Chess_xxt45.svg/1024px-Chess_xxt45.svg.png";
            }

            response.Close();
            return uri;
        }

        //Check Version
        public static int CheckVersion(string CurrentVersion ,int BuildVersion , out string UpdatedVersion, out int UpdatedBuildVersion)
        {
            try
            {
                var client = new WebClient();
                //인터넷 연결확인
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    //TODO
                    var buf = client.DownloadString("https://gigahitz-41617.firebaseio.com/Config/UpdatedVersion.json");
                    UpdatedVersion = buf.Split(':')[1]; // get Version in ":2.1.4:13:" from firebase database
                    UpdatedBuildVersion = Convert.ToInt32(buf.Split(':')[2]); // get Build Version in ":2.1.4:13:" from firebase database
                }
                else
                    throw new Exception(); //인터넷 연결안됬을 때
            }
            catch (Exception)
            {
                UpdatedVersion = CurrentVersion;
                UpdatedBuildVersion = BuildVersion;
                return -1; // 인터넷 연결 안됨
            }

            if (UpdatedBuildVersion > BuildVersion)  //Updated file exist
            {
                return 0; // 현재 버전이 업데이트 버전보다 낮을 경우
            }
            else if(UpdatedBuildVersion < BuildVersion)
            {
                return 1; // 현재 버전이 업데이트 버전보다 높을 경우 // 테스트 중
            }

            UpdatedVersion = CurrentVersion;
            UpdatedBuildVersion = BuildVersion;
            return -1; // 현재 버전과 업데이트 버전이 동일할 경우
        }

        public static async Task<bool> Load()
        {
            PianoSound.Initialize(24);

            #region Level2
            PianoSound.AddSystemSound("Do2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Di2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Re2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Ri2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Mi2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Fa2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Fi2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("So2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Si2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("La2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Li2");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Ti2");
            Thread.Sleep(sleep);
            #endregion

            #region Level3
            PianoSound.AddSystemSound("Do3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Di3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Re3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Ri3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Mi3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Fa3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Fi3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("So3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Si3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("La3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Li3");
            Thread.Sleep(sleep);
            PianoSound.AddSystemSound("Ti3");
            Thread.Sleep(sleep);
            #endregion

            return await Task.FromResult<bool>(true);
        }

        public static void soundLoad()
        {
            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                SoundEffect.Initialize(2);

                SoundEffect.AddSystemSound("Tik");
                SoundEffect.AddSystemSound("Tok");
            }
            else
            {
                SoundEffect.Initialize(6);

                SoundEffect.AddSystemSound("Tik");
                SoundEffect.AddSystemSound("Tok");
                SoundEffect.AddSystemSound("Tok");
                SoundEffect.AddSystemSound("Tok");
                SoundEffect.AddSystemSound("Tok");
                SoundEffect.AddSystemSound("Tok");
            }
        }

        public static void SystemSoundRelease()
        {
            PianoSound.Release();
            SoundEffect.Release();
        }
    }
}
