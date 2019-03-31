using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

using GigaHitz.iOS.Api;
using CarouselView.FormsPlugin;

namespace GigaHitz.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            CarouselView.FormsPlugin.iOS.CarouselViewRenderer buf_1;

            global::Xamarin.Forms.Forms.Init();
            global::CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();

            ObjCRuntime.Class.ThrowOnInitFailure = false;

            buf_1 = null;

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
