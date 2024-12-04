using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.Library;

public class LibraryForm : ViewModelBase
{
    #region Declaration

    public ICommand CurrentCommand { get; set; }
    public ICommand HistoryCommand { get; set; }
    public ICommand CurrentExpandCollapseClickCommand { get; set; }
    public ICommand HistoryExpandCollapseClickCommand { get; set; }

    #endregion

    #region Properties

    private bool _isHistory = false;

    public bool IsHistory
    {
        get => _isHistory;
        set
        {
            _isHistory = value;
            OnPropertyChanged(nameof(IsHistory));
        }
    }

    private bool _isCurrent = true;

    public bool IsCurrent
    {
        get => _isCurrent;
        set
        {
            _isCurrent = value;
            OnPropertyChanged(nameof(IsCurrent));
        }
    }

    private FontAttributes _currentFontType = FontAttributes.Bold;

    public FontAttributes CurrentFontType
    {
        get => _currentFontType;
        set
        {
            _currentFontType = value;
            OnPropertyChanged(nameof(CurrentFontType));
        }
    }

    private FontAttributes _historyFontType = FontAttributes.None;

    public FontAttributes HistoryFontType
    {
        get => _historyFontType;
        set
        {
            _historyFontType = value;
            OnPropertyChanged(nameof(HistoryFontType));
        }
    }

    private IList<BindableLibraryView> _libraryList;

    public IList<BindableLibraryView> LibraryList
    {
        get => _libraryList;
        set
        {
            _libraryList = value;
            OnPropertyChanged(nameof(LibraryList));
        }
    }

    private IList<BindableLibraryView> _selectedLibraryList;

    public IList<BindableLibraryView> SelectedLibraryList
    {
        get => _selectedLibraryList;
        set
        {
            _selectedLibraryList = value;
            OnPropertyChanged(nameof(SelectedLibraryList));
        }
    }

    private IList<BindableLibraryView> _libraryHistoryList;

    public IList<BindableLibraryView> LibraryHistoryList
    {
        get => _libraryHistoryList;
        set
        {
            _libraryHistoryList = value;
            OnPropertyChanged(nameof(LibraryHistoryList));
        }
    }

    private IList<BindableLibraryView> _selectedHistoryLibraryList;

    public IList<BindableLibraryView> SelectedHistoryLibraryList
    {
        get => _selectedHistoryLibraryList;
        set
        {
            _selectedHistoryLibraryList = value;
            OnPropertyChanged(nameof(SelectedHistoryLibraryList));
        }
    }

    private bool _isNoRecordLibrary = false;

    public bool IsNoRecordLibrary
    {
        get => _isNoRecordLibrary;
        set
        {
            _isNoRecordLibrary = value;
            OnPropertyChanged(nameof(IsNoRecordLibrary));
        }
    }

    private bool _isNoRecordHistory = false;

    public bool IsNoRecordHistory
    {
        get => _isNoRecordHistory;
        set
        {
            _isNoRecordHistory = value;
            OnPropertyChanged(nameof(IsNoRecordHistory));
        }
    }

    private bool _currentShadow = true;

    public bool CurrentShadow
    {
        get => _currentShadow;
        set
        {
            _currentShadow = value;
            OnPropertyChanged(nameof(CurrentShadow));
        }
    }

    private bool _historyShadow = false;

    public bool HistoryShadow
    {
        get => _historyShadow;
        set
        {
            _historyShadow = value;
            OnPropertyChanged(nameof(HistoryShadow));
        }
    }

    private decimal _currentHoldingButtonOpacity;

    public decimal CurrentHoldingButtonOpacity
    {
        get => _currentHoldingButtonOpacity;
        set
        {
            _currentHoldingButtonOpacity = value;
            OnPropertyChanged(nameof(CurrentHoldingButtonOpacity));
        }
    }

    private decimal _historyButtonOpacity;

    public decimal HistoryButtonOpacity
    {
        get => _historyButtonOpacity;
        set
        {
            _historyButtonOpacity = value;
            OnPropertyChanged(nameof(HistoryButtonOpacity));
        }
    }

    #endregion


    public LibraryForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods

    private async void InitializePage()
    {
        CurrentHoldingButtonOpacity = 1.0m;
        HistoryButtonOpacity = 0.5m;
        CurrentCommand = new Command(CurrentCommandClicked);
        HistoryCommand = new Command(HistoryCommandClicked);
        CurrentExpandCollapseClickCommand = new Command<BindableLibraryView>(CurrentExpandCollapseClicked);
        HistoryExpandCollapseClickCommand = new Command<BindableLibraryView>(HistoryExpandCollapseClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
    }

    private async Task GetDetails()
    {
        try
        {
            var data = await ApiHelper.GetObject<List<BindableLibraryView>>(
                string.Format(TextResource.GetGetLibraryDataUrl, AppSettings.Current.SelectedStudent.ItemId),
                cacheKeyPrefix: "library", cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
            var libraryList = data.Where(m =>
                m.GroupName.Equals("Issues", StringComparison.OrdinalIgnoreCase) ||
                m.GroupName.Equals("Overdue", StringComparison.OrdinalIgnoreCase)).ToList();
            LibraryList = libraryList;
            foreach (var item in LibraryList)
                if (item.GroupName.Equals("Overdue", StringComparison.OrdinalIgnoreCase))
                {
                    item.GroupName = item.GroupName.Replace("Overdue", "Over due");
                    item.IsOverDue = true;
                }

            var historyData = data.Where(m => m.GroupName.Equals("History", StringComparison.OrdinalIgnoreCase));
            LibraryHistoryList = _mapper.Map<IList<BindableLibraryView>>(historyData);

            IsNoRecordLibrary = LibraryList.Count == 0;
            IsNoRecordHistory = LibraryHistoryList.Count == 0;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private void HistoryCommandClicked(object obj)
    {
        CurrentHoldingButtonOpacity = 0.5m;
        HistoryButtonOpacity = 1.0m;
        IsHistory = true;
        IsCurrent = false;
        CurrentShadow = false;
        HistoryShadow = true;
        CurrentFontType = FontAttributes.None;
        HistoryFontType = FontAttributes.Bold;
    }

    private void CurrentCommandClicked(object obj)
    {
        CurrentHoldingButtonOpacity = 1.0m;
        HistoryButtonOpacity = 0.5m;
        IsHistory = false;
        IsCurrent = true;
        CurrentShadow = true;
        HistoryShadow = false;
        CurrentFontType = FontAttributes.Bold;
        HistoryFontType = FontAttributes.None;
    }

    public void HistoryExpandCollapseClicked(BindableLibraryView libraryData)
    {
        try
        {
            if (libraryData != null)
            {
                foreach (var item in LibraryHistoryList.ToList())
                    if (item != null)
                    {
                        if (item.Title == libraryData.Title)
                        {
                            item.DetailsVisibility = !item.DetailsVisibility;
                            item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png")
                                ? "dropdown_gray.png"
                                : "uparrow_gray.png";
                        }
                        else
                        {
                            item.DetailsVisibility = false;
                            item.ArrowImageSource = "dropdown_gray.png";
                        }
                    }

                MessagingCenter.Send("", "HistoryExpandCollapse");
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public void CurrentExpandCollapseClicked(BindableLibraryView libraryData)
    {
        try
        {
            if (libraryData != null)
            {
                foreach (var item in LibraryList.ToList())
                    if (item != null)
                    {
                        if (item.Title == libraryData.Title)
                        {
                            item.DetailsVisibility = !item.DetailsVisibility;
                            item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png")
                                ? "dropdown_gray.png"
                                : "uparrow_gray.png";
                        }
                        else
                        {
                            item.DetailsVisibility = false;
                            item.ArrowImageSource = "dropdown_gray.png";
                        }
                    }

                MessagingCenter.Send("", "CurrentExpandCollapse");
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public override async void GetStudentData()
    {
        try
        {
            await GetDetails();
            base.GetStudentData();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    #endregion
}