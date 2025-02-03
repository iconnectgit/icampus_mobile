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
#if IOS
    using Foundation;
    using Firebase.Crashlytics;
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
                            var result = await HelperMethods.PerformLogin(Email.Value, Password.Value, deviceId);
                            if (result.IsLoginSuccessful)
                            {
                                await ICCacheManager.SaveSecureObject<string>("icampus_pwd", Password.Value);
                                await ICCacheManager.SaveSecureObject<string>("icampus_email", Email.Value);
                                await ApiHelper.ShowProcessingIndicatorPopup();
                                IsLoginFailedLabelVisible = false;
                                AppSettings.Current = new AppSettings();
                                AppSettings.Current.UserId = result.UserSessionData.UserId;
                                AppSettings.Current.SchoolCampusId = result.SchoolCampusId;
                                AppSettings.Current.UserLoginName = result.UserSessionData.UserLoginName;
                                AppSettings.Current.UserPassword = result.UserSessionData.UserPassword;
                                AppSettings.Current.DetailedDisplayName = result.UserSessionData.DetailedDisplayName_1;
                                AppSettings.Current.UserFullName = string.Concat(result.UserSessionData.FirstName_1+" "+result.UserSessionData.LastName_1);
                                AppSettings.Current.ApiUrl = result.PortalApiUrl;
                                AppSettings.Current.UserSessionUid = result.UserSessionUid;
                                AppSettings.Current.PortalUrl = result.PortalUrl;
                                AppSettings.Current.BeamHeaderNotificationCount = result.UserNotificationCount;
                                AppSettings.Current.AssignmentNotificationCount = result.UserAssignmentNotificationCount;
                                AppSettings.Current.BeamHeaderImage = result.HeaderBackGroundImagePath?.Replace("https","http");
                                AppSettings.Current.TeacherDesignation = result.UserSessionData.TeacherAccessLevels != null ? result.UserSessionData.TeacherAccessLevels.Designation:string.Empty;
                                AppSettings.Current.StudentDesignation = result.UserSessionData.StudentData != null ? result.UserSessionData.StudentData.ClassName : string.Empty;

                                AppSettings.Current.AcademicYearTitle = result.AcademicYearTitle;
                                AppSettings.Current.FamilyData = result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? result.UserSessionData.FamilyData : new FamilySessionData();
                                
                                AppSettings.Current.IsMandatoryUpdate = result.IsMandatoryUpdate;
                                AppSettings.Current.VersionNumber = result.VersionNumber;
                                AppSettings.Current.FatherEmail = !string.IsNullOrEmpty(result.UserSessionData.Email1)? result.UserSessionData.Email1:string.Empty;
                                AppSettings.Current.MotherEmail = !string.IsNullOrEmpty(result.UserSessionData.Email2)?result.UserSessionData.Email2:string.Empty;
                                if(!string.IsNullOrEmpty(AppSettings.Current.FatherEmail)&&!string.IsNullOrEmpty(AppSettings.Current.MotherEmail)&&AppSettings.Current.FatherEmail.ToLower().Equals(AppSettings.Current.MotherEmail.ToLower()))
                                {
                                    AppSettings.Current.MotherEmail = string.Empty;
                                }

                                AppSettings.Current.WeekStartDay = result.WeekStartDay;
                                AppSettings.Current.Settings = result.MobileAppSettings;
                                AppSettings.Current.LogoUrl = result.LogoUrl;
                                AppSettings.Current.DisplayName = result.DisplayName;
                                AppSettings.Current.FirstName = result.FirstName;
                                AppSettings.Current.SchoolNextWorkingDate = result.SchoolNextWorkingDate.ToLocalTime();
                                AppSettings.Current.MenuStructureList = _mapper.Map<List<BindableModuleStructureView>>(result.MenuList.ToList());
                                
                                foreach (var item in AppSettings.Current.MenuStructureList)
                                {
                                    item.ModuleImageUrl = item.ModuleImageUrl?.Replace("https", "http");
                                }
                                var index = AppSettings.Current.MenuStructureList.ToList().FindIndex(x => x.ModuleCode != null && x.ModuleCode.ToLower().Equals("notificationcenter"));
                                if(index>=0)
                                {
                                    var element = AppSettings.Current.MenuStructureList[index];
                                    AppSettings.Current.MenuStructureList.RemoveAt(index);
                                    AppSettings.Current.MenuStructureList.Insert(0, element);
                                }

                                try
                                {
                                    foreach (var item in AppSettings.Current.MenuStructureList)
                                    {
                                        if (item != null)
                                        {
                                            if (item.ModuleCode != null)
                                            {
                                                if (item.ModuleCode.ToLower().Equals("settings"))
                                                {
                                                    AppSettings.Current.SettingsMenuImageUrl = item.ModuleImageUrl;
                                                }
                                                if (item.ModuleCode.ToLower().Equals("home"))
                                                {
                                                    AppSettings.Current.HomeMenuImageUrl = item.ModuleImageUrl?.Replace("https" , "http");
                                                }
                                            }
                                        }
                                    }
                                    if (AppSettings.Current.MenuStructureList != null)
                                    {
                                        AppSettings.Current.CircularIconVisibility = AppSettings.Current.MenuStructureList.Where(x => x.ModuleCode != null && x.ModuleCode.ToLower().Trim().Equals("messagefromschool")).FirstOrDefault() != null ? true : false;
                                        AppSettings.Current.BeamCommunicationAndBellIconVisibility = AppSettings.Current.MenuStructureList.Where(x => x.ModuleCode != null && x.ModuleCode.ToLower().Equals("communication")).FirstOrDefault() != null ? true : false;
                                        AppSettings.Current.FooterMenuList = new ObservableCollection<BindableModuleStructureView>(AppSettings.Current.MenuStructureList?.Where(x => x.IsFooterMenuOnMobile && x.ModuleCode != null && !(x.ModuleCode.ToLower().Equals("home") || x.ModuleCode.ToLower().Equals("contactus") || x.ModuleCode.ToLower().Equals("messagefromschool") || x.ModuleCode.ToLower().Equals("settings"))).Take(3).ToList());
                                        var homeData = new ObservableCollection<BindableModuleStructureView>(AppSettings.Current.MenuStructureList?.Where(x => x.ModuleCode == "Home"));
                                        AppSettings.Current.FooterMenuList.Insert(0, homeData.FirstOrDefault());
                                        AppSettings.Current.FooterMenuList.Add(new BindableModuleStructureView() {ModuleCode="News", ModuleShortName="News",ModuleName="News",ShowIcon=true });
                                        AppSettings.Current.FooterMenuList.Where(x => x.ModuleCode.Contains("Home")).FirstOrDefault().IsSelected = true;
                                        AppSettings.Current.SelectedFooterMenu = AppSettings.Current.FooterMenuList.Where(x => x.ModuleCode.Contains("Home")).FirstOrDefault();
                                        AppSettings.Current.LandingPageMenuList = (from i in AppSettings.Current.MenuStructureList let found = AppSettings.Current.FooterMenuList.Any(j => j == i) where !found select i).ToList();
                                        var removeList=AppSettings.Current.LandingPageMenuList.Where(x=>x.ModuleCode!=null&&(x.ModuleCode.ToLower().Equals("home")|| x.ModuleCode.ToLower().Equals("communication") || x.ModuleCode.ToLower().Equals("messagefromschool"))).ToList();
                                        AppSettings.Current.LandingPageMenuList= (from i in AppSettings.Current.LandingPageMenuList let found = removeList.Any(j => j == i) where !found select i).ToList();
                                        AppSettings.Current.LandingPageMenuList = AppSettings.Current.LandingPageMenuList.OrderBy(x=>x.MobileAppMenuSequence).ToList();

                                        if (AppSettings.Current.AssignmentNotificationCount>0)
                                        {
                                            var notificationCenterModuleCode = AppSettings.Current.MenuStructureList.Where(x => x.ModuleCode != null && x.ModuleCode.ToLower().Equals("notificationcenter")).FirstOrDefault();
                                            if(notificationCenterModuleCode!=null)
                                            {
                                                notificationCenterModuleCode.ModuleName = notificationCenterModuleCode.ModuleName + string.Concat(" ", "(", AppSettings.Current.AssignmentNotificationCount, ")");
                                            }
                                        }
                                    }
                                    
                                    AppSettings.Current.LogoutItem.ModuleName = "Logout";
                                    AppSettings.Current.LogoutItem.ModuleImageUrl = "logout_icon.png";
                                }
                                catch (Exception ex)
                                {
                                    var tokenStatus = string.IsNullOrEmpty(App.RefreshedToken) ? "Token_Empty" : "Token_Available";
                                    HelperMethods.LogEvent("Exception2", 
                                        $"Exception2 - {ex.Message} - Token - {tokenStatus}");
                                }

                                


                                AppSettings.Current.StudentList = _mapper.Map<List<BindableStudentPickListItem>>(result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? result.UserSessionData.FamilyStudentList.ToList() : new List<StudentPickListItem>());
                                AppSettings.Current.StudentList = _mapper.Map<List<BindableStudentPickListItem>>(result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? result.UserSessionData.FamilyStudentList.ToList() : new List<StudentPickListItem>());

                                foreach (var item in AppSettings.Current.StudentList)
                                {
                                    item.AvatarImagePath = item.AvatarImagePath?.Replace("https", "http");
                                }

                                #region Registered Unregistered student list
                                AppSettings.Current.RegisteredStudentList = _mapper.Map<List<BindableStudentPickListItem>>(result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? result.UserSessionData.FamilyStudentList.ToList() : new List<StudentPickListItem>());
                                foreach (var item in AppSettings.Current.RegisteredStudentList)
                                {
                                    item.AvatarImagePath = item.AvatarImagePath?.Replace("https", "http");
                                }
                                if (result.UserSessionData.FamilyStudentList != null && result.UserSessionData.FamilyStudentList.ToList().Count > 0)
                                {
                                    var registeredStudentList = result.UserSessionData.FamilyAllStudentList.Where(x=>x.IsRegistered==true).ToList();
                                    if(registeredStudentList.ToList()!=null&&registeredStudentList.ToList().Count >0)
                                    {
                                        AppSettings.Current.AllStudentList = _mapper.Map<List<BindableStudentPickListItem>>(result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? result.UserSessionData.FamilyStudentList.ToList() : new List<StudentPickListItem>());
                                    }
                                    else
                                    {
                                        AppSettings.Current.AllStudentList = _mapper.Map<List<BindableStudentPickListItem>>(result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? result.UserSessionData.FamilyAllStudentList.ToList() : new List<StudentPickListItem>());
                                    }
                                }
                                else if (result.UserSessionData.FamilyAllStudentList != null && result.UserSessionData.FamilyAllStudentList.ToList().Count > 0)
                                {
                                    AppSettings.Current.AllStudentList = _mapper.Map<List<BindableStudentPickListItem>>(result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? result.UserSessionData.FamilyAllStudentList.ToList() : new List<StudentPickListItem>());
                                }
                                foreach (var item in AppSettings.Current.AllStudentList)
                                {
                                    item.AvatarImagePath = item.AvatarImagePath?.Replace("https", "http");
                                }

                                #endregion

                                AppSettings.Current.IsStudentFooterVisible = result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? true : false;
                                AppSettings.Current.IsParent = result.UserSessionData.PortalUserType == PortalUserTypes.Parent ? true : false;
                                AppSettings.Current.IsTeacher = result.UserSessionData.PortalUserType == PortalUserTypes.Teacher ? true : false;
                                AppSettings.Current.IsStudent = result.UserSessionData.PortalUserType == PortalUserTypes.Student ? true : false;
                                AppSettings.Current.PortalUserType = result.UserSessionData.PortalUserType;
                                if (AppSettings.Current.PortalUserType == PortalUserTypes.Student)
                                {
                                    AppSettings.Current.SelectedStudent.ItemId = result.UserSessionData.UserRefId.ToString();
                                    AppSettings.Current.SelectedStudent.AvatarImagePath = result.UserSessionData.StudentData.AvatarImagePath?.Replace("https", "http");
                                    AppSettings.Current.SelectedStudentFromAllStudentList.ItemId = result.UserSessionData.UserRefId.ToString();
                                }

                                AppSettings.Current.FileUploadSettings = result.UserSessionData.FileUploadSettings;
                                AppSettings.Current.Email = Email.Value;
                                AppSettings.Current.Settings.BgColor = "#ffffff";
                                AppSettings.Current.CacheRecordSize = result.CacheRecordSize < TextResource.MinimumCacheRecordSize.ToInteger() ? TextResource.MinimumCacheRecordSize.ToInteger() : result.CacheRecordSize;
                                AppSettings.Current.UserRefId = result.UserSessionData.UserRefId;
                                if(AppSettings.Current.IsParent)
                                {
                                    AppSettings.Current.DisplayId = !string.IsNullOrEmpty(result.UserSessionData.AlternateId) ? result.UserSessionData.AlternateId : string.Empty;
                                }
                                else if(AppSettings.Current.IsTeacher)
                                {
                                    AppSettings.Current.DisplayId = !string.IsNullOrEmpty(result.UserSessionData.AlternateId) ? result.UserSessionData.AlternateId : string.Empty;
                                }
                                else if(AppSettings.Current.IsStudent)
                                {
                                    AppSettings.Current.DisplayId = !string.IsNullOrEmpty(result.UserSessionData.AlternateId) ? result.UserSessionData.AlternateId : string.Empty;
                                }

                                ICCacheManager.SaveObject<bool>(TextResource.PushNotificationKey, result.IsPushNotificationEnabled);
                                ICCacheManager.SaveObject<AppSettings>("AppSettings", AppSettings.Current);

                                

                                MainThread.BeginInvokeOnMainThread(() => AppSettings.Current.IsBusy = false);
                                await ApiHelper.HideProcessingIndicatorPopup();
                                App.CustomAlertIdList = new List<int>();
                                App.SurveyIdList = new List<int>();
                                HomeForm homeForm = new HomeForm(_mapper, Navigation, _nativeServices)
                                {
                                    SelectedModule = AppSettings.Current.MenuStructureList?.Where(x => x.ModuleName.ToLower().Contains("home"))?.FirstOrDefault(),
                                    IsPageLoaded = true
                                };
                                Preferences.Set("IsLogin", true);
                                 
                                var homePage = new HomePage()
                                {
                                    BindingContext = homeForm
                                };
                                await Navigation.PushAsync(homePage);
                                if (AppSettings.Current.PortalUserType == PortalUserTypes.Parent)
                                {
                                    var list = AppSettings.Current.StudentList.Where(x => !string.IsNullOrEmpty(x.BirthdayNotficationMessage)).ToList();
                                    var item = list.FirstOrDefault();
                                    if(item != null)
                                    {
                                        await CheckBirthdayNotification(item.BirthdayNotficationMessage, item.StudentName, item.ItemId);
                                    }
                                }
                                else if (AppSettings.Current.PortalUserType == PortalUserTypes.Student)
                                {
                                    await CheckBirthdayNotification(result.UserSessionData.StudentData.BirthdayNotficationMessage, result.FirstName, null);
                                }
                                else if (AppSettings.Current.PortalUserType == PortalUserTypes.Teacher)
                                {
                                    //CheckBirthdayNotification(result.UserSessionData.BirthdayNotficationMessage, result.UserSessionData.DisplayName);
                                }

                            }
                            else
                            {
                                LblValidationMessage = result.LoginMessage;
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