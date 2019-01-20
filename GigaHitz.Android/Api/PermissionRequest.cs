using Android.OS;
using Android;
using Android.App;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Xamarin.Forms;
using GigaHitz.PermissionApi;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: Dependency(typeof(GigaHitz.Droid.Api.PermissionRequest))]
namespace GigaHitz.Droid.Api
{
    public class PermissionRequest : IPermission
    {
        protected static Activity _activity;

        object locker = new object();
        const int permissioncode = 25;

        TaskCompletionSource<Dictionary<Permission, PermissionStatus>> tcs;
        Dictionary<Permission, PermissionStatus> results;

        public static void Init(Activity activity)
        {
            _activity = activity;
        }

        public Task<PermissionStatus> CheckPermissionAsync(Permission permission)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var names = GetManifestNames(permission);

                foreach (var name in names)
                {
                    if (_activity.CheckSelfPermission(name) != Android.Content.PM.Permission.Granted)
                        return Task.FromResult(PermissionStatus.Denied);
                }
            }
            return Task.FromResult(PermissionStatus.Granted);
        }

        public async Task<Dictionary<Permission, PermissionStatus>> RequestPermissionsAsync(params Permission[] permissions)
        {
            if (tcs != null && !tcs.Task.IsCompleted)
            {
                tcs.SetCanceled();
                tcs = null;
            }
            lock (locker)
            {
                results = new Dictionary<Permission, PermissionStatus>();
            }

            var permissionsToRequest = new List<string>();
            foreach (var permission in permissions)
            {
                var result = await CheckPermissionAsync(permission);
                if (result != PermissionStatus.Granted)
                {
                    var names = GetManifestNames(permission);
                    //check to see if we can find manifest names
                    //if we can't add as unknown and continue
                    if ((names?.Count ?? 0) == 0)
                    {
                        lock (locker)
                        {
                            if (!results.ContainsKey(permission))
                                results.Add(permission, PermissionStatus.Unknown);
                        }
                        continue;
                    }

                    permissionsToRequest.AddRange(names);
                }
                else
                {
                    //if we are granted you are good!
                    lock (locker)
                    {
                        if (!results.ContainsKey(permission))
                            results.Add(permission, PermissionStatus.Granted);
                    }
                }
            }

            if (permissionsToRequest.Count == 0)
                return results;

            tcs = new TaskCompletionSource<Dictionary<Permission, PermissionStatus>>();

            ActivityCompat.RequestPermissions(_activity, permissionsToRequest.ToArray(), permissioncode);

            return await tcs.Task;
        }

        List<string> GetManifestNames(Permission permission)
        {
            var permissionNames = new List<string>();
            switch (permission)
            {
                case Permission.Microphone:
                    {
                        permissionNames.Add(Manifest.Permission.RecordAudio);
                    }
                    break;
                case Permission.Storage:
                    {
                        permissionNames.Add(Manifest.Permission.ReadExternalStorage);
                        permissionNames.Add(Manifest.Permission.WriteExternalStorage);
                    }
                    break;
                default:
                    return null;
            }
            return permissionNames;
        }
    }
}
