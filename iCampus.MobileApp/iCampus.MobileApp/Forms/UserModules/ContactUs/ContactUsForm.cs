using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.ContactUs;

public class ContactUsForm : ViewModelBase
{
    #region Declarations

    private string _webViewUrl;

    public string WebViewUrl
    {
        get => _webViewUrl;
        set
        {
            _webViewUrl = value;
            OnPropertyChanged(nameof(WebViewUrl));
        }
    }

    #endregion


    public ContactUsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null,
        null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Private Methods

    private async void InitializePage()
    {
        GetContactUsWebViewUrl();
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
    }

    private void GetContactUsWebViewUrl()
    {
        try
        {
            WebViewUrl = AppSettings.Current.PortalUrl + "/contactus";
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }
    #endregion
}