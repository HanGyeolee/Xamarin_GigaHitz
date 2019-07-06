using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.OS;
using Android.Media;
using Android.Content.Res;

[assembly: Dependency(typeof(GigaHitz.Droid.SoundEffect_Android))]
namespace GigaHitz.Droid
{
    public class SoundEffect_Android : Interfaces.ISoundEffect
    {
        SoundPool sp;
        AssetManager asset;

        int count;
        int[] SoundId;
        int[] StreamId;

        public void Initialize(int Index)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop) // android 5.0
            {
                AudioAttributes attr = new AudioAttributes.Builder()
                    .SetUsage(AudioUsageKind.Game)
                    .SetContentType(AudioContentType.Sonification)
                    .Build();
                sp = new SoundPool.Builder()
                    .SetAudioAttributes(attr)
                    .SetMaxStreams(Index)
                    .Build();
            }
            else
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
                sp = new SoundPool(Index, Android.Media.Stream.System, 0);
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.

            count = 0;

            asset = Android.App.Application.Context.Assets;
            SoundId = new int[Index];
            StreamId = new int[Index];

        }

        public bool Play(int Index)
        {
            if(Index < count)
                StreamId[Index] = sp.Play(SoundId[Index], 1, 1, 0, 0, 1);
            else
                StreamId[count - 1] = sp.Play(SoundId[count - 1], 1, 1, 0, 0, 1);
            return true;
        }

        public void AddSystemSound(string filePath)
        {
            string path = Path.Combine("sounds", filePath + ".mp3");
            var afd = asset.OpenFd(path);
            SoundId[count] = sp.Load(afd, 1);
            sp.SetVolume(SoundId[count++], 1, 1);
        }

        public async Task<bool> Stop(int Index)
        {
            if (Index < count)
                sp.Stop(StreamId[Index]);
            else
                sp.Stop(StreamId[count - 1]);
            return await Task.FromResult<bool>(true);
        }

        public void Release()
        {
            sp.Release();
            SoundId = null;
            StreamId = null;
        }
    }
}
