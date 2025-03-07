using System.ComponentModel;
using System.Globalization;
using Akavache;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms;
using iCampus.MobileApp.Views;
using Splat;

namespace iCampus.MobileApp;

public partial class App : Application
{
    public static bool IsInitialized { get; set; }
    public static bool IsStudentPopupOpen { get; set; }
    public static string RefreshedToken { get; set; }
    public static bool IsUserVersionUpdate { get; set; }
    public static string ClientCode { get; set; }
    public static string DeviceID { get; set; }
    public static bool IsSurveyOn { get; set; }
    public static bool IsDataCollectionOn { get; set; }
    public static bool IsSurveyFromLogin { get; set; }
    public static bool IsMandateSurvey { get; set; }
    public static bool IsMandateDataCollection { get; set; }
    public static bool IsUserAlertDone { get; set; }
    private static NotificationData _notificationValues;
    public static HomeForm HomePageInstance { get; set; }
    public static NotificationData NotificationValues
    {
        get => _notificationValues;
        set
        {
            _notificationValues = value;
            MessagingCenter.Send(_notificationValues, "PushData");
        }
    }

    private static List<int> _customAlertIdList = new();

    public static List<int> CustomAlertIdList
    {
        get => _customAlertIdList;
        set => _customAlertIdList = value;
    }

    public static bool IsTokenUpdated { get; set; }
    public static List<int> SurveyIdList { get; set; }

    public static string ClientGroupCode { get; set; }

    public App(bool flag = false)
    {
        InitializeComponent();

        var nativeServices = Locator.Current.GetService<INativeServices>();
        BlobCache.EnsureInitialized();
        BlobCache.ForcedDateTimeKind = DateTimeKind.Local;
        CultureInfo defaultCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
        CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

        
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MyMappingProfile>(); 
        });

        var mapper = mapperConfig.CreateMapper();

        Locator.CurrentMutable.RegisterConstant(mapper, typeof(IMapper));

        MainPage = new NavigationPage(new LoginPage(mapper, nativeServices)); 
        
        //FlowListView.Init();
    }

    protected override void OnStart()
    {
        BlobCache.ApplicationName = "iCampus.MobileApp";
        BlobCache.EnsureInitialized();
        BlobCache.ForcedDateTimeKind = DateTimeKind.Local;
#if ANDROID
        Task<PermissionStatus> status = Permissions.RequestAsync<Permissions.PostNotifications>();
#endif
    }

    protected override void OnSleep()
    {
        // Handle when your app sleeps
    }

    protected override void OnResume()
    {
        base.OnResume();
        var currentPage = Application.Current.MainPage?.Navigation?.NavigationStack?.LastOrDefault();
    
        if (currentPage is not LoginPage) 
        {
            HomePageInstance?.GetHomePageData();
        }
    }
}