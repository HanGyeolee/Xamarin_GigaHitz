﻿using System;
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
            player = new MediaPlayer();

            player.SetDataSource(filePath);

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
            player = new MediaPlayer();
            asset = Android.App.Application.Context.Assets;

            player.SetDataSource(asset.OpenFd("scales/" + filePath + ".mp3"));

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
                player.Reset();
                player = null;
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
                player.Pause();
        }

        public void Finished(EventHandler e)
        {
            player.Completion += e;
        }
    }
}
