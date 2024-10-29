using System.Windows.Input;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class OnlinePaymentTermsandConditionsForm : ViewModelBase
{
    #region Declarations

    public ICommand AcceptCommand { get; set; }

    #endregion

    #region Properties

    private string _termsAndConditions;

    public string TermsAndConditions
    {
        get => _termsAndConditions;
        set
        {
            _termsAndConditions = value;
            OnPropertyChanged(nameof(TermsAndConditions));
        }
    }

    private decimal _vatAmount;

    public decimal VatAmount
    {
        get => _vatAmount;
        set
        {
            _vatAmount = value;
            OnPropertyChanged(nameof(VatAmount));
        }
    }

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

    private decimal _actualAmount;

    public decimal ActualAmount
    {
        get => _actualAmount;
        set
        {
            _actualAmount = value;
            OnPropertyChanged(nameof(ActualAmount));
        }
    }

    private List<OnlinePaymentInvoiceSubDetails> _invoiceSubDetailsList = new();

    public List<OnlinePaymentInvoiceSubDetails> InvoiceSubDetailsList
    {
        get => _invoiceSubDetailsList;
        set
        {
            _invoiceSubDetailsList = value;
            OnPropertyChanged(nameof(InvoiceSubDetailsList));
        }
    }

    private int _academicYearForBillingDetails;

    public int AcademicYearForBillingDetails
    {
        get => _academicYearForBillingDetails;
        set
        {
            _academicYearForBillingDetails = value;
            OnPropertyChanged(nameof(AcademicYearForBillingDetails));
        }
    }

    private string _minimumPaymentDetailsToPaymentUrl;

    public string MinimumPaymentDetailsToPaymentUrl
    {
        get => _minimumPaymentDetailsToPaymentUrl;
        set
        {
            _minimumPaymentDetailsToPaymentUrl = value;
            OnPropertyChanged(nameof(MinimumPaymentDetailsToPaymentUrl));
        }
    }

    private int _mode;

    public int Mode
    {
        get => _mode;
        set
        {
            _mode = value;
            OnPropertyChanged(nameof(Mode));
        }
    }

    private string _reasonForPayment;

    public string ReasonForPayment
    {
        get => _reasonForPayment;
        set
        {
            _reasonForPayment = value;
            OnPropertyChanged(nameof(ReasonForPayment));
        }
    }

    private string _cardNumberEnc;

    public string CardNumberEnc
    {
        get => _cardNumberEnc;
        set
        {
            _cardNumberEnc = value;
            OnPropertyChanged(nameof(CardNumberEnc));
        }
    }

    private string _selectedChequeListString;

    public string SelectedChequeListString
    {
        get => _selectedChequeListString;
        set
        {
            _selectedChequeListString = value;
            OnPropertyChanged(nameof(SelectedChequeListString));
        }
    }

    private string _invoiceIds;

    public string InvoiceIds
    {
        get => _invoiceIds;
        set
        {
            _invoiceIds = value;
            OnPropertyChanged(nameof(InvoiceIds));
        }
    }

    private string _studentIds;

    public string StudentIds
    {
        get => _studentIds;
        set
        {
            _studentIds = value;
            OnPropertyChanged(nameof(StudentIds));
        }
    }

    private string _proformaListString;

    public string ProformaListString
    {
        get => _proformaListString;
        set
        {
            _proformaListString = value;
            OnPropertyChanged(nameof(ProformaListString));
        }
    }

    #endregion

    public OnlinePaymentTermsandConditionsForm(INavigation navigation) : base(null, null, null)
    {
        Navigation = navigation;
        AcceptCommand = new Command(AcceptClicked);
    }

    #region Methods

    private async void AcceptClicked()
    {
        var billingDetailsForm = new BillingDetailsForm(Navigation)
        {
            PageTitle = "Billing Details",
            MenuVisible = false,
            BackVisible = true,
            IsPopUpPage = false,
            Amount = Amount,
            InvoiceSubDetailsList = InvoiceSubDetailsList,
            MinimumPaymentDetailsToPaymentUrl = MinimumPaymentDetailsToPaymentUrl,
            ActualAmount = ActualAmount,
            VatAmount = VatAmount,
            Mode = Mode,
            ReasonForPayment = ReasonForPayment,
            CardNumberEnc = CardNumberEnc,
            PendingAndBouncedPdcListString = SelectedChequeListString,
            InvoiceIds = InvoiceIds,
            StudentIds = StudentIds,
            ProformaListString = ProformaListString,
            AcademicYearForBillingDetails = AcademicYearForBillingDetails
        };

        BillingDetailsPage billingDetailsPage = new BillingDetailsPage()
        {
            BindingContext = billingDetailsForm
        }; 
        AppSettings.Current.CurrentPopup?.Close(); 
        var currentPage = Navigation.NavigationStack.LastOrDefault();
        await Navigation.PushAsync(billingDetailsPage);  
        if (currentPage != null)
        {
            Navigation.RemovePage(currentPage);  
        }
        
    }
    #endregion
}



