using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Exam;

namespace iCampus.MobileApp.Forms.UserModules.Exam;

public class ExamForm : ViewModelBase
{
    #region Declarations

    public ICommand ListTappedCommand { get; set; }
    public ICommand AttachmentListTappedCommand { get; set; }
    public ICommand DonwloadTappedCommand { get; set; }
    public ICommand ExpandCollapseClickCommand { get; set; }
    public ICommand ArrowClickedCommand { get; set; }

    #endregion

    #region Properties

    private bool _isEnableCaching = true;
    private IList<BindableExamScheduleView> _examScheduleList = new List<BindableExamScheduleView>();

    public IList<BindableExamScheduleView> ExamScheduleList
    {
        get => _examScheduleList;
        set
        {
            _examScheduleList = value;
            OnPropertyChanged(nameof(ExamScheduleList));
        }
    }

    private BindableExamScheduleView _defaultExamSchedule;

    public BindableExamScheduleView DefaultExamSchedule
    {
        get => _defaultExamSchedule;
        set
        {
            _defaultExamSchedule = value;
            OnPropertyChanged(nameof(DefaultExamSchedule));
        }
    }

    private BindableExamScheduleView _selectedExamSchedule;

    public BindableExamScheduleView SelectedExamSchedule
    {
        get => _selectedExamSchedule;
        set
        {
            _selectedExamSchedule = value;
            OnPropertyChanged(nameof(SelectedExamSchedule));
        }
    }

    private bool _isNoRecordMsg;

    public bool IsNoRecordMsg
    {
        get => _isNoRecordMsg;
        set
        {
            _isNoRecordMsg = value;
            OnPropertyChanged(nameof(IsNoRecordMsg));
        }
    }

    #endregion

    public ExamForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        ListTappedCommand = new Command<BindableExamScheduleView>(ListViewTapped);
        AttachmentListTappedCommand = new Command(AttachmentListClicked);
        DonwloadTappedCommand = new Command(DownloadClicked);
        ExpandCollapseClickCommand = new Command<BindableExamScheduleView>(ExpandCollapseClicked);
        ArrowClickedCommand = new Command<BindableExamScheduleView>(ExpandCollapseClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
    }

    #region Methods

    public void ExpandCollapseClicked(BindableExamScheduleView bindableExamScheduleView)
    {
        if (bindableExamScheduleView != null)
            foreach (var item in ExamScheduleList)
                if (item != null)
                {
                    if (item.ExamId == bindableExamScheduleView.ExamId)
                    {
                        if (!(string.IsNullOrEmpty(bindableExamScheduleView.ExamRequirements) &&
                              bindableExamScheduleView.ExamFiles.Count() == 0))
                        {
                            item.DetailsVisibility = !item.DetailsVisibility;
                            item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png")
                                ? "dropdown_gray.png"
                                : "uparrow_gray.png";
                        }
                    }
                    else
                    {
                        item.DetailsVisibility = false;
                        item.ArrowImageSource = "dropdown_gray.png";
                    }
                }

        MessagingCenter.Send("", "ExpandCollapse");
    }

    private async void ListViewTapped(BindableExamScheduleView obj)
    {
        if (obj != null)
        {
            ExamDetailForm examDetailForm = new(_mapper, _nativeServices, Navigation)
            {
                MenuVisible = false,
                BackVisible = true,
                PageTitle = PageTitle,
                ExamScheduleList = ExamScheduleList,
                SelectedPosition = ExamScheduleList.IndexOf(obj),
                SelectedExamType = obj.ExamDate >= DateTime.Now
                    ? TextResource.UpcomingExamText
                    : TextResource.PastExamText
            };
            SelectedExamSchedule = null;
            ExamDetailPage examDetailPage = new ExamDetailPage()
            {
                BindingContext = examDetailForm
            };
            await Navigation.PushAsync(examDetailPage);
        }
    }

    public async Task<IList<BindableExamScheduleView>> GetExamListByStudent()
    {
        try
        {
            var examList = await ApiHelper.GetObjectList<BindableExamScheduleView>(
                string.Format("{0}?userRefId={1}&gradeId=&examDate=&examId=&isPostedByMe=false&loadAllCourses=false",
                    TextResource.ExamApiUrl, AppSettings.Current.SelectedStudent.ItemId), _isEnableCaching);
            ExamScheduleList = _mapper.Map<IList<BindableExamScheduleView>>(examList);

            foreach (var exam in ExamScheduleList)
                if (exam.ExamFiles != null && exam.ExamFiles.Count() > 0)
                    foreach (var file in exam.ExamFiles)
                        file.ParentFieldId = exam.ExamId;

            IsNoRecordMsg = !ExamScheduleList.Any();

            DefaultExamSchedule = ExamScheduleList.FirstOrDefault();

            if (ExamScheduleList != null && ExamScheduleList.Count > 0 && !string.IsNullOrEmpty(NotificationItemId))
            {
                var examView = ExamScheduleList.Where(x => x.ExamId == Convert.ToInt32(NotificationItemId))
                    .FirstOrDefault();
                if (examView != null)
                    ListViewTapped(examView);
                NotificationItemId = null;
            }

            return ExamScheduleList;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            return new List<BindableExamScheduleView>();
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
                    if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                        await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
                }
                else
                {
                    var selectedExam = ExamScheduleList.Where(x => x.ExamId == selectedAttachment.ParentFieldId)
                        .FirstOrDefault();
                    if (selectedAttachment.FileStatus == 0)
                    {
                        selectedExam.ExamFiles.Where(x => x.FileName == selectedAttachment.FileName).FirstOrDefault()
                            .FileStatus = 1;
                        var filePath = await HelperMethods.DownloadAndReturnFilePath(selectedAttachment.FilePath, _nativeServices);
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            selectedExam.ExamFiles.Where(x => x.FileName == selectedAttachment.FileName)
                                .FirstOrDefault().FileDevicePath = filePath;
                            selectedExam.ExamFiles.Where(x => x.FileName == selectedAttachment.FileName)
                                .FirstOrDefault().FileStatus = 2;
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

    public override async void GetStudentData()
    {
        base.GetStudentData();
        await GetExamListByStudent();
    }

    #endregion
}