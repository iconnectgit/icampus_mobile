using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class KGInformationforKGForm : ViewModelBase
{
    public KGInformationforKGForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null,
        null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
    }
}
