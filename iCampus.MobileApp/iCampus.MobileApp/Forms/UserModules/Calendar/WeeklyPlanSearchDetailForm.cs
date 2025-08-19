using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Calendar;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Calendar;

public class WeeklyPlanSearchDetailForm : ViewModelBase
{
    #region Declaration
    public ICommand AttachmentListTappedCommand { get; set; }
    


    #endregion

    #region Properties
    private BindableAgendaView _selectedGroupedAgendaData;
    public BindableAgendaView SelectedGroupedAgendaData
    {
        get => _selectedGroupedAgendaData;
        set
        {
            _selectedGroupedAgendaData = value;
            OnPropertyChanged(nameof(SelectedGroupedAgendaData));
        }
    }
    private IList<AgendaView> _attachmentList;
    public IList<AgendaView> AttachmentList
    {
        get => _attachmentList;
        set
        {
            _attachmentList = value;
            OnPropertyChanged(nameof(AttachmentList));
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
    private TeacherCalendarDisplaySetting _calendarControlSetting;
    public TeacherCalendarDisplaySetting CalendarControlSetting
    {
        get => _calendarControlSetting;
        set
        {
            _calendarControlSetting = value;
            OnPropertyChanged(nameof(CalendarControlSetting));
        }
    }
    private bool _isAttachmentVisible;
    public bool IsAttachmentVisible
    {
        get => _isAttachmentVisible;
        set
        {
            _isAttachmentVisible = value;
            OnPropertyChanged(nameof(IsAttachmentVisible));
        }
    }
    
    #endregion

    public WeeklyPlanSearchDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(
        null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region methods

    void InitializePage()
    {
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        AttachmentListTappedCommand = new Command<AttachmentFileView>(AttachmentListTappedCommandMethod);
    }

    private async void AttachmentListTappedCommandMethod(AttachmentFileView attachmentFileView)
    {
        try
        {
            if (!string.IsNullOrEmpty(attachmentFileView.FilePath))
                await HelperMethods.OpenFileForPreview(attachmentFileView.FilePath, _nativeServices);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
            
    }
    public override async void GetStudentData()
    {
        WeeklyPlanSearchForm weeklyPlanSearch = new(_mapper, _nativeServices, Navigation)
        {
            PageTitle = PageTitle,
            MenuVisible = true
        };
        AppSettings.Current.IsDisplayAllStudentList = false;
        weeklyPlanSearch.OpenStudentSelection();
        WeeklyPlanSearchPage weeklyPlanSearchPage = new()
        {
            BindingContext = weeklyPlanSearch
        };
        await Navigation.PushAsync(weeklyPlanSearchPage);
    }
    #endregion
    
}