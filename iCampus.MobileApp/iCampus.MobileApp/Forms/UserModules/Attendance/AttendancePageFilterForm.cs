using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.Attendance;

public class AttendancePageFilterForm : ViewModelBase
{
    #region Declarations

    public ICommand SearchClickCommand { get; set; }

    #endregion

    #region Properties

    private List<PickListItem> _groupPeriodList = new();

    public List<PickListItem> GroupPeriodList
    {
        get => _groupPeriodList;
        set
        {
            _groupPeriodList = value;
            OnPropertyChanged(nameof(GroupPeriodList));
        }
    }

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


    private PickListItem _selectedGroupPeriod = new();

    public PickListItem SelectedGroupPeriod
    {
        get => _selectedGroupPeriod;
        set
        {
            _selectedGroupPeriod = value;
            OnPropertyChanged(nameof(SelectedGroupPeriod));
            IsDateVisible = SelectedGroupPeriod.ItemId.ToLower() == "d" ? true : false;
            IsTermVisible = SelectedGroupPeriod.ItemId.ToLower() == "t" ? true : false;
        }
    }

    private ExtPickListItem _selectedTerm = new();

    public ExtPickListItem SelectedTerm
    {
        get => _selectedTerm;
        set
        {
            _selectedTerm = value;
            OnPropertyChanged(nameof(SelectedTerm));
        }
    }

    public bool _isDateVisible;

    public bool IsDateVisible
    {
        get => _isDateVisible;
        set
        {
            _isDateVisible = value;
            OnPropertyChanged(nameof(IsDateVisible));
        }
    }

    public bool _isTermVisible;

    public bool IsTermVisible
    {
        get => _isTermVisible;
        set
        {
            _isTermVisible = value;
            OnPropertyChanged(nameof(IsTermVisible));
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

    #endregion

    public AttendancePageFilterForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(
        mapper, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        SearchClickCommand = new Command(SearchClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
    }

    private void SearchClicked(object obj)
    {
        MessagingCenter.Send(this, "Search");
        Navigation.PopAsync();
        //HostScreen.Router.NavigateBack.Execute();
    }
}