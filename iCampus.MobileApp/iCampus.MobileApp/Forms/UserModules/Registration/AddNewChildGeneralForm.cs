using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class AddNewChildGeneralForm : ViewModelBase
{
    #region Declaration
    #endregion
    #region properties
       

    #endregion
    public AddNewChildGeneralForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
    }
    #region Methods
    private async void InitializePage()
    {
    }
    #endregion
}