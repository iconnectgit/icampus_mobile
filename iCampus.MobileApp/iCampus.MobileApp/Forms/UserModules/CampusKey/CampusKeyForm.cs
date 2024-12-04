using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.UserModules.OnlinePayment;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.CampusKey;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.CampusKey;

public class CampusKeyForm : ViewModelBase
{
    #region Declarations

    public ICommand ListTappedCommand { get; set; }
    public ICommand TopupCommand { get; set; }
    public CampusKeyView _campusKeyData = new();
    public CampusKeyView _campusKeyInvoiceViewModel = new();
    public ICommand PayNowCommand { get; set; }
    public ICommand TopHistoryCommand { get; set; }
    public ICommand EditDailyLimitCommand { get; set; }
    public ICommand SaveDailyLimitCommand { get; set; }

    #endregion


    #region Properties

    private bool _isTransactionalDetailsAvailable;

    public bool IsTransactionalDetailsAvailable
    {
        get => _isTransactionalDetailsAvailable;
        set
        {
            _isTransactionalDetailsAvailable = value;
            OnPropertyChanged(nameof(IsTransactionalDetailsAvailable));
        }
    }

    private bool _noDataExist = false;

    public bool NoDataExist
    {
        get => _noDataExist;
        set
        {
            _noDataExist = value;
            OnPropertyChanged(nameof(NoDataExist));
        }
    }

    private bool _noActiveCardExist = false;

    public bool NoActiveCardExist
    {
        get => _noActiveCardExist;
        set
        {
            _noActiveCardExist = value;
            OnPropertyChanged(nameof(NoActiveCardExist));
        }
    }

    private int paymentType;

    public CampusKeyView CampusKeyData
    {
        get => _campusKeyData;
        set
        {
            _campusKeyData = value;
            OnPropertyChanged(nameof(CampusKeyData));
        }
    }

    public CampusKeyView CampusKeyInvoiceViewModel
    {
        get => _campusKeyInvoiceViewModel;
        set
        {
            _campusKeyInvoiceViewModel = value;
            OnPropertyChanged(nameof(CampusKeyInvoiceViewModel));
        }
    }

    private StudentCardInformation _studentCardInformation;

    public StudentCardInformation StudentCardInformation
    {
        get => _studentCardInformation;
        set
        {
            _studentCardInformation = value;
            OnPropertyChanged(nameof(StudentCardInformation));
        }
    }

    private TransactionDetails _selectedTransaction = new();

    public TransactionDetails SelectedTransaction
    {
        get => _selectedTransaction;
        set
        {
            _selectedTransaction = value;
            OnPropertyChanged(nameof(SelectedTransaction));
        }
    }

    private bool _isTopupDetailVisible;

    public bool IsTopupDetailVisible
    {
        get => _isTopupDetailVisible;
        set
        {
            _isTopupDetailVisible = value;
            OnPropertyChanged(nameof(IsTopupDetailVisible));
        }
    }

    private bool _isPayNow;

    public bool IsPayNow
    {
        get => _isPayNow;
        set
        {
            _isPayNow = value;
            OnPropertyChanged(nameof(IsPayNow));
        }
    }

    private StudentCardInformation _currencyInformation = new();

    public StudentCardInformation CurrencyInformation
    {
        get => _currencyInformation;
        set
        {
            _currencyInformation = value;
            OnPropertyChanged(nameof(CurrencyInformation));
        }
    }

    private decimal _minTopUpAmount;

    public decimal MinTopUpAmount
    {
        get => _minTopUpAmount;
        set
        {
            _minTopUpAmount = value;
            OnPropertyChanged(nameof(MinTopUpAmount));
        }
    }

    private decimal _topupAmount;

    public string TopupAmount
    {
        get => _topupAmount <= 0 ? string.Empty : Convert.ToString(_topupAmount);
        set
        {
            if (decimal.TryParse(HelperMethods.ConvertNumerals(value), out var input))
            {
                _topupAmount = input;
                OnPropertyChanged(nameof(TopupAmount));
            }
        }
    }

    private int _academicYear;

    public int AcademicYear
    {
        get => _academicYear;
        set
        {
            _academicYear = value;
            OnPropertyChanged(nameof(AcademicYear));
        }
    }

    private string _currencyCode;

    public string CurrencyCode
    {
        get => _currencyCode;
        set
        {
            _currencyCode = value;
            OnPropertyChanged(nameof(CurrencyCode));
        }
    }

    private string _currentAcademicYear;

    public string CurrentAcademicYear
    {
        get => _currentAcademicYear;
        set
        {
            _currentAcademicYear = value;
            OnPropertyChanged(nameof(CurrentAcademicYear));
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

    private string _noInvoiceMessage;

    public string NoInvoiceMessage
    {
        get => _noInvoiceMessage;
        set
        {
            _noInvoiceMessage = value;
            OnPropertyChanged(nameof(NoInvoiceMessage));
        }
    }

    private string _expenseLimit;

    public string ExpenseLimit
    {
        get => _expenseLimit;
        set
        {
            _expenseLimit = value;
            OnPropertyChanged(nameof(ExpenseLimit));
        }
    }

    private string _studentCardNumber;

    public string StudentCardNumber
    {
        get => _studentCardNumber;
        set
        {
            _studentCardNumber = value;
            OnPropertyChanged(nameof(StudentCardNumber));
        }
    }

    private string _studentCardNumberFormatted;

    public string StudentCardNumberFormatted
    {
        get => _studentCardNumberFormatted;
        set
        {
            _studentCardNumberFormatted = value;
            OnPropertyChanged(nameof(StudentCardNumberFormatted));
        }
    }

    private string _lastTopUpAmount;

    public string LastTopUpAmount
    {
        get => _lastTopUpAmount;
        set
        {
            _lastTopUpAmount = value;
            OnPropertyChanged(nameof(LastTopUpAmount));
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

    private PaymentSettingsView _paymentSettings = new();

    public PaymentSettingsView PaymentSettings
    {
        get => _paymentSettings;
        set
        {
            _paymentSettings = value;
            OnPropertyChanged(nameof(PaymentSettings));
        }
    }

    private bool _isAllowUpdatingDailyLimit;

    public bool IsAllowUpdatingDailyLimit
    {
        get => _isAllowUpdatingDailyLimit;
        set
        {
            _isAllowUpdatingDailyLimit = value;
            OnPropertyChanged(nameof(IsAllowUpdatingDailyLimit));
        }
    }

    private bool _showDailyLimit = true;

    public bool ShowDailyLimit
    {
        get => _showDailyLimit;
        set
        {
            _showDailyLimit = value;
            OnPropertyChanged(nameof(ShowDailyLimit));
        }
    }

    private bool _showUpdatingDailyLimit = false;

    public bool ShowUpdatingDailyLimit
    {
        get => _showUpdatingDailyLimit;
        set
        {
            _showUpdatingDailyLimit = value;
            OnPropertyChanged(nameof(ShowUpdatingDailyLimit));
        }
    }

    private decimal _updatingDailyLimitAmount;

    public string UpdatingDailyLimitAmount
    {
        get => _updatingDailyLimitAmount <= 0 ? string.Empty : Convert.ToString(_updatingDailyLimitAmount);
        set
        {
            if (decimal.TryParse(HelperMethods.ConvertNumerals(value), out var input))
            {
                _updatingDailyLimitAmount = input;
                OnPropertyChanged(nameof(UpdatingDailyLimitAmount));
            }
        }
    }

    private decimal _minDailyLimit;

    public decimal MinDailyLimit
    {
        get => _minDailyLimit;
        set
        {
            _minDailyLimit = value;
            OnPropertyChanged(nameof(MinDailyLimit));
        }
    }

    #endregion

    public CampusKeyForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }


    #region Private Methods

    private async void InitializePage()
    {
        FormTitle = TextResource.CampusKeyPageTitle;
        BackVisible = false;
        MenuVisible = true;
        IsTopupDetailVisible = false;
        IsPayNow = false;
        ListTappedCommand = new Command<TransactionDetails>(ListViewTapped);
        TopupCommand = new Command(TopupClicked);
        TopHistoryCommand = new Command(TopupHistoryClicked);
        PayNowCommand = new Command(PayNowCommandClicked);
        EditDailyLimitCommand = new Command(EditDailyLimitMethod);
        SaveDailyLimitCommand = new Command(SaveDailyLimitMethod);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        SetBeamAppViews();
        MessagingCenter.Subscribe<string>("", "RefreshPaymentHistoryData", async (args) =>
        {
            var studentId = AppSettings.Current.SelectedStudent.ItemId;
            IsPayNow = false;
            ClearStudentCacheData(studentId);
        });
    }

    private async Task<CampusKeyView> GetCampusKeyList()
    {
        try
        {
            IsPayNow = false;
            IsTopupDetailVisible = false;
            var studentId = AppSettings.Current.SelectedStudent.ItemId;
            var cacheKeyPrefix = "cashlessdata";
            CampusKeyData = await ApiHelper.GetObject<CampusKeyView>(
                TextResource.CampusKeyApiUrl + "?studentId=" + AppSettings.Current.SelectedStudent.ItemId,
                cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
            IsTransactionalDetailsAvailable = CampusKeyData.TransactionDetails.Any();
            if (IsTransactionalDetailsAvailable)
                CampusKeyData.TransactionDetails.ToList().ForEach(x =>
                    x.TransactionDateFormatted = x.TransactionDate.ToString("MMM dd, yyyy"));


            CurrentAcademicYear = Convert.ToString(CampusKeyData.FinancialYear);
            MinTopUpAmount = Convert.ToDecimal(CampusKeyData.CampusKeySettings.MinTopUpAmount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
            CurrencyCode = CampusKeyData.StudentCardInformation.CurrencyCode;
            IsAllowUpdatingDailyLimit =
                AppSettings.Current.IsParent && CampusKeyData.CampusKeySettings.AllowUpdatingDailyLimit;
            MinDailyLimit = CampusKeyData.CampusKeySettings.MinDailyLimit;
            var studentdata =
                CampusKeyData.StudentCardInformationList.FirstOrDefault(x => x.StudentId.ToString() == studentId);
            if (studentdata != null)
            {
                ExpenseLimit = studentdata.ExpenseLimit;
                StudentCardNumber = studentdata.StudentCardNumber;
                StudentCardNumberFormatted = studentdata.StudentCardNumberFormatted;
                LastTopUpAmount = studentdata.TopUpAmount;
            }
            else
            {
                ExpenseLimit = string.Empty;
                StudentCardNumber = string.Empty;
                StudentCardNumberFormatted = string.Empty;
                LastTopUpAmount = string.Empty;
            }

            if (CampusKeyData.CampusKeySettings.EnableCardTopUp && !string.IsNullOrEmpty(StudentCardNumber))
                IsTopupDetailVisible = true;

            NoActiveCardExist = !IsTopupDetailVisible;
            NoDataExist = NoActiveCardExist ? false : !IsTransactionalDetailsAvailable;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
        }

        return CampusKeyData;
    }

    private async Task GetDetails()
    {
        try
        {
            paymentType = Convert.ToInt32(PaymentTypes.CanteenTopup);
            var cacheKeyPrefix = "paymentsettings";
            PaymentSettings = await ApiHelper.GetObject<PaymentSettingsView>(
                string.Format(TextResource.GetPaymentSettingsUrl, paymentType), cacheKeyPrefix: cacheKeyPrefix,
                cacheType: ApiHelper.CacheTypeParam.LoadFromCache);

            if (PaymentSettings != null)
            {
                AppSettings.Current.OnlinePaymentCurrencyCode = PaymentSettings.CurrencyData.CurrencyCode;
                AppSettings.Current.OnlinePaymentCurrencyRoundingDigits = PaymentSettings.CurrencyData.RoundingDigits;
                PrintLogo = PaymentSettings.PrintLogoUrl;
                ConfirmationMessage = PaymentSettings.ConfirmationMessage;
                AppSettings.Current.TransactionFailedMessage = PaymentSettings.FailureMessage;
                AppSettings.Current.PaymentCancelledMessage = PaymentSettings.CancelMessage;
                TermsAndConditions = PaymentSettings.TermsAndConditionMessage;
                NoInvoiceMessage = PaymentSettings.NoInvoiceMessage;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            //Crashes.TrackError(ex);
        }
    }

    private async void ListViewTapped(TransactionDetails obj)
    {
        try
        {
            if (obj != null)
            {
                if (obj.CashlessTransactionTypeName == "TOPUP")
                {
                }
                else
                {
                    //TODO:list view click event
                    CampusKeyDetailsForm campusKeyDetailform = new (_mapper, _nativeServices, Navigation)
                    {
                        CampusKeyObject = obj,
                        BackVisible = true,
                        MenuVisible = false,
                        PageTitle = PageTitle
                    };
                    CampusKeyInvoiceViewModel = await campusKeyDetailform.GetCampusKeyInvoiceDetails(obj.TransactionId);
                    campusKeyDetailform.InVoiceDetailsObject = CampusKeyInvoiceViewModel.InvoiceDetails;
                    campusKeyDetailform.ListViewHeight = CampusKeyInvoiceViewModel.InvoiceDetails.Count() * 45;
                    campusKeyDetailform.TotalBalance = CampusKeyInvoiceViewModel.InvoiceDetails.Sum(x => x.Balance);
                    campusKeyDetailform.TotalQuantity = CampusKeyInvoiceViewModel.InvoiceDetails.Sum(x => Convert.ToInt32(x.Quantity));
                    SelectedTransaction = null;

                    CampusKeyDetails campusKeyDetails = new CampusKeyDetails()
                    {
                        BindingContext = campusKeyDetailform
                    };
                    await Navigation.PushAsync(campusKeyDetails);
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
        }
    }


    private async void TopupClicked()
    {
        try
        {
            TopupAmount = Convert.ToString(decimal.MinValue);
            await GetCampusKeyList();
            if (!NoActiveCardExist)
                IsPayNow = true;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
        }
    }

    private async void PayNowCommandClicked(object obj)
    {
        try
        {
            if (MinTopUpAmount > _topupAmount)
            {
                var action = Application.Current.MainPage.DisplayAlert("", TextResource.TopupText + MinTopUpAmount + " " + CurrencyCode, TextResource.OkText);
            }
            else
            {
                var confirmPaymentForm = new ConfirmPaymentForm(Navigation)
                {
                    PageTitle = "Confirm Payment",
                    MenuVisible = false,
                    BackVisible = true,
                    IsPopUpPage = false,
                    AcademicYear = CurrentAcademicYear,
                    Amount = Convert.ToDecimal(
                        _topupAmount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits)),
                    Mode = Convert.ToInt32(PaymentTypes.CanteenTopup),
                    CardNumberEnc = StudentCardNumber,
                    StudentIds = AppSettings.Current.SelectedStudent.ItemId,
                    ConfirmationMessage = ConfirmationMessage,
                    TermsAndConditions = TermsAndConditions
                };
                var confirmPaymentPage = new ConfirmPaymentPage()
                {
                    BindingContext = confirmPaymentForm
                };
                await Navigation.PushAsync(confirmPaymentPage);
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
        }
    }

    private async void SaveDailyLimitMethod(object obj)
    {
        try
        {
            _updatingDailyLimitAmount =
                decimal.Parse(
                    _updatingDailyLimitAmount.ToFormattedAmount(AppSettings.Current
                        .OnlinePaymentCurrencyRoundingDigits));
            if (MinDailyLimit > _updatingDailyLimitAmount)
            {
                var action = Application.Current.MainPage.DisplayAlert("",
                    TextResource.DailyLimitText + MinDailyLimit + " " + CurrencyCode, TextResource.OkText);
                UpdatingDailyLimitAmount = Convert.ToString(decimal.MinValue);
            }
            else
            {
                ShowDailyLimit = true;
                ShowUpdatingDailyLimit = false;
                var studentId = AppSettings.Current.SelectedStudent.ItemId;
                var operationDetails = await ApiHelper.PostRequest<OperationDetails>(
                    string.Format(TextResource.SaveDailyLimitApiUrl, studentId, _updatingDailyLimitAmount),
                    AppSettings.Current.ApiUrl);
                if (operationDetails.Success)
                {
                    await HelperMethods.ShowAlert("", TextResource.DailyLimitSaveSuccessMessage);
                    ExpenseLimit = Convert.ToString(_updatingDailyLimitAmount) + " " + CurrencyCode;
                    await ClearCampusKeyCacheData(studentId);
                }
                else
                {
                    await HelperMethods.ShowAlert("", TextResource.Error);
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
        }
    }

    private void EditDailyLimitMethod(object obj)
    {
        try
        {
            UpdatingDailyLimitAmount = Convert.ToString(decimal.MinValue);
            ShowDailyLimit = false;
            ShowUpdatingDailyLimit = true;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
        }
    }

    private async void TopupHistoryClicked(object obj)
    {
        try
        {
            TopupHistoryForm topupHistoryForm = new TopupHistoryForm(_mapper, _nativeServices, Navigation)
            {
                PageTitle = "Portal Topup History",
                MenuVisible = false,
                BackVisible = true,
                IsPopUpPage = false,
                PrintLogo = PrintLogo
            };
            topupHistoryForm.OpenStudentSelection();
            PortalTopupHistoryPage portalTopupHistoryPage = new PortalTopupHistoryPage()
            {
                BindingContext = topupHistoryForm
            };
            await Navigation.PushAsync(portalTopupHistoryPage);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
        }
    }

    public override async void GetStudentData()
    {
        try
        {
            base.GetStudentData();
            await GetDetails();
            await GetCampusKeyList();
            TopupAmount = Convert.ToString(decimal.MinValue);
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
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
            if (AppSettings.Current.IsParent || AppSettings.Current.IsTeacher)
            {
                AppSettings.Current.IsRegisteredStudentListVisible = false;
                AppSettings.Current.IsAllStudentListVisible = true;
            }
            else
            {
                AppSettings.Current.IsRegisteredStudentListVisible =
                    AppSettings.Current.IsAllStudentListVisible = false;
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
        }
    }

    private async void ClearStudentCacheData(string id)
    {
        var allKeys = await ICCacheManager.GetAllKeys();
        foreach (var key in allKeys)
            if (key.StartsWith("campuskeyhistory") && key.EndsWith($"_{id}"))
                ICCacheManager.InvalidateObject<CampusKeyView>(key);
    }

    private async Task ClearCampusKeyCacheData(string studentId)
    {
        var allKeys = await ICCacheManager.GetAllKeys();
        foreach (var key in allKeys)
            if (key.StartsWith("cashlessdata") && key.EndsWith($"_{studentId}"))
                ICCacheManager.InvalidateObject<CampusKeyView>(key);
    }

    #endregion
}