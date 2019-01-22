using System;
using Xamarin.Forms;
using AVFoundation;
using Foundation;

[assembly: Dependency(typeof(GigaHitz.iOS.AudioRecorder_iOS))]
namespace GigaHitz.iOS
{
    public class AudioRecorder_iOS : Interfaces.IAudioRecorder
    {
        AVAudioRecorder recorder;

        public void Recording()
        {
            if(recorder != null)
                recorder.Record();
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

        public bool Setting(string filePath)
        {
            NSUrl url;
            try
            {
                url = new NSUrl(filePath);
            }
            catch(Exception e)
            {
                return false;
            }
            NSError err;

            var settings = new AudioSettings()
            {
                Format = AudioToolbox.AudioFormatType.MPEG4AAC,
                AudioQuality = AVAudioQuality.High,
                EncoderBitRate = (int)(256 * 1024 * 8),
                SampleRate = 48000f,
                NumberChannels = 1,
            };

            recorder = AVAudioRecorder.Create(url, settings, out err);

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
