using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Calendar;
using iCampus.Portal.EditModels;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class WeeklyPlanSearchForm : ViewModelBase
{
    #region Declaration
    public ICommand ShowAllWeeksCommand { get; set; }
    public ICommand SearchClickCommand { get; set; }
    public ICommand WeeklyExpandCollapseClickCommand { get; set; }
    public ICommand GroupedAgendaDataCollapseClickCommand { get; set; }
    public ICommand ExportToPDFClickCommand { get; set; }
    
    #endregion

    #region Properties
    private AgendaWeeklyGroupViewModel _agendaWeeklyGroupData ;

    public AgendaWeeklyGroupViewModel AgendaWeeklyGroupData
    {
        get => _agendaWeeklyGroupData;
        set
        {
            _agendaWeeklyGroupData = value;
            OnPropertyChanged(nameof(AgendaWeeklyGroupData));
        }
    }
    
    private AgendaWeeklyGroupWithAgenda _agendaWeeklySearchData = new ();

    public AgendaWeeklyGroupWithAgenda AgendaWeeklySearchData
    {
        get => _agendaWeeklySearchData;
        set
        {
            _agendaWeeklySearchData = value;
            OnPropertyChanged(nameof(AgendaWeeklySearchData));
        }
    }
    private IList<DateRangePickListItem> _dateRangeList;

    public IList<DateRangePickListItem> DateRangeList
    {
        get => _dateRangeList;
        set
        {
            _dateRangeList = value;
            OnPropertyChanged(nameof(DateRangeList));
        }
    }
    private DateRangePickListItem _selectedDateRangeList;

    public DateRangePickListItem SelectedDateRangeList
    {
        get => _selectedDateRangeList;
        set
        {
            _selectedDateRangeList = value;
            OnPropertyChanged(nameof(SelectedDateRangeList));
        }
    }
    private IList<CurriculumView> _courseList;

    public IList<CurriculumView> CourseList
    {
        get => _courseList;
        set
        {
            _courseList = value;
            OnPropertyChanged(nameof(CourseList));
        }
    }
    
    private IList<CurriculumView> _allCourseList;

    public IList<CurriculumView> AllCourseList
    {
        get => _allCourseList;
        set
        {
            _allCourseList = value;
            OnPropertyChanged(nameof(AllCourseList));
        }
    }
    private CurriculumView _selectedCourseList;

    public CurriculumView SelectedCourseList
    {
        get => _selectedCourseList;
        set
        {
            _selectedCourseList = value;
            OnPropertyChanged(nameof(SelectedCourseList));
            GetClassesMethod();
        }
    }
    private IList<PickListItem> _gradeList;
    public IList<PickListItem> GradeList
    {
        get => _gradeList;
        set
        {
            _gradeList = value;
            OnPropertyChanged(nameof(GradeList));
        }
    }
    private PickListItem _selectedGradeList;

    public PickListItem SelectedGradeList
    {
        get => _selectedGradeList;
        set
        {
            _selectedGradeList = value;
            OnPropertyChanged(nameof(SelectedGradeList));
            FilterCourseList();
        }
    }
    private IList<PickListItem> _classList;

    public IList<PickListItem> ClassList
    {
        get => _classList;
        set
        {
            _classList = value;
            OnPropertyChanged(nameof(ClassList));
        }
    }
    private PickListItem _selectedClassList;

    public PickListItem SelectedClassList
    {
        get => _selectedClassList;
        set
        {
            _selectedClassList = value;
            OnPropertyChanged(nameof(SelectedClassList));
        }
    }
    private ObservableCollection<GroupingWeeklyPlan<string, BindableAgendaWeeklyGroupView>> _agendaWeeklyGroupList 
        = new();

    public ObservableCollection<GroupingWeeklyPlan<string, BindableAgendaWeeklyGroupView>> AgendaWeeklyGroupList
    {
        get => _agendaWeeklyGroupList;
        set
        {
            _agendaWeeklyGroupList = value;
            OnPropertyChanged(nameof(AgendaWeeklyGroupList));
        }
    }

    private BindableAgendaWeeklyGroupView _selectedAgendaWeeklyGroupList;
    public BindableAgendaWeeklyGroupView SelectedAgendaWeeklyGroupList
    {
        get => _selectedAgendaWeeklyGroupList;
        set
        {
            _selectedAgendaWeeklyGroupList = value;
            OnPropertyChanged(nameof(SelectedAgendaWeeklyGroupList));
        }
    }
    private ObservableCollection<BindableAgendaView> _groupedAgendaData = new ();
    public ObservableCollection<BindableAgendaView> GroupedAgendaData
    {
        get => _groupedAgendaData;
        set
        {
            _groupedAgendaData = value;
            OnPropertyChanged(nameof(GroupedAgendaData));
        }
    }
    private BindableAgendaView _selectedGroupedAgendaData;
    public BindableAgendaView SelectedGroupedAgendaData
    {
        get => _selectedGroupedAgendaData;
        set
        {
            _selectedGroupedAgendaData = value;
            OnPropertyChanged(nameof(SelectedGroupedAgendaData));
        }
    }
    private TeacherCalendarDisplaySetting _calendarControlSetting;
    public TeacherCalendarDisplaySetting CalendarControlSetting
    {
        get => _calendarControlSetting;
        set
        {
            _calendarControlSetting = value;
            OnPropertyChanged(nameof(CalendarControlSetting));
        }
    }
    private CalendarApprovalPermissionView _calendarApprovalPermissions;
    public CalendarApprovalPermissionView CalendarApprovalPermissions
    {
        get => _calendarApprovalPermissions;
        set
        {
            _calendarApprovalPermissions = value;
            OnPropertyChanged(nameof(CalendarApprovalPermissions));
        }
    }
     
    private IList<AgendaView> _attachmentList;
    public IList<AgendaView> AttachmentList
    {
        get => _attachmentList;
        set
        {
            _attachmentList = value;
            OnPropertyChanged(nameof(AttachmentList));
        }
    }
    
    bool _isAllWeekSelected;
    public bool IsAllWeekSelected
    {
        get => _isAllWeekSelected;
        set
        {
            _isAllWeekSelected = value;
            OnPropertyChanged(nameof(IsAllWeekSelected));
        }
    }
    bool _isGradeListVisible;
    public bool IsGradeListVisible
    {
        get => _isGradeListVisible;
        set
        {
            _isGradeListVisible = value;
            OnPropertyChanged(nameof(IsGradeListVisible));
        }
    }
    bool _isClassListVisible;
    public bool IsClassListVisible
    {
        get => _isClassListVisible;
        set
        {
            _isClassListVisible = value;
            OnPropertyChanged(nameof(IsClassListVisible));
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
    private bool _isExportToPDFVisisble= false;

    public bool IsExportToPDFVisisble
    {
        get => _isExportToPDFVisisble;
        set
        {
            _isExportToPDFVisisble = value;
            OnPropertyChanged(nameof(IsExportToPDFVisisble));
        }
    }
    private int _agendaWeeklyGroupListHeight;

    public int AgendaWeeklyGroupListHeight
    {
        get => _agendaWeeklyGroupListHeight;
        set
        {
            _agendaWeeklyGroupListHeight = value;
            OnPropertyChanged(nameof(AgendaWeeklyGroupListHeight));
        }
    }
    #endregion
    
    public WeeklyPlanSearchForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods
    private async void InitializePage()
    {
        ShowAllWeeksCommand = new Command(ShowAllWeeksCommandMethod);
        SearchClickCommand = new Command(SearchClickCommandMethod);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        WeeklyExpandCollapseClickCommand = new Command<BindableAgendaWeeklyGroupView>(WeeklyExpandCollapseClicked);
        GroupedAgendaDataCollapseClickCommand = new Command<BindableAgendaView>(GroupedAgendaDataCollapseClickMethod);
        ExportToPDFClickCommand = new Command(ExportToPDFClickCommandMethod);
        
        GetWeeklyPlansDetails();

    }
    
    private async void GetWeeklyPlansDetails()
    {
        try
        {
            var studentId = AppSettings.Current.IsTeacher ? null : AppSettings.Current.SelectedStudent.ItemId;
            IsClassListVisible = IsGradeListVisible = AppSettings.Current.IsTeacher;
            AgendaWeeklyGroupData =
                await ApiHelper.GetObject<AgendaWeeklyGroupViewModel>(
                    string.Format(TextResource.InitAgendaWeeklyPlanSearchApi, studentId, null));
            
            DateRangeList = AgendaWeeklyGroupData.DateRangeList
                .Where(x => x.IsCurrentOrFutureWeek)
                .ToList();
            AllCourseList = AgendaWeeklyGroupData.CourseList;
                
            CourseList = AllCourseList.GroupBy(c => new { c.CurriculumId, c.CurriculumName })
                .Select(g => g.First())
                .ToList();;
            GradeList = AgendaWeeklyGroupData.Grades;
            if (DateRangeList != null && DateRangeList.Any())
            {
                SelectedDateRangeList = DateRangeList.First();
            }
            GradeList = AgendaWeeklyGroupData.Grades;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }
    
    private void FilterCourseList()
    {
        if (_selectedGradeList != null && int.TryParse(_selectedGradeList.ItemId, out int selectedId))
        {
            CourseList = AllCourseList
                .Where(c => c.GradeId == selectedId)
                .GroupBy(c => new { c.CurriculumId, c.CurriculumName })
                .Select(g => g.First())
                .ToList();
        }
        else
        {
            CourseList = AllCourseList.ToList();
        }
    }
    private async void GetClassesMethod()
    {
        try
        {
            var gradeId = SelectedGradeList.ItemId; 
            var curriculumId = SelectedCourseList.CurriculumId;
            ClassList = await ApiHelper.GetObject<List<PickListItem>>(
                string.Format(TextResource.getclassesforresourceApi, gradeId, curriculumId));

        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }
    void ShowAllWeeksCommandMethod()
    {
        try
        {
            DateRangeList = (IsAllWeekSelected
                    ? AgendaWeeklyGroupData.DateRangeList
                    : AgendaWeeklyGroupData.DateRangeList
                        .Where(x => x.IsCurrentOrFutureWeek))
                .ToList();

            if (DateRangeList.Any())
                SelectedDateRangeList = DateRangeList.First();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void SearchClickCommandMethod()
    {
        try
        {
            var studentId = AppSettings.Current.IsTeacher ? null : AppSettings.Current.SelectedStudent.ItemId;
            var weekStartDate = SelectedDateRangeList.ItemId;
            int? courseId = SelectedCourseList == null || string.IsNullOrEmpty(SelectedCourseList.CurriculumName) 
                ? null 
                : SelectedCourseList.CurriculumId;
            string gradeId = AppSettings.Current.IsTeacher
                ? (!string.IsNullOrEmpty(SelectedGradeList?.ItemId) 
                    ? SelectedGradeList.ItemId 
                    : null)
                : null;
            
            string classId = AppSettings.Current.IsTeacher
                ? (!string.IsNullOrEmpty(SelectedClassList?.ItemId) 
                    ? SelectedClassList.ItemId 
                    : null)
                : null;
            AgendaWeeklySearchData =
                await ApiHelper.GetObject<AgendaWeeklyGroupWithAgenda>(
                    string.Format(TextResource.SearchAgendaWeeklyPlanListApi, studentId, weekStartDate, gradeId, classId, courseId, null, false));

            //AgendaWeeklyGroupList = new ObservableCollection<BindableAgendaWeeklyGroupView>(_mapper.Map<List<BindableAgendaWeeklyGroupView>>(
                //AgendaWeeklySearchData.AgendaWeeklyGroupList));
                
            var mappedList = _mapper.Map<List<BindableAgendaWeeklyGroupView>>(
                    AgendaWeeklySearchData.AgendaWeeklyGroupList);

            var grouped = mappedList
                    .GroupBy(x => $"Weekly Plan ({x.GradeName})")
                    .Select(g => new GroupingWeeklyPlan<string, BindableAgendaWeeklyGroupView>(g.Key, g))
                    .ToList();

            AgendaWeeklyGroupList = new ObservableCollection<GroupingWeeklyPlan<string, BindableAgendaWeeklyGroupView>>(grouped);
            AgendaWeeklyGroupListHeight = AgendaWeeklyGroupList.Count * 100;
            GroupedAgendaData = new ObservableCollection<BindableAgendaView>(
                _mapper.Map<List<BindableAgendaView>>(
                    AgendaWeeklySearchData.GroupedAgendaData
                        .OrderBy(x => x.DueDate) 
                        .ToList()
                )
            );
            CalendarControlSetting = AgendaWeeklySearchData.CalendarControlSetting;
            CalendarApprovalPermissions = AgendaWeeklySearchData.CalendarApprovalPermissions;
            AttachmentList = _mapper.Map<List<AgendaView>>(AgendaWeeklySearchData.AgendaAttachmentList);
            
            foreach (var item in GroupedAgendaData)
            {
                if (DateTime.TryParse(item.DueDate, out var parsedDate))
                {
                    item.DueDate = parsedDate.ToString("dddd, MMMM dd, yyyy");
                }
            }
            IsExportToPDFVisisble = AgendaWeeklyGroupList.Any();
            IsWeeklyNoRecordMsg = !AgendaWeeklyGroupList.Any();
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
                AgendaWeeklyGroupListHeight = AgendaWeeklyGroupList.Count * 100;
                foreach (var group in AgendaWeeklyGroupList)
                {
                    foreach (var item in group)
                    {
                        if (item.AgendaWeeklyGroupId == bindableAgendaWeekly.AgendaWeeklyGroupId)
                        {
                            item.WeeklyPostDetailsVisibility = !item.WeeklyPostDetailsVisibility;
                            item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png")
                                ? "dropdown_gray.png"
                                : "uparrow_gray.png";

                            if (item.WeeklyPostDetailsVisibility)
                            {
                                AgendaWeeklyGroupListHeight += 220;
                            }
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
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void GroupedAgendaDataCollapseClickMethod(BindableAgendaView bindableAgendaView)
    {
        try
        {
            WeeklyPlanSearchDetailForm weeklyPlanSearchDetailForm = new(_mapper, _nativeServices, Navigation)
            {
                PageTitle = PageTitle,
                SelectedGroupedAgendaData = bindableAgendaView,
                CalendarControlSetting = CalendarControlSetting,
                BackVisible = true
            };
            weeklyPlanSearchDetailForm.AttachmentList = AttachmentList
                .Where(x => x.AgendaId == bindableAgendaView.AgendaId)
                .ToList();
            weeklyPlanSearchDetailForm.AttachmentListViewHeight = AttachmentList.Count * 30;
            weeklyPlanSearchDetailForm.IsAttachmentVisible = weeklyPlanSearchDetailForm.AttachmentList.Any();
            var weeklyPlanSearchDetail = new WeeklyPlanSearchDetailPage()
            {
                BindingContext = weeklyPlanSearchDetailForm
            };
            await Navigation.PushAsync(weeklyPlanSearchDetail);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }
    private async void ExportToPDFClickCommandMethod()
    {
        try
        {
            string GetItemId(PickListItem item) => !string.IsNullOrEmpty(item?.ItemId) ? item.ItemId : null;

            var studentId = AppSettings.Current.IsTeacher ? null : AppSettings.Current.SelectedStudent.ItemId;
            var weekStartDate = SelectedDateRangeList.ItemId;
            var gradeId = AppSettings.Current.IsTeacher ? GetItemId(SelectedGradeList) : null;
            var classId = AppSettings.Current.IsTeacher ? GetItemId(SelectedClassList) : null;
            var courseId = (string)null; 
            var weeklyGroupId = (string)null; 
            var sid = AppSettings.Current.UserSessionUid;

            var exportToPDFURL =
                $"{AppSettings.Current.PortalUrl}/calendarquickpost/ExportAgendaWeeklyPlanList?" +
                $"studentId={studentId}&weekStartDate={weekStartDate}&gradeId={gradeId}" +
                $"&classId={classId}&courseId={courseId}&weeklyGroupId={weeklyGroupId}&sid={sid}";
            
            if (!string.IsNullOrEmpty(exportToPDFURL))
                await HelperMethods.OpenFileForPreview(exportToPDFURL, _nativeServices);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public override async void GetStudentData()
    {
        base.GetStudentData();
        IsAllWeekSelected = false;
        IsWeeklyNoRecordMsg = false;
        IsExportToPDFVisisble = false;
        DateRangeList = new List<DateRangePickListItem>();
        CourseList = new List<CurriculumView>();
        AgendaWeeklyGroupList = new ObservableCollection<GroupingWeeklyPlan<string, BindableAgendaWeeklyGroupView>>();
        GroupedAgendaData = new ObservableCollection<BindableAgendaView>();
        GetWeeklyPlansDetails();
    }
    #endregion
}