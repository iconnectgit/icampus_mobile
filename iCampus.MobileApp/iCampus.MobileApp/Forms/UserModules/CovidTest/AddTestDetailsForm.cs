using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.CovidTest;

public class AddTestDetailsForm : ViewModelBase
{
    #region Declarations

    public ICommand SelectFileCommand { get; set; }
    public ICommand PositiveCheckedChangedCommand { get; set; }
    public ICommand NegativeCheckedChangedCommand { get; set; }
    public ICommand SaveClickedCommand { get; set; }

    #endregion

    public AddTestDetailsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper,
        null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Properties

    private string _personTested;

    public string PersonTested
    {
        get
        {
            //SetData();
            return _personTested;
        }
        set
        {
            _personTested = value;
            OnPropertyChanged(nameof(PersonTested));
        }
    }

    private string _positiveRadioButtonImage;

    public string PositiveRadioButtonImage
    {
        get => _positiveRadioButtonImage;
        set
        {
            _positiveRadioButtonImage = value;
            OnPropertyChanged(nameof(PositiveRadioButtonImage));
        }
    }

    private string _negativeRadioButtonImage;

    public string NegativeRadioButtonImage
    {
        get => _negativeRadioButtonImage;
        set
        {
            _negativeRadioButtonImage = value;
            OnPropertyChanged(nameof(NegativeRadioButtonImage));
        }
    }

    private DateTime _testDate;

    public DateTime TestDate
    {
        get => _testDate;
        set
        {
            _testDate = value;
            OnPropertyChanged(nameof(TestDate));
        }
    }

    private string _testLocation;

    public string TestLocation
    {
        get => _testLocation;
        set
        {
            _testLocation = value;
            OnPropertyChanged(nameof(TestLocation));
        }
    }

    private ObservableCollection<AttachmentFileView> _attachmentFiles = new();

    public ObservableCollection<AttachmentFileView> AttachmentFiles
    {
        get => _attachmentFiles;
        set
        {
            _attachmentFiles = value;
            OnPropertyChanged(nameof(AttachmentFiles));
        }
    }

    private int _attachmentListViewHeight;

    public int AttachmentListViewHeight
    {
        get => _attachmentListViewHeight;
        set
        {
            _attachmentListViewHeight = value;
            OnPropertyChanged(nameof(AttachmentListViewHeight));
        }
    }
    private string _mandatoryFieldErrorMessage;
    public string MandatoryFieldErrorMessage
    {
        get => _mandatoryFieldErrorMessage;
        set
        {
            _mandatoryFieldErrorMessage = value;
            OnPropertyChanged(nameof(MandatoryFieldErrorMessage));
        }
    }

    private bool _isLocationErrorMessageVisible = false;

    public bool IsLocationErrorMessageVisible
    {
        get => _isLocationErrorMessageVisible;
        set
        {
            _isLocationErrorMessageVisible = value;
            OnPropertyChanged(nameof(IsLocationErrorMessageVisible));
        }
    }

    private bool _isPersonTestedErrorMessageVisible = false;

    public bool IsPersonTestedErrorMessageVisible
    {
        get => _isPersonTestedErrorMessageVisible;
        set
        {
            _isPersonTestedErrorMessageVisible = value;
            OnPropertyChanged(nameof(IsPersonTestedErrorMessageVisible));
        }
    }

    private bool _isTestResultErrorMessageVisible = false;

    public bool IsTestResultErrorMessageVisible
    {
        get => _isTestResultErrorMessageVisible;
        set
        {
            _isTestResultErrorMessageVisible = value;
            OnPropertyChanged(nameof(IsTestResultErrorMessageVisible));
        }
    }

    private CovidTestView _selectedDetails = new();

    public CovidTestView SelectedDetails
    {
        get => _selectedDetails;
        set
        {
            _selectedDetails = value;
            OnPropertyChanged(nameof(SelectedDetails));
        }
    }

    private bool _testStatus;

    public bool TestStatus
    {
        get => _testStatus;
        set
        {
            _testStatus = value;
            OnPropertyChanged(nameof(TestStatus));
        }
    }

    private DateTime _currentDate = DateTime.Now;

    public DateTime CurrentDate
    {
        get => _currentDate;
        set
        {
            _currentDate = value;
            OnPropertyChanged(nameof(CurrentDate));
        }
    }

    private ExtPickListItem _selectedPersonTested = new();

    public ExtPickListItem SelectedPersonTested
    {
        get => _selectedPersonTested;
        set
        {
            _selectedPersonTested = value;
            OnPropertyChanged(nameof(SelectedPersonTested));
            IsChildListVisible =
                _selectedPersonTested != null && !string.IsNullOrEmpty(_selectedPersonTested.ItemId) &&
                _selectedPersonTested.ItemId.ToLower().Equals("c")
                    ? true
                    : false;
            if (_selectedPersonTested != null && !string.IsNullOrEmpty(_selectedPersonTested.ItemId) &&
                _selectedPersonTested.ItemId.ToLower().Equals("f"))
                SelectedDetails.PersonName = PersonTested =
                    SelectedDetails != null && !string.IsNullOrEmpty(SelectedDetails.FatherName)
                        ? SelectedDetails.FatherName
                        : string.Empty;
            else if (_selectedPersonTested != null && !string.IsNullOrEmpty(_selectedPersonTested.ItemId) &&
                     _selectedPersonTested.ItemId.ToLower().Equals("m"))
                SelectedDetails.PersonName = PersonTested =
                    SelectedDetails != null && !string.IsNullOrEmpty(SelectedDetails.MotherName)
                        ? SelectedDetails.MotherName
                        : string.Empty;
            else if (_selectedPersonTested != null && !string.IsNullOrEmpty(_selectedPersonTested.ItemId) &&
                     _selectedPersonTested.ItemId.ToLower().Equals("c"))
                SelectedDetails.PersonName = PersonTested = string.Empty;
        }
    }

    private BindableStudentPickListItem _selectedChild = new();

    public BindableStudentPickListItem SelectedChild
    {
        get => _selectedChild;
        set
        {
            _selectedChild = value;
            OnPropertyChanged(nameof(SelectedChild));
            if (_selectedChild != null && !string.IsNullOrEmpty(_selectedChild.ItemName))
            {
                SelectedDetails.PersonName = PersonTested = _selectedChild.ItemName;
            }
            // PersonTested = _selectedChild != null && !string.IsNullOrEmpty(_selectedChild.ItemName)
            //     ? _selectedChild.ItemName
            //     : string.Empty;
            // SelectedDetails.PersonName = PersonTested;
        }
    }

    private ObservableCollection<ExtPickListItem> _personTypeList = new();

    public ObservableCollection<ExtPickListItem> PersonTypeList
    {
        get => _personTypeList;
        set
        {
            _personTypeList = value;
            OnPropertyChanged(nameof(PersonTypeList));
        }
    }

    private bool _isChildListVisible = false;

    public bool IsChildListVisible
    {
        get => _isChildListVisible;
        set
        {
            _isChildListVisible = value;
            OnPropertyChanged(nameof(IsChildListVisible));
        }
    }

    private bool _isPersonTypeListVisible = false;

    public bool IsPersonTypeListVisible
    {
        get => _isPersonTypeListVisible;
        set
        {
            _isPersonTypeListVisible = value;
            OnPropertyChanged(nameof(IsPersonTypeListVisible));
        }
    }

    private int _userRefId;

    public int UserRefId
    {
        get => _userRefId;
        set
        {
            _userRefId = value;
            OnPropertyChanged(nameof(UserRefId));
        }
    }

    private int _userTypeId;

    public int UserTypeId
    {
        get => _userTypeId;
        set
        {
            _userTypeId = value;
            OnPropertyChanged(nameof(UserTypeId));
        }
    }

    private int _personType;

    public int PersonType
    {
        get => _personType;
        set
        {
            _personType = value;
            OnPropertyChanged(nameof(PersonType));
        }
    }

    #endregion

    #region Methods

    private async void InitializePage()
    {
        SelectFileCommand = new Command(SelectFileClicked);
        PositiveCheckedChangedCommand = new Command(PositiveClicked);
        NegativeCheckedChangedCommand = new Command(NegativeClicked);
        PositiveRadioButtonImage = "unselected_radio_button.png";
        NegativeRadioButtonImage = "selected_radio_button.png";
        SaveClickedCommand = new Command(SaveClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        MandatoryFieldErrorMessage = TextResource.MandatoryFieldErrorMessage;
        TestDate = DateTime.Now;
        IsPersonTypeListVisible = AppSettings.Current.IsTeacher ? false : true;
    }

    private async void SelectFileClicked(object obj)
    {
        try
        {
            var fileData = await HelperMethods.PickFileFromDevice();
            if (fileData == null)
            {
                return;
            }
            if (AttachmentFiles.Any(x => x.FileName.Equals(fileData.FileName, StringComparison.OrdinalIgnoreCase)))
            {
                await HelperMethods.ShowAlert("", "This file has already been added.");
                return;
            }
            AttachmentFiles = new ObservableCollection<AttachmentFileView>();
            AttachmentFiles.AddFileToList(fileData);
            AttachmentListViewHeight = AttachmentFiles.Count * 60;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void PositiveClicked()
    {
        PositiveRadioButtonImage = "selected_radio_button.png";
        NegativeRadioButtonImage = "unselected_radio_button.png";
        TestStatus = true;
    }

    private async void NegativeClicked()
    {
        PositiveRadioButtonImage = "unselected_radio_button.png";
        NegativeRadioButtonImage = "selected_radio_button.png";
        TestStatus = false;
    }

    private async void SaveClicked()
    {
        try
        {
            if (IsValid())
            {
                SelectedDetails.TestStatus = TestStatus;
                var list = new List<AttachmentFileView>();
                if (AttachmentFiles != null && AttachmentFiles.Count > 0)
                {
                    list = new List<AttachmentFileView>(AttachmentFiles);
                    SelectedDetails.DocumentName = list.FirstOrDefault().FileName;
                }

                SelectedDetails.TestLocation = TestLocation;
                SelectedDetails.TestDate = TestDate;
                SelectedDetails.UploadedDate = TestDate;

                if (AppSettings.Current.IsTeacher)
                {
                    if (AppSettings.Current.UserRefId != null)
                        //SelectedDetails.UserRefId = Convert.ToInt16(AppSettings.Current.UserRefId);
                        SelectedDetails.UserRefId = (int)AppSettings.Current.UserRefId;
                }
                else if (AppSettings.Current.IsParent)
                {
                    if (SelectedPersonTested != null && !string.IsNullOrEmpty(SelectedPersonTested.ItemId) &&
                        SelectedPersonTested.ItemId.ToLower().Equals("f"))
                    {
                        if (AppSettings.Current.UserRefId != null)
                            SelectedDetails.UserRefId = (int)AppSettings.Current.UserRefId;
                        SelectedDetails.UserTypeId = (int)PortalUserTypes.Parent;
                        SelectedDetails.PersonType = SelectedPersonTested.ItemId;
                        ;
                    }
                    else if (SelectedPersonTested != null && !string.IsNullOrEmpty(SelectedPersonTested.ItemId) &&
                             SelectedPersonTested.ItemId.ToLower().Equals("m"))
                    {
                        if (AppSettings.Current.UserRefId != null)
                            SelectedDetails.UserRefId = (int)AppSettings.Current.UserRefId;
                        SelectedDetails.UserTypeId = (int)PortalUserTypes.Parent;
                        SelectedDetails.PersonType = SelectedPersonTested.ItemId;
                        ;
                    }
                    else if (SelectedPersonTested != null && !string.IsNullOrEmpty(SelectedPersonTested.ItemId) &&
                             SelectedPersonTested.ItemId.ToLower().Equals("c"))
                    {
                        if (SelectedChild != null) SelectedDetails.UserRefId = Convert.ToInt32(SelectedChild.ItemId);
                        SelectedDetails.UserTypeId = (int)PortalUserTypes.Student;
                        SelectedDetails.PersonType = SelectedPersonTested.ItemId;
                        if (SelectedChild.ItemId != null)
                            SelectedDetails.TestedChild = Convert.ToInt32(SelectedChild.ItemId);
                    }
                }

                var result = await ApiHelper.PostMultiDataRequestAsync<bool>(TextResource.UpdateCovidTestData,
                    AppSettings.Current.ApiUrl, SelectedDetails, list);
                if (result)
                {
                    MessagingCenter.Send<AddTestDetailsForm>(this, "AddTestDetails");
                    await Navigation.PopAsync();
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private bool IsValid()
    {
        IsLocationErrorMessageVisible = string.IsNullOrEmpty(TestLocation) ? true : false;
        IsTestResultErrorMessageVisible =
            AttachmentFiles.ToList() != null && AttachmentFiles.ToList().FirstOrDefault() != null ? false : true;
        IsPersonTestedErrorMessageVisible = string.IsNullOrEmpty(PersonTested) ? true : false;
        if (IsLocationErrorMessageVisible || IsTestResultErrorMessageVisible ||
            IsPersonTestedErrorMessageVisible) return false;
        return true;
    }

    public async Task SetTestStatus()
    {
        if (TestStatus)
            PositiveClicked();
        else
            NegativeClicked();
        AttachmentFiles.AddFileToList(SelectedDetails.AttachmentView);
        AttachmentListViewHeight = AttachmentFiles.Count * 60;
    }

    public async Task SetData()
    {
        try
        {
            if (SelectedPersonTested != null && !string.IsNullOrEmpty(SelectedPersonTested.ItemId) &&
                SelectedPersonTested.ItemId.ToLower().Equals("f"))
                SelectedDetails.PersonName = PersonTested =
                    SelectedDetails != null && !string.IsNullOrEmpty(SelectedDetails.FatherName)
                        ? SelectedDetails.FatherName
                        : string.Empty;
            if (AppSettings.Current.IsTeacher)
                SelectedDetails.PersonName = PersonTested = AppSettings.Current.DetailedDisplayName;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    #endregion
}