using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Resources;

public class ResourcesFilterForm : ViewModelBase
{
    #region Declarations
    private Popup _currentPopup;
    public ICommand SearchClickCommand { get; set; }
    public ICommand ArrowClickedCommand { get; set; }
    public ICommand AttachmentClickCommand { get; set; }

    #endregion

    #region Properties

    private ResourceViewModel _resourceData = new();

    public ResourceViewModel ResourceData
    {
        get => _resourceData;
        set
        {
            _resourceData = value;
            OnPropertyChanged(nameof(ResourceData));
        }
    }

    private ObservableCollection<BindableResourceView> _resourceList = new();

    public ObservableCollection<BindableResourceView> ResourceList
    {
        get => _resourceList;
        set
        {
            _resourceList = value;
            OnPropertyChanged(nameof(ResourceList));
        }
    }

    private ObservableCollection<BindableResourceView> _filteredResourceList = new();

    public ObservableCollection<BindableResourceView> FilteredResourceList
    {
        get => _filteredResourceList;
        set
        {
            _filteredResourceList = value;
            OnPropertyChanged(nameof(FilteredResourceList));
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

    private CurriculumView _selectedCourse = new();

    public CurriculumView SelectedCourse
    {
        get => _selectedCourse;
        set
        {
            _selectedCourse = value;
            OnPropertyChanged(nameof(SelectedCourse));
        }
    }

    private DateTime _fromDate = DateTime.Now;

    public DateTime FromDate
    {
        get => _fromDate;
        set
        {
            if (value <= ToDate)
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
                DateErrorMessageVisibility = false;
            }
            else
            {
                DateErrorMessageVisibility = true;
            }
        }
    }

    private DateTime _toDate = DateTime.Now;

    public DateTime ToDate
    {
        get => _toDate;
        set
        {
            if (value >= FromDate)
            {
                _toDate = value;
                OnPropertyChanged(nameof(ToDate));
                DateErrorMessageVisibility = false;
            }
            else
            {
                DateErrorMessageVisibility = true;
            }
        }
    }

    private bool _isNoRecordMsg = false;

    public bool IsNoRecordMsg
    {
        get => _isNoRecordMsg;
        set
        {
            _isNoRecordMsg = value;
            OnPropertyChanged(nameof(IsNoRecordMsg));
        }
    }

    private bool _dateErrorMessageVisibility = false;

    public bool DateErrorMessageVisibility
    {
        get => _dateErrorMessageVisibility;
        set
        {
            _dateErrorMessageVisibility = value;
            OnPropertyChanged(nameof(DateErrorMessageVisibility));
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

    #endregion

    public ResourcesFilterForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper,
        null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        SearchClickCommand = new Command(SearchClicked);
        ArrowClickedCommand = new Command<BindableResourceView>(ArrowClicked);
        AttachmentClickCommand = new Command<BindableResourceView>(AttachmentClicked);

        GetResourceData();
    }

    #region Methods

    public override async void GetStudentData()
    {
        base.GetStudentData();
        await GetResourceData();
        SearchClicked();
    }

    private async void SearchClicked()
    {
        var curriculumId = SelectedCourse != null && SelectedCourse.CurriculumId != 0
            ? SelectedCourse.CurriculumId.ToString()
            : null;
        var termId = SelectedTerm != null && !string.IsNullOrEmpty(SelectedTerm.ItemId)
            ? SelectedTerm.ItemId.ToString()
            : null;
        var filteredList = _resourceList
            .Where(resource =>
                (curriculumId == null || resource.CurriculumId?.ToString() == curriculumId) &&
                (termId == null || resource.TermId?.ToString() == termId) &&
                DateTime.Parse(resource.Date).Date >= FromDate.Date &&
                DateTime.Parse(resource.Date).Date <= ToDate.Date)
            .ToList();
        FilteredResourceList = new ObservableCollection<BindableResourceView>(filteredList);
        IsNoRecordMsg = FilteredResourceList != null && FilteredResourceList.Count > 0 ? false : true;
    }

    public async Task GetResourceData()
    {
        try
        {
            var cacheKeyPrefix = "resources";
            var curriculumId = SelectedCourse?.CurriculumId == 0 ? null : SelectedCourse?.CurriculumId.ToString();
            var loadFilterPanelLists = !TermList.Any();
            var apiUrl = string.Format(TextResource.ResourcesDataAPIUrl, AppSettings.Current.SelectedStudent.ItemId, "",
                "", "", "", loadFilterPanelLists);
            ResourceData = await ApiHelper.GetObject<ResourceViewModel>(apiUrl, cacheKeyPrefix: cacheKeyPrefix,
                cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
            if (ResourceData != null)
            {
                ResourceList = ResourceData.ResourcesList != null && ResourceData.ResourcesList.Count() > 0
                    ? ResourceList = _mapper.Map<ObservableCollection<BindableResourceView>>(
                        new ObservableCollection<BindableResourceView>(
                            _mapper.Map<List<BindableResourceView>>(ResourceData.ResourcesList)))
                    : new ObservableCollection<BindableResourceView>();
                if (loadFilterPanelLists)
                {
                    TermList = ResourceData.Terms != null ? ResourceData.Terms.ToList() : new List<PickListItem>();
                    CourseList = ResourceData.Courses != null
                        ? ResourceData.Courses.ToList()
                        : new List<CurriculumView>();
                }

                var sortedResources = ResourceList.OrderByDescending(r => DateTime.Parse(r.Date));
                ResourceList = new ObservableCollection<BindableResourceView>(sortedResources);
                foreach (var item in ResourceList) item.IsCourseVisible = item.CurriculumId != null ? true : false;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            //Crashes.TrackError(ex);
        }

        IsNoRecordMsg = ResourceList != null && ResourceList.Count > 0 ? false : true;
    }

    public void SetList()
    {
        var tempCourseList = new List<CurriculumView>
        {
            new()
            {
                CurriculumName = "All Courses",
                CurriculumId = 000
            }
        };
        var tempTermList = new List<PickListItem>
        {
            new()
            {
                ItemName = "All Terms",
                ItemId = ""
            }
        };

        tempCourseList.AddRange(CourseList);
        CourseList = tempCourseList;
        tempTermList.AddRange(TermList);
        TermList = tempTermList;
    }

    private async void ArrowClicked(BindableResourceView bindableResourceView)
    {
        if (bindableResourceView != null)
            foreach (var item in ResourceList.ToList())
                if (item != null)
                {
                    if (item.ResourceId == bindableResourceView.ResourceId)
                    {
                        item.DescriptionVisibility = !item.DescriptionVisibility;
                        item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png")
                            ? "dropdown_gray.png"
                            : "uparrow_gray.png";
                    }
                    else
                    {
                        item.DescriptionVisibility = false;
                        item.ArrowImageSource = "dropdown_gray.png";
                    }
                }
                    
        

        MessagingCenter.Send("", "ExpandCollapse");
    }

    private async void AttachmentClicked(BindableResourceView sender)
    {
        AttachmentListPopupForm attachmentListPopupForm = new(_mapper, _nativeServices, Navigation)
        {
            SelectedAttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(sender.AttachmentFileList)
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
        _currentPopup = popup;
    }

    #endregion
}