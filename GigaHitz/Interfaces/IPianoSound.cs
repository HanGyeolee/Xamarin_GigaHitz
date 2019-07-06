using System;
using System.Threading.Tasks;

namespace GigaHitz.Interfaces
{
    public interface IPianoSound
    {
        void AddSystemSound(string filePath);
        void AddSystemSound(string filePath, int index);
        void Initialize(int Index);
        bool Play(int Index);
        Task<bool> Stop(int Index);
        void Release();
    }
}
