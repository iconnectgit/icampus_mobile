using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.Portal.EditModels;
using iCampus.Portal.ViewModels;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class QuickPostForm : ViewModelBase
	{
        #region Declaration
        public ICommand SaveEditorCommand { get; set; }
        private Popup _currentPopup;
        private string _selectedHtmlContentKey;
        private bool isHandlingCheck = false;
        private BindableQuickPost _currentQuickPost;
        public ICommand NewPostRadioButtonCommand { get; set; }
        public ICommand EditPostRadioButtonCommand { get; set; }
        public ICommand RemarkEditCommand { get; set; }
        public ICommand CurriculamStandardsEditCommand { get; set; }
        public ICommand LearningOutcomesEditCommand { get; set; }
        public ICommand AssignmentEditedCommand { get; set; }
        public ICommand SearchCourseCommand { get; set; }
        public ICommand CourseSelectionCommand { get; set; }
        public ICommand AgendaForChangedCommand { get; set; }
        public ICommand DateRangeSelectionCommand { get; set; }
        public ICommand CourseListTappedCommand { get; set; }
        public ICommand DateRangeListTappedCommand { get; set; }
        public ICommand DateRangeChangedCommand { get; set; }
        public ICommand ClassesSelectionCommand { get; set; }
        public ICommand ClassSelectionCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand AttachmentCommand { get; set; }
        public ICommand DeleteAttachmentClickCommand { get; set; }
        public ICommand MonthSelectionCommand { get; set; }
        public ICommand MonthChangedCommand { get; set; }
        public ICommand GroupChangedCommand { get; set; }
        public ICommand GroupSelectionCommand { get; set; }
        public ICommand MonthListTappedCommand { get; set; }
        public ICommand GroupListTappedCommand { get; set; }
        public ICommand CancelCommand { get; set; }


        #endregion
        #region Properties
        private string _newPostRadioButtonImage;
        public string NewPostRadioButtonImage
        {
            get => _newPostRadioButtonImage;
            set
            {
                _newPostRadioButtonImage = value;
                OnPropertyChanged(nameof(NewPostRadioButtonImage));
            }
        }
        private string _editPostRadioButtonImage;
        public string EditPostRadioButtonImage
        {
            get => _editPostRadioButtonImage;
            set
            {
                _editPostRadioButtonImage = value;
                OnPropertyChanged(nameof(EditPostRadioButtonImage));
            }
        }
        bool _isNavigationBarVisible;
        public bool IsNavigationBarVisible
        {
            get => _isNavigationBarVisible;
            set
            {
                _isNavigationBarVisible = value;
                OnPropertyChanged(nameof(IsNavigationBarVisible));
            }
        }
        string _remarkText;
        public string RemarkText
        {
            get => _remarkText;
            set
            {
                _remarkText =
                    "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>"
                    + value
                    + "</body></html>";
                OnPropertyChanged(nameof(RemarkText));
            }
        }
        bool _isNewPostVisible = true;
        public bool IsNewPostVisible
        {
            get => _isNewPostVisible;
            set
            {
                _isNewPostVisible = value;
                OnPropertyChanged(nameof(IsNewPostVisible));
            }
        }
        bool _isEditPostVisible;
        public bool IsEditPostVisible
        {
            get => _isEditPostVisible;
            set
            {
                _isEditPostVisible = value;
                OnPropertyChanged(nameof(IsEditPostVisible));
            }
        }
        bool _isCancelButtonVisibleForTeacher;
        public bool IsCancelButtonVisibleForTeacher
        {
            get => _isCancelButtonVisibleForTeacher;
            set
            {
                _isCancelButtonVisibleForTeacher = value;
                OnPropertyChanged(nameof(IsCancelButtonVisibleForTeacher));
            }
        }
        IList<AttachmentFileView> _attachmentFiles = new ObservableCollection<AttachmentFileView>();
        public IList<AttachmentFileView> AttachmentFiles
        {
            get => _attachmentFiles;
            set
            {
                _attachmentFiles = value;
                OnPropertyChanged(nameof(AttachmentFiles));
            }
        }
        string _curriculamStandardsText;
        public string CurriculamStandardsText
        {
            get
            {
                return _curriculamStandardsText;
            }
            set
            {
                _curriculamStandardsText =
                    "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>"
                    + value
                    + "</body></html>";
                OnPropertyChanged(nameof(CurriculamStandardsText));
            }
        }
        string _learningOutcomesText;
        public string LearningOutcomesText
        {
            get
            {
                return _learningOutcomesText;
            }
            set
            {
                _learningOutcomesText = value;
                OnPropertyChanged(nameof(LearningOutcomesText));
            }
        }
        string _assignmentEditedText;
        public string AssignmentEditedText
        {
            get
            {
                if (string.IsNullOrEmpty(_assignmentEditedText))
                {
                    _assignmentEditedText = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" + _assignmentEditedText + "</body></html>";
                }
                return _assignmentEditedText;
            }
            set
            {
                _assignmentEditedText = value;
                OnPropertyChanged(nameof(AssignmentEditedText));
            }
        }
        int _attachmentListViewHeight = 0;
        public int AttachmentListViewHeight
        {
            get => _attachmentListViewHeight;
            set
            {
                _attachmentListViewHeight = value;
                OnPropertyChanged(nameof(AttachmentListViewHeight));
            }
        }
        AgendaWeeklyGroupEdit _quickPostData = new AgendaWeeklyGroupEdit();
        public AgendaWeeklyGroupEdit QuickPostData
        {
            get => _quickPostData;
            set
            {
                _quickPostData = value;
                OnPropertyChanged(nameof(QuickPostData));
            }
        }
        IList<PickListItem> _filteredCourseList = new List<PickListItem>();
        public IList<PickListItem> FilteredCourseList
        {
            get
            {
                return _filteredCourseList;
            }
            set
            {
                _filteredCourseList = value;
                OnPropertyChanged(nameof(FilteredCourseList));
                NoDataExist = FilteredCourseList != null && FilteredCourseList.Count > 0 ? false : true;
            }
        }
        bool _noDataExist = false;
        public bool NoDataExist
        {
            get => _noDataExist;
            set
            {
                _noDataExist = value;
                OnPropertyChanged(nameof(NoDataExist));
            }
        }
        bool _noDataExistforDateRange = false;
        public bool NoDataExistDateRange
        {
            get => _noDataExistforDateRange;
            set
            {
                _noDataExistforDateRange = value;
                OnPropertyChanged(nameof(NoDataExistDateRange));
            }
        }
        IList<PickListItem> _monthList = new List<PickListItem>();
        public IList<PickListItem> MonthList
        {
            get
            {
                return _monthList;
            }
            set
            {
                _monthList = value;
                OnPropertyChanged(nameof(MonthList));
            }
        }
        IList<PickListItem> _groupList = new List<PickListItem>();
        public IList<PickListItem> GroupList
        {
            get
            {
                return _groupList;
            }
            set
            {
                _groupList = value;
                OnPropertyChanged(nameof(GroupList));
            }
        }
        IList<PickListItem> _courseList = new List<PickListItem>();
        public IList<PickListItem> CourseList
        {
            get
            {
                return _courseList;
            }
            set
            {
                _courseList = value;
                OnPropertyChanged(nameof(CourseList));
            }
        }
        string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        bool _courseListVisibility = false;
        public bool CourseListVisibility
        {
            get => _courseListVisibility;
            set
            {
                _courseListVisibility = value;
                OnPropertyChanged(nameof(CourseListVisibility));
            }
        }
        bool _monthListVisibility = false;
        public bool MonthListVisibility
        {
            get => _monthListVisibility;
            set
            {
                _monthListVisibility = value;
                OnPropertyChanged(nameof(MonthListVisibility));
            }
        }
        bool _groupListVisibility = false;
        public bool GroupListVisibility
        {
            get => _groupListVisibility;
            set
            {
                _groupListVisibility = value;
                OnPropertyChanged(nameof(GroupListVisibility));
            }
        }
        
        private int _courseListViewHeight;
        public int CourseListViewHeight
        {
            get => _courseListViewHeight;
            set
            {
                _courseListViewHeight = value;
                OnPropertyChanged(nameof(CourseListViewHeight));
            }
        }
        private int _monthListViewHeight;
        public int MonthListViewHeight
        {
            get => _monthListViewHeight;
            set
            {
                _monthListViewHeight = value;
                OnPropertyChanged(nameof(MonthListViewHeight));
            }
        }
        private int _groupListViewHeight;
        public int GroupListViewHeight
        {
            get => _groupListViewHeight;
            set
            {
                _groupListViewHeight = value;
                OnPropertyChanged(nameof(GroupListViewHeight));
            }
        }
        bool _isClassesSelected;
        public bool IsClassesSelected
        {
            get => _isClassesSelected;
            set
            {
                _isClassesSelected = value;
                OnPropertyChanged(nameof(IsClassesSelected));
            }
        }
        PickListItem _selectedCourse = new PickListItem();
        public PickListItem SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                OnPropertyChanged(nameof(SelectedCourse));                
                if (_selectedCourse != null && !string.IsNullOrEmpty(_selectedCourse.ItemName))
                {
                    if (!IsEditMode)
                    {
                        //GetClassesInAddMode();
                    }
                }
            }
        }
        PickListItem _selectedMonthList = new PickListItem();
        public PickListItem SelectedMonthList
        {
            get => _selectedMonthList;
            set
            {
                _selectedMonthList = value;
                OnPropertyChanged(nameof(SelectedMonthList));
            }
        }
        PickListItem _selectedGroupList = new PickListItem();
        public PickListItem SelectedGroupList
        {
            get => _selectedGroupList;
            set
            {
                _selectedGroupList = value;
                OnPropertyChanged(nameof(SelectedGroupList));
            }
        }
        bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
            }
        }
        bool _isEnableLearningOutcomes;
        public bool IsEnableLearningOutcomes
        {
            get => _isEnableLearningOutcomes;
            set
            {
                _isEnableLearningOutcomes = value;
                OnPropertyChanged(nameof(IsEnableLearningOutcomes));
            }
        }
        string _selectedAgendaForText;
        public string SelectedAgendaForText
        {
            get => _selectedAgendaForText;
            set
            {
                _selectedAgendaForText = value;
                OnPropertyChanged(nameof(SelectedAgendaForText));
            }
        }
        string _monthSelectedText;
        public string MonthSelectedText
        {
            get => _monthSelectedText;
            set
            {
                _monthSelectedText = value;
                OnPropertyChanged(nameof(MonthSelectedText));
            }
        }
        string _groupSelectedText;
        public string GroupSelectedText
        {
            get => _groupSelectedText;
            set
            {
                _groupSelectedText = value;
                OnPropertyChanged(nameof(GroupSelectedText));
            }
        }
        bool _isAgendaForErrorVisible = false;
        public bool IsAgendaForErrorVisible
        {
            get => _isAgendaForErrorVisible;
            set
            {
                _isAgendaForErrorVisible = value;
                OnPropertyChanged(nameof(IsAgendaForErrorVisible));
            }
        }
        IList<PickListItem> _dateRangeList = new List<PickListItem>();
        public IList<PickListItem> DateRangeList
        {
            get
            {
                return _dateRangeList;
            }
            set
            {
                _dateRangeList = value;
                OnPropertyChanged(nameof(DateRangeList));
                NoDataExistDateRange = DateRangeList != null && DateRangeList.Count > 0 ? false : true;
            }
        }
        IList<AgendaWeeklyGroupView> _editAgendaWeeklyData = new List<AgendaWeeklyGroupView>();
        public IList<AgendaWeeklyGroupView> EditAgendaWeeklyData
        {
            get
            {
                return _editAgendaWeeklyData;
            }
            set
            {
                _editAgendaWeeklyData = value;
                OnPropertyChanged(nameof(EditAgendaWeeklyData));
            }
        }
        string _selectedDateRangeText;
        public string SelectedDateRangeText
        {
            get => _selectedDateRangeText;
            set
            {
                _selectedDateRangeText = value;
                OnPropertyChanged(nameof(SelectedDateRangeText));
            }
        }
        bool _dateRangeListVisibility = false;
        public bool DateRangeListVisibility
        {
            get => _dateRangeListVisibility;
            set
            {
                _dateRangeListVisibility = value;
                OnPropertyChanged(nameof(DateRangeListVisibility));
            }
        }
        PickListItem _selectedDateRange = new PickListItem();
        public PickListItem SelectedDateRange
        {
            get => _selectedDateRange;
            set
            {
                _selectedDateRange = value;
                OnPropertyChanged(nameof(SelectedDateRange));
                if (_selectedDateRange != null && !string.IsNullOrEmpty(_selectedDateRange.ItemName))
                {
                    if (!IsEditMode)
                    {
                        //GetClassesInAddMode();
                    }
                }
            }
        }
        private int _dateRangeListViewHeight;
        public int DateRangeListViewHeight
        {
            get => _dateRangeListViewHeight;
            set
            {
                _dateRangeListViewHeight = value;
                OnPropertyChanged(nameof(DateRangeListViewHeight));
            }
        }
        AgendaClassModel _agendaClassData = new AgendaClassModel();
        public AgendaClassModel AgendaClassData
        {
            get => _agendaClassData;
            set
            {
                _agendaClassData = value;
                OnPropertyChanged(nameof(AgendaClassData));
            }
        }
        AgendaWeeklyGroupEdit _agendaWeeklyData = new AgendaWeeklyGroupEdit();
        public AgendaWeeklyGroupEdit AgendaWeeklyData
        {
            get
            {
                return _agendaWeeklyData;
            }
            set
            {
                _agendaWeeklyData = value;
                OnPropertyChanged(nameof(AgendaWeeklyData));
            }
        }
        string _selectedAgendaWeeklyData;
        public string SelectedAgendaWeeklyData
        {
            get => _selectedAgendaWeeklyData;
            set
            {
                _selectedAgendaWeeklyData = value;
                OnPropertyChanged(nameof(SelectedAgendaWeeklyData));
            }
        }
        bool _classesVisibility;
        public bool ClassesVisibility
        {
            get => _classesVisibility;
            set
            {
                _classesVisibility = value;
                OnPropertyChanged(nameof(ClassesVisibility));
            }
        }
        ObservableCollection<BindableAgendaClassView> _classList = new ObservableCollection<BindableAgendaClassView>();
        public ObservableCollection<BindableAgendaClassView> ClassList
        {
            get
            {
                return _classList;
            }
            set
            {
                _classList = value;
                OnPropertyChanged(nameof(ClassList));
            }
        }
        private int _classListViewHeight;
        public int ClassListViewHeight
        {
            get => _classListViewHeight;
            set
            {
                _classListViewHeight = value;
                OnPropertyChanged(nameof(ClassListViewHeight));
            }
        }
        Thickness _flowListViewMargin;
        public Thickness FlowListViewMargin
        {
            get
            {
                _flowListViewMargin = Device.RuntimePlatform == Device.iOS ? (_nativeServices.SystemVersionCheck() ? new Thickness(10, -15, 0, 0) : new Thickness(10, 0, 10, 0)) : new Thickness(10, 0, 10, 0);
                return _flowListViewMargin;
            }
            set
            {
                _flowListViewMargin = value;
                OnPropertyChanged(nameof(FlowListViewMargin));
            }
        }
        bool _classSelectionErrorVisibility = false;
        public bool ClassSelectionErrorVisibility
        {
            get => _classSelectionErrorVisibility;
            set
            {
                _classSelectionErrorVisibility = value;
                OnPropertyChanged(nameof(ClassSelectionErrorVisibility));
            }
        }
        bool _isTitleVisible = false;
        public bool IsTitleVisible
        {
            get => _isTitleVisible;
            set
            {
                _isTitleVisible = value;
                OnPropertyChanged(nameof(IsTitleVisible));
            }
        }
        bool _isDateRangeVisible = false;
        public bool IsDateRangeVisible
        {
            get => _isDateRangeVisible;
            set
            {
                _isDateRangeVisible = value;
                OnPropertyChanged(nameof(IsDateRangeVisible));
            }
        }
        string _groupTitle;
        public string GroupTitle
        {
            get => _groupTitle;
            set
            {
                _groupTitle = value;
                OnPropertyChanged(nameof(GroupTitle));
            }
        }
        string _dateRangeText;
        public string DateRangeText
        {
            get => _dateRangeText;
            set
            {
                _dateRangeText = value;
                OnPropertyChanged(nameof(DateRangeText));
            }
        }
        string _weekStartDate;
        public string WeekStartDate
        {
            get => _weekStartDate;
            set
            {
                _weekStartDate = value;
                OnPropertyChanged(nameof(WeekStartDate));
            }
        }
        string _weekEndDate;
        public string WeekEndDate
        {
            get => _weekEndDate;
            set
            {
                _weekEndDate = value;
                OnPropertyChanged(nameof(WeekEndDate));
            }
        }
        
        int _agendaWeeklyGroupId;
        public int AgendaWeeklyGroupId
        {
            get => _agendaWeeklyGroupId;
            set
            {
                _agendaWeeklyGroupId = value;
                OnPropertyChanged(nameof(AgendaWeeklyGroupId));
            }
        }
        ObservableCollection<BindableQuickPost> _dateWiseQuickPost = new ObservableCollection<BindableQuickPost>();
        public ObservableCollection<BindableQuickPost> DateWiseQuickPost
        {
            get => _dateWiseQuickPost;
            set
            {
                _dateWiseQuickPost = value;
                OnPropertyChanged(nameof(DateWiseQuickPost));
            }
        }
        bool _curriculumStandardsVisibility = false;
        public bool CurriculumStandardsVisibility
        {
            get => _curriculumStandardsVisibility;
            set
            {
                _curriculumStandardsVisibility = value;
                OnPropertyChanged(nameof(CurriculumStandardsVisibility));
            }
        }
        bool _remarkVisibility = false;
        public bool RemarkVisibility
        {
            get => _remarkVisibility;
            set
            {
                _remarkVisibility = value;
                OnPropertyChanged(nameof(RemarkVisibility));
            }
        }
        bool _saveButtonVisibility = false;
        public bool SaveButtonVisibility
        {
            get => _saveButtonVisibility;
            set
            {
                _saveButtonVisibility = value;
                OnPropertyChanged(nameof(SaveButtonVisibility));
            }
        }
        private int _typeId;
        public int TypeId
        {
            get => _typeId;
            set
            {
                _typeId = value;
                OnPropertyChanged(nameof(TypeId));
            }
        }
        IList<AgendaView> _groupedAgendaAttachmentDataString = new List<AgendaView>();
        public IList<AgendaView> GroupedAgendaAttachmentDataString
        {
            get
            {
                return _groupedAgendaAttachmentDataString;
            }
            set
            {
                _groupedAgendaAttachmentDataString = value;
                OnPropertyChanged(nameof(GroupedAgendaAttachmentDataString));
            }
        }
        bool _isFromEditButton = false;
        public bool IsFromEditButton
        {
            get => _isFromEditButton;
            set
            {
                _isFromEditButton = value;
                OnPropertyChanged(nameof(IsFromEditButton));
            }
        }
        private string _editedHtmlContent;
        public string EditedHtmlContent
        {
            get => _editedHtmlContent;
            set
            {
                _editedHtmlContent = value;
                OnPropertyChanged(nameof(EditedHtmlContent));
            }
        }
        private string _htmlEditorTitle;
        public string HtmlEditorTitle
        {
            get => _htmlEditorTitle;
            set
            {
                _htmlEditorTitle = value;
                OnPropertyChanged(nameof(HtmlEditorTitle));
            }
        }
        #endregion
        public QuickPostForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        #region Methods
        private async void InitializePage()
        {
            NewPostRadioButtonImage = "selected_radio_button.png";
            EditPostRadioButtonImage = "unselected_radio_button.png";
            NewPostRadioButtonCommand = new Command(NewPostRadioButtonMethod);
            EditPostRadioButtonCommand = new Command(EditPostRadioButtonMethod);
            RemarkEditCommand = new Command(RemarkEditMethod);
            CurriculamStandardsEditCommand = new Command(CurriculamStandardsEditMethod);
            LearningOutcomesEditCommand = new Command<BindableQuickPost>(LearningOutcomesEditMethod);
            AssignmentEditedCommand = new Command<BindableQuickPost>(AssignmentEditedMethod);
            SearchCourseCommand = new Command(SearchCourse);
            CourseSelectionCommand = new Command(CourseSelectionMethod);
            AgendaForChangedCommand = new Command<PickListItem>(AgendaForChanged);
            DateRangeSelectionCommand = new Command(DateRangeSelectionMethod);
            CourseListTappedCommand = new Command<PickListItem>(CourseListTappedMethod);
            DateRangeListTappedCommand = new Command<PickListItem>(DateRangeListTappedMethod);
            DateRangeChangedCommand = new Command<PickListItem>(DateRangeChangedMethod);
            ClassesSelectionCommand = new Command(ClassesSelectionMethod);
            ClassSelectionCommand = new Command<BindableAgendaClassView>(ClassSelectionMethod);
            SaveCommand = new Command(SaveClickedMethod);
            AttachmentCommand = new Command<BindableQuickPost>(AttachmentMethod);
            DeleteAttachmentClickCommand = new Command(DeleteAttachmentClicked);
            MonthSelectionCommand = new Command(MonthSelectionMethod);
            MonthChangedCommand = new Command<PickListItem>(MonthChangedMethod);
            GroupChangedCommand = new Command<PickListItem>(GroupChangedMethod);
            GroupSelectionCommand = new Command(GroupSelectionMethod);
            MonthListTappedCommand = new Command<PickListItem>(MonthListTappedMethod);
            GroupListTappedCommand = new Command<PickListItem>(GroupListTappedMethod);
            CancelCommand = new Command(CancelWeeklyAgendaMethod);
            SaveEditorCommand = new Command(SaveEditorClickedMethod);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);

            
            MessagingCenter.Subscribe<string>("", "AllClassSelection", async (args) =>
            {
                if (args != null)
                {
                    if (args.ToString().ToLower().Equals("true"))
                        ClassList.ToList().ForEach(x => x.IsChecked = true);
                    else
                        ClassList.ToList().ForEach(x => x.IsChecked = false);
                }
            });

        }

        public async Task GetQuickPostData()
        {
            try
            {
                QuickPostData = await ApiHelper.GetObject<AgendaWeeklyGroupEdit>(string.Format(TextResource.AddNewAgendaQuickPostDataApi));
                IsCancelButtonVisibleForTeacher = IsEditMode = QuickPostData.IsEditMode;
                TypeId = QuickPostData.TypeId;
                CourseList = FilteredCourseList = QuickPostData.AgendaForList;
                DateRangeList = QuickPostData.DateRangeList;
                DateRangeListViewHeight = CourseListViewHeight = DateRangeList.Count() * 32;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void SearchCourse()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                FilteredCourseList = new ObservableCollection<PickListItem>(CourseList.Where(i => i.ItemName.ToLower().Contains(SearchText.ToLower())).ToList());
            }
            else
            {
                FilteredCourseList = CourseList;
            }
            CourseListViewHeight = FilteredCourseList.Count() * 32;
        }
        private void CourseSelectionMethod()
        {
            CourseListVisibility = !CourseListVisibility;
            IsClassesSelected = false;
        }
        private void MonthSelectionMethod()
        {
            MonthListVisibility = !MonthListVisibility;
        }
        private void GroupSelectionMethod()
        {
            GroupListVisibility = !GroupListVisibility;
        }
        async void AgendaForChanged(PickListItem obj)
        {
            if (obj != null)
            {
                SelectedAgendaForText = obj.ItemName;
                //await WarningTextVisibilitySettings();
            }
        }
        async void MonthChangedMethod(PickListItem obj)
        {
            if (obj != null)
            {
                MonthSelectedText = obj.ItemName;
                //await WarningTextVisibilitySettings();
            }
        }
        async void GroupChangedMethod(PickListItem obj)
        {
            if (obj != null)
            {
                GroupSelectedText = obj.ItemName;
                //await WarningTextVisibilitySettings();
            }
        }
        
        //async Task WarningTextVisibilitySettings()
        //{
        //    try
        //    {
        //        if (IsEditMode)
        //        {
        //            if (SelectedAgenda != null && SelectedAgenda.IsStudentSubmissionAllowed)
        //            {
        //                DeleteSubmissionWarningTextVisibility = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperMethods.DisplayException(ex, this.PageTitle);
        //    }
        //}
        private async void CourseListTappedMethod(PickListItem pickListItem)
        {
            try
            {
                if (pickListItem != null)
                {
                    SelectedCourse = pickListItem;
                    CourseListVisibility = false;
                    if (!IsEditMode)
                    {
                        AgendaClassData = await ApiHelper.GetObject<AgendaClassModel>(string.Format(TextResource.GetClassesForWeeklyPlanApi, SelectedCourse.ItemId));
                        if (AgendaClassData != null)
                        {
                            ClassList = AgendaClassData.AgendaClassList != null ? _mapper.Map<ObservableCollection<BindableAgendaClassView>>(AgendaClassData.AgendaClassList) : new ObservableCollection<BindableAgendaClassView>();
                            if (ClassList != null)
                            {
                                ClassListViewHeight = ClassList != null && ClassList.Count > 0 ? (ClassList.Count % 3 == 0 ? (ClassList.Count / 3) * 50 : ((ClassList.Count / 3) * 50) + 50)  : ClassListViewHeight;
                                ClassesVisibility = !IsEditMode;
                            }
                        }
                        GroupTitle = AgendaClassData.DefaultGroupTitle;
                        IsTitleVisible = true;
                    }
                    
                }
                if(IsEditMode)
                {
                    IsTitleVisible = false;
                    IsDateRangeVisible = false;
                    ClassesVisibility = false;
                    CurriculumStandardsVisibility = false;
                    RemarkVisibility = false;
                    DateWiseQuickPost = new ObservableCollection<BindableQuickPost>();
                    EditAgendaWeeklyData = await ApiHelper.GetObjectList<AgendaWeeklyGroupView>(string.Format(TextResource.GetAgendaWeeklyGroupDataApi, SelectedCourse.ItemId));
                    MonthList = new List<PickListItem>();
                    MonthList = EditAgendaWeeklyData
                        .GroupBy(m => new { m.MonthOfWeekStartDate, m.MonthNameOfWeekStartDate })
                        .Select(g => new PickListItem
                        {
                            ItemId = Convert.ToString(g.Key.MonthOfWeekStartDate),
                            ItemName = g.Key.MonthNameOfWeekStartDate
                        })
                        .ToList();
                    SelectedMonthList = new PickListItem();
                    MonthListViewHeight = MonthList.Count * 32;
                    GroupList = new List<PickListItem>();
                    SelectedGroupList = new PickListItem();
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void MonthListTappedMethod(PickListItem pickListItem)
        {
            try
            {
                if (pickListItem != null)
                {
                    IsTitleVisible = false;
                    IsDateRangeVisible = false;
                    ClassesVisibility = false;
                    CurriculumStandardsVisibility = false;
                    RemarkVisibility = false;
                    DateWiseQuickPost = new ObservableCollection<BindableQuickPost>();
                    GroupList = new List<PickListItem>();
                    SelectedGroupList = new PickListItem();


                    SelectedMonthList = pickListItem;
                    MonthListVisibility = false;
                    GroupList = EditAgendaWeeklyData
                        .Where(m => m.MonthOfWeekStartDate.ToString() == SelectedMonthList.ItemId.ToString())
                        .Select(m => new PickListItem
                        {
                            ItemId = m.AgendaWeeklyGroupId.ToString(), // Assuming AgendaWeeklyGroupId is of type int
                            ItemName = m.Title
                        })
                        .Distinct()
                        .ToList();
                    GroupListViewHeight = GroupList.Count * 32;
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void GroupListTappedMethod(PickListItem pickListItem)
        {
            try
            {
                if (pickListItem != null)
                {
                    SelectedGroupList = pickListItem;
                    GroupListVisibility = false;

                    DateRangeListTappedMethod(SelectedGroupList);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void DateRangeSelectionMethod()
        {
            DateRangeListVisibility = !DateRangeListVisibility;
        }
        private async void DateRangeListTappedMethod(PickListItem pickListItem)
        {
            try
            {
                if (pickListItem != null)
                {
                    SelectedDateRange = pickListItem;
                    DateRangeListVisibility = false;
                    CurriculumStandardsVisibility = true;
                    SaveButtonVisibility = true;
                    RemarkVisibility = true;
                    var agendaWeeklyGroupId = IsEditMode ? SelectedGroupList.ItemId : null;
                    var weekRangeId = !IsEditMode ? SelectedDateRange.ItemId : null;
                    AgendaWeeklyData = await ApiHelper.GetObject<AgendaWeeklyGroupEdit>(string.Format(TextResource.LoadAgendaWeeklyPostFormApi, agendaWeeklyGroupId, weekRangeId));
                    IsEnableLearningOutcomes = AgendaWeeklyData.CalendarControlSetting.EnableLearningOutcomes;
                    AgendaWeeklyGroupId = AgendaWeeklyData.AgendaWeeklyGroupDetails.AgendaWeeklyGroupId;
                    WeekStartDate = AgendaWeeklyData.AgendaWeeklyGroupDetails.WeekStartDate.ToString("dd-MMM-yyyy");
                    WeekEndDate = AgendaWeeklyData.AgendaWeeklyGroupDetails.WeekEndDate.ToString("dd-MMM-yyyy");
                    if (AgendaWeeklyData != null && AgendaWeeklyData.AgendaWeekDatesFormatted != null)
                    {
                        DateWiseQuickPost = new ObservableCollection<BindableQuickPost>();
                        foreach (var item in AgendaWeeklyData.AgendaWeekDatesFormatted)
                        {
                            BindableQuickPost quickPost = new BindableQuickPost();
                            quickPost.AgendaWeekDatesFormatted = item;
                            DateWiseQuickPost.Add(quickPost);
                        }
                    }
                    else
                    {
                        DateWiseQuickPost = new ObservableCollection<BindableQuickPost>();
                    }
                    if(IsEditMode)
                    {
                        GroupTitle = AgendaWeeklyData.AgendaWeeklyGroupDetails.Title;
                        DateRangeText = AgendaWeeklyData.AgendaWeeklyGroupDetails.DateRange;
                        IsTitleVisible = true;
                        IsDateRangeVisible = true;
                        ClassList = _mapper.Map<ObservableCollection<BindableAgendaClassView>>(AgendaWeeklyData.AgendaClassesViewModel.AgendaClassList);
                        if (ClassList != null)
                        {
                            ClassListViewHeight = ClassList != null && ClassList.Count > 0 ? (ClassList.Count % 3 == 0 ? (ClassList.Count / 3) * 50 : ((ClassList.Count / 3) * 50) + 50) : ClassListViewHeight;
                            var classData = ClassList.Where(x => !x.IsChecked);
                            IsClassesSelected = !classData.Any();
                            ClassesVisibility = true;
                        }
                        CurriculamStandardsText = AgendaWeeklyData.AgendaWeeklyGroupDetails.CurriculumStandards;
                        RemarkText = AgendaWeeklyData.AgendaWeeklyGroupDetails.Remarks;
                        CurriculumStandardsVisibility = true;
                        RemarkVisibility = true;
                        var groupedAgendaData = AgendaWeeklyData.GroupedAgendaData.ToList();
                        var agendaAttachmentList = AgendaWeeklyData.AgendaAttachmentList;

                        for (int i = 0; i < groupedAgendaData.Count; i++)
                        {
                            var item = groupedAgendaData[i];
                            var bindableQuickPost = DateWiseQuickPost[i];

                            bindableQuickPost.AgendaUId = item.AgendaUId;
                            bindableQuickPost.AgendaId = item.AgendaId;
                            bindableQuickPost.AgendaDescription = item.AgendaDescription;
                            bindableQuickPost.LearningOutcomes = item.LearningOutcomes;
                            bindableQuickPost.IsDeleted = item.IsDeleted;
                            bindableQuickPost.AttachmentFiles = new ObservableCollection<AttachmentFileView>(agendaAttachmentList
                                .Where(a => a.AgendaId == item.AgendaId)
                                .Select(a => new AttachmentFileView
                                {
                                    FileName = a.Attachment,
                                }));
                            bindableQuickPost.AttachmentListViewHeight = bindableQuickPost.AttachmentFiles.Count * 40;
                        }
                        IsCancelButtonVisibleForTeacher = AgendaWeeklyData.IsDeleteButtonVisibleForTeacher;
                        SaveButtonVisibility = AgendaWeeklyData.IsDeleteButtonVisibleForTeacher;
                    }
                    
                    var datesList = AgendaWeeklyData.AgendaWeekDates;
                    for (int i = 0; i < datesList.Count; i++)
                    {
                        if (i < DateWiseQuickPost.Count)
                        {
                            DateWiseQuickPost[i].DueDate = datesList[i].ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            break;
                        }
                    }
                    foreach (var item in DateWiseQuickPost)
                    {
                        item.AssignmentsLabel = AgendaWeeklyData.CalendarControlSetting.AssignmentsLabel;
                        item.LearningOutcomesLabel = AgendaWeeklyData.CalendarControlSetting.LearningOutcomesLabel;
                        //item.AttachmentListViewHeight = item.AttachmentFiles.Count * 40;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void DateRangeChangedMethod(PickListItem obj)
        {
            if (obj != null)
            {
                SelectedDateRangeText = obj.ItemName;
                //await WarningTextVisibilitySettings();
            }
        }
        private void ClassesSelectionMethod()
        {
            if (isHandlingCheck) return;
            try
            {
                isHandlingCheck = true;
                if (IsClassesSelected)
                {
                    ClassList.ToList().ForEach(x => x.IsChecked = true);
                }
                else
                {
                    ClassList.ToList().ForEach(x => x.IsChecked = false);
                    //if (IsEditMode)
                    //{
                    //    await WarningTextVisibilitySettings();
                    //}
                }
            }
            finally
            {
                isHandlingCheck = false; 
            }
        }
        private void ClassSelectionMethod(BindableAgendaClassView classes)
        {
            if (isHandlingCheck) return;
            try
            {
                isHandlingCheck = true;
                if (classes != null)
                {
                    if (ClassList != null)
                    {
                        var selectedClass = ClassList.Where(x => x.ClassId == classes.ClassId)?.FirstOrDefault();
                        if (selectedClass != null)
                        {
                            selectedClass.IsChecked = classes.IsChecked;
                        }
                        bool isAnyDeSelectedClass = ClassList.Any(x => !x.IsChecked);
                        if (isAnyDeSelectedClass)
                        {
                            IsClassesSelected = false;
                        }
                        else
                        {
                            IsClassesSelected = true;
                        }
                    }
                }
            }
            finally
            {
                isHandlingCheck = false; 
            }
        }
        private async void SaveClickedMethod()
        {
            if (isValid())
            {
                try
                {
                    AgendaWeeklyGroupEdit agendaWeeklyGroupEdit = new AgendaWeeklyGroupEdit();
                    agendaWeeklyGroupEdit.TypeId = TypeId;
                    agendaWeeklyGroupEdit.AgendaWeeklyGroupDetails.CurriculumStandards = CurriculamStandardsText;
                    agendaWeeklyGroupEdit.AgendaWeeklyGroupDetails.Remarks = RemarkText;
                    var attachmentList = new List<AttachmentFileView>();
                    List<string> fileList = new List<string>();
                    foreach (var item in DateWiseQuickPost)
                    {
                        if (item.AttachmentFiles != null && item.AttachmentFiles.Count > 0)
                        {
                            //attachmentList.Add((AttachmentFileView)new List<AttachmentFileView>(item.AttachmentFiles));
                            foreach (var attachment in item.AttachmentFiles)
                            {
                                attachmentList.Add(attachment); 
                            }
                        }
                    }
                   
                    List<BindableAgendaClassView> selectedClassIdList = ClassList.Where(x => x.IsChecked).ToList();
                    List<AgendaClassView> classList = new List<AgendaClassView>();
                    foreach (var item in selectedClassIdList)
                    {
                        if (item != null)
                        {
                            classList.Add(new AgendaClassView() { ClassId = item.ClassId, IsElective = item.IsElective });
                        }
                    }
                    agendaWeeklyGroupEdit.ClassIds = JsonConvert.SerializeObject(classList);

                    agendaWeeklyGroupEdit.GroupedAgendaAttachmentDataString = JsonConvert.SerializeObject(GroupedAgendaAttachmentDataString);
                    agendaWeeklyGroupEdit.AgendaWeeklyGroupDetails.Title = GroupTitle;
                    agendaWeeklyGroupEdit.AgendaWeeklyGroupDetails.AgendaWeeklyGroupId = AgendaWeeklyGroupId;
                    agendaWeeklyGroupEdit.AgendaWeeklyGroupDetails.WeekStartDateString = WeekStartDate;
                    agendaWeeklyGroupEdit.AgendaWeeklyGroupDetails.WeekEndDateString = WeekEndDate; 
                    agendaWeeklyGroupEdit.CurriculumId = Convert.ToInt16(SelectedCourse.ItemId);

                    var serializedList = new List<object>();
                    foreach (var item in DateWiseQuickPost)
                    {
                        if(item.AgendaDescription != null || item.LearningOutcomes != null)
                        {
                            serializedList.Add(new BindableQuickPost()
                            {
                                AgendaUId = item.AgendaUId,
                                AgendaDescription = item.AgendaDescription,
                                LearningOutcomes = item.LearningOutcomes,
                                DueDate = item.DueDate
                            });
                        }
                    }
                    string serializedJson = JsonConvert.SerializeObject(serializedList);
                    agendaWeeklyGroupEdit.GroupedAgendaDataString = serializedJson;
                    
                    
                    OperationDetails result = await ApiHelper.PostMultiDataRequestAsync<OperationDetails>(TextResource.SaveAgendaQuickPostDataApi, AppSettings.Current.ApiUrl, agendaWeeklyGroupEdit, attachmentList);
                    if (result.Success)
                    {
                        await HelperMethods.ShowAlert("", PageTitle + " " + TextResource.ResourceSaveSuccessMessage);
                        await Navigation.PopAsync();
                        if (IsFromEditButton)
                        {
                            await Navigation.PopAsync();
                        }
                        MessagingCenter.Send<string>("", "LoadWeeklyPlan");
                    }
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                    //Crashes.TrackError(ex);
                }
            }
        }
        bool isValid()
        {
            if (string.IsNullOrEmpty(SelectedCourse.ItemName))
            {
                IsAgendaForErrorVisible = true;
            }
            else if (ClassList != null)
            {
                var classData = ClassList.Where(x => x.IsChecked).ToList();
                if (classData == null || classData.Count() <= 0)
                {
                    IsAgendaForErrorVisible = false;
                    ClassSelectionErrorVisibility = true;
                }
                else if (classData != null && classData.Count() > 0)
                {
                    ClassSelectionErrorVisibility = false;
                    IsAgendaForErrorVisible = false;
                }
            }
            if (IsAgendaForErrorVisible != true && ClassSelectionErrorVisibility != true)
            {
                return true;
            }
            return false;
        }
        async void CancelWeeklyAgendaMethod()
        {
            try
            {
                var action = await App.Current.MainPage.DisplayAlert("", TextResource.CancelAgendaText, TextResource.YesText, TextResource.NoText);
                if (action)
                {
                    var apiUrl = string.Format(TextResource.DeleteAgendaWeeklyPlanApi, AgendaWeeklyGroupId);
                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(apiUrl);
                    if (result.Success)
                    {
                        await Navigation.PopAsync();
                        if(IsFromEditButton)
                        {
                            await Navigation.PopAsync();
                            MessagingCenter.Send<string>("", "LoadWeeklyPlan");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void SaveEditorClickedMethod()
        {
            string wrappedHtmlContent = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" 
                                        + EditedHtmlContent 
                                        + "</body></html>";
            switch (_selectedHtmlContentKey)
            {
                case "remarktext":
                    RemarkText = wrappedHtmlContent;
                    break;
                case "curriculamstandardstext":
                    CurriculamStandardsText = wrappedHtmlContent;
                    break;
                case "learningoutcomes":
                    if (_currentQuickPost != null)
                    {
                        _currentQuickPost.LearningOutcomes = wrappedHtmlContent; 
                    }
                    break;
                case "agendadescription":
                    if (_currentQuickPost != null)
                    {
                        _currentQuickPost.AgendaDescription = wrappedHtmlContent; 
                    }
                    break;
            }
            _currentPopup?.Close();
        }
        private void OpenHtmlEditPopup(string htmlText, string key, string title)
        {
            EditedHtmlContent = ExtractBodyContent(htmlText);
            _selectedHtmlContentKey = key;
            HtmlEditorTitle = title;
            EditHtmlContentPopup htmlContentPopup = new ()
            {
                BindingContext = this
            };
            SetPopupInstance(htmlContentPopup);
            Application.Current.MainPage.ShowPopup(htmlContentPopup);
        }
        public void SetPopupInstance(Popup popup)
        {
            _currentPopup = popup;
        }
        private string ExtractBodyContent(string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return string.Empty; 
            }

            int bodyStart = htmlText.IndexOf("<body>") + 6;
            int bodyEnd = htmlText.IndexOf("</body>");

            if (bodyStart != -1 && bodyEnd != -1)
            {
                string bodyContent = htmlText.Substring(bodyStart, bodyEnd - bodyStart).Trim();
        
                string strippedContent = Regex.Replace(bodyContent, "<.*?>", string.Empty).Trim();
        
                return strippedContent;
            }
            else
            {
                string strippedContent = Regex.Replace(htmlText, "<.*?>", string.Empty).Trim();
                
                return strippedContent;
            }
        }



        private async void RemarkEditMethod(object obj)
        {
            try
            {
                OpenHtmlEditPopup(RemarkText, "remarktext", "Remarks");
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void CurriculamStandardsEditMethod(object obj)
        {
            try
            {
                OpenHtmlEditPopup(CurriculamStandardsText, "curriculamstandardstext", "Curriculum Standards");
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void LearningOutcomesEditMethod(BindableQuickPost quickPost)
        {
            try
            {
                _currentQuickPost = quickPost;
                OpenHtmlEditPopup(quickPost.LearningOutcomes, "learningoutcomes", quickPost.LearningOutcomesLabel);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void AssignmentEditedMethod(BindableQuickPost quickPost)
        {
            try
            {
                _currentQuickPost = quickPost;
                OpenHtmlEditPopup(quickPost.AgendaDescription, "agendadescription", quickPost.AssignmentsLabel);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        //private string RemoveUnwantedHTML(string html)
        //{
        //    string pattern = @"<(meta|script)[^>]*>";

        //    string cleanedHTML = Regex.Replace(html, pattern, string.Empty);

        //    return cleanedHTML;
        //}
        private string RemoveUnwantedHTML(string html)
        {
            // Remove meta and script tags
            string pattern = @"<(meta|script)[^>]*>|</?(meta|script)[^>]*>";
            string cleanedHTML = Regex.Replace(html, pattern, string.Empty);

            // Extract content inside the body tag
            pattern = @"<body[^>]*>(.*?)<\/body>";
            Match match = Regex.Match(cleanedHTML, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match.Success)
            {
                cleanedHTML = match.Groups[1].Value;
            }

            // Wrap content in paragraph tag if it's plain text
            if (!cleanedHTML.Contains("<"))
            {
                cleanedHTML = $"<p>{cleanedHTML}</p>";
            }

            return cleanedHTML;
        }
        private void NewPostRadioButtonMethod(object obj)
        {
            try
            {
                NewPostRadioButtonImage = "selected_radio_button.png";
                EditPostRadioButtonImage = "unselected_radio_button.png";
                IsNewPostVisible = true;
                IsEditPostVisible = false;
                IsEditMode = false;
                IsTitleVisible = false;
                IsDateRangeVisible = false;
                ClassesVisibility = false;
                CurriculumStandardsVisibility = false;
                RemarkVisibility = false;
                SelectedAgendaForText = string.Empty;
                SelectedDateRangeText = string.Empty;
                GroupTitle = string.Empty;
                CurriculamStandardsText = string.Empty;
                RemarkText = string.Empty;
                DateWiseQuickPost = new ObservableCollection<BindableQuickPost>();
                SaveButtonVisibility = false;
                IsCancelButtonVisibleForTeacher = false;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private void EditPostRadioButtonMethod(object obj)
        {
            try
            {
                NewPostRadioButtonImage = "unselected_radio_button.png";
                EditPostRadioButtonImage = "selected_radio_button.png";
                IsNewPostVisible = false;
                IsEditPostVisible = true;
                IsEditMode = true;
                IsTitleVisible = false;
                IsDateRangeVisible = false;
                ClassesVisibility = false;
                CurriculumStandardsVisibility = false;
                RemarkVisibility = false;
                SelectedAgendaForText = string.Empty;
                SelectedDateRangeText = string.Empty;
                MonthSelectedText = string.Empty;
                GroupSelectedText = string.Empty;
                GroupTitle = string.Empty;
                DateWiseQuickPost = new ObservableCollection<BindableQuickPost>();
                SaveButtonVisibility = false;
                IsCancelButtonVisibleForTeacher = false;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void AttachmentMethod(BindableQuickPost bindableQuickPost)
        {
            try
            {
                AttachmentFileView fileData = await HelperMethods.PickFileFromDevice();
                if (fileData == null)
                {
                    return;
                }

                string foundSpecialChars = FindSpecialCharacters(fileData.FileName);
                if (!string.IsNullOrEmpty(foundSpecialChars))
                {
                    await HelperMethods.ShowAlert(TextResource.AlertsPageTitle,
                        "File name can't contain any of the following character(s): " + foundSpecialChars);
                    return;
                }
                if (bindableQuickPost.AttachmentFiles.Any(x => x.FileName.Equals(fileData.FileName, StringComparison.OrdinalIgnoreCase)))
                {
                    await HelperMethods.ShowAlert("", "This file has already been added.");
                    return;
                }
                if (bindableQuickPost.AttachmentFiles == null)
                {
                    bindableQuickPost.AttachmentFiles = new ObservableCollection<AttachmentFileView>();
                }
                
                bindableQuickPost.AttachmentFiles.AddFileToList(fileData);
                bindableQuickPost.AttachmentListViewHeight = bindableQuickPost.AttachmentFiles.Count * 40;

                MessagingCenter.Send("", "UpdateAttachmentListView");
                var agendaItem = new AgendaView
                {
                    Attachment = fileData.FileName,
                    DueDate = bindableQuickPost.DueDate,
                };
                if (IsEditMode)
                {
                    agendaItem.IsDeleted = false;
                    agendaItem.AgendaUId = bindableQuickPost.AgendaUId;
                }
                
                GroupedAgendaAttachmentDataString.Add(agendaItem);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                throw;
            }
        }
        private string FindSpecialCharacters(string fileName)
        {
            string specialCharacters = "!@#$%^&*()+[]{}|;:'\"<>,?/~`";
            var foundSpecialCharacters = fileName.Where(c => specialCharacters.Contains(c)).Distinct();
            string specialCharactersString = string.Join(", ", foundSpecialCharacters);

            return specialCharactersString;
        }
        private async void DeleteAttachmentClicked(object obj)
        {
            try
            {
                if (obj != null)
                {
                    AttachmentFileView attachmentFile = (AttachmentFileView)obj;
                    for (int i = GroupedAgendaAttachmentDataString.Count - 1; i >= 0; i--)
                    {
                        if (GroupedAgendaAttachmentDataString[i].Attachment == attachmentFile.FileName)
                        {
                            GroupedAgendaAttachmentDataString.RemoveAt(i);
                        }
                    }
                    var action = await App.Current.MainPage.DisplayAlert("", TextResource.DeleteText, TextResource.YesText, TextResource.NoText);
                    if (action)
                    {
                        //AttachmentFileView attachmentFile = (AttachmentFileView)obj;
                        foreach (var item in DateWiseQuickPost)
                        {
                            var itemToRemove = item.AttachmentFiles.FirstOrDefault(x => x.FileName == attachmentFile.FileName);
                            if (itemToRemove != null)
                            {
                                var agendaItem = new AgendaView
                                {
                                    Attachment = attachmentFile.FileName,
                                    DueDate = item.DueDate,
                                };
                                if (IsEditMode)
                                {
                                    agendaItem.IsDeleted = true;
                                    agendaItem.AgendaUId = item.AgendaUId;
                                    GroupedAgendaAttachmentDataString.Add(agendaItem);
                                }
                                item.AttachmentFiles.Remove(itemToRemove);
                                item.AttachmentListViewHeight = item.AttachmentFiles.Count * 40;
                                MessagingCenter.Send("", "UpdateAttachmentListView");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        #endregion
    }