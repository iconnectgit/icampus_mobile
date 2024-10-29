using System.Windows.Input;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class CardDetailsForm : ViewModelBase
{
    #region Declarations

    public ICommand ExpiryMonthTextChangedCommand { get; set; }
    public ICommand ExpiryYearTextChangedCommand { get; set; }
    public ICommand PayCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    #endregion

    #region Properties

    private decimal _amount;

    public decimal Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            OnPropertyChanged(nameof(Amount));
        }
    }

    private string _cardNumber;

    public string CardNumber
    {
        get => _cardNumber;
        set
        {
            _cardNumber = value;
            OnPropertyChanged(nameof(CardNumber));
        }
    }

    private string _expiryMonth;

    public string ExpiryMonth
    {
        get => _expiryMonth;
        set
        {
            _expiryMonth = value;
            OnPropertyChanged(nameof(ExpiryMonth));
        }
    }

    private string _expiryYear;

    public string ExpiryYear
    {
        get => _expiryYear;
        set
        {
            _expiryYear = value;
            OnPropertyChanged(nameof(ExpiryMonth));
        }
    }

    private string _cvv;

    public string Cvv
    {
        get => _cvv;
        set
        {
            _cvv = value;
            OnPropertyChanged(nameof(Cvv));
        }
    }

    private string _cardHolderName;

    public string CardHolderName
    {
        get => _cardHolderName;
        set
        {
            _cardHolderName = value;
            OnPropertyChanged(nameof(CardNumber));
        }
    }

    private string _mandatoryFieldErrorMessage;

    public string MandatoryFieldErrorMessage
    {
        get => _mandatoryFieldErrorMessage;
        set
        {
            _mandatoryFieldErrorMessage = value;
            OnPropertyChanged(nameof(MandatoryFieldErrorMessage));
        }
    }

    private bool _isCardNumberErrorMessageVisible;

    public bool IsCardNumberErrorMessageVisible
    {
        get => _isCardNumberErrorMessageVisible;
        set
        {
            _isCardNumberErrorMessageVisible = value;
            OnPropertyChanged(nameof(IsCardNumberErrorMessageVisible));
        }
    }

    private bool _isExpiryMonthErrorMessageVisible;

    public bool IsExpiryMonthErrorMessageVisible
    {
        get => _isExpiryMonthErrorMessageVisible;
        set
        {
            _isExpiryMonthErrorMessageVisible = value;
            OnPropertyChanged(nameof(IsExpiryMonthErrorMessageVisible));
        }
    }

    private bool _isExpiryYearErrorMessageVisible;

    public bool IsExpiryYearErrorMessageVisible
    {
        get => _isExpiryYearErrorMessageVisible;
        set
        {
            _isExpiryYearErrorMessageVisible = value;
            OnPropertyChanged(nameof(IsExpiryYearErrorMessageVisible));
        }
    }

    private bool _isCardHolderNameErrorMessageVisible;

    public bool IsCardHolderNameErrorMessageVisible
    {
        get => _isCardHolderNameErrorMessageVisible;
        set
        {
            _isCardHolderNameErrorMessageVisible = value;
            OnPropertyChanged(nameof(IsCardNumberErrorMessageVisible));
        }
    }

    private bool _isCvvErrorMessageVisible;

    public bool IsCvvErrorMessageVisible
    {
        get => _isCvvErrorMessageVisible;
        set
        {
            _isCvvErrorMessageVisible = value;
            OnPropertyChanged(nameof(IsCvvErrorMessageVisible));
        }
    }

    private BindableReceiptDetailView _receiptDetails = new();

    public BindableReceiptDetailView ReceiptDetails
    {
        get => _receiptDetails;
        set
        {
            _receiptDetails = value;
            OnPropertyChanged(nameof(ReceiptDetails));
        }
    }

    private OnlinePaymentFilterView _billingDetails = new();

    public OnlinePaymentFilterView BillingDetails
    {
        get => _billingDetails;
        set
        {
            _billingDetails = value;
            OnPropertyChanged(nameof(BillingDetails));
        }
    }

    #endregion

    public CardDetailsForm(INavigation navigation) : base(null, null, null)
    {
        Navigation = navigation;
        InitializePage();
    }

    private async Task InitializePage()
    {
        ExpiryMonthTextChangedCommand = new Command(ExpiryMonthTextChanged);
        ExpiryYearTextChangedCommand = new Command(ExpiryYearTextChanged);
        PayCommand = new Command(PayClicked);
        CancelCommand = new Command(CancelClicked);
        MandatoryFieldErrorMessage = TextResource.MandatoryFieldErrorMessage;
        await GetBillingDetails();
    }

    #region Methods

    private async void ExpiryMonthTextChanged()
    {
        if (!string.IsNullOrEmpty(ExpiryMonth))
            //if(ExpiryMonth.Length<2)
            //{
            //    ExpiryMonth = string.Concat("0",ExpiryMonth);
            //}
            if (ExpiryMonth.Length > 2)
                ExpiryMonth = ExpiryMonth.Remove(ExpiryMonth.Length - 1);
    }

    private async void ExpiryYearTextChanged()
    {
        if (!string.IsNullOrEmpty(ExpiryYear))
            if (ExpiryYear.Length > 4)
                ExpiryYear = ExpiryYear.Remove(ExpiryYear.Length - 1);
    }

    private async void PayClicked()
    {
        if (!isValid())
            return;
        await InitiateNGeniusPayment();
    }

    private bool isValid()
    {
        IsCardNumberErrorMessageVisible = string.IsNullOrEmpty(CardNumber) ? true : false;
        IsExpiryMonthErrorMessageVisible = string.IsNullOrEmpty(ExpiryMonth) ? true : false;
        IsExpiryYearErrorMessageVisible = string.IsNullOrEmpty(ExpiryYear) ? true : false;
        IsCvvErrorMessageVisible = string.IsNullOrEmpty(Cvv) ? true : false;
        IsCardHolderNameErrorMessageVisible = string.IsNullOrEmpty(CardHolderName) ? true : false;
        if (string.IsNullOrEmpty(CardNumber) || string.IsNullOrEmpty(ExpiryMonth) || string.IsNullOrEmpty(ExpiryYear) ||
            string.IsNullOrEmpty(Cvv) || string.IsNullOrEmpty(CardHolderName)) return false;
        return true;
    }

    private async Task InitiateNGeniusPayment()
    {
        try
        {
            var nGeniusOnlinePaymentCardDetails = new BindableNGeniusOnlinePaymentCardDetails();
            nGeniusOnlinePaymentCardDetails.OrderId = BillingDetails.OrderId;
            nGeniusOnlinePaymentCardDetails.Amount = Amount.ToString();
            nGeniusOnlinePaymentCardDetails.PAN = CardNumber;
            nGeniusOnlinePaymentCardDetails.Expiry = string.Concat(ExpiryYear, "-",
                ExpiryMonth.Length < 2 ? string.Concat("0", ExpiryMonth) : ExpiryMonth);
            nGeniusOnlinePaymentCardDetails.CVV = Cvv;
            nGeniusOnlinePaymentCardDetails.CardHolderName = CardHolderName;
            var result = await ApiHelper.PostRequest<OperationDetails>(
                string.Format(TextResource.InitiateNGeniusPAyment), AppSettings.Current.ApiUrl,
                nGeniusOnlinePaymentCardDetails, true);
            if (result.Success)
            {
                await GetTaxReceiptData();
            }
            else
            {
                //OperationDetails transactionFailed = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.UpdateTransactionFailed, BillingDetails.OrderId));
                //if (transactionFailed!=null)
                //{
                //    if(transactionFailed.Success)
                //    {
                TransactionFailedForm transactionFailedForm = new TransactionFailedForm();
                transactionFailedForm.PageTitle = PageTitle;
                transactionFailedForm.MenuVisible = false;
                transactionFailedForm.BackVisible = true;
                transactionFailedForm.IsPopUpPage = false;
                transactionFailedForm.TransactionFailedMessage = AppSettings.Current.TransactionFailedMessage;
                // if (HostScreen.Router.GetCurrentViewModel().GetType() != typeof(TransactionFailedForm))
                //     HostScreen.Router.Navigate.Execute(transactionFailedForm).Subscribe();
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async Task GetTaxReceiptData()
    {
        try
        {
            if (BillingDetails != null)
            {
                var receiptDetails = await ApiHelper.PostRequest<BindableReceiptDetailView>(
                    string.Format(TextResource.GetTaxReceiptDetailsAfterSuccessfulPayment, BillingDetails.StudentId,
                        BillingDetails.OrderId), AppSettings.Current.ApiUrl);
                if (receiptDetails != null)
                {
                    ReceiptDetails = receiptDetails;
                    ReceiptDetails.PrintLogo = receiptDetails.PaymentSettings.PrintLogo;
                    TaxReceiptForm taxReceiptForm = new TaxReceiptForm(_nativeServices, Navigation);
                    taxReceiptForm.SelectedPaymentHistory = ReceiptDetails;
                    taxReceiptForm.PageTitle = PageTitle;
                    taxReceiptForm.MenuVisible = false;
                    taxReceiptForm.BackVisible = true;
                    taxReceiptForm.IsPopUpPage = false;
                    taxReceiptForm.ParentName = AppSettings.Current.UserFullName;
                    // if (HostScreen.Router.GetCurrentViewModel().GetType() != typeof(TaxReceiptForm))
                    //     HostScreen.Router.Navigate.Execute(taxReceiptForm).Subscribe();
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async Task GetBillingDetails()
    {
        try
        {
            var result =
                await ApiHelper.GetObject<OnlinePaymentFilterView>(string.Format(TextResource.GetBillingDetails));
            if (result != null) BillingDetails = result;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async Task UpdateFailedPayment()
    {
        try
        {
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void CancelClicked()
    {
        try
        {
            var result =
                await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.UpdatePaymentCancelled,
                    BillingDetails.OrderId));
            if (result != null && result.Success)
            {
                PaymentCancelForm paymentCancelForm = new PaymentCancelForm();
                paymentCancelForm.PageTitle = PageTitle;
                paymentCancelForm.MenuVisible = false;
                paymentCancelForm.BackVisible = true;
                paymentCancelForm.IsPopUpPage = false;
                paymentCancelForm.PaymentCancelledMessage = AppSettings.Current.PaymentCancelledMessage;
                // if (HostScreen.Router.GetCurrentViewModel().GetType() != typeof(PaymentCancelForm))
                //     HostScreen.Router.Navigate.Execute(paymentCancelForm).Subscribe();
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    #endregion
}