using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class RequestForReregisterForm : ViewModelBase
	{
        #region Declaration
        public ICommand UpdatePhotoClickCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        
        #endregion
        #region Properties
        private string _studentIds;
        public string StudentIds
        {
            get => _studentIds;
            set
            {
                _studentIds = value;
                OnPropertyChanged(nameof(StudentIds));
            }
        }
        IEnumerable<RegistrationExistingStudentView> _existingStudents;
        public IEnumerable<RegistrationExistingStudentView> ExistingStudents
        {
            get => _existingStudents;
            set
            {
                _existingStudents = value;
                OnPropertyChanged(nameof(ExistingStudents));
            }
        }
        private bool _isRegistrationFeesVisible;
        public bool IsRegistrationFeesVisible
        {
            get => _isRegistrationFeesVisible;
            set
            {
                _isRegistrationFeesVisible = value;
                OnPropertyChanged(nameof(IsRegistrationFeesVisible));
            }
        }
        private bool _isStudentImageUploadVisible;
        public bool IsStudentImageUploadVisible
        {
            get => _isStudentImageUploadVisible;
            set
            {
                _isStudentImageUploadVisible = value;
                OnPropertyChanged(nameof(IsStudentImageUploadVisible));
            }
        }
        private decimal _totalFeesAmount;
        public decimal TotalFeesAmount
        {
            get => _totalFeesAmount;
            set
            {
                _totalFeesAmount = value;
                OnPropertyChanged(nameof(TotalFeesAmount));
            }
        }
        private string _currencyCode;
        public string CurrencyCode
        {
            get => _currencyCode;
            set
            {
                _currencyCode = value;
                OnPropertyChanged(nameof(CurrencyCode));
            }
        }
        private bool _isConfirmButtonVisible;
        public bool IsConfirmButtonVisible
        {
            get => _isConfirmButtonVisible;
            set
            {
                _isConfirmButtonVisible = value;
                OnPropertyChanged(nameof(IsConfirmButtonVisible));
            }
        }
        private bool _isUploadPhotoButtonVisible;
        public bool IsUploadPhotoButtonVisible
        {
            get => _isUploadPhotoButtonVisible;
            set
            {
                _isUploadPhotoButtonVisible = value;
                OnPropertyChanged(nameof(IsUploadPhotoButtonVisible));
            }
        }
        private int _listViewHeight;
        public int ListViewHeight
        {
            get => _listViewHeight;
            set
            {
                _listViewHeight = value;
                OnPropertyChanged(nameof(ListViewHeight));
            }
        }
        private long? _familyId;
        public long? FamilyId
        {
            get => _familyId;
            set
            {
                _familyId = value;
                OnPropertyChanged(nameof(FamilyId));
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
        
        #endregion

        public RequestForReregisterForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        #region Methods
        private async void InitializePage()
        {
            UpdatePhotoClickCommand = new Command<RegistrationExistingStudentView>(UpdatePhotoClickMethod);
            ConfirmCommand = new Command(ConfirmMethod);
        }

        public async Task GetReregistrationData()
        {
            try
            {
                var reregistrationdata = await ApiHelper.GetObject<RegistrationAppViewModel>(string.Format(TextResource.RequestForReregistrationApi, StudentIds));
                var settings = reregistrationdata.RegistrationSettings;
                ExistingStudents = reregistrationdata.ExistingStudents;
                ListViewHeight = ExistingStudents.Count() * 170;
                IsRegistrationFeesVisible = settings.IsRegistrationFees;
                IsStudentImageUploadVisible = settings.IsStudentImageUploadOptionEnabled;
                TotalFeesAmount = ExistingStudents.Select(a => a.TotalFeesAmount).FirstOrDefault();
                CurrencyCode = ExistingStudents.Select(x => x.CurrencyCode).FirstOrDefault();
                bool isAllStudentPictureValidated = settings.IsExistingStudentPictureValidated(ExistingStudents);
                
                IsConfirmButtonVisible = isAllStudentPictureValidated;
                IsUploadPhotoButtonVisible = !isAllStudentPictureValidated;

            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void UpdatePhotoClickMethod(RegistrationExistingStudentView student)
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
        public void SetPopupInstance(Popup popup)
        {
            AppSettings.Current.CurrentPopup = popup;
        }
        private async void ConfirmMethod(object obj)
        {
            try
            {
                var actionCode = "NEW";
                OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.SaveExisitingStudentsReRegistrationApi, FamilyId, StudentIds, actionCode), AppSettings.Current.ApiUrl);
                if (result.Success)
                {
                    string message = TextResource.RegistrationSaveMessage + "\n\n" + (!AllowDirectRegistrationPayment ? "Please make payment through registration payment page in Portal" : "");
                    await HelperMethods.ShowAlert("", message);
                    MessagingCenter.Send<string>("", "UpdateReregistration");
                    await Navigation.PopAsync();
                }
                else
                {
                    await HelperMethods.ShowAlert("", TextResource.Error);
                }

            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        #endregion
    }