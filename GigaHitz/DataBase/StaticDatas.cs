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
    class Colorset
    {
        public Color NC;
        public Color NCDD;
        public Color EC;
        public Color ECD;
        public Color ECDD;
        public Color RC;
        public Color RCD;
        public Color RCDD;
        public Color IC;

        public Colorset()
        {
            NC = Color.FromHex("DF71A9");
            NCDD = Color.FromHex("CD5D95");
            EC = Color.FromHex("879CFE");
            ECD = Color.FromHex("7E92F4");
            ECDD = Color.FromHex("7588EA");
            RC = Color.FromHex("F6AC64");
            RCD = Color.FromHex("EDA25A");
            RCDD = Color.FromHex("E49850");
            IC = Color.FromHex("A16FD6");
        }

        public static Colorset Json2ColorSet(string json)
        {
            Colorset colorset = new Colorset();

            var StrSet = json.Split('\"');

            colorset.EC = Color.FromHex(StrSet[3]);     // 1
            colorset.ECD = Color.FromHex(StrSet[7]);    // 5
            colorset.ECDD = Color.FromHex(StrSet[11]);  // 9
            colorset.IC = Color.FromHex(StrSet[15]);    // 13
            colorset.NC = Color.FromHex(StrSet[19]);    // 17
            colorset.NCDD = Color.FromHex(StrSet[23]);   // 21
            colorset.RC = Color.FromHex(StrSet[27]);  // 25
            colorset.RCD = Color.FromHex(StrSet[31]);    // 29
            colorset.RCDD = Color.FromHex(StrSet[35]);   // 31

            return colorset;
        }
    }

    public static class AppVersion
    {
        public static string Name { get; private set; } = "2.1.7";
        public static int Numb { get; private set; } = 28;
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

        private static readonly int[] priority = { 50, 25, 5, 1 };
        private static readonly bool[] IsPianoSoundLoadDone = { false, false, false, false };

        //readonly static int sleep = 1;

        public static void Init(IProgress<double> progress)
        {
            soundLoad(); // metronome sound load
            progress.Report(25);
            CheckDate(); // check datetime now
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

        private static void SetResourcesColor(string SpecialDay)
        {
            Colorset colorset = new Colorset();
            var client = new WebClient();

            string tmp = "0";
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                    tmp = client.DownloadString($"https://gigahitz-41617.firebaseio.com/Event/{SpecialDay}.json");
                else
                    throw new Exception();

                var l = tmp.Length - 2;
                tmp = tmp.Substring(1, l);
                colorset = Colorset.Json2ColorSet(tmp);

                Device.BeginInvokeOnMainThread(delegate
                {
                    Application.Current.Resources["NoticeColor"] = colorset.NC;
                    Application.Current.Resources["NoticeColor_DeepDark"] = colorset.NCDD;
                    Application.Current.Resources["etcColor"] = colorset.EC;
                    Application.Current.Resources["etcColor_Dark"] = colorset.ECD;
                    Application.Current.Resources["etcColor_DeepDark"] = colorset.ECDD;
                    Application.Current.Resources["RecordColor"] = colorset.RC;
                    Application.Current.Resources["RecordColor_Dark"] = colorset.RCD;
                    Application.Current.Resources["RecordColor_DeepDark"] = colorset.RCDD;
                    Application.Current.Resources["infoColor"] = colorset.IC;
                });
            }
            catch (Exception)
            {
                return ;
            }
        }

        public static short CheckDate()
        {
            var now = DateTime.Now.ToLocalTime();
            var M = now.Month;
            var D = now.Day;

            if (M == 12 && 23 < D && D < 26)
            {
                SetResourcesColor("Christmas");
            }
            else if (M == 10 && D == 31)
            {
                SetResourcesColor("Halloween");
            }

            return 0;
        }

        public static string CheckNotice()
        {
            string not;
            var client = new WebClient();

            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
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
        public static int CheckVersion(out string UpdatedVersion, out int UpdatedBuildVersion)
        {
            try
            {
                var client = new WebClient();
                //인터넷 연결확인
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    var buf = client.DownloadString("https://gigahitz-41617.firebaseio.com/Config/UpdatedVersion.json");
                    UpdatedVersion = buf.Split(':')[1]; // get Version in ":2.1.4:13:" from firebase database
                    UpdatedBuildVersion = Convert.ToInt32(buf.Split(':')[2]); // get Build Version in ":2.1.4:13:" from firebase database
                }
                else
                    throw new Exception(); //인터넷 연결안됬을 때
            }
            catch (Exception)
            {
                UpdatedVersion = AppVersion.Name;
                UpdatedBuildVersion = AppVersion.Numb;
                return -1; // 인터넷 연결 안됨
            }

            if (UpdatedBuildVersion > AppVersion.Numb)  //Updated file exist
            {
                return 0; // 현재 버전이 업데이트 버전보다 낮을 경우
            }
            else if(UpdatedBuildVersion < AppVersion.Numb)
            {
                return 1; // 현재 버전이 업데이트 버전보다 높을 경우 // 테스트 중
            }

            UpdatedVersion = AppVersion.Name;
            UpdatedBuildVersion = AppVersion.Numb;
            return -1; // 현재 버전과 업데이트 버전이 동일할 경우
        }

        public static void Load()
        {
            PianoSound.Initialize(48);

            Task t1 = new Task(() => Loadscale(1));
            Task t2 = new Task(() => Loadscale(2));
            Task t3 = new Task(() => Loadscale(3));
            Task t4 = new Task(() => Loadscale(4));

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
        }
        /*
        public static void Load(CancellationToken token)
        {
            PianoSound.Initialize(48);

            Task t1 = new Task(() => Loadscale(1), token);
            Task t2 = new Task(() => Loadscale(2), token);
            Task t3 = new Task(() => Loadscale(3), token);
            Task t4 = new Task(() => Loadscale(4), token);

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

#if false
            ///*
            #region Level2
            PianoSound.AddSystemSound("Do2", 12);
            PianoSound.AddSystemSound("Di2", 13);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Re2", 14);
            PianoSound.AddSystemSound("Ri2", 15);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Mi2", 16);
            PianoSound.AddSystemSound("Fa2", 17);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Fi2", 18);
            PianoSound.AddSystemSound("So2", 19);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Si2", 20);
            PianoSound.AddSystemSound("La2", 21);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Li2", 22);
            PianoSound.AddSystemSound("Ti2", 23);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            #endregion

            #region Level3
            PianoSound.AddSystemSound("Do3", 24);
            PianoSound.AddSystemSound("Di3", 25);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Re3", 26);
            PianoSound.AddSystemSound("Ri3", 27);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Mi3", 28);
            PianoSound.AddSystemSound("Fa3", 29);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Fi3", 30);
            PianoSound.AddSystemSound("So3", 31);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Si3", 32);
            PianoSound.AddSystemSound("La3", 33);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Li3", 34);
            PianoSound.AddSystemSound("Ti3", 35);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            #endregion

            #region Level4
            PianoSound.AddSystemSound("Do4", 36);
            PianoSound.AddSystemSound("Di4", 37);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Re4", 38);
            PianoSound.AddSystemSound("Ri4", 39);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Mi4", 40);
            PianoSound.AddSystemSound("Fa4", 41);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Fi4", 42);
            PianoSound.AddSystemSound("So4", 43);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Si4", 44);
            PianoSound.AddSystemSound("La4", 45);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            PianoSound.AddSystemSound("Li4", 46);
            PianoSound.AddSystemSound("Ti4", 47);
            try
            {
                token.ThrowIfCancellationRequested();
            }
            catch (Exception)
            {
                return await Task.FromResult<bool>(false); // cancel
            }
            finally
            {
                Thread.Sleep(sleep);
            }
            #endregion

#endif
        }
        //*/
        public static void ReleasePianoSound()
        {
            IsPianoSoundLoadDone[0] = false;
            IsPianoSoundLoadDone[1] = false;
            IsPianoSoundLoadDone[2] = false;
            IsPianoSoundLoadDone[3] = false;
        }

        public static bool PianoSoundsAreLoadingDone()
        {
            return IsPianoSoundLoadDone[0] & IsPianoSoundLoadDone[1] & IsPianoSoundLoadDone[2] & IsPianoSoundLoadDone[3];
        }

        public static void SetPianoLoadingPriority(int level, int pri)
        {
            priority[level - 1] = pri;
        }

        private static async void Loadscale(int level)
        {
            int gap = (level - 1) * 12;

#region Level1
            PianoSound.AddSystemSound("Do" + level, 0 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("Di" + level, 1 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("Re" + level, 2 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("Ri" + level, 3 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("Mi" + level, 4 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("Fa" + level, 5 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("Fi" + level, 6 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("So" + level, 7 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("Si" + level, 8 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("La" + level, 9 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("Li" + level, 10 + gap);
            await Task.Delay(priority[level - 1]);
            PianoSound.AddSystemSound("Ti" + level, 11 + gap);
            await Task.Delay(priority[level - 1]);
            #endregion

            //Console.WriteLine("::::::::Loaded complete Level : " + level);

            IsPianoSoundLoadDone[level - 1] = true;
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
            ReleasePianoSound();
            SoundEffect.Release();
        }
    }
}
