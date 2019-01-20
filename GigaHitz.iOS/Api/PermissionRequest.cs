using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLocation;
using AVFoundation;
using Foundation;
using CoreMotion;
using UIKit;
using System.Diagnostics;
using GigaHitz.PermissionApi;
using Xamarin.Forms;

[assembly: Dependency(typeof(GigaHitz.iOS.Api.PermissionRequest))]
namespace GigaHitz.iOS.Api
{
    public class PermissionRequest : IPermission
    {
        public Task<PermissionStatus> CheckPermissionAsync(Permission permission)
        {
            switch(permission)
            {
                case Permission.Microphone:
                    return Task.FromResult(GetRecordPermissionStatus(AVMediaType.Audio));
            }
            return Task.FromResult(PermissionStatus.Granted);
        }

        public Task<Dictionary<Permission, PermissionStatus>> RequestPermissionsAsync(params Permission[] permissions)
        {
            var results = new Dictionary<Permission, PermissionStatus>();

            foreach (var permission in permissions)
            {
                if (results.ContainsKey(permission))
                    continue;
                switch (permission)
                {
                    case Permission.Microphone:
                        try
                        {
                            AVAudioSession.SharedInstance().RequestRecordPermission((bool granted) =>
                                results.Add(permission, (granted ? PermissionStatus.Granted : PermissionStatus.Denied))
                            );
                        }
                        catch (Exception e)
                        {
                            results.Add(permission, PermissionStatus.Unknown);
                        }
                        break;
                }
                if (!results.ContainsKey(permission))
                    results.Add(permission, PermissionStatus.Granted);
            }

            return Task.FromResult(results);
        }

        #region AV: Camera and Microphone
        PermissionStatus GetRecordPermissionStatus(NSString mediaType)
        {
            var status = AVAudioSession.SharedInstance().RecordPermission;
            switch (status)
            {
                case AVAudioSessionRecordPermission.Granted:
                    return PermissionStatus.Granted;
                case AVAudioSessionRecordPermission.Denied:
                    return PermissionStatus.Denied;
                default:
                    return PermissionStatus.Unknown;
            }
        }
        #endregion
    }
}
