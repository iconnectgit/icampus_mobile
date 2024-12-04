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

        BindableAttachmentFileView _attachment;
        public BindableAttachmentFileView Attachment
        {
            get => _attachment;
            set
            {
                _attachment = value;
                OnPropertyChanged(nameof(Attachment));
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
            this.AttachmentClickCommand = new Command(AttachmentClicked);
            this.DownloadTappedCommand = new Command(DownloadClicked);
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

        private async void AttachmentClicked(object obj)
        {
            if (obj != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Attachment.FilePath))
                        await HelperMethods.OpenFileForPreview(Attachment.FilePath, _nativeServices);
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                }
            }
        }
        private async void DownloadClicked(object obj)
        {
            try
                {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    if (!string.IsNullOrEmpty(Attachment.FilePath))
                        await HelperMethods.OpenFileForPreview(Attachment.FilePath, _nativeServices);
                }
                else
                {
                    if (Attachment.FileStatus == 0)
                    {
                        Attachment.FileStatus = 1;
                        string filePath = await HelperMethods.DownloadAndReturnFilePath(Attachment.FilePath, _nativeServices);
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            Attachment.FileDevicePath = filePath;
                            Attachment.FileStatus = 2;
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