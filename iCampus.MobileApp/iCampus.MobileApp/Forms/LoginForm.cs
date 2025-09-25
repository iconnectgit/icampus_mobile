using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Library.FormValidation;
using iCampus.MobileApp.Views;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.Portal.ViewModels;
#if IOS
    using Foundation;
#elif ANDROID
    using Firebase;
    using Firebase.Crashlytics;
#endif

namespace iCampus.MobileApp.Forms;

public class LoginForm : ViewModelBase
{
    #region Declarations
    public INavigation Navigation { get; set; }
    private readonly INativeServices _nativeServices;
    private readonly IMapper _mapper;
    bool isEmailValid;
    bool isPasswordValid;
    
    private string clientCode;
    private string troubleSignInEmail;
    public ICommand EmailTextChangedEventCommand { get; set; }
    public ICommand PasswordTextChangedEventCommand { get; set; }
    public ICommand SignInButtonCommand { get; set; }
    public ICommand TroubleSignInCommand { get; set; }
    public ICommand ContinueOTPCommand { get; set; }

    private string clientGroupCode;
    
    #endregion

    #region Properties

    private ValidatableObject<string> _email;
    public ValidatableObject<string> Email
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    private ValidatableObject<string> _password;
    public ValidatableObject<string> Password
    {
        get => _password;
        set
        {
            if (_password != value)
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
    }

    private ValidatableObject<string> _loginFailed;
    public ValidatableObject<string> LoginFailed
    {
        get => _loginFailed;
        set
        {
            if (_loginFailed != value)
            {
                _loginFailed = value;
                OnPropertyChanged(nameof(LoginFailed));
            }
        }
    }

    private string _iCampusText;
    public string ICampusText
    {
        get => _iCampusText;
        set
        {
            if (_iCampusText != value)
            {
                _iCampusText = value;
                OnPropertyChanged(nameof(ICampusText));
            }
        }
    }

    private string _troubleSignInInformation;
    public string TroubleSignInInformation
    {
        get => _troubleSignInInformation;
        set
        {
            if (_troubleSignInInformation != value)
            {
                _troubleSignInInformation = value;
                OnPropertyChanged(nameof(TroubleSignInInformation));
            }
        }
    }

    private bool _isEmailErrorLabelVisible;
    public bool IsEmailErrorLableVisible
    {
        get => _isEmailErrorLabelVisible;
        set
        {
            if (_isEmailErrorLabelVisible != value)
            {
                _isEmailErrorLabelVisible = value;
                OnPropertyChanged(nameof(IsEmailErrorLableVisible));
            }
        }
    }

    private Color _emailEntryErrorColor;
    public Color EmailErrorEntryColor
    {
        get => _emailEntryErrorColor;
        set
        {
            if (_emailEntryErrorColor != value)
            {
                _emailEntryErrorColor = value;
                OnPropertyChanged(nameof(EmailErrorEntryColor));
            }
        }
    }

    private Color _passwordEntryErrorColor;
    public Color PasswordErrorEntryColor
    {
        get => _passwordEntryErrorColor;
        set
        {
            if (_passwordEntryErrorColor != value)
            {
                _passwordEntryErrorColor = value;
                OnPropertyChanged(nameof(PasswordErrorEntryColor));
            }
        }
    }

    private bool _isPasswordErrorLabelVisibleNew;
    public bool PasswordErrorEntryColorNew
    {
        get => _isPasswordErrorLabelVisibleNew;
        set
        {
            if (_isPasswordErrorLabelVisibleNew != value)
            {
                _isPasswordErrorLabelVisibleNew = value;
                OnPropertyChanged(nameof(PasswordErrorEntryColorNew));
            }
        }
    }

    private bool _isLoginFaliledLabelVisible;
    public bool IsLoginFailedLabelVisible
    {
        get => _isLoginFaliledLabelVisible;
        set
        {
            if (_isLoginFaliledLabelVisible != value)
            {
                _isLoginFaliledLabelVisible = value;
                OnPropertyChanged(nameof(IsLoginFailedLabelVisible));
            }
        }
    }

    private string _lblValidationMessage;
    public string LblValidationMessage
    {
        get => _lblValidationMessage;
        set
        {
            if (_lblValidationMessage != value)
            {
                _lblValidationMessage = value;
                OnPropertyChanged(nameof(LblValidationMessage));
            }
        }
    }

    private string _customLoginScreenBackground;
    public string CustomLoginScreenBackground
    {
        get => _customLoginScreenBackground;
        set
        {
            if (_customLoginScreenBackground != value)
            {
                _customLoginScreenBackground = value;
                OnPropertyChanged(nameof(CustomLoginScreenBackground));
            }
        }
    }
    private string _logo = "icampus_icon_logo.png";
    public string Logo
    {
        get => _logo;
        set
        {
            if (_logo != value)
            {
                _logo = value;
                OnPropertyChanged(nameof(Logo));
            }
        }
    }

    private bool _poweredByInterconnectVisibility = true;
    public bool PoweredByInterconnectVisibility
    {
        get => _poweredByInterconnectVisibility;
        set
        {
            if (_poweredByInterconnectVisibility != value)
            {
                _poweredByInterconnectVisibility = value;
                OnPropertyChanged(nameof(PoweredByInterconnectVisibility));
            }
        }
    }

    public bool IsTitleVisible { get; set; }


    private Color _placeholderColor;
    public Color PlaceholderColor
    {
        get => _placeholderColor;
        set
        {
            if (_placeholderColor != value)
            {
                _placeholderColor = value;
                OnPropertyChanged(nameof(PlaceholderColor));
            }
        }
    }
    private string _oneTimePassword;
    public string OneTimePassword
    {
        get => _oneTimePassword;
        set
        {
            if (_oneTimePassword != value)
            {
                _oneTimePassword = value;
                OnPropertyChanged(nameof(OneTimePassword));
            }
        }
    }
    private string _otpEmailActionMessage;
    public string OtpEmailActionMessage
    {
        get => _otpEmailActionMessage;
        set
        {
            if (_otpEmailActionMessage != value)
            {
                _otpEmailActionMessage = value;
                OnPropertyChanged(nameof(OtpEmailActionMessage));
            }
        }
    }
    bool _isOtpEmailActionMessageVisible;
    public bool IsOtpEmailActionMessageVisible
    {
        get => _isOtpEmailActionMessageVisible;
        set
        {
            _isOtpEmailActionMessageVisible = value;
            OnPropertyChanged(nameof(IsOtpEmailActionMessageVisible));
        }
    }
    bool _isContinueButtonVisible = true;
    public bool IsContinueButtonVisible
    {
        get => _isContinueButtonVisible;
        set
        {
            _isContinueButtonVisible = value;
            OnPropertyChanged(nameof(IsContinueButtonVisible));
        }
    }
    private string _otpEmailActionIdentifier;
    public string OtpEmailActionIdentifier
    {
        get => _otpEmailActionIdentifier;
        set
        {
            if (_otpEmailActionIdentifier != value)
            {
                _otpEmailActionIdentifier = value;
                OnPropertyChanged(nameof(OtpEmailActionIdentifier));
            }
        }
    }
    private MobileAppLoginResultView _result;
    public MobileAppLoginResultView Result
    {
        get => _result;
        set
        {
            if (_result != value)
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }
    }
    public string NOOTP { get; set; } = string.Empty;

    #endregion
    
    public LoginForm(INavigation navigation, IMapper mapper, INativeServices nativeServices) : base(mapper, nativeServices, navigation)     
    {
        Navigation = navigation;
        _mapper = mapper;
        _nativeServices = nativeServices;
        InitializePage();
    }
    
     #region Methods
        private async void InitializePage()
        {
            SignInButtonCommand = new Command(ExecuteSignInButton);
            EmailTextChangedEventCommand = new Command(ExecuteEmailTextChangedCommand);
            PasswordTextChangedEventCommand = new Command(ExecutePasswordTextChangedCommand);
            TroubleSignInCommand = new Command(TroubleSignInClicked);
            ContinueOTPCommand = new Command(ContinueOTPCommandMethod);
            ICampusText = !string.IsNullOrEmpty(App.ClientCode) ? App.ClientCode : TextResource.ICampusText;
            IsTitleVisible = (!string.IsNullOrEmpty(App.ClientCode)||!string.IsNullOrEmpty(App.ClientGroupCode)) ? false : true;
            EmailErrorEntryColor = PasswordErrorEntryColor = Color.FromHex("D6D7DD");
            IsEmailErrorLableVisible = PasswordErrorEntryColorNew = IsLoginFailedLabelVisible = false;
            isEmailValid = isPasswordValid = false;
            _email = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            AddValidations();
            SetLoginScreenBackground();
            PoweredByInterconnectVisibility = clientGroupCode.ToLower().Equals("beam") ? false : true;
            await AutoLogin();

        }

        private async void ContinueOTPCommandMethod()
        {
            _nativeServices.GetDeviceID(async (deviceId) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(OneTimePassword))
                    {
                        IsOtpEmailActionMessageVisible = true;
                        OtpEmailActionMessage = "Please enter your OTP Code";
                    }
                    else
                    {
                        Result = await HelperMethods.PerformLogin(Email.Value, Password.Value, deviceId, OneTimePassword, OtpEmailActionIdentifier);
                    
                        if (!string.IsNullOrEmpty(Result.LoginMessage))
                        {
                            if (Result.MessageKey == "OtpSessionExpired")
                            {
                                AppSettings.Current.CurrentPopup?.Close();
                                LblValidationMessage = Result.LoginMessage;
                                IsLoginFailedLabelVisible = true;
                            }
                            else
                            {
                                OtpEmailActionMessage = Result.LoginMessage;
                                IsOtpEmailActionMessageVisible = true;
                                IsContinueButtonVisible = true;
                            }
                        }
                        else
                        {
                            AppSettings.Current.CurrentPopup?.Close();
                            await AppSettingsMethod();
                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex);
                }
            });
        }

        private void AddValidations()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = TextResource.EmptyEmailError
            });
            Email.Validations.Add(new EmailRule<string>
            {
                ValidationMessage = TextResource.InvalidEmailError
            });
            Password.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = TextResource.EmptyPasswordError
            });
        }
        private void ExecuteEmailTextChangedCommand()
        {
            isEmailValid = Email.Validate();
            if (isEmailValid)
            {
                IsEmailErrorLableVisible = false;
                EmailErrorEntryColor = Color.FromHex("D6D7DD");
            }
        }
        private void ExecutePasswordTextChangedCommand()
        {
            isPasswordValid = Password.Validate();
            if (isPasswordValid)
            {
                PasswordErrorEntryColorNew = false;
                PasswordErrorEntryColor = Color.FromHex("D6D7DD");
            }
        }
        private void ForceCrashForTesting()
        {
            string crashTest = null;
            crashTest.ToString(); 
        }
        private async void ExecuteSignInButton()
        {
            try
            {
                HelperMethods.LogEvent("login_execute_signin_button", 
                    $"Login ExecuteSignInButton - {Email.Value} - {DeviceInfo.Name} - {DeviceInfo.Model} - {DeviceInfo.Platform}");
                //ForceCrashForTesting(); 
            }
            catch (Exception ex)
            {
                var tokenStatus = string.IsNullOrEmpty(App.RefreshedToken) ? "Token_Empty" : "Token_Available";
                HelperMethods.LogEvent("Exception1", 
                    $"Exception1 - {ex.Message} - Token - {tokenStatus}");
                HelperMethods.DisplayException(ex);
            }
            if (IsLoginFormValid())
            {
                try
                {
                    _nativeServices.GetDeviceID(async (deviceId) =>
                    {
                        try
                        {
                            Result = await HelperMethods.PerformLogin(Email.Value, Password.Value, deviceId, NOOTP, "");
                            
                            if (String.IsNullOrEmpty(Result.LoginMessage))
                            {
                                if (Result.OtpSettings.EnableOtpVerification)
                                {
                                    OneTimePassword = String.Empty;
                                    OtpEmailActionIdentifier = Result.OtpEmailAction.Identifier;
                                    OtpEmailActionMessage = String.Empty;
                                    if (!Result.OtpEmailAction.Success)
                                    {
                                        OtpEmailActionMessage = Result.OtpEmailAction.Message;
                                        IsOtpEmailActionMessageVisible = true;
                                        IsContinueButtonVisible = false;
                                    }
                                    var otpPopup = new OtpPopup
                                    {
                                        BindingContext = this
                                    };
                                    SetPopupInstance(otpPopup);
                                    Application.Current.MainPage.ShowPopup(otpPopup);
                                }
                                else
                                {
                                    await AppSettingsMethod();
                                }
                            }
                            else
                            {
                                LblValidationMessage = Result.LoginMessage;
                                IsLoginFailedLabelVisible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            var tokenStatus = string.IsNullOrEmpty(App.RefreshedToken) ? "Token_Empty" : "Token_Available";
                            HelperMethods.LogEvent("Exception3", 
                                $"Exception3 - {ex.Message} - Token - {tokenStatus}");
                            HelperMethods.DisplayException(ex);
                            Preferences.Get("IsLogin", false);
                        }
                    });

                }
                catch (Exception ex)
                {
                    var tokenStatus = string.IsNullOrEmpty(App.RefreshedToken) ? "Token_Empty" : "Token_Available";
                    HelperMethods.LogEvent("Exception4", 
                        $"Exception4 - {ex.Message} - Token - {tokenStatus}");
                    HelperMethods.DisplayException(ex);
                    Preferences.Get("IsLogin", false);
                }
            }
        }

        private async Task AppSettingsMethod()
        {
            await ICCacheManager.SaveSecureObject<string>("icampus_pwd", Password.Value);
            await ICCacheManager.SaveSecureObject<string>("icampus_email", Email.Value);
            await ApiHelper.ShowProcessingIndicatorPopup();
            IsLoginFailedLabelVisible = false;
            AppSettings.Current = new AppSettings();
            AppSettings.Current.UserId = Result.UserSessionData.UserId;
            AppSettings.Current.SchoolCampusId = Result.SchoolCampusId;
            AppSettings.Current.UserLoginName = Result.UserSessionData.UserLoginName;
            AppSettings.Current.UserPassword = Result.UserSessionData.UserPassword;
            AppSettings.Current.DetailedDisplayName =
                Result.UserSessionData.DetailedDisplayName_1;
            AppSettings.Current.UserFullName = string.Concat(
                Result.UserSessionData.FirstName_1 + " " + Result.UserSessionData.LastName_1);
            AppSettings.Current.ApiUrl = Result.PortalApiUrl;
            AppSettings.Current.UserSessionUid = Result.UserSessionUid;
            AppSettings.Current.PortalUrl = Result.PortalUrl;
            AppSettings.Current.BeamHeaderNotificationCount = Result.UserNotificationCount;
            AppSettings.Current.AssignmentNotificationCount =
                Result.UserAssignmentNotificationCount;
            AppSettings.Current.BeamHeaderImage =
                Result.HeaderBackGroundImagePath?.Replace("https", "http") ?? string.Empty;
            AppSettings.Current.TeacherDesignation =
                Result.UserSessionData.TeacherAccessLevels != null
                    ? Result.UserSessionData.TeacherAccessLevels.Designation
                    : string.Empty;
            AppSettings.Current.StudentDesignation = Result.UserSessionData.StudentData != null
                ? Result.UserSessionData.StudentData.ClassName
                : string.Empty;

            AppSettings.Current.AcademicYearTitle = Result.AcademicYearTitle;
            AppSettings.Current.FamilyData =
                Result.UserSessionData.PortalUserType == PortalUserTypes.Parent
                    ? Result.UserSessionData.FamilyData
                    : new FamilySessionData();

            AppSettings.Current.IsMandatoryUpdate = Result.IsMandatoryUpdate;
            AppSettings.Current.VersionNumber = Result.VersionNumber;
            AppSettings.Current.FatherEmail =
                !string.IsNullOrEmpty(Result.UserSessionData.Email1)
                    ? Result.UserSessionData.Email1
                    : string.Empty;
            AppSettings.Current.MotherEmail =
                !string.IsNullOrEmpty(Result.UserSessionData.Email2)
                    ? Result.UserSessionData.Email2
                    : string.Empty;
            if (!string.IsNullOrEmpty(AppSettings.Current.FatherEmail) &&
                !string.IsNullOrEmpty(AppSettings.Current.MotherEmail) &&
                AppSettings.Current.FatherEmail.ToLower()
                    .Equals(AppSettings.Current.MotherEmail.ToLower()))
                AppSettings.Current.MotherEmail = string.Empty;

            AppSettings.Current.WeekStartDay = Result.WeekStartDay;
            AppSettings.Current.Settings = Result.MobileAppSettings;
            AppSettings.Current.LogoUrl = Result.LogoUrl;
            AppSettings.Current.DisplayName = Result.DisplayName;
            AppSettings.Current.FirstName = Result.FirstName;
            AppSettings.Current.SchoolNextWorkingDate =
                Result.SchoolNextWorkingDate.ToLocalTime();
            AppSettings.Current.MenuStructureList =
                _mapper.Map<List<BindableModuleStructureView>>(Result.MenuList.ToList());

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

            try
            {
                foreach (var item in AppSettings.Current.MenuStructureList)
                    if (item != null)
                        if (item.ModuleCode != null)
                        {
                            if (item.ModuleCode.ToLower().Equals("settings"))
                                AppSettings.Current.SettingsMenuImageUrl = item.ModuleImageUrl;
                            if (item.ModuleCode.ToLower().Equals("home"))
                                AppSettings.Current.HomeMenuImageUrl =
                                    item.ModuleImageUrl?.Replace("https", "http");
                        }

                if (AppSettings.Current.MenuStructureList != null)
                {
                    AppSettings.Current.CircularIconVisibility =
                        AppSettings.Current.MenuStructureList.Where(x =>
                            x.ModuleCode != null && x.ModuleCode.ToLower().Trim()
                                .Equals("messagefromschool")).FirstOrDefault() != null
                            ? true
                            : false;
                    AppSettings.Current.BeamCommunicationAndBellIconVisibility =
                        AppSettings.Current.MenuStructureList.Where(x =>
                            x.ModuleCode != null &&
                            x.ModuleCode.ToLower().Equals("communication")).FirstOrDefault() !=
                        null
                            ? true
                            : false;
                    AppSettings.Current.FooterMenuList =
                        new ObservableCollection<BindableModuleStructureView>(AppSettings
                            .Current.MenuStructureList?.Where(x =>
                                x.IsFooterMenuOnMobile && x.ModuleCode != null &&
                                !(x.ModuleCode.ToLower().Equals("home") ||
                                  x.ModuleCode.ToLower().Equals("contactus") ||
                                  x.ModuleCode.ToLower().Equals("messagefromschool") ||
                                  x.ModuleCode.ToLower().Equals("settings"))).Take(3).ToList());
                    var homeData = new ObservableCollection<BindableModuleStructureView>(
                        AppSettings.Current.MenuStructureList?.Where(
                            x => x.ModuleCode == "Home"));
                    AppSettings.Current.FooterMenuList.Insert(0, homeData.FirstOrDefault());
                    AppSettings.Current.FooterMenuList.Add(new BindableModuleStructureView()
                    {
                        ModuleCode = "News", ModuleShortName = "News", ModuleName = "News",
                        ShowIcon = true
                    });
                    AppSettings.Current.FooterMenuList.Where(x => x.ModuleCode.Contains("Home"))
                        .FirstOrDefault().IsSelected = true;
                    AppSettings.Current.SelectedFooterMenu = AppSettings.Current.FooterMenuList
                        .Where(x => x.ModuleCode.Contains("Home")).FirstOrDefault();
                    AppSettings.Current.LandingPageMenuList =
                        (from i in AppSettings.Current.MenuStructureList
                            let found = AppSettings.Current.FooterMenuList.Any(j => j == i)
                            where !found
                            select i).ToList();
                    var removeList = AppSettings.Current.LandingPageMenuList.Where(x =>
                        x.ModuleCode != null && (x.ModuleCode.ToLower().Equals("home") ||
                                                 x.ModuleCode.ToLower()
                                                     .Equals("communication") ||
                                                 x.ModuleCode.ToLower()
                                                     .Equals("messagefromschool"))).ToList();
                    AppSettings.Current.LandingPageMenuList =
                        (from i in AppSettings.Current.LandingPageMenuList
                            let found = removeList.Any(j => j == i)
                            where !found
                            select i).ToList();
                    AppSettings.Current.LandingPageMenuList = AppSettings.Current
                        .LandingPageMenuList.OrderBy(x => x.MobileAppMenuSequence).ToList();

                    if (AppSettings.Current.AssignmentNotificationCount > 0)
                    {
                        var notificationCenterModuleCode = AppSettings.Current.MenuStructureList
                            .Where(x => x.ModuleCode != null &&
                                        x.ModuleCode.ToLower().Equals("notificationcenter"))
                            .FirstOrDefault();
                        if (notificationCenterModuleCode != null)
                            notificationCenterModuleCode.ModuleName =
                                notificationCenterModuleCode.ModuleName + string.Concat(" ",
                                    "(", AppSettings.Current.AssignmentNotificationCount, ")");
                    }
                }

                AppSettings.Current.LogoutItem.ModuleName = "Logout";
                AppSettings.Current.LogoutItem.ModuleImageUrl = "logout_icon.png";
            }
            catch (Exception ex)
            {
                var tokenStatus = string.IsNullOrEmpty(App.RefreshedToken)
                    ? "Token_Empty"
                    : "Token_Available";
                HelperMethods.LogEvent("Exception2",
                    $"Exception2 - {ex.Message} - Token - {tokenStatus}");
            }


            AppSettings.Current.StudentList = _mapper.Map<List<BindableStudentPickListItem>>(
                Result.UserSessionData.PortalUserType == PortalUserTypes.Parent
                    ? Result.UserSessionData.FamilyStudentList.ToList()
                    : new List<StudentPickListItem>());

            foreach (var item in AppSettings.Current.StudentList)
                item.AvatarImagePath = item.AvatarImagePath?.Replace("https", "http");

            #region Registered Unregistered student list

            AppSettings.Current.RegisteredStudentList =
                _mapper.Map<List<BindableStudentPickListItem>>(
                    Result.UserSessionData.PortalUserType == PortalUserTypes.Parent
                        ? Result.UserSessionData.FamilyStudentList.ToList()
                        : new List<StudentPickListItem>());
            foreach (var item in AppSettings.Current.RegisteredStudentList)
                item.AvatarImagePath = item.AvatarImagePath?.Replace("https", "http");
            if (Result.UserSessionData.FamilyStudentList != null &&
                Result.UserSessionData.FamilyStudentList.ToList().Count > 0)
            {
                var registeredStudentList = Result.UserSessionData.FamilyAllStudentList
                    .Where(x => x.IsRegistered == true).ToList();
                if (registeredStudentList.ToList() != null &&
                    registeredStudentList.ToList().Count > 0)
                    AppSettings.Current.AllStudentList =
                        _mapper.Map<List<BindableStudentPickListItem>>(
                            Result.UserSessionData.PortalUserType == PortalUserTypes.Parent
                                ? Result.UserSessionData.FamilyStudentList.ToList()
                                : new List<StudentPickListItem>());
                else
                    AppSettings.Current.AllStudentList =
                        _mapper.Map<List<BindableStudentPickListItem>>(
                            Result.UserSessionData.PortalUserType == PortalUserTypes.Parent
                                ? Result.UserSessionData.FamilyAllStudentList.ToList()
                                : new List<StudentPickListItem>());
            }
            else if (Result.UserSessionData.FamilyAllStudentList != null &&
                     Result.UserSessionData.FamilyAllStudentList.ToList().Count > 0)
            {
                AppSettings.Current.AllStudentList =
                    _mapper.Map<List<BindableStudentPickListItem>>(
                        Result.UserSessionData.PortalUserType == PortalUserTypes.Parent
                            ? Result.UserSessionData.FamilyAllStudentList.ToList()
                            : new List<StudentPickListItem>());
                AppSettings.Current.StudentList = AppSettings.Current.AllStudentList;
            }

            foreach (var item in AppSettings.Current.AllStudentList)
                item.AvatarImagePath = item.AvatarImagePath?.Replace("https", "http");

            #endregion

            AppSettings.Current.IsStudentFooterVisible =
                Result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? true : false;
            AppSettings.Current.IsParent =
                Result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? true : false;
            AppSettings.Current.IsTeacher =
                Result.UserSessionData.PortalUserType == PortalUserTypes.Teacher ? true : false;
            AppSettings.Current.IsStudent =
                Result.UserSessionData.PortalUserType == PortalUserTypes.Student ? true : false;
            AppSettings.Current.PortalUserType = Result.UserSessionData.PortalUserType;
            if (AppSettings.Current.PortalUserType == PortalUserTypes.Student)
            {
                AppSettings.Current.SelectedStudent.ItemId =
                    Result.UserSessionData.UserRefId.ToString();
                AppSettings.Current.SelectedStudent.AvatarImagePath =
                    Result.UserSessionData.StudentData.AvatarImagePath
                        ?.Replace("https", "http");
                AppSettings.Current.SelectedStudentFromAllStudentList.ItemId =
                    Result.UserSessionData.UserRefId.ToString();
            }
            AppSettings.Current.UserCommunicationNotificationCount =
                Result.CommunicationNotifications.Count(n => !n.IsRead);
            AppSettings.Current.UserAnnouncementsCount =
                Result.UserNotifications
                    .Where(n => n.NotificationGroup.Equals("Announcements", StringComparison.OrdinalIgnoreCase))
                    .Count();
            AppSettings.Current.UserNotificationCount =
                Result.UserNotifications
                    .Where(n => n.NotificationGroup.Equals("Notifications", StringComparison.OrdinalIgnoreCase))
                    .Count();
            AppSettings.Current.FileUploadSettings = Result.UserSessionData.FileUploadSettings;
            AppSettings.Current.Email = Email.Value;
            AppSettings.Current.Settings.BgColor = "#ffffff";
            AppSettings.Current.CacheRecordSize =
                Result.CacheRecordSize < TextResource.MinimumCacheRecordSize.ToInteger()
                    ? TextResource.MinimumCacheRecordSize.ToInteger()
                    : Result.CacheRecordSize;
            AppSettings.Current.UserRefId = Result.UserSessionData.UserRefId;
            if (AppSettings.Current.IsParent)
                AppSettings.Current.DisplayId =
                    !string.IsNullOrEmpty(Result.UserSessionData.AlternateId)
                        ? Result.UserSessionData.AlternateId
                        : string.Empty;
            else if (AppSettings.Current.IsTeacher)
                AppSettings.Current.DisplayId =
                    !string.IsNullOrEmpty(Result.UserSessionData.AlternateId)
                        ? Result.UserSessionData.AlternateId
                        : string.Empty;
            else if (AppSettings.Current.IsStudent)
                AppSettings.Current.DisplayId =
                    !string.IsNullOrEmpty(Result.UserSessionData.AlternateId)
                        ? Result.UserSessionData.AlternateId
                        : string.Empty;

            ICCacheManager.SaveObject<bool>(TextResource.PushNotificationKey,
                Result.IsPushNotificationEnabled);
            ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);


            MainThread.BeginInvokeOnMainThread(() => AppSettings.Current.IsBusy = false);
            await ApiHelper.HideProcessingIndicatorPopup();
            App.CustomAlertIdList = new List<int>();
            App.SurveyIdList = new List<int>();
            
            
            var homeForm = new HomeForm(_mapper, Navigation, _nativeServices)
            {
                SelectedModule = AppSettings.Current.MenuStructureList?
                    .FirstOrDefault(x => x.ModuleName.ToLower().Contains("home")),
                IsPageLoaded = true
            };

            Preferences.Set("IsLogin", true);

            var homePage = new HomePage
            {
                BindingContext = homeForm
            };

            await Navigation.PushAsync(homePage);

            if (AppSettings.Current.PortalUserType == PortalUserTypes.Parent)
            {
                var list = AppSettings.Current.StudentList.Where(x =>
                    !string.IsNullOrEmpty(x.BirthdayNotficationMessage)).ToList();
                var item = list.FirstOrDefault();
                if (item != null)
                    await CheckBirthdayNotification(item.BirthdayNotficationMessage,
                        item.StudentName, item.ItemId);
            }
            else if (AppSettings.Current.PortalUserType == PortalUserTypes.Student)
            {
                await CheckBirthdayNotification(
                    Result.UserSessionData.StudentData.BirthdayNotficationMessage,
                    Result.FirstName, null);
            }
            else if (AppSettings.Current.PortalUserType == PortalUserTypes.Teacher)
            {
                //CheckBirthdayNotification(result.UserSessionData.BirthdayNotficationMessage, result.UserSessionData.DisplayName);
            }
        }
        private async Task CheckBirthdayNotification(string message, string name, string itemId)
        {
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    BirthdayPopupForm birthdayPopupForm = new ()
                    {
                        Message = message,
                        StudentName = name,
                        ItemId = itemId,
                        Title = TextResource.BirthdayPopupTitle
                    };
                    BirthdayPopup birthdayPopup = new ()
                    {
                        BindingContext = birthdayPopupForm
                    };
                    SetPopupInstance(birthdayPopup);
                    Application.Current.MainPage.ShowPopup(birthdayPopup);                }
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }
        public void SetPopupInstance(Popup popup)
        {
            AppSettings.Current.CurrentPopup = popup;
        }
        private bool IsLoginFormValid()
        {
            isEmailValid = Email.Validate();
            isPasswordValid = Password.Validate();
            if (isEmailValid && isPasswordValid)
                return true;
            else
            {
                if (!isEmailValid)
                {
                    IsEmailErrorLableVisible = true;
                    EmailErrorEntryColor = Colors.Red;
                    return false;
                }
                else if (!isPasswordValid)
                {
                    PasswordErrorEntryColorNew = true;
                    PasswordErrorEntryColor = Colors.Red;
                    return false;
                }
                return false;
            }
        }

        private async void TroubleSignInClicked()
        {
            if (clientCode.Equals("CS"))
            {
                troubleSignInEmail = TextResource.TroubleSignInEmailCS;
            }
            else if (clientCode.Equals("CAS"))
            {
                troubleSignInEmail = TextResource.TroubleSignInEmailCAS;
            }
            else if (clientGroupCode.Equals("Beam"))
            {
                await HelperMethods.ShowAlert(TextResource.TroubleSignInTitle, TextResource.BeamTroubleSignInInformation);
                return;
            }
            else
            {
                troubleSignInEmail = TextResource.TroubleSignInGeneralEmail;
            }
            if (Device.RuntimePlatform == Device.Android)
                await HelperMethods.ShowAlert(TextResource.TroubleSignInTitle, string.Format(string.Concat(TextResource.TroubleSignInInformation,troubleSignInEmail)));
            else
                await HelperMethods.ShowAlert(TextResource.TroubleSignInTitle, string.Format(string.Concat(TextResource.TroubleSignInInformation, troubleSignInEmail)));
        }
        private void SetLoginScreenBackground()
        {
            clientCode = (!string.IsNullOrEmpty(App.ClientCode)) ? App.ClientCode : string.Empty;
            clientGroupCode= (!string.IsNullOrEmpty(App.ClientGroupCode)) ? App.ClientGroupCode : string.Empty;
            if (clientCode.Equals("CS"))
            {
                CustomLoginScreenBackground = "cs_loginscreen_background.png";
                Logo = "cs_logo.png";
            }
            else if (clientCode.Equals("CAS"))
            {
                CustomLoginScreenBackground = "cas_loginscreen_background.png";
                Logo = "cas_logo.png";
            }
            else if (clientCode.Equals("GIPA"))
            {
                Logo = "gipa_logo.png";
            }
            else if (clientCode.Equals("Madar"))
            {
                Logo = "madar_logo.png";
            }
            else if (clientGroupCode.Equals("Beam"))
            {
                //CustomLoginScreenBackground = "BeamLoginBackground.png";
                //PlaceholderColor = Color.LightGray;


                CustomLoginScreenBackground = "BeamBackgroundImage.png";
                //CustomLoginScreenBackground = "beambackground.png";
                //CustomLoginScreenBackground = "beamss.png";

            }
        }
        private async Task AutoLogin()
        {
            try
            {
                string email = await ICCacheManager.GetSecureObject<string>("icampus_email");
                string password = await ICCacheManager.GetSecureObject<string>("icampus_pwd");

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    //Analytics.TrackEvent("AutoLogin ExecuteSignInButton - " + email + " - " + DeviceInfo.Name + " - " + DeviceInfo.Model + " - " + DeviceInfo.Platform);
                    Email.Value = email;
                    Password.Value = password;
                    await ICCacheManager.ClearCache();
                    NOOTP = "NOOTP";
                    ExecuteSignInButton();
                }
            }
            catch (Exception ex)
            {
                HelperMethods.TrackCrashlytics(ex);
            }

        }
        #endregion
}