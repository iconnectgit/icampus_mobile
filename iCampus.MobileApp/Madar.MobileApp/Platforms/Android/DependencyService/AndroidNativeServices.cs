using System.Diagnostics;
using System.Net;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Helpers;
using Microsoft.Maui.Controls.PlatformConfiguration;
using NotificationManagerCompat = AndroidX.Core.App.NotificationManagerCompat;
using Process = Java.Lang.Process;
using Android.OS;
using CommunityToolkit.Maui.Alerts;
using iCampus.MobileApp.DependencyService;
using Toast = Android.Widget.Toast;
namespace Madar.MobileApp.DependencyService;

public class AndroidNativeServices : INativeServices
{
        public void ChangeStatusBarColor(int r, int g, int b)
        {
            MainActivity activity = MainActivity.Instance;
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
            double diff = 0;
            await ApiHelper.ShowProcessingIndicatorPopup();
            
            var externalFolderPath = Path.Combine(FileSystem.CacheDirectory, "iCampusV2");
            if (!Directory.Exists(externalFolderPath))
            {
                Directory.CreateDirectory(externalFolderPath);
            }

            string fileName = Path.GetFileName(filePath);

            if (!string.IsNullOrEmpty(fileId))
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                fileName = fileName.Replace(fileNameWithoutExt, $"{fileNameWithoutExt}_{fileId}");
            }

            string uniqueFileName = $"{filePath.GetHashCode()}_{fileName}";
            fileName = Uri.UnescapeDataString(uniqueFileName);
            var cacheDir = Android.App.Application.Context.CacheDir;
            var fileDevicePath = Path.Combine(cacheDir.AbsolutePath, "iCampusV2", fileName);

            if (File.Exists(fileDevicePath))
                diff = DateTime.Now.Subtract(new FileInfo(fileDevicePath).CreationTime).TotalMinutes;

            if (!File.Exists(fileDevicePath) || diff > 15)
            {
                try
                {
                    WebClient webClient = new WebClient();
                    byte[] bytes = await webClient.DownloadDataTaskAsync(filePath);
                    File.WriteAllBytes(fileDevicePath, bytes);
                }
                catch (Exception)
                {
                    await ApiHelper.HideProcessingIndicatorPopup();
                    Toast.MakeText(Android.App.Application.Context, "File not exist", ToastLength.Short).Show();
                    return;
                }
            }

            
            var file = new Java.IO.File(fileDevicePath);
            file.SetReadable(true);

            if (!string.IsNullOrEmpty(fileDevicePath))
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var uri = AndroidX.Core.Content.FileProvider.GetUriForFile(
                        Android.App.Application.Context,
                        $"{Android.App.Application.Context.PackageName}.provider",
                        file);
                    // var uri = FileProvider.GetUriForFile(
                    //     Android.App.Application.Context,
                    //     "com.companyname.icampus.mobileapp.provider",
                    //     file);

                    
                    var mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(MimeTypeMap.GetFileExtensionFromUrl(uri.ToString()));
                    var intent = new Intent(Intent.ActionView);
                    intent.SetDataAndType(uri, mimeType);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.NewTask);


                    // if (intent.ResolveActivity(Android.App.Application.Context.PackageManager) != null)
                    // {
                    //     Application.Context.StartActivity(intent);
                    // }
                    // else
                    // {
                    //     Toast.MakeText(Android.App.Application.Context, "No app available to open this file", ToastLength.Short).Show();
                    // }
                    try
                    {
                        MainActivity.Instance.StartActivity(intent);
                         //Android.App.Application.Context.StartActivity(intent);
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(Android.App.Application.Context, "Unable to find application to perform this action", ToastLength.Short).Show();
                    }
                    finally
                    {
                        Task.Run(ApiHelper.HideProcessingIndicatorPopup);
                    }
                });
            }
        }
        public void KillProcess()
        {
            //MainActivity.Instance.FinishAffinity();
            //Process.KillProcess(Process.MyPid());
        }

        public void ShowAlert(string title, string message)
        {
            // Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(MainActivity.Instance);
            //  AlertDialog alert = dialog.Create();
            //  alert.SetTitle(title);
            //  alert.SetMessage(message);
            //  alert.SetButton("OK", (c, ev) => { });
            //  alert.Show();
            // alert.Window.SetLayout(MainActivity.screenWidth, alert.Window.Attributes.Height);
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
                Intent intent = new Intent();
                
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    intent.SetAction(Android.Provider.Settings.ActionAppNotificationSettings);
                    intent.PutExtra(Android.Provider.Settings.ExtraAppPackage, MainActivity.Instance.PackageName);
                }
                else
                {
                    intent.SetAction("android.settings.APP_NOTIFICATION_SETTINGS");
                    intent.PutExtra("app_package", MainActivity.Instance.PackageName);
                    intent.PutExtra("app_uid", MainActivity.Instance.PackageManager.GetApplicationInfo(MainActivity.Instance.PackageName, PackageInfoFlags.MetaData).Uid);
                }
                
                MainActivity.Instance.StartActivityForResult(intent, 12);
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }

        public async Task CheckNotificationSetting()
        {
            AppSettings.Current.IsPushNotificationEnable = NotificationManagerCompat.From(MainActivity.Instance).AreNotificationsEnabled();
        }
        
        
        public Task<string> GetDownloadFolderPathAsync()
        {
            var folderPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)?.AbsolutePath;

            if (string.IsNullOrEmpty(folderPath))
                throw new InvalidOperationException("Unable to access Downloads directory.");

            return Task.FromResult(folderPath); // Return Task for async consistency
        }
        public bool SystemVersionCheck()
        {
            throw new NotImplementedException();
        }
    }