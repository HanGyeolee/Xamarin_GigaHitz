using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Foundation;
using AVFoundation;

[assembly: Dependency(typeof(GigaHitz.iOS.PianoSound_iOS))]
namespace GigaHitz.iOS
{
    public class PianoSound_iOS : Interfaces.IPianoSound
    {
        AVAudioPlayer[] sp;
        NSError err;
        int count;


        public void Initialize(int Index)
        {
            sp = new AVAudioPlayer[Index];
            count = 0;
        }

        public bool Play(int Index)
        {
            if (Index < count)
            {
                sp[Index].Play();
                return true;
            }
            return false;
        }

        public void AddSystemSound(string filePath)
        {
            var path = new NSUrl("sounds/" + filePath + ".mp3");
            sp[count] = new AVAudioPlayer(path, "mp3", out err);
            sp[count++].PrepareToPlay();
        }

        public async Task<bool> Stop(int Index)
        {
            if (Index < count)
            {
                if (sp[Index].Playing)
                    sp[Index].Stop();
                sp[Index].CurrentTime = 0;
                sp[Index].PrepareToPlay();
                return await Task.FromResult<bool>(true);
            }
            return await Task.FromResult<bool>(false);
        }

        public void Release()
        {
            sp = null;
        }

    }
}
