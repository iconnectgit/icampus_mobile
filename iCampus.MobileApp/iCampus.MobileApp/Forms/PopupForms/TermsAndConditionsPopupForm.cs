using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.PopupForms;

public class TermsAndConditionsPopupForm : ViewModelBase
{
    #region Declaration
    public ICommand AcceptCommand { get; set; }
    public event EventHandler <OperationDetails> TermConditionAccepted;

    #endregion

    #region Properties
    private string _termsConditionMessage;
    public string TermsConditionMessage
    {
        get => _termsConditionMessage;
        set
        {
            _termsConditionMessage = value;
            OnPropertyChanged(nameof(TermsConditionMessage));
        }
    }
    #endregion
    public TermsAndConditionsPopupForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        AcceptCommand = new Command(AcceptMethod);
    }

    #region Methods
    private async void AcceptMethod(object obj)
    {
        OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.LogRegistrationFamilyTermsAndConditionApi), AppSettings.Current.ApiUrl);
        TermConditionAccepted?.Invoke(this, (OperationDetails)result);
    }
    #endregion
}