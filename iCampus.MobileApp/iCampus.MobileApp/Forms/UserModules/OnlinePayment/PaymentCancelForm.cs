namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class PaymentCancelForm : ViewModelBase
{
    #region Properties

    private string _paymentCancelledMessage;

    public string PaymentCancelledMessage
    {
        get => _paymentCancelledMessage;
        set
        {
            _paymentCancelledMessage = value;
            OnPropertyChanged(nameof(PaymentCancelledMessage));
        }
    }

    #endregion

    public PaymentCancelForm() : base(null, null, null)
    {
    }
}