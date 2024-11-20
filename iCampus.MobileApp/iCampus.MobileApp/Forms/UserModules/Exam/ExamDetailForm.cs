using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.Exam;

public class ExamDetailForm : ViewModelBase
{
    #region Declarations

    public ICommand AttachmentListTappedCommand { get; set; }
    public ICommand DonwloadTappedCommand { get; set; }
    public ICommand PreviousClickCommand { get; set; }
    public ICommand NextClickCommand { get; set; }

    #endregion

    #region Properties

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

    private int _selectedPosition;

    public int SelectedPosition
    {
        get => _selectedPosition;
        set
        {
            _selectedPosition = value;
            OnPropertyChanged(nameof(SelectedPosition));
            IsPreviousVisible = SelectedPosition > 0 ? true : false;
            IsNextVisible = SelectedPosition < ExamScheduleList.Count - 1 ? true : false;
            AttachmentList =
                _mapper.Map<List<BindableAttachmentFileView>>(ExamScheduleList[SelectedPosition].ExamFiles);
            ListViewHeight = ExamScheduleList[SelectedPosition].ExamFiles.Count() * 50;
        }
    }

    private string _selectedExamType;

    public string SelectedExamType
    {
        get => _selectedExamType;
        set
        {
            _selectedExamType = value;
            OnPropertyChanged(nameof(SelectedExamType));
        }
    }

    private bool _isPreviousVisible;

    public bool IsPreviousVisible
    {
        get => _isPreviousVisible;
        set
        {
            _isPreviousVisible = value;
            OnPropertyChanged(nameof(IsPreviousVisible));
        }
    }

    private bool _isNextVisible;

    public bool IsNextVisible
    {
        get => _isNextVisible;
        set
        {
            _isNextVisible = value;
            OnPropertyChanged(nameof(IsNextVisible));
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

    #endregion

    public ExamDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null,
        null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        AttachmentListTappedCommand = new Command(AttachmentListClicked);
        DonwloadTappedCommand = new Command(DownloadClicked);
        PreviousClickCommand = new Command(PreviousClicked);
        NextClickCommand = new Command(NextClicked);
    }

    #region Methods

    private void NextClicked(object obj)
    {
        if (SelectedPosition < ExamScheduleList.Count - 1) SelectedPosition++;
    }

    private void PreviousClicked(object obj)
    {
        if (SelectedPosition > 0) SelectedPosition--;
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
                        var filePath = await HelperMethods.DownloadAndReturnFilePath(selectedAttachment.FilePath, _nativeServices);
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

    #endregion
}