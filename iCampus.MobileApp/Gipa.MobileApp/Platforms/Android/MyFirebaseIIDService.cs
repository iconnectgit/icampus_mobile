using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;
using Firebase.Messaging;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Helpers;
using Newtonsoft.Json;

namespace Gipa.MobileApp;

[Service(Enabled = true, Exported = true)]
[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
public class MyFirebaseIIDService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnNewToken(string token)
        {
            iCampus.MobileApp.App.RefreshedToken = token;
            iCampus.MobileApp.App.IsTokenUpdated = false;
            Log.Debug(TAG, "Refreshed token: " + iCampus.MobileApp.App.RefreshedToken);
            if (!string.IsNullOrEmpty(iCampus.MobileApp.App.RefreshedToken))
            {
                HelperMethods.SendRegistrationToServer(iCampus.MobileApp.App.RefreshedToken);
            }
        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            try
            {
                SendNotification(message.GetNotification().Body, message.Data, message.GetNotification().Title);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private void SendNotification(string messageBody, IDictionary<string, string> data, string title)
        {
            try
            {
                var intent = new Intent(this, typeof(MainActivity)); intent.AddFlags(ActivityFlags.ClearTop);
                NotificationData notificationData = new NotificationData();
                int notification_ID = Guid.NewGuid().GetHashCode();
                foreach (var key in data.Keys)
                {
                    switch (key)
                    {
                        case "notificationType":
                            notificationData.notificationType = data[key];
                            break;
                        case "primaryKey":
                            notificationData.primaryKey = data[key];
                            break;
                        case "userRefId":
                            notificationData.userRefId = data[key];
                            break;
                        case "notificationModuleName":
                            notificationData.notificationModuleName = data[key];
                            break;
                        case "notificationSubType":
                            notificationData.notificationSubType = data[key];
                            break;
                    }
                }
                intent.PutExtra("notification_data", JsonConvert.SerializeObject(notificationData));

                var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.P)
                  ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
                    : PendingIntentFlags.UpdateCurrent;

                var pendingActivityIntent = PendingIntent.GetActivity(this, MainActivity.NOTIFICATION_ID, intent, pendingIntentFlags);
                
                var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID).
                    SetSmallIcon(Resource.Drawable.gipa_icon_logo).SetContentTitle(title)
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                    .SetContentText(messageBody).SetAutoCancel(true).SetContentIntent(pendingActivityIntent);
                var notificationManager = NotificationManagerCompat.From(this);
                notificationManager.Notify(notification_ID, notificationBuilder.Build());
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }
    }