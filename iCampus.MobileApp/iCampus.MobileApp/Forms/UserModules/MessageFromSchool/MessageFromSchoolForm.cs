using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Enums;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.MessageFromSchool;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.MessageFromSchool;

public class MessageFromSchoolForm : ViewModelBase
{
    #region Declaration

    private Popup _currentPopup;
    public ICommand CircularsTabbedCommand { get; set; }
    public ICommand AlertsTabbedCommand { get; set; }
    public ICommand AlertHistoryTabbedCommand { get; set; }
    public ICommand AttachmentClickCommand { get; set; }
    public ICommand LinksClickCommand { get; set; }
    public ICommand CircularListTappedCommand { get; set; }
    public ICommand AlertsListTappedCommand { get; set; }
    public ICommand HistoryListTappedCommand { get; set; }
    public ICommand FilterClickCommand { get; set; }

    #endregion

    private bool _isAlertsAvailable;
    private bool _isEnableCaching = true;

    public bool IsAlertsAvailable
    {
        get => _isAlertsAvailable;
        set
        {
            _isAlertsAvailable = value;
            OnPropertyChanged(nameof(IsAlertsAvailable));
        }
    }

    private bool _isHistoryAlertsAvailable;

    public bool IsHistoryAlertsAvailable
    {
        get => _isHistoryAlertsAvailable;
        set
        {
            _isHistoryAlertsAvailable = value;
            OnPropertyChanged(nameof(IsHistoryAlertsAvailable));
        }
    }

    private bool _isCircularDataAvailable;

    public bool IsCircularDataAvailable
    {
        get => _isCircularDataAvailable;
        set
        {
            _isCircularDataAvailable = value;
            OnPropertyChanged(nameof(IsCircularDataAvailable));
        }
    }

    private IList<CircularView> circularList = new List<CircularView>();

    public IList<CircularView> CircularList
    {
        get => circularList;
        set
        {
            circularList = value;
            OnPropertyChanged(nameof(CircularList));
        }
    }

    private CircularView _selectedCircular = new();

    public CircularView SelectedCircular
    {
        get => _selectedCircular;
        set
        {
            _selectedCircular = value;
            OnPropertyChanged(nameof(SelectedCircular));
        }
    }

    #region Custom Alerts

    private FeedBackAlertMessageView _selectedAlert = new();

    public FeedBackAlertMessageView SelectedAlert
    {
        get => _selectedAlert;
        set
        {
            _selectedAlert = value;
            OnPropertyChanged(nameof(SelectedAlert));
        }
    }

    private CustomAlertStatisticsView _selectedHistoryAlert = new();

    public CustomAlertStatisticsView SelectedHistoryAlert
    {
        get => _selectedHistoryAlert;
        set
        {
            _selectedHistoryAlert = value;
            OnPropertyChanged(nameof(SelectedHistoryAlert));
        }
    }

    private ObservableCollection<FeedBackAlertMessageView> _bindableCustomAlertList = new();

    public ObservableCollection<FeedBackAlertMessageView> BindableCustomAlertList
    {
        get => _bindableCustomAlertList;
        set
        {
            _bindableCustomAlertList = value;
            OnPropertyChanged(nameof(BindableCustomAlertList));
        }
    }

    private ObservableCollection<CircularView> _bindableCircularList = new();

    public ObservableCollection<CircularView> BindableCircularList
    {
        get => _bindableCircularList;
        set
        {
            _bindableCircularList = value;
            OnPropertyChanged(nameof(BindableCircularList));
        }
    }

    private IList<FeedBackAlertMessageView> customAlertsList = new List<FeedBackAlertMessageView>();

    public IList<FeedBackAlertMessageView> CustomAlertList
    {
        get => customAlertsList;
        set
        {
            customAlertsList = value;
            OnPropertyChanged(nameof(CustomAlertList));
        }
    }

    private IList<FeedBackAlertMessageView> _filteredCustomAlertList = new List<FeedBackAlertMessageView>();

    public IList<FeedBackAlertMessageView> FilteredCustomAlertList
    {
        get => _filteredCustomAlertList;
        set
        {
            _filteredCustomAlertList = value;
            OnPropertyChanged(nameof(FilteredCustomAlertList));
        }
    }

    private IList<CustomAlertStatisticsView> historyAlertsList = new List<CustomAlertStatisticsView>();

    public IList<CustomAlertStatisticsView> HistoryAlertsList
    {
        get => historyAlertsList;
        set
        {
            historyAlertsList = value;
            OnPropertyChanged(nameof(HistoryAlertsList));
        }
    }

    private IList<CustomAlertStatisticsView> _filteredHistoryAlertsList = new List<CustomAlertStatisticsView>();

    public IList<CustomAlertStatisticsView> FilteredHistoryAlertsList
    {
        get => _filteredHistoryAlertsList;
        set
        {
            _filteredHistoryAlertsList = value;
            OnPropertyChanged(nameof(FilteredHistoryAlertsList));
        }
    }

    private DateTime _fromDate;

    public DateTime FromDate
    {
        get => _fromDate;
        set
        {
            _fromDate = value;
            OnPropertyChanged(nameof(FromDate));
        }
    }

    private DateTime _toDate;

    public DateTime ToDate
    {
        get => _toDate;
        set
        {
            _toDate = value;
            OnPropertyChanged(nameof(ToDate));
        }
    }

    private IList<BindableAttachmentFileView> _selectedAttachmentList;

    public IList<BindableAttachmentFileView> SelectedAttachmentList
    {
        get => _selectedAttachmentList;
        set
        {
            _selectedAttachmentList = value;
            OnPropertyChanged(nameof(SelectedAttachmentList));
        }
    }

    private decimal _circularsButtonOpacity;
    public decimal CircularsButtonOpacity
    {
        get => _circularsButtonOpacity;
        set
        {
            _circularsButtonOpacity = value;
            OnPropertyChanged(nameof(CircularsButtonOpacity));
        }
    }

    private decimal _alertsButtonOpacity;
    public decimal AlertsButtonOpacity
    {
        get => _alertsButtonOpacity;
        set
        {
            _alertsButtonOpacity = value;
            OnPropertyChanged(nameof(AlertsButtonOpacity));
        }
    }

    private decimal _alertHistoryButtonOpacity;
    public decimal AlertHistoryButtonOpacity
    {
        get => _alertHistoryButtonOpacity;
        set
        {
            _alertHistoryButtonOpacity = value;
            OnPropertyChanged(nameof(AlertHistoryButtonOpacity));
        }
    }

    private bool _isCircularsVisible;
    public bool IsCircularsVisible
    {
        get => _isCircularsVisible;
        set
        {
            _isCircularsVisible = value;
            OnPropertyChanged(nameof(IsCircularsVisible));
        }
    }

    private bool _isAlertsVisible;
    public bool IsAlertsVisible
    {
        get => _isAlertsVisible;
        set
        {
            _isAlertsVisible = value;
            OnPropertyChanged(nameof(IsAlertsVisible));
        }
    }

    private bool _isAlertHistoryVisible;
    public bool IsAlertHistoryVisible
    {
        get => _isAlertHistoryVisible;
        set
        {
            _isAlertHistoryVisible = value;
            OnPropertyChanged(nameof(IsAlertHistoryVisible));
        }
    }

    public string CircularNotificationId { get; set; }
    public string AlertNotificationId { get; set; }

    #endregion

    public MessageFromSchoolForm(IMapper mapper, INativeServices nativeServices, INavigation navigation, NotificationData notification = null) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        if (notification != null)
        {
            var notificationType = (MobileNotificationTypes)Convert.ToInt32(notification.notificationType);
            if (notificationType.Equals(MobileNotificationTypes.Circulars))
                CircularNotificationId = notification.primaryKey;
            else
                AlertNotificationId = notification.primaryKey;
        }
        
        MessagingCenter.Subscribe<string>(this, "NavigateToMessageFromSchool", async (res) => { InitializePage(); });
        InitializePage();
    }

    private async void InitializePage()
    {
        try
        {
            IsCircularsVisible = true;
            IsAlertsVisible = false;
            IsAlertHistoryVisible = false;
            CircularsButtonOpacity = 1.0m;
            AlertsButtonOpacity = 0.5m;
            AlertHistoryButtonOpacity = 0.5m;
            CircularsTabbedCommand = new Command(CircularsClicked);
            AlertsTabbedCommand = new Command(AlertsClicked);
            AlertHistoryTabbedCommand = new Command(AlertHistoryClicked);
            CircularListTappedCommand = new Command<CircularView>(CircularListViewTapped);
            AlertsListTappedCommand = new Command<FeedBackAlertMessageView>(AlertListViewTapped);
            HistoryListTappedCommand = new Command<CustomAlertStatisticsView>(HistoryListViewTapped);
            FilterClickCommand = new Command(FilterClicked);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            MenuVisible = true;
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            LinksClickCommand = new Command<CircularView>(LinksClicked);
            AttachmentClickCommand = new Command<CircularView>(AttachmentClicked);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            await GetData();
            MessagingCenter.Subscribe<FeedBackAlertMessageView>(this, "CustomAlertSavedSuccess",
                async (submittedAlert) =>
                {
                    if (submittedAlert != null)
                    {
                        CustomAlertList.Remove(submittedAlert);
                        BindableCustomAlertList.Remove(submittedAlert);
                        IsAlertsAvailable = !BindableCustomAlertList.Any();
                    }
                });
            MessagingCenter.Subscribe<MessageFromSchoolFilterForm>(this, "SearchMessageFromSchool",
                async (filterFormData) =>
                {
                    FromDate = filterFormData.FromDate;
                    ToDate = filterFormData.ToDate;
                    await GetCircularList(true);
                    FilteredCustomAlertList = CustomAlertList.Where(x =>
                        x.CustomAlertsExpiryDate >= FromDate.Date && x.CustomAlertsExpiryDate <= ToDate.Date).ToList();
                    FilteredHistoryAlertsList = HistoryAlertsList.Where(x =>
                        x.CustomAlertsCheckedDate.ToDateTime() >= FromDate.Date &&
                        x.CustomAlertsCheckedDate.ToDateTime() <= ToDate.Date).ToList();
                    BindableCustomAlertList =
                        new ObservableCollection<FeedBackAlertMessageView>(FilteredCustomAlertList);
                    IsAlertsAvailable = !BindableCustomAlertList.Any();
                    IsHistoryAlertsAvailable = !FilteredHistoryAlertsList.Any();
                });
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.MessageFromSchoolPageTitle);
        }
    }

    // public MessageFromSchoolForm(NotificationData notification) : base(null, null, null)
    // {
    //     var notificationType = (MobileNotificationTypes)Convert.ToInt32(notification.notificationType);
    //     if (notificationType.Equals(MobileNotificationTypes.Circulars))
    //         CircularNotificationId = notification.primaryKey;
    //     else
    //         AlertNotificationId = notification.primaryKey;
    //     GetData();
    // }

    private void CircularsClicked(object obj)
    {
        IsCircularsVisible = true;
        IsAlertsVisible = false;
        IsAlertHistoryVisible = false;
        CircularsButtonOpacity = 1.0m;
        AlertsButtonOpacity = 0.5m;
        AlertHistoryButtonOpacity = 0.5m;
    }

    private void AlertsClicked(object obj)
    {
        IsCircularsVisible = false;
        IsAlertsVisible = true;
        IsAlertHistoryVisible = false;
        CircularsButtonOpacity = 0.5m;
        AlertsButtonOpacity = 1.0m;
        AlertHistoryButtonOpacity = 0.5m;
    }

    private void AlertHistoryClicked(object obj)
    {
        IsCircularsVisible = false;
        IsAlertsVisible = false;
        IsAlertHistoryVisible = true;
        CircularsButtonOpacity = 0.5m;
        AlertsButtonOpacity = 0.5m;
        AlertHistoryButtonOpacity = 1.0m;
    }
    private async void FilterClicked(object obj)
    {
        MessageFromSchoolFilterForm messageFromSchoolFilterForm = new(_mapper, _nativeServices, Navigation)
        {
            PageTitle = PageTitle,
            MenuVisible = false,
            BackVisible = true,
            FromDate = FromDate,
            ToDate = ToDate
        };    
        MessageFromSchoolFilterPage messageFromSchoolFilterPage = new()
        {
            BindingContext = messageFromSchoolFilterForm
        };
        await Navigation.PushAsync(messageFromSchoolFilterPage);
    }

    private async Task GetData()
    {
        try
        {
            await GetCircularList();
            await GetCustomAlerts();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async Task<IList<FeedBackAlertMessageView>> GetCustomAlerts()
    {
        try
        {
            var caheType = !string.IsNullOrEmpty(AlertNotificationId)
                ? ApiHelper.CacheTypeParam.LoadFromServerAndCache
                : ApiHelper.CacheTypeParam.LoadFromCache;

            var alertData = await ApiHelper.GetObject<CustomAlertsView>(
                string.Format(TextResource.CustomAlertsApiUrl, true), attachStudentIdIfParent: false,
                cacheKeyPrefix: "customalertsdata", cacheType: caheType);
            if (alertData != null)
            {
                CustomAlertList = alertData.FeedBackAlertList;
                FilteredCustomAlertList = customAlertsList;
                HistoryAlertsList = alertData.CustomAlertStatistics;

                HistoryAlertsList = HistoryAlertsList
                    .OrderByDescending(x => Convert.ToDateTime(x.CustomAlertsCheckedDate)).ToList();

                FilteredHistoryAlertsList = HistoryAlertsList;
                BindableCustomAlertList = new ObservableCollection<FeedBackAlertMessageView>(FilteredCustomAlertList);
                IsAlertsAvailable = !BindableCustomAlertList.Any();
                IsHistoryAlertsAvailable = !FilteredHistoryAlertsList.Any();
                if (BindableCustomAlertList != null && BindableCustomAlertList.Count > 0 &&
                    !string.IsNullOrEmpty(AlertNotificationId))
                {
                    var alertView = BindableCustomAlertList
                        .Where(x => x.CustomAlertsId == Convert.ToInt32(AlertNotificationId)).FirstOrDefault();
                    if (alertView != null)
                        AlertListViewTapped(alertView);
                    AlertNotificationId = null;
                }
            }

            return CustomAlertList;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.MessageFromSchoolPageTitle);
            return new List<FeedBackAlertMessageView>();
        }
    }

    #region Private methods

    public async Task<IList<CircularView>> GetCircularList(bool isSearch = false)
    {
        try
        {
            var caheType = !string.IsNullOrEmpty(CircularNotificationId)
                ? ApiHelper.CacheTypeParam.LoadFromServerAndCache
                : ApiHelper.CacheTypeParam.LoadFromCache;

            var loadHomePageData = false;
            var apiUrl = string.Format(TextResource.CircularListApiUrl, "", "", loadHomePageData);
            var initialCircularList = await ApiHelper.GetObjectList<CircularView>(apiUrl,
                attachStudentIdIfParent: false, cacheKeyPrefix: "circularlist", cacheType: caheType);
            if (isSearch)
                CircularList = initialCircularList
                    .Where(circular => circular.FormattedCircularDate.ToDateTime().Date >= FromDate.Date &&
                                       circular.FormattedCircularDate.ToDateTime().Date <= ToDate.Date).ToList();
            else
                CircularList = initialCircularList;
            IsCircularDataAvailable = !CircularList.Any();
            if (CircularList != null && CircularList.Count > 0 && !string.IsNullOrEmpty(CircularNotificationId))
            {
                var circularView = CircularList.Where(x => x.CircularId == Convert.ToInt32(CircularNotificationId))
                    .FirstOrDefault();
                if (circularView != null)
                    CircularListViewTapped(circularView);
                CircularNotificationId = null;
            }

            return CircularList;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.MessageFromSchoolPageTitle);
            return new List<CircularView>();
        }
    }

    private async void LinksClicked(CircularView sender)
    {
        WebsiteLinksPopupForm websiteLinksPopupForm = new WebsiteLinksPopupForm(_mapper, _nativeServices, Navigation)
        {
            PageTitle = this.PageTitle,
            IsInternalPage = sender.IsInternalPage,
            SelectedWebsiteLinks = sender.WebsiteLinks
        };
        foreach (var link in websiteLinksPopupForm.SelectedWebsiteLinks)
        {
            if (!string.IsNullOrEmpty(link.Title))
                link.Title = Uri.UnescapeDataString(link.Title);
        }
        var websiteLinksPopup = new WebsiteLinksPopup()
        {
            BindingContext = websiteLinksPopupForm
        };
        SetPopupInstance(websiteLinksPopup);
        Application.Current.MainPage.ShowPopup(websiteLinksPopup);
    }

    private async void AttachmentClicked(CircularView sender)
    {
        AttachmentListPopupForm attachmentListPopupForm = new(_mapper, _nativeServices, Navigation)
        {
            SelectedAttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(sender.AttachmentList)
        };
        var attachmentListPopup = new AttachmentListPopup()
        {
            BindingContext = attachmentListPopupForm
        };
        SetPopupInstance(attachmentListPopup);
        await Application.Current.MainPage.ShowPopupAsync(attachmentListPopup);
    }
    public void SetPopupInstance(Popup popup)
    {
        AppSettings.Current.CurrentPopup = popup;
    }

    private async void AlertListViewTapped(FeedBackAlertMessageView obj)
    {
        try
        {
            if (obj != null)
            {
                CustomAlertPopupForm popupForm = new (_mapper, _nativeServices, Navigation)
                {
                    CustomAlertObject = obj,
                    ListViewHeight = obj.CustomAlertUserButtonList.Count * 40,
                    TitleColor = obj.CategoryColor,
                    IsCloseOption = (CustomAlertMessageTypes)obj.CustomCategoryMsgId == CustomAlertMessageTypes.Regular ? true : false
                };
                SelectedAlert = null;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void CircularListViewTapped(CircularView obj)
    {
        try
        {
            if (obj != null)
            {
                CircularDetailForm circularDetailForm = new(_mapper, _nativeServices, Navigation)
                {
                    CircularObject = obj,
                    AttachmentListViewHeight = obj.AttachmentList.Count * 40,
                    LinkListViewHeight = obj.WebsiteLinksCount * 40,
                    AttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(obj.AttachmentList)
                };
                foreach (var link in circularDetailForm.CircularObject.WebsiteLinks)
                {
                    if (!string.IsNullOrEmpty(link.Url))
                        link.Title = Uri.UnescapeDataString(link.Title);
                }

                SelectedCircular = null;
                CircularDetail circularDetail = new()
                {
                    BindingContext = circularDetailForm
                };
                await Navigation.PushAsync(circularDetail);
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void HistoryListViewTapped(CustomAlertStatisticsView obj)
    {
        try
        {
            if (obj != null)
            {
                HistoryDetailForm historyDetails = new(_mapper, _nativeServices, Navigation)
                {
                    HistoryDetailsObject = obj,
                    IsTitleAvailable = !string.IsNullOrEmpty(obj.CustomAlertsName),
                    IsMessageAvailable = !string.IsNullOrEmpty(obj.CustomAlertsMessage),
                    IsDateAvailable = !string.IsNullOrEmpty(obj.CustomAlertsCheckedDate),
                    IsAcknowledgedAvailable = !string.IsNullOrEmpty(obj.CustomAlertsIsAcknowledged),
                    IsFeedbackAvailable = !string.IsNullOrEmpty(obj.FeedbackData)
                };
                SelectedHistoryAlert = null;
                HistoryDetailsPage historyDetailsPage = new()
                {
                    BindingContext = historyDetails
                };
                await Navigation.PushAsync(historyDetailsPage);
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }
    #endregion
}