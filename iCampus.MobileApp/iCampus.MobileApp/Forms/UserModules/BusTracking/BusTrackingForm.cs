using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.BusTracking;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.BusTracking;

public class BusTrackingForm : ViewModelBase
{
    #region Declarations

    public ICommand ListTappedCommand { get; set; }
    public ICommand TrackButtonCommand { get; set; }

    #endregion

    #region Properties

    private BusTrackingView _busTrackingInformation = new();

    public BusTrackingView BusTrackingInformation
    {
        get => _busTrackingInformation;
        set
        {
            _busTrackingInformation = value;
            OnPropertyChanged(nameof(BusTrackingInformation));
        }
    }

    private BusTrackingView _selectedBusTracking = new();

    public BusTrackingView SelectedBusTracking
    {
        get => _selectedBusTracking;
        set
        {
            _selectedBusTracking = value;
            OnPropertyChanged(nameof(SelectedBusTracking));
        }
    }

    private BusTrackingView _currentStudentBusTracking = new();

    public BusTrackingView CurrentStudentBusTracking
    {
        get => _currentStudentBusTracking;
        set
        {
            _currentStudentBusTracking = value;
            OnPropertyChanged(nameof(CurrentStudentBusTracking));
        }
    }

    private IList<BusAttendanceView> _busAttendanceList;

    public IList<BusAttendanceView> BusAttendanceList
    {
        get => _busAttendanceList;
        set
        {
            _busAttendanceList = value;
            OnPropertyChanged(nameof(BusAttendanceList));
        }
    }

    public string BusAttendanceHeaderTitle { get; set; }

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

    private bool _pickUpDataAvailable;

    public bool PickUpDataAvailable
    {
        get => _pickUpDataAvailable;
        set
        {
            _pickUpDataAvailable = value;
            OnPropertyChanged(nameof(PickUpDataAvailable));
        }
    }

    private bool _dropOffDataAvailable;

    public bool DropOffDataAvailable
    {
        get => _dropOffDataAvailable;
        set
        {
            _dropOffDataAvailable = value;
            OnPropertyChanged(nameof(DropOffDataAvailable));
        }
    }

    #endregion

    public BusTrackingForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null,  null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region private Methods

    private async void InitializePage()
    {
        FormTitle = TextResource.BusTrackingPageTitle;
        BackVisible = false;
        MenuVisible = true;
        IsVisiblBackIconAndPageTitle = false;
        TrackButtonCommand = new Command(TrackBusButtonClicked);
        PickUpDataAvailable = DropOffDataAvailable = false;
        NoDataExist = false;
    }

    public async Task<BusTrackingView> GetBusTrackingInformation()
    {
        try
        {
            //SideMenuPanelForm objForm = new SideMenuPanelForm();
            BusTrackingInformation = await ApiHelper.GetObject<BusTrackingView>(
                string.Format(TextResource.BusTrackingApiUrl, AppSettings.Current.SelectedStudent.ItemId));
            CurrentStudentBusTracking = BusTrackingInformation.BusTrackingList.FirstOrDefault();
            PickUpDataAvailable = BusTrackingInformation.PickUpBusId.HasValue;
            DropOffDataAvailable = BusTrackingInformation.DropOffBusID.HasValue;
            NoDataExist = !PickUpDataAvailable && !DropOffDataAvailable;
            return BusTrackingInformation;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.BusTrackingPageTitle);
            return new BusTrackingView();
        }
    }

    private async void TrackBusButtonClicked(object parameter)
    {
        var result = parameter.ToString().ToLower();
        if (BusTrackingInformation.BusInformationSettings.IsBusInformationEnabled)
        {
            var thirdPartyWebUrl = result == "pickup" ? BusTrackingInformation.PickUpBusTrackingCompanyApplicationLink :
                result == "dropoff" ? BusTrackingInformation.DropOffBusTrackingCompanyApplicationLink : "";
            if (!string.IsNullOrEmpty(thirdPartyWebUrl))
                Launcher.Default.OpenAsync(new Uri(thirdPartyWebUrl));
        }
        else
        {
            var isPickup = parameter.ToString().ToLower().Equals("pickup") ? true : false;
            var busId = isPickup ? BusTrackingInformation.PickUpBusId : BusTrackingInformation.DropOffBusID;
            var shiftId = isPickup ? BusTrackingInformation.PickUpBusShiftId : BusTrackingInformation.DropOffBusShiftId;
            BusAttendanceHeaderTitle = string.Format(TextResource.BusAttendanceHeaderTitle,
                isPickup ? TextResource.ToSchool : TextResource.FromSchool);
            if (shiftId.HasValue)
            {
                var apiUrl = string.Format(TextResource.BusAttendanceDetailsAPIUrl,
                    AppSettings.Current.SelectedStudent.ItemId, busId, shiftId);
                BusAttendanceList = await ApiHelper.GetObjectList<BusAttendanceView>(apiUrl);
                BusTrackingDetailForm busTrackingDetailForm = new(_mapper, _nativeServices, Navigation)
                {
                    BusAttendanceList = BusAttendanceList,
                    BackVisible = true,
                    MenuVisible = false,
                    IsVisiblBackIconAndPageTitle = false,
                    PageTitle = BusAttendanceHeaderTitle
                };
                var busTrackingDetailPage = new BusTrackingDetailPage()
                {
                    BindingContext = busTrackingDetailForm
                };
                await Navigation.PushAsync(busTrackingDetailPage);
            }
        }
    }

    public override async void GetStudentData()
    {
        base.GetStudentData();
        await GetBusTrackingInformation();
    }

    #endregion
}