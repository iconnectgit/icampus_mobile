using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Communication;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Communication;

public class CommunicationDetailsForm : ViewModelBase
{
    #region Declarations
    public ICommand DownloadTappedCommand { get; set; }
    public ICommand PreviewIconTappedCommand { get; set; }
    public ICommand DeleteIconTappedCommand { get; set; }
    public ICommand ReplyIconTappedCommand { get; set; }
    public ICommand ScreenSizeChangedCommand { get; set; }

    #endregion

    #region Properties

    private ObservableCollection<BindableCommunicationMessageView> _communicationMessageDetails = new();

    public ObservableCollection<BindableCommunicationMessageView> CommunicationMessageDetails
    {
        get => _communicationMessageDetails;
        set
        {
            _communicationMessageDetails = value;
            OnPropertyChanged(nameof(CommunicationMessageDetails));
        }
    }

    private int _listViewHeight;

    public int ListViewHeight
    {
        get => _listViewHeight;
        set
        {
            _listViewHeight = value;
            OnPropertyChanged(nameof(ListViewHeight));
        }
    }

    private int _attachmentListViewHeight;

    public int AttachmentListViewHeight
    {
        get => _attachmentListViewHeight;
        set
        {
            _attachmentListViewHeight = value;
            OnPropertyChanged(nameof(AttachmentListViewHeight));
        }
    }

    private BindableCommunicationMessageView _selectedMessage;

    public BindableCommunicationMessageView SelectedMessage
    {
        get => _selectedMessage;
        set
        {
            _selectedMessage = value;
            OnPropertyChanged(nameof(SelectedMessage));
        }
    }

    private BindableCommunicationMessageView _replyMessageDetails;

    public BindableCommunicationMessageView ReplyMessageDetails
    {
        get => _replyMessageDetails;
        set
        {
            _replyMessageDetails = value;
            OnPropertyChanged(nameof(ReplyMessageDetails));
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

    public Guid? MessageUId { get; set; }

    private IList<BindableCommunicationMessageView> _communicationMessageDetailsList;

    public IList<BindableCommunicationMessageView> CommunicationMessageDetailsList
    {
        get => _communicationMessageDetailsList;
        set
        {
            _communicationMessageDetailsList = value;
            OnPropertyChanged(nameof(CommunicationMessageDetailsList));
        }
    }

    #endregion

    public CommunicationDetailsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods

    private async void InitializePage()
    {
        DownloadTappedCommand = new Command(DownloadClicked);
        PreviewIconTappedCommand = new Command(PreviewIconClicked);
        ReplyIconTappedCommand = new Command<BindableCommunicationMessageView>(ReplyIconTapped);
        IsVisibleCommunicationHeaderDeleteIcon = true;
        CommunicationHeaderDeleteTappedCommand = new Command(DeleteIconTapped);
        ScreenSizeChangedCommand = new Command(ScreeSizeChanged);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        MessagingCenter.Subscribe<BindableCommunicationMessageView>(this, "refreshMessageDetailsList",
            (repliedMessageDetails) =>
            {
                if (repliedMessageDetails != null) RefreshCommunicationDetails();
            });
        SetBeamViews();
    }

    private async void DownloadClicked(object obj)
    {
        if (obj != null)
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
                        //TODO: need to check
                        var selectedAttachmentMessage = CommunicationMessageDetails.ToList()
                            .Where(i => i.MessageId == selectedAttachment.AttachmentMessageId).FirstOrDefault();
                        var attachmentList = selectedAttachmentMessage.BindableAttachmentList.ToList();
                        attachmentList[attachmentList.IndexOf(selectedAttachment)].FileStatus = 1;
                        var filePath = await HelperMethods.DownloadAndReturnFilePath(selectedAttachment.FilePath, _nativeServices);
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            attachmentList[attachmentList.IndexOf(selectedAttachment)].FileDevicePath = filePath;
                            attachmentList[attachmentList.IndexOf(selectedAttachment)].FileStatus = 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void PreviewIconClicked(object obj)
    {
        if (obj != null)
            try
            {
                var selectedAttachment = (BindableAttachmentFileView)obj;
                await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void DeleteIconTapped()
    {
        var deleteTapAction = await Application.Current.MainPage.DisplayAlert(TextResource.DeleteConfirmationTitle,
            TextResource.DeleteConfirmationMessageText, TextResource.YesText, TextResource.NoText);
        if (deleteTapAction)
            try
            {
                var operationDetails =
                    await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.DeleteMessageAPIUrl,
                        MessageUId, true));
                if (operationDetails.Success)
                {
                    MessagingCenter.Send(SelectedMessage, "DeleteMessage");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void ReplyIconTapped(BindableCommunicationMessageView obj)
    {
        try
        {
            if(obj == null)
                return;
            SendMessageForm sendMessageForm = new SendMessageForm(_mapper, _nativeServices, Navigation)
            {
                CommunicationMessageDetails = obj,
                SelectedRecipientList = new ObservableCollection<CommunicationMessageView>(obj.RecipientList),
                ReplyParentId = MessageId,
                IsReplyToParent = SelectedMessage.IsReplyToParent,
                IsEnabledRecipientSelection = SelectedMessage.IsReplyToParent,
                IsVisibleInfoIcon = false,
                IsEnabledMessage = true,
                IsEnabledSubject = true
            };
            sendMessageForm.MessageSubject.Value = obj.MessageSubject;
            sendMessageForm.RecipientListHeight = sendMessageForm.SelectedRecipientList.Count < 7
                ? sendMessageForm.SelectedRecipientList.Count * 40
                : 280;
            sendMessageForm.SearchedString.Value = string.Empty;
            sendMessageForm.ReceipientPlaceHolder = sendMessageForm.SelectedRecipientList.Count <= 0
                ? TextResource.BeamSelectRecipientText
                : "";
            var sendMessagePage = new SendMessagePage()
            {
                BindingContext = sendMessageForm
            };

            await Navigation.PushAsync(sendMessagePage);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private void ScreeSizeChanged()
    {
        CommunicationMessageDetails =
            new ObservableCollection<BindableCommunicationMessageView>(CommunicationMessageDetails);
    }

    private async void RefreshCommunicationDetails()
    {
        try
        {
            var cacheKeyPrefix = $"MessageDetail_{MessageId}";
            CommunicationMessageDetailsList = await ApiHelper.GetObjectList<BindableCommunicationMessageView>(
                TextResource.CommunicationDetailsAPIUrl + "?messageId=" + MessageId + "&folderType=" +
                AppSettings.Current.CommunicationMessageType,
                cacheType: ApiHelper.CacheTypeParam.LoadFromServerAndCache, cacheKeyPrefix: cacheKeyPrefix);
            CommunicationMessageDetails =
                new ObservableCollection<BindableCommunicationMessageView>(CommunicationMessageDetailsList);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void SetBeamViews()
    {
        BeamHeaderMessageIconVisibility = false;
        BeamCommunicationHeaderDeleteIconVisibility = true;
        IsVisibleCommunicationHeaderDeleteIcon = false;
    }

    #endregion
}