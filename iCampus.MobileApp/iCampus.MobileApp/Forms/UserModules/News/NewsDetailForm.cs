using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.News;

public class NewsDetailForm:ViewModelBase
    {
        #region Declarations
        public ICommand AttachmentClickCommand { get; set; }
        public ICommand DownloadTappedCommand { get; set; }
        public ICommand WebsiteLinksTappedCommand { get; set; }

        #endregion

        #region Properties
        BindableSiteNewsView siteNewsObject = new BindableSiteNewsView();
        
        public BindableSiteNewsView SiteNewsObject
        {
            get => siteNewsObject;
            set
            {
                siteNewsObject = value;
                OnPropertyChanged(nameof(SiteNewsObject));
            }
        }

        WebsiteLinkView _selectedWebsiteLink;
        public WebsiteLinkView SelectedWebsiteLink
        {
            get => _selectedWebsiteLink;
            set
            {
                _selectedWebsiteLink = value;
                OnPropertyChanged(nameof(SelectedWebsiteLink));
            }
        }

        ObservableCollection<BindableAttachmentFileView> _attachments = new ();
        public ObservableCollection<BindableAttachmentFileView> Attachments
        {
            get => _attachments;
            set
            {
                _attachments = value;
                OnPropertyChanged(nameof(Attachments));
            }
        }
        private double _webViewHeight;
        public double WebViewHeight
        {
            get => _webViewHeight;
            set
            {
                if (_webViewHeight != value)
                {
                    _webViewHeight = value;
                    OnPropertyChanged(nameof(WebViewHeight));
                }
            }
        }
        private string _formattedNewsData;
        public string FormattedNewsData
        {
            get => _formattedNewsData;
            set
            {
                if (_formattedNewsData != value)
                {
                    _formattedNewsData = value;
                    OnPropertyChanged(nameof(FormattedNewsData));
                }
            }
        }
        BindableAttachmentFileView _selectedattachments;
        public BindableAttachmentFileView SelectedAttachments
        {
            get => _selectedattachments;
            set
            {
                _selectedattachments = value;
                OnPropertyChanged(nameof(SelectedAttachments));
            }
        }
        #endregion

        public NewsDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        private void InitializePage()
        {
            this.BackVisible = true;
            this.BackTitle = TextResource.BackTitle;
            this.PageTitle = TextResource.NewsPageTitle;
            this.AttachmentClickCommand = new Command<BindableAttachmentFileView>(AttachmentClicked);
            this.DownloadTappedCommand = new Command<BindableAttachmentFileView>(DownloadClicked);
            this.WebsiteLinksTappedCommand = new Command<WebsiteLinkView>(WebsiteLinkClicked);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            //MessagingCenter.Subscribe<string>("", "ListViewRightSwipeNewsDetails", async (arg) =>
            //{
            //  await SideMenuClicked();
            //    MessagingCenter.Unsubscribe<string>("", "ListViewRightSwipeNewsDetails");

            //});
            MessagingCenter.Subscribe<string>("", "ListViewRightSwipeNewsDetailsSubscribe", (arg) =>
            {
                MessagingCenter.Subscribe<string>("", "ListViewRightSwipeNewsDetails", async (argu) =>
                {
                    //await SideMenuClicked();
                });

            });
        }

        private async void AttachmentClicked(BindableAttachmentFileView obj)
        {
            if (obj != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(obj.FilePath))
                        await HelperMethods.OpenFileForPreview(obj.FilePath, _nativeServices);
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                }
            }
        }
        private async void DownloadClicked(BindableAttachmentFileView obj)
        {
            try
                {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (!string.IsNullOrEmpty(obj.FilePath))
                        await HelperMethods.OpenFileForPreview(obj.FilePath, _nativeServices);
                }
                else
                {
                    if (obj.FileStatus == 0)
                    {
                        obj.FileStatus = 1;
                        string filePath = await HelperMethods.DownloadAndReturnFilePath(obj.FilePath, _nativeServices);
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            obj.FileDevicePath = filePath;
                            obj.FileStatus = 2;
                        }
                    }
                }
            }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                }
        }
        private void WebsiteLinkClicked(WebsiteLinkView sender)
        {
            if (sender != null)
            {
               HelperMethods.OpenWebsiteLinks(sender.Url,this.PageTitle,SiteNewsObject.IsInternalPage);
               SelectedWebsiteLink = null;
            }
        }
    }