using System;
using Xamarin.Forms;

using Android.Media;

[assembly: Dependency(typeof(GigaHitz.Droid.AudioRecorder_Android))]
namespace GigaHitz.Droid
{
    public class AudioRecorder_Android : Interfaces.IAudioRecorder
    {
        MediaRecorder recorder;
        static int BitsPerSec = 256 * 1024;
        static int SRate = 44100;

        public void Recording()
        {
            if(recorder != null)
            {
                recorder.Start();
            }
        }
       
        public void Release()
        {
            if (recorder != null)
            {
                recorder.Reset();
                recorder = null;
            }
        }

        public void Reset()
        {
            if (recorder != null)
            {
                recorder.Reset();
            }
        }

        public bool SetBitRate(int kbps)
        {
            switch(kbps)
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
                    BitsPerSec = 256 * 1024;
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
                    SRate = tmp;
                    return true;
                default:
                    SRate = 44100;
                    return false;
            }
        }

        public bool Setting(string filePath)
        {
            // mediorecoder
            recorder = new MediaRecorder();

            try
            {
                recorder.SetAudioSource(AudioSource.Mic);
                recorder.SetOutputFormat(OutputFormat.Mpeg4);
                recorder.SetAudioEncoder(AudioEncoder.Aac);
                recorder.SetAudioEncodingBitRate(BitsPerSec); //bits per sec
                recorder.SetAudioSamplingRate(SRate);
                recorder.SetAudioChannels(1);
                recorder.SetOutputFile(filePath);
            }
            catch
            {
                recorder = null;
                return false;
            }

            recorder.Prepare();
            return true;
        }

        public void Stop()
        {
            if (recorder != null)
                recorder.Stop();
        }
    }
}
