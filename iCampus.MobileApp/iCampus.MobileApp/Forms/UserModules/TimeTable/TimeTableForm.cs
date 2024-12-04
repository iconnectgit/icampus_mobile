using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.TimeTable;

public class TimeTableForm : ViewModelBase
{
    #region Declarations

    public ICommand AttachmentClickCommand { get; set; }
    public ICommand DownloadTappedCommand { get; set; }

    #endregion

    #region Properties

    private BindableAttachmentFileView _selectedAttachment;

    public BindableAttachmentFileView SelectedAttachment
    {
        get => _selectedAttachment;
        set
        {
            _selectedAttachment = value;
            OnPropertyChanged(nameof(SelectedAttachment));
        }
    }

    private StudentTimetableView _studentTimeTable;

    public StudentTimetableView StudentTimeTable
    {
        get => _studentTimeTable;
        set
        {
            _studentTimeTable = value;
            OnPropertyChanged(nameof(StudentTimeTable));
        }
    }

    private bool _isFileAvailable;

    public bool IsFileAvailable
    {
        get => _isFileAvailable;
        set
        {
            _isFileAvailable = value;
            OnPropertyChanged(nameof(IsFileAvailable));
        }
    }

    private bool _noDataExist;

    public bool NoDataExist
    {
        get => _noDataExist;
        set
        {
            _noDataExist = value;
            OnPropertyChanged(nameof(NoDataExist));
        }
    }

    #endregion

    public TimeTableForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Private Methods

    private async void InitializePage()
    {
        AttachmentClickCommand = new Command(AttachmentClicked);
        DownloadTappedCommand = new Command(DownloadClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        BackVisible = false;
        MenuVisible = true;
        InitPageSetup();
    }

    private void InitPageSetup()
    {
        NoDataExist = false;
        IsFileAvailable = false;
        SelectedAttachment = new BindableAttachmentFileView();
    }

    private async Task<StudentTimetableView> DisplayStudentTimeTable()
    {
        try
        {
            var studentId = AppSettings.Current.IsTeacher ? null : AppSettings.Current.SelectedStudent.ItemId;
            StudentTimeTable = await ApiHelper.GetObject<StudentTimetableView>(
                string.Format(TextResource.TimeTableApiUrl, studentId), cacheKeyPrefix: "timetable",
                cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
            if (!string.IsNullOrEmpty(StudentTimeTable.TimeTableFilePath))
            {
                IsFileAvailable = true;
                SelectedAttachment = new BindableAttachmentFileView(StudentTimeTable.TimeTableFileName,
                    StudentTimeTable.TimeTableFilePath);
                HelperMethods.OpenFileForPreview(StudentTimeTable.TimeTableFilePath, _nativeServices);
            }
            else
            {
                IsFileAvailable = false;
            }

            NoDataExist = !IsFileAvailable;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.TimeTablePageTitle);
        }

        return StudentTimeTable;
    }

    public override async void GetStudentData()
    {
        base.GetStudentData();
        InitPageSetup();
        await DisplayStudentTimeTable();
    }

    private void AttachmentClicked(object obj)
    {
        if (obj != null)
            try
            {
                HelperMethods.OpenFileForPreview(obj.ToString(), _nativeServices);
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
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    if (!string.IsNullOrEmpty(SelectedAttachment.FilePath))
                        await HelperMethods.OpenFileForPreview(SelectedAttachment.FilePath, _nativeServices);
                }
                else
                {
                    if (SelectedAttachment.FileStatus == 0)
                    {
                        SelectedAttachment.FileStatus = 1;
                        var isDownloaded = await HelperMethods.DownloadFile(SelectedAttachment.FilePath);
                        if (isDownloaded) SelectedAttachment.FileStatus = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                SelectedAttachment.FileStatus = 0;
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    #endregion
}