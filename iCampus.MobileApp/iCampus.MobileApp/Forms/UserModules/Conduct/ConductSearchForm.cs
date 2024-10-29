using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Conduct;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Conduct;

public class ConductSearchForm : ViewModelBase
{
    #region Declaration

    public ICommand SearchClickCommand { get; set; }
    public ICommand ListTappedCommand { get; set; }

    #endregion

    #region Properties

    private StudentConductViewModel _studentConductData = new();

    public StudentConductViewModel StudentConductData
    {
        get => _studentConductData;
        set
        {
            _studentConductData = value;
            OnPropertyChanged(nameof(StudentConductData));
        }
    }

    private StudentConductViewModel _finalStudentConductData = new();

    public StudentConductViewModel FinalStudentConductData
    {
        get => _finalStudentConductData;
        set
        {
            _finalStudentConductData = value;
            OnPropertyChanged(nameof(FinalStudentConductData));
        }
    }

    private StudentConductViewModel _filterStudentConductData = new();

    public StudentConductViewModel FilterStudentConductData
    {
        get => _filterStudentConductData;
        set
        {
            _filterStudentConductData = value;
            OnPropertyChanged(nameof(FilterStudentConductData));
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

    private bool _hasConductData;

    public bool HasConductData
    {
        get => _hasConductData;
        set
        {
            _hasConductData = value;
            OnPropertyChanged(nameof(HasConductData));
        }
    }

    private string _conductType = string.Empty;

    public string ConductType
    {
        get => _conductType;
        set
        {
            _conductType = value;
            OnPropertyChanged(nameof(ConductType));
        }
    }

    private ExtPickListItem _selectedConductType = new();

    public ExtPickListItem SelectedConductType
    {
        get => _selectedConductType;
        set
        {
            _selectedConductType = value;
            OnPropertyChanged(nameof(SelectedConductType));
            if (!string.IsNullOrEmpty(SelectedConductType.ItemName))
                IsConductTypeErrVisible = false;
        }
    }

    private IList<ExtPickListItem> _conductTypeList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> ConductTypeList
    {
        get => _conductTypeList;
        set
        {
            _conductTypeList = value;
            OnPropertyChanged(nameof(ConductTypeList));
        }
    }

    private string _conductPageTitle;

    public string ConductPageTitle
    {
        get => _conductPageTitle;
        set
        {
            _conductPageTitle = value;
            OnPropertyChanged(nameof(ConductPageTitle));
        }
    }

    private StudentConductView _selectedConduct = new();

    public StudentConductView SelectedConduct
    {
        get => _selectedConduct;
        set
        {
            _selectedConduct = value;
            OnPropertyChanged(nameof(SelectedConduct));
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

    private int _screenWidth;

    public int ScreenWidth
    {
        get => _screenWidth;
        set
        {
            _screenWidth = value;
            OnPropertyChanged(nameof(ScreenWidth));
        }
    }

    private int _collectionViewSpanCount = 1;

    public int CollectionViewSpanCount
    {
        get => _collectionViewSpanCount;
        set
        {
            _collectionViewSpanCount = value;
            OnPropertyChanged(nameof(CollectionViewSpanCount));
        }
    }

    private bool _isConductTypeErrVisible;

    public bool IsConductTypeErrVisible
    {
        get => _isConductTypeErrVisible;
        set
        {
            _isConductTypeErrVisible = value;
            OnPropertyChanged(nameof(IsConductTypeErrVisible));
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

    private bool _isNoRecordMsgSearch = false;

    public bool IsNoRecordMsgSearch
    {
        get => _isNoRecordMsgSearch;
        set
        {
            _isNoRecordMsgSearch = value;
            OnPropertyChanged(nameof(IsNoRecordMsgSearch));
        }
    }

    private bool _dateErrorMessageVisibility;

    public bool DateErrorMessageVisibility
    {
        get => _dateErrorMessageVisibility;
        set
        {
            _dateErrorMessageVisibility = value;
            OnPropertyChanged(nameof(DateErrorMessageVisibility));
        }
    }
    #endregion

    public ConductSearchForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null,
        null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Method

    private async void InitializePage()
    {
        SearchClickCommand = new Command(SearchClicked);
        ListTappedCommand = new Command<StudentConductView>(ListViewTapped);
        await GetStudentConductData();
    }

    public override async void GetStudentData()
    {
        base.GetStudentData();
        await GetStudentConductData();
        SearchClicked();
    }

    private async Task GetStudentConductData()
    {
        try
        {
            var cacheKeyPrefix = "studentconduct";
            var loadFilterPanelLists = !ConductTypeList.Any();
            var apiUrl = string.Format(TextResource.StudentConductDataApiUrl, "", "", "",
                AppSettings.Current.SelectedStudent.ItemId, loadFilterPanelLists);
            StudentConductData = await ApiHelper.GetObject<StudentConductViewModel>(apiUrl,
                attachStudentIdIfParent: true, cacheKeyPrefix: cacheKeyPrefix,
                cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
            NoDataExist = !StudentConductData.StudentConducts.Any();
            CollectionViewSpanCount = StudentConductData.ConductSummaries.Count();
            foreach (var item in StudentConductData.ConductSummaries)
                if (item.Summary != null && item.Summary.ToLower().Contains("demerits"))
                    item.Summary = "Demerits";
                else if (item.Summary != null && item.Summary.ToLower().Contains("merits") &&
                         !item.Summary.ToLower().Contains("demerits")) item.Summary = "Merits";
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void SearchClicked()
    {
        try
        {
            if (ValidateData())
            {
                var filteredList = StudentConductData.StudentConducts
                    .Where(conduct => conduct.FormattedIncidentDate.ToDateTime().Date >= FromDate.Date &&
                                      conduct.FormattedIncidentDate.ToDateTime().Date <= ToDate.Date &&
                                      conduct.ConductTypeCode == SelectedConductType.ItemId)
                    .ToList();

                FilterStudentConductData = new StudentConductViewModel
                {
                    StudentConducts = filteredList,
                    ConductSummaries = StudentConductData.ConductSummaries,
                    ConductTypeList = StudentConductData.ConductTypeList
                };
                IsNoRecordMsgSearch = FilterStudentConductData?.StudentConducts?.Count() > 0 ? false : true;

                var totalPoints = 0;

                if (FilterStudentConductData.StudentConducts.Count() > 0)
                {
                    foreach (var item in FilterStudentConductData.StudentConducts)
                        totalPoints += item.ConductTypeCode.Equals("D")
                            ? Math.Abs(item.Points) * -1
                            : Math.Abs(item.Points);
                    FilterStudentConductData.StudentConducts.First().Points = Math.Abs(totalPoints);
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.FilterAgendaTitle);
        }
    }

    private async void ListViewTapped(StudentConductView obj)
    {
        if (obj != null)
        {
            ConductDetailForm conductDetailForm = new(_mapper, _nativeServices, Navigation)
            {
                SelectedConduct = obj,
                PageTitle = PageTitle
            };
            ConductDetailPage conductDetailPage = new ConductDetailPage()
            {
                BindingContext = conductDetailForm
            };
            await Navigation.PushAsync(conductDetailPage);
            SelectedConduct = null;
        }
    }

    private bool ValidateData()
    {
        IsConductTypeErrVisible = SelectedConductType != null && SelectedConductType.ItemName != null ? false : true;
        return !IsConductTypeErrVisible;
    }

    #endregion
}