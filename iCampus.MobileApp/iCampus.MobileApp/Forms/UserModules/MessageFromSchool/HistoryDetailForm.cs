using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.MessageFromSchool;

public class HistoryDetailForm : ViewModelBase
{
    private CustomAlertStatisticsView _historyDetailsObject = new();

    public CustomAlertStatisticsView HistoryDetailsObject
    {
        get => _historyDetailsObject;
        set
        {
            _historyDetailsObject = value;
            OnPropertyChanged(nameof(HistoryDetailsObject));
        }
    }

    private bool _isTitleAvailable;

    public bool IsTitleAvailable
    {
        get => _isTitleAvailable;
        set
        {
            _isTitleAvailable = value;
            OnPropertyChanged(nameof(IsTitleAvailable));
        }
    }

    private bool _isMessageAvailable;

    public bool IsMessageAvailable
    {
        get => _isMessageAvailable;
        set
        {
            _isMessageAvailable = value;
            OnPropertyChanged(nameof(IsMessageAvailable));
        }
    }

    private bool _isDateAvailable;

    public bool IsDateAvailable
    {
        get => _isDateAvailable;
        set
        {
            _isDateAvailable = value;
            OnPropertyChanged(nameof(IsDateAvailable));
        }
    }

    private bool _isAcknowledgedAvailable;

    public bool IsAcknowledgedAvailable
    {
        get => _isAcknowledgedAvailable;
        set
        {
            _isAcknowledgedAvailable = value;
            OnPropertyChanged(nameof(IsAcknowledgedAvailable));
        }
    }

    private bool _isFeedbackAvailable;

    public bool IsFeedbackAvailable
    {
        get => _isFeedbackAvailable;
        set
        {
            _isFeedbackAvailable = value;
            OnPropertyChanged(nameof(IsFeedbackAvailable));
        }
    }

    private string _alertMessage;

    public string AlertMessage
    {
        get => _alertMessage;
        set
        {
            _alertMessage = value;
            OnPropertyChanged(nameof(AlertMessage));
        }
    }

    public HistoryDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null,
        null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    private async void InitializePage()
    {
        BackVisible = true;
        BackTitle = TextResource.BackTitle;
        PageTitle = "Alert History";
    }
}