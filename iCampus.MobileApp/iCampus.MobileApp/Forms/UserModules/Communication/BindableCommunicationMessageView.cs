using System.ComponentModel;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.Portal.ViewModels;
using Splat;

namespace iCampus.MobileApp.Forms.UserModules.Communication;

public class BindableCommunicationMessageView : INotifyPropertyChanged
    {
        #region Properties
        private IMapper _mapper;
        private bool isRead;
        public bool IsRead
        {
            get { return isRead; }
            set
            {
                isRead = value;
                OnPropertyChanged("IsRead");
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        private Color _messageBackgroundColor;
        public Color MessageBackgroundColor
        {
            get { return _messageBackgroundColor; }
            set
            {
                _messageBackgroundColor = value;
                OnPropertyChanged("MessageBackgroundColor");
            }
        }
        private string _circularImageSource = "unselected_circle_icon";
        public string CircularImageSource
        {
            get { return _circularImageSource; }
            set
            {
                _circularImageSource = value;
                OnPropertyChanged("CircularImageSource");
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string MessageSubject { get; set; }
        public string MessageText { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AttachmentFile { get; set; }
        public string AttachmentPath { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverEmailAddress { get; set; }
        public int ParentId { get; set; }
        public bool HasAttachments { get; set; }
        public string MessageTo { get; set; }
        public int SenderUserId { get; set; }
        public bool IsNewMessage { get; set; }
        public string ReceiverId { get; set; }
        public bool IsRejected { get; set; }
        public bool IsApproved { get; set; }
        public short ReceiverPortalUserTypeId { get; set; }
        public short SenderPortalUserTypeId { get; set; }
        public char ApprovalStatus { get; set; }
        public string RejectedComments { get; set; }
        public string MessageBrief { get; set; }
        public int WebsiteLinksCount { get; set; }
        public IEnumerable<WebsiteLinkView> WebsiteLinks { get; set; }
        public string GridTeacherName { get; set; }
        public string RecepientNames { get; set; }
        public List<CommunicationMessageView> RecipientList { get; set; }
        public bool IsPending { get; set; }
        public bool IsSentMessageIconVisible { get; set; }
        private int _attachmenListViewHeight { get; set; }
        public string SentDateFormatted { get; set; }
        public int AttachmentListViewHeight
        {
            get
            {
                return _attachmenListViewHeight;
            }
            set
            {
                _attachmenListViewHeight = value;
            }
        }
        private int _attachmentCount { get; set; }
        public int AttachmentCount
        {
            get
            {
                return _attachmentCount;
            }
            set
            {
                _attachmentCount = value;
                AttachmentListViewHeight = value * 40;
            }
        }
        private IEnumerable<AttachmentFileView> _attachmentList { get; set; }
        public IEnumerable<AttachmentFileView> AttachmentList
        {
            get
            {
                return _attachmentList;
            }
            set
            {
                _attachmentList = value;
                BindableAttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(value);
                MapMessageIdToAttachment();
            }
        }
        private IEnumerable<BindableAttachmentFileView> _bindableAttachmentList { get; set; }
        public IEnumerable<BindableAttachmentFileView> BindableAttachmentList
        {
            get
            {
                return _bindableAttachmentList;
            }
            set
            {
                _bindableAttachmentList = value;
            }
        }
        private int _attachmentMessageId { get; set; }
        public int AttachmentMessageId
        {
            get
            {
                return _attachmentMessageId;
            }
            set
            {
                _attachmentMessageId = value;
            }
        }
        private int _messageId { get; set; }
        public int MessageId
        {
            get
            {
                return _messageId;
            }
            set
            {
                _messageId = value;
                AttachmentMessageId = value;
            }
        }
        public Guid? MessageUId { get; set; }
        private string _senderName { get; set; }
        public string SenderName
        {
            get
            {
                return _senderName;
            }
            set
            {
                _senderName = value;
            }
        }

        private int _messagesCount;
        public int MessagesCount
        {
            get
            {
                return _messagesCount;
            }
            set
            {
                _messagesCount = value;
                OnPropertyChanged("MessagesCount");
            }
        }
        private bool _isSentMessageDetailsIconVisible { get; set; }
        public bool IsSentMessageDetailsIconVisible
        {
            get
            {
                _isSentMessageDetailsIconVisible = (SenderUserId.CompareTo(AppSettings.Current.UserId) == 0) ? true : false;
                return _isSentMessageDetailsIconVisible;
            }
            set
            {
               _isSentMessageDetailsIconVisible = value;
            }
        }

        public bool IsReplyToParent { get; set; }

        public CommunicationSettingView CommunicationSettings { get; set; }

        #endregion
        public BindableCommunicationMessageView()
        {
            _mapper = Locator.Current.GetService<IMapper>();
            RecipientList = new List<CommunicationMessageView>();
            CommunicationSettings = new CommunicationSettingView();
        }
        #region Private methods
        private void MapMessageIdToAttachment()
        {
            foreach (var item in BindableAttachmentList)
            {
                item.AttachmentMessageId = MessageId;
            }
        }
        #endregion
    }