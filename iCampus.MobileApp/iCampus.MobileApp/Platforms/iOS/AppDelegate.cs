using System;
using System.Threading.Tasks;
using Firebase.CloudMessaging;
using Foundation;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Helpers;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using UIKit;
using UserNotifications;

namespace iCampus.MobileApp;

[Foundation.Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate, IMessagingDelegate, IUNUserNotificationCenterDelegate
{
    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        NSDictionary dict = new NSDictionary("FIRAnalyticsDebugEnabled", true);
        NSUserDefaults.StandardUserDefaults.RegisterDefaults(dict);
        Firebase.Core.App.Configure();     

        if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
        {
            // iOS 10 or later
            var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge |
                              UNAuthorizationOptions.Sound;
            UNUserNotificationCenter.Current.RequestAuthorization(authOptions,
                (granted, error) => { Console.WriteLine(granted); });

            // For iOS 10 display notification (sent via APNS)
            UNUserNotificationCenter.Current.Delegate = this;

            // For iOS 10 data message (sent via FCM)
            Messaging.SharedInstance.Delegate = this;
        }
        else
        {
            // iOS 9 or before
            //var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
            //var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
            //UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var userNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge |
                                            UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(userNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
            else
            {
                var notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge |
                                        UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

        UIApplication.SharedApplication.RegisterForRemoteNotifications();
        App.DeviceID = UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        //AppSettings.Current.IsPushNotificationEnable = UIApplication.SharedApplication.CurrentUserNotificationSettings.Types != UIUserNotificationType.None;
        RequestPushNotificationPermission();
        CheckPushNotificationSettingsAsync();
        return base.FinishedLaunching(app, options);
    }
    [Foundation.Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
    public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
    {
        completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound);
    }
    [Foundation.Export("messaging:didReceiveRegistrationToken:")]
    public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
    {
        Console.WriteLine($"Firebase registration token: {fcmToken}");
        App.RefreshedToken = fcmToken;
        App.IsTokenUpdated = false;

        HelperMethods.SendRegistrationToServer(App.RefreshedToken);
    }
    [Foundation.Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
    public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
    {
        var userInfo = response.Notification.Request.Content.UserInfo;
        SetNotificationData(userInfo); // Your custom method to handle notification data

        completionHandler();
    }
    // public override void DidReceiveRemoteNotification (UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
    // {
    //     SetNotificationData(userInfo);
    //     completionHandler(UIBackgroundFetchResult.NewData);
    // }
    public override void WillEnterForeground(UIApplication uiApplication)
    {
        base.WillEnterForeground(uiApplication);
        AppSettings.Current.IsPushNotificationEnable = UIApplication.SharedApplication.CurrentUserNotificationSettings.Types != UIUserNotificationType.None;
    }
    private void SetNotificationData(NSDictionary userInfo)
    {
        NotificationData notificationData = new NotificationData();
        if (userInfo != null)
        {
            foreach (var data in userInfo)
            {
                if (userInfo.ContainsKey((NSString)"notificationType"))
                    notificationData.notificationType = userInfo.ValueForKey((NSString)"notificationType").ToString();
                if (userInfo.ContainsKey((NSString)"primaryKey"))
                    notificationData.primaryKey = userInfo.ValueForKey((NSString)"primaryKey").ToString();
                if (userInfo.ContainsKey((NSString)"userRefId"))
                    notificationData.userRefId = userInfo.ValueForKey((NSString)"userRefId").ToString();
                if (userInfo.ContainsKey((NSString)"notificationModuleName"))
                    notificationData.notificationModuleName = userInfo.ValueForKey((NSString)"notificationModuleName").ToString();
                if (userInfo.ContainsKey((NSString)"notificationSubType"))
                    notificationData.notificationSubType = userInfo.ValueForKey((NSString)"notificationSubType").ToString();

            }
            App.NotificationValues = notificationData;
        }
    }
    public async Task CheckPushNotificationSettingsAsync()
    {
        var notificationCenter = UNUserNotificationCenter.Current;
    
        // Request the current notification settings
        var settings = await notificationCenter.GetNotificationSettingsAsync();
    
        // Check if notifications are enabled
        AppSettings.Current.IsPushNotificationEnable = 
            settings.AuthorizationStatus == UNAuthorizationStatus.Authorized;
    }
    private void RequestPushNotificationPermission()
    {
        // Request permission to show notifications
        UNUserNotificationCenter.Current.RequestAuthorization(
            UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
            (granted, error) =>
            {
                if (granted)
                {
                    Console.WriteLine("Notification permission granted.");

                    // Register for remote notifications
                    InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                }
                else
                {
                    Console.WriteLine("Notification permission denied.");
                }

                if (error != null)
                {
                    Console.WriteLine($"Error requesting notification permission: {error.LocalizedDescription}");
                }
            });
    }

}