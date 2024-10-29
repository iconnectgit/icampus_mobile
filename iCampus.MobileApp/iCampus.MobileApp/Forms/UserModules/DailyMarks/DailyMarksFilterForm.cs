using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.DailyMarks;

public class DailyMarksFilterForm : ViewModelBase
{
    #region Declarations

    public ICommand SearchClickCommand { get; set; }

    #endregion

    #region Properties

    private IList<ExtPickListItem> _termList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> TermList
    {
        get => _termList;
        set
        {
            _termList = value;
            OnPropertyChanged(nameof(TermList));
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

    private int _selectedTermIndex = 0;

    public int SelectedTermIndex
    {
        get => _selectedTermIndex;
        set
        {
            _selectedTermIndex = value;
            OnPropertyChanged(nameof(SelectedTermIndex));
            if (value < 0)
                IsTermErrVisible = false;
        }
    }

    private int _selectedCourseIndex = -1;

    public int SelectedCourseIndex
    {
        get => _selectedCourseIndex;
        set
        {
            _selectedCourseIndex = value;
            OnPropertyChanged(nameof(SelectedCourseIndex));
        }
    }

    private bool _isTermErrVisible;

    public bool IsTermErrVisible
    {
        get => _isTermErrVisible;
        set
        {
            _isTermErrVisible = value;
            OnPropertyChanged(nameof(IsTermErrVisible));
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

    #endregion

    public DailyMarksFilterForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null,
        null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        SearchClickCommand = new Command(SearchClicked);
    }

    #region Methods

    private bool ValidateData()
    {
        return SelectedTermIndex >= 0;
    }

    private async void SearchClicked(object obj)
    {
        if (ValidateData())
        {
            MessagingCenter.Send(this, "SearchDailyMarks");
            await Navigation.PopAsync();
            //HostScreen.Router.NavigateBack.Execute();
        }
    }

    #endregion
}