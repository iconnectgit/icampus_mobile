using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class AddNewChildAttachmentsForm : ViewModelBase
{
    #region Declaration
    #endregion
    #region properties
       

    #endregion
    public AddNewChildAttachmentsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
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