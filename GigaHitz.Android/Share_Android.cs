using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.App;
using Xamarin.Forms;

[assembly: Dependency(typeof(GigaHitz.Droid.Share_Android))]
namespace GigaHitz.Droid
{
    public class Share_Android : Interfaces.IShare
    {
        private static Activity instance;

        public Task<bool> Share(string filePath, string fileName)
        {
            var file = new Java.IO.File(filePath);
            var uri = Android.Net.Uri.FromFile(file);

            var intent = new Intent(Intent.ActionSend);
            intent.SetType("audio/*");
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            intent.PutExtra(Android.Content.Intent.ExtraStream, uri);

            //intent.SetPackage("com.kakao.talk");
            intent.PutExtra(Android.Content.Intent.ExtraTitle, fileName);
            intent.PutExtra(Android.Content.Intent.ExtraSubject, fileName);
            intent.PutExtra(Android.Content.Intent.ExtraText, fileName + " 은 기가히츠 (GigaHitz) - 녹음, 피아노, Record, Piano 어플에서 녹음되었습니다.");
            try
            {
                var intentChooser = Intent.CreateChooser(intent, "공유");
                intentChooser.SetFlags(ActivityFlags.ClearTop);
                intentChooser.SetFlags(ActivityFlags.NewTask);
                instance.StartActivity(intentChooser);
            }
            catch (Exception e)
            {
                Console.Write("Exception :" + e);
                return Task.FromResult<bool>(false);
            }
            return Task.FromResult<bool>(true);
        }

        public static void Init(Activity activity)
        {
            instance = activity;
        }
    }
}