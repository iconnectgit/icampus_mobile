using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using AutoMapper;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Enums;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Forms.UserModules.Attendance;
using iCampus.MobileApp.Forms.UserModules.Calendar;
using iCampus.MobileApp.Forms.UserModules.Registration;
using iCampus.MobileApp.Helpers.CustomCalendar;
using iCampus.MobileApp.Views;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.Portal.ViewModels;
using iCampus.Common.Helpers;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Mopups.Services;
using Application = Microsoft.Maui.Controls.Application;
#if IOS
    using Foundation;
    using Firebase.Crashlytics;
#elif ANDROID
    using Firebase;
    using Firebase.Crashlytics;
#endif
namespace iCampus.MobileApp.Helpers;

public class HelperMethods
    {
        #region Class Level Variables
        private static INativeServices _nativeServices;
        public List<TextResourceView> _resourceList = null;
        #endregion

        public HelperMethods(INativeServices nativeServices)
        {
            _resourceList = new List<TextResourceView>();
            _nativeServices = nativeServices;
        }

        #region Properties
        public List<TextResourceView> ResourceList { get { return _resourceList; } set { _resourceList = value; } }

        #endregion


        #region Core Methods

        public async Task<string> GetResourceText(string resourceCategory, string resourceKey)
        {
            Func<TextResourceView, bool> predicate = r => r.ResourceKey.Equals(resourceKey, StringComparison.OrdinalIgnoreCase);
            IEnumerable<TextResourceView> requiredList = ResourceList.Where(predicate);
            bool refreshList = true;
            string resourceText = string.Empty;

            if (requiredList.Any())
            {
                DateTime? lastRefreshedDateTime = requiredList.FirstOrDefault().LastRefreshedDateTime;
                if (lastRefreshedDateTime.HasValue)
                {
                    if (DateTime.Now.Subtract(lastRefreshedDateTime.Value).TotalMinutes > 30) //TODO: get mins from local resource
                    {
                        refreshList = false;
                    }
                }
            }

            if (refreshList)
            {
                // call api
                var resourceApiUrl = string.Format("api/home/getresourcelist?category={0}", resourceCategory);
                var newResourceList = await ApiHelper.GetObjectList<TextResourceView>(resourceApiUrl);
                ResourceList.RemoveAll(r => r.ResourceCategory.Equals(resourceCategory, StringComparison.OrdinalIgnoreCase));
                ResourceList.AddRange(newResourceList);
            }

            if (ResourceList.Any())
                resourceText = ResourceList.Where(predicate).FirstOrDefault().ResourceText;

            return resourceText;
        }
        
        public static async Task<MobileAppLoginResultView> PerformLogin(string emailId, string password,string deviceId)
        {
           string clientGroupCode = (!string.IsNullOrEmpty(App.ClientGroupCode)) ? App.ClientGroupCode : string.Empty;
            string loginUrl = (clientGroupCode.ToLower().Equals("beam"))?TextResource.BeamMobileAppLoginUrlLive:TextResource.MobileAppLoginUrlLive;
            string loginApiURL = Convert.ToBoolean(TextResource.IsDemoApp) ? TextResource.MobileAppLoginUrlDemo : loginUrl;
            string refreshedToken = string.Empty;
            //bool isLogin = Application.Current.Properties.ContainsKey("IsLogin") ? (bool)Application.Current.Properties["IsLogin"] : false;
            bool isLogin = Preferences.Get("IsLogin", false);
            refreshedToken = string.Empty;
            refreshedToken = "fbRAStrT3YE:APA91bEUdVNj9TVHJt-FphR6kWbaIpp4YqYrARp3DgP7UxetjFblVpFRpJ3n5mfy6ETtBQ9nFN2NgqvsadYNvo8RdAS5YbKBVuAJwZaQocjMuF6M85En9C3GII4YW2uCuIOVRLCMIefq";
            if (!string.IsNullOrEmpty(App.RefreshedToken))
            {
                refreshedToken = App.RefreshedToken;
                await ICCacheManager.SaveSecureObject<string>("RefreshedToken", App.RefreshedToken);
            }
            else
            {
                try
                {
                    refreshedToken = await ICCacheManager.GetSecureObject<string>("RefreshedToken");// "eHDFLUX_1fs:APA91bFCgvBOHCld26HOg2UCDdja2QejvtiPWv4x_qhpzR3px8HseeFxsi_ehqJJtlMTsvNISQWq3naqOj5iI-C7P7_bGUSBNy-DW04-_cj1J5rgBfBenlYdTGSjh3W0LDpwRHeRxiT9";
                }
                catch (Exception ex)
                {
                    //Crashes.TrackError(ex);
                }
            }
            string loginApiFullUrl = string.Empty;
            if (clientGroupCode.ToLower().Equals("beam"))
            {
                loginApiFullUrl = "InitiateLogin?emailId=" + emailId + "&password=" + password + "&token="
                + refreshedToken + "&deviceId=" + deviceId + "&deviceType=" + Device.RuntimePlatform + "&clientCode=" + App.ClientCode + "&clientGroupCode=" + App.ClientGroupCode;
            }
            else
            {
                loginApiFullUrl = "InitiateLogin?emailId=" + emailId + "&password=" + password + "&token="
                + refreshedToken + "&deviceId=" + deviceId + "&deviceType=" + Device.RuntimePlatform + "&clientCode=" + App.ClientCode;
            }

            try
            {
                MobileAppLoginResultView result = await ApiHelper.PostRequest<MobileAppLoginResultView>(loginApiFullUrl, loginApiURL, null);
                return result;
            }
            catch (Exception ex)
            {
                var tokenStatus = string.IsNullOrEmpty(App.RefreshedToken) ? "Token_Empty" : "Token_Available";
                HelperMethods.LogEvent("Exception5", 
                    $"Exception5 - {ex.Message} - Token - {tokenStatus}");
                Console.WriteLine(ex);
                throw;
            }
            
        }

        public static async void DisplayException(Exception ex, string moduleName = "")
        {
            //Crashes.TrackError(ex);
            await ApiHelper.HideProcessingIndicatorPopup();
            Application.Current.MainPage.ShowPopup(new ExceptionAlertPopup());
        }

        public static async Task Logout(IMapper mapper, INativeServices nativeServices, INavigation navigation)
        {
            try
            {
                App.SurveyIdList = new List<int>();
                
                if (MopupService.Instance.PopupStack.Any())
                {
                    await MopupService.Instance.PopAsync();
                }
                var action = await App.Current.MainPage.DisplayAlert(TextResource.LogoutPopupTitle, TextResource.LogoutPopupConfirmationText, TextResource.YesText, TextResource.NoText);
                if (action)
                {
                    try
                    {
                        bool result = await ApiHelper.PostRequest<bool>(string.Format(TextResource.LogoutApiUrl, App.RefreshedToken, App.DeviceID, Device.RuntimePlatform), AppSettings.Current.ApiUrl);
                        if (result)
                        {
                            AppSettings.Current = new AppSettings();
                            //App.Current.Properties["IsLogin"] = false;
                            Preferences.Get("IsLogin", false);
                            await ICCacheManager.SaveSecureObject<string>("icampus_pwd", string.Empty);
                            await ICCacheManager.SaveSecureObject<string>("icampus_email", string.Empty);
                            ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
                            //await App.Current.SavePropertiesAsync();

                            LoginPage loginPage = new (mapper, nativeServices);
                            await navigation.PushAsync(loginPage);
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayException(ex);
                    }
                }
                
                
                // Analytics.TrackEvent("Logout clicked - " + AppSettings.Current.Email + " - " + DeviceInfo.Name + " - " + DeviceInfo.Model + " - " + DeviceInfo.Platform);
                // if (PopupNavigation.Instance.PopupStack.Any())
                // {
                //     await PopupNavigation.Instance.PopAllAsync();
                // }
                // App.SurveyIdList = new List<int>();
                // if (Device.RuntimePlatform == Device.Android)
                // {
                //     Xamarin.Forms.DependencyService.Get<INativeServices>().ShowAlertWithTwoButtons(TextResource.LogoutPopupTitle, TextResource.LogoutPopupConfirmationText, TextResource.YesText, TextResource.NoText, async (action) => {
                //         if (action)
                //         {
                //             try
                //             {
                //                 bool result = await ApiHelper.PostRequest<bool>(string.Format(TextResource.LogoutApiUrl,App.RefreshedToken, App.DeviceID, Device.RuntimePlatform), AppSettings.Current.ApiUrl);
                //                 if (result)
                //                 {
                //                     App.Current.Properties["IsLogin"] = false;
                //                     AppSettings.Current = new AppSettings();
                //                     await App.Current.SavePropertiesAsync();
                //                     await ICCacheManager.SaveSecureObject<string>("icampus_pwd", string.Empty);
                //                     await ICCacheManager.SaveSecureObject<string>("icampus_email", string.Empty);
                //                     ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
                //                     hostScreen.Router.NavigateAndReset.Execute(new LoginForm()).Subscribe();
                //
                //                 }
                //             }
                //             catch (Exception ex)
                //             {
                //                 HelperMethods.DisplayException(ex);
                //             }
                //            
                //         }
                //     });
                // }
                // else
                // {
                //     var action = await App.Current.MainPage.DisplayAlert(TextResource.LogoutPopupTitle, TextResource.LogoutPopupConfirmationText, TextResource.YesText, TextResource.NoText);
                //     if (action)
                //     {
                //         try
                //         {
                //             bool result = await ApiHelper.PostRequest<bool>(string.Format(TextResource.LogoutApiUrl, App.RefreshedToken, App.DeviceID, Device.RuntimePlatform), AppSettings.Current.ApiUrl);
                //             if (result)
                //             {
                //                 AppSettings.Current = new AppSettings();
                //                 App.Current.Properties["IsLogin"] = false;
                //                 await ICCacheManager.SaveSecureObject<string>("icampus_pwd", string.Empty);
                //                 await ICCacheManager.SaveSecureObject<string>("icampus_email", string.Empty);
                //                 ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
                //                 await App.Current.SavePropertiesAsync();
                //                 hostScreen.Router.NavigateAndReset.Execute(new LoginForm()).Subscribe();
                //             }
                //         }
                //         catch (Exception ex)
                //         {
                //             HelperMethods.DisplayException(ex);
                //         }
                //     }
                // }
                //
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }
        }


        #endregion

        #region Files

        public static async Task<bool> DownloadFile(string fileUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                using var httpClient = new HttpClient();
                using var response = await httpClient.GetAsync(fileUrl, cancellationToken);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to download file: {response.ReasonPhrase}");

                var fileName = Path.GetFileName(new Uri(fileUrl).AbsolutePath) ?? $"download_{DateTime.Now:yyyyMMdd_HHmmss}.bin";
                string folderPath;
#if ANDROID
            folderPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
#elif IOS
                folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#else
            folderPath = FileSystem.Current.AppDataDirectory; // Fallback for other platforms
#endif

                var filePath = Path.Combine(folderPath, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fileStream, cancellationToken);

                await Toast.Make($"File downloaded successfully: {filePath}").Show(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                await Toast.Make($"File download failed: {ex.Message}").Show(cancellationToken);
                return false;
            }
            //bool isDownloaded = false;
            // await Task.Run(() =>
            // {
            //     var downloadManager = CrossDownloadManager.Current;
            //     var file = downloadManager.CreateDownloadFile(fileUrl);
            //     downloadManager.Start(file, true);
            //     while (!isDownloaded)
            //     {
            //         isDownloaded = string.IsNullOrEmpty(GetDownloadedFilePath(file))?false:true;
            //     }
            // });
            //return isDownloaded;
        }

        // public static string GetDownloadedFilePath(IDownloadFile file)
        // {
        //     if (file == null)
        //     {
        //         return string.Empty;
        //     }
        //     switch (file.Status)
        //     {
        //         case DownloadFileStatus.INITIALIZED:
        //         case DownloadFileStatus.PAUSED:
        //         case DownloadFileStatus.PENDING:
        //         case DownloadFileStatus.RUNNING:
        //             return string.Empty;
        //
        //         case DownloadFileStatus.COMPLETED:
        //         case DownloadFileStatus.CANCELED:
        //         case DownloadFileStatus.FAILED:
        //             return file.DestinationPathName;
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }
        // }

        public static async Task OpenFileForPreview(string path,INativeServices nativeServices,string fileId = null)
        {
            try
            {
                nativeServices.DownloadAndPreviewFile(path, fileId);
            }
            catch(Exception ex)
            {
                 await ApiHelper.ValidateInternetConnectivityAndDisplayMessage();
            }
        }

        public static string GetAttachmentImageSourceFromType(FileTypes fileTypes)
        {
            string imageSource = string.Empty;
            switch (fileTypes)
            {
                case FileTypes.Txt:
                    imageSource = "txt_icon.png";
                    break;
                case FileTypes.Ppt:
                case FileTypes.Pptx:
                case FileTypes.Pptm:
                case FileTypes.Pps:
                case FileTypes.Pot:
                case FileTypes.Gwb:
                    imageSource = "powerpoint_icon.png";
                    break;
                case FileTypes.Doc:
                case FileTypes.Docx:
                case FileTypes.Docm:
                case FileTypes.Dot:
                case FileTypes.Odt:
                    imageSource = "word_icon.png";
                    break;
                case FileTypes.Pdf:
                    imageSource = "pdf_icon.png";
                    break;
                case FileTypes.Ods:
                case FileTypes.Xls:
                case FileTypes.Xlsx:
                    imageSource = "excel_icon.png";
                    break;
                case FileTypes.Png:
                case FileTypes.Jpeg:
                case FileTypes.Jpg:
                case FileTypes.Gif:
                    imageSource = "default_image.png";
                    break;
                default:
                    imageSource = "attachment_icon.png";
                    break;
            }
            return imageSource;
        }

        public static string GetAttachmentFileStatusImage(int fileStatus)
        {
            string imageSource = "download.png";
            switch (fileStatus)
            {
                case 1:
                    imageSource = "loader.gif";
                    break;
                case 2:
                    imageSource = "done_icon.png";
                    break;
                default:
                    imageSource = "download.png";
                    break;
            }
            return imageSource;
        }
        
        public async static Task<AttachmentFileView> PickFileFromDevice()
        {
            return await GetUploadedFileFromDevice(DocumentUploadType.File);
        }

        public async static Task<AttachmentFileView> PickImageFromDevice()
        {
            return await GetUploadedFileFromDevice(DocumentUploadType.Image);
        }

        private async static Task<AttachmentFileView> GetUploadedFileFromDevice(DocumentUploadType documentUploadType)
        {
            AttachmentFileView attachmentFileView = null;
            try
            {
                FileResult fileData = await FilePicker.PickAsync();
                if (fileData != null && ValidateDocumentType(fileData.FileName, fileData.FullPath.Length, documentUploadType))
                {
                    attachmentFileView = new AttachmentFileView();


                    byte[] imageArray = null;

                    using (MemoryStream memory = new MemoryStream())
                    {

                        var stream = await fileData.OpenReadAsync();
                        stream.CopyTo(memory);
                        imageArray = memory.ToArray();
                        attachmentFileView.FileData = imageArray;
                    }

                    attachmentFileView.FileName = fileData.FileName.Replace(" ", "_");
                    attachmentFileView.FilePath = fileData.FullPath;
                }
                return attachmentFileView;
            }
            catch (Exception ex)
            {
                return attachmentFileView;
            }
        }
        
        public static bool ValidateDocumentType(string fileName, DocumentUploadType documentUploadType)
        {
            return ValidateDocumentType(fileName, null, documentUploadType);
        }

        public static bool ValidateDocumentType(string fileName, int? fileSize, DocumentUploadType documentUploadType)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;
            string fileExt = Path.GetExtension(fileName).ToLower();
            string[] validExtensions = documentUploadType == DocumentUploadType.File ?
                AppSettings.Current.FileUploadSettings.AllowedFileExtensionsArray : AppSettings.Current.FileUploadSettings.AllowedImageExtensionsArray;
            if (!validExtensions.Contains(fileExt.Replace(".", "")))
            {
                App.Current.MainPage.DisplayAlert(TextResource.Error, string.Format(TextResource.FileTypeNotAllowed, fileExt), TextResource.OkText);
                return false;
            }
            else if (fileSize.HasValue && (fileSize > (AppSettings.Current.FileUploadSettings.FileUploadLimit * 1024 * 1024)))
            {
                App.Current.MainPage.DisplayAlert(TextResource.Error, string.Format(TextResource.LargeFileSize, AppSettings.Current.FileUploadSettings.FileUploadLimit), TextResource.OkText);
                return false;
            }
            else return true;
        }



        public async static Task PerformAutoLogout(string url)
        {
            // bool IsLogin = (bool)Application.Current.Properties["IsLogin"];
            // Analytics.TrackEvent("PerformAutoLogout - " + url + " - " + AppSettings.Current.Email + " - " + DeviceInfo.Name + " - " + DeviceInfo.Model + " - " + DeviceInfo.Platform + " - " + IsLogin);
            //
            // if (IsLogin)
            // {
            //     AppSettings.Current.IsSessionExpiredAlert = true;
            //     await HelperMethods.ShowActionAlert(TextResource.AutoLogoutTitle, TextResource.AutoLogoutMessage, TextResource.OkText, async () =>
            //     {
            //         AppSettings.Current.IsSessionExpiredAlert = false;
            //         if (PopupNavigation.Instance.PopupStack.Any())
            //         {
            //             await PopupNavigation.Instance.PopAllAsync();
            //         }
            //         AppSettings.Current = new AppSettings();
            //         App.Current.Properties["IsLogin"] = false;
            //         ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
            //         await App.Current.SavePropertiesAsync();
            //         hostScreen.Router.NavigateAndReset.Execute(new LoginForm()).Subscribe();
            //
            //     });
            // }
        }
        public static async Task<string> DownloadAndReturnFilePath(string fileUrl, INativeServices nativeServices, CancellationToken cancellationToken = default)
        {
            try
            {
                using var httpClient = new HttpClient();
                using var response = await httpClient.GetAsync(fileUrl, cancellationToken);
            
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Failed to download file: {response.ReasonPhrase}");
            
                var fileName = Path.GetFileName(new Uri(fileUrl).AbsolutePath) ?? $"download_{DateTime.Now:yyyyMMdd_HHmmss}.bin";
            
                var folderPath = await nativeServices.GetDownloadFolderPathAsync();
                    
                var filePath = Path.Combine(folderPath, fileName);
            
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fileStream, cancellationToken);
            
                await Toast.Make($"File downloaded successfully: {filePath}").Show(cancellationToken);
                return filePath;
            }
            catch (Exception ex)
            {
                await Toast.Make($"File download failed: {ex.Message}").Show(cancellationToken);
                return string.Empty;
            }
        }
        public static void ShowDownloadCompleteNotification(string filePath, string fileName)
        {
            // var context = Platform.CurrentActivity ?? Android.App.Application.Context;
            // var intent = new Intent(Intent.ActionView);
            // intent.SetDataAndType(Android.Net.Uri.Parse(filePath), "*/*");
            // intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission);
            //
            // PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            //
            // var notificationBuilder = new NotificationCompat.Builder(context, "download_channel")
            //     .SetContentTitle("Download Complete")
            //     .SetContentText(fileName)
            //     .SetSmallIcon(Resource.Drawable.ic_download_done)
            //     .SetContentIntent(pendingIntent)
            //     .SetAutoCancel(true);
            //
            // var notificationManager = NotificationManagerCompat.From(context);
            // notificationManager.Notify(1001, notificationBuilder.Build());
        }
        #endregion

        #region other methods
        public static DateTime GetCurrentWeekStartDate()
        {
            DateTime date = DateTime.Today;
            return date.AddDays(((int)date.DayOfWeek - AppSettings.Current.WeekStartDay) * -1);
        }
        public static DateTime GetCurrentWeekEndDate()
        {
            return GetCurrentWeekStartDate().AddDays(7).AddSeconds(-1);
        }

        public static DateTime GetWeekStartDate(DateTime date)
        {
            return date.AddDays(((int)date.DayOfWeek - AppSettings.Current.WeekStartDay) * -1);
        }
        public static DateTime GetWeekEndDate(DateTime date)
        {
            DateTime startDate = date.AddDays(((int)date.DayOfWeek - AppSettings.Current.WeekStartDay) * -1);
            return startDate.AddDays(7).AddSeconds(-1);
        }

        public static async Task ShowAlert(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, TextResource.OkText);
        }

        public static async Task ShowActionAlert(string title,string message,string buttonText,Action afterHideCallback)
        {
            await Application.Current.MainPage.DisplayAlert(title,message,buttonText);

            afterHideCallback?.Invoke();
        }

        public static async Task ShowAlertWithAction(string title, string message, Action okAction)
        {
            await App.Current.MainPage.DisplayAlert(title, message, TextResource.OkText);
            okAction?.Invoke();
        }

        public static List<BindablePickListItem> GetAttendanceType()
        {
            List<BindablePickListItem> TypeList = new List<BindablePickListItem>();
            try
            {
                TypeList = new List<BindablePickListItem>() { new BindablePickListItem() { ItemId="P", ItemName= "Present",ItemColor="#22B14C"},
                                                      new BindablePickListItem() { ItemId="A", ItemName= "Absent",ItemColor="#ED1C24"},
                                                      new BindablePickListItem() { ItemId="L", ItemName= "Late",ItemColor="#FFC90E"},
                                                      new BindablePickListItem() { ItemId="E", ItemName= "Left Early",ItemColor="#99D9EA"}};
                return TypeList;
            }
            catch (Exception ex)
            {
                return TypeList;
            }
        }
        public static byte[] GetStreamFromUrl(string url)
        {
            byte[] imageData = null;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                using (var wc = new System.Net.WebClient())
                    imageData = wc.DownloadData(url);
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
                imageData = null;
            }
            return imageData;
        }
        public static async void SendRegistrationToServer(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(App.RefreshedToken))
                {
                    await ICCacheManager.SaveSecureObject<string>("RefreshedToken", App.RefreshedToken);
                }
                bool IsLogin = false;
                // if (App.Current.Properties.ContainsKey("IsLogin"))
                //     IsLogin = (bool)Application.Current.Properties["IsLogin"];
                IsLogin = Preferences.Get("IsLogin", false);
                bool isInternetConnected = ApiHelper.ValidateInternetConnectivity();
                bool isPushNotificationEnable = await ICCacheManager.GetObject<bool>(TextResource.PushNotificationKey);

                if (isInternetConnected && !App.IsTokenUpdated && IsLogin && !string.IsNullOrEmpty(App.RefreshedToken) && !string.IsNullOrEmpty(AppSettings.Current.ApiUrl) && isPushNotificationEnable)
                {
                    _nativeServices.GetDeviceID(async (deviceId) =>
                     {
                         try
                         {
                             var result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.UpdateTokenApiUrl, token, deviceId, Device.RuntimePlatform), AppSettings.Current.ApiUrl);
                             if (result.Success)
                             {
                                 App.IsTokenUpdated = true;
                             }
                         }
                         catch (Exception ex)
                         {
                             await ApiHelper.HideProcessingIndicatorPopup();
                         }
                     });
                }
            }
            catch (Exception ex)
            {
                await ApiHelper.HideProcessingIndicatorPopup();
            }
        }

        public static void OpenWebsiteLinks(string url,string pageTitle, bool isInternalPage = false)
        {
            try
                {
                url = url.Replace("\\", "/");
                if (isInternalPage)
                    {
                   string clientCode = (!string.IsNullOrEmpty(App.ClientGroupCode)) ? App.ClientGroupCode : string.Empty;
                    if(clientCode.ToLower().Equals("beam"))
                    {
                        if (!url.Contains("&view=mobile"))
                        {
                            url = url + "&view=mobile";
                        }
                    }
                    else
                    {
                        if (!url.Contains("&view=m"))
                        {
                            url = url + "&view=m";
                        }
                    }

                    // WebViewForm webViewForm = new WebViewForm();
                    // webViewForm.PageTitle = pageTitle;
                    // webViewForm.BackVisible = true;
                    // webViewForm.WebViewUrl = url;
                    // hostScreen.Router.Navigate.Execute(webViewForm).Subscribe();
                    }
                    else
                    {
                        url = url.Replace("\\", "/");
                        if (url.StartsWith("http://") || url.StartsWith("https://"))
                        {
                            //Device.OpenUri(new Uri(url));
                            Launcher.OpenAsync(new Uri(url));
                        }
                        else
                        {
                            url = "http://" + url;
                            //Device.OpenUri(new Uri(url));
                            Launcher.OpenAsync(new Uri(url));
                        }
                    }
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, pageTitle);
                }
            }
        
        public static ObservableCollection<Week> GetWeekList(DateTime anyDate)
        {
            ObservableCollection<Week> weeks = new ObservableCollection<Week>();
            var startDate = GetWeekStartDate(anyDate.Date);
            var endDate = startDate.AddDays(6).Date;
        
            for (var weekDate = startDate; weekDate.Date <= endDate.Date; weekDate = weekDate.AddDays(1))
            {
                string displayDay = weekDate.DayOfWeek.ToString().Substring(0, 3).ToUpper();
                weeks.Add(new Week { Date = weekDate.Date, DisplayDate = weekDate.Day, DisplayDayOfWeek = displayDay, IsToday = (weekDate.Date == DateTime.Today) });
            }
            return weeks;
        }

        public async static Task<List<ColorData>> GetAgendaColorList(DateTime fromDate)
        {
            try
            {
                List<AgendaView> agendaViews = new List<AgendaView>();
                CalendarViewModel calendarViewModel = new CalendarViewModel();
                calendarViewModel = await ApiHelper.GetObject<CalendarViewModel>(string.Format(TextResource.GetAgendaColorApiUrl, new DateTime(fromDate.Year, fromDate.Month, 1).ToPickerDateFormatted(), "M", AppSettings.Current.SelectedStudent.ItemId));
                if(calendarViewModel!=null)
                {
                    agendaViews = calendarViewModel.AgendaData.ToList() ;
                }
                var dateColorDictionary = agendaViews.GroupBy(d => d.AgendaDate).
                        ToDictionary(grp => grp.Key, dates => dates.Select(d => d.WorkTypeColor).Distinct().ToList()
                    );
                List<ColorData> ColorAgendaList = dateColorDictionary.Select(x => new ColorData
                {
                    Color = x.Value,
                    Date = x.Key.Value
                }).ToList();
                AppSettings.Current.IsMonthlyView = true;
                return ColorAgendaList;
            }
            catch(Exception ex)
            {
                DisplayException(ex);
                return new List<ColorData>();
            }
        }

        public static void UpdatePushNotificationSetting(bool IsPushNotificationEnable, Action<bool> output, INativeServices nativeServices)
        {
            bool isInternetConnected = ApiHelper.ValidateInternetConnectivity();
            nativeServices.GetDeviceID(async (deviceId) =>
             {
                 try
                 {
                     if (isInternetConnected && !string.IsNullOrEmpty(App.RefreshedToken))
                     {
                         var result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.UpdateNotificationSettingApiUrl, IsPushNotificationEnable, App.RefreshedToken, deviceId, Device.RuntimePlatform), AppSettings.Current.ApiUrl);
                         if (result.Success)
                         {
                             //Analytics.TrackEvent("DEVICETOKEN - " + App.RefreshedToken + " - " + DateTime.Now + " - " + DeviceInfo.Name + " - " + DeviceInfo.Model + " - " + DeviceInfo.Platform);
                             ICCacheManager.SaveObject<bool>(TextResource.PushNotificationKey, IsPushNotificationEnable);
                             output?.Invoke(true);
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     output?.Invoke(false);
                     DisplayException(ex);
                 }
             });
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static string ConvertNumerals(string input)
        {
            input = Regex.Replace(input, @"[,\u066B\u060C]", ".");
            var newCharArr = input
                .Select(ch => char.IsDigit(ch) ? char.GetNumericValue(ch).ToString() : ch.ToString())
                .ToArray();

            return string.Join("", newCharArr);
        }
        public static void ShowHideVisibilityLinkedFields(BindableFormFieldsView field, ObservableCollection<BindableFormFieldsView> details)
        {
            string parentFieldValue = string.Empty;

            switch (field.EditorType)
            {
                case EditorTypes.YesOrNo:
                    parentFieldValue = field.FieldValue?.ToString().ToLower() == "true" ? "Y" : "N";
                    break;
                case EditorTypes.Selectbox:
                    parentFieldValue = Convert.ToString(field.FieldValue);
                    break;
                case EditorTypes.AutoComplete:
                    parentFieldValue = Convert.ToString(field.FieldValue);
                    break;
                case EditorTypes.CheckBox:
                    parentFieldValue = field.FieldValue?.ToString().ToLower() == "true" ? "true" : "false";
                    break;
            }

            if (field.HasLinkedChildren > 0)
            {
                var linkedChildren = details.Where(f => !string.IsNullOrEmpty(f.VisibilityLinkedField) && f.VisibilityLinkedField.StartsWith(field.FieldName + "-")).ToList();

                if (linkedChildren != null && linkedChildren.Any())
                {
                    foreach (var linkedChild in linkedChildren)
                    {
                        string visibilityLinkedField = linkedChild.VisibilityLinkedField;
                        string[] visibilityLinkedFieldElements = visibilityLinkedField.Split('-');

                        if (visibilityLinkedFieldElements.Length >= 2)
                        {
                            string visibilityLinkedFieldConnectingValue = visibilityLinkedFieldElements[1];
                            bool isVisible = visibilityLinkedFieldConnectingValue == parentFieldValue;

                            if (visibilityLinkedFieldElements.Length == 3)
                            {
                                string showOrHide = visibilityLinkedFieldElements[2].ToLower();
                                if (showOrHide == "hide")
                                {
                                    isVisible = visibilityLinkedFieldConnectingValue != parentFieldValue && !string.IsNullOrEmpty(parentFieldValue);
                                }
                            }

                            linkedChild.IsVisible = isVisible;
                        }
                    }
                }
            }
        }
        public static Page GetCurrentPage()
        {
            var mainPage = Application.Current.MainPage;
            if (mainPage is NavigationPage navigationPage)
            {
                return navigationPage.CurrentPage;
            }
            return mainPage;
        }
        public static void LogEvent(string eventName, string message)
        {
#if ANDROID
            var firebaseAnalytics = Firebase.Analytics.FirebaseAnalytics.GetInstance(Android.App.Application.Context);
            var bundle = new Android.OS.Bundle();
            bundle.PutString("message", message);
            firebaseAnalytics.LogEvent(eventName, bundle);
#elif IOS
            var parameters = new Foundation.NSDictionary<Foundation.NSString, Foundation.NSObject>(
                new Foundation.NSString("message"), new Foundation.NSString(message));
            Firebase.Analytics.Analytics.LogEvent(eventName, parameters);
#endif
        }

        public static void TrackCrashlytics(Exception exception)
        {
            try
            {
#if IOS
            var errorInfo = new Dictionary<object, object> {
                { NSError.LocalizedDescriptionKey, exception.Message },
                { NSError.LocalizedFailureReasonErrorKey, "Managed Failure" },
                { NSError.LocalizedRecoverySuggestionErrorKey, "Check your code or logs for more details." }
            };
        var error = new NSError(new NSString("NonFatalError"),-1001,
                    NSDictionary.FromObjectsAndKeys(errorInfo.Values.ToArray(), errorInfo.Keys.ToArray(), errorInfo.Keys.Count));

                Crashlytics.SharedInstance.RecordError(error);

#elif ANDROID
                FirebaseCrashlytics.Instance.RecordException(Java.Lang.Throwable.FromException(exception));
#endif

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recording exception in Crashlytics: {ex.Message}");
            }
        }

        #endregion
    }