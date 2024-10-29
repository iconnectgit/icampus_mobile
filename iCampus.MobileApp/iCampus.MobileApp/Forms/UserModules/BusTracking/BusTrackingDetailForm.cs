using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.BusTracking;

public class BusTrackingDetailForm : ViewModelBase
{
    #region Properties

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

    #endregion

    public BusTrackingDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null,
        null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    private async void InitializePage()
    {
        FormTitle = TextResource.BusTrackingPageTitle;
        MenuVisible = true;
        IsVisiblBackIconAndPageTitle = true;
    }
}