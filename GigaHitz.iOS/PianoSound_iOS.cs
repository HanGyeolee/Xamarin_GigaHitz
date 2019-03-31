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
                sp[Index].Play();
            else
                sp[count - 1].Play();
            return true;
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
            }
            else
            {
                if (sp[count - 1].Playing)
                    sp[count - 1].Stop();
                sp[count - 1].CurrentTime = 0;
                sp[count - 1].PrepareToPlay();
            }

            return await Task.FromResult<bool>(true);
        }

        public void Release()
        {
            sp = null;
        }

    }
}
