using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.Settings;

public class WebViewForm:ViewModelBase
{
    #region Properties
    string _webViewUrl;
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
    public WebViewForm(IMapper mapper, INativeServices nativeServices, INavigation navigation, string id = null) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
    }
}