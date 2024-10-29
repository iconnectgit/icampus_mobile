using System.Net;
using Foundation;
using iCampus.MobileApp.Helpers;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using UIKit;

namespace iCampus.MobileApp.DependencyService;

public class iOSNativeServices : INativeServices
    {
        public void ChangeStatusBarColor(int r, int g, int b)
        {
            try
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    if (UIApplication.SharedApplication.KeyWindow != null)
                    {
                        CoreGraphics.CGRect statusFrame = UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame;
                        UIView statusBar = new UIView(statusFrame);
                        statusBar.BackgroundColor = Color.FromRgb(r, g, b).ToUIColor();
                        UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);
                    }
                }
                else
                {
                    UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
                    statusBar.BackgroundColor = Color.FromRgb(r, g, b).ToUIColor();
                    statusBar.SetValueForKey(Colors.White.ToUIColor(), new NSString("foregroundColor"));
                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        public async void DownloadAndPreviewFile(string filePath, string fileId)
        {
            try
            {
                double diff = 0;
                var externalFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "iCampusV2");
                if (!Directory.Exists(externalFolderPath))
                {
                    Directory.CreateDirectory(externalFolderPath);
                }

                string fileName = Path.GetFileName(filePath);

                if (!string.IsNullOrEmpty(fileId))
                {
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                    fileName = fileName.Replace(fileNameWithoutExt, fileNameWithoutExt + "_" + fileId);
                }

                string uniqueFileName = $"{filePath.GetHashCode()}_{fileName}";
                fileName = Uri.UnescapeDataString(uniqueFileName);

                var fileDevicePath = Path.Combine(externalFolderPath, fileName);
                if (File.Exists(fileDevicePath))
                    diff = DateTime.Now.Subtract(new FileInfo(fileDevicePath).CreationTime).TotalMinutes;

                if (!File.Exists(fileDevicePath) || diff > 15)
                {
                    try
                    {
                        await ApiHelper.ShowProcessingIndicatorPopup();
                        using (var webClient = new WebClient())
                        {
                            Byte[] bytes = webClient.DownloadData(filePath);
                            File.WriteAllBytes(fileDevicePath, bytes);
                        }

                        await ApiHelper.HideProcessingIndicatorPopup();
                    }
                    catch (Exception ex)
                    {
                        await ApiHelper.HideProcessingIndicatorPopup();
                        // Handle exception or notify user
                    }
                }

                var previewController = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(fileDevicePath));
                previewController.Delegate =
                    new UIDocumentInteractionControllerDelegateClass(UIApplication.SharedApplication.KeyWindow
                        .RootViewController);

                Device.BeginInvokeOnMainThread(() => { previewController.PresentPreview(true); });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void GetDeviceID(Action<string> result)
        {
            var deviceId = UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            result(deviceId);
        }

        public void KillProcess()
        {
            // Implementation for iOS is generally not allowed for security reasons
        }

        public void SetToolBarColor(string color)
        {
            // iOS does not provide a way to set the toolbar color directly
        }

        public void ShowAlert(string title, string message)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        public void ShowAlertWithTwoButtons(string title, string message, string yesBtn, string noBtn, Action<bool> result)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create(yesBtn, UIAlertActionStyle.Default, _ => result(true)));
            alert.AddAction(UIAlertAction.Create(noBtn, UIAlertActionStyle.Cancel, _ => result(false)));
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        public void CheckLocationPermission(Action<bool> result)
        {
            // iOS does not have a direct way to check location permission status
            result(true);
        }

        public void NotificationToggled(Action<bool> result = null)
        {
            var settingsUrl = new NSUrl(UIApplication.OpenSettingsUrlString);
            if (UIApplication.SharedApplication.CanOpenUrl(settingsUrl))
            {
                UIApplication.SharedApplication.OpenUrl(settingsUrl);
                // You can check notification settings here if needed
                result?.Invoke(true);
            }
            else
            {
                result?.Invoke(false);
            }
        }

        public void CheckNotificationSetting()
        {
            // iOS does not provide a direct API for checking notification settings
        }

        public bool SystemVersionCheck()
        {
            return UIDevice.CurrentDevice.CheckSystemVersion(15, 0);
        }
    }
