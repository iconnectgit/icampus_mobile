using System.Diagnostics;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using iCampus.MobileApp.Forms;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Activity = Android.App.Activity;
using NotificationManagerCompat = AndroidX.Core.App.NotificationManagerCompat;
using Process = Java.Lang.Process;

namespace iCampus.MobileApp.DependencyService;

public class AndroidNativeServices : INativeServices
{
        public void ChangeStatusBarColor(int r, int g, int b)
        {
            var activity = (Activity)Platform.CurrentActivity;

            if (activity != null)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
                {
                    var window = activity.Window;
                    //Android.Views.Window? window = activity.Window;
                    window?.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    window?.SetStatusBarColor(Android.Graphics.Color.Rgb(r, g, b));
                }
            }
        }
        
        public async void DownloadAndPreviewFile(string filePath, string fileId)
        {
            // Implementation remains the same
        }

        public void KillProcess()
        {
            //MainActivity.Instance.FinishAffinity();
            //Process.KillProcess(Process.MyPid());
        }

        public void ShowAlert(string title, string message)
        {
            //Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(MainActivity.Instance);
            // AlertDialog alert = dialog.Create();
            // alert.SetTitle(title);
            // alert.SetMessage(message);
            // alert.SetButton("OK", (c, ev) => { });
            // alert.Show();
            //alert.Window.SetLayout(MainActivity.screenWidth, alert.Window.Attributes.Height);
        }

        public void ShowAlertWithTwoButtons(string title, string message, string yesBtn, string noBtn, Action<bool> result)
        {
            //Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(MainActivity.Instance);
            //AlertDialog alert = dialog.Create();
            // alert.SetTitle(title);
            // alert.SetMessage(message);
            // alert.SetButton(yesBtn, (c, ev) => result(true));
            // alert.SetButton2(noBtn, (c, ev) => result(false));
            // alert.Show();
            //alert.Window.SetLayout(MainActivity.screenWidth, alert.Window.Attributes.Height);
        }

        public void GetDeviceID(Action<string> result)
        {
            var deviceId = Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            result(deviceId);
        }

        public void SetToolBarColor(string color)
        {
            //var activity = MainActivity.Instance;
            //var toolbar = activity.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.TopToolbar);
            //toolbar?.SetBackgroundColor(Android.Graphics.Color.ParseColor(color));
        }

        public void CheckLocationPermission(Action<bool> result)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                //result(MainActivity.Instance.CheckSelfPermission(Android.Manifest.Permission.AccessCoarseLocation) == Permission.Granted);
            }
            else
            {
                result(true);
            }
        }

        public void NotificationToggled(Action<bool> result = null)
        {
            try
            {
                // Intent intent = new Intent();
                //
                // if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                // {
                //     intent.SetAction(Android.Provider.Settings.ActionAppNotificationSettings);
                //     intent.PutExtra(Android.Provider.Settings.ExtraAppPackage, MainActivity.Instance.PackageName);
                // }
                // else
                // {
                //     intent.SetAction("android.settings.APP_NOTIFICATION_SETTINGS");
                //     intent.PutExtra("app_package", MainActivity.Instance.PackageName);
                //     intent.PutExtra("app_uid", MainActivity.Instance.PackageManager.GetApplicationInfo(MainActivity.Instance.PackageName, PackageInfoFlags.MetaData).Uid);
                // }
                //
                // MainActivity.Instance.StartActivityForResult(intent, 12);
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }

        public void CheckNotificationSetting()
        {
            //AppSettings.Current.IsPushNotificationEnable = NotificationManagerCompat.From(MainActivity.Instance).AreNotificationsEnabled();
        }
        
        

        public bool SystemVersionCheck()
        {
            throw new NotImplementedException();
        }
    }