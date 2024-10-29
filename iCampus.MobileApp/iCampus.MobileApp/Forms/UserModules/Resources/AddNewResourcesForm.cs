using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Resources;

public class AddNewResourcesForm : ViewModelBase
{
    #region Declarations

    public ICommand SelectFileCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    public ICommand DeleteAttachmentClickCommand { get; set; }

    #endregion

    #region Properties

    private bool _isSelectAll;

    public bool IsSelectAll
    {
        get => _isSelectAll;
        set
        {
            _isSelectAll = value;
            OnPropertyChanged(nameof(IsSelectAll));
        }
    }

    private bool _isPublishSelected;

    public bool IsPublishSelected
    {
        get => _isPublishSelected;
        set
        {
            _isPublishSelected = value;
            OnPropertyChanged(nameof(IsPublishSelected));
        }
    }

    private ObservableCollection<BindableResourcesPickListItem> _classesList = new();

    public ObservableCollection<BindableResourcesPickListItem> ClassesList
    {
        get => _classesList;
        set
        {
            _classesList = value;
            OnPropertyChanged(nameof(ClassesList));
            if (_classesList != null && _classesList.Count > 0) ClassSelectionVisibility = true;
        }
    }

    private List<BindableResourcesPickListItem> _classList = new();

    public List<BindableResourcesPickListItem> ClassList
    {
        get => _classList;
        set
        {
            _classList = value;
            OnPropertyChanged(nameof(ClassList));
        }
    }

    private BindableResourcesPickListItem _selectedClass = new BindableResourcesPickListItem();

    public BindableResourcesPickListItem SelectedClass
    {
        get => _selectedClass;
        set
        {
            _selectedClass = value;
            OnPropertyChanged(nameof(SelectedClass));
        }
    }

    private List<PickListItem> _gradeList = new();

    public List<PickListItem> GradeList
    {
        get => _gradeList;
        set
        {
            _gradeList = value;
            OnPropertyChanged(nameof(GradeList));
        }
    }

    private PickListItem _selectedGrade = new();

    public PickListItem SelectedGrade
    {
        get => _selectedGrade;
        set
        {
            _selectedGrade = value;
            OnPropertyChanged(nameof(SelectedGrade));
            if (_selectedGrade != null && !string.IsNullOrEmpty(_selectedGrade.ItemName))
            {
                if (!(PreviousSelectedGrade.ItemId == SelectedGrade.ItemId)) SelectedCourse = new CurriculumView();
                GradeErrorMessageVisibility = false;
                GetCourseListByGrade(SelectedGrade);
            }
        }
    }

    private List<PickListItem> _termList = new();

    public List<PickListItem> TermList
    {
        get => _termList;
        set
        {
            _termList = value;
            OnPropertyChanged(nameof(TermList));
        }
    }

    private PickListItem _selectedTerm = new();

    public PickListItem SelectedTerm
    {
        get => _selectedTerm;
        set
        {
            _selectedTerm = value;
            OnPropertyChanged(nameof(SelectedTerm));
        }
    }

    private bool _classSelectionVisibility;

    public bool ClassSelectionVisibility
    {
        get => _classSelectionVisibility;
        set
        {
            _classSelectionVisibility = value;
            OnPropertyChanged(nameof(ClassSelectionVisibility));
        }
    }

    private int _collectionViewHeight;

    public int CollectionViewHeight
    {
        get => _collectionViewHeight;
        set
        {
            _collectionViewHeight = value;
            OnPropertyChanged(nameof(CollectionViewHeight));
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

    private BindableResourceView _selectedResource = new BindableResourceView();

    public BindableResourceView SelectedResource
    {
        get => _selectedResource;
        set
        {
            _selectedResource = value;
            OnPropertyChanged(nameof(SelectedResource));
        }
    }

    private string _title;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    private string _data;

    public string Data
    {
        get => _data;
        set
        {
            _data = value;
            OnPropertyChanged(nameof(Data));
        }
    }

    private bool _titleErrorMessageVisibility;

    public bool TitleErrorMessageVisibility
    {
        get => _titleErrorMessageVisibility;
        set
        {
            _titleErrorMessageVisibility = value;
            OnPropertyChanged(nameof(TitleErrorMessageVisibility));
        }
    }

    private bool _classErrorMessageVisibility;

    public bool ClassErrorMessageVisibility
    {
        get => _classErrorMessageVisibility;
        set
        {
            _classErrorMessageVisibility = value;
            OnPropertyChanged(nameof(ClassErrorMessageVisibility));
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

    private List<ResourceClassView> _selectedClassList = new();

    public List<ResourceClassView> SelectedClassList
    {
        get => _selectedClassList;
        set
        {
            _selectedClassList = value;
            OnPropertyChanged(nameof(SelectedClassList));
        }
    }

    private List<string> _deletedAttachmentFileName = new();

    public List<string> DeletedAttachmentFileName
    {
        get => _deletedAttachmentFileName;
        set
        {
            _deletedAttachmentFileName = value;
            OnPropertyChanged(nameof(DeletedAttachmentFileName));
        }
    }

    private List<int> _deletedAttachmentFileID = new();

    public List<int> DeletedAttachmentFileID
    {
        get => _deletedAttachmentFileID;
        set
        {
            _deletedAttachmentFileID = value;
            OnPropertyChanged(nameof(DeletedAttachmentFileID));
        }
    }

    private List<CurriculumView> _courseList = new();

    public List<CurriculumView> CourseList
    {
        get => _courseList;
        set
        {
            _courseList = value;
            OnPropertyChanged(nameof(CourseList));
        }
    }

    private List<CurriculumView> _filterCourseList = new();

    public List<CurriculumView> FilterCourseList
    {
        get => _filterCourseList;
        set
        {
            _filterCourseList = value;
            OnPropertyChanged(nameof(FilterCourseList));
        }
    }

    private CurriculumView _selectedCourse = new();

    public CurriculumView SelectedCourse
    {
        get => _selectedCourse;
        set
        {
            if (value != null && !value.Equals(_selectedCourse))
            {
                _selectedCourse = value;
                OnPropertyChanged(nameof(SelectedCourse));
                if (_selectedCourse.CurriculumId != 0)
                {
                    CourseErrorMessageVisibility = false;
                    GetClassListByGrade();
                }
            }
        }
    }

    private bool _isEditMode;

    public bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            _isEditMode = value;
            OnPropertyChanged(nameof(IsEditMode));
        }
    }

    private int _count = 1;

    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            OnPropertyChanged(nameof(Count));
        }
    }

    private PickListItem _previousSelectedGrade = new();

    public PickListItem PreviousSelectedGrade
    {
        get => _previousSelectedGrade;
        set
        {
            _previousSelectedGrade = value;
            OnPropertyChanged(nameof(PreviousSelectedGrade));
        }
    }

    private CurriculumView _previousSelectedCourse = new();

    public CurriculumView PreviousSelectedCourse
    {
        get => _previousSelectedCourse;
        set
        {
            _previousSelectedCourse = value;
            OnPropertyChanged(nameof(PreviousSelectedCourse));
        }
    }

    private bool _gradeErrorMessageVisibility;

    public bool GradeErrorMessageVisibility
    {
        get => _gradeErrorMessageVisibility;
        set
        {
            _gradeErrorMessageVisibility = value;
            OnPropertyChanged(nameof(GradeErrorMessageVisibility));
        }
    }

    private bool _courseErrorMessageVisibility;

    public bool CourseErrorMessageVisibility
    {
        get => _courseErrorMessageVisibility;
        set
        {
            _courseErrorMessageVisibility = value;
            OnPropertyChanged(nameof(CourseErrorMessageVisibility));
        }
    }

    private bool _termErrorMessageVisibility;

    public bool TermErrorMessageVisibility
    {
        get => _termErrorMessageVisibility;
        set
        {
            _termErrorMessageVisibility = value;
            OnPropertyChanged(nameof(TermErrorMessageVisibility));
        }
    }

    #endregion

    public AddNewResourcesForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper,
        null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods

    public override void GetStudentData()
    {
        base.GetStudentData();
    }

    private async void InitializePage()
    {
        SelectFileCommand = new Command(AddAttachmentClicked);
        SaveCommand = new Command(SaveClicked);
        DeleteAttachmentClickCommand = new Command(DeleteAttachmentClicked);
        MandatoryFieldErrorMessage = TextResource.MandatoryFieldErrorMessage;
        MessagingCenter.Subscribe<BindableResourcesPickListItem>(this, "UpdateClassSelection", async (args) =>
        {
            if (args != null)
            {
                BindableResourcesPickListItem bindableResourcesPickListItem = (BindableResourcesPickListItem)args;
                var selectedClass = ClassesList.Where(x => x.ItemName.Equals(bindableResourcesPickListItem.ItemName))
                    ?.FirstOrDefault();
                if (selectedClass != null) selectedClass.IsSelected = bindableResourcesPickListItem.IsSelected;
            }
        });
        MessagingCenter.Subscribe<string>("", "AllClassSelection", async (args) =>
        {
            if (args != null)
            {
                if (args.ToString().ToLower().Equals("true"))
                    ClassesList.ToList().ForEach(x => x.IsSelected = true);
                else
                    ClassesList.ToList().ForEach(x => x.IsSelected = false);
            }
        });

        MessagingCenter.Subscribe<string>("", "PublishSelection", async (args) =>
        {
            if (args != null) IsPublishSelected = args.ToString().ToLower().Equals("true") ? true : false;
        });
    }


    private async void AddAttachmentClicked()
    {
        try
        {
            var fileData = await HelperMethods.PickFileFromDevice();
            AttachmentFiles.AddFileToList(fileData);
            AttachmentListViewHeight = AttachmentFiles.Count * 40;
            MessagingCenter.Send("", "UpdateAttachmentListView");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async void SaveClicked()
    {
        if (!ValidateData())
            try
            {
                SelectedResource.Data = Data;
                SelectedResource.Title_1 = Title;
                SelectedResource.GradeId = SelectedGrade != null && SelectedGrade.ItemId != null
                    ? Convert.ToInt32(SelectedGrade.ItemId)
                    : 0;
                SelectedResource.TermId = SelectedTerm != null && SelectedTerm.ItemId != null
                    ? Convert.ToInt32(SelectedTerm.ItemId)
                    : 0;
                SelectedResource.IsPublished = IsPublishSelected;
                SelectedResource.CurriculumId = SelectedCourse.CurriculumId;
                List<BindableResourcesPickListItem> selectedClassesList = ClassesList != null && ClassesList.Count() > 0
                    ? ClassesList.Where(x => x.IsSelected == true)?.ToList()
                    : new List<BindableResourcesPickListItem>();
                SelectedResource.SelectedClasses = SelectedGrade.ItemId == PreviousSelectedGrade.ItemId &&
                                                   SelectedCourse.CurriculumId == PreviousSelectedCourse.CurriculumId
                    ? string.Join(",", SelectedClassList.Select(x => x.ClassId))
                    : string.Join(",", selectedClassesList.Select(x => x.ItemId));

                var list = new List<AttachmentFileView>();
                if (AttachmentFiles != null && AttachmentFiles.Count > 0)
                {
                    list = new List<AttachmentFileView>(AttachmentFiles);
                    var fileNames = new string[list.Count()];
                    for (var i = 0; i < list.Count(); i++) fileNames[i] = list[i].FileName;
                    SelectedResource.AttachmentsArray = fileNames;
                }
                else
                {
                    SelectedResource.AttachmentsArray = new string[] { };
                }

                if (DeletedAttachmentFileID != null && DeletedAttachmentFileID.Count > 0)
                    SelectedResource.DeletedAttachmentsArray = new[]
                        { string.Join(",", DeletedAttachmentFileName.Select(x => x)) };
                else
                    SelectedResource.DeletedAttachmentsArray = new string[] { };
                if (SelectedResource.AttachmentList != null)
                    SelectedResource.ExistingAttachmentIds = string.Join(",",
                        SelectedResource.AttachmentList.Select(x => x.AttachmentId));
                var result = await ApiHelper.PostMultiDataRequestAsync<bool>(TextResource.UpdateResourceDataApiUrl,
                    AppSettings.Current.ApiUrl, SelectedResource, list);
                if (result)
                {
                    MessagingCenter.Send<AddNewResourcesForm>(this, "UpdateResourcesData");
                    await HelperMethods.ShowAlert("", PageTitle + " " + TextResource.ResourceSaveSuccessMessage);
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
                //Crashes.TrackError(ex);
            }
    }

    private async void DeleteAttachmentClicked(object obj)
    {
        if (obj != null)
        {
            var action = await Application.Current.MainPage.DisplayAlert("", TextResource.DeleteText,
                TextResource.YesText, TextResource.NoText);
            if (action)
            {
                var attachmentFile = (AttachmentFileView)obj;
                if (attachmentFile.FileData == null)
                {
                    var fileid = SelectedResource.AttachmentList
                        .Where(x => x.AttachmentFile.Equals(attachmentFile.FileName)).SingleOrDefault().AttachmentId;
                    DeletedAttachmentFileID.Add(fileid);
                    DeletedAttachmentFileName.Add(attachmentFile.FileName);
                }

                AttachmentFiles.Remove(attachmentFile);
            }
        }
    }

    private async void GetClassListByGrade()
    {
        try
        {
            var curriculumId = SelectedCourse != null && SelectedCourse.CurriculumId == 0
                ? null
                : SelectedCourse?.CurriculumId.ToString();
            var classList = await ApiHelper.GetObjectList<BindableResourcesPickListItem>(
                string.Format(TextResource.GetTeacherClassesByGradeApiUrl, Convert.ToInt16(SelectedGrade.ItemId),
                    Convert.ToInt16(curriculumId)));
            ClassList = _mapper.Map<List<BindableResourcesPickListItem>>(classList);
            ClassesList = new ObservableCollection<BindableResourcesPickListItem>(ClassList);
            CollectionViewHeight = ClassesList != null && ClassesList.Count > 0
                ? (ClassList.Count % 3 == 0 ? ClassList.Count / 3 * 50 : ClassList.Count / 3 * 50)
                : CollectionViewHeight;
            if (SelectedClassList != null && SelectedClassList.Count > 0)
            {
                var selectedClassList = ClassesList
                    .Where(x => SelectedClassList.Any(y => y.ClassId.ToString() == x.ItemId)).ToList();
                if (selectedClassList != null) selectedClassList.ForEach(x => x.IsSelected = true);
                var unSelectedClass = ClassList.Where(x => x.IsSelected == false)?.ToList();
                IsSelectAll = unSelectedClass != null && unSelectedClass.Count() > 0 ? false : true;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            //Crashes.TrackError(ex);
        }
    }

    private async void GetCourseListByGrade(PickListItem selectedGrade)
    {
        try
        {
            if (!IsEditMode && selectedGrade != null && selectedGrade.ItemId != null && CourseList != null)
            {
                FilterCourseList = CourseList.Where(x => x.GradeId.ToString().Equals(selectedGrade.ItemId)).ToList();
                ClassSelectionVisibility = false;
            }

            Count++;
            if (IsEditMode && Count == 3) IsEditMode = false;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            //Crashes.TrackError(ex);
        }
    }

    private bool ValidateData()
    {
        try
        {
            GradeErrorMessageVisibility = SelectedGrade.ItemId == null;
            CourseErrorMessageVisibility = SelectedCourse.CurriculumId == 0;
            TermErrorMessageVisibility = SelectedTerm.ItemId == null;
            List<BindableResourcesPickListItem> selectedClassesList = ClassesList != null && ClassesList.Count() > 0
                ? ClassesList.Where(x => x.IsSelected == true)?.ToList()
                : new List<BindableResourcesPickListItem>();
            ClassErrorMessageVisibility = SelectedGrade.ItemId == PreviousSelectedGrade.ItemId &&
                                          SelectedCourse.CurriculumId == PreviousSelectedCourse.CurriculumId
                ? !SelectedClassList.Any()
                : !selectedClassesList.Any();
            TitleErrorMessageVisibility = !string.IsNullOrEmpty(Title) ? false : true;
            var isValidate = ClassErrorMessageVisibility || TitleErrorMessageVisibility ||
                             GradeErrorMessageVisibility || CourseErrorMessageVisibility || TermErrorMessageVisibility;
            return isValidate;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            //Crashes.TrackError(ex);
            return true;
        }
    }

    #endregion
}