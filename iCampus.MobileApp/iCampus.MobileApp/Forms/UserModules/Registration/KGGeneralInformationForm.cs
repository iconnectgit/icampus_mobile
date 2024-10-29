using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class KGGeneralInformationForm : ViewModelBase
{
    public KGGeneralInformationForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null,
        null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
    }
}