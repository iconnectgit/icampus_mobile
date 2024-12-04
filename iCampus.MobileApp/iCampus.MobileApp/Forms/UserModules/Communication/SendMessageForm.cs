using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Library.FormValidation;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Communication;

public class SendMessageForm : ViewModelBase
{
    #region Declarations
    private bool isMessageBodyValid;
    private bool isMessageSubjectValid;
    private int _parentId;
    public ICommand SearchRecipientCommand { get; set; }
    public ICommand ComposeSendClickCommand { get; set; }
    public ICommand RecipientSelectionCommand { get; set; }
    public ICommand ComposeAttachmentClickCommand { get; set; }
    public ICommand SelectRecipientTappedCommand { get; set; }
    public ICommand DeleteAttachmentClickCommand { get; set; }
    public ICommand MessageSubjectTextChangedCommand { get; set; }
    public ICommand MessageBodyTextChangedCommand { get; set; }

    public ICommand CloseListCommand { get; set; }
    public ICommand DeleteRecipientCommand { get; set; }
    public ICommand RecipientCheckboxChangeCommand { get; set; }
    public ICommand GradePickerChangeCommand { get; set; }
    public ICommand ClassPickerChangeCommand { get; set; }
    public ICommand SortByChangeCommand { get; set; }
    public ICommand SortByTapCommand { get; set; }
    public ICommand OrderByCheckboxChangeCommand { get; set; }
    public ICommand SelectionCommand { get; set; }
    public ICommand FromListTappedCommand { get; set; }
    public ICommand ListChangedCommand { get; set; }

    #endregion

    #region Properties

    private ObservableCollection<CommunicationMessageView> _recipientList = new();

    public ObservableCollection<CommunicationMessageView> RecipientList
    {
        get => _recipientList;
        set
        {
            _recipientList = value;
            OnPropertyChanged(nameof(RecipientList));
        }
    }

    private ObservableCollection<CommunicationMessageView> _filteredRecipientList = new();

    public ObservableCollection<CommunicationMessageView> FilteredRecipientList
    {
        get => _filteredRecipientList;
        set
        {
            _filteredRecipientList = value;
            OnPropertyChanged(nameof(FilteredRecipientList));
        }
    }

    private ObservableCollection<CommunicationMessageView> _selectedRecipientList = new();

    public ObservableCollection<CommunicationMessageView> SelectedRecipientList
    {
        get => _selectedRecipientList;
        set
        {
            _selectedRecipientList = value;
            OnPropertyChanged(nameof(SelectedRecipientList));
        }
    }

    private ObservableCollection<string> _bindableRecipientList = new();

    public ObservableCollection<string> BindableRecipientList
    {
        get => _bindableRecipientList;
        set
        {
            _bindableRecipientList = value;
            OnPropertyChanged(nameof(BindableRecipientList));
        }
    }

    private ValidatableObject<string> _searchedString;

    public ValidatableObject<string> SearchedString
    {
        get => _searchedString;
        set
        {
            _searchedString = value;
            OnPropertyChanged(nameof(SearchedString));
        }
    }

    private ValidatableObject<string> _messageSubject;

    public ValidatableObject<string> MessageSubject
    {
        get => _messageSubject;
        set
        {
            _messageSubject = value;
            OnPropertyChanged(nameof(MessageSubject));
        }
    }

    private ValidatableObject<string> _messageBody;

    public ValidatableObject<string> MessageBody
    {
        get => _messageBody;
        set
        {
            _messageBody = value;
            OnPropertyChanged(nameof(MessageBody));
        }
    }

    private ObservableCollection<PickListItem> _selectedMessageRecipientList = new();

    public ObservableCollection<PickListItem> SelectedMessageRecipientList
    {
        get => _selectedMessageRecipientList;
        set
        {
            _selectedMessageRecipientList = value;
            OnPropertyChanged(nameof(SelectedMessageRecipientList));
        }
    }

    private bool _isEnabledRecipientSelection = false;

    public bool IsEnabledRecipientSelection
    {
        get => _isEnabledRecipientSelection;
        set
        {
            _isEnabledRecipientSelection = value;
            OnPropertyChanged(nameof(IsEnabledRecipientSelection));
        }
    }

    private bool _isEnabledSubject;

    public bool IsEnabledSubject
    {
        get => _isEnabledSubject;
        set
        {
            _isEnabledSubject = value;
            OnPropertyChanged(nameof(IsEnabledSubject));
        }
    }

    private bool _isEnabledMessage;

    public bool IsEnabledMessage
    {
        get => _isEnabledMessage;
        set
        {
            _isEnabledMessage = value;
            OnPropertyChanged(nameof(IsEnabledMessage));
        }
    }

    private bool _isEnabledToField;

    public bool IsEnabledToField
    {
        get => _isEnabledToField;
        set
        {
            _isEnabledToField = value;
            OnPropertyChanged(nameof(IsEnabledToField));
        }
    }

    private bool _isVisibleRecipientList;

    public bool IsVisibleRecipientList
    {
        get => _isVisibleRecipientList;
        set
        {
            _isVisibleRecipientList = value;
            OnPropertyChanged(nameof(IsVisibleRecipientList));
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

    private CommunicationMessageView _selectedRecipientItem = new();

    public CommunicationMessageView SelectedRecipientItem
    {
        get => _selectedRecipientItem;
        set
        {
            _selectedRecipientItem = value;
            OnPropertyChanged(nameof(SelectedRecipientItem));
        }
    }

    private string _selectedRecipient;

    public string SelectedRecipient
    {
        get => _selectedRecipient;
        set
        {
            _selectedRecipient = value;
            OnPropertyChanged(nameof(SelectedRecipient));
        }
    }

    private ObservableCollection<AttachmentFileView> _attachmentFiles = new();

    public ObservableCollection<AttachmentFileView> AttachmentFiles
    {
        get => _attachmentFiles;
        set
        {
            _attachmentFiles = value;
            OnPropertyChanged(nameof(AttachmentFiles));
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

    private bool _isRecipientErrorLabelVisible;

    public bool IsRecipientErrorLabelVisible
    {
        get => _isRecipientErrorLabelVisible;
        set
        {
            _isRecipientErrorLabelVisible = value;
            OnPropertyChanged(nameof(IsRecipientErrorLabelVisible));
        }
    }

    private bool _isMessageSubjectErrorLabelVisible;

    public bool IsMessageSubjectErrorLabelVisible
    {
        get => _isMessageSubjectErrorLabelVisible;
        set
        {
            _isMessageSubjectErrorLabelVisible = value;
            OnPropertyChanged(nameof(IsMessageSubjectErrorLabelVisible));
        }
    }

    private bool _isMessageBodyErrorLabelVisible;

    public bool IsMessageBodyErrorLabelVisible
    {
        get => _isMessageBodyErrorLabelVisible;
        set
        {
            _isMessageBodyErrorLabelVisible = value;
            OnPropertyChanged(nameof(IsMessageBodyErrorLabelVisible));
        }
    }

    private BindableCommunicationMessageView _communicationMessageDetails;

    public BindableCommunicationMessageView CommunicationMessageDetails
    {
        get => _communicationMessageDetails;
        set
        {
            _communicationMessageDetails = value;
            OnPropertyChanged(nameof(CommunicationMessageDetails));
        }
    }

    private string _replyRecipientId;

    public string ReplyRecipientId
    {
        get => _replyRecipientId;
        set
        {
            _replyRecipientId = value;
            OnPropertyChanged(nameof(ReplyRecipientId));
        }
    }

    private string _recipientId;

    public string RecipientId
    {
        get => _recipientId;
        set
        {
            _recipientId = value;
            OnPropertyChanged(nameof(RecipientId));
        }
    }

    private int _replyParentId;

    public int ReplyParentId
    {
        get => _replyParentId;
        set
        {
            _replyParentId = value;
            OnPropertyChanged(nameof(ReplyParentId));
        }
    }

    private string _emptyRecipientErrorMessage;

    public string EmptyRecipientErrorMessage
    {
        get => _emptyRecipientErrorMessage;
        set
        {
            _emptyRecipientErrorMessage = value;
            OnPropertyChanged(nameof(EmptyRecipientErrorMessage));
        }
    }

    private bool _isVisibleInfoIcon;

    public bool IsVisibleInfoIcon
    {
        get => _isVisibleInfoIcon;
        set
        {
            _isVisibleInfoIcon = value;
            OnPropertyChanged(nameof(IsVisibleInfoIcon));
        }
    }

    private BindableCommunicationMessageView _repliedMessageDetails;

    public BindableCommunicationMessageView RepliedMessageDetails
    {
        get => _repliedMessageDetails;
        set
        {
            _repliedMessageDetails = value;
            OnPropertyChanged(nameof(RepliedMessageDetails));
        }
    }

    private IList<ExtPickListItem> _gradeList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> GradeList
    {
        get => _gradeList;
        set
        {
            _gradeList = value;
            OnPropertyChanged(nameof(GradeList));
        }
    }

    private IList<ExtPickListItem> _classList = new List<ExtPickListItem>();

    public IList<ExtPickListItem> ClassList
    {
        get => _classList;
        set
        {
            _classList = value;
            OnPropertyChanged(nameof(ClassList));
        }
    }

    private ExtPickListItem _selectedGrade = new();

    public ExtPickListItem SelectedGrade
    {
        get => _selectedGrade;
        set
        {
            _selectedGrade = value;
            OnPropertyChanged(nameof(SelectedGrade));
        }
    }

    private ExtPickListItem _selectedClass = new();

    public ExtPickListItem SelectedClass
    {
        get => _selectedClass;
        set
        {
            _selectedClass = value;
            OnPropertyChanged(nameof(SelectedClass));
        }
    }

    private bool _isGradeErrVisible;

    public bool IsGradeErrVisible
    {
        get => _isGradeErrVisible;
        set
        {
            _isGradeErrVisible = value;
            OnPropertyChanged(nameof(IsGradeErrVisible));
        }
    }

    private bool _isClassErrVisible;

    public bool IsClassErrVisible
    {
        get => _isClassErrVisible;
        set
        {
            _isClassErrVisible = value;
            OnPropertyChanged(nameof(IsClassErrVisible));
        }
    }

    private ObservableCollection<PickListItem> _sortByList = new();

    public ObservableCollection<PickListItem> SortByList
    {
        get => _sortByList;
        set
        {
            _sortByList = value;
            OnPropertyChanged(nameof(SortByList));
        }
    }

    private string _receipientPlaceHolder;

    public string ReceipientPlaceHolder
    {
        get => _receipientPlaceHolder;
        set
        {
            _receipientPlaceHolder = value;
            OnPropertyChanged(nameof(ReceipientPlaceHolder));
        }
    }

    private bool _isAllSelected;

    public bool IsAllSelected
    {
        get => _isAllSelected;
        set
        {
            _isAllSelected = value;
            OnPropertyChanged(nameof(IsAllSelected));
        }
    }

    private bool _isAllSelectedManual;

    public bool IsAllSelectedManual
    {
        get => _isAllSelectedManual;
        set
        {
            _isAllSelectedManual = value;
            OnPropertyChanged(nameof(IsAllSelectedManual));
        }
    }

    private int _recipientListHeight = 0;

    public int RecipientListHeight
    {
        get => _recipientListHeight;
        set
        {
            _recipientListHeight = value;
            OnPropertyChanged(nameof(RecipientListHeight));
        }
    }

    private bool _sortPickerVisible;

    public bool SortPickerVisible
    {
        get => _sortPickerVisible;
        set
        {
            _sortPickerVisible = value;
            OnPropertyChanged(nameof(SortPickerVisible));
        }
    }

    private bool _isClassSelected;

    public bool IsClassSelected
    {
        get => _isClassSelected;
        set
        {
            _isClassSelected = value;
            OnPropertyChanged(nameof(IsClassSelected));
        }
    }

    private bool _isNameSelected;

    public bool IsNameSelected
    {
        get => _isNameSelected;
        set
        {
            _isNameSelected = value;
            OnPropertyChanged(nameof(IsNameSelected));
        }
    }

    private bool _isEnabledFromField;

    public bool IsEnabledFromField
    {
        get => _isEnabledFromField;
        set
        {
            _isEnabledFromField = value;
            OnPropertyChanged(nameof(IsEnabledFromField));
        }
    }

    private bool _isVisibleFromList = false;

    public bool IsVisibleFromList
    {
        get => _isVisibleFromList;
        set
        {
            _isVisibleFromList = value;
            OnPropertyChanged(nameof(IsVisibleFromList));
        }
    }
    private int _selectedSortBy;

    public int SelectedSortBy
    {
        get => _selectedSortBy;
        set
        {
            _selectedSortBy = value;
            OnPropertyChanged(nameof(SelectedSortBy));
        }
    }


    private string _messageStudentId;

    public string MessageStudentId
    {
        get => _messageStudentId;
        set
        {
            _messageStudentId = value;
            OnPropertyChanged(nameof(MessageStudentId));
        }
    }
    BindableStudentPickListItem _selectedFromText;
    public BindableStudentPickListItem SelectedFromText
    {
        get => _selectedFromText;
        set
        {
            _selectedFromText = value;
            OnPropertyChanged(nameof(SelectedFromText));
        }
    }
    private string _emptyFromErrorMessage;
    public string EmptyFromErrorMessage
    {
        get => _emptyFromErrorMessage;
        set
        {
            _emptyFromErrorMessage = value;
            OnPropertyChanged(nameof(EmptyFromErrorMessage));
        }    
    }
    private bool _isFromErrorLabelVisible;
    public bool IsFromErrorLabelVisible
    {
        get => _isFromErrorLabelVisible;
        set
        {
            _isFromErrorLabelVisible = value;
            OnPropertyChanged(nameof(IsFromErrorLabelVisible));
        }        
    }
    ObservableCollection<BindableStudentPickListItem> _emailFromList;
    public ObservableCollection<BindableStudentPickListItem> EmailFromList
    {
        get => _emailFromList;
        set
        {
            _emailFromList = value;
            OnPropertyChanged(nameof(EmailFromList));
        }
    }
    private int _fromListViewHeight;
    public int FromListViewHeight
    {
        get => _fromListViewHeight;
        set
        {
            _fromListViewHeight = value;
            OnPropertyChanged(nameof(FromListViewHeight));
        }
    }
    private string _selectedText;
    public string SelectedText
    {
        get => _selectedText;
        set
        {
            _selectedText = value;
            OnPropertyChanged(nameof(SelectedText));
        }
    }
    private string _testData;
    public string TestData
    {
        get => _testData;
        set
        {
            _testData = value;
            OnPropertyChanged(nameof(TestData));
        }
    }
    
    public bool IsReplyToParent { get; set; }

    #endregion


    public SendMessageForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods

    private async void InitializePage()
    {
        BackVisible = true;
        PageTitle = TextResource.SendMessagePageTitle;
        RecipientPlaceHolderText();
        SearchRecipientCommand = new Command(SearchRecipient);
        ComposeSendClickCommand = new Command(SendMessageClicked);
        RecipientSelectionCommand = new Command<CommunicationMessageView>(RecipientSelection);
        ComposeAttachmentClickCommand = new Command(AttachmentTapped);
        SelectRecipientTappedCommand = new Command(SelectRecipientTapped);
        DeleteAttachmentClickCommand = new Command(DeleteAttachmentClicked);
        MessageSubjectTextChangedCommand = new Command(MessageSubjectTextChanged);
        MessageBodyTextChangedCommand = new Command(MessageBodyTextChanged);
        CloseListCommand = new Command(CloseReceipientSelection);
        DeleteRecipientCommand = new Command<CommunicationMessageView>(DeleteRecipient);
        RecipientCheckboxChangeCommand = new Command(RecipientCheckboxChanged);
        GradePickerChangeCommand = new Command(GradeSelectionChanged);
        ClassPickerChangeCommand = new Command(ClassSelectionChanged);
        SortByChangeCommand = new Command(SortByChanged);
        _searchedString = new ValidatableObject<string>();
        _messageSubject = new ValidatableObject<string>();
        _messageBody = new ValidatableObject<string>();
        SortByTapCommand = new Command(SortTapped);
        OrderByCheckboxChangeCommand = new Command(OrderByChanged);
        SelectionCommand = new Command(FromListSelection);
        FromListTappedCommand = new Command<BindableStudentPickListItem>(FromListTapped);
        ListChangedCommand = new Command<BindableStudentPickListItem>(FromListChanged);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        IsNameSelected = true;
        IsComposeAttachmentVisible = true;
        IsComposeSendVisible = true;
        GradeList.Add(new ExtPickListItem { ItemId = null, ItemName = "All" });
        SelectedGrade = GradeList[0];

        ClassList.Add(new ExtPickListItem { ItemId = null, ItemName = "All" });
        SelectedClass = ClassList[0];

        IsEnabledToField = AppSettings.Current.IsParent ? false : true;
        IsEnabledSubject = AppSettings.Current.IsParent ? false : true;
        IsEnabledMessage = AppSettings.Current.IsParent ? false : true;

        if (AppSettings.Current.IsTeacher) await GetGradeList();
        SetBeamViews();
    }

    private void OrderByChanged(object obj)
    {
        try
        {
            if (obj != null)
            {
                if (obj.Equals("0") && IsNameSelected)
                {
                    if (_isClassSelected) IsClassSelected = false;
                    SelectedSortBy = 0;
                }
                else if (obj.Equals("1") && IsClassSelected)
                {
                    if (_isNameSelected) IsNameSelected = false;
                    SelectedSortBy = 1;
                }

                SortPickerVisible = false;
                FilterRecipientList();
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    private void SortTapped(object obj)
    {
        SortPickerVisible = !SortPickerVisible;
        if (SortPickerVisible && !_isNameSelected && !_isClassSelected)
        {
            IsNameSelected = true;
            FilterRecipientList();
        }
    }

    private void GetSelectedRecipientListHeight()
    {
        var singleRowHeight = DeviceInfo.Platform == DevicePlatform.Android ? 32 : 27;
        RecipientListHeight = SelectedRecipientList.Count < 7
            ? SelectedRecipientList.Count * singleRowHeight
            : singleRowHeight * 7;
    }

    private async Task GetGradeList()
    {
        try
        {
            var cacheKeyPrefix = "gradelist";
            var data = await ApiHelper.GetObject<CommunicationMessageView>(TextResource.CommunicationGradeListApiUrl,
                cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
            if (data != null)
            {
                var gradeData = data.Grades;
                gradeData.Insert(0, new ExtPickListItem { ItemId = null, ItemName = "All" });
                GradeList = gradeData;
                SelectedGrade = GradeList.FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async Task GetClassList()
    {
        try
        {
            var selectedGradeId = SelectedGrade != null && !string.IsNullOrEmpty(SelectedGrade.ItemId)
                ? SelectedGrade.ItemId
                : "0";
            var classData =
                await ApiHelper.GetObjectList<ExtPickListItem>(string.Format(TextResource.CommunicationClassListApiUrl,
                    selectedGradeId));
            classData.Insert(0, new ExtPickListItem { ItemId = null, ItemName = "All" });
            ClassList = classData;
            SelectedClass = ClassList.FirstOrDefault();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void GradeSelectionChanged(object obj)
    {
        try
        {
            if (SelectedGrade != null && !string.IsNullOrEmpty(SelectedGrade.ItemId)) IsGradeErrVisible = false;
            await GetClassList();
            FilterRecipientList();
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    private void ClassSelectionChanged(object obj)
    {
        try
        {
            if (SelectedClass != null && !string.IsNullOrEmpty(SelectedClass.ItemId)) IsClassErrVisible = false;
            FilterRecipientList();
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    private void SortByChanged(object obj)
    {
        FilterRecipientList();
    }

    private void SearchRecipient()
    {
        IsVisibleRecipientList = true;
        if (SearchedString != null && !string.IsNullOrEmpty(SearchedString.Value) && SearchedString.Value.Length > 0)
        {
            if (FilteredRecipientList != null && FilteredRecipientList.Count > 0)
                FilteredRecipientList = new ObservableCollection<CommunicationMessageView>(FilteredRecipientList
                    .Where(i => i.UserName.ToLower().Contains(SearchedString.Value.ToLower())).ToList());
            else
                FilteredRecipientList = new ObservableCollection<CommunicationMessageView>(RecipientList
                    .Where(i => i.UserName.ToLower().Contains(SearchedString.Value.ToLower())).ToList());
        }
        else
        {
            FilterRecipientList();
        }

        ListViewHeight = FilteredRecipientList.Count < 20 ? FilteredRecipientList.Count * 30 : 600;
        RecipientPlaceHolderText();
    }

    private void FilterRecipientList()
    {
        FilteredRecipientList = RecipientList;

        if (SelectedGrade != null && !string.IsNullOrEmpty(SelectedGrade.ItemId))
            FilteredRecipientList = new ObservableCollection<CommunicationMessageView>(FilteredRecipientList
                .Where(i => i.GradeId.ToString() == SelectedGrade.ItemId).ToList());
        if (SelectedClass != null && !string.IsNullOrEmpty(SelectedClass.ItemId))
            FilteredRecipientList = new ObservableCollection<CommunicationMessageView>(FilteredRecipientList
                .Where(i => i.ClassId.ToString() == SelectedClass.ItemId).ToList());

        FilteredRecipientList = SelectedSortBy == 1
            ? new ObservableCollection<CommunicationMessageView>(FilteredRecipientList.OrderBy(x => x.ClassSequence)
                .ToList())
            : FilteredRecipientList;

        ListViewHeight = FilteredRecipientList.Count < 20 ? FilteredRecipientList.Count * 30 : 600;
        CheckSelectAllCheckbox();
        IsRecipientErrorLabelVisible = false;
        foreach (var recipient in SelectedRecipientList.ToList()) 
        {
            FilteredRecipientList.Remove(recipient);
        }
        //FilteredRecipientList.Remove(SelectedRecipientList);
    }

    private async void SendMessageClicked()
    {
        try
        {
            IsVisibleRecipientList = false;
            AddValidations();
            if (IsSendMessageFormValid())
            {
                RecipientId = SelectedRecipientList == null || SelectedRecipientList.Count == 0
                    ? ReplyRecipientId
                    : string.Join(",", SelectedRecipientList.Select(x => x.UserId));
                var _attachmentList = new List<AttachmentFileView>(AttachmentFiles);
                _parentId = ReplyParentId > 0 ? ReplyParentId : 0;
                MessageStudentId = AppSettings.Current.IsParent && ReplyParentId == 0 && SelectedFromText != null
                    ? SelectedFromText.ItemId
                    : null;
                var operationDetails = await ApiHelper.PostMultiDataRequestAsync<OperationDetails>(
                    string.Format(TextResource.SendMessageApiUrl, RecipientId, MessageBody.Value, MessageSubject.Value,
                        _parentId, IsReplyToParent, MessageStudentId), AppSettings.Current.ApiUrl, null,
                    _attachmentList);
                if (operationDetails.Success)
                {
                    if (CommunicationMessageDetails == null)
                    {
                        MessagingCenter.Send("", "refreshList");
                    }
                    else
                    {
                        MessagingCenter.Send(CommunicationMessageDetails, "refreshList");
                    }
                    if (_parentId > 0) 
                        MessagingCenter.Send(CommunicationMessageDetails, "refreshMessageDetailsList");
                    await Navigation.PopAsync();
                }
                else
                {
                    await HelperMethods.ShowAlert(PageTitle, TextResource.SendingFailedMessage);
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void RecipientSelection(CommunicationMessageView obj)
    {
        if (obj != null)
        {
            if (SelectedRecipientList.IndexOf(obj) == -1)
            {
                if (Device.RuntimePlatform == Device.iOS)
                    MessagingCenter.Send<string>("", "SearchUnfocus");

                if (SelectedRecipientList.Count < AppSettings.Current.MaxAllowedRecipientCount)
                {
                    SelectedRecipientList.Add(obj);
                    GetSelectedRecipientListHeight();

                    IsRecipientErrorLabelVisible = false;
                }
                else
                {
                    await HelperMethods.ShowAlert("",
                        string.Format(TextResource.RecipientRestrictionAlertMessage,
                            AppSettings.Current.MaxAllowedRecipientCount));
                }

                RecipientPlaceHolderText();
                SortPickerVisible = false;
                CheckSelectAllCheckbox();
            }

            SelectedRecipientItem = null;
            SearchedString.Value = string.Empty;
        }
    }

    private void CloseReceipientSelection()
    {
        IsVisibleRecipientList = false;
    }

    private void DeleteRecipient(CommunicationMessageView obj)
    {
        if (obj != null && IsEnabledRecipientSelection)
        {
            SelectedRecipientList.Remove(obj);
            RecipientPlaceHolderText();
            GetSelectedRecipientListHeight();
            CheckSelectAllCheckbox();
            FilteredRecipientList.Add(obj);
        }
    }

    private async void RecipientCheckboxChanged()
    {
        try
        {
            if (IsAllSelected)
            {
                //var list1 = FilteredRecipientList.Except(SelectedRecipientList);
                var list = new ObservableCollection<CommunicationMessageView>(
                    FilteredRecipientList.Except(SelectedRecipientList));
                if (list != null && list.Count > 0)
                {
                    if (list.Count + SelectedRecipientList.Count <= AppSettings.Current.MaxAllowedRecipientCount)
                    {
                        //SelectedRecipientList.Add(list);
                        foreach (var item in list)
                        {
                            SelectedRecipientList.Add(item);
                        }
                    }
                    else
                    {
                        IsAllSelected = false;
                        await HelperMethods.ShowAlert("",
                            string.Format(TextResource.RecipientRestrictionAlertMessage,
                                AppSettings.Current.MaxAllowedRecipientCount));
                    }
                }
            }
            else
            {
                if (!IsAllSelectedManual)
                    if (SelectedRecipientList != null && SelectedRecipientList.Count > 0)
                    {
                        var list = SelectedRecipientList.ToList().FindAll(elem => FilteredRecipientList.Contains(elem));
                        //SelectedRecipientList.Remove(list);
                        foreach (var item in list)
                        {
                            SelectedRecipientList.Remove(item);
                        }
                    }
            }

            IsAllSelectedManual = false;
            RecipientPlaceHolderText();
            GetSelectedRecipientListHeight();
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    private async void AttachmentTapped(object obj)
    {
        var fileData = await HelperMethods.PickFileFromDevice();
        AttachmentFiles.AddFileToList(fileData);
        AttachmentListViewHeight = AttachmentFiles.Count * 40;
    }

    private void SelectRecipientTapped(object obj)
    {
        RecipientPlaceHolderText();
        IsVisibleRecipientList = true;
        FilterRecipientList();
    }

    private async void DeleteAttachmentClicked(object obj)
    {
        if (obj != null)
        {
            var action = await Application.Current.MainPage.DisplayAlert("", TextResource.DeleteText,
                TextResource.YesText, TextResource.NoText);
            if (action)
            {
                var attachmentFile = (AttachmentFileView)obj;
                AttachmentFiles.Remove(attachmentFile);
            }
        }
    }

    private void MessageSubjectTextChanged(object obj)
    {
        IsMessageSubjectErrorLabelVisible = string.IsNullOrEmpty(MessageSubject.Value) ? true : false;
    }

    private void MessageBodyTextChanged(object obj)
    {
        IsMessageBodyErrorLabelVisible = string.IsNullOrEmpty(MessageBody.Value) ? true : false;
    }

    private void AddValidations()
    {
        MessageSubject.Validations.Add(new IsNotNullOrEmptyRule<string>
        {
            ValidationMessage = TextResource.EmptyMessageSubjectErrror
        });
        MessageBody.Validations.Add(new IsNotNullOrEmptyRule<string>
        {
            ValidationMessage = TextResource.EmptyMessageBodyError
        });
        EmptyRecipientErrorMessage = TextResource.EmptyRecipientError;
        EmptyFromErrorMessage = TextResource.EmptyFromError;
    }

    public bool IsSendMessageFormValid()
    {
        isMessageSubjectValid = MessageSubject.Validate();
        isMessageBodyValid = MessageBody.Validate();
        IsRecipientErrorLabelVisible = !isRecipientValid();
        IsFromErrorLabelVisible = !(SelectedFromText?.ItemId != null);

        if (AppSettings.Current.IsParent && ReplyParentId == 0)
        {
            if (IsRecipientErrorLabelVisible || !isMessageSubjectValid || !isMessageBodyValid ||
                IsFromErrorLabelVisible)
            {
                IsMessageSubjectErrorLabelVisible = !isMessageSubjectValid;
                IsMessageBodyErrorLabelVisible = !isMessageBodyValid;
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (IsRecipientErrorLabelVisible || !isMessageSubjectValid || !isMessageBodyValid)
            {
                IsMessageSubjectErrorLabelVisible = !isMessageSubjectValid;
                IsMessageBodyErrorLabelVisible = !isMessageBodyValid;
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private bool isRecipientValid()
    {
        return SelectedRecipientList != null && SelectedRecipientList.Count > 0 ? true :
            !IsEnabledRecipientSelection ? true : false;
    }

    private void RecipientPlaceHolderText()
    {
        ReceipientPlaceHolder = SelectedRecipientList == null || SelectedRecipientList.Count <= 0
            ? TextResource.BeamSelectRecipientText
            : "";
    }

    private void CheckSelectAllCheckbox()
    {
        try
        {
            if (SelectedRecipientList != null && FilteredRecipientList != null && SelectedRecipientList.Count() > 0 &&
                FilteredRecipientList.Count() > 0)
            {
                var result = SelectedRecipientList.ToList()
                    .FindAll(elem => FilteredRecipientList.ToList().Contains(elem)).ToList();
                var hasAll = result.Count() == FilteredRecipientList.Count();
                if (hasAll)
                {
                    IsAllSelected = true;
                }
                else
                {
                    IsAllSelectedManual = true;
                    IsAllSelected = false;
                }
            }
            else if ((FilteredRecipientList == null || FilteredRecipientList.Count() == 0 ||
                      SelectedRecipientList == null || SelectedRecipientList.Count == 0) && IsAllSelected)
            {
                IsAllSelectedManual = true;
                IsAllSelected = false;
            }
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
        }
    }

    private void FromListSelection()
    {
        try
        {
            EmailFromList = new ObservableCollection<BindableStudentPickListItem>(AppSettings.Current.StudentList);
            IsVisibleFromList = !IsVisibleFromList;
            IsVisibleRecipientList = false;
            IsFromErrorLabelVisible = false;
            FromListViewHeight = AppSettings.Current.StudentList.Count * 30;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void FromListTapped(BindableStudentPickListItem pickListItem)
    {
        try
        {
            if (pickListItem != null)
            {
                SelectedFromText = pickListItem;
                GetAndCacheRecipientsList(SelectedFromText.ItemId);
                IsVisibleFromList = false;
                IsEnabledToField = true;
                IsEnabledSubject = true;
                IsEnabledMessage = true;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void FromListChanged(BindableStudentPickListItem obj)
    {
        if (obj != null) 
            SelectedText = obj.ItemName;
    }

    private async void GetAndCacheRecipientsList(string studentId)
    {
        var cacheKeyPrefix = "recipientcachelist" + studentId;
        var RecipientCacheList = new List<CommunicationMessageView>();
        try
        {
            var RecipientListData = await ApiHelper.GetObjectList<CommunicationMessageView>(
                string.Format(TextResource.RecipientListApiUrl, studentId, null, 0, null),
                cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
            RecipientList = new ObservableCollection<CommunicationMessageView>(RecipientListData);
            FilterRecipientList();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void SetBeamViews()
    {
        BeamCommunicationHeaderAttachmentIconVisibility = true;
        BeamCommunicationHeaderSendMessageIconVisibility = true;
        BeamHeaderMessageIconVisibility = false;
        BeamHeaderNotificationIconVisibility = false;
    }

    #endregion
}