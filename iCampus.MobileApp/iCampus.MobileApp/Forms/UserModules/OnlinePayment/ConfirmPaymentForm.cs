using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using iCampus.Portal.ViewModels;
using Mopups.Services;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class ConfirmPaymentForm : ViewModelBase
{
    #region Declarations

    public ICommand ConfirmToPayNowCommand { get; set; }

    #endregion

    #region Properties

    private string _academicYear;

    public string AcademicYear
    {
        get => _academicYear;
        set
        {
            _academicYear = value;
            OnPropertyChanged(nameof(AcademicYear));
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

    private string _currency;

    public string Currency
    {
        get => _currency;
        set
        {
            _currency = value;
            OnPropertyChanged(nameof(Currency));
        }
    }

    private string _confirmationMessage;

    public string ConfirmationMessage
    {
        get => _confirmationMessage;
        set
        {
            _confirmationMessage = value;
            OnPropertyChanged(nameof(ConfirmationMessage));
        }
    }

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
            OnPropertyChanged(nameof(_proformaListString));
        }
    }

    #endregion

    public ConfirmPaymentForm(INavigation navigation) : base(null, null, null)
    {
        Navigation = navigation;
        ConfirmToPayNowCommand = new Command(ConfirmPayNowClicked);
    }

    #region Methods

    private async void ConfirmPayNowClicked()
    {
        if (string.IsNullOrEmpty(TermsAndConditions))
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
            var currentPage = Navigation.NavigationStack.LastOrDefault();
            await Navigation.PushAsync(billingDetailsPage);  
            if (currentPage != null)
            {
                Navigation.RemovePage(currentPage);  
            }
            
        }
        else
        {
            OnlinePaymentTermsandConditionsForm onlinePaymentTermsandConditionsForm =
                new OnlinePaymentTermsandConditionsForm(Navigation);
            onlinePaymentTermsandConditionsForm.PageTitle = "Terms And Conditions";
            onlinePaymentTermsandConditionsForm.MenuVisible = false;
            onlinePaymentTermsandConditionsForm.BackVisible = false;
            onlinePaymentTermsandConditionsForm.IsPopUpPage = true;
            onlinePaymentTermsandConditionsForm.TermsAndConditions = TermsAndConditions;
            onlinePaymentTermsandConditionsForm.Amount = Amount;
            onlinePaymentTermsandConditionsForm.InvoiceSubDetailsList = InvoiceSubDetailsList;
            onlinePaymentTermsandConditionsForm.MinimumPaymentDetailsToPaymentUrl = MinimumPaymentDetailsToPaymentUrl;
            onlinePaymentTermsandConditionsForm.ActualAmount = ActualAmount;
            onlinePaymentTermsandConditionsForm.VatAmount = VatAmount;
            onlinePaymentTermsandConditionsForm.Mode = Mode;
            onlinePaymentTermsandConditionsForm.ReasonForPayment = ReasonForPayment;
            onlinePaymentTermsandConditionsForm.CardNumberEnc = CardNumberEnc;
            onlinePaymentTermsandConditionsForm.SelectedChequeListString = SelectedChequeListString;
            onlinePaymentTermsandConditionsForm.InvoiceIds = InvoiceIds;
            onlinePaymentTermsandConditionsForm.StudentIds = StudentIds;
            onlinePaymentTermsandConditionsForm.ProformaListString = ProformaListString;
            onlinePaymentTermsandConditionsForm.AcademicYearForBillingDetails = AcademicYearForBillingDetails;
            
            var termsAndConditionsPopup = new OnlinePaymentTermsandConditionsPopup()
            {
                BindingContext = onlinePaymentTermsandConditionsForm
            };
            MopupService.Instance.PushAsync(termsAndConditionsPopup);
        }
    }
    public void SetPopupInstance(Popup popup)
    {
        AppSettings.Current.CurrentPopup = popup;
    }

    #endregion
}