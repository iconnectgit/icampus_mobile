using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.Event;

public class EventDetailForm : ViewModelBase
    {
        #region Declarations
        public ICommand PreviewIconTappedCommand { get; set; }
        public ICommand DownloadTappedCommand { get; set; }
        public ICommand WebsiteLinksTappedCommand { get; set; }
        #endregion
        #region Properties
        BindableCalendarView _selectedEvent = new BindableCalendarView();
        public BindableCalendarView SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
            }
        }

        DateTime selectedDate;
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

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

        public EventDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            this.BackVisible = true;
            this.PreviewIconTappedCommand = new Command(PreviewIconClicked);
            this.DownloadTappedCommand = new Command(DownloadClicked);
            this.WebsiteLinksTappedCommand = new Command<WebsiteLinkView>(WebsiteLinkClicked);
            MessagingCenter.Subscribe<string>("", "EventDetailsSwipeSubscribe", (arg) =>
            {
                MessagingCenter.Subscribe<string>("", "EventDetailsSwipe", async (a) =>
                {
                    //await SideMenuClicked();
                });
            });
        }

        private async void DownloadClicked(object obj)
        {
            if (obj != null)
            {
                try
                {
                    var selectedAttachment = (BindableAttachmentFileView)obj;
                    if(Device.RuntimePlatform==Device.iOS)
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
        private async void PreviewIconClicked(Object obj)
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

        private void WebsiteLinkClicked(WebsiteLinkView sender)
        {
            if (sender != null)
            {
                HelperMethods.OpenWebsiteLinks(sender.Url, this.PageTitle, SelectedEvent.IsInternalPage);
                CurrentWebsiteLink = null;
            }
        }
    }