using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.MessageFromSchool;

public class CircularsForm : ViewModelBase
{
    #region Declaration

    public ICommand AttachmentClickCommand { get; set; }
    public ICommand LinksClickCommand { get; set; }
    public ICommand WebsiteLinksTappedCommand { get; set; }

    #endregion

    public CircularsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null,
        null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Properties

    private DateTime _fromDate;

    public DateTime FromDate
    {
        get => _fromDate;
        set
        {
            _fromDate = value;
            OnPropertyChanged(nameof(FromDate));
        }
    }

    private DateTime _toDate;

    public DateTime ToDate
    {
        get => _toDate;
        set
        {
            _toDate = value;
            OnPropertyChanged(nameof(ToDate));
        }
    }

    private IList<CircularView> circularList = new List<CircularView>();

    public IList<CircularView> CircularList
    {
        get => circularList;
        set
        {
            circularList = value;
            OnPropertyChanged(nameof(CircularList));
        }
    }

    #endregion

    private async void InitializePage()
    {
        try
        {
            FromDate = FromDate == DateTime.MinValue ? DateTime.Now : FromDate;
            ToDate = ToDate == DateTime.MinValue ? DateTime.Now : ToDate;
            PageTitle = TextResource.CircularsPageTitle;
            MenuVisible = true;
            AttachmentClickCommand = new Command(AttachmentClicked);
            LinksClickCommand = new Command(LinksClicked);
            WebsiteLinksTappedCommand = new Command<string>(WebsiteLinkClicked);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            await GetCircularList();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.MessageFromSchoolPageTitle);
        }
    }

    #region Private methods

    public async Task<IList<CircularView>> GetCircularList()
    {
        try
        {
            CircularList = await ApiHelper.GetObjectList<CircularView>(TextResource.CircularListApiUrl +
                                                                       "?fromDate=null"
                                                                       + "&toDate=null",
                attachStudentIdIfParent: false);
            return CircularList;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.MessageFromSchoolPageTitle);
            return new List<CircularView>();
        }
    }

    private async void LinksClicked()
    {
    }

    private async void AttachmentClicked(object obj)
    {
        if (obj != null) await HelperMethods.DownloadFile(obj.ToString());
    }

    private async void WebsiteLinkClicked(string sender)
    {
        if (sender.StartsWith("http://") || sender.StartsWith("https://"))
        {
            await Launcher.OpenAsync(new Uri(sender));
        }
        else
        {
            sender = "http://" + sender;
            await Launcher.OpenAsync(new Uri(sender));
        }
    }

    #endregion
}