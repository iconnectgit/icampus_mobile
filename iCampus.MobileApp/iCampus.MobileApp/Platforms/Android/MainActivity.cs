using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Tasks;
using Android.OS;
using Android.Widget;
using AndroidX.Core.App;
using AutoMapper;
using Firebase;
using Firebase.Messaging;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Helpers;
using Newtonsoft.Json;

namespace iCampus.MobileApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity, IOnSuccessListener
{
    public static MainActivity Instance = null;
    internal static readonly int NOTIFICATION_ID = 100;
    internal static readonly string CHANNEL_ID = "my_notification_channel";
    protected IMapper _mapper;
    protected INativeServices _nativeServices;
    protected INavigation Navigation;
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        try
        {
            IsPlayServicesAvailable();
            CreateNotificationChannel();
            var firebaseApp = FirebaseApp.InitializeApp(this);
            if (firebaseApp == null)
            {
                Console.WriteLine("Firebase initialization failed.");
                return; // Stop further processing as Firebase is not available
            }
            FirebaseMessaging.Instance.GetToken().AddOnSuccessListener(this);
            _mapper = MauiApplication.Current.Services.GetService<IMapper>();
            _nativeServices = MauiApplication.Current.Services.GetService<INativeServices>();
            Navigation = MauiApplication.Current.Services.GetService<INavigation>();

        }
        catch(Exception ex)
        {
            //Crashes.TrackError(ex);
        }
        if(Intent.Extras != null)
            HandlePushNotification(Intent);
        AppSettings.Current.IsPushNotificationEnable = NotificationManagerCompat.From(this).AreNotificationsEnabled();
        Instance = this; 
    }
    public void OnSuccess(Java.Lang.Object result)
    {
        if (result is Java.Lang.String tokenString)
        {
            // Handle the token when it's available
            var fcmToken = tokenString.ToString();
            App.RefreshedToken = fcmToken;
            Console.WriteLine("FCM Token: " + fcmToken);
        }
        else
        {
            Console.WriteLine("FCM Token not available.");
            if (result is Java.Lang.Object tokenObject)
            {
                App.RefreshedToken = tokenObject.Class.GetMethod("getToken").Invoke(tokenObject).ToString();
            }
        }
    }
    protected override void OnNewIntent(Intent intent)
    {
        HandlePushNotification(intent);
        ViewModelBase viewModelBase = new (_mapper, _nativeServices, Navigation);
        viewModelBase.PushNotificationClick(App.NotificationValues);
        base.OnNewIntent(intent);
    }
    private void HandlePushNotification(Intent intent)
    {
        NotificationData notificationData = new NotificationData();
        if (intent.Extras != null)
        {
            foreach (var key in intent.Extras.KeySet())
            {
                string value = intent.Extras.GetString(key);
                switch (key)
                {
                    case "notification_data":
                        notificationData = JsonConvert.DeserializeObject<NotificationData>(value);
                        break;
                    case "notificationType":
                        notificationData.notificationType = value;
                        break;
                    case "primaryKey":
                        notificationData.primaryKey = value;
                        break;
                    case "userRefId":
                        notificationData.userRefId = value;
                        break;
                    case "notificationModuleName":
                        notificationData.notificationModuleName = value;
                        break;
                    case "notificationSubType":
                        notificationData.notificationSubType = value;
                        break;
                }
            }
            App.NotificationValues = notificationData;
        }
    }

    protected override void OnResume()
    {
        base.OnResume();
        AppSettings.Current.IsPushNotificationEnable = NotificationManagerCompat.From(this).AreNotificationsEnabled();
        IsPlayServicesAvailable();
    }
    public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (!GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Toast.MakeText(this, "This device is not supported",ToastLength.Short).Show();
                    Finish();
                }
                return false;
            }
            else
            {
                //Google Play Services is available
                return true;
            }
        }
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return;
            }

            var channel = new NotificationChannel(MainActivity.CHANNEL_ID, "FCM Notifications", NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                if (requestCode == 12)
                {
                    AppSettings.Current.IsPushNotificationEnable = NotificationManagerCompat.From(this).AreNotificationsEnabled();
                    HelperMethods.UpdatePushNotificationSetting(AppSettings.Current.IsPushNotificationEnable, null, _nativeServices);
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }


}