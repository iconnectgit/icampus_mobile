using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Communication;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Communication;

public class CommunicationForm : ViewModelBase
{
    #region Declarations

    private Popup _currentPopup;
    private string parameter = "I";
    private int selectedMessageCount = 0;
    private List<Guid?> _messageList = new();
    public ICommand MessageTypeSelectedCommand { get; set; }
    private IList<BindableCommunicationMessageView> communicationMessageDetails;
    public ICommand AttachmentClickCommand { get; set; }
    public ICommand CircularIconClickCommand { get; set; }
    public ICommand ComposeMessageCommand { get; set; }
    public ICommand ScreenSizeChangedCommand { get; set; }
    public ICommand AttachmentListTappedCommand { get; set; }
    public ICommand DownloadTappedCommand { get; set; }

    #endregion

    #region Properties

    private IList<string> _messageTypeList = new List<string>();

    public IList<string> MessageTypeList
    {
        get => _messageTypeList;
        set
        {
            _messageTypeList = value;
            OnPropertyChanged(nameof(MessageTypeList));
        }
    }

    private IList<BindableCommunicationMessageView> _communicationMessageList;

    public IList<BindableCommunicationMessageView> CommunicationMessageList
    {
        get => _communicationMessageList;
        set
        {
            _communicationMessageList = value;
            OnPropertyChanged(nameof(CommunicationMessageList));
        }
    }

    private ObservableCollection<BindableCommunicationMessageView> _bindableCommunicationMessageList = new();

    public ObservableCollection<BindableCommunicationMessageView> BindableCommunicationMessageList
    {
        get => _bindableCommunicationMessageList;
        set
        {
            _bindableCommunicationMessageList = value;
            OnPropertyChanged(nameof(BindableCommunicationMessageList));
        }
    }

    public ICommand ListTappedCommand { get; set; }

    private IList<BindableCommunicationMessageView> _communicationMessageDetails;

    public IList<BindableCommunicationMessageView> CommunicationMessageDetails
    {
        get => _communicationMessageDetails;
        set
        {
            _communicationMessageDetails = value;
            OnPropertyChanged(nameof(CommunicationMessageDetails));
        }
    }

    private BindableCommunicationMessageView _selectedMessage = new();

    public BindableCommunicationMessageView SelectedMessage
    {
        get => _selectedMessage;
        set
        {
            _selectedMessage = value;
            OnPropertyChanged(nameof(SelectedMessage));
        }
    }

    public ICommand ListItemSelectedCommand { get; set; }

    private int _selectedMessageCount;

    public int SelectedMessageCount
    {
        get => _selectedMessageCount;
        set
        {
            _selectedMessageCount = value;
            OnPropertyChanged(nameof(SelectedMessageCount));
        }
    }

    private int _messageId;

    public int MessageId
    {
        get => _messageId;
        set
        {
            _messageId = value;
            OnPropertyChanged(nameof(MessageId));
        }
    }

    private BindableCommunicationMessageView _communicationMessage = new();

    public BindableCommunicationMessageView CommunicationMessage
    {
        get => _communicationMessage;
        set
        {
            _communicationMessage = value;
            OnPropertyChanged(nameof(CommunicationMessage));
        }
    }

    private bool _isNoMessage;

    public bool IsNoMessage
    {
        get => _isNoMessage;
        set
        {
            _isNoMessage = value;
            OnPropertyChanged(nameof(IsNoMessage));
        }
    }

    private IList<BindableAttachmentFileView> _selectedAttachmentList;

    public IList<BindableAttachmentFileView> SelectedAttachmentList
    {
        get => _selectedAttachmentList;
        set
        {
            _selectedAttachmentList = value;
            OnPropertyChanged(nameof(SelectedAttachmentList));
        }
    }

    #endregion
    

    public CommunicationForm(IMapper mapper, INativeServices nativeServices, INavigation navigation, string notificationItemId = null) : base(null, null,
        null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        NotificationItemId = notificationItemId;
        InitializePage();
    }

    private async void InitializePage()
    {
        PageTitle = TextResource.InboxText;
        MenuVisible = true;
        IsVisiblBackIconAndPageTitle = false;
        ListTappedCommand = new Command<BindableCommunicationMessageView>(ListViewTapped);
        CommunicationMessageTypeClickedEvent = new Command(MessageTypeClicked);
        AppSettings.Current.CommunicationMessageType = parameter;
        AttachmentClickCommand = new Command<BindableCommunicationMessageView>(AttachmentClicked);
        CommunicationHeaderDeleteTappedCommand = new Command(DeleteIconTapped);
        ComposeMessageCommand = new Command(ComposeMessageTapped);
        CircularIconClickCommand = new Command<BindableCommunicationMessageView>(CircularIconClicked);
        ScreenSizeChangedCommand = new Command(ScreeSizeChanged);
        MessageTypeSelectedCommand = new Command(MessageTypeSelected);
        AttachmentListTappedCommand = new Command(AttachmentListClicked);
        DownloadTappedCommand = new Command(DownloadClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        IsVisibleDropDownIcon = true;
        MessagingCenter.Subscribe<BindableCommunicationMessageView>(this, "refreshList", async (messageList) =>
        {
            if (messageList != null)
            {
                await ClearCommunicationCacheData();
                await GetCommunicationMessageList();
            }
        });
        MessagingCenter.Subscribe<string>(this, "refreshList", async (sender) =>
        {
            if (string.IsNullOrEmpty(sender))
            {
                await ClearCommunicationCacheData();
                await GetCommunicationMessageList();
            }
        });
        MessagingCenter.Subscribe<BindableCommunicationMessageView>(this, "DeleteMessage", async (deletedMessage) =>
        {
            if (deletedMessage != null)
            {
                BindableCommunicationMessageList.Remove(deletedMessage);
                await ClearCommunicationCacheData();
                await GetCommunicationMessageList();
            }
        });
        if(!string.IsNullOrEmpty(NotificationItemId))
            await GetCommunicationMessageList();
    }

    #region Methods

    private async Task GetCommunicationMessageList()
    {
        try
        {
            if (!string.IsNullOrEmpty(NotificationItemId))
                await ClearCommunicationCacheData();
            var cacheKeyPrefix = "GetCommunicationMessages_" + parameter;
            var communicationMessageView = await ApiHelper.GetObject<CommunicationMessageView>(
                string.Format(TextResource.CommunicationApiUrl, parameter),
                false, cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);


            if (_communicationMessage != null)
            {
                AppSettings.Current.MaxAllowedRecipientCount =
                    communicationMessageView.CommunicationSettings.MaxAllowedRecipientCount;
                CommunicationMessageList = _mapper?.Map<List<BindableCommunicationMessageView>>(communicationMessageView?.MessageList);
                IsNoMessage = CommunicationMessageList?.Count > 0 ? false : true;

                //Analytics.TrackEvent("CommListCOunt - " + CommunicationMessageList + " - " + DateTime.Now + " - " +
                //                    DeviceInfo.Name + " - " + DeviceInfo.Model + " - " + DeviceInfo.Platform);
            }

            if (CommunicationMessageList != null && CommunicationMessageList.Count > 0)
            {
                BindableCommunicationMessageList =
                    new ObservableCollection<BindableCommunicationMessageView>(CommunicationMessageList);
                IsNoMessage = BindableCommunicationMessageList.Count > 0 ? false : true;
                if (AppSettings.Current.CommunicationMessageType != null &&
                    AppSettings.Current.CommunicationMessageType.ToLower().Equals("s"))
                    BindableCommunicationMessageList.ToList().ForEach(i => i.IsSentMessageIconVisible = true);
                else
                    BindableCommunicationMessageList.ToList().ForEach(i => i.IsSentMessageIconVisible = false);
                if (BindableCommunicationMessageList != null && BindableCommunicationMessageList.Count > 0 &&
                    !string.IsNullOrEmpty(NotificationItemId))
                {
                    var messageView = BindableCommunicationMessageList
                        .Where(x => x.MessageId == Convert.ToInt32(NotificationItemId)).FirstOrDefault();
                    if (messageView != null)
                        ListViewTapped(messageView);
                    NotificationItemId = null;
                }
            }
            else
            {
                BindableCommunicationMessageList = new ObservableCollection<BindableCommunicationMessageView>();
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CommunicationPageTitle);
        }
    }

    private async void ListViewTapped(BindableCommunicationMessageView obj)
    {
        try
        {
            if (obj != null)
            {
                var messageDetails = (BindableCommunicationMessageView)obj;
                var cacheKeyPrefix = $"MessageDetail_{messageDetails.MessageId}";
                CommunicationMessageDetails = await ApiHelper.GetObjectList<BindableCommunicationMessageView>(
                    TextResource.CommunicationDetailsAPIUrl + "?messageId=" + messageDetails.MessageId +
                    "&folderType=" + parameter,
                    cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);

                CommunicationDetailsForm communicationDetailsForm = new(_mapper, _nativeServices, Navigation)
                {
                    CommunicationMessageDetails =
                        new ObservableCollection<BindableCommunicationMessageView>(CommunicationMessageDetails),
                    PageTitle = PageTitle,
                    MenuVisible = false,
                    BackVisible = true,
                    ListViewHeight = CommunicationMessageDetails.ToList().Count * 40,
                    MessageId = messageDetails.MessageId,
                    MessageUId = messageDetails.MessageUId,
                    SelectedMessage = messageDetails
                };
                BindableCommunicationMessageList.Where(i => i.MessageId == messageDetails.MessageId).FirstOrDefault()
                    .IsRead = true;
                CommunicationDetails communicationDetails = new()
                {
                    BindingContext = communicationDetailsForm
                };
                await Navigation.PushAsync(communicationDetails);
                SelectedMessage = null;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private void MessageTypeClicked()
    {
        var communicationPopup = new CommunicationMessageTypePopup()
        {
            BindingContext = this
        };
        SetPopupInstance(communicationPopup);
        Application.Current.MainPage.ShowPopup(communicationPopup);
    }

    public void SetPopupInstance(Popup popup)
    {
        AppSettings.Current.CurrentPopup = popup;
    }

    private async void MessageTypeSelected(object messageType)
    {
        if (messageType.ToString().ToLower().Equals("inbox"))
        {
            parameter = "I";
            AppSettings.Current.CommunicationMessageType = parameter;
            PageTitle = TextResource.InboxText;
            SelectedMessageCount = 0;
            _messageList.Clear();
            SelectedMessageCountCheck();
            await GetCommunicationMessageList();
        }
        else if (messageType.ToString().ToLower().Equals("sent"))
        {
            parameter = "S";
            AppSettings.Current.CommunicationMessageType = parameter;
            PageTitle = TextResource.SentText;
            SelectedMessageCount = 0;
            _messageList.Clear();
            SelectedMessageCountCheck();
            await GetCommunicationMessageList();
        }

        AppSettings.Current.CurrentPopup?.Close();
    }

    private async void AttachmentClicked(BindableCommunicationMessageView sender)
    {
        try
        {
            AttachmentListPopupForm attachmentListPopupForm = new(_mapper, _nativeServices, Navigation)
            {
                SelectedAttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(sender.AttachmentList)
            };
            var attachmentListPopup = new AttachmentListPopup()
            {
                BindingContext = attachmentListPopupForm
            };
            SetPopupInstance(attachmentListPopup);
            await Application.Current.MainPage.ShowPopupAsync(attachmentListPopup);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void CircularIconClicked(BindableCommunicationMessageView sender)
    {
        var isSelectedStatus = BindableCommunicationMessageList.Where(i => i.MessageId == sender.MessageId)
            .FirstOrDefault().IsSelected;
        var selectedMessage = BindableCommunicationMessageList.Where(i => i.MessageId == sender.MessageId)
            .FirstOrDefault();
        if (isSelectedStatus)
        {
            selectedMessage.CircularImageSource = "unselected_circle_icon";
            selectedMessage.MessageBackgroundColor = Colors.White;
            selectedMessage.IsSelected = false;
            SelectedMessageCount--;
            SelectedMessageCountCheck();
            _messageList.Remove(_messageList.Single(i => i == sender.MessageUId));
        }
        else
        {
            selectedMessage.CircularImageSource = "blue_circle_icon";
            selectedMessage.MessageBackgroundColor = Color.FromHex("#E0E0E0");
            selectedMessage.IsSelected = true;
            SelectedMessageCount++;
            SelectedMessageCountCheck();
            _messageList.Add(sender.MessageUId);
        }
    }

    private void SelectedMessageCountCheck()
    {
        IsVisibleCommunicationHeaderDeleteIcon =
            IsVisibleSelectedMessageCount = SelectedMessageCount > 0 ? true : false;
        BeamHeaderMessageIconVisibility =
            BeamHeaderNotificationIconVisibility = !IsVisibleCommunicationHeaderDeleteIcon;
    }

    private async void DeleteIconTapped()
    {
        var deleteTapAction = await Application.Current.MainPage.DisplayAlert(TextResource.DeleteConfirmationTitle,
            TextResource.DeleteConfirmationMessageText, TextResource.YesText, TextResource.NoText);
        if (deleteTapAction)
            try
            {
                var operationDetails = await ApiHelper.PostRequest<OperationDetails>(
                    string.Format(TextResource.DeleteMessageAPIUrl, string.Join(",", _messageList), true));
                if (operationDetails.Success)
                {
                    SelectedMessageCount = 0;
                    _messageList.Clear();
                    SelectedMessageCountCheck();
                    await ClearCommunicationCacheData();
                    await GetCommunicationMessageList();
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void ComposeMessageTapped()
    {
        var recipientList = await GetAndCacheRecipientsList();
        var sendMessageForm = new SendMessageForm(_mapper, _nativeServices, Navigation)
        {
            RecipientList = new ObservableCollection<CommunicationMessageView>(recipientList),
            IsVisibleInfoIcon = AppSettings.Current.IsTeacher ? true : false,
            IsEnabledRecipientSelection = true,
            IsEnabledFromField = AppSettings.Current.IsParent ? true : false
        };
        var sendMessagePage = new SendMessagePage()
        {
            BindingContext = sendMessageForm
        };
        await Navigation.PushAsync(sendMessagePage);
    }

    private async Task<List<CommunicationMessageView>> GetAndCacheRecipientsList()
    {
        var cacheKeyPrefix = "recipientcachelist";
        var studentId = AppSettings.Current.IsParent ? AppSettings.Current.SelectedStudent.ItemId : null;
        var RecipientCacheList = new List<CommunicationMessageView>();
        try
        {
            if (!AppSettings.Current.IsParent)
            {
                var RecipientListData = await ApiHelper.GetObjectList<CommunicationMessageView>(
                    string.Format(TextResource.RecipientListApiUrl, studentId, null, 0, null),
                    cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
                RecipientCacheList = RecipientListData;
                return RecipientCacheList;
            }

            return RecipientCacheList;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            return RecipientCacheList;
        }
    }

    private async Task ClearCommunicationCacheData()
    {
        var allKeys = await ICCacheManager.GetAllKeys();
        foreach (var key in allKeys)
            if (key.StartsWith("GetCommunicationMessages"))
                ICCacheManager.InvalidateObject<CampusKeyView>(key);
    }

    private void ScreeSizeChanged()
    {
        BindableCommunicationMessageList =
            new ObservableCollection<BindableCommunicationMessageView>(BindableCommunicationMessageList);
    }

    private async void AttachmentListClicked(object obj)
    {
        if (obj != null)
            try
            {
                if (Device.RuntimePlatform == Device.iOS) _currentPopup?.Close();
                var selectedAttachment = (BindableAttachmentFileView)obj;
                if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                    await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void DownloadClicked(object obj)
    {
        if (obj != null)
            try
            {
                var selectedAttachment = (BindableAttachmentFileView)obj;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    _currentPopup?.Close();
                    if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                        await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
                }
                else
                {
                    if (selectedAttachment.FileStatus == 0)
                    {
                        SelectedAttachmentList[SelectedAttachmentList.IndexOf(selectedAttachment)].FileStatus = 1;
                        var filePath = await HelperMethods.DownloadAndReturnFilePath(selectedAttachment.FilePath, _nativeServices);
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            SelectedAttachmentList[SelectedAttachmentList.IndexOf(selectedAttachment)].FileDevicePath =
                                filePath;
                            SelectedAttachmentList[SelectedAttachmentList.IndexOf(selectedAttachment)].FileStatus = 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }
    public override async void GetStudentData()
    {
        try
        {
            await GetCommunicationMessageList();
            base.GetStudentData();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }
    #endregion
}