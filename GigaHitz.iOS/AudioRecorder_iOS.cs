using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using AVFoundation;
using Foundation;

[assembly: Dependency(typeof(GigaHitz.iOS.AudioRecorder_iOS))]
namespace GigaHitz.iOS
{
    public class AudioRecorder_iOS : Interfaces.IAudioRecorder
    {
        AVAudioRecorder recorder;
        static int BitsPerSec = 256 * 1024;
        static float SRate = 48000f;

        public void Recording(Action update)
        {
            if (recorder != null)
            {
                recorder.Record();
                new Task(update).Start();
            }
        }

        public void Release()
        {
            if (recorder != null)
            {
                if (recorder.Recording)
                    recorder.Stop();
                recorder = null;
            }
        }

        public void Reset()
        {
            recorder = null;
        }

        public bool SetBitRate(int kbps)
        {
            switch (kbps)
            {
                case 96:
                case 112:
                case 128:
                case 160:
                case 192:
                case 224:
                case 256:
                case 320:
                    BitsPerSec = kbps * 1024;
                    return true;
                default:
                    return false;
            }
        }

        public bool SetSampleRate(float rate)
        {
            int tmp = (int)(rate);
            switch (tmp)
            {
                case 32000:
                case 44100:
                case 48000:
                    SRate = rate;
                    return true;
                default:
                    return false;
            }
        }

        public bool Setting(string filePath)
        {
            NSUrl url;
            try
            {
                url = new NSUrl(filePath);
            }
            catch(Exception)
            {
                return false;
            }
            NSError err;

            var settings = new AudioSettings()
            {
                Format = AudioToolbox.AudioFormatType.AppleLossless,
                AudioQuality = AVAudioQuality.High,
                EncoderBitRate = BitsPerSec, //bits per sec
                SampleRate = SRate,
                NumberChannels = 1,
            };

            try
            {
                recorder = AVAudioRecorder.Create(url, settings, out err);
            }
            catch(Exception)
            {
                return false;
            }
            recorder.PrepareToRecord();

            return true;
        }

        public void Stop()
        {
            if (recorder != null)
                if (recorder.Recording)
                    recorder.Stop();
        }
    }
}
