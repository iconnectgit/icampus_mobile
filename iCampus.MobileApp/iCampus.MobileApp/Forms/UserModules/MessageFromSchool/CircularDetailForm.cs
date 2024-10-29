using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.MessageFromSchool;

public class CircularDetailForm : ViewModelBase
    {

        #region Declaration
        public ICommand AttachmentListTappedCommand { get; set; }
        public ICommand WebsiteLinksTappedCommand { get; set; }

        public ICommand DownloadTappedCommand { get; set; }
        #endregion

        #region Properties
        int _attachmentListViewHeight;
        public int AttachmentListViewHeight
        {
            get => _attachmentListViewHeight;
            set
            {
                _attachmentListViewHeight = value;
                OnPropertyChanged(nameof(AttachmentListViewHeight));
            }
        }

        int _linkListViewHeight;
        public int LinkListViewHeight
        {
            get => _linkListViewHeight;
            set
            {
                _linkListViewHeight = value;
                OnPropertyChanged(nameof(LinkListViewHeight));
            }
        }

        CircularView circularObject = new CircularView();
        public CircularView CircularObject
        {
            get => circularObject;
            set
            {
                circularObject = value;
                OnPropertyChanged(nameof(CircularObject));
            }
        }
        IList<BindableAttachmentFileView> _attachmentList;
        public IList<BindableAttachmentFileView> AttachmentList
        {
            get => _attachmentList;
            set
            {
                _attachmentList = value;
                OnPropertyChanged(nameof(AttachmentList));
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
        #endregion
        public CircularDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        private async void InitializePage()
        {
            this.BackVisible = true;
            this.BackTitle = TextResource.BackTitle;
            this.PageTitle = TextResource.CircularsPageTitle;
            WebsiteLinksTappedCommand = new Command<WebsiteLinkView>(LinksClicked);
            AttachmentListTappedCommand = new Command(AttachmentClicked);
            this.DownloadTappedCommand = new Command(DownloadClicked);
        }

        private void LinksClicked(WebsiteLinkView sender)
        {
            if (sender != null)
            {
                HelperMethods.OpenWebsiteLinks(sender.Url, this.PageTitle, CircularObject.IsInternalPage);
                CurrentWebsiteLink = null;
            }
        }

        private async void AttachmentClicked(object obj)
        {
            if (obj != null)
            {
                try
                {
                    var selectedAttachment = (BindableAttachmentFileView)obj;
                    if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                       await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                }
            }

        }

        private async void DownloadClicked(object obj)
        {
            if (obj != null)
            {
                try
                {
                    var selectedAttachment = (BindableAttachmentFileView)obj;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                            await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
                    }
                    else
                    {
                        if (selectedAttachment.FileStatus == 0)
                        {
                            AttachmentList[AttachmentList.IndexOf(selectedAttachment)].FileStatus = 1;
                            string filePath = await HelperMethods.DownloadAndReturnFilePath(selectedAttachment.FilePath);
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                AttachmentList[AttachmentList.IndexOf(selectedAttachment)].FileDevicePath = filePath;
                                AttachmentList[AttachmentList.IndexOf(selectedAttachment)].FileStatus = 2;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                }
            }
        }
    }