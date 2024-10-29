namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class TransactionFailedForm : ViewModelBase
{
    #region Properties

    private string _transactionFailedMessage;

    public string TransactionFailedMessage
    {
        get => _transactionFailedMessage;
        set
        {
            _transactionFailedMessage = value;
            OnPropertyChanged(nameof(TransactionFailedMessage));
        }
    }

    #endregion

    public TransactionFailedForm() : base(null, null, null)
    {
    }
}