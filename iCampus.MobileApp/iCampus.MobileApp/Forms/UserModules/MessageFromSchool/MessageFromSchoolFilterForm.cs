using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.MessageFromSchool;

public class MessageFromSchoolFilterForm : ViewModelBase
{
    #region Declarations

    public ICommand SearchClickCommand { get; set; }

    #endregion

    #region Properties

    private DateTime _fromDate = DateTime.Now;
    public DateTime FromDate
    {
        get => _fromDate;
        set
        {
            if (value <= ToDate)
            {
                _fromDate = value;
                OnPropertyChanged(nameof(FromDate));
                DateErrorMessageVisibility = false;
            }
            else
            {
                DateErrorMessageVisibility = true;
            }
        }
    }

    private DateTime _toDate = DateTime.Now;

    public DateTime ToDate
    {
        get => _toDate;
        set
        {
            if (value >= FromDate)
            {
                _toDate = value;
                OnPropertyChanged(nameof(ToDate));
                DateErrorMessageVisibility = false;
            }
            else
            {
                DateErrorMessageVisibility = true;
            }
        }
    }

    private bool _dateErrorMessageVisibility;

    public bool DateErrorMessageVisibility
    {
        get => _dateErrorMessageVisibility;
        set
        {
            _dateErrorMessageVisibility = value;
            OnPropertyChanged(nameof(DateErrorMessageVisibility));
        }
    }
    #endregion

    public MessageFromSchoolFilterForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(
        null, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        SearchClickCommand = new Command(SearchClicked);
    }

    #region Methods

    private async void SearchClicked(object obj)
    {
        MessagingCenter.Send(this, "SearchMessageFromSchool");
        await Navigation.PopAsync();
        //HostScreen.Router.NavigateBack.Execute();
    }

    #endregion
}