using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.DigitalPass;

public class DigitalPassForm : ViewModelBase
{
    #region Declarations

    #endregion

    #region Properties

    #endregion

    public DigitalPassForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null,
        null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods

    private async void InitializePage()
    {
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
    }

    private void GetDigitalPassData()
    {
    }

    public override async void GetStudentData()
    {
        try
        {
            GetDigitalPassData();
            base.GetStudentData();
        }
        catch (Exception ex)
        {
            Helpers.HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    #endregion
}