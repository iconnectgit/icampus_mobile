using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views;
using iCampus.MobileApp.Views.UserModules.Settings;

namespace iCampus.MobileApp.Forms.UserModules.Settings;

public class SettingsForm : ViewModelBase
    {
        #region Declarations
        public ICommand ListTappedCommand { get; set; }
        public ICommand LogoutClickCommand { get; set; }
        public ICommand SwitchToggledEventCommand { get; set; }
        #endregion

        #region Properties
        string _selectedOption;
        public string SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
            }
        }
        string _versionText;
        public string VersionText
        {
            get => _versionText;
            set
            {
                _versionText = value;
                OnPropertyChanged(nameof(VersionText));
            }
        }

        #endregion
        public SettingsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation, string id = null) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            ListTappedCommand = new Command(ListViewTapped);
            LogoutClickCommand = new Command(LogoutClicked);
            SwitchToggledEventCommand = new Command(PushNotificationToggled);
            InitializePage();
        }

        #region Methods

        private async void InitializePage()
        {
            try
            {
                BeamMenuClickCommand = new Command(BeamMenuClicked);
                BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
                BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
                BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
                await _nativeServices.CheckNotificationSetting();
                VersionText = TextResource.VersionText + " " + VersionTracking.CurrentVersion;
                HelperMethods.UpdatePushNotificationSetting(AppSettings.Current.IsPushNotificationEnable,null, _nativeServices);
            }
            catch (KeyNotFoundException ex)
            {
                HelperMethods.DisplayException(ex);
            }
        }
        private void PushNotificationToggled()
        {
            try
            {
                Device.InvokeOnMainThreadAsync(() =>
                {
                    HelperMethods.UpdatePushNotificationSetting(AppSettings.Current.IsPushNotificationEnable,(res)=> {
                        if (res)
                            _nativeServices.NotificationToggled();
                    }, _nativeServices);
                });
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        private async void LogoutClicked(object obj)
        {
            try
            {
                HelperMethods.Logout(_mapper, _nativeServices, Navigation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async void ListViewTapped(object obj)
        {
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                string selectedValue = obj.ToString().ToLower();
                WebViewForm webViewForm = new (_mapper, _nativeServices, Navigation);
                WebViewPage webViewPage = new WebViewPage()
                {
                    BindingContext = webViewForm
                };
                SelectedOption = null;
                switch (selectedValue)
                {
                    case "about":
                        webViewForm.PageTitle = obj.ToString();
                        webViewForm.BackVisible = true;
                        webViewForm.WebViewUrl = TextResource.AboutUrl;
                        await Navigation.PushAsync(webViewPage);
                        break;
                    case "help center":
                        webViewForm.PageTitle = obj.ToString();
                        webViewForm.BackVisible = true;
                        webViewForm.WebViewUrl = TextResource.HelpCenterUrl;
                        await Navigation.PushAsync(webViewPage);
                        break;
                    case "privacy policy":
                        webViewForm.PageTitle = obj.ToString();
                        webViewForm.BackVisible = true;
                        webViewForm.WebViewUrl = TextResource.PrivacyPolicyUrl;
                        await Navigation.PushAsync(webViewPage);
                        break;
                    case "report an issue":
                        SetReportAnIssuePopupMessage();
                        break;
                    case "clear cache":
                        var action = await App.Current.MainPage.DisplayAlert(TextResource.ClearCacheText, TextResource.ClearCacheAlertMsg, TextResource.YesText, TextResource.NoText);
                        if (action)
                        {
                            await ICCacheManager.ClearCache();
                            Preferences.Set("IsLogin", true);
                            AppSettings.Current.IsBeamAppViewsInitialized = false;
                            //App.Current.Properties["IsLogin"] = false;
                            await Navigation.PushAsync(new LoginPage(_mapper, _nativeServices));
                            //HostScreen.Router.NavigateAndReset.Execute(new LoginForm()).Subscribe();
                        }
                        break;
                    case "change password":
                        var output = await ApiHelper.GetObject<bool>(string.Format(TextResource.ChangePasswordApi, AppSettings.Current.Email));
                        if (!output)
                        {
                            await HelperMethods.ShowAlert("Alert!", TextResource.ChangePasswordNotAllowMsg);
                        }
                        else
                        {
                            // ChangePasswordForm changePasswordForm = new ChangePasswordForm();
                            // changePasswordForm.PageTitle = "Change Password";
                            // changePasswordForm.MenuVisible = false;
                            // changePasswordForm.BackVisible = true;
                            // changePasswordForm.IsPopUpPage = false;
                            // HostScreen.Router.Navigate.Execute(changePasswordForm).Subscribe();
                        }
                        break;
                }
            }
        }

        async void SetReportAnIssuePopupMessage()
        {
            string reportanIssuePopupMessage = string.Empty;
            if(App.ClientGroupCode?.ToLower()==("beam"))
            {
                reportanIssuePopupMessage = TextResource.BeamReportIssueAlertMsg;
            }
            else
            {
                reportanIssuePopupMessage = TextResource.ReportIssueAlertMsg;
            }
            await HelperMethods.ShowAlert(TextResource.ReportIssueAlertTitle, reportanIssuePopupMessage);
        }
        #endregion
    }