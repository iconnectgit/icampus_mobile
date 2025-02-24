using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Medical;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Medical;

public class MedicalForm : ViewModelBase
{
    #region Declarations

    public ICommand IncidentsTabbedCommand { get; set; }
    public ICommand VaccinationTabbedCommand { get; set; }
    public ICommand SearchClickCommand { get; set; }
    public ICommand FilterClickCommand { get; set; }

    #endregion

    #region Properties

    private ObservableCollection<MedicalIncidentView> _incidantsAndAilmentsDataList = new();

    public ObservableCollection<MedicalIncidentView> IncidantsAndAilmentsDataList
    {
        get => _incidantsAndAilmentsDataList;
        set
        {
            _incidantsAndAilmentsDataList = value;
            OnPropertyChanged(nameof(IncidantsAndAilmentsDataList));
        }
    }

    private StudentMedicalInfoView _incidantsAndAilmentsData = new();

    public StudentMedicalInfoView IncidantsAndAilmentsData
    {
        get => _incidantsAndAilmentsData;
        set
        {
            _incidantsAndAilmentsData = value;
            OnPropertyChanged(nameof(IncidantsAndAilmentsData));
        }
    }

    private StudentMedicalInfoView _vaccinationData = new();

    public StudentMedicalInfoView VaccinationData
    {
        get => _vaccinationData;
        set
        {
            _vaccinationData = value;
            OnPropertyChanged(nameof(VaccinationData));
        }
    }

    private ObservableCollection<StudentImmunizationView> _vaccinationRecordsList = new();

    public ObservableCollection<StudentImmunizationView> VaccinationRecordsList
    {
        get => _vaccinationRecordsList;
        set
        {
            _vaccinationRecordsList = value;
            OnPropertyChanged(nameof(VaccinationRecordsList));
        }
    }

    private string _medicalPageTitle;

    public string MedicalPageTitle
    {
        get => _medicalPageTitle;
        set
        {
            _medicalPageTitle = value;
            OnPropertyChanged(nameof(MedicalPageTitle));
        }
    }

    private bool _isIncidentsAndAilmentsNoDataFoundVisibility;

    public bool IsIncidentsAndAilmentsNoDataFoundVisibility
    {
        get => _isIncidentsAndAilmentsNoDataFoundVisibility;
        set
        {
            _isIncidentsAndAilmentsNoDataFoundVisibility = value;
            OnPropertyChanged(nameof(IsIncidentsAndAilmentsNoDataFoundVisibility));
        }
    }

    private bool _isVaccinationNoDataFoundVisibility;

    public bool IsVaccinationNoDataFoundVisibility
    {
        get => _isVaccinationNoDataFoundVisibility;
        set
        {
            _isVaccinationNoDataFoundVisibility = value;
            OnPropertyChanged(nameof(IsVaccinationNoDataFoundVisibility));
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

    private IList<PickListItem> _medicalIncidentsTypeList = new List<PickListItem>();

    public IList<PickListItem> MedicalIncidentsTypeList
    {
        get => _medicalIncidentsTypeList;
        set
        {
            _medicalIncidentsTypeList = value;
            OnPropertyChanged(nameof(MedicalIncidentsTypeList));
        }
    }

    private IList<PickListItem> _vaccinationPeriodsList = new List<PickListItem>();

    public IList<PickListItem> VaccinationPeriodsList
    {
        get => _vaccinationPeriodsList;
        set
        {
            _vaccinationPeriodsList = value;
            OnPropertyChanged(nameof(VaccinationPeriodsList));
        }
    }

    private string _incidentType = string.Empty;

    public string IncidentType
    {
        get => _incidentType;
        set
        {
            _incidentType = value;
            OnPropertyChanged(nameof(IncidentType));
        }
    }

    private string _vaccinationPeriod = string.Empty;

    public string VaccinationPeriod
    {
        get => _vaccinationPeriod;
        set
        {
            _vaccinationPeriod = value;
            OnPropertyChanged(nameof(VaccinationPeriod));
        }
    }

    private bool _isIncidentFilterPage = true;

    public bool IsIncidentFilterPage
    {
        get => _isIncidentFilterPage;
        set
        {
            _isIncidentFilterPage = value;
            OnPropertyChanged(nameof(IsIncidentFilterPage));
        }
    }

    private bool _isTypeErrVisible;

    public bool IsTypeErrVisible
    {
        get => _isTypeErrVisible;
        set
        {
            _isTypeErrVisible = value;
            OnPropertyChanged(nameof(IsTypeErrVisible));
        }
    }

    private bool _isPeriodErrVisible;

    public bool IsPeriodErrVisible
    {
        get => _isPeriodErrVisible;
        set
        {
            _isPeriodErrVisible = value;
            OnPropertyChanged(nameof(IsPeriodErrVisible));
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
            if (!string.IsNullOrEmpty(SelectedType.ItemName)) IsTypeErrVisible = false;
        }
    }

    private PickListItem _selectedPeriod = new();

    public PickListItem SelectedPeriod
    {
        get => _selectedPeriod;
        set
        {
            _selectedPeriod = value;
            OnPropertyChanged(nameof(SelectedPeriod));
            if (!string.IsNullOrEmpty(SelectedPeriod.ItemName))
                IsPeriodErrVisible = false;
        }
    }
    
    private decimal _incidentstButtonOpacity;

    public decimal IncidentstButtonOpacity
    {
        get => _incidentstButtonOpacity;
        set
        {
            _incidentstButtonOpacity = value;
            OnPropertyChanged(nameof(IncidentstButtonOpacity));
        }
    }

    private decimal _vaccinationButtonOpacity;

    public decimal VaccinationButtonOpacity
    {
        get => _vaccinationButtonOpacity;
        set
        {
            _vaccinationButtonOpacity = value;
            OnPropertyChanged(nameof(VaccinationButtonOpacity));
        }
    }

    private bool _isIncidentsVisible;

    public bool IsIncidentsVisible
    {
        get => _isIncidentsVisible;
        set
        {
            _isIncidentsVisible = value;
            OnPropertyChanged(nameof(IsIncidentsVisible));
        }
    }

    private bool _isVaccinationVisible;

    public bool IsVaccinationVisible
    {
        get => _isVaccinationVisible;
        set
        {
            _isVaccinationVisible = value;
            OnPropertyChanged(nameof(IsVaccinationVisible));
        }
    }
    private bool _isSearchButtonVisible;

    public bool IsSearchButtonVisible
    {
        get => _isSearchButtonVisible;
        set
        {
            _isSearchButtonVisible = value;
            OnPropertyChanged(nameof(IsSearchButtonVisible));
        }
    }


    #endregion

    public MedicalForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods

    private async void InitializePage()
    {
        HelperMethods.GetSelectedStudent();
        IsSearchButtonVisible = true;
        IsVaccinationVisible = false;
        IsIncidentsVisible = true;
        IncidentstButtonOpacity = 1.0m;
        VaccinationButtonOpacity = 0.5m;
        IncidentsTabbedCommand = new Command(IncidentsTabbedClicked);
        VaccinationTabbedCommand = new Command(VaccinationTabbedClicked);
        SearchClickCommand = new Command(SearchClicked);
        FilterClickCommand = new Command(FilterClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        if (IsIncidentFilterPage)
        {
            ToDate = DateTime.Now;
            FromDate = ToDate.AddYears(-1);
        }

        MessagingCenter.Subscribe<MedicalForm, int>(this, "MedicalTabPosition", (s, position) =>
        {
            if (position == 0) IsIncidentFilterPage = true;
            if (position == 1) IsIncidentFilterPage = false;
        });
    }

    private void IncidentsTabbedClicked()
    {
        IsVaccinationVisible = false;
        IsIncidentsVisible = true;
        IncidentstButtonOpacity = 1.0m;
        VaccinationButtonOpacity = 0.5m;
        IsSearchButtonVisible = true;
    }

    private void VaccinationTabbedClicked()
    {
        IsVaccinationVisible = true;
        IsIncidentsVisible = false;
        IncidentstButtonOpacity = 0.5m;
        VaccinationButtonOpacity = 1.0m;
        IsSearchButtonVisible = false;
    }
    public override async void GetStudentData()
    {
        base.GetStudentData();
        await GetMedicalIncidentsData();
        await GetMedicalVaccinationDetails();
    }

    public override void BackClicked(object obj)
    {
        base.BackClicked(obj);
        PageTitle = MedicalPageTitle;
        MenuVisible = true;
        BackVisible = false;
    }

    private async Task GetMedicalIncidentsData()
    {
        try
        {
            var loadFilterPanelLists = true;
            var apiUrl = string.Format(TextResource.GetStudentMedicalIncidents,
                AppSettings.Current.SelectedStudent.ItemId, "", "", IncidentType, loadFilterPanelLists);
            IncidantsAndAilmentsData = await ApiHelper.GetObject<StudentMedicalInfoView>(apiUrl,
                cacheKeyPrefix: "medicalincidentdata", cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
            if (IncidantsAndAilmentsData != null)
            {
                if (IncidantsAndAilmentsData.MedicalIncidentList != null &&
                    IncidantsAndAilmentsData.MedicalIncidentList.Count > 0)
                    IncidantsAndAilmentsDataList =
                        new ObservableCollection<MedicalIncidentView>(IncidantsAndAilmentsData.MedicalIncidentList);
                else
                    IncidantsAndAilmentsDataList = new ObservableCollection<MedicalIncidentView>();
            }

            IsIncidentsAndAilmentsNoDataFoundVisibility = !IncidantsAndAilmentsDataList.Any();
            if (loadFilterPanelLists)
            {
                MedicalIncidentsTypeList = IncidantsAndAilmentsData.MedicalIncidentTypes;
                SelectedType = MedicalIncidentsTypeList != null && MedicalIncidentsTypeList.Count > 0
                    ? MedicalIncidentsTypeList.FirstOrDefault()
                    : new PickListItem();
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async Task GetMedicalVaccinationDetails()
    {
        try
        {
            var loadFilterPanelLists = !VaccinationPeriodsList.Any();
            var apiUrl = string.Format(TextResource.GetStudentMedicalVaccinationDetails,
                AppSettings.Current.SelectedStudent.ItemId, "", "", VaccinationPeriod, loadFilterPanelLists);
            VaccinationData = await ApiHelper.GetObject<StudentMedicalInfoView>(apiUrl,
                cacheKeyPrefix: "medicalvaccinationdata", cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
            if (VaccinationData != null)
            {
                if (VaccinationData.VaccinationList != null && VaccinationData.VaccinationList.ToList().Count > 0)
                    VaccinationRecordsList =
                        new ObservableCollection<StudentImmunizationView>(VaccinationData.VaccinationList);
                else
                    VaccinationRecordsList = new ObservableCollection<StudentImmunizationView>();
            }

            IsVaccinationNoDataFoundVisibility =
                VaccinationRecordsList != null && VaccinationRecordsList.Count > 0 ? false : true;
            if (loadFilterPanelLists)
            {
                VaccinationPeriodsList = VaccinationData.VaccinationPeriods;
                SelectedPeriod = VaccinationPeriodsList != null && VaccinationPeriodsList.Count > 0
                    ? VaccinationPeriodsList.FirstOrDefault()
                    : new PickListItem();
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void SearchClicked(object obj)
    {
        try
        {
            IncidentType = SelectedType.ItemName;
            if (ValidateData())
            {
                PageTitle = MedicalPageTitle;
                MenuVisible = true;
                BackVisible = false;
                IsPopUpPage = false;
                // Func<PopupPage, bool> predicate = x => x.GetType() == typeof(MedicalFilterPage);
                // if (PopupNavigation.Instance.PopupStack.Any(predicate))
                // {
                //     var popupPage = PopupNavigation.Instance.PopupStack.FirstOrDefault(predicate);
                //     await PopupNavigation.Instance.RemovePageAsync(popupPage);
                // }
                await Navigation.PopAsync();
                if (IncidentType == "Any")
                    IncidantsAndAilmentsDataList = new ObservableCollection<MedicalIncidentView>(
                        IncidantsAndAilmentsDataList
                            .Where(item =>
                                DateTime.Parse(item.IncidentDate) >= FromDate.Date &&
                                DateTime.Parse(item.IncidentDate) <= ToDate.Date
                            )
                    );
                else
                    IncidantsAndAilmentsDataList = new ObservableCollection<MedicalIncidentView>(
                        IncidantsAndAilmentsDataList
                            .Where(item =>
                                item.MedicalIncidentTypeName == IncidentType &&
                                DateTime.Parse(item.IncidentDate) >= FromDate.Date &&
                                DateTime.Parse(item.IncidentDate) <= ToDate.Date
                            )
                    );
                IsIncidentsAndAilmentsNoDataFoundVisibility = !IncidantsAndAilmentsDataList.Any();
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.FilterAgendaTitle);
        }
    }

    private bool ValidateData()
    {
        if (!string.IsNullOrEmpty(SelectedType.ItemName) && SelectedType.ItemName.ToLower().Equals("any"))
            IsTypeErrVisible = false;
        else
            IsTypeErrVisible = IncidentType != null && SelectedType.ItemName != null ? false : true;
        return !IsTypeErrVisible;
    }

    private async void FilterClicked(object obj)
    {
        try
        {
            MedicalSearchForm medicalSearchForm = new(_mapper, _nativeServices, Navigation)
            {
                PageTitle = TextResource.FilterMedicalTitle,
                MenuVisible = false,
                BackVisible = true,
                MedicalIncidentsTypeList = MedicalIncidentsTypeList
            };
            MedicalSearchPage medicalSearchPage = new MedicalSearchPage()
            {
                BindingContext = medicalSearchForm
            };
            await Navigation.PushAsync(medicalSearchPage);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    #endregion
}