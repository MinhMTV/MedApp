
using Foundation;
using System.Linq;
using UIKit;

namespace CBMTraining.iOS
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
            Rg.Plugins.Popup.Popup.Init();
            NativeMedia.Platform.Init(GetTopViewController);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            // We have checked to see if the device is running iOS 8, if so we are required to ask for the user's permission to receive notifications    
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
                );

                app.RegisterUserNotificationSettings(notificationSettings);
            }


            // check for a notification    
            if (options != null)
            {
                if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
                {
                    UILocalNotification localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
                    if (localNotification != null)
                    {
                        new UIAlertView(localNotification.AlertAction, localNotification.AlertBody, null, "OK", null).Show();
                        // reset our badge    
                        UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
                    }
                }
            }


            return base.FinishedLaunching(app, options);
        }

        public UIViewController GetTopViewController()
        {
            var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (vc is UINavigationController navController)
                vc = navController.ViewControllers.Last();

            return vc;
        }



        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {

            UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
            okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }
    }
}
