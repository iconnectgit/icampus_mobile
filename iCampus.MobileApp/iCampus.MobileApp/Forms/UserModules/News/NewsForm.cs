using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views;
using iCampus.MobileApp.Views.UserModules.News;

namespace iCampus.MobileApp.Forms.UserModules.News;

public class NewsForm:ViewModelBase
    {
        #region Declarations
        ApiHelper.CacheTypeParam _cacheType;
        public ICommand RefreshedCommand { get; set; }
        public ICommand ListTappedCommand { get; set; }
        public ICommand BeamMenuClickCommand { get; set; }
        public ICommand BeamHeaderMessageIconClickCommand { get; set; }
        public ICommand BeamHeaderNotificationIconClickCommand { get; set; }
        public ICommand BeamHeaderStudentImageClickCommand { get; set; }
        #endregion Declarations

        #region properties
        ObservableCollection<BindableSiteNewsView> _newsList = new ObservableCollection<BindableSiteNewsView>();
        public ObservableCollection<BindableSiteNewsView> NewsList
        {
            get => _newsList;
            set
            {
                _newsList = value;
                OnPropertyChanged(nameof(NewsList));
            }
        }
        bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        bool _newsStatus;
        public bool NewsStatus
        {
            get => _newsStatus;
            set
            {
                _newsStatus = value;
                OnPropertyChanged(nameof(NewsStatus));
            }
        }
        string _newsUrl;
        public string NewsUrl
        {
            get => _newsUrl;
            set
            {
                _newsUrl = value;
                OnPropertyChanged(nameof(NewsUrl));
            }
        }
        bool _isNewsSelectedFromFooter;
        public bool IsNewsSelectedFromFooter
        {
            get => _isNewsSelectedFromFooter;
            set
            {
                _isNewsSelectedFromFooter = value;
                OnPropertyChanged(nameof(IsNewsSelectedFromFooter));
            }
        }
        BindableSiteNewsView _selectedNews = new BindableSiteNewsView();
        public BindableSiteNewsView SelectedNews
        {
            get => _selectedNews;
            set
            {
                _selectedNews = value;
                OnPropertyChanged(nameof(SelectedNews));
            }
        }
        bool _isPageLoaded;
        public bool IsPageLoaded
        {
            get => _isPageLoaded;
            set
            {
                _isPageLoaded = value;
                OnPropertyChanged(nameof(IsPageLoaded));
            }
        }
        public string NewsNotificationId { get; set; }
        #endregion properties
        public NewsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation, bool isFromNotification = false) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage(isFromNotification);
        }
        #region Methods
        async void InitializePage(bool isFromNotification = false)
        {
            this.RefreshedCommand = new Command(RefreshNews);
            this.ListTappedCommand = new Command<BindableSiteNewsView>(ListViewTapped);
            this.BeamMenuClickCommand = new Command(BeamMenuClicked);
            this.BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            this.BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            this.BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            await LoadHomePageNews(isFromNotification, false);
        }
        public async Task<IList<BindableSiteNewsView>> GetNewsList(bool isFromNotification = false, bool isRefresh = false)
        {
            try
            {
                string cacheKeyPrefix = "newslist";
                _cacheType = isFromNotification || isRefresh ? ApiHelper.CacheTypeParam.LoadFromServerAndCache : ApiHelper.CacheTypeParam.LoadFromCache;
                NewsList = new ObservableCollection<BindableSiteNewsView>(await ApiHelper.GetObjectList<BindableSiteNewsView>(
                    TextResource.HomePageNewsPath, cacheType: _cacheType, cacheKeyPrefix: cacheKeyPrefix));

                if (NewsList != null && NewsList.Count > 0 && !string.IsNullOrEmpty(NewsNotificationId))
                {
                    BindableSiteNewsView newsView = NewsList.Where(x => x.SiteNewsId == Convert.ToInt32(NewsNotificationId)).FirstOrDefault();
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
                    //Analytics.TrackEvent("Home GetNewsList ex - " + AppSettings.Current.Email + " - " + DeviceInfo.Name + " - " + DeviceInfo.Model + " - " + DeviceInfo.Platform + " - " + ex);

                    //App.Current.Properties["IsLogin"] = false;
                    Preferences.Set("IsLogin", false);
                    AppSettings.Current = new AppSettings();
                    ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);
                    //await App.Current.SavePropertiesAsync();
                    //HostScreen.Router.NavigateAndReset.Execute(new LoginForm()).Subscribe();
                    await Navigation.PushAsync(new LoginPage(_mapper, _nativeServices));
                }
                else
                {
                    HelperMethods.DisplayException(ex, TextResource.NewsPageTitle);
                }
                return new List<BindableSiteNewsView>();
            }
        }
        private async void RefreshNews()
        {
            IsRefreshing = true;
            await LoadHomePageNews(false, true);
            IsRefreshing = false;
        }
        private async Task LoadHomePageNews(bool isFromNotification, bool isRefresh)
        {
            NewsStatus = AppSettings.Current.Settings.NewsStatus;
            if (NewsStatus)
                await GetNewsList(isFromNotification, isRefresh);
            else
                NewsUrl = AppSettings.Current.Settings.DefaultUrl;
        }

        private async void ListViewTapped(BindableSiteNewsView obj)
        {
            
            if (obj != null)
            {
                _ = await ApiHelper.GetObjectList<object>(string.Format(TextResource.InsertNewsLog, obj.SiteNewsId), isLoader: false);
                
                BindableAttachmentFileView bindableAttachmentFile = new BindableAttachmentFileView();
                bindableAttachmentFile.FileName = obj.AttachmentName;
                bindableAttachmentFile.FilePath = obj.AttachmentPath;
                bindableAttachmentFile.FileStatus = 0;
                
                NewsDetailForm newsDetailForm = new NewsDetailForm(_mapper, _nativeServices, Navigation)
                {
                    SiteNewsObject = obj,
                };
                newsDetailForm.FormattedNewsData = "<html><head><meta name='viewport' content='width=device-width, initial-scale=1.0'>"
                                                      + "<style>body { font-size: 14px; font-family: Arial; font-weight: normal; color: #555555; margin: 0; padding: 0; }</style>"
                                                      + "<script type='text/javascript'>"
                                                      + "function getHeight() { window.location.href = 'webview://height=' + document.body.scrollHeight; }"
                                                      + "window.onload = getHeight;"
                                                      + "</script>"
                                                      + "</head><body>" + obj.NewsData + "</body></html>";

                newsDetailForm.Attachment = bindableAttachmentFile;
                NewsDetailPage newsDetailPage = new NewsDetailPage()
                {
                    BindingContext = newsDetailForm
                };
                await Navigation.PushAsync(newsDetailPage);
                SelectedNews = null;
            }
        }
        #endregion Methods
    }