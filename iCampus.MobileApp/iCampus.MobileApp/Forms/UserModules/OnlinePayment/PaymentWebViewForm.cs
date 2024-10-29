using System.Windows.Input;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class PaymentWebViewForm:ViewModelBase
{
    #region Declarations
    public ICommand GoBackCommand { get; set; }
    #endregion
    #region Properties
    string _webViewUrl;
    public string WebViewUrl
    {
        get => _webViewUrl;
        set
        {
            _webViewUrl = value;
            OnPropertyChanged(nameof(WebViewUrl));
        }
    }
    #endregion
    public PaymentWebViewForm(INavigation navigation) :  base(null, null, null)
    {
        Navigation = navigation;
        GoBackCommand = new Command(GoBackClicked);
        RefreshPaymentHistoryData();
    }

    private void RefreshPaymentHistoryData()
    {
        MessagingCenter.Send("", "RefreshPaymentHistoryData");
    }
    #region Method
    private void GoBackClicked()
    {
        // HostScreen.Router.NavigateBack.Execute().Subscribe();
        // OnlinePaymentForm onlinePaymentForm = new OnlinePaymentForm();
        // if (HostScreen.Router.GetCurrentViewModel().GetType() != typeof(OnlinePaymentForm))
        //     HostScreen.Router.Navigate.Execute(onlinePaymentForm).Subscribe();
        // onlinePaymentForm.OpenStudentSelection();
    }
    #endregion
}