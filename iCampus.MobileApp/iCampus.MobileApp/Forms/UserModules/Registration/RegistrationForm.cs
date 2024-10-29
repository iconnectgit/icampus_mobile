using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Forms.UserModules.OnlinePayment;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using iCampus.MobileApp.Views.UserModules.Registration;
using iCampus.Portal.ViewModels;
using iCampus.Portal.ViewModels.Enums;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class RegistrationForm : ViewModelBase
	{
        #region Declaration
        public ICommand UpdateDetailsCommand { get; set; }
        public ICommand BookAppointmentCommand { get; set; }
        public ICommand ReregistrationCommand { get; set; }
        public ICommand CancelRequestCommand { get; set; }
        public ICommand CurrentYearRegistrationCommand { get; set; }
        public ICommand NextYearRegistrationCommand { get; set; }
        public ICommand RegistrationInstructionsCommand { get; set; }
        public ICommand ExpandCollapseClickCommand { get; set; }
        public ICommand NewExpandCollapseClickCommand { get; set; }
        public ICommand StudentListExpandCollapseCommand { get; set; }
        public ICommand ModifyBookingCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CurrentYearCheckboxClickCommand { get; set; }
        public ICommand NextYearCheckboxClickCommand { get; set; }
        public ICommand NextYearReregCheckboxClickCommand { get; set; }
        public ICommand CurrentYearReregCheckboxClickCommand { get; set; }
        public ICommand UpdatePhotoClickCommand { get; set; }
        public ICommand PayNowCommandCurrentYear { get; set; }
        public ICommand PayNowCommandNextYear { get; set; }
        public ICommand ViewReceiptClickCommand { get; set; }
        public ICommand NewStudentsCollapseClickCommand { get; set; }
        public ICommand DeleteNewStudentCommand { get; set; }
        public ICommand EditNewStudentsDetalsCommand { get; set; }
        public ICommand BookAppointmentPerStudentCommand { get; set; }
        public ICommand CancelAppointmentPerStudentCommand { get; set; }
        public ICommand KGInformationCommand { get; set; }
        #endregion
        #region Properties
        RegistrationAppViewModel _registrationData = new RegistrationAppViewModel();
        public RegistrationAppViewModel RegistrationData
        {
            get => _registrationData;
            set
            {
                _registrationData = value;
                OnPropertyChanged(nameof(RegistrationData));
            }
        }
        private bool _isErrorMessageVisible;
        public bool IsErrorMessageVisible
        {
            get => _isErrorMessageVisible;
            set
            {
                _isErrorMessageVisible = value;
                OnPropertyChanged(nameof(IsErrorMessageVisible));
            }
        }
        private bool _isRegistrationDataVisible;
        public bool IsRegistrationDataVisible
        {
            get => _isRegistrationDataVisible;
            set
            {
                _isRegistrationDataVisible = value;
                OnPropertyChanged(nameof(IsRegistrationDataVisible));
            }
        }
        private bool _isAppointmentButtonVisible;
        public bool IsAppointmentButtonVisible
        {
            get => _isAppointmentButtonVisible;
            set
            {
                _isAppointmentButtonVisible = value;
                OnPropertyChanged(nameof(IsAppointmentButtonVisible));
            }
        }
        private bool _isInstructionVisible;
        public bool IsInstructionVisible
        {
            get => _isInstructionVisible;
            set
            {
                _isInstructionVisible = value;
                OnPropertyChanged(nameof(IsInstructionVisible));
            }
        }
        private string _registrationInstructions;
        public string RegistrationInstructions
        {
            get => _registrationInstructions;
            set
            {
                _registrationInstructions = value;
                OnPropertyChanged(nameof(RegistrationInstructions));
            }
        }
        private bool _isReRegistrationVisible;
        public bool IsReRegistrationVisible
        {
            get => _isReRegistrationVisible;
            set
            {
                _isReRegistrationVisible = value;
                OnPropertyChanged(nameof(IsReRegistrationVisible));
            }
        }
        IEnumerable<RegistrationAppointmentBookingView> _familyAppointmentBookings;
        public IEnumerable<RegistrationAppointmentBookingView> FamilyAppointmentBookings
        {
            get => _familyAppointmentBookings;
            set
            {
                _familyAppointmentBookings = value;
                OnPropertyChanged(nameof(FamilyAppointmentBookings));
            }
        }
        RegistrationAppointmentBookingView _selectedFamilyAppointment;
        public RegistrationAppointmentBookingView SelectedFamilyAppointment
        {
            get => _selectedFamilyAppointment;
            set
            {
                _selectedFamilyAppointment = value;
                OnPropertyChanged(nameof(SelectedFamilyAppointment));
            }
        }
        ObservableCollection<BindableRegistrationNewStudentView> _registrationNewStudentList = new ObservableCollection<BindableRegistrationNewStudentView>();
        public ObservableCollection<BindableRegistrationNewStudentView> RegistrationNewStudentList
        {
            get => _registrationNewStudentList;
            set
            {
                _registrationNewStudentList = value;
                OnPropertyChanged(nameof(RegistrationNewStudentList));
            }
        }
        ObservableCollection<BindableRegistrationExistingStudentView> _currentYearExistingStudents = new ObservableCollection<BindableRegistrationExistingStudentView>();
        public ObservableCollection<BindableRegistrationExistingStudentView> CurrentYearExistingStudents
        {
            get => _currentYearExistingStudents;
            set
            {
                _currentYearExistingStudents = value;
                OnPropertyChanged(nameof(CurrentYearExistingStudents));
            }
        }
        BindableRegistrationExistingStudentView _selectedExistingStudent;
        public BindableRegistrationExistingStudentView SelectedExistingStudent
        {
            get => _selectedExistingStudent;
            set
            {
                _selectedExistingStudent = value;
                OnPropertyChanged(nameof(SelectedExistingStudent));
            }
        }
        ObservableCollection<BindableRegistrationExistingStudentView> _nextYearExistingStudents = new ObservableCollection<BindableRegistrationExistingStudentView>();
        public ObservableCollection<BindableRegistrationExistingStudentView> NextYearExistingStudents
        {
            get => _nextYearExistingStudents;
            set
            {
                _nextYearExistingStudents = value;
                OnPropertyChanged(nameof(NextYearExistingStudents));
            }
        }
        ObservableCollection<BindableRegistrationExistingStudentView> _studentNamesList = new ObservableCollection<BindableRegistrationExistingStudentView>();
        public ObservableCollection<BindableRegistrationExistingStudentView> StudentNamesList
        {
            get => _studentNamesList;
            set
            {
                _studentNamesList = value;
                OnPropertyChanged(nameof(StudentNamesList));
            }
        }
        private bool _isBookingsVisible;
        public bool IsBookingsVisible
        {
            get => _isBookingsVisible;
            set
            {
                _isBookingsVisible = value;
                OnPropertyChanged(nameof(IsBookingsVisible));
            }
        }
        private bool _showCurrentYearRegistrationPanel;
        public bool ShowCurrentYearRegistrationPanel
        {
            get => _showCurrentYearRegistrationPanel;
            set
            {
                _showCurrentYearRegistrationPanel = value;
                OnPropertyChanged(nameof(ShowCurrentYearRegistrationPanel));
            }
        }
        private bool _showStudentNameListOnly;
        public bool ShowStudentNameListOnly
        {
            get => _showStudentNameListOnly;
            set
            {
                _showStudentNameListOnly = value;
                OnPropertyChanged(nameof(ShowStudentNameListOnly));
            }
        }
        private string _reregistrationCompletedLabel;
        public string ReregistrationCompletedLabel
        {
            get => _reregistrationCompletedLabel;
            set
            {
                _reregistrationCompletedLabel = value;
                OnPropertyChanged(nameof(ReregistrationCompletedLabel));
            }
        }
        private string _currentYearExistingStudentGridTitle;
        public string CurrentYearExistingStudentGridTitle
        {
            get => _currentYearExistingStudentGridTitle;
            set
            {
                _currentYearExistingStudentGridTitle = value;
                OnPropertyChanged(nameof(CurrentYearExistingStudentGridTitle));
            }
        }
        private string _nextYearExistingStudentGridTitle;
        public string NextYearExistingStudentGridTitle
        {
            get => _nextYearExistingStudentGridTitle;
            set
            {
                _nextYearExistingStudentGridTitle = value;
                OnPropertyChanged(nameof(NextYearExistingStudentGridTitle));
            }
        }
        private string _kgInformationMissingMessage;
        public string KgInformationMissingMessage
        {
            get => _kgInformationMissingMessage;
            set
            {
                _kgInformationMissingMessage = value;
                OnPropertyChanged(nameof(KgInformationMissingMessage));
            }
        }
        private bool _showRegistrationFees;
        public bool ShowRegistrationFees
        {
            get => _showRegistrationFees;
            set
            {
                _showRegistrationFees = value;
                OnPropertyChanged(nameof(ShowRegistrationFees));
            }
        }
        private bool _showTransportationDeposit;
        public bool ShowTransportationDeposit
        {
            get => _showTransportationDeposit;
            set
            {
                _showTransportationDeposit = value;
                OnPropertyChanged(nameof(ShowTransportationDeposit));
            }
        }
        private bool _isModifyVisible;
        public bool IsModifyVisible
        {
            get => _isModifyVisible;
            set
            {
                _isModifyVisible = value;
                OnPropertyChanged(nameof(IsModifyVisible));
            }
        }
        private bool _currentYearExistingStudentsVisible;
        public bool CurrentYearExistingStudentsVisible
        {
            get => _currentYearExistingStudentsVisible;
            set
            {
                _currentYearExistingStudentsVisible = value;
                OnPropertyChanged(nameof(CurrentYearExistingStudentsVisible));
            }
        }
        private bool _nextYearExistingStudentsVisible;
        public bool NextYearExistingStudentsVisible
        {
            get => _nextYearExistingStudentsVisible;
            set
            {
                _nextYearExistingStudentsVisible = value;
                OnPropertyChanged(nameof(NextYearExistingStudentsVisible));
            }
        }
        private bool _studentNamesListVisible;
        public bool StudentNamesListVisible
        {
            get => _studentNamesListVisible;
            set
            {
                _studentNamesListVisible = value;
                OnPropertyChanged(nameof(StudentNamesListVisible));
            }
        }
        private int _currentYearListViewHeight;
        public int CurrentYearListViewHeight
        {
            get => _currentYearListViewHeight;
            set
            {
                _currentYearListViewHeight = value;
                OnPropertyChanged(nameof(CurrentYearListViewHeight));
            }
        }
        private int _nextYearListViewHeight;
        public int NextYearListViewHeight
        {
            get => _nextYearListViewHeight;
            set
            {
                _nextYearListViewHeight = value;
                OnPropertyChanged(nameof(NextYearListViewHeight));
            }
        }
        private int _studentsListViewHeight;
        public int StudentsListViewHeight
        {
            get => _studentsListViewHeight;
            set
            {
                _studentsListViewHeight = value;
                OnPropertyChanged(nameof(StudentsListViewHeight));
            }
        }
        private int _appointmnetListViewHeight;
        public int AppointmentListViewHeight
        {
            get => _appointmnetListViewHeight;
            set
            {
                _appointmnetListViewHeight = value;
                OnPropertyChanged(nameof(AppointmentListViewHeight));
            }
        }
        private int _newStudentsListViewHeight;
        public int NewStudentsListViewHeight
        {
            get => _newStudentsListViewHeight;
            set
            {
                _newStudentsListViewHeight = value;
                OnPropertyChanged(nameof(NewStudentsListViewHeight));
            }
        }
        private string _confirmationMessage;
        public string ConfirmationMessage
        {
            get => _confirmationMessage;
            set
            {
                _confirmationMessage = value;
                OnPropertyChanged(nameof(ConfirmationMessage));
            }
        }
        private string _termsAndConditions;
        public string TermsAndConditions
        {
            get => _termsAndConditions;
            set
            {
                _termsAndConditions = value;
                OnPropertyChanged(nameof(TermsAndConditions));
            }
        }
        private bool _requestReRegistrationButtonVisible;
        public bool RequestReRegistrationButtonVisible
        {
            get => _requestReRegistrationButtonVisible;
            set
            {
                _requestReRegistrationButtonVisible = value;
                OnPropertyChanged(nameof(RequestReRegistrationButtonVisible));
            }
        }
        private bool _cancelRequestReRegistrationButtonVisible;
        public bool CancelRequestReRegistrationButtonVisible
        {
            get => _cancelRequestReRegistrationButtonVisible;
            set
            {
                _cancelRequestReRegistrationButtonVisible = value;
                OnPropertyChanged(nameof(CancelRequestReRegistrationButtonVisible));
            }
        }
        private bool _allowDirectRegistrationPayment;
        public bool AllowDirectRegistrationPayment
        {
            get => _allowDirectRegistrationPayment;
            set
            {
                _allowDirectRegistrationPayment = value;
                OnPropertyChanged(nameof(AllowDirectRegistrationPayment));
            }
        }
        List<RegistrationAppointmentAvailableTimeView> _appointmentAvailableTimeData = new List<RegistrationAppointmentAvailableTimeView>();
        public List<RegistrationAppointmentAvailableTimeView> AppointmentAvailableTimeData
        {
            get => _appointmentAvailableTimeData;
            set
            {
                _appointmentAvailableTimeData = value;
                OnPropertyChanged(nameof(AppointmentAvailableTimeData));
            }
        }
        private bool _isCurrentYear;
        public bool IsCurrentYear
        {
            get => _isCurrentYear;
            set
            {
                _isCurrentYear = value;
                OnPropertyChanged(nameof(IsCurrentYear));
            }
        }
        private bool _isAddChildVisible;
        public bool IsAddChildVisible
        {
            get => _isAddChildVisible;
            set
            {
                _isAddChildVisible = value;
                OnPropertyChanged(nameof(IsAddChildVisible));
            }
        }
        private bool _isTermsAndConditionPopUpEnabled;
        public bool IsTermsAndConditionPopUpEnabled
        {
            get => _isTermsAndConditionPopUpEnabled;
            set
            {
                _isTermsAndConditionPopUpEnabled = value;
                OnPropertyChanged(nameof(IsTermsAndConditionPopUpEnabled));
            }
        }
        private string _termsConditionMessage;
        public string TermsConditionMessage
        {
            get => _termsConditionMessage;
            set
            {
                _termsConditionMessage = value;
                OnPropertyChanged(nameof(TermsConditionMessage));
            }
        }
        private bool _isWaitingListMessage;
        public bool IsWaitingListMessage
        {
            get => _isWaitingListMessage;
            set
            {
                _isWaitingListMessage = value;
                OnPropertyChanged(nameof(IsWaitingListMessage));
            }
        }
        #endregion
        public RegistrationForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
            MessagingCenter.Subscribe<string>("", "UpdateReregistration", async (argu) =>
            {
                await GetRegistrationData();
            });
        }

        #region Methods
        private async void InitializePage()
        {
            GetRegistrationData();
            UpdateDetailsCommand = new Command(UpdateDetailsMethod);
            BookAppointmentCommand = new Command(BookAppointmentMethod);
            ReregistrationCommand = new Command(ReregistrationMethod);
            CancelRequestCommand = new Command(CancelRequestMethod);
            CurrentYearRegistrationCommand = new Command(CurrentYearRegistrationMethod);
            NextYearRegistrationCommand = new Command(NextYearRegistrationMethod);
            RegistrationInstructionsCommand = new Command(RegistrationInstructionsMethod);
            ExpandCollapseClickCommand = new Command<BindableRegistrationExistingStudentView>(ExpandCollapseClicked);
            NewExpandCollapseClickCommand = new Command<BindableRegistrationExistingStudentView>(NewExpandCollapseClicked);
            StudentListExpandCollapseCommand = new Command<BindableRegistrationExistingStudentView>(StudentListExpandCollapseClicked);
            NewStudentsCollapseClickCommand = new Command<BindableRegistrationNewStudentView>(NewStudentsCollapseClicked);
            ModifyBookingCommand = new Command(BookAppointmentMethod);
            CancelCommand = new Command<RegistrationAppointmentBookingView>(CancelBookingMethod);
            CurrentYearCheckboxClickCommand = new Command<BindableRegistrationExistingStudentView>(CurrentYearCheckboxClickMethod);
            NextYearCheckboxClickCommand = new Command<BindableRegistrationExistingStudentView>(NextYearCheckboxClickMethod);
            NextYearReregCheckboxClickCommand = new Command<BindableRegistrationExistingStudentView>(NextYearReregCheckboxClickMethod);
            CurrentYearReregCheckboxClickCommand = new Command<BindableRegistrationExistingStudentView>(CurrentYearReregCheckboxClickMethod);
            UpdatePhotoClickCommand = new Command<BindableRegistrationExistingStudentView>(UpdatePhotoClickMethod);
            PayNowCommandCurrentYear = new Command<BindableRegistrationExistingStudentView>(obj => PayNowMethod(obj, isCurrentYear: true));
            PayNowCommandNextYear = new Command<BindableRegistrationExistingStudentView>(obj => PayNowMethod(obj, isCurrentYear: false));
            ViewReceiptClickCommand = new Command<BindableRegistrationExistingStudentView>(ViewReceiptClickMethod);
            DeleteNewStudentCommand = new Command<BindableRegistrationNewStudentView>(DeleteNewStudentMethod);
            EditNewStudentsDetalsCommand = new Command<BindableRegistrationNewStudentView>(EditNewStudentsDetalsMethod);
            BookAppointmentPerStudentCommand = new Command<BindableRegistrationNewStudentView>(BookAppointmentPerStudentMethod);
            CancelAppointmentPerStudentCommand = new Command<BindableRegistrationNewStudentView>(CancelAppointmentPerStudentMethod);
            KGInformationCommand = new Command<BindableRegistrationNewStudentView>(KGInformationMethod);
        }

        public async Task GetRegistrationData()
        {
            try
            {
                RegistrationData = await ApiHelper.GetObject<RegistrationAppViewModel>(string.Format(TextResource.GetRegistrationPageDataApi, ""));
                IsErrorMessageVisible = !string.IsNullOrEmpty(RegistrationData.ErrorMessage);
                if (IsErrorMessageVisible)
                {
                    IsRegistrationDataVisible = false;
                    return;
                }
                var settings = RegistrationData.RegistrationSettings;
                TermsConditionMessage = settings.TermsConditionMessage;
                IsTermsAndConditionPopUpEnabled = !IsErrorMessageVisible && settings.IsTermCondition && !string.IsNullOrEmpty(settings.TermsConditionMessage) && !settings.IsFamilyTermsLogExist;
                if (IsTermsAndConditionPopUpEnabled)
                {
                    TermsAndConditionsPopupForm termsAndConditionsPopupForm = new (_mapper, _nativeServices, Navigation)
                    {
                        TermsConditionMessage = TermsConditionMessage
                    };
                    var termsAndConditionsPopup = new TermsAndConditionsPopupPage()
                    {
                        BindingContext = termsAndConditionsPopupForm
                    };
                    SetPopupInstance(termsAndConditionsPopup);
                    Application.Current.MainPage.ShowPopup(termsAndConditionsPopup);
                
                    termsAndConditionsPopupForm.TermConditionAccepted += async (sender, result) =>
                    {
                        if (result.Success == true)
                        {
                            IsTermsAndConditionPopUpEnabled = false;
                            if (result.Identifier == "EmailError")
                            {
                                await HelperMethods.ShowAlert("", result.Message);
                            }
                            AppSettings.Current.CurrentPopup.Close();
                        }
                        else
                        {
                            await HelperMethods.ShowAlert("", result.Message);
                        }
                    };
                }

                IsWaitingListMessage = !string.IsNullOrEmpty(RegistrationData.WaitingListMessage);
                IsRegistrationDataVisible = true;
                PageTitle = RegistrationData.PageTitle;

                RegistrationInstructions = settings.FrontPage;
                RequestReRegistrationButtonVisible = settings.IsAllowReRegistration;
                AllowDirectRegistrationPayment = settings.AllowDirectRegistrationPayment;
                IsCurrentYear = settings.IsCurrentYear;

                IsInstructionVisible = !string.IsNullOrEmpty(settings.FrontPage);

                IsAppointmentButtonVisible = settings.IsAppointmentBookingEnabled && !settings.IsPerStudentBooking;

                if (!settings.IsPerStudentBooking || settings.IsBooking)
                {
                    FamilyAppointmentBookings = RegistrationData.FamilyAppointmentBookings;
                    AppointmentListViewHeight = FamilyAppointmentBookings.Count() * 50;
                    IsBookingsVisible = FamilyAppointmentBookings.Any();
                    IsAppointmentButtonVisible = settings.IsAppointmentBookingEnabled && FamilyAppointmentBookings.Count() <= 0;
                    IsModifyVisible = settings.IsBooking;
                }

                ReregistrationCompletedLabel = settings.ReregistrationCompletedLabel;
                CurrentYearExistingStudentGridTitle = RegistrationData.CurrentYearExistingStudentGridTitle;
                NextYearExistingStudentGridTitle = RegistrationData.NextYearExistingStudentGridTitle;
                KgInformationMissingMessage = settings.KgInformationMissingMessage;
                CurrentYearExistingStudents = new ObservableCollection<BindableRegistrationExistingStudentView>(_mapper.Map<List<BindableRegistrationExistingStudentView>>(RegistrationData.CurrentYearExistingStudents));
                NextYearExistingStudents = new ObservableCollection<BindableRegistrationExistingStudentView>(_mapper.Map<List<BindableRegistrationExistingStudentView>>(RegistrationData.NextYearExistingStudents));
                StudentNamesList = new ObservableCollection<BindableRegistrationExistingStudentView>(_mapper.Map<List<BindableRegistrationExistingStudentView>>(RegistrationData.StudentNamesList));

                CurrentYearExistingStudentsVisible = CurrentYearExistingStudents.Any();
                NextYearExistingStudentsVisible = NextYearExistingStudents.Any();
                StudentNamesListVisible = StudentNamesList.Any();

                CurrentYearListViewHeight = CurrentYearExistingStudents.Count * 65;
                NextYearListViewHeight = NextYearExistingStudents.Count * 65;
                StudentsListViewHeight = StudentNamesList.Count * 65;

                RegistrationNewStudentList = new ObservableCollection<BindableRegistrationNewStudentView>(_mapper.Map<List<BindableRegistrationNewStudentView>>(RegistrationData.NewStudents));
                NewStudentsListViewHeight = RegistrationNewStudentList.Count() * 52;

                foreach (var student in RegistrationNewStudentList)
                {
                    bool allowEditingNewStudents = settings.AllowEditingStudent(student.IsDataPulledInIBOS, student.IsDataNotPulledOrNotProceededInIBOS);
                    if (allowEditingNewStudents)
                    {
                        student.IsEditDeleteButtonVisible = true;
                        if (settings.IsKgStudentRegistrationFormEnabled(student.GradeLevelNo))
                        {
                            student.IsKGInformationButtonVisible = true;
                            student.KgInformationButtonCaption = "KG Info" + (!student.IsKgInformationSubmitted ? " (Mandatory)" : "");
                        }
                    }
                    student.IsPerStudentAppointmentButtonVisible = settings.IsPerStudentAppointmentBookingEnabled(student.GradeId);
                    if (student.RegistrationBookingId > 0)
                    {
                        student.IsBookingDateButtonVisible = true;
                        student.IsCancelButtonVisible = true;
                        student.IsPerStudentAppointmentButtonVisible = false;
                    }
                }

                if (settings.IsRegistrationFees)
                {
                    ShowRegistrationFees = true;
                    if (settings.IsTransportationDepositEnabled)
                    {
                        foreach (var student in CurrentYearExistingStudents)
                        {
                            student.ShowTransportationDeposit = student.TransportationFeeMessage != "-";
                        }
                        foreach (var student in NextYearExistingStudents)
                        {
                            student.ShowTransportationDeposit = student.TransportationFeeMessage != "-";

                        }
                    }
                }

                AppSettings.Current.IsGenderWiseSettingEnabled = settings.IsGenderWiseSettingEnabled;
                IsAddChildVisible = RegistrationData.CanAddChildForCurrentYear || RegistrationData.CanAddChildForNextYear;
                RequestReRegistrationButtonVisible = RegistrationData.RequestReRegistrationButtonVisible;
                CancelRequestReRegistrationButtonVisible = RegistrationData.CancelRequestReRegistrationButtonVisible;
                ConfirmationMessage = RegistrationData.PaymentSettings.ConfirmationMessage;
                TermsAndConditions = RegistrationData.PaymentSettings.TermsAndConditionMessage;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void UpdateDetailsMethod(object obj)
        {
            try
            {
                UpdateFamilyDetailsForm updateFamilyDetails = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Update Family Details",
                    MenuVisible = false,
                    BackVisible = true
                };    
                await updateFamilyDetails.GetRegistrationFormFamilyDetails();
                UpdateFamilyDetailsPage updateFamilyDetailsPage = new UpdateFamilyDetailsPage()
                {
                    BindingContext = updateFamilyDetails
                };
                await Navigation.PushAsync(updateFamilyDetailsPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void BookAppointmentMethod(object obj)
        {
            try
            {
                AppointmentAvailableTimeData = await ApiHelper.GetObjectList<RegistrationAppointmentAvailableTimeView>(string.Format(TextResource.GetRegistrationAvailableTimesApi, RegistrationData.ExistingOrNewFamilyId));
                RegistrationAppointmentPopupForm registrationAppointmentPopupForm = new (_mapper, _nativeServices, Navigation)
                {
                    FamilyId = RegistrationData.ExistingOrNewFamilyId,
                    AppointmentAvailableTimeData = AppointmentAvailableTimeData,
                    AppointmentDateList = AppointmentAvailableTimeData.GroupBy(x => x.BookingDateFormatted).Select(dt => dt.FirstOrDefault()).ToList()
                };
                if (registrationAppointmentPopupForm.AppointmentDateList != null && registrationAppointmentPopupForm.AppointmentDateList.Count() > 0)
                {
                    var registrationAppointmentPopup = new RegistrationAppointmentPopup()
                    {
                        BindingContext = registrationAppointmentPopupForm
                    };
                    SetPopupInstance(registrationAppointmentPopup);
                    Application.Current.MainPage.ShowPopup(registrationAppointmentPopup);
                }
                else
                    await HelperMethods.ShowAlert(this.PageTitle, TextResource.NoAppointmentDateMessage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        public void SetPopupInstance(Popup popup)
        {
            AppSettings.Current.CurrentPopup = popup;
        }

        private async void ReregistrationMethod(object obj)
        {
            RequestForReregisterForm requestForReregister = new (_mapper, _nativeServices, Navigation)
            {
                PageTitle = "Request For Reregister",
                MenuVisible = false,
                BackVisible = true,
                StudentIds = string.Join(",", NextYearExistingStudents
                    .Where(student => student.ReRegistrationCheckBoxChecked)
                    .Select(student => student.StudentId.ToString())),
                FamilyId = RegistrationData.ExistingOrNewFamilyId,
                AllowDirectRegistrationPayment = AllowDirectRegistrationPayment
            };
            if (!string.IsNullOrEmpty(requestForReregister.StudentIds))
            {
                await requestForReregister.GetReregistrationData();
                RequestForReregisterPage requestForReregisterPage = new ()
                {
                    BindingContext = requestForReregister
                };
                await Navigation.PushAsync(requestForReregisterPage);
            }
        }
        
        private async void CancelRequestMethod(object obj)
        {
            try
            {
                string studentIds = string.Join(",", NextYearExistingStudents.Select(student => student.StudentId.ToString()));
                var familyId = RegistrationData.ExistingOrNewFamilyId;
                var actionCode = "CANCEL";
                var action = await App.Current.MainPage.DisplayAlert("", TextResource.CancelRegistrationText, TextResource.YesText, TextResource.NoText);
                if (action)
                {
                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.SaveExisitingStudentsReRegistrationApi, familyId, studentIds, actionCode), AppSettings.Current.ApiUrl);
                    if (result.Success)
                    {
                        await HelperMethods.ShowAlert("", TextResource.RegistrationSaveMessage);
                        await GetRegistrationData();
                    }
                    else
                    {
                        await HelperMethods.ShowAlert("", TextResource.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void CurrentYearRegistrationMethod(object obj)
        {
            try
            {
                if (IsTermsAndConditionPopUpEnabled)
                {
                    TermsAndConditionsPopupForm termsAndConditionsPopupForm = new (_mapper, _nativeServices, Navigation)
                    {
                        TermsConditionMessage = TermsConditionMessage
                    };
                    var termsAndConditionsPopup = new TermsAndConditionsPopupPage()
                    {
                        BindingContext = termsAndConditionsPopupForm
                    };
                    SetPopupInstance(termsAndConditionsPopup);
                    Application.Current.MainPage.ShowPopup(termsAndConditionsPopup);
                    
                    termsAndConditionsPopupForm.TermConditionAccepted += async (sender, result) =>
                    {
                        if (result.Success == true)
                        {
                            IsTermsAndConditionPopUpEnabled = false;
                            if (result.Identifier == "EmailError")
                            {
                                await HelperMethods.ShowAlert("", result.Message);
                            }
                            AppSettings.Current.CurrentPopup.Close();
                            await NavigateToAddNewChildForm(RegistrationData.CurrentAcademicBeginYear, true);
                        }
                        else
                        {
                            await HelperMethods.ShowAlert("", result.Message);
                        }
                    };
                }
                else
                {
                    await NavigateToAddNewChildForm(RegistrationData.CurrentAcademicBeginYear, true);
                }
                
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async Task NavigateToAddNewChildForm(int academicYear, bool isCurrentYear)
        {
            try
            {
                AddNewChildForm addNewChildForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Add new child",
                    MenuVisible = false,
                    BackVisible = true
                };
                AppSettings.Current.AcademicYear = academicYear;
                await addNewChildForm.GetRegistrationFormStudentDetails(0, isCurrentYear);
                AddNewChildPage addNewChildPage = new AddNewChildPage()
                {
                    BindingContext = addNewChildForm
                };
                await Navigation.PushAsync(addNewChildPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }


        private async void NextYearRegistrationMethod(object obj)
        {
            try
            {
                if (IsTermsAndConditionPopUpEnabled)
                {
                    TermsAndConditionsPopupForm termsAndConditionsPopupForm = new (_mapper, _nativeServices, Navigation)
                    {
                        TermsConditionMessage = TermsConditionMessage
                    };
                    var termsAndConditionsPopup = new TermsAndConditionsPopupPage()
                    {
                        BindingContext = termsAndConditionsPopupForm
                    };
                    SetPopupInstance(termsAndConditionsPopup);
                    Application.Current.MainPage.ShowPopup(termsAndConditionsPopup);
                    
                    termsAndConditionsPopupForm.TermConditionAccepted += async (sender, result) =>
                    {
                        if (result.Success == true)
                        {
                            IsTermsAndConditionPopUpEnabled = false;
                            if (result.Identifier == "EmailError")
                            {
                                await HelperMethods.ShowAlert("", result.Message);
                            }
                            AppSettings.Current.CurrentPopup.Close();
                            await NavigateToAddNewChildForm(RegistrationData.NextAcademicBeginYear, false);
                        }
                        else
                        {
                            await HelperMethods.ShowAlert("", result.Message);
                        }
                    };
                }
                else
                {
                    await NavigateToAddNewChildForm(RegistrationData.NextAcademicBeginYear, false);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private void RegistrationInstructionsMethod(object obj)
        {
            RegistrationInstructionPopup registrationInstructionPopup = new()
            {
                BindingContext = this
            };
            Application.Current.MainPage.ShowPopup(registrationInstructionPopup);
        }
        private void ExpandCollapseClicked(BindableRegistrationExistingStudentView bindableRegistration)
        {
            try
            {
                if (bindableRegistration != null)
                {
                    foreach (var item in CurrentYearExistingStudents.ToList())
                    {
                        if (item != null)
                        {
                            if (item.StudentId == bindableRegistration.StudentId)
                            {
                                item.DetailsVisibility = !item.DetailsVisibility;
                                item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                                CurrentYearListViewHeight += item.DetailsVisibility ? 150 : -150;
                            }
                        }
                    }
                    MessagingCenter.Send("", "CurrentExpandCollapse");
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void NewExpandCollapseClicked(BindableRegistrationExistingStudentView bindableRegistration)
        {
            try
            {
                if (bindableRegistration != null)
                {
                    foreach (var item in NextYearExistingStudents.ToList())
                    {
                        if (item != null)
                        {
                            if (item.StudentId == bindableRegistration.StudentId)
                            {
                                item.DetailsVisibility = !item.DetailsVisibility;
                                item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                                NextYearListViewHeight += item.DetailsVisibility ? 150 : -150;
                            }
                        }
                    }
                    MessagingCenter.Send("", "NextExpandCollapse");
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void NewStudentsCollapseClicked(BindableRegistrationNewStudentView bindableRegistration)
        {
            try
            {
                if (bindableRegistration != null)
                {
                    foreach (var item in RegistrationNewStudentList.ToList())
                    {
                        if (item != null)
                        {
                            if (item.RegistrationId == bindableRegistration.RegistrationId)
                            {
                                item.DetailsVisibility = !item.DetailsVisibility;
                                item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                            }
                            else
                            {
                                item.DetailsVisibility = false;
                                item.ArrowImageSource = "dropdown_gray.png";
                            }
                        }
                    }
                    MessagingCenter.Send("", "NewStudentsExpandCollapse");
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void StudentListExpandCollapseClicked(BindableRegistrationExistingStudentView bindableRegistration)
        {
            try
            {
                if (bindableRegistration != null)
                {
                    foreach (var item in StudentNamesList.ToList())
                    {
                        if (item != null)
                        {
                            if (item.StudentId == bindableRegistration.StudentId)
                            {
                                item.DetailsVisibility = !item.DetailsVisibility;
                                item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                                StudentsListViewHeight += item.DetailsVisibility ? 150 : -150;
                            }
                        }
                    }
                    MessagingCenter.Send("", "StudentListExpandCollapse");
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        
        private async void ModifyBookingMethod(object obj)
        {
            try
            {
                // AppointmentAvailableTimeData = await ApiHelper.GetObjectList<RegistrationAppointmentAvailableTimeView>(string.Format(TextResource.GetRegistrationAvailableTimesApi, RegistrationData.ExistingOrNewFamilyId));
                // RegistrationAppointmentPopupForm appointmentPopupForm = new RegistrationAppointmentPopupForm();
                // appointmentPopupForm.FamilyId = RegistrationData.ExistingOrNewFamilyId;
                // appointmentPopupForm.AppointmentAvailableTimeData = AppointmentAvailableTimeData;
                // appointmentPopupForm.AppointmentDateList = AppointmentAvailableTimeData.GroupBy(x => x.BookingDateFormatted).Select(dt => dt.FirstOrDefault()).ToList();
                // if (appointmentPopupForm.AppointmentDateList != null && appointmentPopupForm.AppointmentDateList.Count() > 0)
                //     PopupNavigation.Instance.PushAsync(new RegistrationAppointmentPopup(appointmentPopupForm), true);
                // else
                //     await HelperMethods.ShowAlert(this.PageTitle, TextResource.NoAppointmentDateMessage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void CancelBookingMethod(RegistrationAppointmentBookingView registrationAppointment)
        {
            try
            {
                var action = await App.Current.MainPage.DisplayAlert("", string.Format(TextResource.RegistrationAppointmentCancelMessage), TextResource.YesText, TextResource.NoText);
                if (action)
                {
                    var registrationBookingUId = registrationAppointment.RegistrationBookingUId;
                    var registrationBookingId = registrationAppointment.RegistrationBookingId;
                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.BookStudentRegistrationAppointmentApi,
                        RegistrationData.ExistingOrNewFamilyId, registrationBookingUId, "", "", "", "", registrationBookingId, BookingActions.Cancel), AppSettings.Current.ApiUrl);

                    await HelperMethods.ShowAlertWithAction("", result.Message, async () =>
                    {
                        await GetRegistrationData();
                    });
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void CurrentYearCheckboxClickMethod(BindableRegistrationExistingStudentView student)
        {
            try
            {
                if (student.TransportationFeeCheckBoxChecked)
                {
                    student.TransportationFeeCheckBoxChecked = false;
                }
                else
                {
                    var action = await App.Current.MainPage.DisplayAlert("Transportation Services", "Do you wish to include transportation services to the academic year (2022-2023) ?", "Accept", "Cancel");
                    if (action)
                    {
                        student.TransportationFeeCheckBoxChecked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void NextYearCheckboxClickMethod(BindableRegistrationExistingStudentView student)
        {
            try
            {
                if(student.TransportationFeeCheckBoxChecked)
                {
                    student.TransportationFeeCheckBoxChecked = false;
                }
                else
                {
                    var action = await App.Current.MainPage.DisplayAlert("Transportation Services", "Do you wish to include transportation services to the academic year (2022-2023) ?", "Accept", "Cancel");
                    if (action)
                    {
                        student.TransportationFeeCheckBoxChecked = true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void CurrentYearReregCheckboxClickMethod(BindableRegistrationExistingStudentView student)
        {
            try
            {
                if (!student.ReRegistrationCheckBoxDisabled)
                {
                    student.ReRegistrationCheckBoxChecked = !student.ReRegistrationCheckBoxChecked;
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void NextYearReregCheckboxClickMethod(BindableRegistrationExistingStudentView student)
        {
            try
            {
                if (!student.ReRegistrationCheckBoxDisabled)
                {
                    student.ReRegistrationCheckBoxChecked = !student.ReRegistrationCheckBoxChecked;
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void UpdatePhotoClickMethod(BindableRegistrationExistingStudentView student)
        {
            try
            {
                var userProfileData = await ApiHelper.GetObject<UserProfileView>(string.Format(TextResource.GetProfilePhotoViewApi, student.StudentId));
                UpdatePhotoPopupForm updatePhotoPopupForm = new (_mapper, _nativeServices, Navigation)
                {
                    ProfilePicturePath = string.Empty
                };    
                updatePhotoPopupForm.ProfilePicturePath = userProfileData.ProfilePicturePath;
                updatePhotoPopupForm.StudentId = student.StudentId;
                var updatePhotoPopup = new UpdatePhotoPopup()
                {
                    BindingContext = updatePhotoPopupForm
                };
                SetPopupInstance(updatePhotoPopup);
                Application.Current.MainPage.ShowPopup(updatePhotoPopup);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void PayNowMethod(BindableRegistrationExistingStudentView obj, bool isCurrentYear)
        {
            try
            {
                var list = new List<ProformaInvoiceDetails>
                {
                    new ProformaInvoiceDetails()
                    {
                        StudentID = obj.StudentId,
                        ProFormaID = obj.StudentId,
                        Description = "Reregistration Fee",
                        TotalAmount = isCurrentYear ? obj.CurrentYearFeesAmount : obj.FeesAmount
                    }
                };
                ConfirmPaymentForm confirmPaymentForm = new(Navigation)
                {
                    PageTitle = "Confirm Payment",
                    MenuVisible = false,
                    BackVisible = true,
                    IsPopUpPage = false,
                    AcademicYear = isCurrentYear ? RegistrationData.CurrentAcademicBeginYear.ToString() : RegistrationData.NextAcademicBeginYear.ToString(),
                    Amount = isCurrentYear ? obj.CurrentYearFeesAmount : obj.FeesAmount,
                    Mode = (int)PaymentTypes.ReRegistrationPayment,
                    ConfirmationMessage = ConfirmationMessage,
                    StudentIds = obj.StudentId.ToString(),
                    AcademicYearForBillingDetails = isCurrentYear ? RegistrationData.CurrentAcademicBeginYear : RegistrationData.NextAcademicBeginYear,
                    TermsAndConditions = TermsAndConditions
                };
                AppSettings.Current.OnlinePaymentCurrencyCode = obj.CurrencyCode;
                confirmPaymentForm.ProformaListString = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                var confirmPaymentPage = new ConfirmPaymentPage()
                {
                    BindingContext = confirmPaymentForm
                };
                await Navigation.PushAsync(confirmPaymentPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        
        private void ViewReceiptClickMethod(BindableRegistrationExistingStudentView obj)
        {
            try
            {

            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void DeleteNewStudentMethod(BindableRegistrationNewStudentView student)
        {
            try
            {
                var action = await App.Current.MainPage.DisplayAlert("", string.Format(TextResource.DeleteText), TextResource.YesText, TextResource.NoText);
                if (action)
                {
                    var registrationUId = student.RegistrationUId;
                    var registrationId = student.RegistrationId;
                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.DeleteRegistrationDataApi,
                        registrationUId, registrationId), AppSettings.Current.ApiUrl);

                    await HelperMethods.ShowAlertWithAction("", result.Message, async () =>
                    {
                        await GetRegistrationData();
                    });
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void EditNewStudentsDetalsMethod(BindableRegistrationNewStudentView student)
        {
            try
            {
                AddNewChildForm addNewChildForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "Edit new child",
                    MenuVisible = false,
                    BackVisible = true
                };
                await addNewChildForm.GetRegistrationFormStudentDetails(student.RegistrationId, IsCurrentYear);
                AddNewChildPage addNewChildPage = new AddNewChildPage()
                {
                    BindingContext = addNewChildForm
                };
                await Navigation.PushAsync(addNewChildPage);            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void BookAppointmentPerStudentMethod(BindableRegistrationNewStudentView student)
        {
            try
            {
                AppointmentAvailableTimeData = await ApiHelper.GetObjectList<RegistrationAppointmentAvailableTimeView>(string.Format(TextResource.GetRegistrationAvailableTimesPerStudentApi, RegistrationData.ExistingOrNewFamilyId, student.RegistrationId));
                RegistrationAppointmentPopupForm registrationAppointmentPopupForm = new (_mapper, _nativeServices, Navigation)
                {
                    FamilyId = RegistrationData.ExistingOrNewFamilyId,
                    RegistrationId = student.RegistrationId,
                    AppointmentAvailableTimeData = AppointmentAvailableTimeData,
                    AppointmentDateList = AppointmentAvailableTimeData.GroupBy(x => x.BookingDateFormatted).Select(dt => dt.FirstOrDefault()).ToList()
                };
                if (registrationAppointmentPopupForm.AppointmentDateList != null && registrationAppointmentPopupForm.AppointmentDateList.Count() > 0)
                {
                    var registrationAppointmentPopup = new RegistrationAppointmentPopup()
                    {
                        BindingContext = registrationAppointmentPopupForm
                    };
                    SetPopupInstance(registrationAppointmentPopup);
                    Application.Current.MainPage.ShowPopup(registrationAppointmentPopup);
                }
                else
                    await HelperMethods.ShowAlert(this.PageTitle, TextResource.NoAppointmentDateMessage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void CancelAppointmentPerStudentMethod(BindableRegistrationNewStudentView student)
        {
            try
            {
                var action = await App.Current.MainPage.DisplayAlert("", string.Format(TextResource.RegistrationAppointmentCancelMessage), TextResource.YesText, TextResource.NoText);
                if (action)
                {
                    var registrationId = student.RegistrationId;
                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.BookStudentRegistrationAppointmentApi,
                        RegistrationData.ExistingOrNewFamilyId, "", registrationId, "", "", "", "", BookingActions.CancelPerStudentBooking), AppSettings.Current.ApiUrl);

                    await HelperMethods.ShowAlertWithAction("", result.Message, async () =>
                    {
                        await GetRegistrationData();
                    });
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void KGInformationMethod(BindableRegistrationNewStudentView student)
        {
            try
            {
                KGInformationForm kGInformationForm = new (_mapper, _nativeServices, Navigation)
                {
                    PageTitle = "KG Information",
                    MenuVisible = false,
                    BackVisible = true
                };
                await kGInformationForm.GetKGInformationDetails(student.RegistrationId);
                KGInformationPage kgInformationPage = new()
                {
                    BindingContext = kGInformationForm
                };
                await Navigation.PushAsync(kgInformationPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        #endregion
    }