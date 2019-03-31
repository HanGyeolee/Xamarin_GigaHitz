using System;

namespace GigaHitz.Interfaces
{
    public interface IAudioRecorder
    {
        bool SetBitRate(int kbps);
        bool SetSampleRate(float rate);
        bool Setting(string filePath);
        void Recording();
        void Stop();
        void Reset();
        void Release();
    }
}
