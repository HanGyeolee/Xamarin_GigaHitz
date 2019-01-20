using System;
using Xamarin.Forms;

using Android.Media;

[assembly: Dependency(typeof(GigaHitz.Droid.AudioRecorder_Android))]
namespace GigaHitz.Droid
{
    public class AudioRecorder_Android : Interfaces.IAudioRecorder
    {
        MediaRecorder recorder;

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

        public bool Setting(string filePath)
        {
            // mediorecoder
            recorder = new MediaRecorder();

            try
            {
                recorder.SetAudioSource(AudioSource.Mic);
                recorder.SetOutputFormat(OutputFormat.Mpeg4);
                recorder.SetAudioEncoder(AudioEncoder.AacEld);
                recorder.SetAudioSamplingRate(44100);
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
                recorder.Reset();
        }
    }
}
