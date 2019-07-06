using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

using Foundation;
using CoreGraphics;
using UIKit;

using Xamarin.Forms;

[assembly: Dependency(typeof(GigaHitz.iOS.Share_iOS))]
namespace GigaHitz.iOS
{
    public class Share_iOS : Interfaces.IShare
    {
        public Task<bool> Share(string filePath, string fileName)
        {
            var item = new NSUrl(filePath);

            var activityItems = new[] { item };
            
            var activityController = new UIActivityViewController(activityItems, null);

            #region exclude
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                activityController.ExcludedActivityTypes = new NSString[]
                {
                    UIActivityType.AddToReadingList,
                    UIActivityType.AirDrop,
                    UIActivityType.AssignToContact,
                    UIActivityType.Message,
                    UIActivityType.PostToFacebook,
                    UIActivityType.PostToFlickr,
                    UIActivityType.PostToTencentWeibo,
                    UIActivityType.PostToTwitter,
                    UIActivityType.PostToVimeo,
                    UIActivityType.PostToWeibo,
                    UIActivityType.Print,
                    UIActivityType.SaveToCameraRoll
                };
            else if (UIDevice.CurrentDevice.CheckSystemVersion(10, 9))
                activityController.ExcludedActivityTypes = new NSString[]
                {
                    UIActivityType.AddToReadingList,
                    UIActivityType.AirDrop,
                    UIActivityType.AssignToContact,
                    UIActivityType.Message,
                    UIActivityType.OpenInIBooks,
                    UIActivityType.PostToFacebook,
                    UIActivityType.PostToFlickr,
                    UIActivityType.PostToTencentWeibo,
                    UIActivityType.PostToTwitter,
                    UIActivityType.PostToVimeo,
                    UIActivityType.PostToWeibo,
                    UIActivityType.Print,
                    UIActivityType.SaveToCameraRoll
                };
            else
                activityController.ExcludedActivityTypes = new NSString[]
                {
                    UIActivityType.AddToReadingList,
                    UIActivityType.AirDrop,
                    UIActivityType.AssignToContact,
                    UIActivityType.MarkupAsPdf,
                    UIActivityType.Message,
                    UIActivityType.OpenInIBooks,
                    UIActivityType.PostToFacebook,
                    UIActivityType.PostToFlickr,
                    UIActivityType.PostToTencentWeibo,
                    UIActivityType.PostToTwitter,
                    UIActivityType.PostToVimeo,
                    UIActivityType.PostToWeibo,
                    UIActivityType.Print,
                    UIActivityType.SaveToCameraRoll
                };
            #endregion

            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }

            topController.PresentViewController(activityController, true, () => { });

            return Task.FromResult<bool>(true);
        }
    }
}
