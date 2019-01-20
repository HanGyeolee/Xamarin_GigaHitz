using System;

namespace GigaHitz.Interfaces
{
    public interface IAudioRecorder
    {
        bool Setting(string filePath);
        void Recording();
        void Stop();
        void Reset();
        void Release();
    }
}
