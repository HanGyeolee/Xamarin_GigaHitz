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
    public static class StaticDatas
    {
        public static IPianoSound PianoSound { get; private set; } = DependencyService.Get<IPianoSound>();
        public static ISoundEffect SoundEffect { get; private set; } = DependencyService.Get<ISoundEffect>();
        public static IShare Share { get; private set; } = DependencyService.Get<Interfaces.IShare>();
        public static List<string> Uris { get; private set; }
        public static short Num { get; private set; } = 0;
        public static short NumPosterOpened { get; private set; } = 0;

        readonly static int sleep = 2;
        readonly private static string CheckVersionUrl = "https://gigahitz-41617.firebaseio.com/Config/UpdatedVersion.json";
        readonly private static string CheckPosterUrl  = "https://gigahitz-41617.firebaseio.com/Config/PosterNum.json";
        readonly private static string PosterUrl       = "https://gigahitz.page.link/";

        public static void Init(IProgress<double> progress)
        {

            soundLoad(); // metronome sound load
            progress.Report(33);
            var b = Load().Result;
            progress.Report(66);
            CheckPosterNum(); // get poster number from firebase database
            progress.Report(100);
        }

        // Initialize Image
        public static async Task<short> InitializeUris(CancellationToken token)
        {
            if (Num > 0)
            {
                Uris = null;
                var i = Num;
                if (Num <= 10)
                {
                    Uris = new List<string>(Num);
                    while (i > 0)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                        }
                        catch(Exception)
                        {
                            return 1; // cancel
                        }
                        await Task.Delay(5);
                        if (NetworkInterface.GetIsNetworkAvailable())
                            //get Unshorten URI of poster from shorten URI
                            Uris.Add(GetPosterUrl(i--));
                        else
                            return -1; // 인터넷 연결 실패 // Task.FromResult<bool>(false);
                    }
                }
                else
                {
                    Uris = new List<string>(10);
                    while (i > Num - 10)
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                        }
                        catch (Exception)
                        {
                            return 1; // cancel
                        }
                        await Task.Delay(5);
                        if (NetworkInterface.GetIsNetworkAvailable())
                            //get Unshorten URI of poster from shorten URI
                            Uris.Add(GetPosterUrl(i--));
                        else
                            return -1; //Task.FromResult<bool>(false);
                    }
                }
            }
            return 0; // 정상종료 //Task.FromResult<bool>(true);
        }

        /*
        // Initialize Image
        public static Task<bool> InitializeUris(ref IProgress<double> progress)
        {
            if (Num > 0)
            {
                Uris = null;
                var i = Num;
                if (Num <= 10)
                {
                    Uris = new List<string>(Num);
                    while (i > 0)
                    {
                        progress.Report((1 - (i / (double)Num)) * 80 + 20);
                        if (NetworkInterface.GetIsNetworkAvailable())
                            //get Unshorten URI of poster from shorten URI
                            Uris.Add(GetPosterUrl(i--));
                        else
                            return Task.FromResult<bool>(false);
                    }
                }
                else
                {
                    int j = 0;
                    Uris = new List<string>(10);
                    while (i > Num - 10)
                    {
                        progress.Report(j++ / 10.0 * 80 + 20);
                        if (NetworkInterface.GetIsNetworkAvailable())
                            //get Unshorten URI of poster from shorten URI
                            Uris.Add(GetPosterUrl(i--));
                        else
                            return Task.FromResult<bool>(false);
                    }
                }
            }
            return Task.FromResult<bool>(true);
        }
        //*/

        // Retire Image Uri
        public static Task<bool> RetireUris()
        {
            if (Num > 0)
            {
                var i = Num;
                var buf = Uris.Capacity;
                if (i <= 10)
                {
                    Uris.Capacity = Num;
                    while (i > buf)
                        if (NetworkInterface.GetIsNetworkAvailable())
                            //get Unshorten URI of poster from shorten URI
                            Uris.Insert(Num - i, GetPosterUrl(i--));
                        else
                            return Task.FromResult<bool>(false);
                }
                else
                {
                    while (i > 10)
                        if (NetworkInterface.GetIsNetworkAvailable())
                        {
                            //Remove item at last
                            Uris.RemoveAt(9);
                            //get Unshorten URI of poster from shorten URI
                            //Insert item at first
                            Uris.Insert(Num - i, GetPosterUrl(i--));
                        }
                        else
                            return Task.FromResult<bool>(false);
                }
            }
            return Task.FromResult<bool>(true);
        }

        // get poster number from firebase database
        public static short CheckPosterNum()
        {
            Num = 0;
            var client = new WebClient();
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                    Num = Convert.ToInt16(client.DownloadString(CheckPosterUrl));
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                Num = 8;
            }
            if (Num <= 10)
                NumPosterOpened = Num;
            else
                NumPosterOpened = 10;

            return Num;
        }

        //get shorten uri
        private static string GetPosterUrl(int index)
        {
            var client = new WebClient();

            var addr = PosterUrl + index.ToString();

            string uri;
            try
            {
                //unshortener method
                uri = WebRequest.Create(addr).GetResponse().ResponseUri.AbsoluteUri;
            }
            catch(Exception)
            {
                return null;
            }

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
                    var buf = client.DownloadString(CheckVersionUrl);
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
