using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Attendance;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Attendance;

public class StudentAttendanceFilterForm : ViewModelBase
{
    #region Declarations

    public ICommand GradePickerChangeCommand { get; set; }
    public ICommand ClassPickerChangeCommand { get; set; }
    public ICommand SearchClickCommand { get; set; }
    public ICommand CoursePickerChangeCommand { get; set; }
    public ICommand ElectiveCheckboxChangeCommand { get; set; }
    public ICommand DateSelectionChangeCommand { get; set; }

    #endregion

    #region Properties

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

    private DateTime _maxDate;

    public DateTime MaxDate
    {
        get => _maxDate;
        set
        {
            _maxDate = value;
            OnPropertyChanged(nameof(MaxDate));
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

    private ExtPickListItem _selectedClass = new();

    public ExtPickListItem SelectedClass
    {
        get => _selectedClass;
        set
        {
            _selectedClass = value;
            OnPropertyChanged(nameof(SelectedClass));
            if (SelectedClass != null && !string.IsNullOrEmpty(SelectedClass.ItemName))
            {
                ClassSelectionChanged();
                IsClassErrVisible = false;
            }
                
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
            if (SelectedPeriod != null && !string.IsNullOrEmpty(SelectedPeriod.ItemName))
                IsPeriodErrVisible = false;
        }
    }

    private bool _isHolidayDateVisible;

    public bool IsHolidayDateVisible
    {
        get => _isHolidayDateVisible;
        set
        {
            _isHolidayDateVisible = value;
            OnPropertyChanged(nameof(IsHolidayDateVisible));
        }
    }

    #region GridProperty

    private int _gridClassLabelRow;

    public int GridClassLabelRow
    {
        get => _gridClassLabelRow;
        set
        {
            _gridClassLabelRow = value;
            OnPropertyChanged(nameof(GridClassLabelRow));
        }
    }

    private int _gridClassPickerRow;

    public int GridClassPickerRow
    {
        get => _gridClassPickerRow;
        set
        {
            _gridClassPickerRow = value;
            OnPropertyChanged(nameof(GridClassPickerRow));
        }
    }

    private int _gridClassErrorRow;

    public int GridClassErrorRow
    {
        get => _gridClassErrorRow;
        set
        {
            _gridClassErrorRow = value;
            OnPropertyChanged(nameof(GridClassErrorRow));
        }
    }

    private int _gridClassColumn;

    public int GridClassColumn
    {
        get => _gridClassColumn;
        set
        {
            _gridClassColumn = value;
            OnPropertyChanged(nameof(GridClassColumn));
        }
    }

    private int _gridPeriodLabelRow;

    public int GridPeriodLabelRow
    {
        get => _gridPeriodLabelRow;
        set
        {
            _gridPeriodLabelRow = value;
            OnPropertyChanged(nameof(GridPeriodLabelRow));
        }
    }

    private int _gridPeriodPickerRow;

    public int GridPeriodPickerRow
    {
        get => _gridPeriodPickerRow;
        set
        {
            _gridPeriodPickerRow = value;
            OnPropertyChanged(nameof(GridPeriodPickerRow));
        }
    }

    private int _gridPeriodErrorRow;

    public int GridPeriodErrorRow
    {
        get => _gridPeriodErrorRow;
        set
        {
            _gridPeriodErrorRow = value;
            OnPropertyChanged(nameof(GridPeriodErrorRow));
        }
    }

    private int _gridPeriodColumn;

    public int GridPeriodColumn
    {
        get => _gridPeriodColumn;
        set
        {
            _gridPeriodColumn = value;
            OnPropertyChanged(nameof(GridPeriodColumn));
        }
    }

    private int _gridCourseLabelRow;

    public int GridCourseLabelRow
    {
        get => _gridCourseLabelRow;
        set
        {
            _gridCourseLabelRow = value;
            OnPropertyChanged(nameof(GridCourseLabelRow));
        }
    }

    private int _gridCoursePickerRow;

    public int GridCoursePickerRow
    {
        get => _gridCoursePickerRow;
        set
        {
            _gridCoursePickerRow = value;
            OnPropertyChanged(nameof(GridCoursePickerRow));
        }
    }

    private int _gridCourseErrorRow;

    public int GridCourseErrorRow
    {
        get => _gridCourseErrorRow;
        set
        {
            _gridCourseErrorRow = value;
            OnPropertyChanged(nameof(GridCourseErrorRow));
        }
    }

    private int _gridCourseColumn;

    public int GridCourseColumn
    {
        get => _gridCourseColumn;
        set
        {
            _gridCourseColumn = value;
            OnPropertyChanged(nameof(GridCourseColumn));
        }
    }

    #endregion

    private ExtPickListItem _selectedGrade = new();

    public ExtPickListItem SelectedGrade
    {
        get => _selectedGrade;
        set
        {
            _selectedGrade = value;
            OnPropertyChanged(nameof(SelectedGrade));
            if (SelectedGrade != null && !string.IsNullOrEmpty(SelectedGrade.ItemName))
            {
                GradeSelectionChanged();
                IsGradeErrVisible = false;
            }
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
            if (IsElective && SelectedCourse != null && !string.IsNullOrEmpty(SelectedCourse.ItemName))
            {
                CourseSelectionChanged();
                IsCourseErrVisible = false;
            }
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

    private bool _isGradeErrVisible;

    public bool IsGradeErrVisible
    {
        get => _isGradeErrVisible;
        set
        {
            _isGradeErrVisible = value;
            OnPropertyChanged(nameof(IsGradeErrVisible));
        }
    }

    private bool _isClassErrVisible;

    public bool IsClassErrVisible
    {
        get => _isClassErrVisible;
        set
        {
            _isClassErrVisible = value;
            OnPropertyChanged(nameof(IsClassErrVisible));
        }
    }

    private bool _isPeriodErrVisible;

    public bool IsPeriodErrVisible
    {
        get => _isPeriodErrVisible;
        set
        {
            _isPeriodErrVisible = value;
            OnPropertyChanged(nameof(IsPeriodErrVisible));
        }
    }

    private bool _isCourseErrVisible;

    public bool IsCourseErrVisible
    {
        get => _isCourseErrVisible;
        set
        {
            _isCourseErrVisible = value;
            OnPropertyChanged(nameof(IsCourseErrVisible));
        }
    }

    private TakeAttendanceView _filterData = new();

    public TakeAttendanceView FilterDataValue
    {
        get => _filterData;
        set
        {
            _filterData = value;
            OnPropertyChanged(nameof(FilterDataValue));
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

    #endregion

    public StudentAttendanceFilterForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(
        mapper, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        SearchClickCommand = new Command(SearchClicked);
        //GradePickerChangeCommand = new Command<ExtPickListItem>(GradeSelectionChanged);
        //ClassPickerChangeCommand = new Command<ExtPickListItem>(ClassSelectionChanged);
        //CoursePickerChangeCommand = new Command<ExtPickListItem>(CourseSelectionChanged);
        ElectiveCheckboxChangeCommand = new Command(ElectiveCheckChanged);
        DateSelectionChangeCommand = new Command(DateSelectionChanged);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        SetGridDefintion();
    }

    #region Methods

    public async void InitializePage()
    {
        SelectedDate = SelectedDate != null && SelectedDate != DateTime.MinValue
            ? SelectedDate
            : Convert.ToDateTime(DateTime.Today.AddDays(-1).ToString(TextResource.DateFormatKey3));
        MaxDate = DateTime.Today.AddHours(23);
        LoadFilterPanelLists = GradeList.Count == 0;
        if (LoadFilterPanelLists)
            await GetFilterListData();
    }

    private async void ClassSelectionChanged()
    {
        if (SelectedClass != null) await GetPeriodByClass();
    }

    private async void GradeSelectionChanged()
    {
        if (SelectedGrade != null) await GetCourseAndClassByGrade();
    }

    private async void DateSelectionChanged(object obj)
    {
        if (SelectedGrade != null && !string.IsNullOrEmpty(SelectedGrade.ItemId) && SelectedClass != null &&
            !string.IsNullOrEmpty(SelectedClass.ItemId))
        {
            PeriodList = new List<ExtPickListItem>();
            await GetPeriodByClass();
        }
    }

    private async void CourseSelectionChanged()
    {
        if (SelectedCourse != null && IsElective) await GetTeacherElectiveClasses();
    }

    private void ElectiveCheckChanged(object obj)
    {
        ClassList = new List<ExtPickListItem>();
        PeriodList = new List<ExtPickListItem>();
        CourseList = new List<ExtPickListItem>();
        SetGridDefintion();
        if (IsElective)
        {
            if (SelectedGrade != null && !string.IsNullOrEmpty(SelectedGrade.ItemId))
                Task.Run(async () => { await GetCourseAndClassByGrade(); });

            if (SelectedCourse != null && !string.IsNullOrEmpty(SelectedCourse.ItemName))
                IsCourseErrVisible = false;
        }
        else
        {
            IsCourseErrVisible = false;
            if (SelectedGrade != null && !string.IsNullOrEmpty(SelectedGrade.ItemId))
                Task.Run(async () => { await GetCourseAndClassByGrade(); });
        }
    }

    private bool ValidateData()
    {
        IsGradeErrVisible = SelectedGrade != null && SelectedGrade.ItemName != null ? false : true;
        IsClassErrVisible = SelectedClass != null && SelectedClass.ItemName != null ? false : true;
        IsPeriodErrVisible = SelectedPeriod != null && SelectedPeriod.ItemName != null ? false : true;
        if (IsElective)
            IsCourseErrVisible = SelectedCourse != null && SelectedCourse.ItemName != null ? false : true;
        else
            IsCourseErrVisible = false;
        return !IsGradeErrVisible && !IsClassErrVisible && !IsPeriodErrVisible && !IsCourseErrVisible ? true : false;
    }

    private async void SearchClicked(object obj)
    {
        if (ValidateData())
        {
            var navigationStack = Application.Current.MainPage.Navigation.NavigationStack;
            var page = navigationStack.ElementAtOrDefault(navigationStack.Count - 2);
            if (page.GetType() == typeof(StudentAttendanceForm))
            {
                MessagingCenter.Send(this, "SearchStudentAttendance");
                Navigation.PopAsync();
            }
            else
            {
                MessagingCenter.Send(this, "SearchStudentAttendance");
                StudentAttendanceForm studentAttendanceForm = new(_mapper, _nativeServices, Navigation)
                {
                    PageTitle = TextResource.StudentAttendancePageTitle,
                    MenuVisible = true
                };
                await studentAttendanceForm.FilterStudentAttendance(this);
                StudentAttendance studentAttendance = new StudentAttendance()
                {
                    BindingContext = studentAttendanceForm
                };
                await Navigation.PushAsync(studentAttendance);
            }
        }
    }

    public async Task GetCourseAndClassByGrade()
    {
        try
        {
            FilterDataValue = await SISApiHelper.GetObject<TakeAttendanceView>(
                string.Format(TextResource.GetStudentCourseAndClassApiUrl, SelectedGrade.ItemId, IsElective));
            if (FilterDataValue != null)
            {
                ClassList = FilterDataValue.Classes;
                CourseList = FilterDataValue.CourseList;
                AttendanceMode = FilterDataValue.AttendanceMode;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public async Task<IList<ExtPickListItem>> GetTeacherElectiveClasses()
    {
        try
        {
            ClassList = await SISApiHelper.GetObjectList<ExtPickListItem>(
                string.Format(TextResource.GetTeacherRelativeClassesByCourse, Convert.ToInt16(SelectedCourse.ItemId)));
            return ClassList;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            return new List<ExtPickListItem>();
        }
    }

    public async Task<IList<ExtPickListItem>> GetPeriodByClass()
    {
        try
        {
            var checkPeriodTime = SelectedDate >= DateTime.Today ? true : false;
            PeriodList = await SISApiHelper.GetObjectList<ExtPickListItem>(string.Format(
                TextResource.GetStudentPeriodApiUrl, SelectedClass.ItemId, SelectedDate.ToDateTime(), IsElective,
                checkPeriodTime, true));
            if (PeriodList != null && PeriodList.FirstOrDefault().ItemId == "-1" &&
                PeriodList.FirstOrDefault().ItemName.ToLower().Equals("holiday"))
            {
                IsHolidayDateVisible = true;
                PeriodList = new List<ExtPickListItem>();
            }
            else
            {
                IsHolidayDateVisible = false;
            }

            return PeriodList;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            return new List<ExtPickListItem>();
        }
    }

    public async Task<TakeAttendanceView> GetFilterListData()
    {
        try
        {
            AttendaceData =
                await SISApiHelper.GetObject<TakeAttendanceView>(
                    string.Format(TextResource.StudentAttendanceFilterListApiUrl, LoadFilterPanelLists));
            if (AttendaceData != null)
                DefaultSortByCode = AttendaceData.ByDefaultSortStudentsBy;
            if (AttendaceData != null && LoadFilterPanelLists)
                GradeList = AttendaceData.Grades;

            return AttendaceData;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            return new TakeAttendanceView();
        }
    }

    private void SetGridDefintion()
    {
        if (IsElective)
        {
            GridClassColumn = 0;
            GridClassLabelRow = 6;
            GridClassPickerRow = 7;
            GridClassErrorRow = 8;

            GridPeriodColumn = 2;
            GridPeriodLabelRow = 6;
            GridPeriodPickerRow = 7;
            GridPeriodErrorRow = 8;

            GridCourseColumn = 2;
            GridCourseLabelRow = 3;
            GridCoursePickerRow = 4;
            GridCourseErrorRow = 5;
        }
        else
        {
            GridClassColumn = 2;
            GridClassLabelRow = 3;
            GridClassPickerRow = 4;
            GridClassErrorRow = 5;

            GridPeriodColumn = 0;
            GridPeriodLabelRow = 6;
            GridPeriodPickerRow = 7;
            GridPeriodErrorRow = 8;

            GridCourseColumn = 2;
            GridCourseLabelRow = 6;
            GridCoursePickerRow = 7;
            GridCourseErrorRow = 8;
        }
    }

    #endregion
}