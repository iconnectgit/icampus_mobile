using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Helpers.CustomCalendar;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Calendar;
using iCampus.Portal.EditModels;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class CalendarForm : ViewModelBase
{
    #region Declarations

    public ICommand ListTappedCommand { get; set; }
    public ICommand FilterClickCommand { get; set; }
    public ICommand AttachmentClickCommand { get; set; }
    public ICommand DonwloadTappedCommand { get; set; }
    public ICommand AttachmentListTappedCommand { get; set; }
    public ICommand LinksClickCommand { get; set; }
    public ICommand WebsiteLinksTappedCommand { get; set; }
    public ICommand SearchClickCommand { get; set; }
    public ICommand PreviousClickCommand { get; set; }
    public ICommand NextClickCommand { get; set; }
    public ICommand WeekDateSelectionCommand { get; set; }
    public ICommand WeeklyPlanClickCommand { get; set; }
    public ICommand MonthlyDateChosenCommand { get; set; }
    public ICommand OpenCalendarCommand { get; set; }
    public ICommand ArrowClickedCommand { get; set; }
    public ICommand NewPostClickedCommand { get; set; }
    public ICommand ViewSubmissionsClickedCommand { get; set; }
    public ICommand CurrentWeekClickedCommand { get; set; }
    public ICommand CourseSelectionCommand { get; set; }
    public ICommand CourseListTappedCommand { get; set; }
    public ICommand AgendaForChangedCommand { get; set; }
    public ICommand QuickPostClickedCommand { get; set; }
    public ICommand AddPostCancelClickedCommand { get; set; }
    public ICommand AddPostPopupClickedCommand { get; set; }
    public ICommand WeeklyPlanClickedCommand { get; set; }
    public ICommand WeeklyExpandCollapseClickCommand { get; set; }
    public ICommand WeeklyListTappedCommand { get; set; }

    #endregion

    #region Properties

    private ApiHelper.CacheTypeParam _cacheType = ApiHelper.CacheTypeParam.LoadFromCache;
    private string _toDateValue = string.Empty;
    private string _showReminderDataValue = string.Empty;

    private BindableAgendaView _selectedAgenda = new();

    public BindableAgendaView SelectedAgenda
    {
        get => _selectedAgenda;
        set
        {
            _selectedAgenda = value;
            OnPropertyChanged(nameof(SelectedAgenda));
        }
    }

    private BindableAgendaView _weeklySelectedAgenda = new();

    public BindableAgendaView WeeklySelectedAgenda
    {
        get => _weeklySelectedAgenda;
        set
        {
            _weeklySelectedAgenda = value;
            OnPropertyChanged(nameof(WeeklySelectedAgenda));
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

    private ObservableCollection<BindableAgendaView> _agendaList = new();

    public ObservableCollection<BindableAgendaView> AgendaList
    {
        get => _agendaList;
        set
        {
            _agendaList = value;
            OnPropertyChanged(nameof(AgendaList));
        }
    }

    private ObservableCollection<BindableAgendaView> _defaultAgendaList = new();

    public ObservableCollection<BindableAgendaView> DefaultAgendaList
    {
        get => _defaultAgendaList;
        set
        {
            _defaultAgendaList = value;
            OnPropertyChanged(nameof(DefaultAgendaList));
        }
    }

    private ObservableCollection<BindableAgendaView> _bindableAgendaList = new();

    public ObservableCollection<BindableAgendaView> BindableAgendaList
    {
        get => _bindableAgendaList;
        set
        {
            _bindableAgendaList = value;
            OnPropertyChanged(nameof(BindableAgendaList));
        }
    }

    private ObservableCollection<BindableAgendaView> _searchAgendaList = new();

    public ObservableCollection<BindableAgendaView> SearchAgendaList
    {
        get => _searchAgendaList;
        set
        {
            _searchAgendaList = value;
            OnPropertyChanged(nameof(SearchAgendaList));
        }
    }

    private List<BindableAgendaView> _agendaListForNotificationClick = new();

    public List<BindableAgendaView> AgendaListForNotificationClick
    {
        get => _agendaListForNotificationClick;
        set
        {
            _agendaListForNotificationClick = value;
            OnPropertyChanged(nameof(AgendaListForNotificationClick));
        }
    }

    private ObservableCollection<Grouping<string, BindableAgendaView>> _groupedAgendaList;

    public ObservableCollection<Grouping<string, BindableAgendaView>> GroupedAgendaList
    {
        get => _groupedAgendaList;
        set
        {
            _groupedAgendaList = value;
            OnPropertyChanged(nameof(GroupedAgendaList));
        }
    }

    private ObservableCollection<Week> _weekList = new();

    public ObservableCollection<Week> WeekList
    {
        get => _weekList;
        set
        {
            _weekList = value;
            OnPropertyChanged(nameof(WeekList));
        }
    }

    private IEnumerable<WebsiteLinkView> _selectedWebsiteLinks;

    public IEnumerable<WebsiteLinkView> SelectedWebsiteLinks
    {
        get => _selectedWebsiteLinks;
        set
        {
            _selectedWebsiteLinks = value;
            OnPropertyChanged(nameof(SelectedWebsiteLinks));
        }
    }

    private AgendaViewModel _agendaData = new();

    public AgendaViewModel AgendaData
    {
        get => _agendaData;
        set
        {
            _agendaData = value;
            OnPropertyChanged(nameof(AgendaData));
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

    private DateTime _selectedDate;

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged(nameof(SelectedDate));
        }
    }

    private string _agendaTypeId;

    public string AgendaTypeId
    {
        get => _agendaTypeId;
        set
        {
            _agendaTypeId = value;
            OnPropertyChanged(nameof(AgendaTypeId));
        }
    }

    private ExtPickListItem _selectedAgendaType = new();

    public ExtPickListItem SelectedAgendaType
    {
        get => _selectedAgendaType;
        set
        {
            _selectedAgendaType = value;
            OnPropertyChanged(nameof(SelectedAgendaType));
        }
    }

    private IList<ExtPickListItem> _agendaTypeList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> AgendaTypeList
    {
        get => _agendaTypeList;
        set
        {
            _agendaTypeList = value;
            OnPropertyChanged(nameof(AgendaTypeList));
        }
    }

    private string _calendarPageTitle;

    public string CalendarPageTitle
    {
        get => _calendarPageTitle;
        set
        {
            _calendarPageTitle = value;
            OnPropertyChanged(nameof(CalendarPageTitle));
        }
    }

    private bool _showReminderData;

    public bool ShowReminderData
    {
        get => _showReminderData;
        set
        {
            _showReminderData = value;
            OnPropertyChanged(nameof(ShowReminderData));
        }
    }

    private bool _isNoRecordMsg;

    public bool IsNoRecordMsg
    {
        get => _isNoRecordMsg;
        set
        {
            _isNoRecordMsg = value;
            OnPropertyChanged(nameof(IsNoRecordMsg));
        }
    }

    private bool _isWeeklyView = true;

    public bool IsWeeklyView
    {
        get => _isWeeklyView;
        set
        {
            _isWeeklyView = value;
            OnPropertyChanged(nameof(IsWeeklyView));
        }
    }

    private DateTime _weekDate;

    public DateTime WeekDate
    {
        get => _weekDate;
        set
        {
            _weekDate = value;
            OnPropertyChanged(nameof(WeekDate));
        }
    }

    private string _weekTitleText;

    public string WeekTitleText
    {
        get => _weekTitleText;
        set
        {
            _weekTitleText = value;
            OnPropertyChanged(nameof(WeekTitleText));
        }
    }

    private Week _selectedWeekDate = new();

    public Week SelectedWeekDate
    {
        get => _selectedWeekDate;
        set
        {
            _selectedWeekDate = value;
            OnPropertyChanged(nameof(SelectedWeekDate));
        }
    }

    private List<ColorData> _colorAgendaList = new();

    public List<ColorData> ColorAgendaList
    {
        get => _colorAgendaList;
        set
        {
            _colorAgendaList = value;
            OnPropertyChanged(nameof(ColorAgendaList));
        }
    }

    private AgendaEdit _addPostData;

    public AgendaEdit AddPostData
    {
        get => _addPostData;
        set
        {
            _addPostData = value;
            OnPropertyChanged(nameof(AddPostData));
        }
    }

    private IList<CalendarAgendaTypePickListItem> _agendaTypes = new List<CalendarAgendaTypePickListItem>();

    public IList<CalendarAgendaTypePickListItem> AgendaTypes
    {
        get => _agendaTypes;
        set
        {
            _agendaTypes = value;
            OnPropertyChanged(nameof(AgendaTypes));
        }
    }

    private IList<PickListItem> _courseList = new List<PickListItem>();

    public IList<PickListItem> CourseList
    {
        get => _courseList;
        set
        {
            _courseList = value;
            OnPropertyChanged(nameof(CourseList));
        }
    }

    private IList<PickListItem> _gradeList = new List<PickListItem>();

    public IList<PickListItem> GradeList
    {
        get => _gradeList;
        set
        {
            _gradeList = value;
            OnPropertyChanged(nameof(GradeList));
        }
    }

    public bool DateHandlerAlreadyRunning { get; set; }
    public bool AgendaDataApiAlreadyRunning { get; set; }

    private bool _viewSubmissionsOptionVisibility = true;

    public bool ViewSubmissionsOptionVisibility
    {
        get => _viewSubmissionsOptionVisibility;
        set
        {
            _viewSubmissionsOptionVisibility = value;
            OnPropertyChanged(nameof(ViewSubmissionsOptionVisibility));
        }
    }

    private bool _isCreatorVisible = false;

    public bool IsCreatorVisible
    {
        get => _isCreatorVisible;
        set
        {
            _isCreatorVisible = value;
            OnPropertyChanged(nameof(IsCreatorVisible));
        }
    }

    private bool _isTodayView = true;

    public bool IsTodayView
    {
        get => _isTodayView;
        set
        {
            _isTodayView = value;
            OnPropertyChanged(nameof(IsTodayView));
        }
    }

    private string _buttonTitle;

    public string ButtonTitle
    {
        get => _buttonTitle;
        set
        {
            _buttonTitle = value;
            OnPropertyChanged(nameof(ButtonTitle));
        }
    }

    private bool _courseListVisibility = false;

    public bool CourseListVisibility
    {
        get => _courseListVisibility;
        set
        {
            _courseListVisibility = value;
            OnPropertyChanged(nameof(CourseListVisibility));
        }
    }

    private CurriculumView _selectedCourse = new();

    public CurriculumView SelectedCourse
    {
        get => _selectedCourse;
        set
        {
            _selectedCourse = value;
            OnPropertyChanged(nameof(SelectedCourse));
        }
    }

    private string _coursePickerPlaceHolder;

    public string CoursePickerPlaceHolder
    {
        get => _coursePickerPlaceHolder;
        set
        {
            _coursePickerPlaceHolder = value;
            OnPropertyChanged(nameof(CoursePickerPlaceHolder));
        }
    }

    private bool _noDataExist = false;

    public bool NoDataExist
    {
        get => _noDataExist;
        set
        {
            _noDataExist = value;
            OnPropertyChanged(nameof(NoDataExist));
        }
    }

    private string _selectedAgendaForText;

    public string SelectedAgendaForText
    {
        get => _selectedAgendaForText;
        set
        {
            _selectedAgendaForText = value;
            OnPropertyChanged(nameof(SelectedAgendaForText));
        }
    }

    private bool _isSearchVisible = false;

    public bool IsSearchVisible
    {
        get => _isSearchVisible;
        set
        {
            _isSearchVisible = value;
            OnPropertyChanged(nameof(IsSearchVisible));
        }
    }

    private string _searchLabel;

    public string SearchLabel
    {
        get => _searchLabel;
        set
        {
            _searchLabel = value;
            OnPropertyChanged(nameof(SearchLabel));
        }
    }

    private int? _courseListViewHeight;

    public int? CourseListViewHeight
    {
        get => _courseListViewHeight;
        set
        {
            _courseListViewHeight = value;
            OnPropertyChanged(nameof(CourseListViewHeight));
        }
    }

    private IEnumerable<CurriculumView> _courseListforSearch;

    public IEnumerable<CurriculumView> CourseListforSearch
    {
        get => _courseListforSearch;
        set
        {
            _courseListforSearch = value;
            OnPropertyChanged(nameof(CourseListforSearch));
        }
    }

    private bool _noDataFound = false;

    public bool NoDataFound
    {
        get => _noDataFound;
        set
        {
            _noDataFound = value;
            OnPropertyChanged(nameof(NoDataFound));
        }
    }

    private bool _loadFilterPanelList;

    public bool LoadFilterPanelList
    {
        get => _loadFilterPanelList;
        set
        {
            _loadFilterPanelList = value;
            OnPropertyChanged(nameof(LoadFilterPanelList));
        }
    }

    private string _cacheKeyPrefix;

    public string CacheKeyPrefix
    {
        get => _cacheKeyPrefix;
        set
        {
            _cacheKeyPrefix = value;
            OnPropertyChanged(nameof(CacheKeyPrefix));
        }
    }

    private bool _isQuickPostEnabled = false;

    public bool IsQuickPostEnabled
    {
        get => _isQuickPostEnabled;
        set
        {
            _isQuickPostEnabled = value;
            OnPropertyChanged(nameof(IsQuickPostEnabled));
        }
    }

    private const string _PARAMETER_DATE_FORMAT = "yyyy-MMM-dd";
    private IEnumerable<BindableAgendaWeeklyGroupView> _weeklyPlanDataList;

    public IEnumerable<BindableAgendaWeeklyGroupView> WeeklyPlanDataList
    {
        get => _weeklyPlanDataList;
        set
        {
            _weeklyPlanDataList = value;
            OnPropertyChanged(nameof(WeeklyPlanDataList));
        }
    }

    private BindableAgendaWeeklyGroupView _weeklySelectedQuickPostAgenda;

    public BindableAgendaWeeklyGroupView WeeklySelectedQuickPostAgenda
    {
        get => _weeklySelectedQuickPostAgenda;
        set
        {
            _weeklySelectedQuickPostAgenda = value;
            OnPropertyChanged(nameof(WeeklySelectedQuickPostAgenda));
        }
    }

    private string _weeklyPlanHeaderText;

    public string WeeklyPlanHeaderText
    {
        get => _weeklyPlanHeaderText;
        set
        {
            _weeklyPlanHeaderText = value;
            OnPropertyChanged(nameof(WeeklyPlanHeaderText));
        }
    }

    private bool _isWeeklyPlanVisible = false;

    public bool IsWeeklyPlanVisible
    {
        get => _isWeeklyPlanVisible;
        set
        {
            _isWeeklyPlanVisible = value;
            OnPropertyChanged(nameof(IsWeeklyPlanVisible));
        }
    }

    private bool _isAgendaListVisible = true;

    public bool IsAgendaListVisible
    {
        get => _isAgendaListVisible;
        set
        {
            _isAgendaListVisible = value;
            OnPropertyChanged(nameof(IsAgendaListVisible));
        }
    }

    private bool _isWeeklyNoRecordMsg = false;

    public bool IsWeeklyNoRecordMsg
    {
        get => _isWeeklyNoRecordMsg;
        set
        {
            _isWeeklyNoRecordMsg = value;
            OnPropertyChanged(nameof(IsWeeklyNoRecordMsg));
        }
    }

    #endregion

    public CalendarForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
        MessagingCenter.Subscribe<string>(this, "NavigateToCalendar", async (res) =>
        {
            InitializePage();
            var defaultDate = GetCurrentWeekStartDate();
            WeekDate = defaultDate.Date;
            IsTodayView = true;
            IsWeeklyView = CheckIsWeeklyView(WeekDate);
            AppSettings.Current.RefreshNotificationCenterData = true;
            //NotificationCenterForm notificationForm = new NotificationCenterForm();
            //notificationForm.GetNotificationData();
            await ClearAllAgendaCacheData();
            await GetWeekData(WeekDate, DateTime.Today);
        });
        //MessagingCenter.Subscribe<string>("", "ListViewRightSwipeCalendar", async (arg) =>
        //{
        //    await SideMenuClicked();
        //    MessagingCenter.Unsubscribe<string>("", "ListViewRightSwipeCalendar");

        //});
        MessagingCenter.Subscribe<string>("", "ListViewRightSwipeCalendarSubscribe", (arg) =>
        {
            MessagingCenter.Subscribe<string>("", "ListViewRightSwipeCalendar", async (argu) =>
            {
                //await SideMenuClicked();
            });
        });
        MessagingCenter.Subscribe<BindableAgendaView>(this, "SubmissionMessage", async (submission) =>
        {
            if (submission != null)
            {
                AppSettings.Current.RefreshNotificationCenterData = true;
                // NotificationCenterForm notificationForm = new NotificationCenterForm();
                // notificationForm.GetNotificationData();
                await ClearAllAgendaCacheData();
                await GetAgendaListByStudent(true);
            }
        });
        MessagingCenter.Subscribe<string>(this, "LoadWeeklyPlan", async (args) => { WeeklyPlanClickedMethod(); });
    }

    #region Private methods

    private async void InitializePage()
    {
        FormTitle = TextResource.CalendarPageHomeWorkTitle;
        var defaultDate = GetCurrentWeekStartDate();
        FromDate = FromDate == DateTime.MinValue ? defaultDate : FromDate;
        ToDate = ToDate == DateTime.MinValue ? defaultDate : ToDate;
        MenuVisible = true;
        IsWeeklyView = true;

        WeekDate = defaultDate.Date;

        ListTappedCommand = new Command<BindableAgendaView>(ListViewTapped);
        FilterClickCommand = new Command(FilterClicked);
        AttachmentClickCommand = new Command<BindableAgendaView>(AttachmentClicked);
        DonwloadTappedCommand = new Command(DownloadClicked);
        AttachmentListTappedCommand = new Command(AttachmentListClicked);
        LinksClickCommand = new Command<BindableAgendaView>(LinksClicked);
        WebsiteLinksTappedCommand = new Command<string>(WebsiteLinkClicked);
        PreviousClickCommand = new Command(PreviousClicked);
        NextClickCommand = new Command(NextClicked);
        WeekDateSelectionCommand = new Command(WeekDateSelectionChanged);
        OpenCalendarCommand = new Command(OpenMonthlyCalendar);
        WeeklyPlanClickCommand = new Command(WeeklyPlanClicked);
        MonthlyDateChosenCommand = new Command(MonthlyDateChosen);
        CalendarPageTitle = PageTitle;
        ArrowClickedCommand = new Command<BindableAgendaView>(ArrowClicked);
        NewPostClickedCommand = new Command(NewPostClicked);
        ViewSubmissionsClickedCommand = new Command(ViewSubmissionsClicked);
        CurrentWeekClickedCommand = new Command(CurrentWeekClicked);
        CourseSelectionCommand = new Command(CourseSelection);
        CourseListTappedCommand = new Command<CurriculumView>(CourseListTapped);
        AgendaForChangedCommand = new Command<CurriculumView>(AgendaForChanged);
        QuickPostClickedCommand = new Command(QuickPostClickedMethod);
        AddPostPopupClickedCommand = new Command(AddPostPopupClickedMethod);
        WeeklyPlanClickedCommand = new Command(WeeklyPlanClickedMethod);
        WeeklyExpandCollapseClickCommand = new Command<BindableAgendaWeeklyGroupView>(WeeklyExpandCollapseClicked);
        WeeklyListTappedCommand = new Command<BindableAgendaWeeklyGroupView>(WeeklyListViewTappedMethod);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
    }

    private async void WeeklyPlanClickedMethod()
    {
        try
        {
            WeeklyPlanHeaderText = string.Empty;
            var weekStartDate = AgendaData.WeekStartDate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
            WeeklyPlanDataList =
                await ApiHelper.GetObjectList<BindableAgendaWeeklyGroupView>(
                    string.Format(TextResource.GetAgendaWeeklyPlanDataApi, weekStartDate, "", "", ""));
            var distinctWeeks = WeeklyPlanDataList.Select(x => new { x.WeekStartDate, x.WeekEndDate }).Distinct()
                .ToList();
            foreach (var week in distinctWeeks)
            {
                var startDate = week.WeekStartDate.ToString(StringEnum.GetStringValue(DateFormats.Default));
                var endDate = week.WeekEndDate.ToString(StringEnum.GetStringValue(DateFormats.Default));
                WeeklyPlanHeaderText = startDate + " -" + endDate;
            }

            IsWeeklyNoRecordMsg = WeeklyPlanDataList.Count() <= 0;
            IsWeeklyPlanVisible = WeeklyPlanDataList.Count() > 0;
            IsSearchVisible = false;
            IsAgendaListVisible = false;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private void WeeklyExpandCollapseClicked(BindableAgendaWeeklyGroupView bindableAgendaWeekly)
    {
        try
        {
            if (bindableAgendaWeekly != null)
            {
                foreach (var item in WeeklyPlanDataList.ToList())
                    if (item != null)
                    {
                        if (item.AgendaWeeklyGroupId == bindableAgendaWeekly.AgendaWeeklyGroupId)
                        {
                            item.WeeklyPostDetailsVisibility = !item.WeeklyPostDetailsVisibility;
                            item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png")
                                ? "dropdown_gray.png"
                                : "uparrow_gray.png";
                        }
                        else
                        {
                            item.WeeklyPostDetailsVisibility = false;
                            item.ArrowImageSource = "dropdown_gray.png";
                        }
                    }

                MessagingCenter.Send("", "WeeklyExpandCollapse");
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public async void WeeklyListViewTappedMethod(BindableAgendaWeeklyGroupView obj)
    {
        try
        {
            if (obj != null)
            {
                WeeklyPlanDetailFrom weeklyPlanDetail = new(_mapper, _nativeServices, Navigation)
                {
                    SelectedWeeklyAgenda = obj,
                    PageTitle = PageTitle,
                    IsEditButtonVisibleForTeacher = obj.IsEditButtonVisibleForTeacher,
                    WeeklyPlanHeaderText = WeeklyPlanHeaderText
                };
                WeeklyPlanAgendaDetailPage weeklyPlanAgendaDetailPage = new ()
                {
                    BindingContext = weeklyPlanDetail
                };
                await Navigation.PushAsync(weeklyPlanAgendaDetailPage);
                WeeklySelectedQuickPostAgenda = null;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    private async void AddPostPopupClickedMethod(object obj)
    {
        try
        {
            var addNewPostPopup = new AddNewPostPopup()
            {
                BindingContext = this
            };
            SetPopupInstance(addNewPostPopup);
            Application.Current.MainPage.ShowPopup(addNewPostPopup);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void QuickPostClickedMethod(object obj)
    {
        try
        {
            QuickPostForm quickPost = new(_mapper, _nativeServices, Navigation)
            {
                PageTitle = "Quick Post",
                MenuVisible = false,
                BackVisible = true,
                IsPopUpPage = false
            };
            await quickPost.GetQuickPostData();
            QuickPostPage quickPostPage = new QuickPostPage()
            {
                BindingContext = quickPost
            };
            await Navigation.PushAsync(quickPostPage);
            AppSettings.Current.CurrentPopup?.Close();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void CurrentWeekClicked(object obj)
    {
        IsWeeklyPlanVisible = false;
        IsSearchVisible = true;
        IsAgendaListVisible = true;
        IsWeeklyNoRecordMsg = false;
        IsTodayView = false;
        IsWeeklyView = CheckIsWeeklyView(WeekDate);
        await GetWeekData(WeekDate);
    }

    private async void MonthlyDateChosen(object obj)
    {
        try
        {
            if (DateHandlerAlreadyRunning) return;
            DateHandlerAlreadyRunning = true;
            // if (PopupNavigation.Instance.PopupStack.Any())
            //     await PopupNavigation.Instance.PopAllAsync();

            await ApiHelper.ShowProcessingIndicatorPopup();
            WeekDate = HelperMethods.GetWeekStartDate(SelectedDate);
            var isWeeklyView = CheckIsWeeklyView(WeekDate);
            if (isWeeklyView)
            {
                IsTodayView = true;
                IsWeeklyView = isWeeklyView;
                WeekList = HelperMethods.GetWeekList(WeekDate);
                WeekList = new ObservableCollection<Week>(WeekList.Select(c =>
                {
                    c.IsSelected = false;
                    return c;
                }).ToList());
                SelectedWeekDate = WeekList.Where(x => x.Date.Date == SelectedDate.Date).FirstOrDefault();
                SelectedWeekDate.IsSelected = true;
                WeekList[WeekList.IndexOf(SelectedWeekDate)].IsSelected = true;
                FromDate = WeekDate;
                ToDate = FromDate.AddDays(6);
                CacheKeyPrefix = GetCacheKey("getcalendaragendalist_week", WeekDate.Date);
                await PreGetAgendaListByStudent(true, SelectedDate);
                SelectedWeekDate = null;
            }

            await ApiHelper.HideProcessingIndicatorPopup();
            DateHandlerAlreadyRunning = false;
        }
        catch (Exception ex)
        {
            DateHandlerAlreadyRunning = false;
            await ApiHelper.HideProcessingIndicatorPopup();
            //Crashes.TrackError(ex);
        }
    }

    private async void WeeklyPlanClicked(object obj)
    {
        // Func<PopupPage, bool> predicate = x => x.GetType() == typeof(AgendaFilterPage);
        // if (PopupNavigation.Instance.PopupStack.Any(predicate))
        // {
        //     var popupPage = PopupNavigation.Instance.PopupStack.FirstOrDefault(predicate);
        //     await PopupNavigation.Instance.RemovePageAsync(popupPage);
        // }
        WeekDate = GetCurrentWeekStartDate();
        PageTitle = CalendarPageTitle;
        MenuVisible = true;
        BackVisible = false;
        IsWeeklyView = CheckIsWeeklyView(WeekDate);
        CacheKeyPrefix = GetCacheKey("getcalendaragendalist_wp", WeekDate);
        await GetWeekData(WeekDate);
    }

    private async void WeekDateSelectionChanged(object obj)
    {
        if (obj != null)
            try
            {
                IsWeeklyPlanVisible = false;
                IsSearchVisible = true;
                IsAgendaListVisible = true;
                IsWeeklyNoRecordMsg = false;
                SelectedWeekDate = (Week)obj;
                IsTodayView = true;
                SelectedWeekDate.IsSelected = true;
                WeekList = new ObservableCollection<Week>(WeekList.Select(c =>
                {
                    c.IsSelected = false;
                    return c;
                }).ToList());
                WeekList[WeekList.IndexOf(SelectedWeekDate)].IsSelected = true;
                FromDate = ToDate = SelectedWeekDate.Date;
                CacheKeyPrefix = GetCacheKey("getcalendaragendalist_td", FromDate);
                await GetAgendaListByStudent(false);
                SelectedWeekDate = null;
            }

            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
    }

    private async Task PresetTodaysWeekDay()
    {
        foreach (var week in WeekList)
            if (week.Date == DateTime.Today)
            {
                SelectedWeekDate = week;
                SelectedWeekDate.IsSelected = true;
                IsTodayView = true;
            }
    }

    private async void GetTodayAgendaList()
    {
        try
        {
            WeekList = HelperMethods.GetWeekList(WeekDate);
            var isTodayWithinWeek = false;
            foreach (var item in WeekList)
                if (item.Date == DateTime.Today)
                    isTodayWithinWeek = true;
            if (isTodayWithinWeek)
                FromDate = ToDate = SelectedDate = DateTime.Today;
            else
                FromDate = ToDate = SelectedDate = AppSettings.Current.SchoolNextWorkingDate.Date;
            SelectedWeekDate.IsSelected = true;
            WeekList = new ObservableCollection<Week>(WeekList.Select(c =>
            {
                c.IsSelected = false;
                return c;
            }).ToList());
            IsTodayView = true;
            CacheKeyPrefix = GetCacheKey("getcalendaragendalist_td", FromDate);
            await GetAgendaListByStudent(true);
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    private async void OpenMonthlyCalendar(object obj)
    {
        try
        {
            AppSettings.Current.IsMonthlyView = false;
            if (SelectedWeekDate != null && SelectedWeekDate.Date != DateTime.MinValue &&
                WeekList.Where(x => x.Date.Date == SelectedWeekDate.Date.Date).Count() > 0)
            {
                FromDate = ToDate = SelectedDate = SelectedWeekDate.Date;
                WeekList = new ObservableCollection<Week>(WeekList.Select(c =>
                {
                    c.IsSelected = false;
                    return c;
                }).ToList());
                WeekList[WeekList.IndexOf(SelectedWeekDate)].IsSelected = true;
            }
            else
            {
                FromDate = HelperMethods.GetWeekStartDate(WeekDate);
            }
            //await PopupNavigation.Instance.PushAsync(new CustomCalendar(this), true);
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    private async void NextClicked(object obj)
    {
        WeeklyPlanDataList = new List<BindableAgendaWeeklyGroupView>();
        IsWeeklyPlanVisible = false;
        IsSearchVisible = true;
        IsAgendaListVisible = true;
        IsWeeklyNoRecordMsg = false;
        var isWeeklyView = CheckIsWeeklyView(WeekDate.AddDays(7));
        if (isWeeklyView)
        {
            IsWeeklyView = isWeeklyView;
            WeekDate = WeekDate.AddDays(7);
            SelectedWeekDate = new Week();
            if (WeekTitleText.Contains(TextResource.ThisWeekText)) IsTodayView = true;
            await GetWeekData(WeekDate);
        }
        else
        {
            var defaultDate = GetCurrentWeekStartDate();
            FromDate = defaultDate;
            ToDate = defaultDate;
            OpenFilterPanel();
        }
    }

    private async void PreviousClicked(object obj)
    {
        WeeklyPlanDataList = new List<BindableAgendaWeeklyGroupView>();
        IsWeeklyPlanVisible = false;
        IsSearchVisible = true;
        IsAgendaListVisible = true;
        IsWeeklyNoRecordMsg = false;
        var isWeeklyView = CheckIsWeeklyView(WeekDate.AddDays(-7));
        if (isWeeklyView)
        {
            IsWeeklyView = isWeeklyView;
            WeekDate = WeekDate.AddDays(-7);
            SelectedWeekDate = new Week();
            if (WeekTitleText.Contains(TextResource.ThisWeekText)) IsTodayView = true;
            await GetWeekData(WeekDate);
        }
        else
        {
            var defaultDate = GetCurrentWeekStartDate();
            FromDate = defaultDate;
            ToDate = defaultDate;
            OpenFilterPanel();
        }
    }

    private async Task GetWeekData(DateTime weekDate, DateTime? defaultAgendaListDate = null)
    {
        try
        {
            WeekList = HelperMethods.GetWeekList(weekDate);
            FromDate = HelperMethods.GetWeekStartDate(weekDate);
            ToDate = FromDate.AddDays(6);
            CacheKeyPrefix = GetCacheKey("getcalendaragendalist_week", WeekDate.Date);
            await PreGetAgendaListByStudent(true, defaultAgendaListDate);
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    private bool CheckIsWeeklyView(DateTime weekDate)
    {
        var weekStartDate = HelperMethods.GetWeekStartDate(weekDate.Date);
        var currentWeekStartDate = HelperMethods.GetWeekStartDate(DateTime.Now.Date);
        var weekText = string.Empty;
        if (weekStartDate == currentWeekStartDate)
        {
            weekText = TextResource.ThisWeekText;
        }
        else if (weekStartDate.Date >= currentWeekStartDate.AddDays(7).Date)
        {
            weekText = TextResource.NextWeekText;
            IsTodayView = false;
        }
        else if (weekStartDate.Date <= currentWeekStartDate.Date.AddDays(-7))
        {
            weekText = TextResource.PrevWeekText;
            IsTodayView = false;
        }

        WeekTitleText = weekStartDate.ToString(TextResource.WeekDateFormatKey) + " - " +
                        HelperMethods.GetWeekEndDate(weekDate.Date).ToString(TextResource.WeekDateFormatKey) + "  " +
                        weekText;
        return true;
    }

    private async void AttachmentClicked(BindableAgendaView sender)
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

    private async void LinksClicked(BindableAgendaView sender)
    {
        WebsiteLinksPopupForm websiteLinksPopupForm = new WebsiteLinksPopupForm(_mapper, _nativeServices, Navigation)
        {
            SelectedWebsiteLinks = sender.WebsiteLinks
        };
        var websiteLinksPopup = new WebsiteLinksPopup()
        {
            BindingContext = websiteLinksPopupForm
        };
        SetPopupInstance(websiteLinksPopup);
        Application.Current.MainPage.ShowPopup(websiteLinksPopup);
    }

    private void WebsiteLinkClicked(string sender)
    {
        Launcher.OpenAsync(sender);
        //Device.OpenUri(new Uri(sender));
    }

    private void FilterClicked(object obj)
    {
        OpenFilterPanel();
    }

    private async void OpenFilterPanel()
    {
        try
        {
            CalendarPageTitle = PageTitle;
            PageTitle = TextResource.FilterAgendaTitle;
            MenuVisible = false;
            BackVisible = true;
            IsPopUpPage = true;
            SelectedAgendaType = SelectedAgendaType != null && !string.IsNullOrEmpty(SelectedAgendaType.ItemName)
                ? SelectedAgendaType
                : AgendaTypeList != null && AgendaTypeList.Count > 0
                    ? AgendaTypeList.FirstOrDefault()
                    : new ExtPickListItem();
            //await PopupNavigation.Instance.PushAsync(new AgendaFilterPage(this), true);
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    public async void ListViewTapped(BindableAgendaView obj)
    {
        try
        {
            if (obj != null)
            {
                AgendaDetailForm agendaDetailForm = new(_mapper, _nativeServices, Navigation)
                {
                    SelectedAgenda = obj,
                    SelectedDate = DateTime.Now,
                    AttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(obj.AttachmentList),
                    AttachmentListViewHeight = obj.AttachmentCount * 30,
                    LinkListViewHeight = obj.WebsiteLinksCount * 40,
                    PageTitle = obj.TypeTitle,
                };
                agendaDetailForm.SetEditOptionVisibility();
                agendaDetailForm.IsCreatorVisible = IsCreatorVisible;
                agendaDetailForm.SubmissionComments = agendaDetailForm.SelectedAgenda.StudentComments;
                agendaDetailForm.IsSubmissionAllowed = !AppSettings.Current.IsTeacher && obj.IsStudentSubmissionAllowed;
                if (AppSettings.Current.IsTeacher)
                {
                    agendaDetailForm.IsViewSubmissionVisible = AgendaData.DisplaySetting.EnableStudentSubmissions;
                    agendaDetailForm.IsViewAgendaStudentsVisible = !agendaDetailForm.IsViewSubmissionVisible && AgendaData.DisplaySetting.IsEnablePerStudent;
                }
                try
                {
                    agendaDetailForm.TotalCount = agendaDetailForm.SelectedAgenda.AgendaStudentCount;
                    agendaDetailForm.SubmissionCount = agendaDetailForm.SelectedAgenda.AgendaClassStudents
                        .Where(x => x.StudentSubmittedFilesList.Any(y => !string.IsNullOrEmpty(y.AttachmentUrl)))
                        .ToList().Count();
                    agendaDetailForm.SubmissionAttachmentFiles =
                        _mapper.Map<ObservableCollection<BindableAttachmentFileView>>(agendaDetailForm.SelectedAgenda
                            .StudentSubmittedFilesList);
                    agendaDetailForm.SubmissionListViewHeight = agendaDetailForm.SubmissionAttachmentFiles.Count * 50;
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex);
                }

                AgendaDetail agendaDetail = new ()
                {
                    BindingContext = agendaDetailForm
                };
                await Navigation.PushAsync(agendaDetail);
                SelectedAgenda = null;
                WeeklySelectedAgenda = null;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    public async Task PreGetAgendaListByStudent(bool isRefresh = false, DateTime? defaultAgendaListDate = null)
    {
        if (!AppSettings.Current.IsTeacher)
        {
            if (AppSettings.Current.SelectedStudent != null &&
                !string.IsNullOrEmpty(AppSettings.Current.SelectedStudent.ItemId))
                await GetAgendaListByStudent(isRefresh, defaultAgendaListDate);
        }
        else
        {
            await GetAgendaListByStudent(isRefresh, defaultAgendaListDate);
        }
    }

    public async Task GetAgendaListByStudent(bool isRefresh = false, DateTime? defaultAgendaListDate = null)
    {
        try
        {
            if (AgendaDataApiAlreadyRunning) return;
            AgendaDataApiAlreadyRunning = true;

            //bool loadFilterPanelList = !AgendaTypeList.Any();
            var agendaTypeId = !IsWeeklyView ? AgendaTypeId : null;
            _toDateValue = ToDate == DateTime.MinValue ? null : ToDate.ToString(new CultureInfo("en-US"));
            _showReminderDataValue = ShowReminderData ? ShowReminderData.ToString() : null;

            var apiUrl = TextResource.CalendarAgendaApiUrl + "?fromDate=" +
                         FromDate.ToString(_PARAMETER_DATE_FORMAT, new CultureInfo("en-US"))
                         + "&toDate=" + _toDateValue + "&agendaTypeId=" + agendaTypeId + "&studentId=" +
                         AppSettings.Current.SelectedStudent.ItemId
                         + "&curriculumId=null&postOwnerId=null" + "&loadFilterPanelList=" + LoadFilterPanelList +
                         "&showReminderData=" + _showReminderDataValue;

            if (defaultAgendaListDate.HasValue)
                apiUrl += "&defaultDate=" +
                          defaultAgendaListDate.Value.ToString(_PARAMETER_DATE_FORMAT, new CultureInfo("en-US"));
            else
                apiUrl += "&defaultDate=";
            AgendaData = await ApiHelper.GetObject<AgendaViewModel>(apiUrl, cacheKeyPrefix: CacheKeyPrefix,
                cacheType: ApiHelper.CacheTypeParam.LoadFromCache);

            IsCreatorVisible = AgendaData.DisplaySetting.ShowWeeklyCreator;
            IsQuickPostEnabled = AgendaData.DisplaySetting.EnableQuickPost;

            AgendaList = _mapper.Map<ObservableCollection<BindableAgendaView>>(AgendaData.AgendaList);
            if (defaultAgendaListDate.HasValue)
                DefaultAgendaList = _mapper.Map<ObservableCollection<BindableAgendaView>>(AgendaData.DefaultAgendaList);
            else
                DefaultAgendaList = _mapper.Map<ObservableCollection<BindableAgendaView>>(AgendaData.AgendaList);

            CourseListVisibility = false;
            NoDataFound = false;
            SelectedCourse = new CurriculumView();
            IsSearchVisible = DefaultAgendaList.Count > 0 ? true : false;
            CourseListVisibility = false;
            NoDataFound = false;
            SelectedCourse = new CurriculumView();

            var tempCourseList = new List<CurriculumView>
            {
                new()
                {
                    CurriculumName = "All Courses",
                    CurriculumId = 000
                }
            };
            if (LoadFilterPanelList)
            {
                AgendaTypeList = AgendaData.AgendaTypes;
                tempCourseList.AddRange(AgendaData.CourseList);
                CourseListforSearch = tempCourseList;
                LoadFilterPanelList = false;
            }

            IsNoRecordMsg = DefaultAgendaList.Count > 0 ? false : true;

            if (DefaultAgendaList != null && DefaultAgendaList.Count > 0)
            {
                var grpList = from agenda in DefaultAgendaList
                    group agenda by agenda.TypeTitle
                    into agendaGroup
                    select new Grouping<string, BindableAgendaView>(agendaGroup.Key,
                        agendaGroup.Where(x => x.TypeTitle == agendaGroup.Key).FirstOrDefault(), agendaGroup);
                GroupedAgendaList = new ObservableCollection<Grouping<string, BindableAgendaView>>(grpList);
                if (GroupedAgendaList != null)
                    foreach (var item in GroupedAgendaList)
                        if (item != null)
                            foreach (var listItem in item)
                                if (listItem != null)
                                {
                                    if (string.IsNullOrEmpty(item.FirstOrDefault().CurriculumName))
                                        listItem.AgendaDetailsVisibility = true;
                                    else
                                        listItem.AgendaDetailsVisibility = false;
                                }
            }
            else
            {
                GroupedAgendaList = new ObservableCollection<Grouping<string, BindableAgendaView>>();
            }

            CourseListViewHeight = CourseListforSearch?.Count() * 40;

            var weeklyTypeColor = AgendaList.Where(x => x.TypeTitle.ToLower().Contains("weekly")).FirstOrDefault()
                ?.WorkTypeColor;

            if (isRefresh)
            {
                var dateColorDictionary = AgendaList.GroupBy(d => d.DueDate)
                    .ToDictionary(
                        grp => grp.Key,
                        dates => dates.Select(d => d.WorkTypeColor).Distinct().ToList()
                    );

                WeekList.ToList().ForEach((day) =>
                {
                    day.Colors = new ObservableCollection<string>(
                        dateColorDictionary
                            .Where(x => DateTime.TryParse(x.Key, out _))
                            .Where(x => DateTime.Parse(x.Key).Date == day.Date.Date)
                            .SelectMany(y => y.Value)
                            .ToList()
                    );

                    if (!string.IsNullOrEmpty(weeklyTypeColor) &&
                        !day.Colors.Contains(weeklyTypeColor) &&
                        day.Date >= AgendaData.WeekStartDate.Date &&
                        day.Date <= AgendaData.WeekEndDate.Date)
                        day.Colors.Add(weeklyTypeColor);
                });
            }

            AgendaDataApiAlreadyRunning = false;
            if (!string.IsNullOrEmpty(NotificationItemId))
            {
                await OnNotificationClick();
                var agendaView = AgendaListForNotificationClick
                    .Where(x => x.AgendaId == Convert.ToInt32(NotificationItemId)).FirstOrDefault();
                if (agendaView != null)
                    ListViewTapped(agendaView);
                NotificationItemId = null;
            }
        }
        catch (Exception ex)
        {
            AgendaDataApiAlreadyRunning = false;
            HelperMethods.DisplayException(ex, TextResource.CalendarPageTitle);
            return;
        }
    }

    public async Task GetAgendaColorList(DateTime fromDate, bool isWeekly, bool isloader = true)
    {
        try
        {
            var agendaViews = new List<BindableAgendaView>();
            var calendarViewModel = new CalendarViewModel();

            calendarViewModel = await ApiHelper.GetObject<CalendarViewModel>(string.Format(
                TextResource.GetAgendaColorApiUrl,
                new DateTime(fromDate.Year, fromDate.Month, 1).ToPickerDateFormatted(), "M",
                AppSettings.Current.SelectedStudent.ItemId));
            if (calendarViewModel != null)
                agendaViews = _mapper.Map<List<BindableAgendaView>>(calendarViewModel.AgendaData.ToList());
            var dateColorDictionary = agendaViews.GroupBy(d => d.AgendaDate).ToDictionary(grp => grp.Key,
                dates => dates.Select(d => d.WorkTypeColor).Distinct().ToList()
            );
            if (!isWeekly)
                ColorAgendaList = dateColorDictionary.Select(x => new ColorData
                {
                    Color = x.Value,
                    Date = x.Key.Value
                }).ToList();
            if (isWeekly)
                WeekList.ToList().ForEach((week) =>
                {
                    week.Colors = new ObservableCollection<string>(dateColorDictionary
                        .Where(x => x.Key == week.Date).SelectMany(y => y.Value).ToList());
                });
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    private async void AttachmentListClicked(object obj)
    {
        if (obj != null)
            try
            {
                await HelperMethods.OpenFileForPreview(obj.ToString(), _nativeServices);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void DownloadClicked(object obj)
    {
        if (obj != null)
            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                    await HelperMethods.OpenFileForPreview(obj.ToString(), _nativeServices);
                else
                    await HelperMethods.DownloadFile(obj.ToString());
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    public override async void GetStudentData()
    {
        if (!string.IsNullOrEmpty(NotificationItemId))
            await ClearAllAgendaCacheData();
        ButtonTitle = "View this week" + " " + PageTitle;
        SearchLabel = "Search" + " " + PageTitle;
        IsTodayView = true;
        LoadFilterPanelList = true;
        var defaultDate = GetCurrentWeekStartDate();
        WeekDate = defaultDate.Date;
        base.GetStudentData();
        IsWeeklyView = CheckIsWeeklyView(WeekDate);
        //await PresetTodaysWeekDay();
        //GetTodayAgendaList();
        await GetWeekData(WeekDate, DateTime.Today);
    }

    private DateTime GetCurrentWeekStartDate()
    {
        return AppSettings.Current.SchoolWeekStartDate == DateTime.MinValue
            ? HelperMethods.GetWeekStartDate(DateTime.Now)
            : AppSettings.Current.SchoolWeekStartDate;
    }

    public override async void BackClicked(object obj)
    {
        base.BackClicked(obj);
        PageTitle = CalendarPageTitle;
        MenuVisible = true;
        BackVisible = false;
    }

    public async Task GetDefaultData_Unused_ToDelete()
    {
        await ApiHelper.ShowProcessingIndicatorPopup();
        WeekList = HelperMethods.GetWeekList(SelectedDate);
        CheckIsWeeklyView(SelectedDate);
        WeekDate = SelectedDate;
        await GetAgendaColorList(SelectedDate.Date, true, false);
        SelectedWeekDate = WeekList.Where(x => x.Date.Date == SelectedDate.Date).FirstOrDefault();
        FromDate = ToDate = SelectedWeekDate.Date;
        SelectedWeekDate.IsSelected = true;

        //IsDefaultSelection = false;

        WeekList = new ObservableCollection<Week>(WeekList.Select(c =>
        {
            c.IsSelected = false;
            return c;
        }).ToList());
        WeekList[WeekList.IndexOf(SelectedWeekDate)].IsSelected = true;
        await GetAgendaListByStudent();
        await ApiHelper.HideProcessingIndicatorPopup();
    }

    private async void ArrowClicked(BindableAgendaView bindableAgendaView)
    {
        if (bindableAgendaView != null)
            foreach (var item in GroupedAgendaList.ToList())
                if (item != null)
                    foreach (var listItem in item)
                        if (listItem != null)
                        {
                            if (listItem.AgendaId == bindableAgendaView.AgendaId)
                            {
                                listItem.AgendaDetailsVisibility = !listItem.AgendaDetailsVisibility;
                                listItem.ArrowImageSource = listItem.ArrowImageSource.Equals("uparrow_gray.png")
                                    ? "dropdown_gray.png"
                                    : "uparrow_gray.png";
                            }
                            else
                            {
                                listItem.AgendaDetailsVisibility = false;
                                listItem.ArrowImageSource = "dropdown_gray.png";
                            }
                        }

        MessagingCenter.Send("", "ExpandCollapse");
    }

    private void CourseSelection()
    {
        CourseListVisibility = !CourseListVisibility;
    }

    private void CourseListTapped(CurriculumView pickListItem)
    {
        try
        {
            if (pickListItem != null)
            {
                SelectedCourse = pickListItem;
                CourseListVisibility = false;
                if (SelectedCourse.CurriculumId == 0)
                    SearchAgendaList = DefaultAgendaList;
                else
                    SearchAgendaList = new ObservableCollection<BindableAgendaView>(DefaultAgendaList
                        .Where(i => i.CurriculumId != null && i.CurriculumId.Equals(SelectedCourse.CurriculumId))
                        .ToList());

                if (SearchAgendaList != null && SearchAgendaList.Count > 0)
                {
                    var grpList = from agenda in SearchAgendaList
                        group agenda by agenda.TypeTitle
                        into agendaGroup
                        select new Grouping<string, BindableAgendaView>(agendaGroup.Key,
                            agendaGroup.Where(x => x.TypeTitle == agendaGroup.Key).FirstOrDefault(), agendaGroup);

                    GroupedAgendaList = new ObservableCollection<Grouping<string, BindableAgendaView>>(grpList);
                }
                else
                {
                    GroupedAgendaList = new ObservableCollection<Grouping<string, BindableAgendaView>>();
                }

                NoDataFound = SearchAgendaList.Count > 0 ? false : true;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void AgendaForChanged(CurriculumView obj)
    {
        if (obj != null) SelectedAgendaForText = obj.CurriculumName;
    }

    private async Task OnNotificationClick()
    {
        try
        {
            if (AgendaDataApiAlreadyRunning)
                return;
            AgendaDataApiAlreadyRunning = true;

            var loadFilterPanelList = !AgendaTypeList.Any();
            var agendaTypeId = !IsWeeklyView ? AgendaTypeId : null;
            _toDateValue = "";
            _showReminderDataValue = ShowReminderData ? ShowReminderData.ToString() : null;

            var agendaData = await ApiHelper.GetObject<AgendaViewModel>(TextResource.CalendarAgendaApiUrl +
                                                                        "?fromDate=" +
                                                                        FromDate.ToString(_PARAMETER_DATE_FORMAT,
                                                                            new CultureInfo("en-US"))
                                                                        + "&toDate=" + _toDateValue +
                                                                        "&defaultDate=null&agendaTypeId=" +
                                                                        agendaTypeId + "&studentId=" +
                                                                        AppSettings.Current.SelectedStudent.ItemId
                                                                        + "&curriculumId=null&postOwnerId=null" +
                                                                        "&loadFilterPanelList=" + loadFilterPanelList +
                                                                        "&showReminderData=" + _showReminderDataValue);
            if (agendaData != null)
                AgendaListForNotificationClick = _mapper.Map<List<BindableAgendaView>>(agendaData.AgendaList);
            AgendaDataApiAlreadyRunning = false;
        }
        catch (Exception ex)
        {
            AgendaDataApiAlreadyRunning = false;
            AgendaListForNotificationClick = new List<BindableAgendaView>();
            HelperMethods.DisplayException(ex, TextResource.FilterAgendaTitle);
        }
    }

    private async void NewPostClicked()
    {
        await AddEditData();
        var addNewPostForm = new AddNewPostForm(_mapper, _nativeServices, Navigation)
        {
            PageTitle = TextResource.NewPostText,
            MenuVisible = false,
            BackVisible = true,
            IsPopUpPage = false
        };
        await addNewPostForm.AddDataSettings(AddPostData);
        AddNewPostPage addNewPostPage = new AddNewPostPage()
        {
            BindingContext = addNewPostForm
        };
        await Navigation.PushAsync(addNewPostPage);
        AppSettings.Current.CurrentPopup?.Close();
    }

    public async Task AddEditData()
    {
        try
        {
            AddPostData = await ApiHelper.GetObject<AgendaEdit>(string.Format(TextResource.GetAddEditAgendaPostApi,
                AppSettings.Current.UserId, null));
            if (AddPostData != null)
            {
                AgendaTypes = AddPostData.AgendaTypeList != null
                    ? AddPostData.AgendaTypeList
                    : new List<CalendarAgendaTypePickListItem>();
                CourseList = AddPostData.CourseList != null && AddPostData.CourseList.Count() > 0
                    ? AddPostData.CourseList
                    : new List<PickListItem>();
                GradeList = AddPostData.GradeList != null && AddPostData.GradeList.Count() > 0
                    ? AddPostData.GradeList
                    : new List<PickListItem>();
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            //Crashes.TrackError(ex);
        }
    }

    private void ViewSubmissionsClicked()
    {
        var viewSubmissionsForm = new ViewSubmissionsForm(_mapper, _nativeServices, Navigation);
        viewSubmissionsForm.BackVisible = true;
        viewSubmissionsForm.IsPopUpPage = false;
        viewSubmissionsForm.PageTitle = TextResource.SubmissionsText;
        viewSubmissionsForm.MenuVisible = false;
        //HostScreen.Router.Navigate.Execute(viewSubmissionsForm).Subscribe(); ;
    }

    private async Task ClearAllAgendaCacheData()
    {
        var allKeys = await ICCacheManager.GetAllKeys();

        foreach (var key in allKeys)
            if (key.StartsWith("getcalendaragendalist"))
                ICCacheManager.InvalidateObject<AgendaViewModel>(key);
    }

    public string GetCacheKey(string prefix, DateTime date)
    {
        return $"{prefix}_{date.ToString("yyyyMMdd")}";
    }

    #endregion
}