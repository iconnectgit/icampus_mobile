using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Android.Content.Res;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.Portal.ViewModels;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Enums;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.UserModules.Appointment;
using iCampus.MobileApp.Forms.UserModules.Attendance;
using iCampus.MobileApp.Forms.UserModules.BooksReservation;
using iCampus.MobileApp.Forms.UserModules.BusTracking;
using iCampus.MobileApp.Forms.UserModules.Calendar;
using iCampus.MobileApp.Forms.UserModules.CampusKey;
using iCampus.MobileApp.Forms.UserModules.Certificates;
using iCampus.MobileApp.Forms.UserModules.ChequeReplacement;
using iCampus.MobileApp.Forms.UserModules.Communication;
using iCampus.MobileApp.Forms.UserModules.Complaints;
using iCampus.MobileApp.Forms.UserModules.Conduct;
using iCampus.MobileApp.Forms.UserModules.ContactUs;
using iCampus.MobileApp.Forms.UserModules.CovidTest;
using iCampus.MobileApp.Forms.UserModules.DailyMarks;
using iCampus.MobileApp.Forms.UserModules.DataCollection;
using iCampus.MobileApp.Forms.UserModules.Event;
using iCampus.MobileApp.Forms.UserModules.Exam;
using iCampus.MobileApp.Forms.UserModules.FinancialStatus;
using iCampus.MobileApp.Forms.UserModules.Library;
using iCampus.MobileApp.Forms.UserModules.Medical;
using iCampus.MobileApp.Forms.UserModules.MessageFromSchool;
using iCampus.MobileApp.Forms.UserModules.MiscellaneousPayment;
using iCampus.MobileApp.Forms.UserModules.News;
using iCampus.MobileApp.Forms.UserModules.NotificationCenter;
using iCampus.MobileApp.Forms.UserModules.OnlineLesson;
using iCampus.MobileApp.Forms.UserModules.OnlinePayment;
using iCampus.MobileApp.Forms.UserModules.QuickPayment;
using iCampus.MobileApp.Forms.UserModules.Registration;
using iCampus.MobileApp.Forms.UserModules.ReportCard;
using iCampus.MobileApp.Forms.UserModules.Resources;
using iCampus.MobileApp.Forms.UserModules.Settings;
using iCampus.MobileApp.Forms.UserModules.Survey;
using iCampus.MobileApp.Forms.UserModules.TeacherEvaluation;
using iCampus.MobileApp.Forms.UserModules.TimeTable;
using iCampus.MobileApp.Helpers.CustomCalendar;
using iCampus.MobileApp.Views;
using iCampus.MobileApp.Views.UserModules;
using iCampus.MobileApp.Views.UserModules.Appointment;
using iCampus.MobileApp.Views.UserModules.Attendance;
using iCampus.MobileApp.Views.UserModules.BooksReservation;
using iCampus.MobileApp.Views.UserModules.BusTracking;
using iCampus.MobileApp.Views.UserModules.CampusKey;
using iCampus.MobileApp.Views.UserModules.Certificates;
using iCampus.MobileApp.Views.UserModules.ChequeReplacement;
using iCampus.MobileApp.Views.UserModules.Communication;
using iCampus.MobileApp.Views.UserModules.Complaints;
using iCampus.MobileApp.Views.UserModules.Conduct;
using iCampus.MobileApp.Views.UserModules.ContactUs;
using iCampus.MobileApp.Views.UserModules.CovidTest;
using iCampus.MobileApp.Views.UserModules.DailyMarks;
using iCampus.MobileApp.Views.UserModules.DataCollection;
using iCampus.MobileApp.Views.UserModules.Event;
using iCampus.MobileApp.Views.UserModules.Exam;
using iCampus.MobileApp.Views.UserModules.FinancialStatus;
using iCampus.MobileApp.Views.UserModules.Library;
using iCampus.MobileApp.Views.UserModules.Medical;
using iCampus.MobileApp.Views.UserModules.MessageFromSchool;
using iCampus.MobileApp.Views.UserModules.MiscellaneousPayment;
using iCampus.MobileApp.Views.UserModules.News;
using iCampus.MobileApp.Views.UserModules.NotificationCenter;
using iCampus.MobileApp.Views.UserModules.OnlineLesson;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using iCampus.MobileApp.Views.UserModules.QuickPayment;
using iCampus.MobileApp.Views.UserModules.Registration;
using iCampus.MobileApp.Views.UserModules.ReportCard;
using iCampus.MobileApp.Views.UserModules.Resources;
using iCampus.MobileApp.Views.UserModules.Settings;
using iCampus.MobileApp.Views.UserModules.Survey;
using iCampus.MobileApp.Views.UserModules.TeacherEvaluation;
using iCampus.MobileApp.Views.UserModules.TimeTable;
using Splat;
using Calendar = iCampus.MobileApp.Views.UserModules.Calendar.Calendar;

namespace iCampus.MobileApp.Forms;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #region Declarations
    protected IMapper _mapper;
    protected INativeServices _nativeServices;
    protected INavigation Navigation;
    public ICommand StudentTapCommand { get; set; }
    public ICommand StudentListTappedCommand { get; set; }
    public ICommand BackClickCommand { get; set; }
    public ICommand CommunicationMessageTypeClickedEvent { get; set; }
    public ICommand CommunicationHeaderDeleteTappedCommand { get; set; }
    public ICommand CommunicationHeaderBackArrowTappedCommand { get; set; }
    public ICommand StudentFrameTapCommand { get; set; }

    public ICommand AttendanceSaveTapCommand { get; set; }
    public ICommand SideMenuClickCommand { get; set; }
    public ICommand MenuListTappedCommand { get; set; }
    public ICommand MenuCloseClickCommand { get; set; }
    public ICommand LogoutClickCommand { get; set; }

    public ICommand RightSideMenuClickCommand { get; set; }

    public ICommand BeamMenuClickCommand { get; set; }
    public ICommand BeamHeaderMessageIconClickCommand { get; set; }
    public ICommand BeamHeaderNotificationIconClickCommand { get; set; }
    public ICommand BeamHeaderStudentImageClickCommand { get; set; }
    public ICommand ListTappedCommandForBottomList { get; set; }
    public ICommand FooterMenuClickCommand { get; set; }

    #endregion

    #region Properties

    private string _urlPathSegment;
    public string UrlPathSegment
    {
        get => _urlPathSegment;
        protected set
        {
            _urlPathSegment = value;
            OnPropertyChanged(nameof(UrlPathSegment));
        }
    }

    private string _pageTitle;
    public string PageTitle
    {
        get => _pageTitle;
        set
        {
            _pageTitle = value;
            OnPropertyChanged(nameof(PageTitle));
        }
    }

    private string _themeColor;
    public string ThemeColor
    {
        get => _themeColor;
        set
        {
            _themeColor = value;
            OnPropertyChanged(nameof(ThemeColor));
        }
    }

    private string _defaultButtonColor;
    public string DefaultButtonColor
    {
        get => _defaultButtonColor;
        set
        {
            _defaultButtonColor = value;
            OnPropertyChanged(nameof(DefaultButtonColor));
        }
    }

    private string _backTitle;
    public string BackTitle
    {
        get => _backTitle;
        set
        {
            _backTitle = value;
            OnPropertyChanged(nameof(BackTitle));
        }
    }

    private string _noDataFoundText;
    public string NoDataFound
    {
        get => _noDataFoundText;
        set
        {
            _noDataFoundText = value;
            OnPropertyChanged(nameof(NoDataFound));
        }
    }

    private bool _backVisible;
    public bool BackVisible
    {
        get => _backVisible;
        set
        {
            _backVisible = value;
            OnPropertyChanged(nameof(BackVisible));
        }
    }

    private bool _menuVisible;
    public bool MenuVisible
    {
        get => _menuVisible;
        set
        {
            _menuVisible = value;
            OnPropertyChanged(nameof(MenuVisible));
        }
    }

    private bool _isVisibleHeaderSchoolName;
    public bool IsVisibleHeaderSchoolName
    {
        get => _isVisibleHeaderSchoolName;
        set
        {
            _isVisibleHeaderSchoolName = value;
            OnPropertyChanged(nameof(IsVisibleHeaderSchoolName));
        }
    }

    private bool _isVisibleDropDownIcon;
    public bool IsVisibleDropDownIcon
    {
        get => _isVisibleDropDownIcon;
        set
        {
            _isVisibleDropDownIcon = value;
            OnPropertyChanged(nameof(IsVisibleDropDownIcon));
        }
    }

    private string _formTitle;
    public string FormTitle
    {
        get => _formTitle;
        set
        {
            _formTitle = value;
            OnPropertyChanged(nameof(FormTitle));
        }
    }

    private string _formRightTitle;
    public string FormRightTitle
    {
        get => _formRightTitle;
        set
        {
            _formRightTitle = value;
            OnPropertyChanged(nameof(FormRightTitle));
        }
    }

    private bool _isVisiblBackIconAndPageTitle;
    public bool IsVisiblBackIconAndPageTitle
    {
        get => _isVisiblBackIconAndPageTitle;
        set
        {
            _isVisiblBackIconAndPageTitle = value;
            OnPropertyChanged(nameof(IsVisiblBackIconAndPageTitle));
        }
    }

    private bool _isVisiblBackTitle;
    public bool IsVisiblBackTitle
    {
        get => _isVisiblBackTitle;
        set
        {
            _isVisiblBackTitle = value;
            OnPropertyChanged(nameof(IsVisiblBackTitle));
        }
    }

    private string _nextIcon;
    public string NextIcon
    {
        get => _nextIcon;
        set
        {
            _nextIcon = value;
            OnPropertyChanged(nameof(NextIcon));
        }
    }

    private bool _isPopUpPage;
    public bool IsPopUpPage
    {
        get => _isPopUpPage;
        set
        {
            _isPopUpPage = value;
            OnPropertyChanged(nameof(IsPopUpPage));
        }
    }

    private bool _isVisibleCommunicationHeaderDeleteIcon;
    public bool IsVisibleCommunicationHeaderDeleteIcon
    {
        get => _isVisibleCommunicationHeaderDeleteIcon;
        set
        {
            _isVisibleCommunicationHeaderDeleteIcon = value;
            OnPropertyChanged(nameof(IsVisibleCommunicationHeaderDeleteIcon));
        }
    }

    private bool _isVisibleSaveHeaderIcon;
    public bool IsVisibleSaveHeaderIcon
    {
        get => _isVisibleSaveHeaderIcon;
        set
        {
            _isVisibleSaveHeaderIcon = value;
            OnPropertyChanged(nameof(IsVisibleSaveHeaderIcon));
        }
    }

    private bool _isVisibleBackIcon;
    public bool IsVisibleBackIcon
    {
        get => _isVisibleBackIcon;
        set
        {
            _isVisibleBackIcon = value;
            OnPropertyChanged(nameof(IsVisibleBackIcon));
        }
    }

    private bool _isVisibleSelectedMessageCount;
    public bool IsVisibleSelectedMessageCount
    {
        get => _isVisibleSelectedMessageCount;
        set
        {
            _isVisibleSelectedMessageCount = value;
            OnPropertyChanged(nameof(IsVisibleSelectedMessageCount));
        }
    }

    private IList<UserPushNotificationView> _pushNotificationsList = new List<UserPushNotificationView>();
    public IList<UserPushNotificationView> PushNotificationsList
    {
        get => _pushNotificationsList;
        set
        {
            _pushNotificationsList = value;
            OnPropertyChanged(nameof(PushNotificationsList));
        }
    }

    private ObservableCollection<UserPushNotificationView> _userPushNotifications = new ObservableCollection<UserPushNotificationView>();
    public ObservableCollection<UserPushNotificationView> UserPushNotifications
    {
        get => _userPushNotifications;
        set
        {
            _userPushNotifications = value;
            OnPropertyChanged(nameof(UserPushNotifications));
        }
    }

    private ObservableCollection<UserPushNotificationView> _filterPushNotifications = new ObservableCollection<UserPushNotificationView>();
    public ObservableCollection<UserPushNotificationView> FilterPushNotifications
    {
        get => _filterPushNotifications;
        set
        {
            _filterPushNotifications = value;
            OnPropertyChanged(nameof(FilterPushNotifications));
        }
    }

    private int _pushNotificationCount;
    public int PushNotificationCount
    {
        get => _pushNotificationCount;
        set
        {
            _pushNotificationCount = value;
            OnPropertyChanged(nameof(PushNotificationCount));
        }
    }

    private string _errorMessageText = string.Empty;
    public string ErrorMessageText
    {
        get => _errorMessageText;
        set
        {
            _errorMessageText = value;
            OnPropertyChanged(nameof(ErrorMessageText));
        }
    }

    private BindableStudentPickListItem _selectedStudent = new BindableStudentPickListItem();
    public BindableStudentPickListItem SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            _selectedStudent = value;
            OnPropertyChanged(nameof(SelectedStudent));
        }
    }

    private bool _isDemo;
    public bool IsDemo
    {
        get => _isDemo;
        set
        {
            _isDemo = value;
            OnPropertyChanged(nameof(IsDemo));
        }
    }

    private IList<BindableStudentPickListItem> _studentList = new List<BindableStudentPickListItem>();
    public IList<BindableStudentPickListItem> StudentList
    {
        get => _studentList;
        set
        {
            _studentList = value;
            OnPropertyChanged(nameof(StudentList));
        }
    }

    private IList<BindableModuleStructureView> _menuStructureList = new List<BindableModuleStructureView>();
    public IList<BindableModuleStructureView> MenuStructureList
    {
        get => _menuStructureList;
        set
        {
            _menuStructureList = value;
            OnPropertyChanged(nameof(MenuStructureList));
        }
    }

    private BindableModuleStructureView _selectedMenu = new BindableModuleStructureView();
    public BindableModuleStructureView SelectedMenu
    {
        get => _selectedMenu;
        set
        {
            _selectedMenu = value;
            OnPropertyChanged(nameof(SelectedMenu));
        }
    }

    private BindableModuleStructureView _selectedModule = new BindableModuleStructureView();
    public BindableModuleStructureView SelectedModule
    {
        get => _selectedModule;
        set
        {
            _selectedModule = value;
            OnPropertyChanged(nameof(SelectedModule));
        }
    }

    private Color _highlightColor = Colors.Transparent;
    public Color HighlightColor
    {
        get => _highlightColor;
        set
        {
            _highlightColor = value;
            OnPropertyChanged(nameof(HighlightColor));
        }
    }

    private Color _rightHighlightColor = Colors.Transparent;
    public Color RightHighlightColor
    {
        get => _rightHighlightColor;
        set
        {
            _rightHighlightColor = value;
            OnPropertyChanged(nameof(RightHighlightColor));
        }
    }

    private bool _isComposeAttachmentVisible;
    public bool IsComposeAttachmentVisible
    {
        get => _isComposeAttachmentVisible;
        set
        {
            _isComposeAttachmentVisible = value;
            OnPropertyChanged(nameof(IsComposeAttachmentVisible));
        }
    }
    
    private bool _isComposeSendVisible;
    public bool IsComposeSendVisible
    {
        get => _isComposeSendVisible;
        set
        {
            _isComposeSendVisible = value;
            OnPropertyChanged(nameof(IsComposeSendVisible));
        }
    }
    public SideMenuPanel MenuPage = null;
    public bool isMenuClicked = false;
    public string NotificationItemId { get; set; }
    

    private bool _isDetailVisible;
    public bool IsDetailVisible
    {
        get => _isDetailVisible;
        set
        {
            _isDetailVisible = value;
            OnPropertyChanged(nameof(IsDetailVisible));
        }
    }

    private string _errorMessageTitle;
    public string ErrorMessageTitle
    {
        get => _errorMessageTitle;
        set
        {
            _errorMessageTitle = value;
            OnPropertyChanged(nameof(ErrorMessageTitle));
        }
    }
    private bool _isSettingMenuHidden;
    public bool IsSettingMenuHidden
    {
        get => _isSettingMenuHidden;
        set
        {
            if (_isSettingMenuHidden != value)
            {
                _isSettingMenuHidden = value;
                OnPropertyChanged(nameof(IsSettingMenuHidden));
            }
        }
    }

    private bool _beamHeaderMessageIconVisibility = true;
    public bool BeamHeaderMessageIconVisibility
    {
        get => _beamHeaderMessageIconVisibility;
        set
        {
            if (_beamHeaderMessageIconVisibility != value)
            {
                _beamHeaderMessageIconVisibility = value;
                OnPropertyChanged(nameof(BeamHeaderMessageIconVisibility));
            }
        }
    }

    private bool _beamHeaderNotificationIconVisibility = true;
    public bool BeamHeaderNotificationIconVisibility
    {
        get => _beamHeaderNotificationIconVisibility;
        set
        {
            if (_beamHeaderNotificationIconVisibility != value)
            {
                _beamHeaderNotificationIconVisibility = value;
                OnPropertyChanged(nameof(BeamHeaderNotificationIconVisibility));
            }
        }
    }

    private bool _beamCommunicationHeaderAttachmentIconVisibility = false;
    public bool BeamCommunicationHeaderAttachmentIconVisibility
    {
        get => _beamCommunicationHeaderAttachmentIconVisibility;
        set
        {
            if (_beamCommunicationHeaderAttachmentIconVisibility != value)
            {
                _beamCommunicationHeaderAttachmentIconVisibility = value;
                OnPropertyChanged(nameof(BeamCommunicationHeaderAttachmentIconVisibility));
            }
        }
    }

    private bool _beamCommunicationHeaderSendMessageIconVisibility = false;
    public bool BeamCommunicationHeaderSendMessageIconVisibility
    {
        get => _beamCommunicationHeaderSendMessageIconVisibility;
        set
        {
            if (_beamCommunicationHeaderSendMessageIconVisibility != value)
            {
                _beamCommunicationHeaderSendMessageIconVisibility = value;
                OnPropertyChanged(nameof(BeamCommunicationHeaderSendMessageIconVisibility));
            }
        }
    }

    private bool _beamCommunicationHeaderDeleteIconVisibility = false;
    public bool BeamCommunicationHeaderDeleteIconVisibility
    {
        get => _beamCommunicationHeaderDeleteIconVisibility;
        set
        {
            if (_beamCommunicationHeaderDeleteIconVisibility != value)
            {
                _beamCommunicationHeaderDeleteIconVisibility = value;
                OnPropertyChanged(nameof(BeamCommunicationHeaderDeleteIconVisibility));
            }
        }
    }
    #endregion
    
    public ViewModelBase(IMapper mapper, INativeServices nativeServices, INavigation navigation)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods

    private async void InitializePage()
    {
        BackTitle = TextResource.BackTitle;
        NextIcon = TextResource.NextIcon;
        NoDataFound = TextResource.NoDataFound;
        IsDemo = TextResource.IsDemoApp.ToBoolean();
        DefaultButtonColor = TextResource.DefaultButtonColor;
        if (AppSettings.Current.Settings != null) ThemeColor = AppSettings.Current.Settings.ThemeColor;
        SideMenuClickCommand = new Command(SideMenuClicked);
        //RightSideMenuClickCommand = new Command(RightSideMenuClicked);
        MenuListTappedCommand = new Command<BindableModuleStructureView>(MenuListViewTapped);
        //MenuCloseClickCommand = new Command(MenuCloseClicked);
        LogoutClickCommand = new Command(LogoutClicked);

        MenuStructureList = AppSettings.Current.MenuStructureList;

        StudentTapCommand = new Command(StudentViewTapClicked);
        FooterMenuClickCommand = new Command<BindableModuleStructureView>(FooterMenuClicked);
        StudentListTappedCommand = new Command<BindableStudentPickListItem>(StudentListClicked);
        BackClickCommand = new Command(BackClicked);
        StudentFrameTapCommand = new Command(StudentFrameClicked);

        StudentList = AppSettings.Current.StudentList;
        SelectedStudent = StudentList != null && StudentList.Count > 0
            ? StudentList[0]
            : new BindableStudentPickListItem();
        PushNotificationSettings.Current.IsPushNotificationEnabled = await ICCacheManager.GetObject<bool>(TextResource.PushNotificationKey);
        MenuPage = new SideMenuPanel();

        MessagingCenter.Subscribe<string>("", "SideMenuPanelLeftSwipeSubscribe",
            (arg) =>
            {
                MessagingCenter.Subscribe<string>("", "SideMenuPanelLeftSwipe", async (c) => { await SwipeLeft(); });
            });

        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        ListTappedCommandForBottomList = new Command<BindableModuleStructureView>(MenuListViewTapped);
        
    }

    // private async void StudentFrameClicked(object obj)
    // {
    //     if (AppSettings.Current.SelectedStudent != null && !string.IsNullOrEmpty(AppSettings.Current.SelectedStudent.ItemId))
    //         if (PopupNavigation.Instance.PopupStack.Any())
    //         {
    //             var popupPage = PopupNavigation.Instance.PopupStack.Reverse().FirstOrDefault();
    //             await PopupNavigation.Instance.RemovePageAsync(popupPage);
    //         }
    // }
    
    private async void StudentFrameClicked(object obj)
    {
        if (AppSettings.Current.SelectedStudent != null && !string.IsNullOrEmpty(AppSettings.Current.SelectedStudent.ItemId))
        {
            // Close any currently displayed popups or modals
            var currentModal = Application.Current.MainPage.FindByName<ContentPage>("ModalPage");

            if (currentModal != null)
            {
                // Hide the modal page
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
    }

    private void FooterMenuClicked(BindableModuleStructureView obj)
    {
        if (obj != null) AppSettings.Current.SelectedFooterMenu = obj;
        if (AppSettings.Current.SelectedFooterMenu == null)
            AppSettings.Current.FooterMenuList.ToList().ForEach(b => b.IsSelected = false);
        if (AppSettings.Current.SelectedFooterMenu != null && AppSettings.Current.SelectedFooterMenu.ModuleCode != null)
            if (!AppSettings.Current.SelectedFooterMenu.IsSelected)
            {
                AppSettings.Current.SelectedFooterMenu.IsSelected = true;
                var selectedFooterMenu = AppSettings.Current.FooterMenuList.ToList()
                    .Where(b => b.ModuleCode == AppSettings.Current.SelectedFooterMenu.ModuleCode)?.FirstOrDefault();
                if (selectedFooterMenu != null) selectedFooterMenu.IsSelected = true;
                AppSettings.Current.FooterMenuList.ToList()
                    .FindAll(b => b.ModuleCode != AppSettings.Current.SelectedFooterMenu.ModuleCode)
                    .ForEach(b => b.IsSelected = false);
                MenuListViewTapped(AppSettings.Current.SelectedFooterMenu);
            }
    }

    private async void SideMenuClicked(object obj)
    {
        // try
        // {
        //     var color = Color.FromHex(AppSettings.Current.Settings.ThemeColor);
        //     HighlightColor = color.WithLuminosity(color.Luminosity - color.Luminosity * .1);
        //
        //     if (menuPage != null)
        //         await Application.Current.MainPage.ShowPopupAsync(menuPage);
        //         //await PopupNavigation.Instance.PushAsync(menuPage, true);
        //     else
        //         await Application.Current.MainPage.ShowPopupAsync(sideMenuPanel);
        //         //await PopupNavigation.Instance.PushAsync(new SideMenuPanel(this), true);
        //       
        //     HighlightColor = Colors.Transparent;
        // }
        // catch (Exception ex)
        // {
        //     Crashes.TrackError(ex);
        // }
    }

    // private async void RightSideMenuClicked(object obj)
    // {
    //     try
    //     {
    //         Func<PopupPage, bool> calPredicate = x => x.GetType() == typeof(CustomCalendar);
    //         if (PopupNavigation.Instance.PopupStack.Any(calPredicate))
    //         {
    //             var popupPage = PopupNavigation.Instance.PopupStack.FirstOrDefault(calPredicate);
    //             await PopupNavigation.Instance.RemovePageAsync(popupPage);
    //         }
    //
    //         var color = Color.FromHex(AppSettings.Current.Settings.ThemeColor);
    //         RightHighlightColor = color.WithLuminosity(color.Luminosity - color.Luminosity * .1);
    //         await PopupNavigation.Instance.PushAsync(new RightMenuPanel(), true);
    //         RightHighlightColor = Color.Transparent;
    //     }
    //     catch (Exception ex)
    //     {
    //         Crashes.TrackError(ex);
    //     }
    // }

    public async void StudentViewTapClicked(object obj)
    {
        var getViewModelType = GetCurrentPage();
        //if (getViewModelType == typeof(OnlinePaymentForm) || getViewModelType == typeof(ReportCardForm))
        //     AppSettings.Current.IsDisplayAllStudentList = true;
        // else
        //     AppSettings.Current.IsDisplayAllStudentList = false;
        //
        // if (AppSettings.Current.IsDisplayAllStudentList)
        //     AppSettings.Current.StudentList = AppSettings.Current.AllStudentList;
        // else
        //     AppSettings.Current.StudentList = AppSettings.Current.RegisteredStudentList;
        // await PopupNavigation.Instance.PushAsync(new StudentListPopUp(this), true);
        AppSettings.Current.StudentList = AppSettings.Current.RegisteredStudentList;
        var studentListPopup = new StudentListPopUp()
        {
            BindingContext = this
        };
        SetPopupInstance(studentListPopup);
        Application.Current.MainPage.ShowPopup(studentListPopup);
    }
    public void SetPopupInstance(Popup popup)
    {
        AppSettings.Current.CurrentPopup = popup;
    }

    public async void OpenStudentSelection(bool isDisplayAllStudent = false)
    {
        AppSettings.Current.StudentList = isDisplayAllStudent
            ? AppSettings.Current.AllStudentList
            : AppSettings.Current.RegisteredStudentList;
        if (AppSettings.Current.IsParent && AppSettings.Current.StudentList != null &&
            AppSettings.Current.StudentList.Count > 0)
        {
            if (AppSettings.Current.StudentList.Count == 1)
            {
                AppSettings.Current.SelectedStudent = AppSettings.Current.StudentList.FirstOrDefault();
                AppSettings.Current.IsStudentFooterVisible =
                    AppSettings.Current.PortalUserType == PortalUserTypes.Parent &&
                    AppSettings.Current.SelectedStudent.ItemId != null
                        ? true
                        : false;
                AppSettings.Current.IsStudentFooterVisibleForAllStudentList =
                    AppSettings.Current.PortalUserType == PortalUserTypes.Parent &&
                    AppSettings.Current.SelectedStudentFromAllStudentList.ItemId != null
                        ? true
                        : false;
            }

            if (AppSettings.Current.AllStudentList.Count == 1)
            {
                AppSettings.Current.SelectedStudentFromAllStudentList =
                    AppSettings.Current.AllStudentList.FirstOrDefault();
                AppSettings.Current.IsStudentFooterVisibleForAllStudentList =
                    AppSettings.Current.PortalUserType == PortalUserTypes.Parent &&
                    AppSettings.Current.SelectedStudentFromAllStudentList.ItemId != null
                        ? true
                        : false;
            }


            if (AppSettings.Current.SelectedStudent == null ||
                string.IsNullOrEmpty(AppSettings.Current.SelectedStudent.ItemId))
            {
                if (!isDisplayAllStudent)
                    if (!App.IsStudentPopupOpen)
                    {
                        App.IsStudentPopupOpen = true;
                        //await Application.Current.MainPage.ShowPopupAsync(new StudentListPopUp(this), true);
                        //await PopupNavigation.Instance.PushAsync(new StudentListPopUp(this), true);
                    }
            }
            else if (!isDisplayAllStudent)
            {
                GetStudentData();
            }


            if (AppSettings.Current.SelectedStudentFromAllStudentList == null ||
                string.IsNullOrEmpty(AppSettings.Current.SelectedStudentFromAllStudentList.ItemId))
            {
                if (isDisplayAllStudent)
                    if (!App.IsStudentPopupOpen)
                    {
                        App.IsStudentPopupOpen = true;
                        //await Application.Current.MainPage.ShowPopupAsync(new StudentListPopUp(this), true);
                        //await PopupNavigation.Instance.PushAsync(new StudentListPopUp(this), true);
                    }
            }
            else if (isDisplayAllStudent)
            {
                GetStudentData();
            }
        }
        else if (AppSettings.Current.IsTeacher || AppSettings.Current.PortalUserType == PortalUserTypes.Student)
        {
            GetStudentData();
        }
    }

    private async void StudentListClicked(BindableStudentPickListItem obj)
    {
        if (obj != null)
        {
            if (AppSettings.Current.IsDisplayAllStudentList)
            {
                AppSettings.Current.SelectedStudentFromAllStudentList = obj;
                if (AppSettings.Current.SelectedStudentFromAllStudentList != null)
                {
                    var selectedStudent =
                        AppSettings.Current.RegisteredStudentList.Where(x => x.ItemId.Equals(obj.ItemId));
                    if (selectedStudent != null && selectedStudent.FirstOrDefault() != null)
                        AppSettings.Current.SelectedStudent = obj;
                }
            }
            else
            {
                AppSettings.Current.SelectedStudent = obj;
                AppSettings.Current.SelectedStudentFromAllStudentList = obj;
            }

            SelectedStudent = null;
            App.IsStudentPopupOpen = false;
            AppSettings.Current.IsStudentFooterVisible =
                AppSettings.Current.PortalUserType == PortalUserTypes.Parent &&
                AppSettings.Current.SelectedStudent.ItemId != null
                    ? true
                    : false;
            AppSettings.Current.IsStudentFooterVisibleForAllStudentList =
                AppSettings.Current.PortalUserType == PortalUserTypes.Parent &&
                AppSettings.Current.SelectedStudentFromAllStudentList.ItemId != null
                    ? true
                    : false;
            AppSettings.Current.CurrentPopup?.Close();
            GetStudentData();
            // var popupPage = PopupNavigation.Instance.PopupStack.Reverse().FirstOrDefault();
            // await PopupNavigation.Instance.RemovePageAsync(popupPage);
            // GetStudentData();
            // if (HostScreen.Router.GetCurrentViewModel().GetType() == typeof(BooksReservationForm))
            //     MessagingCenter.Send<string>("", "StudentSelectionIconTapped");
        }
    }

    public virtual void GetStudentData()
    {
    }

    public virtual async void BackClicked(object obj)
    {
        try
        {
            MessagingCenter.Send("", "RefreshQuickPaymentHistory");
            if (obj != null)
                IsPopUpPage = (bool)obj;

            await Navigation.PopAsync();
            
            // var currentPage = Navigation.NavigationStack.LastOrDefault();
            // if (currentPage != null)
            // {
            //     Navigation.RemovePage(currentPage);  
            // }

            // if (IsPopUpPage)
            // {
            //     IsPopUpPage = false;
            //     if (PopupNavigation.Instance.PopupStack != null && PopupNavigation.Instance.PopupStack.Any())
            //     {
            //         var popupPage = PopupNavigation.Instance.PopupStack.Reverse()?.FirstOrDefault();
            //         if (popupPage != null)
            //             await PopupNavigation.Instance.RemovePageAsync(popupPage);
            //     }
            // }
            // else
            // {
            //     await Application.Current.MainPage.Navigation.PopAsync();
            // }
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    public async void HandleMenuSelectionOnBack()
    {
        try
        {
            await Navigation.PopAsync();
            ViewModelBase currentViewModel = null;
            if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            {
                var currentPage = Application.Current.MainPage.Navigation.NavigationStack.Last();
                currentViewModel = currentPage.BindingContext as ViewModelBase;
            }
            if (currentViewModel != null && AppSettings.Current.FooterMenuList != null)
            {
                var data = AppSettings.Current.FooterMenuList?.Where(x =>
                    x.ModuleCode?.ToLower() == currentViewModel.SelectedModule.ModuleCode?.ToLower())?.FirstOrDefault();
                if (data != null)
                {
                    AppSettings.Current.SelectedFooterMenu = data;
                    AppSettings.Current.SelectedFooterMenu.IsSelected = true;
                    AppSettings.Current.FooterMenuList.ToList()
                        .FindAll(b => b.ModuleCode != AppSettings.Current.SelectedFooterMenu.ModuleCode)
                        .ForEach(b => b.IsSelected = false);
                }
                else
                {
                    AppSettings.Current.SelectedFooterMenu = null;
                }
            }
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    public string ConvertToModuleCodeCacheKey(string removeNotificationCacheKey)
    {
        return string.Concat("Noti_Removed_", removeNotificationCacheKey);
    }

    public async void PushNotificationClick(NotificationData notification, INativeServices nativeServices)
    {
        App.NotificationValues = new NotificationData();
        var color = Color.FromArgb(ThemeColor);
        nativeServices.ChangeStatusBarColor((int)(color.Red * 255), (int)(color.Green * 255), (int)(color.Blue * 255));
        MobileNotificationTypes notificationType =
            (MobileNotificationTypes)Convert.ToInt32(notification.notificationType);
        Task.Delay(300);
        switch (notificationType)
        {
            case MobileNotificationTypes.News:
                NewsForm newsForm = new (_mapper, _nativeServices, Navigation, isFromNotification: true)
                {
                    NewsNotificationId = notification.primaryKey,
                    IsNewsSelectedFromFooter = true,
                    PageTitle = notification.notificationModuleName,
                    MenuVisible = true
                };
                NewsPage newsPage = new()
                {
                    BindingContext = newsForm
                };
                //await Navigation.PushAsync(newsPage);
                break;
            case MobileNotificationTypes.Circulars:
                MessageFromSchoolForm messageFromSchoolForm = new (_mapper, _nativeServices, Navigation, notification)
                {
                    MenuVisible = true,
                    PageTitle = notification.notificationModuleName
                };
                MessageFromSchool messageFromSchool = new()
                {
                    BindingContext = messageFromSchoolForm
                };
                //await Navigation.PushAsync(messageFromSchool);
                break;
            case MobileNotificationTypes.Communication:
                CommunicationForm communicationForm = new (_mapper, _nativeServices, Navigation, notification.primaryKey)
                {
                    MenuVisible = true,
                    NotificationItemId = notification.primaryKey,
                    PageTitle = notification.notificationModuleName
                };
                CommunicationPage communicationPage = new()
                {
                    BindingContext = communicationForm
                };
                //await Navigation.PushAsync(communicationPage);                
                break;
            case MobileNotificationTypes.CustomAlert:
                MessageFromSchoolForm customAlertsMessageFromSchoolForm = new MessageFromSchoolForm(_mapper, _nativeServices, Navigation, notification);
                customAlertsMessageFromSchoolForm.MenuVisible = true;
                customAlertsMessageFromSchoolForm.PageTitle = notification.notificationModuleName;
                AppSettings.Current.IsAlertsFromPushNotifications = true;
                MessageFromSchool alertMessageFromSchool = new()
                {
                    BindingContext = customAlertsMessageFromSchoolForm
                };
                //await Navigation.PushAsync(alertMessageFromSchool);
                break;
            case MobileNotificationTypes.Exams:
                ExamForm examForm = new (_mapper, _nativeServices, Navigation)
                {
                    MenuVisible = true,
                    NotificationItemId = notification.primaryKey,
                    PageTitle = notification.notificationModuleName
                };
                if (AppSettings.Current.IsParent)
                    AppSettings.Current.SelectedStudent = AppSettings.Current.StudentList
                        .Where(i => i.ItemId == notification.userRefId).FirstOrDefault();
                examForm.OpenStudentSelection();
                var examPage = new ExamPage()
                {
                    BindingContext = examForm
                };
                //await Navigation.PushAsync(examPage);
                break;
            case MobileNotificationTypes.Events:
                EventForm eventForm = new (_mapper, _nativeServices, Navigation, id: notification.primaryKey)
                {
                    MenuVisible = true,
                    PageTitle = notification.notificationModuleName
                };
                if (AppSettings.Current.IsParent)
                    AppSettings.Current.SelectedStudent = AppSettings.Current.StudentList
                        .Where(i => i.ItemId == notification.userRefId).FirstOrDefault();
                var events = new Events()
                {
                    BindingContext = eventForm
                };
                //await Navigation.PushAsync(events);                
                break;
            case MobileNotificationTypes.Appointment:
                if (AppSettings.Current.IsParent)
                {
                    TeacherAppointmentForm teacherAppointmentForm = new (_mapper, _nativeServices, Navigation, notification.primaryKey)
                    {
                        MenuVisible = true,
                        NotificationItemId = notification.primaryKey,
                        PageTitle = notification.notificationModuleName
                    };
                    AppSettings.Current.SelectedStudent = AppSettings.Current.StudentList
                        .Where(i => i.ItemId == notification.userRefId).FirstOrDefault();
                    AppSettings.Current.IsAppointmentFromPushNotifications = true;
                    teacherAppointmentForm.OpenStudentSelection();
                    TeacherAppointment teacherAppointment = new()
                    {
                        BindingContext = teacherAppointmentForm
                    };
                    //await Navigation.PushAsync(teacherAppointment);
                }
        
                if (AppSettings.Current.IsTeacher)
                {
                    FamilyAppointmentForm familyAppointmentForm = new (_mapper, _nativeServices, Navigation, notification.primaryKey)
                    {
                        MenuVisible = true,
                        NotificationItemId = notification.primaryKey,
                        PageTitle = notification.notificationModuleName
                    };
                    AppSettings.Current.IsAppointmentFromPushNotifications = true;
                    FamilyAppointment familyAppointment = new()
                    {
                        BindingContext = familyAppointmentForm
                    };
                    //await Navigation.PushAsync(familyAppointment);
                }
        
                break;
            case MobileNotificationTypes.Calendar:
                CalendarForm calendarForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = notification.notificationModuleName,
                    //calendarForm.ToDate = DateTime.MinValue;
                    NotificationItemId = notification.primaryKey,
                    AgendaTypeId = null
                };
                if (notification.notificationSubType != null && notification.notificationSubType.ToLower().Equals("r"))
                {
                    calendarForm.FromDate = DateTime.Now;
                    calendarForm.ShowReminderData = true;
                }
                else if (notification.notificationSubType != null &&
                         notification.notificationSubType.ToLower().Equals("a"))
                {
                    calendarForm.FromDate = DateTime.Now.AddDays(1);
                }
        
                if (AppSettings.Current.IsParent)
                    AppSettings.Current.SelectedStudent = AppSettings.Current.StudentList
                        .Where(i => i.ItemId == notification.userRefId).FirstOrDefault();
                AppSettings.Current.IsDisplayAllStudentList = false;
                calendarForm.OpenStudentSelection();
                Calendar calendar = new()
                {
                    BindingContext = calendarForm
                };
                //await Navigation.PushAsync(calendar);
                break;
        }
    }

    #region Side menu method

    // private void MenuCloseClicked(object obj)
    // {
    //     if (obj is Popup popup)
    //     {
    //         popup.Close(); 
    //     }
    // }

    private async Task SwipeLeft()
    {
        //if (PopupNavigation.Instance.PopupStack.Any()) await PopupNavigation.Instance.PopAllAsync();
    }

    public async void MenuListViewTapped(BindableModuleStructureView obj)
    {
        try
        {
            if (obj != null)
            {
                AppSettings.Current.CurrentPopup?.Close();
                AppSettings.Current.CurrentPopup = null;
                SelectedMenu = null;

                // Func<PopupPage, bool> calPredicate = x => x.GetType() == typeof(CustomCalendar);
                // if (PopupNavigation.Instance.PopupStack.Any(calPredicate))
                // {
                //     var popupPage = PopupNavigation.Instance.PopupStack.FirstOrDefault(calPredicate);
                //     await PopupNavigation.Instance.RemovePageAsync(popupPage);
                // }
                //
                // if (PopupNavigation.Instance.PopupStack.Any()) await PopupNavigation.Instance.PopAllAsync();

                if (isMenuClicked) return;
                isMenuClicked = true;

                if (!obj.IsSelected) //click from side menu and not footer menu
                {
                    var data = AppSettings.Current.FooterMenuList
                        ?.Where(x => x.ModuleCode?.ToLower() == obj.ModuleCode?.ToLower())?.FirstOrDefault();
                    if (data != null && !string.IsNullOrEmpty(data.ModuleCode))
                    {
                        AppSettings.Current.SelectedFooterMenu = data;
                        AppSettings.Current.SelectedFooterMenu.IsSelected = true;
                        AppSettings.Current.FooterMenuList.ToList()
                            .FindAll(b => b.ModuleCode != AppSettings.Current.SelectedFooterMenu.ModuleCode)
                            .ForEach(b => b.IsSelected = false);
                    }
                    else
                    {
                        AppSettings.Current.SelectedFooterMenu = null;
                    }
                }

                //await ApiHelper.ShowProcessingIndicatorPopup();
                if (obj.ModuleCode.ToLower().Equals("home"))
                {
                    HomeForm homeForm = new(_mapper, Navigation, _nativeServices)
                    {
                        IsPageLoaded = true,
                        SelectedModule = obj,
                        IsNewsSelectedFromFooter = false
                    };
                    HomePage homePage = new()
                    {
                        BindingContext = homeForm
                    };
                    await Navigation.PushAsync(homePage);
                }
                else if (obj.ModuleCode.ToLower().Equals("calendar"))
                {
                    CalendarForm calendarForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    calendarForm.OpenStudentSelection();

                    Calendar calendar = new()
                    {
                        BindingContext = calendarForm
                    };
                    await Navigation.PushAsync(calendar);
                }
                else if (obj.ModuleCode.ToLower().Equals("cashlesscampus"))
                {
                    CampusKeyForm campusKeyForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    campusKeyForm.OpenStudentSelection();

                    CampusKey campusKey = new()
                    {
                        BindingContext = campusKeyForm
                    };
                    await Navigation.PushAsync(campusKey);
                }
                else if (obj.ModuleCode.ToLower().Equals("timetable"))
                {
                    TimeTableForm timetableForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    timetableForm.OpenStudentSelection();
                    TimeTablePage timeTablePage = new TimeTablePage()
                    {
                        BindingContext = timetableForm
                    };
                    await Navigation.PushAsync(timeTablePage);

                    // RegistrationForm registrationForm = new(_mapper, _nativeServices, Navigation)
                    // {
                    //     PageTitle = "Registration",
                    //     MenuVisible = true,
                    //     SelectedModule = obj
                    // };
                    // AppSettings.Current.IsDisplayAllStudentList = false;
                    // registrationForm.OpenStudentSelection();
                    // RegistrationPage registrationPage = new()
                    // {
                    //     BindingContext = registrationForm
                    // };
                    // await Navigation.PushAsync(registrationPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("communication"))
                {
                    CommunicationForm communicationForm = new(_mapper, _nativeServices, Navigation)
                    {
                        MenuVisible = true,
                        PageTitle = TextResource.InboxText,
                        SelectedModule = obj
                    };
                    CommunicationPage communicationPage = new()
                    {
                        BindingContext = communicationForm
                    };
                    await Navigation.PushAsync(communicationPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("bustracking"))
                {
                    BusTrackingForm busTrackingForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    busTrackingForm.OpenStudentSelection();

                    var busTracking = new BusTracking()
                    {
                        BindingContext = busTrackingForm
                    };
                    await Navigation.PushAsync(busTracking);
                }
                else if (obj.ModuleCode.ToLower().Equals("exams"))
                {
                    ExamForm examForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj,
                        MenuVisible = true
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    examForm.OpenStudentSelection();
                    var examPage = new ExamPage()
                    {
                        BindingContext = examForm
                    };
                    await Navigation.PushAsync(examPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("complaints"))
                {
                    ComplaintsForm complaintsForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj
                    };
                    Complaints complaints = new()
                    {
                        BindingContext = complaintsForm
                    };
                    await Navigation.PushAsync(complaints);
                }
                else if (obj.ModuleCode.ToLower().Equals("settings"))
                {
                    SettingsForm settingsForm = new(_mapper, _nativeServices, Navigation)
                    {
                        MenuVisible = true,
                        SelectedModule = obj,
                        PageTitle = obj.ModuleName
                    };
                    var settings = new Settings()
                    {
                        BindingContext = settingsForm
                    };
                    await Navigation.PushAsync(settings);
                }
                else if (obj.ModuleCode.ToLower().Equals("reportcards"))
                {
                    ReportCardForm reportCardForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = true;
                    reportCardForm.OpenStudentSelection(true);
                    ReportCardPage reportCardPage = new()
                    {
                        BindingContext = reportCardForm
                    };
                    await Navigation.PushAsync(reportCardPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("messagefromschool"))
                {
                    MessageFromSchoolForm messageFromSchoolForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj
                    };
                    MessageFromSchool messageFromSchool = new()
                    {
                        BindingContext = messageFromSchoolForm
                    };
                    await Navigation.PushAsync(messageFromSchool);
                }
                else if (obj.ModuleCode.ToLower().Equals("eventsholidays"))
                {
                    EventForm eventForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj
                    };
                    var events = new Events()
                    {
                        BindingContext = eventForm
                    };
                    await Navigation.PushAsync(events);
                }
                else if (obj.ModuleCode.ToLower().Equals("financialstatus"))
                {
                    FinancialStatusForm financialStatusForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj,
                        MenuVisible = true
                    };
                    FinancialStatusPage financialStatusPage = new()
                    {
                        BindingContext = financialStatusForm
                    };
                    await Navigation.PushAsync(financialStatusPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("attendance"))
                {
                    AttendanceForm attendanceForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj,
                        MenuVisible = true
                    };
                    attendanceForm.GetAttendanceType();
                    attendanceForm.GetAttendanceGroup();
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    attendanceForm.OpenStudentSelection();

                    var attendancePage = new AttendancePage()
                    {
                        BindingContext = attendanceForm
                    };
                    await Navigation.PushAsync(attendancePage);
                }
                else if (obj.ModuleCode.ToLower().Equals("contactus"))
                {
                    ContactUsForm contactUsForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj,
                        MenuVisible = true
                    };
                    ContactUsPage contactUsPage = new()
                    {
                        BindingContext = contactUsForm
                    };
                    await Navigation.PushAsync(contactUsPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("teacherappointment"))
                {
                    TeacherAppointmentForm teacherAppointmentForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    teacherAppointmentForm.OpenStudentSelection();
                    TeacherAppointment teacherAppointment = new()
                    {
                        BindingContext = teacherAppointmentForm
                    };
                    await Navigation.PushAsync(teacherAppointment);
                }
                else if (obj.ModuleCode.ToLower().Equals("conduct"))
                {
                    ConductForm conductForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj,
                        MenuVisible = true
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    conductForm.OpenStudentSelection();
                    ConductPage conductPage = new()
                    {
                        BindingContext = conductForm
                    };
                    await Navigation.PushAsync(conductPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("familyappointment"))
                {
                    FamilyAppointmentForm familyAppointmentForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    FamilyAppointment familyAppointment = new()
                    {
                        BindingContext = familyAppointmentForm
                    };
                    await Navigation.PushAsync(familyAppointment);
                }
                else if (obj.ModuleCode.ToLower().Equals("studentattendance"))
                {
                    StudentAttendanceFilterForm studentAttendanceFilterForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    studentAttendanceFilterForm.InitializePage();
                    StudentAttendanceFilterPage studentAttendanceFilterPage = new()
                    {
                        BindingContext = studentAttendanceFilterForm
                    };
                    await Navigation.PushAsync(studentAttendanceFilterPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("dailymarks"))
                {
                    DailyMarksForm dailyMarksForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj,
                        MenuVisible = true
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    dailyMarksForm.OpenStudentSelection();
                    DailyMarksPage dailyMarksPage = new()
                    {
                        BindingContext = dailyMarksForm
                    };
                    await Navigation.PushAsync(dailyMarksPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("survey"))
                {
                    SurveyForm surveyForm = new(_mapper, _nativeServices, Navigation)
                    {
                        MenuVisible = true,
                        SelectedModule = obj,
                        PageTitle = obj.ModuleName
                    };
                    App.IsSurveyOn = true;
                    surveyForm.IsFromMenu = true;
                    surveyForm.GetAllPendingSurveys();
                    SurveyPage surveyPage = new()
                    {
                        BindingContext = surveyForm
                    };
                    await Navigation.PushAsync(surveyPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("datacollection"))
                {
                    DataCollectionMainForm dataCollectionMainForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj,
                        MenuVisible = true
                    };
                    dataCollectionMainForm.GetAllDataCollection();
                    DataCollectionMainPage dataCollectionMainPage = new()
                    {
                        BindingContext = dataCollectionMainForm
                    };
                    await Navigation.PushAsync(dataCollectionMainPage);
                }
                else if (obj.TypeCode == 3)
                {
                    var finalUrl = string.Empty;
                    if (!string.IsNullOrEmpty(AppSettings.Current.PortalUrl) && !string.IsNullOrEmpty(obj.ModuleUrl))
                        finalUrl = string.Concat(AppSettings.Current.PortalUrl, obj.ModuleUrl, "&uid=",
                            AppSettings.Current.UserSessionUid);
                    await Launcher.Default.OpenAsync(finalUrl);
                }
                else if (obj.ModuleCode.ToLower().Equals("onlinepayment"))
                {
                    OnlinePaymentForm onlinePaymentForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = true;
                    onlinePaymentForm.OpenStudentSelection(true);
                    OnlinePaymentPage onlinePaymentPage = new()
                    {
                        BindingContext = onlinePaymentForm
                    };
                    await Navigation.PushAsync(onlinePaymentPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("covidtest"))
                {
                    TestDetailsForm testDetailsForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj,
                        MenuVisible = true
                    };
                    TestDetails testDetails = new()
                    {
                        BindingContext = testDetailsForm
                    };
                    await Navigation.PushAsync(testDetails);
                }
                //
                //     #region for beam
                //
                //     else if (obj.ModuleName.ToLower().Equals("settings"))
                //     {
                //         SettingsForm settingsForm = new SettingsForm();
                //         settingsForm.MenuVisible = true;
                //         settingsForm.SelectedModule = obj;
                //         settingsForm.PageTitle = obj.ModuleName;
                //         HostScreen.Router.Navigate.Execute(settingsForm).Subscribe();
                //     }
                else if (obj.ModuleName.ToLower().Equals("logout"))
                {
                    HelperMethods.Logout(_mapper, _nativeServices, Navigation);
                }
                //
                //     #endregion for beam
                //
                else if (obj.ModuleCode.ToLower().Equals("medical"))
                {
                    MedicalForm medicalForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj,
                        MenuVisible = true
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    medicalForm.OpenStudentSelection();
                    var medicalPage = new MedicalPage()
                    {
                        BindingContext = medicalForm
                    };
                    await Navigation.PushAsync(medicalPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("news"))
                {
                    NewsForm newsForm = new(_mapper, _nativeServices, Navigation)
                    {
                        IsNewsSelectedFromFooter = true,
                        PageTitle = TextResource.DashboardText,
                        IsPopUpPage = false,
                        MenuVisible = true,
                        BackVisible = false
                    };
                    NewsPage newsPage = new()
                    {
                        BindingContext = newsForm
                    };
                    await Navigation.PushAsync(newsPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("notificationcenter"))
                {
                    NotificationCenterForm notificationCenterForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = string.Concat(obj.ModuleName, " (",
                            DateTime.Now.ToString("dddd, MMMM dd, yyyy"), ")"),
                        SelectedModule = obj
                    };
                    //notificationCenterForm.MenuVisible = true;
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    notificationCenterForm.OpenStudentSelection();

                    NotificationCenterPage notificationCenterPage = new()
                    {
                        BindingContext = notificationCenterForm
                    };
                    await Navigation.PushAsync(notificationCenterPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("resources"))
                {
                    if (AppSettings.Current.IsTeacher)
                    {
                        TeacherResourcesForm teacherResourcesForm = new(_mapper, _nativeServices, Navigation)
                        {
                            PageTitle = obj.ModuleName,
                            SelectedModule = obj,
                            MenuVisible = true
                        };
                        AppSettings.Current.IsDisplayAllStudentList = false;
                        teacherResourcesForm.OpenStudentSelection();

                        TeacherResourcesPage teacherResourcesPage = new()
                        {
                            BindingContext = teacherResourcesForm
                        };
                        await Navigation.PushAsync(teacherResourcesPage);
                    }
                    else
                    {
                        ParentStudentResourcesForm parentStudentResourcesForm =
                            new(_mapper, _nativeServices, Navigation)
                            {
                                PageTitle = obj.ModuleName,
                                SelectedModule = obj,
                                MenuVisible = true
                            };
                        AppSettings.Current.IsDisplayAllStudentList = false;
                        parentStudentResourcesForm.OpenStudentSelection();
                        ParentStudentResourcesPage parentStudentResourcesPage = new()
                        {
                            BindingContext = parentStudentResourcesForm
                        };
                        await Navigation.PushAsync(parentStudentResourcesPage);
                    }
                }
                else if (obj.ModuleCode.ToLower().Equals("booksreservation"))
                {
                    BooksReservationForm booksReservationForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        SelectedModule = obj,
                        MenuVisible = true
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    BooksReservationPage booksReservationPage = new()
                    {
                        BindingContext = booksReservationForm
                    };
                    await Navigation.PushAsync(booksReservationPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("onlinelessons"))
                {
                    OnlineLessonForm onlinelessonform = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    onlinelessonform.OpenStudentSelection();
                    OnlineLessonPage onlineLessonPage = new()
                    {
                        BindingContext = onlinelessonform
                    };
                    await Navigation.PushAsync(onlineLessonPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("quickpayment"))
                {
                    QuickPaymentForm quickPaymentForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    QuickPaymentPage quickPaymentPage = new()
                    {
                        BindingContext = quickPaymentForm
                    };
                    await Navigation.PushAsync(quickPaymentPage);
                    quickPaymentForm.OpenStudentSelection();
                }
                else if (obj.ModuleCode.ToLower().Equals("chequereplacement"))
                {
                    ChequeReplacementForm chequeReplacementForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    chequeReplacementForm.OpenStudentSelection();
                    ChequeReplacementPage chequeReplacementPage = new()
                    {
                        BindingContext = chequeReplacementForm
                    };
                    await Navigation.PushAsync(chequeReplacementPage);
                }
                else if (obj.ModuleCode.ToLower().Equals("certificate"))
                {
                    CertificatesForm certificatesform = new(_nativeServices)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    CertificatesPage certificatesPage = new()
                    {
                        BindingContext = certificatesform
                    };
                    await Navigation.PushAsync(certificatesPage);
                    certificatesform.OpenStudentSelection();
                }

                else if (obj.ModuleCode.ToLower().Equals("miscellaneouspayment"))
                {
                    MiscellaneousPaymentForm miscellaneousPaymentForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    miscellaneousPaymentForm.OpenStudentSelection();
                    MiscellaneousPaymentPage miscellaneousPaymentPage = new()
                    {
                        BindingContext = miscellaneousPaymentForm
                    };
                    await Navigation.PushAsync(miscellaneousPaymentPage);
                }

                else if (obj.ModuleCode.ToLower().Equals("library"))
                {
                    LibraryForm libraryForm = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    LibraryPage libraryPage = new()
                    {
                        BindingContext = libraryForm
                    };
                    await Navigation.PushAsync(libraryPage);
                    libraryForm.OpenStudentSelection();
                }
                else if (obj.ModuleCode.ToLower().Equals("teacherevaluation"))
                {
                    TeacherEvaluationForm teacherEvaluation = new(_mapper, _nativeServices, Navigation)
                    {
                        PageTitle = obj.ModuleName,
                        MenuVisible = true,
                        SelectedModule = obj
                    };
                    AppSettings.Current.IsDisplayAllStudentList = false;
                    TeacherEvaluationPage teacherEvaluationPage = new()
                    {
                        BindingContext = teacherEvaluation
                    };
                    await Navigation.PushAsync(teacherEvaluationPage);
                    teacherEvaluation.OpenStudentSelection();
                }


                //
                //     Func<PopupPage, bool> predicate = x => x.GetType() == typeof(SideMenuPanel);
                //     while (PopupNavigation.Instance.PopupStack.Any(predicate))
                //     {
                //         var popupPage = PopupNavigation.Instance.PopupStack.FirstOrDefault(predicate);
                //         await PopupNavigation.Instance.RemovePageAsync(popupPage);
                //     }
                //
                //     if (ApiHelper.ValidateInternetConnectivity())
                //     {
                //         Func<PopupPage, bool> predicate1 = x => x.GetType() == typeof(ProcessingIndicatorPopup);
                //         if (PopupNavigation.Instance.PopupStack.Any(predicate1))
                //         {
                //             var popupPage = PopupNavigation.Instance.PopupStack.FirstOrDefault(predicate1);
                //             try
                //             {
                //                 await PopupNavigation.Instance.RemovePageAsync(popupPage);
                //             }
                //             catch (Exception ex)
                //             {
                //             }
                //         }
                //     }
                //     else
                //     {
                //         await ApiHelper.HideProcessingIndicatorPopup();
                //     }
                //
                //     await Task.Delay(300);
                isMenuClicked = false;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    // public async Task SideMenuClicked()
    // {
    //     try
    //     {
    //         var color = Color.FromHex(AppSettings.Current.Settings.ThemeColor);
    //         HighlightColor = color.WithLuminosity(color.Luminosity - color.Luminosity * .1);
    //
    //         if (menuPage != null)
    //             await PopupNavigation.Instance.PushAsync(menuPage, true);
    //         else
    //             await PopupNavigation.Instance.PushAsync(new SideMenuPanel(this), true);
    //         HighlightColor = Color.Transparent;
    //     }
    //     catch (Exception ex)
    //     {
    //         Crashes.TrackError(ex);
    //     }
    // }

    // public async void BeamMenuClicked()
    // {
    //     try
    //     {
    //         var color = Color.FromHex(AppSettings.Current.Settings.ThemeColor);
    //         HighlightColor = color.WithLuminosity(color.Luminosity - color.Luminosity * .1);
    //
    //         if (menuPage != null)
    //             await PopupNavigation.Instance.PushAsync(menuPage, true);
    //         else
    //             await PopupNavigation.Instance.PushAsync(new SideMenuPanel(this), true);
    //         HighlightColor = Color.Transparent;
    //     }
    //     catch (Exception ex)
    //     {
    //         Crashes.TrackError(ex);
    //     }
    // }
    
    public void BeamMenuClicked()
    {
        try
        {
            var color = Color.FromArgb(AppSettings.Current.Settings.ThemeColor); // Use FromArgb in .NET MAUI
            HighlightColor = color;
            var currentPage = GetCurrentPage();
            
            
            var sideMenuPanel = new SideMenuPanel()
            {
                BindingContext = this
            };
            SetPopupInstance(sideMenuPanel);
            Application.Current.MainPage.ShowPopup(sideMenuPanel);

            // if (MenuPage != null)
            // { 
            //     currentPage.ShowPopup(new SideMenuPanel(this));
            // }
            // else
            // {
            //     currentPage.ShowPopup(new SideMenuPanel(this));
            // }
            HighlightColor = Colors.Transparent;
        }
        catch (Exception ex)
        {
            // Crashes.TrackError(ex);
            HelperMethods.DisplayException(ex, PageTitle);
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

    public async void BeamHeaderMessageIconClicked()
    {
        CommunicationForm communicationForm = new (_mapper, _nativeServices, Navigation)
        {
            MenuVisible = true,
            PageTitle = TextResource.InboxText
        };

        CommunicationPage communicationPage = new ()
        {
            BindingContext = communicationForm
        };
        await Navigation.PushAsync(communicationPage);
        var data = AppSettings.Current.FooterMenuList?.Where(x => x.ModuleCode?.ToLower() == "communication")?.FirstOrDefault();
        if (data != null && !string.IsNullOrEmpty(data.ModuleCode))
        {
            AppSettings.Current.SelectedFooterMenu = data;
            AppSettings.Current.SelectedFooterMenu.IsSelected = true;
            AppSettings.Current.FooterMenuList.ToList()
                .FindAll(b => b.ModuleCode != AppSettings.Current.SelectedFooterMenu.ModuleCode)
                .ForEach(b => b.IsSelected = false);
        }
        else
        {
            AppSettings.Current.SelectedFooterMenu = null;
        }
    }

    public async void BeamHeaderNotificationIconClicked()
    {
        MessageFromSchoolForm messageFromSchoolForm = new(_mapper, _nativeServices, Navigation)
        {
            PageTitle = AppSettings.Current?.MenuStructureList != null
                ? AppSettings.Current.MenuStructureList
                    .Where(x => x.ModuleCode != null && x.ModuleCode.ToLower().Equals("messagefromschool"))
                    ?.FirstOrDefault()?.ModuleName
                : string.Empty
        };
        MessageFromSchool messageFromSchool = new ()
        {
            BindingContext = messageFromSchoolForm
        };
        await Navigation.PushAsync(messageFromSchool);
        
        var data = AppSettings.Current.FooterMenuList?.Where(x => x.ModuleCode?.ToLower() == "messagefromschool")
            ?.FirstOrDefault();
        if (data != null && !string.IsNullOrEmpty(data.ModuleCode))
        {
            AppSettings.Current.SelectedFooterMenu = data;
            AppSettings.Current.SelectedFooterMenu.IsSelected = true;
            AppSettings.Current.FooterMenuList.ToList()
                .FindAll(b => b.ModuleCode != AppSettings.Current.SelectedFooterMenu.ModuleCode)
                .ForEach(b => b.IsSelected = false);
        }
        else
        {
            AppSettings.Current.SelectedFooterMenu = null;
        }
    }

    private void LogoutClicked(object obj)
    {
        HelperMethods.Logout(_mapper, _nativeServices, Navigation);
    }

    #endregion

    #endregion
}