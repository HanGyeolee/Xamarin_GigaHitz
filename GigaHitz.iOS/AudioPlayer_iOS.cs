using System;
using System.IO;
using Xamarin.Forms;
using AudioToolbox;
using AVFoundation;
using Foundation;

[assembly: Dependency(typeof(GigaHitz.iOS.AudioPlayer_iOS))]
namespace GigaHitz.iOS
{
    public class AudioPlayer_iOS : Interfaces.IAudioPlayer
    {
        AVAudioPlayer player;
        AVPlayerItem item;
        NSError err;
        EventHandler<AVStatusEventArgs> _e;

        readonly float maxVolume = 1f;

        public AudioPlayer_iOS()
        {
            player = null;
        }

        public double GetDurationTime()
        {
            if (player != null)
                return player.Duration;
            else
                return 0;
        }

        public double GetCurrentTime()
        {
            if (player != null)
                return player.CurrentTime;
            else
                return 0;
        }

        public bool Prepare(string filePath)
        {
            if (_e != null && player != null)
                player.FinishedPlaying -= _e;
            //var url = new NSUrl(filePath, false); // can't
            //TODO check err
            var url = NSUrl.FromFilename(filePath); // can't
            AVAsset av = AVAsset.FromUrl(url);
            AVPlayerItem item = AVPlayerItem.FromAsset(av);

            //player = new AVAudioPlayer(url, "m4a", out err);
            player = AVAudioPlayer.FromUrl(url, out err);

            if (player != null)
            {
                player.Volume = maxVolume * 2;

                player.PrepareToPlay();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Prepare(string filePath, int channel)
        {
            if (_e != null && player != null)
                player.FinishedPlaying -= _e;
            //var url = new NSUrl("scales/" + filePath + ".mp3", false);
            var url = NSUrl.FromFilename("scales/" + filePath + ".mp3");

            player = AVAudioPlayer.FromUrl(url, out err);

            if (player != null)
            {
                player.Volume = maxVolume;

                player.PrepareToPlay();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Release()
        {
            if (player != null)
            {
                if(_e != null)
                    player.FinishedPlaying -= _e;
                if (player.Playing)
                    player.Stop();
                player = null;
            }
            return false;
        }

        public void SeekTo(double sec)
        {
            if (player != null)
            {
                if (player.Playing)
                {
                    player.Pause();
                    player.CurrentTime = sec;
                    player.PrepareToPlay();
                    player.Play();
                }
                else
                {
                    player.CurrentTime = sec;
                    player.PrepareToPlay();
                }
            }
        }


        public void Start()
        {
            if (player != null)
                player.Play();
        }

        public void Stop()
        {
            if (player != null)
                if(player.Playing)
                    player.Pause();
        }

        public void Finished(EventHandler e)
        {
            _e = (object sender, AVStatusEventArgs ea) =>
            {
                e?.Invoke(sender, EventArgs.Empty);
            };
            player.FinishedPlaying += _e;
        }
    }
}
