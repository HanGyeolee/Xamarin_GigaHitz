using System;
namespace GigaHitz.Interfaces
{
    public interface IAudioPlayer
    {
        bool Prepare(string filePath);
        bool Prepare(string filePath, int channel);
        void Start();
        void Finished(EventHandler e);

        double GetCurrentTime();
        double GetDurationTime();
        /* iOS T=double
        [avAudioPlayer stop];
        [avAudioPlayer setCurrentTime:<span class="skimlinks-unlinked">aSlider.value</span>];
        [avAudioPlayer prepareToPlay];
        [avAudioPlayer play];
        */

        // seekto(msec) T=int32
        void SeekTo(double sec);
        void Stop();
        void Release();
    }
}
