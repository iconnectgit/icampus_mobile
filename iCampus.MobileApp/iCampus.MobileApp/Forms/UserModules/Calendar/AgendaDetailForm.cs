using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Calendar;
using iCampus.Portal.EditModels;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class AgendaDetailForm : ViewModelBase
{
    #region Declarations

    public ICommand AttachmentListTappedCommand { get; set; }
    public ICommand DownloadTappedCommand { get; set; }
    public ICommand WebsiteLinksTappedCommand { get; set; }
    public ICommand EditClickCommand { get; set; }
    public ICommand AddAttachmentClickCommand { get; set; }
    public ICommand DeleteAttachmentClickCommand { get; set; }
    public ICommand SubmitSubmissionClickCommand { get; set; }
    public ICommand ViewSubmissionsClickedCommand { get; set; }

    #endregion

    #region Properties

    private BindableAgendaView _selectedAgenda = new();

    public BindableAgendaView SelectedAgenda
    {
        get => _selectedAgenda;
        set
        {
            _selectedAgenda = value;
            OnPropertyChanged(nameof(SelectedAgenda));
        }
    }

    private DateTime selectedDate;

    public DateTime SelectedDate
    {
        get => selectedDate;
        set
        {
            selectedDate = value;
            OnPropertyChanged(nameof(SelectedDate));
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

    private int _linkListViewHeight;

    public int LinkListViewHeight
    {
        get => _linkListViewHeight;
        set
        {
            _linkListViewHeight = value;
            OnPropertyChanged(nameof(LinkListViewHeight));
        }
    }

    private int _submissionListViewHeight;

    public int SubmissionListViewHeight
    {
        get => _submissionListViewHeight;
        set
        {
            _submissionListViewHeight = value;
            OnPropertyChanged(nameof(SubmissionListViewHeight));
        }
    }

    private bool _isSubmissionAllowed;

    public bool IsSubmissionAllowed
    {
        get => _isSubmissionAllowed;
        set
        {
            _isSubmissionAllowed = value;
            OnPropertyChanged(nameof(IsSubmissionAllowed));
        }
    }

    private bool _isCreatorVisible = false;

    public bool IsCreatorVisible
    {
        get => _isCreatorVisible;
        set
        {
            _isCreatorVisible = value;
            OnPropertyChanged(nameof(IsCreatorVisible));
        }
    }

    private IList<BindableAttachmentFileView> _attachmentList;

    public IList<BindableAttachmentFileView> AttachmentList
    {
        get => _attachmentList;
        set
        {
            _attachmentList = value;
            OnPropertyChanged(nameof(AttachmentList));
        }
    }

    private bool _editOptionVisibility;

    public bool EditOptionVisibility
    {
        get => _editOptionVisibility;
        set
        {
            _editOptionVisibility = value;
            OnPropertyChanged(nameof(EditOptionVisibility));
        }
    }

    private AgendaEdit _addPostData;

    public AgendaEdit AddPostData
    {
        get => _addPostData;
        set
        {
            _addPostData = value;
            OnPropertyChanged(nameof(AddPostData));
        }
    }

    private IList<CalendarAgendaTypePickListItem> _agendaTypes = new List<CalendarAgendaTypePickListItem>();

    public IList<CalendarAgendaTypePickListItem> AgendaTypes
    {
        get => _agendaTypes;
        set
        {
            _agendaTypes = value;
            OnPropertyChanged(nameof(AgendaTypes));
        }
    }

    private IList<PickListItem> _courseList = new List<PickListItem>();

    public IList<PickListItem> CourseList
    {
        get => _courseList;
        set
        {
            _courseList = value;
            OnPropertyChanged(nameof(CourseList));
        }
    }

    private IList<PickListItem> _gradeList = new List<PickListItem>();

    public IList<PickListItem> GradeList
    {
        get => _gradeList;
        set
        {
            _gradeList = value;
            OnPropertyChanged(nameof(GradeList));
        }
    }

    private List<AgendaView> _existingAttachmentList = new();

    public List<AgendaView> ExistingAttachmentList
    {
        get => _existingAttachmentList;
        set
        {
            _existingAttachmentList = value;
            OnPropertyChanged(nameof(ExistingAttachmentList));
        }
    }

    private ObservableCollection<BindableAttachmentFileView> _submissionAttachmentFiles = new();

    public ObservableCollection<BindableAttachmentFileView> SubmissionAttachmentFiles
    {
        get => _submissionAttachmentFiles;
        set
        {
            _submissionAttachmentFiles = value;
            OnPropertyChanged(nameof(SubmissionAttachmentFiles));
        }
    }

    private List<string> _deletedAttachmentFileID = new();

    public List<string> DeletedAttachmentFileID
    {
        get => _deletedAttachmentFileID;
        set
        {
            _deletedAttachmentFileID = value;
            OnPropertyChanged(nameof(DeletedAttachmentFileID));
        }
    }

    private List<string> _deletedAttachmentFileName = new();

    public List<string> DeletedAttachmentFileName
    {
        get => _deletedAttachmentFileName;
        set
        {
            _deletedAttachmentFileName = value;
            OnPropertyChanged(nameof(DeletedAttachmentFileName));
        }
    }

    private string _submissionComments;

    public string SubmissionComments
    {
        get => _submissionComments;
        set
        {
            _submissionComments = value;
            OnPropertyChanged(nameof(SubmissionComments));
        }
    }

    private int _submissionCount;

    public int SubmissionCount
    {
        get => _submissionCount;
        set
        {
            _submissionCount = value;
            OnPropertyChanged(nameof(SubmissionCount));
        }
    }

    private int _totalCount;

    public int TotalCount
    {
        get => _totalCount;
        set
        {
            _totalCount = value;
            OnPropertyChanged(nameof(TotalCount));
        }
    }

    private WebsiteLinkView _slectedWebsiteLink;

    public WebsiteLinkView SlectedWebsiteLink
    {
        get => _slectedWebsiteLink;
        set
        {
            _slectedWebsiteLink = value;
            OnPropertyChanged(nameof(SlectedWebsiteLink));
        }
    }

    private bool _isViewSubmissionVisible;

    public bool IsViewSubmissionVisible
    {
        get => _isViewSubmissionVisible;
        set
        {
            _isViewSubmissionVisible = value;
            OnPropertyChanged(nameof(IsViewSubmissionVisible));
        }
    }

    private bool _isViewAgendaStudentsVisible;

    public bool IsViewAgendaStudentsVisible
    {
        get => _isViewAgendaStudentsVisible;
        set
        {
            _isViewAgendaStudentsVisible = value;
            OnPropertyChanged(nameof(IsViewAgendaStudentsVisible));
        }
    }

    private int _agendaDescriptionHeight;

    public int AgendaDescriptionHeight
    {
        get => _agendaDescriptionHeight;
        set
        {
            _agendaDescriptionHeight = value;
            OnPropertyChanged(nameof(AgendaDescriptionHeight));
        }
    }

    #endregion

    public AgendaDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null,
        null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        BackVisible = true;
        AttachmentListTappedCommand = new Command(AttachmentListClicked);
        DownloadTappedCommand = new Command(DownloadClicked);
        WebsiteLinksTappedCommand = new Command<WebsiteLinkView>(WebsiteLinkClicked);
        EditClickCommand = new Command(EditClicked);
        AddAttachmentClickCommand = new Command(AddAttachmentClicked);
        DeleteAttachmentClickCommand = new Command(DeleteAttachmentClicked);
        SubmitSubmissionClickCommand = new Command(SubmitSubmissionClicked);
        ViewSubmissionsClickedCommand = new Command(ViewSubmissionsClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        MessagingCenter.Subscribe<string>("", "ScrollViewRightSwipeAgendaSubscribe", (arg) =>
        {
            MessagingCenter.Subscribe<string>("", "ScrollViewRightSwipeAgenda", async (args) =>
            {
                //await SideMenuClicked();
            });
        });
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
                        AttachmentList[AttachmentList.IndexOf(selectedAttachment)].FileStatus = 1;
                        var filePath =
                            await HelperMethods.DownloadAndReturnFilePath(selectedAttachment.FilePath, _nativeServices);
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
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private async void AttachmentListClicked(object obj)
    {
        if (obj != null)
            try
            {
                var selectedAttachment = (BindableAttachmentFileView)obj;
                if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                    await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    private void WebsiteLinkClicked(WebsiteLinkView sender)
    {
        if (sender != null) HelperMethods.OpenWebsiteLinks(sender.Url, PageTitle);
    }

    private async void EditClicked()
    {
        try
        {
            if (SelectedAgenda != null)
            {
                await AddEditData();
                AddNewPostForm addNewPostForm = new(_mapper, _nativeServices, Navigation);
                addNewPostForm.PageTitle = TextResource.EditAgendaText;
                addNewPostForm.MenuVisible = false;
                addNewPostForm.BackVisible = true;
                addNewPostForm.IsPopUpPage = false;
                addNewPostForm.IsEditMode = true;
                addNewPostForm.SubmissionsCount = string.Concat("(", SelectedAgenda.SubmissionCountLevel, " ",
                    TextResource.SubmissionsText, ")");
                addNewPostForm.EditDataSettings(SelectedAgenda);
                addNewPostForm.AgendaTypes = AgendaTypes;
                addNewPostForm.SelectedAgendaTypes = addNewPostForm.AgendaTypes
                    .Where(x => x.ItemId == SelectedAgenda.TypeId.ToString())?.FirstOrDefault();
                addNewPostForm.FilteredCourseList =
                    addNewPostForm.SelectedAgendaTypes.ItemName.ToLower().Equals("weekly") ? GradeList : CourseList;
                if (SelectedAgenda.AgendaWeeklyGroupId > 0)
                {
                    addNewPostForm.AgendaWeeklyGroupId = SelectedAgenda.AgendaWeeklyGroupId;
                    addNewPostForm.IsReminderTextVisible = false;
                    addNewPostForm.IsReminderDateVisible = false;
                    addNewPostForm.IsClassesEnabled = false;
                    addNewPostForm.ClassListOpacity = 0.5F;
                }

                if (addNewPostForm.SelectedAgendaTypes.ItemName.ToLower().Equals("weekly"))
                    addNewPostForm.SelectedCourse = addNewPostForm.FilteredCourseList
                        .Where(x => x.ItemId == SelectedAgenda.GradeId.ToString())?.FirstOrDefault();
                else
                    addNewPostForm.SelectedCourse = addNewPostForm.FilteredCourseList
                        .Where(x => x.ItemId == SelectedAgenda.CurriculumId.ToString())?.FirstOrDefault();
                addNewPostForm.LearningOutcomeLabel = AddPostData.CalendarControlSetting?.LearningOutcomesLabel;
                addNewPostForm.AssignmentLabel = AddPostData.CalendarControlSetting?.AssignmentsLabel;

                addNewPostForm.SelectedAgendaForText =
                    addNewPostForm.SelectedCourse != null &&
                    !string.IsNullOrEmpty(addNewPostForm.SelectedCourse.ItemName)
                        ? addNewPostForm.SelectedCourse.ItemName
                        : string.Empty;
                addNewPostForm.AssignmentsText = !string.IsNullOrEmpty(SelectedAgenda.AgendaDescription)
                    ? Regex.Replace(SelectedAgenda.AgendaDescription, "<.*?>", string.Empty)
                    : string.Empty;
                if (!string.IsNullOrEmpty(addNewPostForm.AssignmentsText))
                    addNewPostForm.AssignmentsText =
                        "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" +
                        addNewPostForm.AssignmentsText + "</body></html>";

                addNewPostForm.GradeList = GradeList;
                addNewPostForm.CourseList = CourseList;

                addNewPostForm.AllowSubmissionsVisibility = AddPostData.IsStudentSubmissionEnabledPerAgendaType;
                addNewPostForm.IsAllowSubmissionsEnabled = string.Equals(SelectedAgenda.StudentSubmissionType, "O",
                    StringComparison.Ordinal);
                addNewPostForm.IsAllowSubmissions = SelectedAgenda.IsStudentSubmissionAllowed ||
                                                    string.Equals(SelectedAgenda.StudentSubmissionType, "A",
                                                        StringComparison.Ordinal);


                addNewPostForm.ExistingAttachmentList = ExistingAttachmentList;
                await addNewPostForm.AddDataSettings(AddPostData);
                var addNewPostPage = new AddNewPostPage()
                {
                    BindingContext = addNewPostForm
                };
                await Navigation.PushAsync(addNewPostPage);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task AddEditData()
    {
        try
        {
            AddPostData = await ApiHelper.GetObject<AgendaEdit>(string.Format(TextResource.GetAddEditAgendaPostApi,
                SelectedAgenda.AgendaId, null));
            if (AddPostData != null)
            {
                AgendaTypes = AddPostData.AgendaTypeList != null
                    ? AddPostData.AgendaTypeList
                    : new List<CalendarAgendaTypePickListItem>();
                CourseList = AddPostData.CourseList != null && AddPostData.CourseList.Count() > 0
                    ? AddPostData.CourseList
                    : new List<PickListItem>();
                GradeList = AddPostData.GradeList != null && AddPostData.GradeList.Count() > 0
                    ? AddPostData.GradeList
                    : new List<PickListItem>();
                ExistingAttachmentList =
                    AddPostData.AgendaAttachmentList != null && AddPostData.AgendaAttachmentList.Count() > 0
                        ? AddPostData.AgendaAttachmentList.ToList()
                        : new List<AgendaView>();
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            //Crashes.TrackError(ex);
        }
    }

    public void SetEditOptionVisibility()
    {
        if (AppSettings.Current.IsTeacher)
            EditOptionVisibility = SelectedAgenda.AgendaEditDeleteStatus.ToEditDeleteStatus().IsEditable;
    }

    public async void AddAttachmentClicked()
    {
        try
        {
            var fileData = await HelperMethods.PickFileFromDevice();
            if (fileData == null) return;
            var foundSpecialChars = FindSpecialCharacters(fileData.FileName);
            if (!string.IsNullOrEmpty(foundSpecialChars))
            {
                await HelperMethods.ShowAlert(TextResource.AlertsPageTitle,
                    "File name can't contain any of the following character(s): " + foundSpecialChars);
                return;
            }

            if (fileData != null && fileData.FileData != null)
            {
                if (SubmissionAttachmentFiles.Any(x => x.FileName.Equals(fileData.FileName, StringComparison.OrdinalIgnoreCase)))
                {
                    await HelperMethods.ShowAlert("", "This file has already been added.");
                    return;
                }
                var file = _mapper.Map<BindableAttachmentFileView>(fileData);
                SubmissionAttachmentFiles.Add(file);
                SubmissionListViewHeight = SubmissionAttachmentFiles.Count * 40;
            }
            MessagingCenter.Send("", "UpdateAttachmentListView");
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    private string FindSpecialCharacters(string fileName)
    {
        var specialCharacters = "!@#$%^&*()+[]{}|;:'\"<>,?/~`";
        var foundSpecialCharacters = fileName.Where(c => specialCharacters.Contains(c)).Distinct();
        var specialCharactersString = string.Join(", ", foundSpecialCharacters);

        return specialCharactersString;
    }

    private async void DeleteAttachmentClicked(object obj)
    {
        try
        {
            if (obj != null)
            {
                var action = await Application.Current.MainPage.DisplayAlert("", TextResource.DeleteText,
                    TextResource.YesText, TextResource.NoText);
                if (action)
                {
                    var attachmentFile = (BindableAttachmentFileView)obj;
                    var fileid = SelectedAgenda.StudentSubmittedFilesList
                        ?.Where(x => x.AttachmentFile.Equals(attachmentFile.FileName))?.FirstOrDefault()
                        ?.StudentSubmissionId;
                    if (fileid != null)
                        DeletedAttachmentFileID.Add(fileid.ToString());

                    DeletedAttachmentFileName.Add(attachmentFile.FileName);
                    SubmissionAttachmentFiles.Remove(attachmentFile);
                }

                SubmissionListViewHeight = SubmissionAttachmentFiles.Count * 50;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public async void SubmitSubmissionClicked()
    {
        try
        {
            if (!string.IsNullOrEmpty(SubmissionComments) || SubmissionAttachmentFiles?.Count() > 0)
            {
                if (SelectedAgenda != null)
                {
                    var classStudentView = new AgendaClassStudentView();
                    classStudentView.AgendaId = SelectedAgenda.AgendaId;
                    classStudentView.StudentId = SelectedAgenda.StudentId.GetValueOrDefault();
                    classStudentView.StudentName = SelectedAgenda.StudentName;
                    classStudentView.AgendaTypeId = SelectedAgenda.TypeId.ToShort();
                    classStudentView.AgendaTypeTitle = SelectedAgenda.TypeTitle;
                    classStudentView.ClassId = SelectedAgenda.ClassId.GetValueOrDefault();
                    classStudentView.ClassName = SelectedAgenda.ClassName;
                    classStudentView.CurriculumName = SelectedAgenda.CurriculumName;

                    if (DeletedAttachmentFileID.Count > 0)
                        classStudentView.DeletedAttachmentFiles =
                            string.Join(",", DeletedAttachmentFileID.Select(x => x));
                    if (DeletedAttachmentFileName.Count > 0)
                        classStudentView.DeletedAttachmentsArray = DeletedAttachmentFileName.ToArray();
                    if (SubmissionAttachmentFiles.Count > 0)
                        classStudentView.AttachmentsArray = SubmissionAttachmentFiles.Select(x => x.FileName).ToArray();

                    classStudentView.StudentComments = SubmissionComments;

                    var list = new List<AttachmentFileView>();
                    if (SubmissionAttachmentFiles != null && SubmissionAttachmentFiles.Count > 0)
                        list = _mapper.Map<List<AttachmentFileView>>(SubmissionAttachmentFiles);


                    classStudentView.StudentSubmittedFilesList = _mapper
                        .Map<ObservableCollection<StudentSubmissionAttachmentView>>(SubmissionAttachmentFiles)
                        ?.ToList();

                    if (classStudentView.StudentSubmittedFilesList == null ||
                        classStudentView.StudentSubmittedFilesList.Count == 0)
                        classStudentView.StudentSubmittedFilesList = new List<StudentSubmissionAttachmentView>();

                    var result = await ApiHelper.PostMultiDataRequestAsync<OperationDetails>(
                        TextResource.SubmissionApiUrl, AppSettings.Current.ApiUrl, classStudentView, list);
                    if (result.Success)
                    {
                        await HelperMethods.ShowAlert(TextResource.SubmissionSaveTitle,
                            TextResource.SubmissionSaveMessage);
                        MessagingCenter.Send(SelectedAgenda, "SubmissionMessage");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        HelperMethods.DisplayException(new Exception(TextResource.ExceptionMessage), PageTitle);
                    }
                }
            }
            else
            {
                await HelperMethods.ShowAlert(TextResource.SubmissionErrorTitle, TextResource.SubmissionErrorMessage);
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    private async void ViewSubmissionsClicked()
    {
        try
        {
            ViewSubmissionsForm viewSubmissionsForm = new(_mapper, _nativeServices, Navigation)
            {
                BackVisible = true,
                IsPopUpPage = false,
                PageTitle = TextResource.SubmissionsText,
                MenuVisible = false,
                ClassList = SelectedAgenda.AgendaClassNames?.Split(',').ToList(),
                IsSubmittedOnlyVisible = !IsViewAgendaStudentsVisible,
                AgendaClassStudents =
                    await ApiHelper.GetObjectList<BindableAgendaClassStudentView>(
                        string.Format(TextResource.GetAgendaStudentListApi, SelectedAgenda.AgendaId))
            };

            viewSubmissionsForm.AgendaClassStudentList = viewSubmissionsForm.AgendaClassStudents;
            ViewSubmissionsPage viewSubmissionsPage = new()
            {
                BindingContext = viewSubmissionsForm
            };
            await Navigation.PushAsync(viewSubmissionsPage);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex);
        }
    }

    public override async void GetStudentData()
    {
        CalendarForm calendarForm = new(_mapper, _nativeServices, Navigation);
        calendarForm.GetStudentData();
    }
}