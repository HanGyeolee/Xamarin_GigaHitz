using System;
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

        public Task<bool> Share(string filePath)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("audio/aac");
            try
            {
                intent.SetPackage("com.kakao.talk");
            }
            catch (Exception)
            {
                return Task.FromResult<bool>(false);
            }

            var intentChooser = Intent.CreateChooser(intent, "공유");
            instance.StartActivity(intentChooser);

            return Task.FromResult<bool>(true);
        }

        public static void Init(Activity activity)
        {
            instance = activity;
        }
    }
}
