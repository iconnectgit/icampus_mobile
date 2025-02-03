using System.Collections.Generic;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using Microsoft.Maui.Controls;

namespace iCampus.MobileApp.Forms.PopupForms;

public class WebsiteLinksPopupForm : ViewModelBase
{
    #region Declarations       
    public ICommand WebsiteLinksTappedCommand { get; set; }
    #endregion

    #region Properties      
    IEnumerable<WebsiteLinkView> _selectedWebsiteLinks;
    public IEnumerable<WebsiteLinkView> SelectedWebsiteLinks
    {
        get => _selectedWebsiteLinks;
        set
        {
            _selectedWebsiteLinks = value;
            OnPropertyChanged(nameof(SelectedWebsiteLinks));
        }
    }

    WebsiteLinkView _currentWebsiteLink;
    public WebsiteLinkView CurrentWebsiteLink
    {
        get => _currentWebsiteLink;
        set
        {
            _currentWebsiteLink = value;
            OnPropertyChanged(nameof(CurrentWebsiteLink));
        }
    }

    bool _isInternalPage;
    public bool IsInternalPage
    {
        get => _isInternalPage;
        set
        {
            _isInternalPage = value;
            OnPropertyChanged(nameof(IsInternalPage));
        }
    }

    string _pageTitle;
    public string PageTitle
    {
        get => _pageTitle;
        set
        {
            _pageTitle = value;
            OnPropertyChanged(nameof(PageTitle));
        }
    }

    #endregion
    public WebsiteLinksPopupForm(IMapper mapper, INativeServices nativeServices, INavigation navigation, string id = null) : base(
        null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        WebsiteLinksTappedCommand = new Command<WebsiteLinkView>(WebsiteLinkClicked);
    }
    
    #region Methods
    private async void WebsiteLinkClicked(WebsiteLinkView sender)
    {
        if (sender != null)
        {
            AppSettings.Current.CurrentPopup?.Close();
            HelperMethods.OpenWebsiteLinks(sender.Url, this.PageTitle, this.IsInternalPage);
        }
    }
    #endregion
}