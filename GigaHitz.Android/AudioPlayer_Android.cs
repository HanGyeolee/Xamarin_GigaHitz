using System;
using Java.IO;
using Xamarin.Forms;
using Android.OS;
using Android.Media;
using Android.Content.Res;
using Android.Content.PM;

[assembly: Dependency(typeof(GigaHitz.Droid.AudioPlayer_Android))]
namespace GigaHitz.Droid
{
    public class AudioPlayer_Android : Interfaces.IAudioPlayer
    {
        MediaPlayer player;
        AssetManager asset;
        EventHandler _e;

        public AudioPlayer_Android()
        {
            asset = Android.App.Application.Context.Assets;
        }

        public double GetDurationTime()
        {
            if (player != null)
                return (player.Duration / 1000.0);
            else
                return 0;
        }

        public double GetCurrentTime()
        {
            if (player != null)
                return (player.CurrentPosition / 1000.0);
            else
                return 0;
        }

        public bool Prepare(string filePath)
        {
            if (_e != null)
                player.Completion -= _e;
            player = new MediaPlayer();
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                player.SetDataSource(filePath);
            }
            else
            {
                FileInputStream fd = new FileInputStream(new File(filePath));
                player.SetDataSource(fd.FD);
                fd.Close();
            }

            player.SetVolume(1, 1);

            try
            {
                player.Prepare();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool Prepare(string filePath, int channel)
        {
            if (_e != null)
                player.Completion -= _e;
            player = new MediaPlayer();
            var fd = asset.OpenFd("scales/" + filePath + ".mp3");

            player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);

            player.SetVolume(1, 1);

            try
            {
                player.Prepare();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public void Release()
        {
            if (player != null)
            {
                if (_e != null)
                    player.Completion -= _e;
                player.Reset();
                player = null;
                _e = null;
            }
        }

        public void SeekTo(double sec)
        {
            if (player != null)
            {
                if (player.IsPlaying)
                {
                    player.Pause();
                    player.SeekTo((int)(sec * 1000));
                    player.Start();
                }
                else
                {
                    player.Pause();
                    player.SeekTo((int)sec * 1000);
                }
            }
        }


        public void Start()
        {
            if (player != null)
                player.Start();
        }

        public void Stop()
        {
            if (player != null)
                if(player.IsPlaying)
                    player.Pause();
        }

        public void Finished(EventHandler e)
        {
            _e = e;
            player.Completion += _e;
        }
    }
}
