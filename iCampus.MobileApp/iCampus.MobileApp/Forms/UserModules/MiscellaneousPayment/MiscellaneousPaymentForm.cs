using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.UserModules.OnlinePayment;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.UserModules.MiscellaneousPayment;

public class MiscellaneousPaymentForm : ViewModelBase
{
    #region Declaration

    public ICommand DetailsExpandCollapseClickCommand { get; set; }
    public ICommand CheckboxClickCommand { get; set; }
    public ICommand PayNowCommand { get; set; }
    public ICommand DownloadCommand { get; set; }

    #endregion

    #region Properties

    private ObservableCollection<BindableInvoiceDetailsView> _invoiceDetailsList = new();

    public ObservableCollection<BindableInvoiceDetailsView> InvoiceDetailsList
    {
        get => _invoiceDetailsList;
        set
        {
            _invoiceDetailsList = value;
            OnPropertyChanged(nameof(InvoiceDetailsList));
        }
    }

    private IList<BindableProformaInvoiceDetails> _proformaList;

    public IList<BindableProformaInvoiceDetails> ProformaList
    {
        get => _proformaList;
        set
        {
            _proformaList = value;
            OnPropertyChanged(nameof(ProformaList));
        }
    }

    private List<BindableProformaInvoiceDetails> _selectedFeeDescriptions;

    public List<BindableProformaInvoiceDetails> SelectedFeeDescriptions
    {
        get => _selectedFeeDescriptions;
        set
        {
            _selectedFeeDescriptions = value;
            OnPropertyChanged(nameof(SelectedFeeDescriptions));
        }
    }

    private decimal _payAmount;

    public decimal PayAmount
    {
        get
        {
            try
            {
                return decimal.Parse(
                    _payAmount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        set
        {
            _payAmount = value;
            OnPropertyChanged(nameof(PayAmount));
        }
    }

    private bool _isPayNowVisible;

    public bool IsPayNowVisible
    {
        get => _isPayNowVisible;
        set
        {
            _isPayNowVisible = value;
            OnPropertyChanged(nameof(IsPayNowVisible));
        }
    }

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

    private BindableReceiptDetailView _selectedPaymentHistory = new();

    public BindableReceiptDetailView SelectedPaymentHistory
    {
        get => _selectedPaymentHistory;
        set
        {
            _selectedPaymentHistory = value;
            OnPropertyChanged(nameof(SelectedPaymentHistory));
        }
    }

    private string _printLogo;

    public string PrintLogo
    {
        get => _printLogo;
        set
        {
            _printLogo = value;
            OnPropertyChanged(nameof(PrintLogo));
        }
    }

    private List<BindablePaymentInvoiceList> _paymentInvoiceList = new();

    public List<BindablePaymentInvoiceList> PaymentInvoiceList
    {
        get => _paymentInvoiceList;
        set
        {
            _paymentInvoiceList = value;
            OnPropertyChanged(nameof(PaymentInvoiceList));
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

    private decimal _payVAT;

    public decimal PayVAT
    {
        get
        {
            try
            {
                return decimal.Parse(
                    _payVAT.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        set
        {
            _payVAT = value;
            OnPropertyChanged(nameof(PayVAT));
        }
    }

    private bool _noInvoiceFoundVisibility = false;

    public bool NoInvoiceFoundVisibility
    {
        get => _noInvoiceFoundVisibility;
        set
        {
            _noInvoiceFoundVisibility = value;
            OnPropertyChanged(nameof(NoInvoiceFoundVisibility));
        }
    }

    private List<BindableProformaInvoiceDetails> SelectedList = new();
    public List<int> SelectedPaymentId = new();

    #endregion


    public MiscellaneousPaymentForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(
        mapper, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Mathods

    private async void InitializePage()
    {
        DetailsExpandCollapseClickCommand = new Command<BindableProformaInvoiceDetails>(DetailsExpandCollapseClicked);
        CheckboxClickCommand = new Command(CheckboxClick);
        PayNowCommand = new Command(PayNowCommandClicked);
        DownloadCommand = new Command<BindableProformaInvoiceDetails>(DownloadClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
    }

    private async Task GetDetails()
    {
        try
        {
            var result = await ApiHelper.GetObject<OperationDetails>(
                string.Format(TextResource.GetMiscellaneousPaymentUrl, AppSettings.Current.SelectedStudent.ItemId));
            if (result.Success)
            {
                var json = JsonConvert.SerializeObject(result.Output);
                var data = JsonConvert.DeserializeObject<BindableInvoiceDetailsView>(json);
                ProformaList = new List<BindableProformaInvoiceDetails>(data.ProformaList);
                AppSettings.Current.OnlinePaymentCurrencyCode = data.PaymentSettings.CurrencyData.CurrencyCode;
                AppSettings.Current.OnlinePaymentCurrencyRoundingDigits =
                    data.PaymentSettings.CurrencyData.RoundingDigits;
                AcademicYear = Convert.ToString(data.AcademicYear);
                ConfirmationMessage = data.PaymentSettings.ConfirmationMessage;
                TermsAndConditions = data.PaymentSettings.TermsAndConditionMessage;
                PrintLogo = data.PaymentSettings.PrintLogo;
                foreach (var item in ProformaList)
                {
                    var isPaid = item.IsPaid;
                    item.IsCheckBoxChecked = isPaid;
                    item.IsCheckBoxEnabled = !isPaid;
                    item.CheckBoxOpacity = isPaid ? 0.5F : 1.0F;
                    item.DownloadIconVisibility = isPaid;
                }

                NoInvoiceFoundVisibility = ProformaList.Count <= 0;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public void DetailsExpandCollapseClicked(BindableProformaInvoiceDetails feeDescription)
    {
        try
        {
            if (feeDescription != null)
            {
                foreach (var item in ProformaList)
                    if (item != null)
                    {
                        if (item.ProFormaID == feeDescription.ProFormaID)
                        {
                            item.DetailsVisibility = !item.DetailsVisibility;
                            item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png")
                                ? "dropdown_gray.png"
                                : "uparrow_gray.png";
                        }
                        else
                        {
                            item.DetailsVisibility = false;
                            item.ArrowImageSource = "dropdown_gray.png";
                        }
                    }

                MessagingCenter.Send("", "OnlinePaymentExpandCollapse");
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public void CheckboxClick(object obj)
    {
        try
        {
            var data = obj as BindableProformaInvoiceDetails;
            if (data != null)
            {
                var isChecked = false;
                if (!data.IsPaid)
                    isChecked = data.IsCheckBoxChecked = !data.IsCheckBoxChecked;
                if (isChecked)
                    SelectedPaymentId.Add(data.ProFormaID);
                else
                    SelectedPaymentId.Remove(data.ProFormaID);
            }

            PayNowAmountCalculation();
            IsPayNowVisible = SelectedPaymentId.Count > 0;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public void PayNowAmountCalculation()
    {
        try
        {
            ProformaListString = string.Empty;
            var payAmount = ProformaList.Where(x => SelectedPaymentId.Contains(x.ProFormaID)).ToList();
            PayAmount = 0;
            PayVAT = 0;
            foreach (var item in payAmount)
            {
                PayAmount += Convert.ToDecimal(item.TotalAmount);
                PayVAT += item.VatAmount;
                ProformaListString = JsonConvert.SerializeObject(SelectedPaymentId);
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void PayNowCommandClicked(object obj)
    {
        try
        {
            InvoiceIds = string.Join(",", SelectedPaymentId);
            SelectedPayList();
            var confirmPaymentForm = new ConfirmPaymentForm(Navigation)
            {
                PageTitle = "Confirm Payment",
                MenuVisible = false,
                BackVisible = true,
                IsPopUpPage = false,
                AcademicYear = AcademicYear,
                Amount = PayAmount,
                StudentIds = AppSettings.Current.SelectedStudent.ItemId,
                Mode = Convert.ToInt32(PaymentTypes.MiscellaneousPayment),
                InvoiceIds = InvoiceIds,
                ProformaListString = ProformaListString,
                VatAmount = PayVAT,
                ConfirmationMessage = ConfirmationMessage,
                TermsAndConditions = TermsAndConditions,
                AcademicYearForBillingDetails = Convert.ToInt32(AcademicYear)
            };
            var confirmPaymentPage = new ConfirmPaymentPage()
            {
                BindingContext = confirmPaymentForm
            };
            await Navigation.PushAsync(confirmPaymentPage);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    public void SelectedPayList()
    {
        try
        {
            SelectedList.Clear();
            foreach (var item in SelectedPaymentId)
                if (ProformaList.Any(x => x.ProFormaID == item))
                    SelectedList.Add(ProformaList.FirstOrDefault(x => x.ProFormaID == item));
            ProformaListString = string.Empty;
            ProformaListString = JsonConvert.SerializeObject(SelectedList);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void DownloadClicked(BindableProformaInvoiceDetails bindableProformaInvoice)
    {
        try
        {
            await GetTaxReceiptData(bindableProformaInvoice);
            var taxReceiptForm = new TaxReceiptForm(_nativeServices, Navigation)
            {
                SelectedPaymentHistory = SelectedPaymentHistory,
                PageTitle = PageTitle,
                MenuVisible = false,
                BackVisible = true,
                IsPopUpPage = false,
                Vat = SelectedPaymentHistory.TotalVAT,
                ParentName = AppSettings.Current.UserFullName
            };
            var taxReceiptView = new TaxReceiptView()
            {
                BindingContext = taxReceiptForm
            };
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async Task GetTaxReceiptData(BindableProformaInvoiceDetails bindableProformaInvoice)
    {
        try
        {
            bindableProformaInvoice.StudentID = null;
            var result = await ApiHelper.PostRequest<BindableReceiptDetailView>(
                string.Format(TextResource.GetTaxReceiptDetails, bindableProformaInvoice.StudentID,
                    Convert.ToInt32(PaymentTypes.MiscellaneousPayment), false, bindableProformaInvoice.OrderId),
                AppSettings.Current.ApiUrl);
            if (result != null)
            {
                SelectedPaymentHistory = result;
                SelectedPaymentHistory.FormattedTransactionDate = result.FormattedTransactionDate;
                if (SelectedPaymentHistory.PaymentInvoiceList != null &&
                    SelectedPaymentHistory.PaymentInvoiceList.Count > 0)
                {
                    SelectedPaymentHistory.StudentName =
                        SelectedPaymentHistory.PaymentInvoiceList.FirstOrDefault().StudentName;
                    SelectedPaymentHistory.GradeName =
                        SelectedPaymentHistory.PaymentInvoiceList.FirstOrDefault().GradeName;
                }

                SelectedPaymentHistory.PrintLogo = PrintLogo;
                SelectedPaymentHistory.AmountInWords = result.AmountInWords;


                if (result.PaymentModule == PaymentTypes.OnlineFees ||
                    result.PaymentModule == PaymentTypes.ReRegistrationPayment)
                    SelectedPaymentHistory.ListAmount = result.Amount;
                else
                    foreach (var item in result.PaymentInvoiceList.OrderBy(x => x.StudentId))
                        SelectedPaymentHistory.ListAmount += item.Amount;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }


    public override async void GetStudentData()
    {
        try
        {
            await GetDetails();
            SelectedPaymentId.Clear();
            IsPayNowVisible = false;
            base.GetStudentData();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private void SetBeamAppViews()
    {
        try
        {
            var clientGroupCode = !string.IsNullOrEmpty(App.ClientGroupCode) ? App.ClientGroupCode : string.Empty;
            if (StudentList != null && StudentList.Count > 0)
                AppSettings.Current.SelectedStudentFromAllStudentList =
                    AppSettings.Current.StudentList.FirstOrDefault();
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    #endregion
}