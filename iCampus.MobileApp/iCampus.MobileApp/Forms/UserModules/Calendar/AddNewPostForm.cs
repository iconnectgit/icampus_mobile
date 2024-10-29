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
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.BooksReservation;
using iCampus.Portal.EditModels;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class AddNewPostForm : ViewModelBase
    {
        #region Declarations
        public ICommand SaveEditorCommand { get; set; }
        private Popup _currentPopup;
        private bool isHandlingCheck = false;
        private string _selectedHtmlContentKey;
        public ICommand DetailsTabbedCommand { get; set; }
        public ICommand AttachmentsTabbedCommand { get; set; }
        public ICommand SearchCourseCommand { get; set; }
        public ICommand CourseSelectionCommand { get; set; }
        public ICommand CourseListTappedCommand { get; set; }
        public ICommand UsersIconClickedCommand { get; set; }
        public ICommand StudentSelectionCommand { get; set; }
        public ICommand ClassSelectionCommand { get; set; }
        public ICommand SelectFileCommand { get; set; }
        public ICommand DeleteAttachmentClickCommand { get; set; }
        public ICommand CalendarIconClickCommand { get; set; }
        public ICommand AttachmentClickCommand { get; set; }
        public ICommand SearchIconClickCommand { get; set; }
        public ICommand SearchButtonClickCommand { get; set; }
        public ICommand AddPostCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand FilebankFileSelectionCommand { get; set; }
        public ICommand IncrementReminderBeforeDaysCommand { get; set; }
        public ICommand DecrementReminderBeforeDaysCommand { get; set; }
        public ICommand ClassesSelectionCommand { get; set; }
        public ICommand CancelAgendaCommand { get; set; }
        public ICommand CloseWarningCommand { get; set; }
        public ICommand AgendaTypeChangedCommand { get; set; }
        public ICommand AgendaForChangedCommand { get; set; }
        public ICommand AllowSubmissionsCheckChangedCommand { get; set; }
        public ICommand CloseAlreadyAddedAttachedErrorPopupCommand { get; set; }
        public ICommand EditClickCommand { get; set; }
        public ICommand LeanningOutcomeEditClickCommand { get; set; }

        int daysTobeAdded;
        private bool _isEnableCaching = true;
        string filebankAttachmentsIds;
        #endregion Declarations

        #region Properties
        bool _allowSubmissionsVisibility;
        public bool AllowSubmissionsVisibility
        {
            get => _allowSubmissionsVisibility;
            set
            {
                _allowSubmissionsVisibility = value;
                OnPropertyChanged(nameof(AllowSubmissionsVisibility));
            }
        }
        bool _isAllowSubmissions;
        public bool IsAllowSubmissions
        {
            get => _isAllowSubmissions;
            set
            {
                _isAllowSubmissions = value;
                OnPropertyChanged(nameof(IsAllowSubmissions));
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

        IList<PickListItem> _gradeList = new List<PickListItem>();
        public IList<PickListItem> GradeList
        {
            get
            {
                return _gradeList;
            }
            set
            {
                _gradeList = value;
                OnPropertyChanged(nameof(GradeList));
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
        string _coursePickerPlaceHolder;
        public string CoursePickerPlaceHolder
        {
            get => _coursePickerPlaceHolder;
            set
            {
                _coursePickerPlaceHolder = value;
                OnPropertyChanged(nameof(CoursePickerPlaceHolder));
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
        bool _courseListVisibility=false;
        public bool CourseListVisibility
        {
            get => _courseListVisibility;
            set
            {
                _courseListVisibility = value;
                OnPropertyChanged(nameof(CourseListVisibility));
            }
        }
        int _limitCount = 0;
        public int LimitCount
        {
            get => _limitCount;
            set
            {
                _limitCount = value;
                OnPropertyChanged(nameof(LimitCount));
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
                if(_selectedCourse!=null&&!string.IsNullOrEmpty(_selectedCourse.ItemName))
                {
                    if(!IsEditMode)
                    {
                        GetClassesInAddMode();
                    }
                }
            }
        }
        string _selectedClassName;
        public string SelectedClassName
        {
            get => _selectedClassName;
            set
            {
                _selectedClassName = value;
                OnPropertyChanged(nameof(SelectedClassName));
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
        ObservableCollection<BindableAgendaClassStudentView> _studentsList = new ObservableCollection<BindableAgendaClassStudentView>();
        public ObservableCollection<BindableAgendaClassStudentView> StudentsList
        {
            get
            {
                return _studentsList;
            }
            set
            {
                _studentsList = value;
                OnPropertyChanged(nameof(StudentsList));
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
        private int _studentListViewHeight;
        public int StudentListViewHeight
        {
            get => _studentListViewHeight;
            set
            {
                _studentListViewHeight = value;
                OnPropertyChanged(nameof(StudentListViewHeight));
            }
        }
        ObservableCollection<AttachmentFileView> _attachmentFiles = new ObservableCollection<AttachmentFileView>();
        public ObservableCollection<AttachmentFileView> AttachmentFiles
        {
            get => _attachmentFiles;
            set
            {
                _attachmentFiles = value;
                OnPropertyChanged(nameof(AttachmentFiles));
            }
        }
        int _attachmentListViewHeight=0;
        public int AttachmentListViewHeight
        {
            get => _attachmentListViewHeight;
            set
            {
                _attachmentListViewHeight = value;
                OnPropertyChanged(nameof(AttachmentListViewHeight));
            }
        }
        bool _datePickerVisibility=false;
        public bool DatePickerVisibility
        {
            get => _datePickerVisibility;
            set
            {
                _datePickerVisibility = value;
                OnPropertyChanged(nameof(DatePickerVisibility));
            }
        }
        ObservableCollection<BindableAgendaView> _fileBankDataList = new ObservableCollection<BindableAgendaView>();
        public ObservableCollection<BindableAgendaView> FileBankDataList
        {
            get => _fileBankDataList;
            set
            {
                _fileBankDataList = value;
                OnPropertyChanged(nameof(FileBankDataList));
            }
        }
        DateTime _fromDate=DateTime.MinValue;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
            }
        }

        DateTime _toDate=DateTime.MaxValue;
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged(nameof(ToDate));
            }
        }
        ObservableCollection<BindableAgendaView> _filteredFileBankAttachmentList = new ObservableCollection<BindableAgendaView>();
        public ObservableCollection<BindableAgendaView> FilteredFileBankAttachmentList
        {
            get => _filteredFileBankAttachmentList;
            set
            {
                _filteredFileBankAttachmentList = value;
                OnPropertyChanged(nameof(FilteredFileBankAttachmentList));
            }
        }
        string _fileBankSearchText=string.Empty;
        public string FileBankSearchText
        {
            get => _fileBankSearchText;
            set
            {
                _fileBankSearchText = value;
                OnPropertyChanged(nameof(FileBankSearchText));
            }
        }
        IList<CalendarAgendaTypePickListItem> _agendaTypes = new List<CalendarAgendaTypePickListItem>();
        public IList<CalendarAgendaTypePickListItem> AgendaTypes
        {
            get
            {
                return _agendaTypes;
            }
            set
            {
                _agendaTypes = value;
                OnPropertyChanged(nameof(AgendaTypes));
            }
        }
        CalendarAgendaTypePickListItem _selectedAgendaTypes = new CalendarAgendaTypePickListItem();
        public CalendarAgendaTypePickListItem SelectedAgendaTypes
        {
            get
            {
                return _selectedAgendaTypes;
            }
            set
            {
                _selectedAgendaTypes = value;
                OnPropertyChanged(nameof(SelectedAgendaTypes));
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
        bool _studentListVisibility = false;
        public bool StudentListVisibility
        {
            get => _studentListVisibility;
            set
            {
                _studentListVisibility = value;
                OnPropertyChanged(nameof(StudentListVisibility));
            }
        }
        DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                OnPropertyChanged(nameof(DueDate));
            }
        }
        DateTime _reminderDate;
        public DateTime ReminderDate
        {
            get => _reminderDate;
            set
            {
                _reminderDate = value;
                OnPropertyChanged(nameof(ReminderDate));
            }
        }
        int _reminderBeforeText=1;
        public int ReminderBeforeText
        {
            get => _reminderBeforeText;
            set
            {
                _reminderBeforeText = value;
                OnPropertyChanged(nameof(ReminderBeforeText));
            }
        }
        bool _isAgendaTypeErrorVisible = false;
        public bool IsAgendaTypeErrorVisible
        {
            get => _isAgendaTypeErrorVisible;
            set
            {
                _isAgendaTypeErrorVisible = value;
                OnPropertyChanged(nameof(IsAgendaTypeErrorVisible));
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

        string _assignmentLabel; 
        public string AssignmentLabel
        {
            get => _assignmentLabel;
            set
            {
                _assignmentLabel = value;
                OnPropertyChanged(nameof(AssignmentLabel));
            }
        }

        string _learningOutcomeLabel;
        public string LearningOutcomeLabel
        {
            get => _learningOutcomeLabel;
            set
            {
                _learningOutcomeLabel = value;
                OnPropertyChanged(nameof(LearningOutcomeLabel));
            }
        }

        string _learningOutcomeText;
        public string LearningOutcomeText
        {
            get
            {
                if (string.IsNullOrEmpty(_learningOutcomeText))
                {
                    _learningOutcomeText = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" + _learningOutcomeText + "</body></html>";
                }
                return _learningOutcomeText;
            }
            set
            {
                _learningOutcomeText = value;
                OnPropertyChanged(nameof(LearningOutcomeText));
            }
        }

        bool _isLearningOutcomeVisible = false;
        public bool IsLearningOutcomeVisible
        {
            get => _isLearningOutcomeVisible;
            set
            {
                _isLearningOutcomeVisible = value;
                OnPropertyChanged(nameof(IsLearningOutcomeVisible));
            }
        }

        string _assignmentsText;
        public string AssignmentsText
        {
            get
            {
                if (string.IsNullOrEmpty(_assignmentsText))
                {
                    _assignmentsText = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" + _assignmentsText + "</body></html>";
                }
                return _assignmentsText;
            }
            set
            {
                _assignmentsText = value;
                OnPropertyChanged(nameof(AssignmentsText));
            }
        }
        DateTime _dueMinimumDate;
        public DateTime DueMinimumDate
        {
            get => _dueMinimumDate;
            set
            {
                _dueMinimumDate = value;
                OnPropertyChanged(nameof(DueMinimumDate));
            }
        }
        AgendaEdit _addPostData;
        public AgendaEdit AddPostData
        {
            get => _addPostData;
            set
            {
                _addPostData = value;
                OnPropertyChanged(nameof(AddPostData));
            }
        }
        bool _isAllowSubmissionsEnabled;
        public bool IsAllowSubmissionsEnabled
        {
            get => _isAllowSubmissionsEnabled;
            set
            {
                _isAllowSubmissionsEnabled = value;
                OnPropertyChanged(nameof(IsAllowSubmissionsEnabled));
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
        DateTime _reminderMinimumDate=DateTime.Now;
        public DateTime ReminderMinimumDate
        {
            get => _reminderMinimumDate;
            set
            {
                _reminderMinimumDate = value;
                OnPropertyChanged(nameof(ReminderMinimumDate));
            }
        }
        DateTime _reminderMaxDate = DateTime.Now;
        public DateTime ReminderMaxDate
        {
            get => _reminderMaxDate;
            set
            {
                _reminderMaxDate = value;
                OnPropertyChanged(nameof(ReminderMaxDate));
            }
        }
        string _submissionsCount;
        public string SubmissionsCount
        {
            get => _submissionsCount;
            set
            {
                _submissionsCount = value;
                OnPropertyChanged(nameof(SubmissionsCount));
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
        BindableAgendaView _selectedAgenda = new BindableAgendaView();
        public BindableAgendaView SelectedAgenda
        {
            get => _selectedAgenda;
            set
            {
                _selectedAgenda = value;
                OnPropertyChanged(nameof(SelectedAgenda));
            }
        }
        IList<AgendaClassStudentView> _classStudentsData = new List<AgendaClassStudentView>();
        public IList<AgendaClassStudentView> ClassStudentsData
        {
            get => _classStudentsData;
            set
            {
                _classStudentsData = value;
                OnPropertyChanged(nameof(ClassStudentsData));
            }
        }
        string _deleteSubmissionWarningText=TextResource.DeleteSubmissionWarningText;
        public string DeleteSubmissionWarningText
        {
            get => _deleteSubmissionWarningText;
            set
            {
                _deleteSubmissionWarningText = value;
                OnPropertyChanged(nameof(DeleteSubmissionWarningText));
            }
        }
        bool _deleteSubmissionWarningTextVisibility = false;
        public bool DeleteSubmissionWarningTextVisibility
        {
            get => _deleteSubmissionWarningTextVisibility;
            set
            {
                _deleteSubmissionWarningTextVisibility = value;
                OnPropertyChanged(nameof(DeleteSubmissionWarningTextVisibility));
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
        ObservableCollection<BindableAgendaView> _fileBankAttachmentList = new ObservableCollection<BindableAgendaView>();
        public ObservableCollection<BindableAgendaView> FileBankAttachmentList
        {
            get => _fileBankAttachmentList;
            set
            {
                _fileBankAttachmentList = value;
                OnPropertyChanged(nameof(FileBankAttachmentList));
            }
        }
        bool _isFileBankAttachmentNotAvailable = false;
        public bool IsFileBankAttachmentNotAvailable
        {
            get => _isFileBankAttachmentNotAvailable;
            set
            {
                _isFileBankAttachmentNotAvailable = value;
                OnPropertyChanged(nameof(IsFileBankAttachmentNotAvailable));
            }
        }
        bool _alreadyAddedAttachmentErrorPopupVisibility = false;
        public bool AlreadyAddedAttachmentErrorPopupVisibility
        {
            get => _alreadyAddedAttachmentErrorPopupVisibility;
            set
            {
                _alreadyAddedAttachmentErrorPopupVisibility = value;
                OnPropertyChanged(nameof(AlreadyAddedAttachmentErrorPopupVisibility));
            }
        }
        string _alreadyAddedAttachmentText = TextResource.AttachmentAlreadyAddedText;
        public string AlreadyAddedAttachmentText
        {
            get => _alreadyAddedAttachmentText;
            set
            {
                _alreadyAddedAttachmentText = value;
                OnPropertyChanged(nameof(AlreadyAddedAttachmentText));
            }
        }
        List<int> _deletedAttachmentFileID = new List<int>();
        public List<int> DeletedAttachmentFileID
        {
            get => _deletedAttachmentFileID;
            set
            {
                _deletedAttachmentFileID = value;
                OnPropertyChanged(nameof(DeletedAttachmentFileID));
            }
        }

        List<string> _deletedAttachmentFileName = new List<string>();
        public List<string> DeletedAttachmentFileName
        {
            get => _deletedAttachmentFileName;
            set
            {
                _deletedAttachmentFileName = value;
                OnPropertyChanged(nameof(DeletedAttachmentFileName));
            }
        }
        List<AgendaView> _existingAttachmentList = new List<AgendaView>();
        public List<AgendaView> ExistingAttachmentList
        {
            get => _existingAttachmentList;
            set
            {
                _existingAttachmentList = value;
                OnPropertyChanged(nameof(ExistingAttachmentList));
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
        List<string> _checkedStudentIds = new List<string>();
        public List<string> CheckedStudentIds
        {
            get => _checkedStudentIds;
            set
            {
                _checkedStudentIds = value;
                OnPropertyChanged(nameof(CheckedStudentIds));
            }
        }
        bool _isReminderTextVisible = true;
        public bool IsReminderTextVisible
        {
            get => _isReminderTextVisible;
            set
            {
                _isReminderTextVisible = value;
                OnPropertyChanged(nameof(IsReminderTextVisible));
            }
        }
        bool _isReminderDateVisible = true;
        public bool IsReminderDateVisible
        {
            get => _isReminderDateVisible;
            set
            {
                _isReminderDateVisible = value;
                OnPropertyChanged(nameof(IsReminderDateVisible));
            }
        }
        bool _isClassesEnabled = true;
        public bool IsClassesEnabled
        {
            get => _isClassesEnabled;
            set
            {
                _isClassesEnabled = value;
                OnPropertyChanged(nameof(IsClassesEnabled));
            }
        }
        float _classListOpacity = 1.0F;
        public float ClassListOpacity
        {
            get => _classListOpacity;
            set
            {
                _classListOpacity = value;
                OnPropertyChanged(nameof(ClassListOpacity));
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
        IList<BindableAttachmentFileView> _selectedAttachmentList;
        public IList<BindableAttachmentFileView> SelectedAttachmentList
        {
            get => _selectedAttachmentList;
            set
            {
                _selectedAttachmentList = value;
                OnPropertyChanged(nameof(SelectedAttachmentList));
            }
        }
        private decimal _detailsButtonOpacity;

        public decimal DetailsButtonOpacity
        {
            get => _detailsButtonOpacity;
            set
            {
                _detailsButtonOpacity = value;
                OnPropertyChanged(nameof(DetailsButtonOpacity));
            }
        }

        private decimal _attachmentsButtonOpacity;

        public decimal AttachmentsButtonOpacity
        {
            get => _attachmentsButtonOpacity;
            set
            {
                _attachmentsButtonOpacity = value;
                OnPropertyChanged(nameof(AttachmentsButtonOpacity));
            }
        }

        private bool _isDetailsVisible;

        public bool IsDetailsVisible
        {
            get => _isDetailsVisible;
            set
            {
                _isDetailsVisible = value;
                OnPropertyChanged(nameof(IsDetailsVisible));
            }
        }

        private bool _isAttachmentsVisible;

        public bool IsAttachmentsVisible
        {
            get => _isAttachmentsVisible;
            set
            {
                _isAttachmentsVisible = value;
                OnPropertyChanged(nameof(IsAttachmentsVisible));
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

        #endregion Properties
        public AddNewPostForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        #region Methods
        async void InitializePage()
        {
            IsDetailsVisible = true;
            IsAttachmentsVisible = false;
            DetailsButtonOpacity = 1.0m;
            AttachmentsButtonOpacity = 0.5m;
            DetailsTabbedCommand = new Command(DetailsClickedMethod);
            AttachmentsTabbedCommand = new Command(AttachmentsClickedMethod);
            SearchCourseCommand = new Command(SearchCourse);
            CourseSelectionCommand = new Command(CourseSelection);
            CourseListTappedCommand = new Command<PickListItem>(CourseListTapped);
            UsersIconClickedCommand = new Command<BindableAgendaClassView>(UsersIconClicked);
            StudentSelectionCommand = new Command<BindableAgendaClassStudentView>(StudentSelection);
            ClassSelectionCommand = new Command<BindableAgendaClassView>(ClassSelection);
            SelectFileCommand = new Command(AddAttachmentClicked);
            DeleteAttachmentClickCommand = new Command(DeleteAttachmentClicked);
            CalendarIconClickCommand = new Command(CalendarIconClicked);
            AttachmentClickCommand = new Command<BindableAgendaView>(AttachmentClicked);
            SearchIconClickCommand = new Command(SearchIconClicked);
            SearchButtonClickCommand = new Command(SearchButtonClicked);
            AddPostCommand = new Command(AddToPostClicked);
            SaveCommand = new Command(SaveClicked);
            FilebankFileSelectionCommand = new Command<BindableAgendaView>(FileBankFileSelection);
            IncrementReminderBeforeDaysCommand = new Command(IncrementReminderBeforeDaysTapped);
            DecrementReminderBeforeDaysCommand = new Command(DecrementReminderBeforeDaysTapped);
            ClassesSelectionCommand = new Command(ClassesCheckChanged);
            CancelAgendaCommand = new Command(CancelAgendaClicked);
            CloseWarningCommand = new Command(CloseWarningClicked);
            AgendaTypeChangedCommand = new Command(AgendaTypeChanged);
            AgendaForChangedCommand = new Command<PickListItem>(AgendaForChanged);
            AllowSubmissionsCheckChangedCommand = new Command(AllowSubmissionsCheckChanged);
            CloseAlreadyAddedAttachedErrorPopupCommand = new Command(CloseAlreadyAddedAttachedErrorPopupClicked);
            EditClickCommand = new Command(EditClicked);
            LeanningOutcomeEditClickCommand = new Command(LeanningOutcomeEditClicked);
            SaveEditorCommand = new Command(SaveEditorClickedMethod);
            DueMinimumDate = DateTime.Now;
            MessagingCenter.Subscribe<string>("", "ReminderDateSelected", async (args) =>
            {
                ReminderBeforeText = (DueDate-ReminderDate).Days;
            });
            MessagingCenter.Subscribe<string>("", "SetReminderMaxDate", async (args) =>
            {
                if(!IsEditMode)
                {
                    if(DueDate.Date.CompareTo(DateTime.Now.Date)==0)
                    {
                        ReminderMinimumDate = DueDate.AddDays(-ReminderBeforeText);
                    }
                    ReminderMaxDate = DueDate.Date;  //AddDays(-1);
                    ReminderDate = DueDate.AddDays(-ReminderBeforeText);
                }
                else
                {
                    DueMinimumDate = DateTime.MinValue;
                    if (DueDate.CompareTo(DateTime.Now.Date) == 0)
                    {
                        ReminderMinimumDate = DueDate.AddDays(-ReminderBeforeText);
                    }
                    else
                    {
                        ReminderMinimumDate = DueDate.AddDays(-ReminderBeforeText);
                        ReminderMaxDate = ReminderMinimumDate;
                    }
                    ReminderDate = DueDate.AddDays(-ReminderBeforeText);
                }
            });
        }
        private void DetailsClickedMethod(object obj)
        {
            IsDetailsVisible = true;
            IsAttachmentsVisible = false;
            DetailsButtonOpacity = 1.0m;
            AttachmentsButtonOpacity = 0.5m;
        }

        private void AttachmentsClickedMethod(object obj)
        {
            IsDetailsVisible = false;
            IsAttachmentsVisible = true;
            DetailsButtonOpacity = 0.5m;
            AttachmentsButtonOpacity = 1.0m;
        }
        private void SaveEditorClickedMethod()
        {
            string wrappedHtmlContent = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" 
                                        + EditedHtmlContent 
                                        + "</body></html>";
            switch (_selectedHtmlContentKey)
            {
                case "learningoutcomes":
                    LearningOutcomeText = wrappedHtmlContent;
                    break;
                case "assignmenttext":
                    AssignmentsText = wrappedHtmlContent;
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
        
        private string ExtractBodyContent(string htmlText)
        {
            int bodyStart = htmlText.IndexOf("<body>") + 6;
            int bodyEnd = htmlText.IndexOf("</body>");

            if (bodyStart != -1 && bodyEnd != -1)
            {
                return htmlText.Substring(bodyStart, bodyEnd - bodyStart).Trim();
            }
            return htmlText; 
        }

        private async void LeanningOutcomeEditClicked(object obj)
        {
            try
            {
                OpenHtmlEditPopup(LearningOutcomeText, "learningoutcomes", LearningOutcomeLabel);
                // if (Device.RuntimePlatform == Device.iOS)
                //     IsNavigationBarVisible = true;
                //
                // CrossTEditor.PageTitle = this.PageTitle;
                // CrossTEditor.SaveText = "Save";
                // CrossTEditor.CancelText = " ";
                //
                // string color = AppSettings.Current.Settings.ThemeColor;// "#000000";
                // await Task.Delay(100);
                // Task.Run(async () =>
                // {
                //     await Task.Delay(500);
                //     Device.BeginInvokeOnMainThread(() =>
                //     {
                //         Xamarin.Forms.DependencyService.Get<INativeServices>().SetToolBarColor(color);
                //     });
                // });
                // //if (string.IsNullOrEmpty(this.AssignmentsText))
                // //{
                // //    this.AssignmentsText = "<html></html>";
                // //}
                // TEditorResponse response = await CrossTEditor.Current.ShowTEditor(this.LearningOutcomeText,characterLimit:LimitCount);
                // IsNavigationBarVisible = false;
                // if (response.IsSave)
                // {
                //     if (response.HTML != null)
                //     {
                //         if (Device.RuntimePlatform == Device.Android)
                //             this.LearningOutcomeText = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" + response.HTML + "</body></html>";
                //         else
                //             this.LearningOutcomeText = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" + response.HTML + "</body></html>";
                //     }
                // }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void EditClicked(object obj)
        {
            try
            {
                OpenHtmlEditPopup(AssignmentsText, "assignmenttext", AssignmentLabel);
                // if (Device.RuntimePlatform == Device.iOS)
                //     IsNavigationBarVisible = true;
                //
                // CrossTEditor.PageTitle = this.PageTitle;
                // CrossTEditor.SaveText = "Save";
                // CrossTEditor.CancelText = "";
                //
                // string color = AppSettings.Current.Settings.ThemeColor;// "#000000";
                // await Task.Delay(100);
                // Task.Run(async () =>
                // {
                //     await Task.Delay(500);
                //     Device.BeginInvokeOnMainThread(() =>
                //     {
                //         Xamarin.Forms.DependencyService.Get<INativeServices>().SetToolBarColor(color);
                //     });
                // });
                // //if (string.IsNullOrEmpty(this.AssignmentsText))
                // //{
                // //    this.AssignmentsText = "<html></html>";
                // //}
                // TEditorResponse response = await CrossTEditor.Current.ShowTEditor(this.AssignmentsText,characterLimit:LimitCount);
                // IsNavigationBarVisible = false;
                // if (response.IsSave)
                // {
                //     if (response.HTML != null)
                //     {
                //         if (Device.RuntimePlatform == Device.Android)
                //             this.AssignmentsText = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" + response.HTML + "</body></html>";
                //         else
                //             this.AssignmentsText = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" + response.HTML + "</body></html>";
                //     }
                // }
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
        private void CourseSelection()
        {
            CourseListVisibility = !CourseListVisibility;
            IsClassesSelected = false;
        }
        private void CourseListTapped(PickListItem pickListItem)
        {
            if(pickListItem!=null)
            {
                SelectedCourse = pickListItem;
                CourseListVisibility = false;
                StudentsList = new ObservableCollection<BindableAgendaClassStudentView>();
                StudentListViewHeight = StudentsList != null && StudentsList.Count > 0 ? (StudentsList.Count % 2 == 0 ? (StudentsList.Count / 2) * 60 : (StudentsList.Count / 2) * 60) + 60 : StudentListViewHeight;
                StudentListVisibility = StudentList != null && StudentList.Count > 0;
            }
        }
        async void UsersIconClicked(BindableAgendaClassView classes)
        {
            if(classes!=null)
            {
                try
                {
                    int? agendaId=null;
                    short? classId=null;
                    bool isElective=false;

                    if (IsEditMode)
                    {
                        agendaId= SelectedAgenda.AgendaId;
                    }
                        classId = classes.ClassId;
                        isElective = classes.IsElective;
                        ClassStudentsData = await ApiHelper.GetObjectList<AgendaClassStudentView>(string.Format(TextResource.GetClassStudentsApi,agendaId,classId,isElective));
                    if (ClassStudentsData != null)
                    {
                        StudentsList = _mapper.Map<ObservableCollection<BindableAgendaClassStudentView>>(ClassStudentsData);
                        StudentListViewHeight = StudentsList != null && StudentsList.Count > 0 ? (StudentsList.Count % 2 == 0 ? (StudentsList.Count / 2) * 60 : (StudentsList.Count / 2) * 60) + 60 : StudentListViewHeight;
                        StudentListVisibility = true;
                    }
                    if(StudentsList != null)
                    {
                        var checkedStudents = from item in ClassList
                                              from student in StudentsList
                                              where item.IsChecked && (item.ClassId == student.ClassId)
                                              select student;
                        if (checkedStudents != null)
                        {
                            if(IsEditMode)
                            {
                                checkedStudents.ToList().ForEach(x => x.IsChecked = x.IsSelected);
                            }
                            else
                            {
                                checkedStudents.ToList().ForEach(x => x.IsChecked = true);
                            }
                        }
                    }
                    
                    SelectedClassName = classes.ClassName;
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                    //Crashes.TrackError(ex);
                }
            }
        }
        async void StudentSelection(BindableAgendaClassStudentView bindableAgendaClassStudentView)
        {
            try
            {
                if (bindableAgendaClassStudentView != null)
                {
                    if (StudentsList != null)
                    {
                        BindableAgendaClassStudentView selectedStudent = StudentsList.Where(x => x.StudentId == bindableAgendaClassStudentView.StudentId)?.FirstOrDefault();
                        if (selectedStudent != null)
                        {
                            selectedStudent.IsChecked = bindableAgendaClassStudentView.IsChecked;
                        }
                        if (selectedStudent.IsChecked)
                        {
                            CheckedStudentIds.Add(Convert.ToString(selectedStudent.StudentId));
                        }
                        else
                        {
                            CheckedStudentIds.Remove(Convert.ToString(selectedStudent.StudentId));
                        }
                        
                        var selectedClass = ClassList.Where(x => x.ClassId == bindableAgendaClassStudentView.ClassId)?.FirstOrDefault();
                        if (selectedClass != null)
                        {
                            bool anyStudentChecked = false;
                            foreach (var student in StudentsList)
                            {
                                if (student.IsChecked)
                                {
                                    anyStudentChecked = true;
                                    break;
                                }
                            }
                            selectedClass.IsChecked = anyStudentChecked;
                        }
                    }
                    if (IsEditMode)
                    {
                        if (!bindableAgendaClassStudentView.IsChecked)
                        {
                            //await WarningTextVisibilitySettings();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        async void ClassSelection(BindableAgendaClassView classes)
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

                        if (StudentsList != null)
                        {
                            var checkedStudents = from item in ClassList
                                from student in StudentsList
                                where item.IsChecked && (item.ClassId == student.ClassId)
                                select student;
                            if (checkedStudents != null && checkedStudents.ToList() != null)
                            {
                                if (checkedStudents.ToList().Count > 0)
                                {
                                    checkedStudents.ToList().ForEach(x =>
                                    {
                                        x.IsChecked = true;
                                        if (!CheckedStudentIds.Contains(Convert.ToString(x.StudentId)))
                                            CheckedStudentIds.Add(Convert.ToString(x.StudentId));
                                    });
                                }
                                else
                                {
                                    StudentsList.ToList().ForEach(x =>
                                    {
                                        x.IsChecked = false;
                                        if (CheckedStudentIds.Contains(Convert.ToString(x.StudentId)))
                                            CheckedStudentIds.Remove(Convert.ToString(x.StudentId));
                                    });
                                }
                            }
                        }
                        bool isAnyDeSelectedClass = ClassList.Any(x => !x.IsChecked);
                        IsClassesSelected = !isAnyDeSelectedClass;
                    }
                    if (IsEditMode)
                    {
                        if (!classes.IsChecked)
                        {
                            //await WarningTextVisibilitySettings();
                        }
                    }
                }
            }
            finally
            {
                isHandlingCheck = false; 
            }
        }
        private async void AddAttachmentClicked()
        {
            AttachmentFileView fileData = await HelperMethods.PickFileFromDevice();
            if (fileData == null)
            {
                return;
            }
            string foundSpecialChars = FindSpecialCharacters(fileData.FileName);
            if (!string.IsNullOrEmpty(foundSpecialChars))
            {
                await HelperMethods.ShowAlert(TextResource.AlertsPageTitle, "File name can't contain any of the following character(s): " + foundSpecialChars);
                return;
            }
            AttachmentFiles.AddFileToList(fileData);
            AttachmentListViewHeight = AttachmentFiles.Count * 40;
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
                    var action = await App.Current.MainPage.DisplayAlert("", TextResource.DeleteText, TextResource.YesText, TextResource.NoText);
                    if (action)
                    {
                        AttachmentFileView attachmentFile = (AttachmentFileView)obj;
                        if (attachmentFile.FileData == null)
                        {
                            var fileid = ExistingAttachmentList.Where(x => x.Attachment.Equals(attachmentFile.FileName)).SingleOrDefault().AttachmentId;
                            DeletedAttachmentFileID.Add(fileid.GetValueOrDefault());
                            DeletedAttachmentFileName.Add(attachmentFile.FileName);
                        }
                        AttachmentFiles.Remove(attachmentFile);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void CalendarIconClicked()
        {
            DatePickerVisibility = !DatePickerVisibility;
            FromDate = DatePickerVisibility?DateTime.Now.AddDays(-7):DateTime.MinValue;
            ToDate = DatePickerVisibility?DateTime.Now:DateTime.MaxValue;
        }
        private async void AttachmentClicked(BindableAgendaView sender)
        {
            AttachmentListPopupForm attachmentListPopupForm = new(_mapper, _nativeServices, Navigation)
            {
                SelectedAttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(sender.AttachmentList)
            };
            var attachmentListPopup = new AttachmentListPopup()
            {
                BindingContext = attachmentListPopupForm
            };
            SetPopupInstance(attachmentListPopup);
            await Application.Current.MainPage.ShowPopupAsync(attachmentListPopup);
        }
        public void SetPopupInstance(Popup popup)
        {
            AppSettings.Current.CurrentPopup = popup;
        }
        private async void SearchIconClicked()
        {
            await GetFileBankData();
        }
        async void SearchButtonClicked()
        {
            await GetFileBankData();    
        }
        private async void AddToPostClicked()
        {
            if(FilteredFileBankAttachmentList!=null)
            {
                var attachmentListFromFileBank = FilteredFileBankAttachmentList.Where(x => x.IsChecked).ToList();
                if(attachmentListFromFileBank != null)
                {
                    List<string> alreadyAddedAttachmentList = new List<string>();
                    foreach (var item in AttachmentFiles)
                    {
                        if(item!=null)
                        {
                            foreach (var attachment in attachmentListFromFileBank)
                            {
                                if(attachment!=null)
                                {
                                    foreach (var attachmentItem in attachment.AttachmentList)
                                    {
                                        if(attachmentItem!=null)
                                        {
                                            if (item.FileName.ToLower().Equals(attachmentItem.FileName.ToLower()))
                                            {
                                                alreadyAddedAttachmentList.Add(attachmentItem.FileName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if(alreadyAddedAttachmentList.ToList().Count>0)
                    {
                        string alreadyAddedAttachmentFilenames = string.Join(",",alreadyAddedAttachmentList);
                        AlreadyAddedAttachmentErrorPopupVisibility = true;
                        AlreadyAddedAttachmentText = string.Concat(AlreadyAddedAttachmentText, alreadyAddedAttachmentFilenames); ;
                        foreach (var item in alreadyAddedAttachmentList)
                        {
                            if(item!=null)
                            {
                              AttachmentFileView attachment= AttachmentFiles.Where(x=>x.FileName.ToLower().Equals(item.ToLower()))?.FirstOrDefault();
                                if(attachment!=null)
                                {
                                    AttachmentFiles.Remove(attachment);
                                }
                            }
                        }
                    }
                    foreach (var item in attachmentListFromFileBank.ToList())
                    {
                        if(item!=null)
                        {
                            foreach (var attachment in item.AttachmentList)
                            {
                                if(attachment!=null)
                                AttachmentFiles.Add(attachment);
                            }
                        }
                    }
                    AttachmentListViewHeight = AttachmentFiles.Count *40 ;
                }
                FilteredFileBankAttachmentList.ToList().ForEach(x => x.IsChecked = false);
            }
        }
        private async void SaveClicked()
        {
            if(isValid())
            {
                try
                {
                    string existingAttachmentIds = string.Empty;
                    AgendaEdit agendaEditPostData = new AgendaEdit();
                    var list = new List<AttachmentFileView>();
                    if (this.AttachmentFiles != null && this.AttachmentFiles.Count > 0)
                    {
                        list = new List<AttachmentFileView>(this.AttachmentFiles);
                        string[] fileNames = new string[list.Count()];
                        for (int i = 0; i < list.Count(); i++)
                        {
                            fileNames[i] = list[i].FileName;
                        }
                        agendaEditPostData.AttachmentsArray = fileNames;
                    }
                    agendaEditPostData.AgendaId = IsEditMode ? Convert.ToInt32(SelectedAgenda.AgendaId) : 0;
                    if (SelectedAgendaTypes.ItemName.ToLower().Equals("weekly"))
                    {
                        agendaEditPostData.GradeId = Convert.ToInt16(SelectedCourse.ItemId);
                    }
                    else
                    {
                        agendaEditPostData.CurriculumId = Convert.ToInt16(SelectedCourse.ItemId);
                    }
                    agendaEditPostData.TypeId = Convert.ToInt32(SelectedAgendaTypes.ItemId);
                    agendaEditPostData.DueDate = DueDate.ToString("MM/dd/yyyy hh:mm:ss tt");
                    agendaEditPostData.AgendaDescription = AssignmentsText;
                    agendaEditPostData.IsStudentSubmissionAllowed = IsAllowSubmissions;
                    agendaEditPostData.AgendaWeeklyGroupId = AgendaWeeklyGroupId;
                    agendaEditPostData.ReminderDate = AgendaWeeklyGroupId > 0 ? (DateTime?)null : ReminderDate;
                    List<BindableAgendaClassView> selectedClassIdList = ClassList.Where(x => x.IsChecked).ToList();
                    List<AgendaClassView> classList = new List<AgendaClassView>();
                    foreach (var item in selectedClassIdList)
                    {
                        if (item != null)
                        {
                            classList.Add(new AgendaClassView() { ClassId = item.ClassId, IsElective = item.IsElective }); ;
                        }
                    }
                    agendaEditPostData.ClassIdList = classList;
                    
                    string studentIdsAsString = string.Join(",", CheckedStudentIds);
                    agendaEditPostData.StudentIds = studentIdsAsString;
                    agendaEditPostData.IsEditMode = IsEditMode?true:false;
                        if (DeletedAttachmentFileID != null && DeletedAttachmentFileID.Count > 0)
                        {
                            agendaEditPostData.DeletedAttachmentsArray = new[] { string.Join(",", DeletedAttachmentFileName.Select(x => x)) };
                        }
                        if(ExistingAttachmentList != null&& ExistingAttachmentList.Count()>0)
                        {
                        existingAttachmentIds = string.Join(",", ExistingAttachmentList.Select(x => x.AttachmentId));
                        }
                    if (filebankAttachmentsIds != null)
                    {
                        existingAttachmentIds= !string.IsNullOrEmpty(existingAttachmentIds)?existingAttachmentIds+","+filebankAttachmentsIds: filebankAttachmentsIds;
                    }
                    agendaEditPostData.ExistingAttachmentIds = existingAttachmentIds;
                    OperationDetails result = await ApiHelper.PostMultiDataRequestAsync<OperationDetails>(TextResource.SaveAgendaPostDataApi, AppSettings.Current.ApiUrl, agendaEditPostData, list);
                    if(result.Success)
                    {
                        //HostScreen.Router.NavigateBack.Execute().Subscribe();
                        await Navigation.PopAsync();
                        if(IsEditMode)
                        {
                            await Navigation.PopAsync();
                            //HostScreen.Router.NavigateBack.Execute().Subscribe();
                        }
                        MessagingCenter.Send<string>("", "NavigateToCalendar");
                    }
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                    //Crashes.TrackError(ex);
                }
            }
        }
        async void CancelAgendaClicked()
        {
            try
            {
                var action = await App.Current.MainPage.DisplayAlert("", TextResource.CancelAgendaText, TextResource.YesText, TextResource.NoText);
                if(action)
                {
                    var apiUrl = string.Format(TextResource.DeleteAgendaApiUrl, SelectedAgenda.AgendaId);
                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(apiUrl);
                    if(result.Success)
                    {
                        await Navigation.PopAsync();
                        await Navigation.PopAsync();
                        MessagingCenter.Send<string>("", "NavigateToCalendar");
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        void CloseWarningClicked()
        {
            DeleteSubmissionWarningTextVisibility = false;
        }
       async void AgendaTypeChanged(object obj)
        {
           await WarningTextVisibilitySettings();
            if (SelectedAgendaTypes != null && !string.IsNullOrEmpty(SelectedAgendaTypes.ItemName))
            {
                SelectedCourse = new PickListItem();
                ClassesVisibility = false;
                IsClassesSelected = false;
                FilteredCourseList = SelectedAgendaTypes.ItemName.ToLower().Equals("weekly") ? GradeList : CourseList;
                CourseListViewHeight = FilteredCourseList.Count() * 32;
                AllowSubmissionsVisibility = true;
                switch (SelectedAgendaTypes.StudentSubmissionType.ToLower())
                {
                    case "a":
                        AllowSubmissionsSettings(true, true, false);
                        break;
                    case "o":
                        AllowSubmissionsSettings(true, false, true);
                        break;
                    case "n":
                        AllowSubmissionsSettings(false);
                        break;
                }
                if (!string.IsNullOrEmpty(SelectedAgendaTypes.DefaultTemplate))
                    AssignmentsText = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" + SelectedAgendaTypes.DefaultTemplate + "</body></html>";
            }
            
            StudentsList = new ObservableCollection<BindableAgendaClassStudentView>();
            StudentListViewHeight = StudentsList != null && StudentsList.Count > 0 ? (StudentsList.Count % 2 == 0 ? (StudentsList.Count / 2) * 60 : (StudentsList.Count / 2) * 60) + 60 : StudentListViewHeight;
            StudentListVisibility = StudentList != null && StudentList.Count > 0;
        }
        async void AgendaForChanged(PickListItem obj)
        {
            if(obj!=null)
            {
                SelectedAgendaForText = obj.ItemName;
                await WarningTextVisibilitySettings();
            }
        }
        async Task WarningTextVisibilitySettings()
        {
            try
            {
                if (IsEditMode)
                {
                    if (SelectedAgenda != null && SelectedAgenda.IsStudentSubmissionAllowed)
                    {
                        DeleteSubmissionWarningTextVisibility = true;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        void FileBankFileSelection(BindableAgendaView bindableAgendaView)
        {
            if (FilteredFileBankAttachmentList != null)
            {
                var selectedAttachment = FilteredFileBankAttachmentList.Where(x => x.AgendaId == bindableAgendaView.AgendaId)?.FirstOrDefault();
                if (selectedAttachment != null)
                {
                    selectedAttachment.IsChecked = bindableAgendaView.IsChecked;
                }
                if(bindableAgendaView.AttachmentIds!=null)
                {
                    filebankAttachmentsIds = bindableAgendaView.AttachmentIds;
                }
            }
            }
        private void IncrementReminderBeforeDaysTapped()
        {
            if (!IsEditMode)
            {
                if ((ReminderMaxDate.Date - ReminderMinimumDate.Date).Days == 0)
                {
                    return;
                }
                else
                {
                    TimeSpan timeSpan = (DueDate.Date.Subtract(ReminderMinimumDate.Date));
                    var totalDays = timeSpan.TotalDays; 
                    if(ReminderBeforeText < totalDays)
                    {
                        ReminderBeforeText++;
                        ReminderDate=ReminderDate.AddDays(-ReminderBeforeText);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                    TimeSpan timeSpan = (DueDate.Date.Subtract(ReminderMinimumDate.Date));
                    var totalDays = timeSpan.TotalDays;
                    if (ReminderBeforeText < totalDays)
                    {
                        ReminderBeforeText++;
                        ReminderMinimumDate = DateTime.Now;
                        ReminderMaxDate = DueDate.AddDays(-1);
                        ReminderDate = ReminderDate.AddDays(-ReminderBeforeText);
                    }
                    else
                    {
                        return;
                    }
            }
        }
        private void DecrementReminderBeforeDaysTapped()
        {
            if (!IsEditMode)
            {
                if ((ReminderMaxDate.Date - ReminderMinimumDate.Date).Days == 0)
                {
                    return;
                }
                else
                {
                    if (ReminderBeforeText > 0)
                    {
                        ReminderBeforeText--;
                        ReminderDate=ReminderDate.AddDays(1);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                    if (ReminderBeforeText > 0)
                    {
                        ReminderBeforeText--;
                        ReminderMinimumDate = DateTime.Now;
                        ReminderMaxDate = DueDate.AddDays(-1);
                        ReminderDate = ReminderDate.AddDays(1);
                    }
                    else
                    {
                        return;
                    }
            }
        }
        async void ClassesCheckChanged()
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
                    if(IsEditMode)
                    {
                        await  WarningTextVisibilitySettings();
                    }

                    StudentsList = new ObservableCollection<BindableAgendaClassStudentView>();
                    StudentListViewHeight = StudentsList != null && StudentsList.Count > 0 ? (StudentsList.Count % 2 == 0 ? (StudentsList.Count / 2) * 60 : (StudentsList.Count / 2) * 60) + 60 : StudentListViewHeight;
                    StudentListVisibility = StudentList != null && StudentList.Count > 0;
                }
            }
            finally
            {
                isHandlingCheck = false; 
            }
        }
        bool isValid()
        {
            if (string.IsNullOrEmpty(SelectedAgendaTypes.ItemName))
            {
                IsAgendaTypeErrorVisible = true;
            }
            else if (string.IsNullOrEmpty(SelectedCourse.ItemName))
            {
                IsAgendaTypeErrorVisible = false;
                IsAgendaForErrorVisible = true;
            }
            else if (ClassList != null)
            {
                var classData = ClassList.Where(x=>x.IsChecked).ToList();
                if (classData==null||classData.Count()<=0)
                {
                    IsAgendaTypeErrorVisible = false;
                    IsAgendaForErrorVisible = false;
                    ClassSelectionErrorVisibility = true;
                }
                else if(classData != null && classData.Count() > 0)
                {
                    ClassSelectionErrorVisibility = false;
                    IsAgendaTypeErrorVisible = false;
                    IsAgendaForErrorVisible = false;
                }
            }
            if(IsAgendaTypeErrorVisible!=true && IsAgendaForErrorVisible!=true && ClassSelectionErrorVisibility!=true)
            {
                return true;
            }
            return false;
        }
       public async Task AddDataSettings(AgendaEdit addPostData)
        {
            try
            {
                AddPostData = addPostData != null ? addPostData : new AgendaEdit();
                if(AddPostData!=null)
                {
                    AgendaTypes= AddPostData.AgendaTypeList!=null&& AddPostData.AgendaTypeList.Count()>0?AddPostData.AgendaTypeList:new List<CalendarAgendaTypePickListItem>();
                    CourseList = AddPostData.CourseList != null && AddPostData.CourseList.Count() > 0 ? AddPostData.CourseList : new List<PickListItem>();
                    GradeList = AddPostData.GradeList != null && AddPostData.GradeList.Count() > 0 ? AddPostData.GradeList : new List<PickListItem>();
                    AppSettings.Current.IsWeekendsDisabled = AddPostData.CalendarControlSetting != null ? AddPostData.CalendarControlSetting.IsWeekendDisabled : false;
                    AppSettings.Current.WeekendDays = AddPostData.WeekendDays;
                    AppSettings.Current.IsCurrentWeekDisabled = addPostData.CalendarControlSetting.IsCurrentWeekDisabled;
                    AssignmentLabel = !string.IsNullOrEmpty(addPostData.CalendarControlSetting.AssignmentsLabel) ? addPostData.CalendarControlSetting.AssignmentsLabel : TextResource.AssignmentLabel;
                    IsLearningOutcomeVisible = addPostData.CalendarControlSetting.EnableLearningOutcomes;

                    if (IsLearningOutcomeVisible)
                    {
                        LearningOutcomeLabel = !string.IsNullOrEmpty(addPostData.CalendarControlSetting?.LearningOutcomesLabel?.Trim()) ? addPostData.CalendarControlSetting.LearningOutcomesLabel : TextResource.LearningOutcomeLabel;
                    }
                    if (addPostData.CalendarControlSetting.IsEnableLimit)
                    {
                        LimitCount = addPostData.CalendarControlSetting.LimitCount;
                    }
                }
                if (AppSettings.Current.IsWeekendsDisabled)
                {
                    int count = 0;
                    if (!string.IsNullOrEmpty(AppSettings.Current.WeekendDays))
                    {
                        do
                        {
                            count++;
                        }
                        while (AppSettings.Current.WeekendDays.Contains(((int)DateTime.Now.AddDays(count).DayOfWeek).ToString()));
                    }
                    daysTobeAdded = count;
                }
                else
                {
                    daysTobeAdded = 1;
                }
                DueDate = DateTime.Now.AddDays(daysTobeAdded);
                ReminderDate = DueDate.AddDays(-1);
                ReminderMaxDate = ReminderDate;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
       public async void EditDataSettings(BindableAgendaView selectedAgenda)
        {
            try
            {
                ReminderMinimumDate = DateTime.MinValue;
                ReminderMaxDate = DateTime.MaxValue;
                if(selectedAgenda!=null)
                {
                    SelectedAgenda = selectedAgenda;
                    DueDate = selectedAgenda.DueDate.ToDateTime();
                    ReminderDate = selectedAgenda.ReminderDate.ToDateTime();
                    ReminderMaxDate = ReminderDate;
                    ReminderBeforeText = (DueDate - ReminderDate).Days;
                    if(SelectedAgenda.AttachmentList!=null&& SelectedAgenda.AttachmentList.Count>0)
                    {
                        AttachmentFiles = new ObservableCollection<AttachmentFileView>(SelectedAgenda.AttachmentList);
                        AttachmentListViewHeight = AttachmentFiles.Count * 40;
                    }
                    await GetClassesForAgenda(selectedAgenda);
                    if(ClassList!=null)
                    {
                        var classData = ClassList.Where(x => !x.IsChecked);
                        IsClassesSelected = classData != null && classData.Count() > 0 ? false : true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        async Task GetClassesForAgenda(BindableAgendaView selectedAgenda)
        {
            try
            {
                string gradeid=string.Empty;
                string curriculumId = string.Empty;
                string agendaID=string.Empty;
                bool isWeekly=false;
                if (IsEditMode)
                {
                    agendaID =  selectedAgenda.AgendaId.ToString();
                    isWeekly = selectedAgenda.IsWeekly;
                    gradeid = selectedAgenda.IsWeekly ? selectedAgenda.GradeId.ToString() : null;
                    curriculumId = selectedAgenda.IsWeekly ? null : selectedAgenda.CurriculumId.ToString();
                }
                else
                {
                    agendaID =  null;
                    isWeekly = SelectedAgendaTypes != null && SelectedAgendaTypes.ItemName.ToLower().Equals("weekly") ? true : false;
                    gradeid = isWeekly ? SelectedCourse.ItemId : null;
                    curriculumId = isWeekly ? null : SelectedCourse.ItemId ;
                }
                AgendaClassData = await ApiHelper.GetObject<AgendaClassModel>(string.Format(TextResource.GetClassesForAgendaApi, agendaID, isWeekly, null, gradeid, curriculumId, null));
                if (AgendaClassData != null)
                {
                    ClassList = AgendaClassData.AgendaClassList != null ? _mapper.Map<ObservableCollection<BindableAgendaClassView>>(AgendaClassData.AgendaClassList) : new ObservableCollection<BindableAgendaClassView>();
                    if(ClassList!=null)
                    {
                        ClassListViewHeight = ClassList != null && ClassList.Count > 0 ? (ClassList.Count % 3 == 0 ? (ClassList.Count / 3) * 70 : (ClassList.Count / 3) * 70) : ClassListViewHeight;
                        ClassesVisibility = true;
                        foreach (var item in ClassList)
                        {
                            item.IsAgendaPerStudent = AgendaClassData.IsAgendaPerStudent;
                        }
                    }
                }
                foreach (var item in ClassList)
                {
                    if(item.IsChecked)
                    {
                        ClassStudentsData = await ApiHelper.GetObjectList<AgendaClassStudentView>(string.Format(TextResource.GetClassStudentsApi, agendaID, item.ClassId, item.IsElective), isLoader: false);
                        if (ClassStudentsData != null)
                        {
                            StudentsList = _mapper.Map<ObservableCollection<BindableAgendaClassStudentView>>(ClassStudentsData);
                        }
                        var selectedStudents = StudentsList.Where(student => student.IsSelected);
                        foreach (var student in selectedStudents)
                        {
                            if (!CheckedStudentIds.Contains(Convert.ToString(student.StudentId)))
                            {
                                CheckedStudentIds.Add(Convert.ToString(student.StudentId));
                            }
                        }
                    }
                }
                StudentsList = new ObservableCollection<BindableAgendaClassStudentView>();

            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        
        public void AllowSubmissionsSettings(bool allowSubmissionsVisibility=false,bool isAllowSubmissions = false, bool isAllowSubmissionsEnabled = false)
        {
            
            AllowSubmissionsVisibility = allowSubmissionsVisibility;
            IsAllowSubmissions = isAllowSubmissions;
            IsAllowSubmissionsEnabled = isAllowSubmissionsEnabled;
        }
        async Task GetClassesInAddMode()
        {
            await GetClassesForAgenda(null);
        }
        async Task DeleteSubmissionData()
        {
            DeleteSubmissionWarningTextVisibility = true;
        }
        async void AllowSubmissionsCheckChanged()
        {
            if(IsEditMode)
            {
               await WarningTextVisibilitySettings();
            }
        }
        async Task GetFileBankData()
        {
            try
            {
                int? studentId=null;
                DateTime? fromDate=FromDate;
                DateTime? toDate=ToDate;
                string searchValue= FileBankSearchText;
                FileBankAttachmentList = new ObservableCollection<BindableAgendaView>(await ApiHelper.GetObjectList<BindableAgendaView>(string.Format(TextResource.GetCalendarAgendaAttachmentFilesApi,studentId,fromDate,toDate,searchValue),_isEnableCaching));
                FileBankAttachmentList = new ObservableCollection<BindableAgendaView>(FileBankAttachmentList.Where(x => x.AttachmentList!=null&&x.AttachmentList.Count > 0)?.ToList());
                FilteredFileBankAttachmentList = FileBankAttachmentList;
                IsFileBankAttachmentNotAvailable = FileBankAttachmentList != null && FileBankAttachmentList.Count > 0 ? false : true;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        void CloseAlreadyAddedAttachedErrorPopupClicked()
        {
            AlreadyAddedAttachmentErrorPopupVisibility = false;
        }

        #endregion Methods
    }