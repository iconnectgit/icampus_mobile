using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

public class FatherDetailsForm : ViewModelBase
{
    #region Declaration
    #endregion
    #region Properties
        
    #endregion
    public FatherDetailsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }
    #region Methods
    private async void InitializePage()
    {
    }
    #endregion
}
public class SelectionListView
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
}