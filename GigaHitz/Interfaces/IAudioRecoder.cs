using System;
using System.Threading.Tasks;

namespace GigaHitz.Interfaces
{
    public interface IAudioRecorder
    {
        bool SetBitRate(int kbps);
        bool SetSampleRate(float rate);
        bool Setting(string filePath);
        void Recording(Action update);
        void Stop();
        void Reset();
        void Release();
    }
}
