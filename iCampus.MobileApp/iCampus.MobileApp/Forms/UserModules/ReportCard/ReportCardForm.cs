using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.ReportCard;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.ReportCard;

public class ReportCardForm : ViewModelBase
{
    #region Declarations

    public ICommand ReportCardCommand { get; set; }
    public ICommand SkillReportCardCommand { get; set; }
    public ICommand FilterClickCommand { get; set; }
    public ICommand AttachmentClickCommand { get; set; }
    public ICommand DownloadTappedCommand { get; set; }
    public ICommand SearchClickCommand { get; set; }

    #endregion

    #region Properties

    private BindableAttachmentFileView _selectedAttachment;
    private bool _isEnableCaching = true;

    public BindableAttachmentFileView SelectedAttachment
    {
        get => _selectedAttachment;
        set
        {
            _selectedAttachment = value;
            OnPropertyChanged(nameof(SelectedAttachment));
        }
    }

    private ReportCardSettingsView _reportCardData;

    public ReportCardSettingsView ReportCardData
    {
        get => _reportCardData;
        set
        {
            _reportCardData = value;
            OnPropertyChanged(nameof(ReportCardData));
        }
    }

    private IEnumerable<ReportCardActiveTerms> _marksReportCardActiveTermsList;

    public IEnumerable<ReportCardActiveTerms> MarksReportCardActiveTermsList
    {
        get => _marksReportCardActiveTermsList;
        set
        {
            _marksReportCardActiveTermsList = value;
            OnPropertyChanged(nameof(MarksReportCardActiveTermsList));
        }
    }

    private IEnumerable<ReportCardActiveTerms> _skillsReportCardActiveTermsList;

    public IEnumerable<ReportCardActiveTerms> SkillsReportCardActiveTermsList
    {
        get => _skillsReportCardActiveTermsList;
        set
        {
            _skillsReportCardActiveTermsList = value;
            OnPropertyChanged(nameof(SkillsReportCardActiveTermsList));
        }
    }

    private ReportCardActiveTerms _selectedTermMarksReport = new();

    public ReportCardActiveTerms SelectedTermMarksReport
    {
        get => _selectedTermMarksReport;
        set
        {
            if (_selectedTermMarksReport != null) SelectedReport = _selectedTermMarksReport;
            if (_selectedTermMarksReport != null && !string.IsNullOrEmpty(_selectedTermMarksReport.TermName))
            {
                AppSettings.Current.SelectedMarksReportCardTerm = _selectedTermMarksReport.TermName;
                AppSettings.Current.SelectedMarksReportCardTermId = _selectedTermMarksReport.TermId;
            }

            _selectedTermMarksReport = value;
            OnPropertyChanged(nameof(SelectedTermMarksReport));
        }
    }

    private async Task SetTermLabelVisibility()
    {
        if (ReportCardOption == ReportCardOptions.MarksReportCard)
        {
            if (SelectedTermMarksReport != null && !string.IsNullOrEmpty(SelectedTermMarksReport.TermName))
                TermLabelVisibility = true;
        }
        else if (ReportCardOption == ReportCardOptions.SkillsReportCard)
        {
            if (SelectedTermSkillReport != null && !string.IsNullOrEmpty(SelectedTermSkillReport.TermName))
                SkillReportCardTermLabelVisibility = true;
        }
    }

    private ReportCardActiveTerms _selectedTermSkillReport = new();

    public ReportCardActiveTerms SelectedTermSkillReport
    {
        get => _selectedTermSkillReport;
        set
        {
            if (_selectedTermSkillReport != null && !string.IsNullOrEmpty(_selectedTermSkillReport.TermName))
            {
                AppSettings.Current.SelectedSkillReportCardTerm = _selectedTermSkillReport.TermName;
                AppSettings.Current.SelectedSkillReportCardTermId = _selectedTermSkillReport.TermId;
            }

            _selectedTermSkillReport = value;
            OnPropertyChanged(nameof(SelectedTermSkillReport));
        }
    }

    private ReportCardActiveTerms _selectedReport = new();

    public ReportCardActiveTerms SelectedReport
    {
        get => _selectedReport;
        set
        {
            _selectedReport = value;
            OnPropertyChanged(nameof(SelectedReport));
        }
    }

    private ReportCardOptions _reportCardOption;

    public ReportCardOptions ReportCardOption
    {
        get => _reportCardOption;
        set
        {
            _reportCardOption = value;
            OnPropertyChanged(nameof(ReportCardOption));
        }
    }

    private bool _isReporCardsAvailable;

    public bool IsReporCardsAvailable
    {
        get => _isReporCardsAvailable;
        set
        {
            _isReporCardsAvailable = value;
            OnPropertyChanged(nameof(IsReporCardsAvailable));
        }
    }

    private bool _isMarksTermVisible;

    public bool IsMarksTermVisible
    {
        get => _isMarksTermVisible;
        set
        {
            _isMarksTermVisible = value;
            OnPropertyChanged(nameof(IsMarksTermVisible));
        }
    }

    private string _reportPageTitle;

    public string ReportPageTitle
    {
        get => _reportPageTitle;
        set
        {
            _reportPageTitle = value;
            OnPropertyChanged(nameof(ReportPageTitle));
        }
    }

    private AttachmentFileView _marksReportCardFileView;

    public AttachmentFileView MarksReportCardFileView
    {
        get => _marksReportCardFileView;
        set
        {
            _marksReportCardFileView = value;
            OnPropertyChanged(nameof(MarksReportCardFileView));
        }
    }

    private AttachmentFileView _skillsReportCardFileView;

    public AttachmentFileView SkillsReportCardFileView
    {
        get => _skillsReportCardFileView;
        set
        {
            _skillsReportCardFileView = value;
            OnPropertyChanged(nameof(SkillsReportCardFileView));
        }
    }

    private bool _isAcademicYearsVisible;

    public bool IsAcademicYearsVisible
    {
        get => _isAcademicYearsVisible;
        set
        {
            _isAcademicYearsVisible = value;
            OnPropertyChanged(nameof(IsAcademicYearsVisible));
        }
    }

    private IEnumerable<ExtPickListItem> _academicYearList;

    public IEnumerable<ExtPickListItem> AcademicYearList
    {
        get => _academicYearList;
        set
        {
            _academicYearList = value;
            OnPropertyChanged(nameof(AcademicYearList));
        }
    }

    private ExtPickListItem _selectedAcademicYear;

    public ExtPickListItem SelectedAcademicYear
    {
        get => _selectedAcademicYear;
        set
        {
            _selectedAcademicYear = value;
            OnPropertyChanged(nameof(SelectedAcademicYear));
            var selectedYear = SelectedAcademicYear != null ? SelectedAcademicYear.ItemId : string.Empty;
            if (!string.IsNullOrEmpty(selectedYear))
                IsTermEnabled = true;
            GetTermList(selectedYear);
        }
    }

    private bool _isTermEnabled;

    public bool IsTermEnabled
    {
        get => _isTermEnabled;
        set
        {
            _isTermEnabled = value;
            OnPropertyChanged(nameof(IsTermEnabled));
        }
    }

    private string _yearText;

    public string YearText
    {
        get => _yearText;
        set
        {
            _yearText = value;
            OnPropertyChanged(nameof(YearText));
        }
    }

    private bool _termSelectionErrorMessageVisibility = false;

    public bool TermSelectionErrorMessageVisibility
    {
        get => _termSelectionErrorMessageVisibility;
        set
        {
            _termSelectionErrorMessageVisibility = value;
            OnPropertyChanged(nameof(TermSelectionErrorMessageVisibility));
        }
    }

    private bool _termLabelVisibility = false;

    public bool TermLabelVisibility
    {
        get => _termLabelVisibility;
        set
        {
            _termLabelVisibility = value;
            OnPropertyChanged(nameof(TermLabelVisibility));
        }
    }

    private bool _noDataFoundFrameVisibility = false;

    public bool NoDataFoundFrameVisibility
    {
        get => _noDataFoundFrameVisibility;
        set
        {
            _noDataFoundFrameVisibility = value;
            OnPropertyChanged(nameof(NoDataFoundFrameVisibility));
        }
    }

    private bool _skillReportCardTermLabelVisibility = false;

    public bool SkillReportCardTermLabelVisibility
    {
        get => _skillReportCardTermLabelVisibility;
        set
        {
            _skillReportCardTermLabelVisibility = value;
            OnPropertyChanged(nameof(SkillReportCardTermLabelVisibility));
        }
    }

    private bool _marksReportCardsVisibility = false;

    public bool MarksReportCardsVisibility
    {
        get => _marksReportCardsVisibility;
        set
        {
            _marksReportCardsVisibility = value;
            OnPropertyChanged(nameof(MarksReportCardsVisibility));
        }
    }

    private bool _skillReportCardsVisibility = false;

    public bool SkillReportCardsVisibility
    {
        get => _skillReportCardsVisibility;
        set
        {
            _skillReportCardsVisibility = value;
            OnPropertyChanged(nameof(SkillReportCardsVisibility));
        }
    }

    private bool _selectTermsOptionForSkillVisibility = true;

    public bool SelectTermsOptionForSkillVisibility
    {
        get => _selectTermsOptionForSkillVisibility;
        set
        {
            _selectTermsOptionForSkillVisibility = value;
            OnPropertyChanged(nameof(SelectTermsOptionForSkillVisibility));
        }
    }

    private bool _selectTermsOptionForMarksVisibility = true;

    public bool SelectTermsOptionForMarksVisibility
    {
        get => _selectTermsOptionForMarksVisibility;
        set
        {
            _selectTermsOptionForMarksVisibility = value;
            OnPropertyChanged(nameof(SelectTermsOptionForMarksVisibility));
        }
    }

    private decimal _reportCardButtonOpacity;

    public decimal ReportCardButtonOpacity
    {
        get => _reportCardButtonOpacity;
        set
        {
            _reportCardButtonOpacity = value;
            OnPropertyChanged(nameof(ReportCardButtonOpacity));
        }
    }

    private decimal _skillReportCardButtonOpacity;

    public decimal SkillReportCardButtonOpacity
    {
        get => _skillReportCardButtonOpacity;
        set
        {
            _skillReportCardButtonOpacity = value;
            OnPropertyChanged(nameof(SkillReportCardButtonOpacity));
        }
    }

    private bool _isSkillReportCardVisible;

    public bool IsSkillReportCardVisible
    {
        get => _isSkillReportCardVisible;
        set
        {
            _isSkillReportCardVisible = value;
            OnPropertyChanged(nameof(IsSkillReportCardVisible));
        }
    }

    private bool _isReportCardVisible;

    public bool IsReportCardVisible
    {
        get => _isReportCardVisible;
        set
        {
            _isReportCardVisible = value;
            OnPropertyChanged(nameof(IsReportCardVisible));
        }
    }

    #endregion

    public ReportCardForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null,
        null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }


    #region Private Methods

    private async void InitializePage()
    {
        ReportCardButtonOpacity = 1.0m;
        SkillReportCardButtonOpacity = 0.5m;
        ReportCardCommand = new Command(ReportCardCommandClicked);
        SkillReportCardCommand = new Command(SkillReportCardClicked);
        MessagingCenter.Subscribe<string>(this, "NavigateToReportCard", async (res) =>
        {
            InitializePage();
            await GetYearList(true);
        });
        BackVisible = false;
        MenuVisible = true;
        AttachmentClickCommand = new Command(AttachmentClicked);
        DownloadTappedCommand = new Command(DownloadClicked);
        FilterClickCommand = new Command(FilterClicked);
        SearchClickCommand = new Command(SearchClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        MessagingCenter.Subscribe<ReportCardForm, int>(this, "TabPosition", (s, position) =>
        {
            if (position == 0)
            {
                ReportCardOption = ReportCardOptions.MarksReportCard;
                YearText = ReportCardData.PreviousReportCardSettings.ShowPrevious
                    ? " ( " + SelectedAcademicYear.ItemName + " ) "
                    : string.Empty;
            }

            if (position == 1 || (position == 0 && !ReportCardData.IsMarksReportCardEnabled &&
                                  ReportCardData.IsSkillsReportCardEnabled))
            {
                YearText = ReportCardData.PreviousReportCardSettings.PreviousSkillReport
                    ? " ( " + SelectedAcademicYear.ItemName + " ) "
                    : string.Empty;
                ReportCardOption = ReportCardOptions.SkillsReportCard;
            }
        });
        MessagingCenter.Subscribe<ReportCardForm>(this, "ReportFilterBackClicked", e =>
        {
            MenuVisible = true;
            BackVisible = false;
            IsPopUpPage = false;
            PageTitle = ReportPageTitle;
        });
        SetBeamAppViews();
    }

    private void ReportCardCommandClicked(object obj)
    {
        ReportCardOption = ReportCardOptions.MarksReportCard;
        IsSkillReportCardVisible = false;
        IsReportCardVisible = !NoDataFoundFrameVisibility;
        ReportCardButtonOpacity = 1.0m;
        SkillReportCardButtonOpacity = 0.5m;
    }

    private void SkillReportCardClicked(object obj)
    {
        ReportCardOption = ReportCardOptions.SkillsReportCard;
        IsSkillReportCardVisible = !NoDataFoundFrameVisibility;
        IsReportCardVisible = false;
        ReportCardButtonOpacity = 0.5m;
        SkillReportCardButtonOpacity = 1.0m;
    }

    private async void FilterClicked(object obj)
    {
        IsMarksTermVisible = ReportCardOption == ReportCardOptions.MarksReportCard;
        IsPopUpPage = true;
        MenuVisible = false;
        BackVisible = true;
        SelectedTermMarksReport = null;
        SelectedTermSkillReport = null;
        SelectedAcademicYear = SelectedAcademicYear != null && !string.IsNullOrEmpty(SelectedAcademicYear.ItemId)
            ? SelectedAcademicYear
            : AcademicYearList != null &&
              AcademicYearList.Where(x => x.ItemId.Contains(ReportCardData.CurrentAcademicBeginYear.ToString())) != null
                ? AcademicYearList.Where(x => x.ItemId.Contains(ReportCardData.CurrentAcademicBeginYear.ToString()))
                    .FirstOrDefault()
                : new ExtPickListItem();

        IsAcademicYearsVisible = (ReportCardOption == ReportCardOptions.MarksReportCard &&
                                  ReportCardData.PreviousReportCardSettings.ShowPrevious) ||
                                 (ReportCardOption == ReportCardOptions.SkillsReportCard &&
                                  ReportCardData.PreviousReportCardSettings.PreviousSkillReport);
        ReportPageTitle = PageTitle;
        PageTitle = ReportCardOption == ReportCardOptions.MarksReportCard
            ? TextResource.ReportCardText
            : TextResource.SkillReportCardText;
        ReportCardFilterPage reportCardFilterPage = new ()
        {
            BindingContext = this
        };
        await Navigation.PushAsync(reportCardFilterPage);
        //await PopupNavigation.Instance.PushAsync(new ReportCardFilterPage(this), true);
    }

    private async void SearchClicked(object obj)
    {
        try
        {
            MenuVisible = true;
            BackVisible = false;
            IsPopUpPage = false;
            PageTitle = ReportPageTitle;
            if (ReportCardOption == ReportCardOptions.MarksReportCard)
            {
                TermSelectionErrorMessageVisibility = SelectedTermMarksReport == null || string.IsNullOrEmpty(SelectedTermMarksReport.TermName) ? true : false;
                await SetTermLabelVisibility();
            }
            else if (ReportCardOption == ReportCardOptions.SkillsReportCard)
            {
                TermSelectionErrorMessageVisibility = SelectedTermSkillReport == null || string.IsNullOrEmpty(SelectedTermSkillReport.TermName) ? true : false;
                await SetTermLabelVisibility();
            }
            if (!TermSelectionErrorMessageVisibility)
            {
                ReportCardData = await GetStudentReportCard(null);
                var currentPage = Navigation.NavigationStack.LastOrDefault();
                if (currentPage != null)
                {
                    Navigation.RemovePage(currentPage);  
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.FilterReportCardTitle);
        }
    }

    private async Task<ReportCardSettingsView> GetTermList(string selectedYear)
    {
        try
        {
            ReportCardData = await ApiHelper.GetObject<ReportCardSettingsView>(
                string.Format(TextResource.ReportCardApiUrl,
                    AppSettings.Current.SelectedStudentFromAllStudentList.ItemId, null, null, selectedYear),
                _isEnableCaching);
            if (ReportCardData != null)
            {
                IsReporCardsAvailable = !ReportCardData.IsReportCardBlocked;

                if (IsReporCardsAvailable)
                {
                    if (ReportCardData.MarksReportCardActiveTermsList != null &&
                        ReportCardData.MarksReportCardActiveTermsList.Count() > 0)
                        MarksReportCardActiveTermsList = ReportCardData.MarksReportCardActiveTermsList;
                    if (ReportCardData.SkillsReportCardActiveTermsList != null &&
                        ReportCardData.SkillsReportCardActiveTermsList.Count() > 0)
                        SkillsReportCardActiveTermsList = ReportCardData.SkillsReportCardActiveTermsList;
                }
            }
            else
            {
                ReportCardData = new ReportCardSettingsView();
                IsReporCardsAvailable = false;
                ReportCardData.ReportCardBlockedMessage = TextResource.NoDataFound;
            }

            return ReportCardData;
        }
        catch (Exception ex)
        {
            ReportCardData = new ReportCardSettingsView();
            IsReporCardsAvailable = false;
            ReportCardData.ReportCardBlockedMessage = TextResource.NoDataFound;
            HelperMethods.DisplayException(ex, TextResource.ReportCardText);
            return ReportCardData;
        }
    }

    private async Task GetYearList(bool isStudentChange = false)
    {
        try
        {
            ReportCardData = await ApiHelper.GetObject<ReportCardSettingsView>(
                string.Format(TextResource.ReportCardApiUrl,
                    AppSettings.Current.SelectedStudentFromAllStudentList.ItemId, null, null, null), _isEnableCaching);
            if (ReportCardData != null)
            {
                IsReporCardsAvailable = !ReportCardData.IsReportCardBlocked;

                if (IsReporCardsAvailable)
                {
                    SelectTermsOptionForMarksVisibility = ReportCardData.IsMarksReportCardEnabled;
                    SelectTermsOptionForSkillVisibility = ReportCardData.IsSkillsReportCardEnabled;
                    if (ReportCardOption != ReportCardOptions.MarksReportCard &&
                        ReportCardOption != ReportCardOptions.SkillsReportCard)
                    {
                        if (ReportCardData.IsMarksReportCardEnabled)
                            ReportCardOption = ReportCardOptions.MarksReportCard;
                        else if (ReportCardData.IsSkillsReportCardEnabled)
                            ReportCardOption = ReportCardOptions.SkillsReportCard;
                    }

                    if (isStudentChange)
                    {
                        ReportCardOption = ReportCardData.IsMarksReportCardEnabled
                            ? ReportCardOptions.MarksReportCard
                            : ReportCardOptions.SkillsReportCard;

                        AcademicYearList = ReportCardData.AcademicYearList;
                        SelectedAcademicYear = AcademicYearList
                            .Where(x => x.ItemId.Contains(ReportCardData.CurrentAcademicBeginYear.ToString()))
                            .FirstOrDefault();
                        MarksReportCardActiveTermsList = ReportCardData.MarksReportCardActiveTermsList;
                        MarksReportCardsVisibility = false;
                        SkillReportCardsVisibility = false;
                        TermLabelVisibility = false;
                        SkillReportCardTermLabelVisibility = false;
                        MarksReportCardFileView = new AttachmentFileView();
                        SkillsReportCardFileView = new AttachmentFileView();
                    }
                    else
                    {
                    }

                    if ((ReportCardOption == ReportCardOptions.MarksReportCard &&
                         ReportCardData.PreviousReportCardSettings.ShowPrevious) ||
                        (ReportCardOption == ReportCardOptions.SkillsReportCard &&
                         ReportCardData.PreviousReportCardSettings.PreviousSkillReport))
                        YearText = SelectedAcademicYear != null
                            ? " ( " + SelectedAcademicYear.ItemName + " ) "
                            : string.Empty;

                    if (ReportCardData.MarksReportCardActiveTermsList != null &&
                        ReportCardData.MarksReportCardActiveTermsList.Count() > 0)
                        MarksReportCardActiveTermsList = ReportCardData.MarksReportCardActiveTermsList;
                    if (ReportCardData.SkillsReportCardActiveTermsList != null &&
                        ReportCardData.SkillsReportCardActiveTermsList.Count() > 0)
                    {
                        SkillsReportCardActiveTermsList = ReportCardData.SkillsReportCardActiveTermsList;
                        SelectedTermSkillReport = SkillsReportCardActiveTermsList.FirstOrDefault();
                    }
                }
                else
                {
                    NoDataFoundFrameVisibility = true;
                    IsReportCardVisible = !NoDataFoundFrameVisibility;
                    IsSkillReportCardVisible = !NoDataFoundFrameVisibility;
                }
            }
            else
            {
                ReportCardData = new ReportCardSettingsView();
                IsReporCardsAvailable = false;
                ReportCardData.ReportCardBlockedMessage = TextResource.NoDataFound;
            }
        }
        catch (Exception ex)
        {
            ReportCardData = new ReportCardSettingsView();
            IsReporCardsAvailable = false;
            ReportCardData.ReportCardBlockedMessage = TextResource.NoDataFound;
            HelperMethods.DisplayException(ex, TextResource.ReportCardText);
        }
    }

    private async Task<ReportCardSettingsView> GetStudentReportCard(int? termId, bool isStudentChange = false)
    {
        ReportCardData = new ReportCardSettingsView();
        if (ReportCardOption == ReportCardOptions.MarksReportCard)
        {
            IsReportCardVisible = true;
            TermSelectionErrorMessageVisibility =
                SelectedTermMarksReport == null || string.IsNullOrEmpty(SelectedTermMarksReport.TermName) ? true : false;
        }
        else if (ReportCardOption == ReportCardOptions.SkillsReportCard)
        {
            IsSkillReportCardVisible = true;
            TermSelectionErrorMessageVisibility =
                SelectedTermSkillReport == null || string.IsNullOrEmpty(SelectedTermSkillReport.TermName) ? true : false;
        }
        if (TermSelectionErrorMessageVisibility) 
            return ReportCardData;
        termId = ReportCardOption == ReportCardOptions.MarksReportCard
            ? SelectedTermMarksReport.TermId
            : SelectedTermSkillReport.TermId;

        SelectedAttachment = new BindableAttachmentFileView();
        var selectedYear = isStudentChange ? null : SelectedAcademicYear.ItemId;
        try
        {
            ReportCardData = await ApiHelper.GetObject<ReportCardSettingsView>(
                string.Format(TextResource.ReportCardApiUrl,
                    AppSettings.Current.SelectedStudentFromAllStudentList.ItemId, termId, null, selectedYear), _isEnableCaching);
            if (ReportCardData != null)
            {
                IsReporCardsAvailable = !ReportCardData.IsReportCardBlocked;

                if (IsReporCardsAvailable)
                {
                    if (ReportCardOption != ReportCardOptions.MarksReportCard && ReportCardOption != ReportCardOptions.SkillsReportCard)
                    {
                        if (ReportCardData.IsMarksReportCardEnabled)
                            ReportCardOption = ReportCardOptions.MarksReportCard;
                        else if (ReportCardData.IsSkillsReportCardEnabled)
                            ReportCardOption = ReportCardOptions.SkillsReportCard;
                    }

                    if (isStudentChange)
                    {
                        ReportCardOption = ReportCardData.IsMarksReportCardEnabled
                            ? ReportCardOptions.MarksReportCard
                            : ReportCardOptions.SkillsReportCard;

                        if (ReportCardOption == ReportCardOptions.MarksReportCard)
                            MarksReportCardFileView = ReportCardData.MarksReportCardFileView;
                        
                        else if (ReportCardOption == ReportCardOptions.SkillsReportCard)
                            SkillsReportCardFileView = ReportCardData.SkillsReportCardFileView;
                        
                        AcademicYearList = ReportCardData.AcademicYearList;
                        SelectedAcademicYear = AcademicYearList
                            .Where(x => x.ItemId.Contains(ReportCardData.CurrentAcademicBeginYear.ToString()))
                            .FirstOrDefault();
                    }
                    else
                    {
                        if (ReportCardOption == ReportCardOptions.MarksReportCard)
                        {
                            MarksReportCardFileView = ReportCardData.MarksReportCardFileView;
                            MarksReportCardsVisibility =
                                MarksReportCardFileView != null && !MarksReportCardFileView.IsError ? true : false;
                        }
                        else if (ReportCardOption == ReportCardOptions.SkillsReportCard)
                        {
                            SkillsReportCardFileView = ReportCardData.SkillsReportCardFileView;
                            SkillReportCardsVisibility =
                                SkillsReportCardFileView != null && !SkillsReportCardFileView.IsError ? true : false;
                        }
                    }

                    if ((ReportCardOption == ReportCardOptions.MarksReportCard && ReportCardData.PreviousReportCardSettings.ShowPrevious) ||
                        (ReportCardOption == ReportCardOptions.SkillsReportCard && ReportCardData.PreviousReportCardSettings.PreviousSkillReport))
                        YearText = " ( " + SelectedAcademicYear.ItemName + " ) ";

                    MessagingCenter.Send(ReportCardData, "ReportCardData", isStudentChange);
                    if (ReportCardData.MarksReportCardActiveTermsList != null && ReportCardData.MarksReportCardActiveTermsList.Count() > 0)
                        MarksReportCardActiveTermsList = ReportCardData.MarksReportCardActiveTermsList;
                    if (ReportCardData.SkillsReportCardActiveTermsList != null && ReportCardData.SkillsReportCardActiveTermsList.Count() > 0)
                        SkillsReportCardActiveTermsList = ReportCardData.SkillsReportCardActiveTermsList;

                    if (ReportCardData.IsMarksReportCardEnabled && (ReportCardOption == ReportCardOptions.MarksReportCard || isStudentChange) &&
                        !MarksReportCardFileView.IsError && !string.IsNullOrEmpty(ReportCardData.MarksReportCardFileView.FilePath))
                    {
                        SelectedAttachment = _mapper.Map<BindableAttachmentFileView>(MarksReportCardFileView);
                        if (ReportCardData.PreviousReportCardSettings.ShowPrevious && SelectedAcademicYear != null &&
                            !string.IsNullOrEmpty(SelectedAcademicYear.ItemId))
                            await HelperMethods.OpenFileForPreview(MarksReportCardFileView.FilePath, _nativeServices,
                                SelectedTermMarksReport.TermId.ToString() + "_" + SelectedAcademicYear.ItemId + "_" +
                                "marksReportCard");
                        else
                            await HelperMethods.OpenFileForPreview(MarksReportCardFileView.FilePath, _nativeServices,
                                SelectedTermMarksReport.TermId.ToString() + "_" + "marksReportCard");
                    }
                    else if (ReportCardData.IsSkillsReportCardEnabled && ReportCardOption == ReportCardOptions.SkillsReportCard &&
                             !SkillsReportCardFileView.IsError && !string.IsNullOrEmpty(SkillsReportCardFileView.FilePath))
                    {
                        SelectedAttachment = _mapper.Map<BindableAttachmentFileView>(SkillsReportCardFileView);
                        if (ReportCardData.PreviousReportCardSettings.PreviousSkillReport &&
                            SelectedAcademicYear != null && !string.IsNullOrEmpty(SelectedAcademicYear.ItemId))
                            await HelperMethods.OpenFileForPreview(SkillsReportCardFileView.FilePath, _nativeServices,
                                SelectedTermSkillReport.TermId.ToString() + "_" + SelectedAcademicYear.ItemId + "_" +
                                "skillsReportCard");
                        else
                            await HelperMethods.OpenFileForPreview(SkillsReportCardFileView.FilePath, _nativeServices,
                                SelectedTermSkillReport.TermId.ToString() + "_" + "skillsReportCard");
                    }
                }
            }
            else
            {
                ReportCardData = new ReportCardSettingsView();
                IsReporCardsAvailable = false;
                ReportCardData.ReportCardBlockedMessage = TextResource.NoDataFound;
                NoDataFoundFrameVisibility = true;
            }

            //IsReportCardVisible = !NoDataFoundFrameVisibility;
            //IsSkillReportCardVisible = !NoDataFoundFrameVisibility;

            return ReportCardData;
        }
        catch (Exception ex)
        {
            ReportCardData = new ReportCardSettingsView();
            IsReporCardsAvailable = false;
            ReportCardData.ReportCardBlockedMessage = TextResource.NoDataFound;
            HelperMethods.DisplayException(ex, TextResource.ReportCardText);
            return ReportCardData;
        }
    }

    private async void AttachmentClicked(object obj)
    {
        if (obj != null)
            try
            {
                var selectedFilter = string.Empty;


                if (ReportCardOption == ReportCardOptions.MarksReportCard)
                    selectedFilter =
                        ReportCardData.PreviousReportCardSettings.ShowPrevious && SelectedAcademicYear != null &&
                        !string.IsNullOrEmpty(SelectedAcademicYear.ItemId)
                            ? AppSettings.Current.SelectedMarksReportCardTermId.ToString() + "_" +
                              SelectedAcademicYear.ItemId + "_" + "marksReportCard"
                            : AppSettings.Current.SelectedMarksReportCardTermId.ToString();
                else
                    selectedFilter =
                        ReportCardData.PreviousReportCardSettings.PreviousSkillReport && SelectedAcademicYear != null &&
                        !string.IsNullOrEmpty(SelectedAcademicYear.ItemId)
                            ? AppSettings.Current.SelectedSkillReportCardTermId.ToString() + "_" +
                              SelectedAcademicYear.ItemId + "_" + "skillsReportCard"
                            : AppSettings.Current.SelectedSkillReportCardTermId.ToString();


                await HelperMethods.OpenFileForPreview(obj.ToString(), _nativeServices, selectedFilter);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void DownloadClicked(object obj)
    {
        if (obj != null)
            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (!string.IsNullOrEmpty(SelectedAttachment.FilePath))
                        await HelperMethods.OpenFileForPreview(SelectedAttachment.FilePath, _nativeServices);
                }
                else
                {
                    if (SelectedAttachment.FileStatus == 0)
                    {
                        SelectedAttachment.FileStatus = 1;
                        var isDownloaded = await HelperMethods.DownloadFile(SelectedAttachment.FilePath);
                        if (isDownloaded) SelectedAttachment.FileStatus = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    public override async void GetStudentData()
    {
        base.GetStudentData();
        NoDataFoundFrameVisibility = false;
        await GetYearList(true);
        IsTermEnabled = false;
    }

    public override void BackClicked(object obj)
    {
        base.BackClicked(obj);
        PageTitle = ReportPageTitle;
        MenuVisible = true;
        BackVisible = false;
    }

    private void SetBeamAppViews()
    {
        var clientGroupCode = !string.IsNullOrEmpty(App.ClientGroupCode) ? App.ClientGroupCode : string.Empty;
        if (StudentList != null && StudentList.Count > 0)
            AppSettings.Current.SelectedStudentFromAllStudentList = AppSettings.Current.StudentList.FirstOrDefault();
        if (AppSettings.Current.IsParent || AppSettings.Current.IsTeacher)
        {
            AppSettings.Current.IsRegisteredStudentListVisible = false;
            AppSettings.Current.IsAllStudentListVisible = true;
        }
        else
        {
            AppSettings.Current.IsRegisteredStudentListVisible = AppSettings.Current.IsAllStudentListVisible = false;
        }
    }

    #endregion
}