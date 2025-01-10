using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Forms.UserModules.MessageFromSchool;
using iCampus.MobileApp.Forms.UserModules.Survey;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Survey;
using iCampus.Portal.ViewModels;
using SQLitePCL;

namespace iCampus.MobileApp.Forms;

public class HomeForm : ViewModelBase
{
    #region Declarations
    public INavigation Navigation { get; set; }
    private IMapper _mapper;
    private INativeServices _nativeServices;
    public ICommand ListTappedCommand { get; set; }
    public ICommand RefreshedCommand { get; set; }
    string homePageDataRefreshApiUrl = string.Empty;

    public ICommand BeamMenuClickCommand { get; set; }
    public ICommand BeamHeaderMessageIconClickCommand { get; set; }
    public ICommand BeamHeaderNotificationIconClickCommand { get; set; }
    public ICommand BeamHeaderStudentImageClickCommand { get; set; }
    public ICommand LandingPageMenuClickCommand { get; set; }

    #endregion

    #region Properties
    private ApiHelper.CacheTypeParam _cacheType;
    private BindableSiteNewsView _selectedNews = new BindableSiteNewsView();
    public BindableSiteNewsView SelectedNews
    {
        get => _selectedNews;
        set
        {
            if (_selectedNews != value)
            {
                _selectedNews = value;
                OnPropertyChanged(nameof(SelectedNews));
            }
        }
    }

    private ObservableCollection<BindableSiteNewsView> _newsList = new ObservableCollection<BindableSiteNewsView>();
    public ObservableCollection<BindableSiteNewsView> NewsList
    {
        get => _newsList;
        set
        {
            if (_newsList != value)
            {
                _newsList = value;
                OnPropertyChanged(nameof(NewsList));
            }
        }
    }

    private MobileAppLoginResultView _homePageData = new MobileAppLoginResultView();
    public MobileAppLoginResultView HomePageData
    {
        get => _homePageData;
        set
        {
            if (_homePageData != value)
            {
                _homePageData = value;
                OnPropertyChanged(nameof(HomePageData));
            }
        }
    }

    private bool _isLoader;
    private bool _isExceptionPopup;

    private string _newsUrl;
    public string NewsUrl
    {
        get => _newsUrl;
        set
        {
            if (_newsUrl != value)
            {
                _newsUrl = value;
                OnPropertyChanged(nameof(NewsUrl));
            }
        }
    }

    private bool _newsStatus;
    public bool NewsStatus
    {
        get => _newsStatus;
        set
        {
            if (_newsStatus != value)
            {
                _newsStatus = value;
                OnPropertyChanged(nameof(NewsStatus));
            }
        }
    }

    private bool _isPageLoaded;
    public bool IsPageLoaded
    {
        get => _isPageLoaded;
        set
        {
            if (_isPageLoaded != value)
            {
                _isPageLoaded = value;
                OnPropertyChanged(nameof(IsPageLoaded));
            }
        }
    }

    private bool _isNewsSelectedFromFooter;
    public bool IsNewsSelectedFromFooter
    {
        get => _isNewsSelectedFromFooter;
        set
        {
            if (_isNewsSelectedFromFooter != value)
            {
                _isNewsSelectedFromFooter = value;
                OnPropertyChanged(nameof(IsNewsSelectedFromFooter));
            }
        }
    }

    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            if (_isRefreshing != value)
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
    }

    private ObservableCollection<FeedBackAlertMessageView> _customAlertsList = new ObservableCollection<FeedBackAlertMessageView>();
    public ObservableCollection<FeedBackAlertMessageView> CustomAlertList
    {
        get => _customAlertsList;
        set
        {
            if (_customAlertsList != value)
            {
                _customAlertsList = value;
                OnPropertyChanged(nameof(CustomAlertList));
            }
        }
    }

    private IList<CustomAlertStatisticsView> _historyAlertsList = new List<CustomAlertStatisticsView>();
    public IList<CustomAlertStatisticsView> HistoryAlertsList
    {
        get => _historyAlertsList;
        set
        {
            if (_historyAlertsList != value)
            {
                _historyAlertsList = value;
                OnPropertyChanged(nameof(HistoryAlertsList));
            }
        }
    }

    public string NewsNotificationId { get; set; }

    #endregion
    
    public HomeForm(IMapper mapper, INavigation navigation, INativeServices nativeServices, bool isFromNotification = false) : base(mapper, nativeServices, navigation)  // Pass dependencies to base class
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        App.HomePageInstance = this;
        InitializePage(isFromNotification);  
    }


    #region Methods

    private async void InitializePage(bool isFromNotification = false)
    {
        try
        {
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            SetBeamAppViews();
            ListTappedCommand = new Command<BindableSiteNewsView>(ListViewTapped);
            RefreshedCommand = new Command(RefreshNews);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            LandingPageMenuClickCommand = new Command<BindableModuleStructureView>(LandingPageMenuClicked);
            MenuVisible = true;
            await InitializeThemeSettings();
            if (!App.IsUserAlertDone)
                Device.StartTimer(TimeSpan.FromMinutes(TextResource.DataRefreshTimerIntervalMinutes.ToDouble()),
                    HomePageDataRefreshingTimer);
            Device.StartTimer(TimeSpan.FromSeconds(5.ToInteger()), CheckVersionUpdate);
            //notification navigation when app is killed in iOS and for android
            if (App.NotificationValues != null && !string.IsNullOrEmpty(App.NotificationValues.notificationType) &&
                !string.IsNullOrEmpty(App.NotificationValues.notificationType))
            {
                PushNotificationClick(App.NotificationValues, _nativeServices);
            }
            else
            {
                if (ApiHelper.ValidateInternetConnectivity() && !App.IsUserAlertDone) 
                    await GetCustomAlerts();
            }

            // Observer for Push notification data for iOS when app is active/background
            MessagingCenter.Subscribe<NotificationData>(this, "PushData", (res) =>
            {
                if (App.NotificationValues != null && !string.IsNullOrEmpty(App.NotificationValues.notificationType) &&
                    !string.IsNullOrEmpty(App.NotificationValues.notificationType))
                    PushNotificationClick(App.NotificationValues, _nativeServices);
                MessagingCenter.Unsubscribe<NotificationData>(this, "PushData");
            });
            MessagingCenter.Subscribe<FeedBackAlertMessageView>(this, "HomeCustomAlertSavedSuccess",
                async (submittedAlert) =>
                {
                    if (submittedAlert != null && CustomAlertList != null && CustomAlertList.Count > 0)
                    {
                        var index = CustomAlertList.IndexOf(CustomAlertList
                            .Where(x => x.CustomAlertsId == submittedAlert.CustomAlertsId).FirstOrDefault());
                        if (index >= 0)
                        {
                            CustomAlertList.RemoveAt(index);
                            if (CustomAlertList.Count > 0)
                                await ShowCustomAlert(CustomAlertList.FirstOrDefault());
                        }
                    }
                });
            MenuPage = new SideMenuPanel();
            IsPageLoaded = true;
            // MessagingCenter.Subscribe<string>("", "ListViewRightSwipeNews", async (arg) =>
            // {
            //     await SideMenuClicked();
            // });
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);

            IsPageLoaded = true;
            Preferences.Set("IsLogin", false);
            // App.Current.Properties["IsLogin"] = false;
            // await App.Current.SavePropertiesAsync();
        }
    }
    private async void RefreshNews()
    {
        var isInternetConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        if (isInternetConnected)
            NewsList = new ObservableCollection<BindableSiteNewsView>(
                await ApiHelper.GetObjectList<BindableSiteNewsView>(TextResource.HomePageNewsPath,
                    attachStudentIdIfParent: false, cacheKeyPrefix: "newslist",
                    cacheType: ApiHelper.CacheTypeParam.LoadFromServerAndCache));
        else
            await ApiHelper.DisplayNoConnectionMessage();
    }

    public async void OnDisappearing()
    {
        // bool didAppCrash = await Crashes.HasCrashedInLastSessionAsync();
        // if (didAppCrash)
        // {
        //     ErrorReport errorReport = await Crashes.GetLastSessionCrashReportAsync();
        // }
    }

    private void ListViewTapped(BindableSiteNewsView obj)
    {
        var htmlContent = string.Empty;
        if (obj != null)
        {
            // //TODO:list view click event
            // NewsDetailForm newsDetailForm = new NewsDetailForm();
            // newsDetailForm.SiteNewsObject = obj;
            // BindableAttachmentFileView bindableAttachmentFile = new BindableAttachmentFileView();
            // bindableAttachmentFile.FileName = obj.AttachmentName;
            // bindableAttachmentFile.FilePath = obj.AttachmentPath;
            // bindableAttachmentFile.FileStatus = 0;
            //
            // if (Device.RuntimePlatform == Device.iOS)
            //     htmlContent = "<html>" + "<head>" + "<style type=\"text/css\">" + "body {" + "font-size: 16;" + "text-align: justify;" + "}" + "</style>" + "</head>" + "<body>" + newsDetailForm.SiteNewsObject.NewsData + "</body>" + "</html>";
            // else
            //     htmlContent = "<html>" + "<head>" + "<style type=\"text/css\">" + "body {" + "font-size: 14;" + "text-align: justify;" + "}" + "</style>" + "</head>" + "<body>" + newsDetailForm.SiteNewsObject.NewsData + "</body>" + "</html>";
            // newsDetailForm.SiteNewsObject.NewsData = htmlContent;
            // newsDetailForm.Attachment = bindableAttachmentFile;
            // HostScreen.Router.Navigate.Execute(newsDetailForm).Subscribe();
            // SelectedNews = null;
        }
    }

    public async Task<IList<BindableSiteNewsView>> GetNewsList(bool isFromNotification = false, bool isRefresh = false)
    {
        try
        {
            var cacheKeyPrefix = "newslist";
            _cacheType = isFromNotification || isRefresh
                ? ApiHelper.CacheTypeParam.LoadFromServerAndCache
                : ApiHelper.CacheTypeParam.LoadFromCache;
            NewsList = new ObservableCollection<BindableSiteNewsView>(
                await ApiHelper.GetObjectList<BindableSiteNewsView>(TextResource.HomePageNewsPath,
                    attachStudentIdIfParent: false, cacheKeyPrefix: cacheKeyPrefix, cacheType: _cacheType));
            if (NewsList != null && NewsList.Count > 0 && !string.IsNullOrEmpty(NewsNotificationId))
            {
                var newsView = NewsList.Where(x => x.SiteNewsId == Convert.ToInt32(NewsNotificationId))
                    .FirstOrDefault();
                if (newsView != null)
                    ListViewTapped(newsView);
                NewsNotificationId = null;
            }

            return NewsList;
        }
        catch (Exception ex)
        {
            if (ex?.Message != null && ex.Message.ToLower().Contains("specified hostname could not be found"))
            {
                try
                {
                    //Analytics.TrackEvent("Home GetNewsList ex - " + AppSettings.Current.Email + " - " + DeviceInfo.Name + " - " + DeviceInfo.Model + " - " + DeviceInfo.Platform + " - " + ex);
                }
                catch (Exception Ex)
                {
                    HelperMethods.DisplayException(Ex);
                }

                Preferences.Set("IsLogin", false);
                //App.Current.Properties["IsLogin"] = false;
                AppSettings.Current = new AppSettings();
                ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
                //await App.Current.SavePropertiesAsync();
                await Navigation.PushAsync(new LoginPage(_mapper, _nativeServices));
                //HostScreen.Router.NavigateAndReset.Execute(new LoginForm()).Subscribe();
            }
            else
            {
                HelperMethods.DisplayException(ex, TextResource.NewsPageTitle);
            }

            return new List<BindableSiteNewsView>();
        }
    }

    private async Task InitializeThemeSettings()
    {
        try
        {
            if (!string.IsNullOrEmpty(AppSettings.Current.SelectedStudent.ItemId))
            {
                ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
            }
            else
            {
                var getThemeSettingsDataFromCache = await ICCacheManager.GetObjectData<AppSettings>("AppSettings");
                if (getThemeSettingsDataFromCache.Item1) AppSettings.Current = getThemeSettingsDataFromCache.Item2;
            }

            PageTitle = "Dashboard";
            ThemeColor = AppSettings.Current.Settings.ThemeColor;
            var color = Colors.White;
            //Xamarin.Forms.DependencyService.Get<INativeServices>().ChangeStatusBarColor((int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255));
            if (!App.IsInitialized)
            {
                // var bootstrapper = new AppBootsrapper();
                // bootstrapper.AppInitialization();
                
                //Call login page
            }
        }
        catch (KeyNotFoundException ex)
        {
            await Navigation.PushAsync(new LoginPage(_mapper, _nativeServices));
            //HostScreen.Router.NavigateAndReset.Execute(new LoginForm()).Subscribe();
        }
    }

    private async Task LoadHomePageNews(bool isFromNotification = false, bool isRefresh = false)
    {
        NewsStatus = AppSettings.Current.Settings.NewsStatus;
        if (NewsStatus)
            await GetNewsList(isFromNotification, isRefresh);
        else
            NewsUrl = AppSettings.Current.Settings.DefaultUrl;
    }

    private bool HomePageDataRefreshingTimer()
    {
        var isInternetConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        //Device.BeginInvokeOnMainThread(() =>
        //{
        if (isInternetConnected) GetHomePageData();
        // });
        return isInternetConnected;
    }

    public async void GetHomePageData()
    {
        try
        {
            VersionTracking.Track();
            //bool isLogin = (bool)Application.Current.Properties["IsLogin"];
            var isLogin = Preferences.Get("IsLogin", false);
            _isLoader = false;
            _isExceptionPopup = false;

            if (isLogin)

            {
                var pwd = string.Empty;
                try
                {
                    pwd = await ICCacheManager.GetSecureObject<string>("icampus_pwd");
                }
                catch (Exception ex)
                {
                    //Crashes.TrackError(ex);
                }

                var cacheKeyPrefix = "homepagedata";
                var clientGroupCode = !string.IsNullOrEmpty(App.ClientGroupCode) ? App.ClientGroupCode : string.Empty;
                homePageDataRefreshApiUrl = string.Format(TextResource.HomePageDataApiUrl, AppSettings.Current.Email,
                    null, null, Device.RuntimePlatform.ToString(), pwd, App.ClientCode, App.ClientGroupCode);
                HomePageData = await ApiHelper.GetObject<MobileAppLoginResultView>(homePageDataRefreshApiUrl, _isLoader,
                    attachStudentIdIfParent: false, cacheKeyPrefix: cacheKeyPrefix,
                    cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
                if (HomePageData.IsPasswordChanged || !HomePageData.IsValidUser)
                {
                    if (!AppSettings.Current.IsSessionExpiredAlert)
                        // Analytics.TrackEvent(name: "Home page data refresh api call",
                        // new Dictionary<string, string> {
                        //     { "DateTime", DateTime.Now.ToString() },
                        //     { "methodName", TextResource.HomePageDataApiUrl},
                        //     {"IsPasswordChanged", HomePageData.IsPasswordChanged.ToString()},
                        //     {"IsValidUser" , HomePageData.IsValidUser.ToString()}
                        // });
                        await HelperMethods.PerformAutoLogout(homePageDataRefreshApiUrl);
                }
                else
                {
                    var cacheKeyPrefixnews = "newslist";
                    ICCacheManager.SaveObjectList<SiteNewsView>(cacheKeyPrefixnews, HomePageData.NewsList.ToList());
                    UpdateHomePageView();
                    UpdateAppSettings();
                }

                var firstLaunchCurrent = VersionTracking.IsFirstLaunchForCurrentVersion;
                if (firstLaunchCurrent && !App.IsUserVersionUpdate)
                {
                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(
                        string.Format(TextResource.UserAppInfoUpdateApiUrl, VersionTracking.CurrentVersion));
                    if (result.Success)
                        App.IsUserVersionUpdate = true;
                }

                if (HomePageData.DataCollection != null &&
                    HomePageData.DataCollection.DataCollectionFormFields != null &&
                    HomePageData.DataCollection.DataCollectionFormFields.Any() && !App.IsSurveyOn &&
                    !App.IsDataCollectionOn)
                    ShowDataCollection(HomePageData.DataCollection);
                else if (HomePageData.UserSurvey != null && HomePageData.UserSurvey.SurveyQuestions != null &&
                         HomePageData.UserSurvey.SurveyQuestions.Any() && !App.IsSurveyOn &&
                         !App.IsDataCollectionOn) ShowSurvey(HomePageData.UserSurvey);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Home page refresh api - " + ex?.Message);
            if (ex?.Message != null && ex.Message.ToLower().Contains("specified hostname could not be found"))
            {
                //Analytics.TrackEvent("Home page refresh api ex - " + AppSettings.Current.Email + " - " + DeviceInfo.Name + " - " + DeviceInfo.Model + " - " + DeviceInfo.Platform + " - " + ex);
                Preferences.Set("IsLogin", true);
                //App.Current.Properties["IsLogin"] = false;
                AppSettings.Current = new AppSettings();
                ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
                //await App.Current.SavePropertiesAsync();
                await Navigation.PushAsync(new LoginPage(_mapper, _nativeServices));
                //HostScreen.Router.NavigateAndReset.Execute(new LoginForm()).Subscribe();
            }

            //Crashes.TrackError(ex);
            if (_isExceptionPopup) HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void UpdateHomePageView()
    {
        var cacheKeyPrefix = "newslist";
        var isInternetConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        var cacheResult =
            await ApiHelper.LoadObjectListFromCache<BindableSiteNewsView>(cacheKeyPrefix, isInternetConnected);
        if (cacheResult.Item1 == true) NewsList = new ObservableCollection<BindableSiteNewsView>(cacheResult.Item2);
    }

    private void UpdateAppSettings()
    {
        try
        {
            if (HomePageData != null)
            {
                AppSettings.Current.ApiUrl = !string.IsNullOrEmpty(HomePageData.PortalApiUrl)
                    ? HomePageData.PortalApiUrl
                    : AppSettings.Current.ApiUrl;
                AppSettings.Current.Settings = HomePageData.MobileAppSettings != null
                    ? HomePageData.MobileAppSettings
                    : AppSettings.Current.Settings;
                AppSettings.Current.LogoUrl = !string.IsNullOrEmpty(HomePageData.LogoUrl)
                    ? HomePageData.LogoUrl
                    : AppSettings.Current.LogoUrl;
                AppSettings.Current.MenuStructureList =
                    HomePageData.MenuList != null && HomePageData.MenuList.ToList().Count > 0
                        ? _mapper.Map<List<BindableModuleStructureView>>(HomePageData.MenuList.ToList())
                        : AppSettings.Current.MenuStructureList;
                foreach (var item in AppSettings.Current.MenuStructureList)
                    item.ModuleImageUrl = item.ModuleImageUrl?.Replace("https", "http");
                var index = AppSettings.Current.MenuStructureList.ToList().FindIndex(x =>
                    x.ModuleCode != null && x.ModuleCode.ToLower().Equals("notificationcenter"));
                if (index >= 0)
                {
                    var element = AppSettings.Current.MenuStructureList[index];
                    AppSettings.Current.MenuStructureList.RemoveAt(index);
                    AppSettings.Current.MenuStructureList.Insert(0, element);
                }

                AppSettings.Current.CacheRecordSize =
                    HomePageData.CacheRecordSize < TextResource.MinimumCacheRecordSize.ToInteger()
                        ? TextResource.MinimumCacheRecordSize.ToInteger()
                        : HomePageData.CacheRecordSize;
                if (AppSettings.Current.MenuStructureList?.FirstOrDefault()?.ModuleCode != null && AppSettings.Current
                        .MenuStructureList.FirstOrDefault().ModuleCode.ToLower().Contains("settings"))
                    AppSettings.Current.SettingsMenuImageUrl = AppSettings.Current.MenuStructureList
                        .SingleOrDefault(a => a.ModuleCode.ToLower().Equals("settings"))?.ModuleImageUrl;

                if (AppSettings.Current.MenuStructureList != null)
                {
                    AppSettings.Current.BeamCommunicationAndBellIconVisibility = AppSettings.Current.MenuStructureList
                        .Where(x => x.ModuleCode != null && x.ModuleCode.ToLower().Equals("communication"))
                        .FirstOrDefault() != null
                        ? true
                        : false;
                    AppSettings.Current.FooterMenuList = new ObservableCollection<BindableModuleStructureView>(
                        AppSettings.Current.MenuStructureList?.Where(x =>
                            x.IsFooterMenuOnMobile && x.ModuleCode != null && !(x.ModuleCode.ToLower().Equals("home") ||
                                x.ModuleCode.ToLower().Equals("contactus") ||
                                x.ModuleCode.ToLower().Equals("messagefromschool") ||
                                x.ModuleCode.ToLower().Equals("settings"))).Take(3).ToList());
                    AppSettings.Current.FooterMenuList.Insert(0,
                        new BindableModuleStructureView()
                            { ModuleCode = "Home", ModuleShortName = "Home", ModuleName = "Home", ShowIcon = true });
                    AppSettings.Current.FooterMenuList.Add(new BindableModuleStructureView()
                        { ModuleCode = "News", ModuleShortName = "News", ModuleName = "News", ShowIcon = true });
                }

                AppSettings.Current.BeamHeaderNotificationCount = HomePageData.UserNotificationCount;
                AppSettings.Current.BeamHeaderImage = HomePageData.HeaderBackGroundImagePath;
                if (AppSettings.Current.MenuStructureList != null)
                {
                    AppSettings.Current.LandingPageMenuList = (from i in AppSettings.Current.MenuStructureList
                        let found = AppSettings.Current.FooterMenuList.Any(j => j == i)
                        where !found
                        select i).ToList();
                    var removeList = AppSettings.Current.LandingPageMenuList.Where(x =>
                        x.ModuleCode != null && (x.ModuleCode.ToLower().Equals("home") ||
                                                 x.ModuleCode.ToLower().Equals("communication") ||
                                                 x.ModuleCode.ToLower().Equals("messagefromschool"))).ToList();
                    AppSettings.Current.LandingPageMenuList = (from i in AppSettings.Current.LandingPageMenuList
                        let found = removeList.Any(j => j == i)
                        where !found
                        select i).ToList();
                    AppSettings.Current.LandingPageMenuList = AppSettings.Current.LandingPageMenuList
                        .OrderBy(x => x.MobileAppMenuSequence).ToList();
                    if (AppSettings.Current.AssignmentNotificationCount > 0)
                    {
                        var notificationCenterModuleCode = AppSettings.Current.MenuStructureList
                            .Where(x => x.ModuleCode != null && x.ModuleCode.ToLower().Equals("notificationcenter"))
                            .FirstOrDefault();
                        if (notificationCenterModuleCode != null)
                            notificationCenterModuleCode.ModuleName = notificationCenterModuleCode.ModuleName +
                                                                      string.Concat(" ", "(",
                                                                          AppSettings.Current
                                                                              .AssignmentNotificationCount, ")");
                    }
                }

                AppSettings.Current.LogoutItem.ModuleName = "Logout";
                AppSettings.Current.LogoutItem.ModuleImageUrl = "logout_icon.png";
                //Analytics.TrackEvent("UpdateAppSettings - " + AppSettings.Current.Email + " - " + DeviceInfo.Name + " - " + DeviceInfo.Model + " - " + DeviceInfo.Platform);

                ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
            }
        }
        catch (Exception ex)
        {
            ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
            //Crashes.TrackError(ex);
        }
    }

    private bool CheckVersionUpdate()
    {
        try
        {
            var isInternetConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            if (isInternetConnected)
            {
                var currentVersion = new Version(VersionTracking.CurrentVersion);
                var latestVersion = new Version(AppSettings.Current.VersionNumber);
                // if (latestVersion.CompareTo(currentVersion) > 0)
                // {
                //     AppUpdatePopupForm appUpdatePopupForm = new ()
                //     {
                //         UpdateMessage = TextResource.UpdateAppMessage,
                //         IsForceUpdate = AppSettings.Current.IsMandatoryUpdate,
                //         UpdateTitle = TextResource.UpdateAppTitle
                //     };
                //     AppUpdatePopup appUpdatePopup = new ()
                //     {
                //         BindingContext = appUpdatePopupForm
                //     };
                //     SetPopupInstance(appUpdatePopup);
                //     Application.Current.MainPage.ShowPopup(appUpdatePopup);
                // }
            }

            return false;
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
            return false;
        }
    }
    private async Task<IList<FeedBackAlertMessageView>> GetCustomAlerts()
    {
        try
        {
            var alertData = await ApiHelper.GetObject<CustomAlertsView>(
                string.Format(TextResource.CustomAlertsApiUrl, false), isLoader: false, attachStudentIdIfParent: false);
            if (alertData != null)
            {
                CustomAlertList = new ObservableCollection<FeedBackAlertMessageView>(alertData.FeedBackAlertList);
                if (_customAlertsList.Count > 0)
                {
                    if (!App.CustomAlertIdList.Contains(_customAlertsList.FirstOrDefault().CustomAlertsId))
                        await ShowCustomAlert(_customAlertsList.FirstOrDefault());
                }
                else if (alertData.DataCollection != null && alertData.DataCollection.DataCollectionFormFields.Any())
                {
                    ShowDataCollection(alertData.DataCollection);
                }
                else if (alertData.UserSurvey != null && alertData.UserSurvey.SurveyQuestions.Any())
                {
                    ShowSurvey(alertData.UserSurvey);
                }
                else
                {
                    App.IsUserAlertDone = true;
                }
            }

            return CustomAlertList;
        }
        catch (Exception ex)
        {
            await ApiHelper.HideProcessingIndicatorPopup();
            //Crashes.TrackError(ex);
            return new List<FeedBackAlertMessageView>();
        }
    }

    public async Task ShowCustomAlert(FeedBackAlertMessageView feedBackAlertMessageView)
    {
        try
        {
            if (!App.CustomAlertIdList.Contains(feedBackAlertMessageView.CustomAlertsId))
            {
                CustomAlertPopupForm customAlertPopupForm = new(_mapper, _nativeServices, Navigation)
                {
                    CustomAlertObject = feedBackAlertMessageView,
                    IsFromHomePage = true,
                    ListViewHeight = feedBackAlertMessageView.CustomAlertUserButtonList.Count * 40,
                    TitleColor = feedBackAlertMessageView.CategoryColor,
                    IsCloseOption = (CustomAlertMessageTypes)feedBackAlertMessageView.CustomCategoryMsgId == CustomAlertMessageTypes.Regular ? true : false
                };
                // CustomAlertPopup customAlertPopup = new()
                // {
                //     BindingContext = customAlertPopupForm
                // };
                // SetPopupInstance(customAlertPopup);
                // Application.Current.MainPage.ShowPopup(customAlertPopup);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async void ShowSurvey(UserSurveyView surveyView)
    {
        var surveyId = surveyView.SurveyQuestions.FirstOrDefault().SurveyId;
        SurveyForm surveyForm = new (_mapper, _nativeServices, Navigation)
        {
            PageTitle = TextResource.SurveyPageTitle,
            UserSurvey = surveyView
        };
        surveyForm.GetSurveyDetails();
        SurveyPage surveyPage = new()
        {
            BindingContext = surveyForm
        };
        await Navigation.PushAsync(surveyPage);
    }

    public void ShowDataCollection(DataCollectionView dataCollectionView)
    {
        if (dataCollectionView != null && dataCollectionView.DataCollectionFormFields != null &&
            dataCollectionView.DataCollectionFormFields.Count() > 0)
        {
            // DataCollectionForm dataCollectionForm = new DataCollectionForm();
            // dataCollectionForm.PageTitle = TextResource.DataCollectionPageTitle;
            // dataCollectionForm.MenuVisible = true;
            // dataCollectionForm.AssignFormData(dataCollectionView.ActiveDataCollectionForm, dataCollectionView.DataCollectionFormFields);
            // if (HostScreen.Router.GetCurrentViewModel().GetType() != typeof(DataCollectionForm))
            //     HostScreen.Router.Navigate.Execute(dataCollectionForm).Subscribe();
        }
    }
    public void SetPopupInstance(Popup popup)
    {
        AppSettings.Current.CurrentPopup = popup;
    }
    public async Task<IList<BindableSiteNewsView>> RefreshNewsList()
    {
        try
        {
            var isInternetConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            var cacheKeyPrefix = "newslist";
            if (isInternetConnected)
                NewsList = new ObservableCollection<BindableSiteNewsView>(
                    await ApiHelper.GetObjectList<BindableSiteNewsView>(TextResource.HomePageNewsPath,
                        attachStudentIdIfParent: false, cacheKeyPrefix: cacheKeyPrefix,
                        cacheType: ApiHelper.CacheTypeParam.LoadFromServerAndCache));
            else
                await ApiHelper.DisplayNoConnectionMessage();
            return NewsList;
        }
        catch (Exception ex)
        {
            if (ex?.Message != null && ex.Message.ToLower().Contains("specified hostname could not be found"))
            {
                //Analytics.TrackEvent("Home page RefreshNewsList ex - " + AppSettings.Current.Email + " - " + DeviceInfo.Name + " - " + DeviceInfo.Model + " - " + DeviceInfo.Platform + " - " + ex);

                Preferences.Set("IsLogin", false);
                //App.Current.Properties["IsLogin"] = false;
                AppSettings.Current = new AppSettings();
                ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
                //await App.Current.SavePropertiesAsync();
                await Navigation.PushAsync(new LoginPage(_mapper, _nativeServices));
                //HostScreen.Router.NavigateAndReset.Execute(new LoginForm()).Subscribe();
            }
            else
            {
                HelperMethods.DisplayException(ex, TextResource.NewsPageTitle);
            }

            return new List<BindableSiteNewsView>();
        }
    }

    private void SetBeamAppViews()
    {
        if (StudentList != null && StudentList.Count > 0)
            AppSettings.Current.SelectedStudent = AppSettings.Current.StudentList.FirstOrDefault();
        if (AppSettings.Current.IsParent)
        {
            AppSettings.Current.IsRegisteredStudentListVisible = true;
            AppSettings.Current.IsAllStudentListVisible = false;
        }
        else
        {
            AppSettings.Current.IsRegisteredStudentListVisible = AppSettings.Current.IsAllStudentListVisible = false;
        }
    }

    private void LandingPageMenuClicked(BindableModuleStructureView obj)
    {
        if (obj != null && obj.ModuleCode != null)
        {
            AppSettings.Current.SelectedLandingScreenMenu = obj;
            AppSettings.Current.FooterMenuList.ToList()
                .FindAll(b => b.ModuleCode != AppSettings.Current.SelectedLandingScreenMenu.ModuleCode)
                .ForEach(b => b.IsSelected = false);
            MenuListViewTapped(obj);
        }
    }

    #endregion
}