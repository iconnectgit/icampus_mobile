using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class ViewSubmissionsForm : ViewModelBase
{
    #region properties

    private bool _isSubmittedOnly;

    public bool IsSubmittedOnly
    {
        get => _isSubmittedOnly;
        set
        {
            _isSubmittedOnly = value;
            OnPropertyChanged(nameof(IsSubmittedOnly));
        }
    }

    private List<string> _classList;

    public List<string> ClassList
    {
        get => _classList;
        set
        {
            _classList = value;
            OnPropertyChanged(nameof(ClassList));
        }
    }

    public string _selectedClass;

    public string SelectedClass
    {
        get => _selectedClass;
        set
        {
            _selectedClass = value;
            OnPropertyChanged(nameof(SelectedClass));
            if (!string.IsNullOrEmpty(SelectedClass))
                SearchClicked();
        }
    }

    public List<BindableAgendaClassStudentView> _agendaClassStudents;

    public List<BindableAgendaClassStudentView> AgendaClassStudents
    {
        get => _agendaClassStudents;
        set
        {
            _agendaClassStudents = value;
            OnPropertyChanged(nameof(AgendaClassStudents));
        }
    }

    public IList<BindableAgendaClassStudentView> _agendaClassStudentList;

    public IList<BindableAgendaClassStudentView> AgendaClassStudentList
    {
        get => _agendaClassStudentList;
        set
        {
            _agendaClassStudentList = value;
            OnPropertyChanged(nameof(AgendaClassStudentList));
        }
    }

    private string _searchText;

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged(nameof(SearchText));
        }
    }


    public ICommand IsSubmittedCheckboxChangeCommand { get; set; }
    public ICommand SearchClickCommand { get; set; }
    public ICommand AttachmentListTappedCommand { get; set; }
    public ICommand DownloadTappedCommand { get; set; }

    #endregion properties

    public ViewSubmissionsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null,
        null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        IsSubmittedCheckboxChangeCommand = new Command(IsSubmittedCheckChanged);
        SearchClickCommand = new Command(SearchClicked);
        AttachmentListTappedCommand = new Command(AttachmentListClicked);
        DownloadTappedCommand = new Command(DownloadClicked);
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
                    if (selectedAttachment.FileStatus == 0 && !string.IsNullOrEmpty(selectedAttachment.FilePath))
                        foreach (var agendaClassStudentView in AgendaClassStudentList)
                        foreach (var attachment in agendaClassStudentView.BindableStudentSubmittedFilesList)
                            if (attachment.FileName.Equals(selectedAttachment.FileName))
                            {
                                attachment.FileStatus = 1;
                                var filePath = await HelperMethods.DownloadAndReturnFilePath(attachment.FilePath);
                                if (!string.IsNullOrEmpty(filePath))
                                {
                                    attachment.FileName = attachment.FileName + "test";
                                    attachment.FileDevicePath = filePath;
                                    attachment.FileStatus = 2;
                                }
                            }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, PageTitle);
            }
    }

    public void SearchClicked()
    {
        var bindableAgendaClassStudentViews = AgendaClassStudents;

        if (!string.IsNullOrEmpty(SearchText))
            bindableAgendaClassStudentViews = bindableAgendaClassStudentViews
                .Where(i => i.StudentName.ToLower().Contains(SearchText.ToLower())).ToList();
        else
            bindableAgendaClassStudentViews = AgendaClassStudents;

        if (SelectedClass != null)
            bindableAgendaClassStudentViews = bindableAgendaClassStudentViews
                .Where(x => x.ClassName.ToLower().Trim().Contains(SelectedClass.ToLower().Trim())).ToList();
        if (IsSubmittedOnly)
            bindableAgendaClassStudentViews = bindableAgendaClassStudentViews
                .Where(x => x.StudentSubmittedFilesList.Any(y => !string.IsNullOrEmpty(y.AttachmentUrl))).ToList();
        AgendaClassStudentList =
            new ObservableCollection<BindableAgendaClassStudentView>(bindableAgendaClassStudentViews);
    }

    private void IsSubmittedCheckChanged(object obj)
    {
        SearchClicked();
    }
}