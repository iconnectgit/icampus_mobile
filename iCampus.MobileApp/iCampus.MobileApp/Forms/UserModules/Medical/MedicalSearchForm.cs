using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Medical;

public class MedicalSearchForm : ViewModelBase
{
    #region Declarations

    public ICommand SearchClickCommand { get; set; }

    #endregion

    #region Properties

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

    private ObservableCollection<MedicalIncidentView> _defaultIncidantsAndAilmentsDataList = new();

    public ObservableCollection<MedicalIncidentView> DefaultIncidantsAndAilmentsDataList
    {
        get => _defaultIncidantsAndAilmentsDataList;
        set
        {
            _defaultIncidantsAndAilmentsDataList = value;
            OnPropertyChanged(nameof(DefaultIncidantsAndAilmentsDataList));
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

    private PickListItem _selectedType = new();

    public PickListItem SelectedType
    {
        get => _selectedType;
        set
        {
            _selectedType = value;
            OnPropertyChanged(nameof(SelectedType));
            if (!string.IsNullOrEmpty(SelectedType?.ItemName)) IsTypeErrVisible = false;
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

    private string _incidentType;

    public string IncidentType
    {
        get => _incidentType;
        set
        {
            _incidentType = value;
            OnPropertyChanged(nameof(IncidentType));
        }
    }

    #endregion

    public MedicalSearchForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null,
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
        GetMedicalIncidentsData();
    }

    public override async void GetStudentData()
    {
        base.GetStudentData();
        await GetMedicalIncidentsData();
        SearchClicked();
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
                    DefaultIncidantsAndAilmentsDataList =
                        new ObservableCollection<MedicalIncidentView>(IncidantsAndAilmentsData.MedicalIncidentList);
                else
                    DefaultIncidantsAndAilmentsDataList = new ObservableCollection<MedicalIncidentView>();
            }
            //IsIncidentsAndAilmentsNoDataFoundVisibility = IncidantsAndAilmentsDataList != null && IncidantsAndAilmentsDataList.Count > 0 ? false : true;
            //MedicalIncidentsTypeList = IncidantsAndAilmentsData.MedicalIncidentTypes;
            //SelectedType = (MedicalIncidentsTypeList != null && MedicalIncidentsTypeList.Count > 0) ? MedicalIncidentsTypeList.FirstOrDefault() : new PickListItem();
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
            IncidantsAndAilmentsDataList = new ObservableCollection<MedicalIncidentView>();
            //IncidentType = SelectedType?.ItemName;
            if (ValidateData())
            {
                if (SelectedType.ItemName.ToLower().Equals("Any", StringComparison.OrdinalIgnoreCase))
                    IncidantsAndAilmentsDataList = new ObservableCollection<MedicalIncidentView>(
                        DefaultIncidantsAndAilmentsDataList
                            .Where(item =>
                                DateTime.Parse(item.IncidentDate) >= FromDate.Date &&
                                DateTime.Parse(item.IncidentDate) <= ToDate.Date
                            )
                    );
                else
                    IncidantsAndAilmentsDataList = new ObservableCollection<MedicalIncidentView>(
                        DefaultIncidantsAndAilmentsDataList
                            .Where(item =>
                                item.MedicalIncidentTypeName == SelectedType.ItemName &&
                                DateTime.Parse(item.IncidentDate) >= FromDate.Date &&
                                DateTime.Parse(item.IncidentDate) <= ToDate.Date
                            )
                    );
                IsIncidentsAndAilmentsNoDataFoundVisibility =
                    IncidantsAndAilmentsDataList != null && IncidantsAndAilmentsDataList.Count > 0 ? false : true;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private bool ValidateData()
    {
        IsTypeErrVisible = SelectedType != null && SelectedType?.ItemName != null ? false : true;
        return !IsTypeErrVisible;
    }

    #endregion
}