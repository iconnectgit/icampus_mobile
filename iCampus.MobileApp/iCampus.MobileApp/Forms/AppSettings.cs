using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Forms.UserModules.Registration;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms;

public class AppSettings : INotifyPropertyChanged
    {
        public static AppSettings Current = new AppSettings();

        #region Properties
        private Popup _currentPopup;
        public Popup CurrentPopup
        {
            get { return _currentPopup; }
            set
            {
                _currentPopup = value;
                OnPropertyChanged("CurrentPopup");
            }
        }
        private MobileAppSettingsView _settings;
        public MobileAppSettingsView Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                OnPropertyChanged("Settings");
            }
        }

        private bool _isStudentFooterVisible;
        public bool IsStudentFooterVisible
        {
            get
            {
                return _isStudentFooterVisible && (AppSettings.Current.SelectedStudent != null && AppSettings.Current.SelectedStudent.ItemId != null);
            }
            set
            {
                _isStudentFooterVisible = value;
                OnPropertyChanged("IsStudentFooterVisible");
            }
        }
        private bool _isStudentFooterVisibleForAllStudentList;
        public bool IsStudentFooterVisibleForAllStudentList
        {
            get
            {
                return _isStudentFooterVisibleForAllStudentList && (AppSettings.Current.SelectedStudentFromAllStudentList != null && AppSettings.Current.SelectedStudentFromAllStudentList.ItemId != null);
            }
            set
            {
                _isStudentFooterVisibleForAllStudentList = value;
                OnPropertyChanged("IsStudentFooterVisibleForAllStudentList");
            }
        }

        IList<BindableStudentPickListItem> _studentList = new List<BindableStudentPickListItem>();
        public IList<BindableStudentPickListItem> StudentList
        {
            get => _studentList;
            set
            {
                _studentList = value;
                OnPropertyChanged("StudentList");
            }
        }

        BindableStudentPickListItem _selectedStudent = new BindableStudentPickListItem();
        public BindableStudentPickListItem SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged("SelectedStudent");
            }
        }

        IList<BindableModuleStructureView> _menuStructureList = new List<BindableModuleStructureView>();
        public IList<BindableModuleStructureView> MenuStructureList
        {
            get => _menuStructureList;
            set
            {
                _menuStructureList = value;
                OnPropertyChanged("MenuStructureList");
            }
        }

        private string _logoUrl;
        public string LogoUrl
        {
            get { return _logoUrl; }
            set
            {
                _logoUrl = value;
                OnPropertyChanged("LogoUrl");
            }
        }

        private byte[] _logoData;
        public byte[] LogoData
        {
            get
            {
                if (ApiHelper.ValidateInternetConnectivity() && LogoUrl != null)
                {
                    _logoData = HelperMethods.GetStreamFromUrl(LogoUrl);
                    return _logoData;
                }
                else
                    return _logoData;
            }
            set
            {
                _logoData = value;
                OnPropertyChanged("LogoData");
            }
        }

        private string _userName;
        public string UserLoginName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }
        private string _userFullName;
        public string UserFullName
        {
            get { return _userFullName; }
            set
            {
                _userFullName = value;
                OnPropertyChanged("UserFullName");
            }
        }
        private string _userPassword;

        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                _userPassword = value;
                OnPropertyChanged("UserPassword");
            }
        }

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }
        public string _communicationMessageType;
        public string CommunicationMessageType
        {
            get { return _communicationMessageType; }
            set
            {
                _communicationMessageType = value;
                OnPropertyChanged("CommunicationMessageType");
            }
        }
        public string _transactionFailedMessage;
        public string TransactionFailedMessage
        {
            get { return _transactionFailedMessage; }
            set
            {
                _transactionFailedMessage = value;
                OnPropertyChanged("TransactionFailedMessage");
            }
        }
        public string _paymentCancelledMessage;
        public string PaymentCancelledMessage
        {
            get { return _paymentCancelledMessage; }
            set
            {
                _paymentCancelledMessage = value;
                OnPropertyChanged("PaymentCancelledMessage");
            }
        }
        public MemoryStream _taxReceiptStream;
        public MemoryStream TaxReceiptStream
        {
            get { return _taxReceiptStream; }
            set
            {
                _taxReceiptStream = value;
                OnPropertyChanged("TaxReceiptStream");
            }
        }
        private bool _isPaymentSuccessful;
        public bool IsPaymentSuccessful
        {
            get { return _isPaymentSuccessful; }
            set
            {
                _isPaymentSuccessful = value;
                OnPropertyChanged("IsPaymentSuccessful");
            }
        }
        private bool _isPaymentWebViewBackButtonVisible = false;
        public bool IsPaymentWebViewBackButtonVisible
        {
            get { return _isPaymentWebViewBackButtonVisible; }
            set
            {
                _isPaymentWebViewBackButtonVisible = value;
                OnPropertyChanged("IsPaymentWebViewBackButtonVisible");
            }
        }
        public bool _isvisibleTeacherListInfoIcon;
        public bool IsVisibleTeacherListInfoIcon
        {
            get { return _isvisibleTeacherListInfoIcon; }
            set
            {
                _isvisibleTeacherListInfoIcon = value;
                OnPropertyChanged("IsVisibleTeacherListInfoIcon");
            }
        }
        public bool _isParent;
        public bool IsParent
        {
            get { return _isParent; }
            set
            {
                _isParent = value;
                OnPropertyChanged("IsParent");
            }
        }
        public bool _isTeacher;
        public bool IsTeacher
        {
            get { return _isTeacher; }
            set
            {
                _isTeacher = value;
                OnPropertyChanged("IsTeacher");
            }
        }
        bool _isStudent;
        public bool IsStudent
        {
            get { return _isStudent; }
            set
            {
                _isStudent = value;
                OnPropertyChanged("IsStudent");
            }
        }
        public PortalUserTypes PortalUserType { get; set; }

        private bool _isAlertsFromPushNotifications;
        public bool IsAlertsFromPushNotifications
        {
            get
            {
                return _isAlertsFromPushNotifications;
            }
            set
            {
                _isAlertsFromPushNotifications = value;
                OnPropertyChanged("IsAlertsFromPushNotifications");
            }
        }

        private bool _isAppointmentFromPushNotifications;
        public bool IsAppointmentFromPushNotifications
        {
            get
            {
                return _isAppointmentFromPushNotifications;
            }
            set
            {
                _isAppointmentFromPushNotifications = value;
                OnPropertyChanged("IsAppointmentFromPushNotifications");
            }
        }

        private int _cacheRecordSize;
        public int CacheRecordSize
        {
            get
            {
                return _cacheRecordSize;
            }
            set
            {
                _cacheRecordSize = value;
                OnPropertyChanged("CacheRecordSize");
            }
        }
        public string _selectedMarksReportCardTerm;
        public string SelectedMarksReportCardTerm
        {
            get { return _selectedMarksReportCardTerm; }
            set
            {
                _selectedMarksReportCardTerm = value;
                OnPropertyChanged("SelectedMarksReportCardTerm");
            }
        }
        public double _selectedMarksReportCardTermId;
        public double SelectedMarksReportCardTermId
        {
            get { return _selectedMarksReportCardTermId; }
            set
            {
                _selectedMarksReportCardTermId = value;
                OnPropertyChanged("SelectedMarksReportCardTermId");
            }
        }
        public double _selectedSkillReportCardTermId;
        public double SelectedSkillReportCardTermId
        {
            get { return _selectedSkillReportCardTermId; }
            set
            {
                _selectedSkillReportCardTermId = value;
                OnPropertyChanged("SelectedSkillReportCardTermId");
            }
        }
        public string _selectedSkillReportCardTerm;
        public string SelectedSkillReportCardTerm
        {
            get { return _selectedSkillReportCardTerm; }
            set
            {
                _selectedSkillReportCardTerm = value;
                OnPropertyChanged("SelectedSkillReportCardTerm");
            }
        }
        public int? UserRefId { get; set; }
        public int? UserId { get; set; }
        public int SchoolCampusId { get; set; }
        public string ApiUrl { get; set; }
        public Guid? UserSessionUid { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public FileUploadSettingsView FileUploadSettings { get; set; }
        public List<SurveyView> SurveyViews { get; set; }
        public ObservableCollection<DataCollectionFieldsView> DataCollectionViews { get; set; }
        private DateTime _schoolNextWorkingDate;
        public DateTime SchoolNextWorkingDate
        {
            get
            {
                return _schoolNextWorkingDate;
            }
            set
            {
                _schoolNextWorkingDate = value;
            }
        }

        private string _settingsMenuImageUrl;
        public string SettingsMenuImageUrl
        {
            get { return _settingsMenuImageUrl; }
            set
            {
                _settingsMenuImageUrl = value;
                OnPropertyChanged("SettingsMenuImageUrl");
            }
        }
        private string _homeMenuImageUrl;
        public string HomeMenuImageUrl
        {
            get { return _homeMenuImageUrl; }
            set
            {
                _homeMenuImageUrl = value;
                OnPropertyChanged("HomeMenuImageUrl");
            }
        }
        bool _isMonthlyView;
        public bool IsMonthlyView
        {
            get { return _isMonthlyView; }
            set
            {
                _isMonthlyView = value;
                OnPropertyChanged("IsMonthlyView");
            }
        }

        private bool _isPushNotificationEnable;
        public bool IsPushNotificationEnable
        {
            get => _isPushNotificationEnable;
            set
            {
                if (_isPushNotificationEnable != value)
                {
                    _isPushNotificationEnable = value;
                    OnPropertyChanged(nameof(IsPushNotificationEnable));
                }
            }
        }

        public bool IsSessionExpiredAlert { get; set; }

        public int MaxAllowedRecipientCount { get; set; }

        public List<string> DataCollectionFormIdList { get; set; }
        private string _onlinePaymentCurrencyCode;
        public string OnlinePaymentCurrencyCode
        {
            get { return _onlinePaymentCurrencyCode; }
            set
            {
                _onlinePaymentCurrencyCode = value;
                OnPropertyChanged("OnlinePaymentCurrencyCode");
            }
        }
        private int? _onlinePaymentCurrencyRoundingDigits;
        public int? OnlinePaymentCurrencyRoundingDigits
        {
            get { return _onlinePaymentCurrencyRoundingDigits; }
            set
            {
                _onlinePaymentCurrencyRoundingDigits = value;
                OnPropertyChanged("OnlinePaymentCurrencyRoundingDigits");
            }
        }
        IList<BindableStudentPickListItem> _allStudentList = new List<BindableStudentPickListItem>();
        public IList<BindableStudentPickListItem> AllStudentList
        {
            get => _allStudentList;
            set
            {
                _allStudentList = value;
                OnPropertyChanged("AllStudentList");
            }
        }
        BindableStudentPickListItem _selectedStudentFromAllStudentList = new BindableStudentPickListItem();
        public BindableStudentPickListItem SelectedStudentFromAllStudentList
        {
            get => _selectedStudentFromAllStudentList;
            set
            {
                _selectedStudentFromAllStudentList = value;
                OnPropertyChanged("SelectedStudentFromAllStudentList");
            }
        }

        IList<BindableStudentPickListItem> _registeredStudentList = new List<BindableStudentPickListItem>();
        public IList<BindableStudentPickListItem> RegisteredStudentList
        {
            get => _registeredStudentList;
            set
            {
                _registeredStudentList = value;
                OnPropertyChanged("RegisteredStudentList");
            }
        }
        BindableStudentPickListItem _selectedStudentFromRegisteredStudentList = new BindableStudentPickListItem();
        public BindableStudentPickListItem SelectedStudentFromRegisteredStudentList
        {
            get => _selectedStudentFromRegisteredStudentList;
            set
            {
                _selectedStudentFromRegisteredStudentList = value;
                OnPropertyChanged("SelectedStudentFromRegisteredStudentList");
            }
        }
        
        ObservableCollection<BindableModuleStructureView> _footerMenuList = new ObservableCollection<BindableModuleStructureView>();
        public ObservableCollection<BindableModuleStructureView> FooterMenuList
        {
            get => _footerMenuList;
            set
            {
                _footerMenuList = value;
                OnPropertyChanged("FooterMenuList");
            }
        }
        
        BindableModuleStructureView _selectedFooterMenu = new BindableModuleStructureView();
        public BindableModuleStructureView SelectedFooterMenu
        {
            get => _selectedFooterMenu;
            set
            {
                _selectedFooterMenu = value;
                OnPropertyChanged("SelectedFooterMenu");
            }
        }
        public int FooterMenuListCount
        {
            get => this.FooterMenuList !=null && this.FooterMenuList.Count > 0 ? this.FooterMenuList.Count  : 2 ;
            
        }

        IList<BindableModuleStructureView> _landingPageMenuList = new List<BindableModuleStructureView>();
        public IList<BindableModuleStructureView> LandingPageMenuList
        {
            get
            {
                return _landingPageMenuList;
            }
            set
            {
                _landingPageMenuList = value;
                OnPropertyChanged("LandingPageMenuList");
            }
        }

        BindableModuleStructureView _selectedLandingScreenMenu = new BindableModuleStructureView();
        public BindableModuleStructureView SelectedLandingScreenMenu
        {
            get => _selectedLandingScreenMenu;
            set
            {
                _selectedLandingScreenMenu = value;
                OnPropertyChanged("SelectedLandingScreenMenu");
            }
        }
        private bool _isDisplayAllStudentList = false;
        public bool IsDisplayAllStudentList
        {
            get
            {
                return _isDisplayAllStudentList;
            }
            set
            {
                _isDisplayAllStudentList = value;
                OnPropertyChanged("IsDisplayAllStudentList");
            }
        }
        private string _detailedDisplayName;
        public string DetailedDisplayName
        {
            get { return _detailedDisplayName; }
            set
            {
                _detailedDisplayName = value;
                OnPropertyChanged("DetailedDisplayName");
            }
        }
        public string PortalUrl { get; set; }
        private bool _isBeamLoginViewVisible = false;

        public bool IsBeamLoginViewVisible
        {
            get {
                _isBeamLoginViewVisible = (!string.IsNullOrEmpty(App.ClientGroupCode) && App.ClientGroupCode.ToLower().Equals("beam")) ? true : false;
                return _isBeamLoginViewVisible;
            }
            set
            {
                _isBeamLoginViewVisible = value;
                OnPropertyChanged("IsBeamLoginViewVisible");
            }
        }

        private bool _isOtherAppsLoginViewVisible = false;
        public bool IsOtherAppsLoginViewVisible
        {
            get {
                _isOtherAppsLoginViewVisible = (!string.IsNullOrEmpty(App.ClientGroupCode) && App.ClientGroupCode.ToLower().Equals("beam")) ? false : true;
                return _isOtherAppsLoginViewVisible;
            }
            set
            {
                _isOtherAppsLoginViewVisible = value;
                OnPropertyChanged("IsOtherAppsLoginViewVisible");
            }
        }

        private bool _isAllStudentListVisible = false;
        public bool IsAllStudentListVisible
        {
            get { return _isAllStudentListVisible; }
            set
            {
                _isAllStudentListVisible = value;
                OnPropertyChanged("IsAllStudentListVisible");
            }
        }

        private bool _isRegisteredStudentListVisible = false;
        public bool IsRegisteredStudentListVisible
        {
            get { return _isRegisteredStudentListVisible; }
            set
            {
                _isRegisteredStudentListVisible = value;
                OnPropertyChanged("IsRegisteredStudentListVisible");
            }
        }

        private Color _backgroundColor=Colors.WhiteSmoke;
        public Color BackgroundColor
        {
            get {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        int _beamHeaderNotificationCount;
        public int BeamHeaderNotificationCount
        {
            get { return _beamHeaderNotificationCount; }
            set
            {
                _beamHeaderNotificationCount = value;
                OnPropertyChanged("BeamHeaderNotificationCount");
            }
        }
        int _assignmentNotificationCount;
        public int AssignmentNotificationCount
        {
            get { return _assignmentNotificationCount; }
            set
            {
                _assignmentNotificationCount = value;
                OnPropertyChanged("AssignmentNotificationCount");
            }
        }
        string _beamHeaderImage;
        public string BeamHeaderImage
        {
            get { return _beamHeaderImage; }
            set
            {
                _beamHeaderImage = value;
                OnPropertyChanged("BeamHeaderImage");
            }
        }

        string _teacherDesignation;
        public string TeacherDesignation
        {
            get { return _teacherDesignation; }
            set
            {
                _teacherDesignation = value;
                OnPropertyChanged("TeacherDesignation");
            }
        }

        string _studentDesignation;
        public string StudentDesignation
        {
            get { return _studentDesignation; }
            set
            {
                _studentDesignation = value;
                OnPropertyChanged("StudentDesignation");
            }
        }

        ModuleStructureView _logoutItem = new ModuleStructureView();
        public ModuleStructureView LogoutItem
        {
            get => _logoutItem;
            set
            {
                _logoutItem = value;
                OnPropertyChanged("LogoutItem");
            }
        }
        string _displayId;
        public string DisplayId
        {
            get { return _displayId; }
            set
            {
                _displayId = value;
                OnPropertyChanged("DisplayId");
            }
        }
        bool _beamCommunicationAndBellIconVisibility = true;
        public bool BeamCommunicationAndBellIconVisibility
        {
            get => _beamCommunicationAndBellIconVisibility;
            set
            {
                _beamCommunicationAndBellIconVisibility = value;
                OnPropertyChanged("BeamCommunicationAndBellIconVisibility");
            }
        }
        public string _fatherEmail;
        public string FatherEmail
        {
            get { return _fatherEmail; }
            set
            {
                _fatherEmail = value;
                OnPropertyChanged("FatherEmail");
            }
        }
        public string _motherEmail;
        public string MotherEmail
        {
            get { return _motherEmail; }
            set
            {
                _motherEmail = value;
                OnPropertyChanged("MotherEmail");
            }
        }
        string _weekendDays = string.Empty;
        public string WeekendDays
        {
            get { return _weekendDays; }
            set
            {
                _weekendDays = value;
                OnPropertyChanged("WeekendDays");
            }
        }
        bool _isWeekendsDisabled=false;
        public bool IsWeekendsDisabled
        {
            get { return _isWeekendsDisabled; }
            set
            {
                _isWeekendsDisabled = value;
                OnPropertyChanged("IsWeekendsDisabled");
            }
        }

        bool _isCurrentWeekDisabled = false;
        public bool IsCurrentWeekDisabled
        {
            get { return _isCurrentWeekDisabled; }
            set
            {
                _isCurrentWeekDisabled = value;
                OnPropertyChanged("IsCurrentWeekDisabled");
            }
        }
        bool _refreshCalendarData = false;
        public bool RefreshCalendarData
        {
            get { return _refreshCalendarData; }
            set
            {
                _refreshCalendarData = value;
                OnPropertyChanged("RefreshCalendarData");
            }
        }
        bool _refreshResourseData = false;
        public bool RefreshResourseData
        {
            get { return _refreshResourseData; }
            set
            {
                _refreshResourseData = value;
                OnPropertyChanged("RefreshResourseData");
            }
        }
        bool _isMandatoryUpdate;
        public bool IsMandatoryUpdate
        {
            get { return _isMandatoryUpdate; }
            set
            {
                _isMandatoryUpdate = value;
                OnPropertyChanged("IsMandatoryUpdate");
            }
        }
        string _versionNumber;
        public string VersionNumber
        {
            get { return _versionNumber; }
            set
            {
                _versionNumber = value;
                OnPropertyChanged("VersionNumber");
            }
        }
        int _weekStartDay;
        public int WeekStartDay
        {
            get { return _weekStartDay; }
            set
            {
                _weekStartDay = value;
                OnPropertyChanged("WeekStartDay");
            }
        }
        DateTime _schoolWeekStartDate;
        public DateTime SchoolWeekStartDate
        {
            get { return _schoolWeekStartDate; }
            set
            {
                _schoolWeekStartDate = value;
                OnPropertyChanged("SchoolWeekStartDate");
            }
        }
        bool _refreshComplaintsList = false;
        public bool RefreshComplaintsList
        {
            get { return _refreshComplaintsList; }
            set
            {
                _refreshComplaintsList = value;
                OnPropertyChanged("RefreshComplaintsList");
            }
        }
        bool _refreshTeacherAppointmentList = false;
        public bool RefreshTeacherAppointmentList
        {
            get { return _refreshTeacherAppointmentList; }
            set
            {
                _refreshTeacherAppointmentList = value;
                OnPropertyChanged("RefreshTeacherAppointmentList");
            }
        }
        bool _refreshFamilyAppointmentList = false;
        public bool RefreshFamilyAppointmentList
        {
            get { return _refreshFamilyAppointmentList; }
            set
            {
                _refreshFamilyAppointmentList = value;
                OnPropertyChanged("RefreshFamilyAppointmentList");
            }
        }
        bool _refreshNotificationCenterData = false;
        public bool RefreshNotificationCenterData
        {
            get { return _refreshNotificationCenterData; }
            set
            {
                _refreshNotificationCenterData = value;
                OnPropertyChanged("RefreshNotificationCenterData");
            }
        }

        ObservableCollection<BindableFormFieldsView> _familyDetails = new ObservableCollection<BindableFormFieldsView>();
        public ObservableCollection<BindableFormFieldsView> FamilyDetails
        {
            get => _familyDetails;
            set
            {
                _familyDetails = value;
                OnPropertyChanged("FamilyDetails");
            }
        }
        List<IDictionary<string, object>> _formData;
        public List<IDictionary<string, object>> FormData
        {
            get { return _formData; }
            set
            {
                _formData = value;
                OnPropertyChanged("FormData");
            }
        }
        long? _familyId;
        public long? FamilyId
        {
            get { return _familyId; }
            set
            {
                _familyId = value;
                OnPropertyChanged("FamilyId");
            }
        }
        ObservableCollection<FileDataView> _attachmentDetail = new ObservableCollection<FileDataView>();
        public ObservableCollection<FileDataView> AttachmentDetail
        {
            get => _attachmentDetail;
            set
            {
                _attachmentDetail = value;
                OnPropertyChanged("AttachmentDetail");
            }
        }
        ObservableCollection<FileDataView> _existingAttachmentDetail = new ObservableCollection<FileDataView>();
        public ObservableCollection<FileDataView> ExistingAttachmentDetail
        {
            get => _existingAttachmentDetail;
            set
            {
                _existingAttachmentDetail = value;
                OnPropertyChanged("ExistingAttachmentDetail");
            }
        }
        string _deletedAttachmentFiles;
        public string DeletedAttachmentFiles
        {
            get => _deletedAttachmentFiles;
            set
            {
                _deletedAttachmentFiles = value;
                OnPropertyChanged("DeletedAttachmentFiles");
            }
        }
        ObservableCollection<BindableFormFieldsView> _studentDetails = new ObservableCollection<BindableFormFieldsView>();
        public ObservableCollection<BindableFormFieldsView> StudentDetails
        {
            get => _studentDetails;
            set
            {
                _studentDetails = value;
                OnPropertyChanged("StudentDetails");
            }
        }
        int _registrationId;
        public int RegistrationId
        {
            get { return _registrationId; }
            set
            {
                _registrationId = value;
                OnPropertyChanged("RegistrationId");
            }
        }
        int _academicYear;
        public int AcademicYear
        {
            get { return _academicYear; }
            set
            {
                _academicYear = value;
                OnPropertyChanged("AcademicYear");
            }
        }
        private string _gradeUrl;
        public string GradeUrl
        {
            get { return _gradeUrl; }
            set
            {
                _gradeUrl = value;
                OnPropertyChanged("GradeUrl");
            }
        }
        bool _isGenderWiseSettingEnabled;
        public bool IsGenderWiseSettingEnabled
        {
            get { return _isGenderWiseSettingEnabled; }
            set
            {
                _isGenderWiseSettingEnabled = value;
                OnPropertyChanged("IsGenderWiseSettingEnabled");
            }
        }
        ObservableCollection<BindableFormFieldsView> _kGStudentDetails = new ObservableCollection<BindableFormFieldsView>();
        public ObservableCollection<BindableFormFieldsView> KGStudentDetails
        {
            get => _kGStudentDetails;
            set
            {
                _kGStudentDetails = value;
                OnPropertyChanged("KGStudentDetails");
            }
        }
        
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }
        FamilySessionData _familyData = new FamilySessionData();
        public FamilySessionData FamilyData
        {
            get => _familyData;
            set
            {
                _familyData = value;
                OnPropertyChanged("FamilyData");
            }
        }
        
        string _academicYearTitle;
        public string AcademicYearTitle
        {
            get => _academicYearTitle;
            set
            {
                _academicYearTitle = value;
                OnPropertyChanged("AcademicYearTitle");
            }
        }
        bool _circularIconVisibility = true;
        public bool CircularIconVisibility
        {
            get => _circularIconVisibility;
            set
            {
                _circularIconVisibility = value;
                OnPropertyChanged("CircularIconVisibility");
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }