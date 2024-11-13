using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.MessageFromSchool;

public class AlertsForm : ViewModelBase
{
    public AlertsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    private void InitializePage()
    {
        this.PageTitle = TextResource.AlertsPageTitle;
    }
}