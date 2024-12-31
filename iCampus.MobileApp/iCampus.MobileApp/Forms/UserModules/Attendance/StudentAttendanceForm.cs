using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Attendance;
using iCampus.Portal.EditModels;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Attendance;

public class StudentAttendanceForm : ViewModelBase
{
    #region Declarations

    private Popup _currentPopup;
    public ICommand FilterClickCommand { get; set; }
    public ICommand CommentClickCommand { get; set; }
    public ICommand AttendanceTypeClickCommand { get; set; }
    public ICommand TypePickerTappedCommand { get; set; }
    public ICommand SaveCommentClickCommand { get; set; }
    public ICommand SortByTapCommand { get; set; }
    public ICommand CloseClickCommand { get; set; }

    #endregion

    #region Properties

    private string _selectedDate;

    public string SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged(nameof(SelectedDate));
        }
    }

    private TakeAttendanceView _attendaceData = new();

    public TakeAttendanceView AttendaceData
    {
        get => _attendaceData;
        set
        {
            _attendaceData = value;
            OnPropertyChanged(nameof(AttendaceData));
        }
    }

    private ExtPickListItem _selectedClass = new();

    public ExtPickListItem SelectedClass
    {
        get => _selectedClass;
        set
        {
            _selectedClass = value;
            OnPropertyChanged(nameof(SelectedClass));
        }
    }

    private ExtPickListItem _selectedPeriod = new();

    public ExtPickListItem SelectedPeriod
    {
        get => _selectedPeriod;
        set
        {
            _selectedPeriod = value;
            OnPropertyChanged(nameof(SelectedPeriod));
        }
    }

    private ExtPickListItem _selectedGrade = new();

    public ExtPickListItem SelectedGrade
    {
        get => _selectedGrade;
        set
        {
            _selectedGrade = value;
            OnPropertyChanged(nameof(SelectedGrade));
        }
    }

    private ExtPickListItem _selectedCourse = new();

    public ExtPickListItem SelectedCourse
    {
        get => _selectedCourse;
        set
        {
            _selectedCourse = value;
            OnPropertyChanged(nameof(SelectedCourse));
        }
    }

    private List<BindablePickListItem> _typeList = new();

    public List<BindablePickListItem> TypeList
    {
        get => _typeList;
        set
        {
            _typeList = value;
            OnPropertyChanged(nameof(TypeList));
        }
    }
    private BindablePickListItem _selectedType;

    public BindablePickListItem SelectedType
    {
        get => _selectedType;
        set
        {
            _selectedType = value;
            OnPropertyChanged(nameof(SelectedType));
        }
    }

    private bool _loadFilterPanelLists;

    public bool LoadFilterPanelLists
    {
        get => _loadFilterPanelLists;
        set
        {
            _loadFilterPanelLists = value;
            OnPropertyChanged(nameof(LoadFilterPanelLists));
        }
    }

    private IList<ExtPickListItem> _gradeList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> GradeList
    {
        get => _gradeList;
        set
        {
            _gradeList = value;
            OnPropertyChanged(nameof(GradeList));
        }
    }

    private IList<ExtPickListItem> _classList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> ClassList
    {
        get => _classList;
        set
        {
            _classList = value;
            OnPropertyChanged(nameof(ClassList));
        }
    }

    private IList<ExtPickListItem> _periodList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> PeriodList
    {
        get => _periodList;
        set
        {
            _periodList = value;
            OnPropertyChanged(nameof(PeriodList));
        }
    }

    private IList<ExtPickListItem> _courseList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> CourseList
    {
        get => _courseList;
        set
        {
            _courseList = value;
            OnPropertyChanged(nameof(CourseList));
        }
    }

    private IList<ExtPickListItem> _attendanceCommentList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> AttendanceCommentList
    {
        get => _attendanceCommentList;
        set
        {
            _attendanceCommentList = value;
            OnPropertyChanged(nameof(AttendanceCommentList));
        }
    }

    private bool _isElective;

    public bool IsElective
    {
        get => _isElective;
        set
        {
            _isElective = value;
            OnPropertyChanged(nameof(IsElective));
        }
    }

    private bool _isArabic;

    public bool IsArabic
    {
        get => _isArabic;
        set
        {
            _isArabic = value;
            OnPropertyChanged(nameof(IsArabic));
        }
    }

    private string _imageSource;

    public string ImageSource
    {
        get => _imageSource;
        set
        {
            _imageSource = value;
            OnPropertyChanged(nameof(ImageSource));
        }
    }

    private AttendanceEdit _attendanceEdit;

    public AttendanceEdit AttendanceEditData
    {
        get => _attendanceEdit;
        set
        {
            _attendanceEdit = value;
            OnPropertyChanged(nameof(AttendanceEditData));
        }
    }

    private ObservableCollection<BindableAttendanceEntryView> _attendanceEntries = new();

    public ObservableCollection<BindableAttendanceEntryView> AttendanceEntries
    {
        get => _attendanceEntries;
        set
        {
            _attendanceEntries = value;
            OnPropertyChanged(nameof(AttendanceEntries));
        }
    }

    private BindableAttendanceEntryView _selectedEntry = new();

    public BindableAttendanceEntryView SelectedEntry
    {
        get => _selectedEntry;
        set
        {
            _selectedEntry = value;
            OnPropertyChanged(nameof(SelectedEntry));
        }
    }

    private string _defaultSortByCode;

    public string DefaultSortByCode
    {
        get => _defaultSortByCode;
        set
        {
            _defaultSortByCode = value;
            OnPropertyChanged(nameof(DefaultSortByCode));
        }
    }

    private string _attendanceMode;

    public string AttendanceMode
    {
        get => _attendanceMode;
        set
        {
            _attendanceMode = value;
            OnPropertyChanged(nameof(AttendanceMode));
        }
    }

    private bool _noDataExist;

    public bool NoDataExist
    {
        get => _noDataExist;
        set
        {
            _noDataExist = value;
            OnPropertyChanged(nameof(NoDataExist));
        }
    }

    #endregion

    public StudentAttendanceForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper,
        null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        IsVisibleSaveHeaderIcon = true;
        FilterClickCommand = new Command(FilterClicked);
        CommentClickCommand = new Command(CommentClicked);
        AttendanceTypeClickCommand = new Command(AttendanceTypeClicked);
        TypePickerTappedCommand = new Command(TypePickerSelected);
        SaveCommentClickCommand = new Command(SaveCommentClicked);
        AttendanceSaveTapCommand = new Command(SaveStudentAttendance);
        CloseClickCommand = new Command(AttendanceTypeCloseClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        NoDataExist = true;
        TypeList = HelperMethods.GetAttendanceType();
        SortByTapCommand = new Command(SortByClicked);
        InitializePage();
    }

    #region Methods

    private void InitializePage()
    {
        MessagingCenter.Subscribe<StudentAttendanceFilterForm>(this, "SearchStudentAttendance",
            async (filterFormData) => { await FilterStudentAttendance(filterFormData); });
    }

    public async Task FilterStudentAttendance(StudentAttendanceFilterForm filterFormData)
    {
        try
        {
            SelectedDate = filterFormData.SelectedDate.ToString(TextResource.DateFormatKey3);
            IsElective = filterFormData.IsElective;
            IsArabic = filterFormData.IsArabic;
            GradeList = filterFormData.GradeList;
            ClassList = filterFormData.ClassList;
            CourseList = filterFormData.CourseList;
            PeriodList = filterFormData.PeriodList;
            SelectedGrade = filterFormData.SelectedGrade;
            SelectedClass = filterFormData.SelectedClass;
            SelectedPeriod = filterFormData.SelectedPeriod;
            SelectedCourse = filterFormData.SelectedCourse;
            AttendanceMode = filterFormData.AttendanceMode;
            DefaultSortByCode = filterFormData.DefaultSortByCode;
            PageTitle = filterFormData.PageTitle;
            await GetAttendanceData();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async void FilterClicked(object obj)
    {
        StudentAttendanceFilterForm studentAttendanceFilterForm = new(_mapper, _nativeServices, Navigation)
        {
            PageTitle = PageTitle,
            MenuVisible = false,
            BackVisible = true,
            SelectedDate = Convert.ToDateTime(SelectedDate),
            IsElective = IsElective,
            IsArabic = IsArabic,
            GradeList = GradeList,
            ClassList = ClassList,
            CourseList = CourseList,
            PeriodList = PeriodList,
            AttendanceMode = AttendanceMode,
            DefaultSortByCode = DefaultSortByCode
        };
        studentAttendanceFilterForm.InitializePage();
        StudentAttendanceFilterPage studentAttendanceFilterPage = new()
        {
            BindingContext = studentAttendanceFilterForm
        };
        await Navigation.PushAsync(studentAttendanceFilterPage);
    }

    private async void CommentClicked(object obj)
    {
        if (obj != null)
        {
            SelectedEntry = (BindableAttendanceEntryView)obj;
            SelectedEntry.AttendanceCommentList = AttendanceCommentList;
        }
        StudentCommentPopup studentCommentPopup = new ()
        {
            BindingContext = this
        };
        SetPopupInstance(studentCommentPopup);
        Application.Current.MainPage.ShowPopup(studentCommentPopup);
    }

    public async Task<TakeAttendanceView> GetAttendanceData()
    {
        try
        {
            var selectedCourse = SelectedCourse != null && SelectedCourse.ItemId != null ? SelectedCourse.ItemId : null;
            AttendaceData = await SISApiHelper.GetObject<TakeAttendanceView>(string.Format(
                TextResource.StudentAttendanceApiUrl,
                SelectedDate, SelectedClass.ItemId, AttendanceMode, selectedCourse, SelectedPeriod.ItemId,
                IsElective, DefaultSortByCode, IsArabic));

            if (AttendaceData != null)
            {
                AttendanceEntries =
                    _mapper.Map<ObservableCollection<BindableAttendanceEntryView>>(
                        new ObservableCollection<AttendanceEntryView>(AttendaceData.AttendanceEntry));
                AttendanceCommentList = AttendaceData.AttendanceComments;
            }

            NoDataExist = !AttendanceEntries.Any();
            if (DefaultSortByCode == "S")
                ImageSource = "sortbyicon.png";
            else
                ImageSource = "sortbyicon_2.png";
            return AttendaceData;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            return new TakeAttendanceView();
        }
    }

    private async void SortByClicked(object obj)
    {
        DefaultSortByCode = DefaultSortByCode == "S" ? "A" : "S";
        await GetAttendanceData();
    }

    private async void AttendanceTypeClicked(object obj)
    {
        if (obj != null) SelectedEntry = (BindableAttendanceEntryView)obj;
        StudentAttendanceTypePopup studentAttendanceTypePopup = new ()
        {
            BindingContext = this
        };
        SetPopupInstance(studentAttendanceTypePopup);
        Application.Current.MainPage.ShowPopup(studentAttendanceTypePopup);
    }
    public void SetPopupInstance(Popup popup)
    {
        _currentPopup = popup;
    }

    private async void TypePickerSelected(object obj)
    {
        try
        {
            if (obj != null)
                AttendanceEntries[AttendanceEntries.IndexOf(SelectedEntry)].SelectedType = (BindablePickListItem)obj;

            _currentPopup?.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async void SaveCommentClicked(object obj)
    {
        _currentPopup?.Close();
    }


    private async void SaveStudentAttendance()
    {
        try
        {
            AttendanceEditData = _mapper.Map<AttendanceEdit>(AttendanceEntries.FirstOrDefault());
            AttendanceEditData.ModifiedTeacherId = AppSettings.Current.UserRefId;
            AttendanceEditData.AttendanceDate = Convert.ToDateTime(SelectedDate);
            AttendanceEditData.ModifiedDate = DateTime.Now;
            if (SelectedCourse != null)
                AttendanceEditData.CurriculumId = Convert.ToInt16(SelectedCourse.ItemId);
            if (SelectedPeriod != null)
                AttendanceEditData.PeriodId = Convert.ToInt16(SelectedPeriod.ItemId);
            if (SelectedClass != null)
            {
                if (IsElective)
                    AttendanceEditData.ElectiveClassId = Convert.ToInt16(SelectedClass.ItemId);
                else
                    AttendanceEditData.ClassId = Convert.ToInt16(SelectedClass.ItemId);
            }

            AttendanceEditData.AttendanceDetails = _mapper.Map<AttendanceDetailsEdit[]>(AttendanceEntries.ToArray());
            var result =
                await SISApiHelper.PostRequest<OperationDetails>(TextResource.UpdateAttendanceApiUrl,
                    AttendanceEditData);
            if (result.Success) await HelperMethods.ShowAlert("", TextResource.AttendanceSaveSuccessMessage);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void AttendanceTypeCloseClicked(object obj)
    {
        await Navigation.PopAsync();
        //await PopupNavigation.Instance.PopAllAsync();
    }

    #endregion
}