using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Attendance;
using iCampus.Portal.ViewModels;
using Microcharts;
using SkiaSharp;

namespace iCampus.MobileApp.Forms.UserModules.Attendance;

public class AttendanceForm : ViewModelBase
{
    #region Declarations

    private Popup _currentPopup;
    public ICommand FilterClickCommand { get; set; }
    public ICommand PickerTappedCommand { get; set; }
    public ICommand TypeClickCommand { get; set; }
    private double sumOfChartSeriesValues;

    #endregion

    #region Properties

    private StudentAttendancePageView _attendancePageData = new();

    public StudentAttendancePageView AttendancePageData
    {
        get => _attendancePageData;
        set
        {
            _attendancePageData = value;
            OnPropertyChanged(nameof(AttendancePageData));
        }
    }

    private Chart _chartData;

    public Chart ChartData
    {
        get => _chartData;
        set
        {
            _chartData = value;
            OnPropertyChanged(nameof(ChartData));
        }
    }

    private List<ChartEntry> _entryList = new();

    public List<ChartEntry> EntryList
    {
        get => _entryList;
        set
        {
            _entryList = value;
            OnPropertyChanged(nameof(EntryList));
        }
    }

    private string _headerLabelTitle;

    public string HeaderLabelTitle
    {
        get => _headerLabelTitle;
        set
        {
            _headerLabelTitle = value;
            OnPropertyChanged(nameof(HeaderLabelTitle));
        }
    }

    private string _dateLabelTitle;

    public string DateLabelTitle
    {
        get => _dateLabelTitle;
        set
        {
            _dateLabelTitle = value;
            OnPropertyChanged(nameof(DateLabelTitle));
        }
    }

    private PickListItem _selectedType = new();

    public PickListItem SelectedType
    {
        get => _selectedType;
        set
        {
            _selectedType = value;
            OnPropertyChanged(nameof(SelectedType));
        }
    }

    private PickListItem _nullableSelectedType = new();

    public PickListItem NullableSelectedType
    {
        get => _nullableSelectedType;
        set
        {
            _nullableSelectedType = value;
            OnPropertyChanged(nameof(NullableSelectedType));
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
        }
    }

    private List<PickListItem> _typeList = new();

    public List<PickListItem> TypeList
    {
        get => _typeList;
        set
        {
            _typeList = value;
            OnPropertyChanged(nameof(TypeList));
        }
    }

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

    private DateTime _fromDate;

    public DateTime FromDate
    {
        get => _fromDate;
        set
        {
            _fromDate = value;
            OnPropertyChanged(nameof(FromDate));
        }
    }

    private DateTime _toDate;

    public DateTime ToDate
    {
        get => _toDate;
        set
        {
            _toDate = value;
            OnPropertyChanged(nameof(ToDate));
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

    private bool _attendanceDetailsVisible;

    public bool AttendanceDetailsVisible
    {
        get => _attendanceDetailsVisible;
        set
        {
            _attendanceDetailsVisible = value;
            OnPropertyChanged(nameof(AttendanceDetailsVisible));
        }
    }

    private ObservableCollection<StudentAttendanceDataView> _attendanceDetailsList = new();

    public ObservableCollection<StudentAttendanceDataView> AttendanceDetailsList
    {
        get => _attendanceDetailsList;
        set
        {
            _attendanceDetailsList = value;
            OnPropertyChanged(nameof(AttendanceDetailsList));
        }
    }

    private bool _isNoRecordMsg;

    public bool IsNoRecordMsg
    {
        get => _isNoRecordMsg;
        set
        {
            _isNoRecordMsg = value;
            OnPropertyChanged(nameof(IsNoRecordMsg));
        }
    }

    private string _attendancePercentage;

    public string AttendancePercentage
    {
        get => _attendancePercentage;
        set
        {
            _attendancePercentage = value;
            OnPropertyChanged(nameof(AttendancePercentage));
        }
    }

    private bool _isPeriodVisible;

    public bool IsPeriodVisible
    {
        get => _isPeriodVisible;
        set
        {
            _isPeriodVisible = value;
            OnPropertyChanged(nameof(IsPeriodVisible));
        }
    }

    private string _totalDaysLabelText;

    public string TotalDaysLabelText
    {
        get => _totalDaysLabelText;
        set
        {
            _totalDaysLabelText = value;
            OnPropertyChanged(nameof(TotalDaysLabelText));
        }
    }

    private List<BindableSeries> _chartDataPercentageList = new();

    public List<BindableSeries> ChartDataPercentageList
    {
        get => _chartDataPercentageList;
        set
        {
            _chartDataPercentageList = value;
            OnPropertyChanged(nameof(ChartDataPercentageList));
        }
    }

    #endregion

    public AttendanceForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null,
        null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        FilterClickCommand = new Command(FilterClicked);
        PickerTappedCommand = new Command<PickListItem>(TypePickerClicked);
        TypeClickCommand = new Command(TypeClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        FromDate = DateTime.Now;
        ToDate = DateTime.Now;
        TotalDaysLabelText = "Total School days till" + " " + DateTime.Today.ToString("dd/MM/y");
        MessagingCenter.Subscribe<AttendancePageFilterForm>(this, "Search", async (filterFormData) =>
        {
            SelectedGroupPeriod = filterFormData.SelectedGroupPeriod;
            SelectedTerm = filterFormData.SelectedTerm;
            FromDate = filterFormData.FromDate;
            ToDate = filterFormData.ToDate;
            await GetAttendanceDataByStudent();
        });
    }

    #region Methods

    public void getHeaderLabelTitle()
    {
        switch (SelectedGroupPeriod.ItemId.ToLower())
        {
            case "m":
                HeaderLabelTitle = "Monthly Attendance\n" + "[ " + DateTime.Now.ToString(TextResource.DateFormatKey1) +
                                   " ]  ";
                break;
            case "w":
                var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                var endDate = startDate.AddDays(7).AddSeconds(-1);
                HeaderLabelTitle = "Weekly Attendance \n" + "[ " + startDate.ToString(TextResource.DateFormatKey3) +
                                   " to " + endDate.ToString(TextResource.DateFormatKey3) + " ]  ";
                break;
            case "d":
                HeaderLabelTitle = "Attendance \n" + "[ " + FromDate.ToString(TextResource.DateFormatKey3) + " to " +
                                   ToDate.ToString(TextResource.DateFormatKey3) + " ] ";
                break;
            case "y":
                HeaderLabelTitle = "Yearly Attendance " + "[ " + DateTime.Now.Year.ToString() + " ] ";
                break;
            case "t":
                HeaderLabelTitle = SelectedTerm != null && !string.IsNullOrEmpty(SelectedTerm.ItemName)
                    ? "Termwise Attendance\n" + "[ " + SelectedTerm.ItemName + " ] "
                    : "Termwise Attendance";
                break;
        }

        DateLabelTitle = "till " + DateTime.Now.ToString(TextResource.DateFormatKey2);
    }

    public override async void GetStudentData()
    {
        base.GetStudentData();
        await GetAttendanceDataByStudent();
        await SetViews();
    }

    public async Task<StudentAttendancePageView> GetAttendanceDataByStudent()
    {
        try
        {
            LoadFilterPanelLists = !TermList.Any();

            SelectedGroupPeriod = SelectedGroupPeriod.ItemId != null
                ? SelectedGroupPeriod
                : GroupPeriodList.Where(x => x.ItemId == "T").FirstOrDefault();
            SelectedType = SelectedType.ItemName != null
                ? SelectedType
                : TypeList.Where(x => x.ItemName.ToLower() == "absent").FirstOrDefault();
            SelectedTerm = SelectedTerm != null && SelectedTerm.ItemId != null ? SelectedTerm : new ExtPickListItem();

            if (SelectedType.ItemName.Equals("Details"))
            {
                AttendanceDetailsVisible = true;
                AttendancePageData = new StudentAttendancePageView();
                var attendanceData = await ApiHelper.GetObject<StudentAttendancePageView>(string.Format(
                    TextResource.AttendanceDetailsApiUrl,
                    null, null, AppSettings.Current.SelectedStudent.ItemId, null, SelectedTerm.ItemId,
                    SelectedGroupPeriod.ItemId, LoadFilterPanelLists));
                AttendanceDetailsList =
                    new ObservableCollection<StudentAttendanceDataView>(attendanceData.StudentAttendanceDataList);
                IsPeriodVisible = attendanceData.AttendanceMode.ToLower() == "p";
                getHeaderLabelTitle();
                IsNoRecordMsg = AttendanceDetailsList.ToList().Count > 0 ? false : true;
            }
            else
            {
                AttendanceDetailsVisible = false;
                AttendancePageData = await ApiHelper.GetObject<StudentAttendancePageView>(string.Format(
                    TextResource.AttendanceApiUrl,
                    null, null, AppSettings.Current.SelectedStudent.ItemId, SelectedType.ItemName, SelectedTerm.ItemId,
                    SelectedGroupPeriod.ItemId, LoadFilterPanelLists));
                if (AttendancePageData != null && AttendancePageData.AttendanceChart != null &&
                    AttendancePageData.AttendanceChart.series != null)
                {
                    EntryList = new List<ChartEntry>();
                    AttendancePercentage =
                        Math.Round(AttendancePageData.AttendanceData.AttendancePercent, 1).ToString();
                    if (AttendancePageData.AttendanceChart.series != null)
                        ChartDataPercentageList =
                            _mapper.Map<List<BindableSeries>>(AttendancePageData.AttendanceChart.series);
                    foreach (var series in ChartDataPercentageList)
                        EntryList.Add(new ChartEntry((float)series.y)
                        {
                            Label = series.name, ValueLabel = series.y.ToString(),
                            Color = series.color != null ? SKColor.Parse(series.color) : SKColor.Parse("#000000")
                        });
                    ChartData = new PieChart()
                    {
                        Entries = EntryList,
                        LabelTextSize = 1
                    };
                    await CalculateChartSeriesPercentage();
                    getHeaderLabelTitle();
                }

                if (AttendancePageData.TermList != null && AttendancePageData.TermList.Count > 0)
                {
                    TermList = AttendancePageData.TermList;
                    if (SelectedTerm == null || SelectedTerm.ItemId == null)
                        SelectedTerm = TermList.FirstOrDefault();
                }
            }

            await SetViews();
            return AttendancePageData;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CalendarPageTitle);
            AttendancePageData = new StudentAttendancePageView();
            ChartData = new DonutChart()
            {
                Entries = new List<ChartEntry>(),
                LabelTextSize = 1
            };
            return AttendancePageData;
        }
    }

    private async void FilterClicked(object obj)
    {
        AttendancePageFilterForm attendancePageFilterForm = new(_mapper, _nativeServices, Navigation)
        {
            PageTitle = TextResource.FilterAttendanceTitle,
            MenuVisible = false,
            BackVisible = true,
            FromDate = FromDate,
            ToDate = ToDate,
            GroupPeriodList = GroupPeriodList,
            TermList = TermList,
            SelectedGroupPeriod = SelectedGroupPeriod.ItemId != null
                ? SelectedGroupPeriod
                : GroupPeriodList.Where(x => x.ItemId == "M").FirstOrDefault(),
            SelectedTerm = SelectedTerm != null ? SelectedTerm : TermList.FirstOrDefault()
        };
        AttendancePageFilter attendancePageFilter = new()
        {
            BindingContext = attendancePageFilterForm
        };
        await Navigation.PushAsync(attendancePageFilter);
    }

    public void GetAttendanceType()
    {
        try
        {
            TypeList = new List<PickListItem>()
            {
                new() { ItemName = "Absent" },
                new() { ItemName = "Late" },
                new() { ItemName = "Leave Early" }
            };
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public void GetAttendanceGroup()
    {
        try
        {
            GroupPeriodList = new List<PickListItem>()
            {
                new() { ItemId = "D", ItemName = "Date Period" },
                new() { ItemId = "M", ItemName = "Month" },
                new() { ItemId = "T", ItemName = "Term" },
                new() { ItemId = "W", ItemName = "Week" },
                new() { ItemId = "Y", ItemName = "Year" }
            };
            SelectedGroupPeriod = GroupPeriodList[2];
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void TypePickerClicked(PickListItem obj)
    {
        if (obj != null)
        {
            SelectedType = (PickListItem)obj;
            NullableSelectedType = null;
            _currentPopup?.Close();
            await GetAttendanceDataByStudent();
        }
    }

    private async void TypeClicked(object obj)
    {
        AttendanceTypePopup attendanceTypePopup = new ()
        {
            BindingContext = this
        };
        SetPopupInstance(attendanceTypePopup);
        Application.Current.MainPage.ShowPopup(attendanceTypePopup);
    }
    public void SetPopupInstance(Popup popup)
    {
        _currentPopup = popup;
    }

    private async Task SetViews()
    {
        try
        {
            var attendanceData = await ApiHelper.GetObject<StudentAttendancePageView>(string.Format(
                TextResource.AttendanceDetailsApiUrl,
                null, null, AppSettings.Current.SelectedStudent.ItemId, SelectedType.ItemName, SelectedTerm.ItemId,
                SelectedGroupPeriod.ItemId, LoadFilterPanelLists));
            AttendanceDetailsList =
                new ObservableCollection<StudentAttendanceDataView>(attendanceData.StudentAttendanceDataList);
            IsPeriodVisible = attendanceData.AttendanceMode.ToLower() == "p";
            getHeaderLabelTitle();
            IsNoRecordMsg = AttendanceDetailsList.ToList().Count > 0 ? false : true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task CalculateChartSeriesPercentage()
    {
        if (ChartDataPercentageList != null)
        {
            sumOfChartSeriesValues = ChartDataPercentageList.Sum(x => x.y);
            foreach (var item in ChartDataPercentageList)
                if (item != null)
                    item.CountPercentage = Math.Round(item.y / sumOfChartSeriesValues * 100);
        }
    }

    #endregion
}