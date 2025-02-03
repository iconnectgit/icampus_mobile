using Foundation;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Helpers;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using UIKit;
using UserNotifications;

namespace Madar.MobileApp.DependencyService;

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
                var filePreviewer = new FilePreviewerImplementation();
                await filePreviewer.PreviewFile(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DownloadAndPreviewFile: {ex.Message}");
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
            try
            {
                NSUrl settingsUrl = new NSUrl(UIApplication.OpenSettingsUrlString);

                if (UIApplication.SharedApplication.CanOpenUrl(settingsUrl))
                {
                    UIApplication.SharedApplication.OpenUrl(settingsUrl, new NSDictionary(), (success) =>
                    {
                        UNUserNotificationCenter.Current.GetNotificationSettings((settings) =>
                        {
                            bool isEnabled = settings.AuthorizationStatus == UNAuthorizationStatus.Authorized || 
                                             settings.AuthorizationStatus == UNAuthorizationStatus.Provisional;

                            AppSettings.Current.IsPushNotificationEnable = isEnabled;

                            result?.Invoke(true);
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex);
                result?.Invoke(false);
            }
        }

        public async Task CheckNotificationSetting()
        {
            var notificationCenter = UNUserNotificationCenter.Current;
            var settings = await notificationCenter.GetNotificationSettingsAsync();
            AppSettings.Current.IsPushNotificationEnable = settings.AuthorizationStatus == UNAuthorizationStatus.Authorized;
            
            //AppSettings.Current.IsPushNotificationEnable = (UIApplication.SharedApplication.CurrentUserNotificationSettings.Types != UIUserNotificationType.None);

            // iOS does not provide a direct API for checking notification settings
        }

        public bool SystemVersionCheck()
        {
            return UIDevice.CurrentDevice.CheckSystemVersion(15, 0);
        }
        private class DocumentInteractionControllerDelegate : UIDocumentInteractionControllerDelegate
        {
            UIViewController _viewController;

            public DocumentInteractionControllerDelegate(UIViewController viewController)
            {
                _viewController = viewController;
            }

            public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
            {
                return _viewController;
            }
        }
        public Task<string> GetDownloadFolderPathAsync()
        {
            return Task.FromResult(string.Empty);
        }
    }