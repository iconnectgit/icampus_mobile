using System.Collections.ObjectModel;
using System.Web;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Enums;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.UserModules.OnlinePayment;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.QuickPayment;

public class QuickPaymentForm : ViewModelBase
    {
        public ICommand PayNowCommand { get; set; }
        public ICommand ShowHistoryCommand { get; set; }
        public ICommand ExpandCollapseClickCommand { get; set; }
        public ICommand DownloadCommand { get; set; }

        private IList<ReceiptDetailView> _receiptDetailList = new List<ReceiptDetailView>();
        PaymentSettingsView _paymentSettings = new PaymentSettingsView();
        public PaymentSettingsView PaymentSettings
        {
            get => _paymentSettings;
            set
            {
                _paymentSettings = value;
                OnPropertyChanged(nameof(PaymentSettings));
            }
        }
        public IList<ReceiptDetailView> ReceiptDetailList
        {
            get => _receiptDetailList;
            set
            {
                _receiptDetailList = value;
                OnPropertyChanged(nameof(ReceiptDetailList));
            }
        }
        private decimal _amount;
        public string Amount
        {
            get => _amount <= 0 ? string.Empty : Convert.ToString(_amount);
            set
            {
                if (decimal.TryParse(HelperMethods.ConvertNumerals(value), out decimal input))
                {
                    _amount = input;
                    OnPropertyChanged(nameof(ReceiptDetailList));
                }
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
        BindableReceiptDetailView _selectedPaymentHistory = new BindableReceiptDetailView();
        public BindableReceiptDetailView SelectedPaymentHistory
        {
            get => _selectedPaymentHistory;
            set
            {
                _selectedPaymentHistory = value;
                OnPropertyChanged(nameof(SelectedPaymentHistory));
            }
        }
        ObservableCollection<BindableReceiptDetailView> _paymentHistoryList = new ObservableCollection<BindableReceiptDetailView>();
        public ObservableCollection<BindableReceiptDetailView> PaymentHistoryList
        {
            get => _paymentHistoryList;
            set
            {
                IsPaymentHistoryNoDataFoundVisibility = _paymentHistoryList != null && _paymentHistoryList.Count > 0 ? false : true; ;
                _paymentHistoryList = value;
                OnPropertyChanged(nameof(PaymentHistoryList));
            }
        }
        bool _isPaymentHistoryNoDataFoundVisibility;
        public bool IsPaymentHistoryNoDataFoundVisibility
        {
            get
            {
                return _isPaymentHistoryNoDataFoundVisibility;
            }
            set
            {
                _isPaymentHistoryNoDataFoundVisibility = value;
                OnPropertyChanged(nameof(IsPaymentHistoryNoDataFoundVisibility));
            }
        }
        private bool _isHistoryShow;
        public bool IsHistoryShow
        {
            get => _isHistoryShow;
            set
            {
                _isHistoryShow = value;
                OnPropertyChanged(nameof(IsHistoryShow));
            }

        }
        private int paymentType;
        private int paymentModeID;
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
        public QuickPaymentForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        
        private async void InitializePage()
        {
            HelperMethods.LogEvent("Device token Info", $"DEVICETOKEN");
            PayNowCommand = new Command(PayNowCommandClicked);
            ShowHistoryCommand = new Command(ShowHistoryCommandClicked);
            DownloadCommand = new Command<BindableReceiptDetailView>(DownloadClicked);
            ExpandCollapseClickCommand = new Command<BindableReceiptDetailView>(ExpandCollapseClicked);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            IsHistoryShow = false;
            MessagingCenter.Subscribe<string>("", "RefreshQuickPaymentHistory", async (args) =>
            {
                var studentId = AppSettings.Current.SelectedStudent.ItemId;
                await ClearStudentCacheData(studentId);
                await GetHistoryList();
            });
            //SetBeamAppViews();
        }

        private async Task GetDetails()
        {
            try
            {
                paymentType = Convert.ToInt32(PaymentTypes.QuickPayment);
                PaymentSettings = await ApiHelper.GetObject<PaymentSettingsView>(string.Format(TextResource.GetPaymentSettingsUrl, paymentType), cacheKeyPrefix:"quickpaymentsettings", cacheType: ApiHelper.CacheTypeParam.LoadFromCache);

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
                    CurrentAcademicYear = Convert.ToString(PaymentSettings.FinancialYear);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }

        private async Task<IList<ReceiptDetailView>> GetHistoryList()
        {
            try
            {
                paymentModeID = Convert.ToInt32(PaymentTypes.QuickPayment);
                var receiptDetailList = await ApiHelper.GetObjectList<ReceiptDetailView>(string.Format(TextResource.TopupHistoryApiUrl, AppSettings.Current.SelectedStudent.ItemId, paymentModeID), cacheKeyPrefix:"quickpaymenthistory", cacheType:ApiHelper.CacheTypeParam.LoadFromCache);

                if (receiptDetailList != null)
                {
                    PaymentHistoryList = new ObservableCollection<BindableReceiptDetailView>(
                        _mapper.Map<List<BindableReceiptDetailView>>(receiptDetailList)
                            .GroupBy(x => x.OrderId)
                            .Select(g => g.First())
                        );
                    foreach (var item in PaymentHistoryList)
                    {
                        if (item != null)
                        {
                            item.TransactionDateTime = item.TransactionDate.ToString("dd-MMM-yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.TopupHistoryPageTitle);
            }
            return ReceiptDetailList;
        }

        private async Task GetTaxReceiptData(BindableReceiptDetailView bindableReceiptDetailView)
        {
            try
            {
                BindableReceiptDetailView result = await ApiHelper.PostRequest<BindableReceiptDetailView>(string.Format(TextResource.GetTaxReceiptDetails, bindableReceiptDetailView.StudentId, bindableReceiptDetailView.PaymentModeId, bindableReceiptDetailView.IsBalanceAmountPayment, bindableReceiptDetailView.OrderId), AppSettings.Current.ApiUrl);
                if (result != null)
                {
                    SelectedPaymentHistory = result;
                    SelectedPaymentHistory.FormattedTransactionDate = bindableReceiptDetailView.FormattedTransactionDate;

                    if (SelectedPaymentHistory.PaymentInvoiceList != null && SelectedPaymentHistory.PaymentInvoiceList.Count > 0)
                    {
                        SelectedPaymentHistory.StudentName = SelectedPaymentHistory.PaymentInvoiceList.FirstOrDefault().StudentName;
                        SelectedPaymentHistory.GradeName = SelectedPaymentHistory.PaymentInvoiceList.FirstOrDefault().GradeName;
                    }
                    SelectedPaymentHistory.PrintLogo = PrintLogo;
                    SelectedPaymentHistory.Amount = bindableReceiptDetailView.Amount;
                    SelectedPaymentHistory.AmountInWords = bindableReceiptDetailView.AmountInWords;
                    if (result.PaymentModule == PaymentTypes.OnlineFees || result.PaymentModule == PaymentTypes.ReRegistrationPayment)
                    {
                        SelectedPaymentHistory.ListAmount = result.Amount;
                    }
                    else
                    {
                        foreach (var item in result.PaymentInvoiceList.OrderBy(x => x.StudentId))
                        {
                            SelectedPaymentHistory.ListAmount += item.Amount;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private async void PayNowCommandClicked(object obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(ReasonForPayment) && !string.IsNullOrEmpty(Amount))
                {
                    ConfirmPaymentForm confirmPaymentForm = new ConfirmPaymentForm(Navigation)
                    {
                        PageTitle = "Confirm Payment",
                        MenuVisible = false,
                        BackVisible = true,
                        IsPopUpPage = false,
                        AcademicYear = CurrentAcademicYear,
                        ReasonForPayment = HttpUtility.UrlEncode(ReasonForPayment),
                        Amount = Convert.ToDecimal(_amount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits)),                       
                        ConfirmationMessage = ConfirmationMessage,
                        TermsAndConditions = TermsAndConditions,
                        Mode = paymentModeID,
                        StudentIds = AppSettings.Current.SelectedStudent.ItemId,
                        AcademicYearForBillingDetails = AcademicYear
                    };

                    ConfirmPaymentPage confirmPaymentPage = new ConfirmPaymentPage()
                    {
                        BindingContext = confirmPaymentForm
                    };
                    await Navigation.PushAsync(confirmPaymentPage);

                }
                else
                {
                    if (string.IsNullOrEmpty(ReasonForPayment))
                        App.Current.MainPage.DisplayAlert("", TextResource.EnterReasonEmptyErrorMessage, TextResource.OkText);
                    else
                        App.Current.MainPage.DisplayAlert("", TextResource.AmountEmptyErrorMessage, TextResource.OkText);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }    
        }
        private void ShowHistoryCommandClicked(object obj)
        {
            try
            {
                IsHistoryShow = true;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void DownloadClicked(BindableReceiptDetailView bindableReceiptDetailView)
        {
            try
            {
                await GetTaxReceiptData(bindableReceiptDetailView);
                TaxReceiptForm taxReceiptForm = new TaxReceiptForm(_nativeServices, Navigation)
                {
                    SelectedPaymentHistory = SelectedPaymentHistory,
                    PageTitle = PageTitle,
                    MenuVisible = false,
                    BackVisible = true,
                    IsPopUpPage = false,
                    ParentName = AppSettings.Current.UserFullName
                };

                TaxReceiptView taxReceiptView = new TaxReceiptView()
                {
                    BindingContext = taxReceiptForm
                };
                await Navigation.PushAsync(taxReceiptView);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private void ExpandCollapseClicked(BindableReceiptDetailView bindableReceiptDetailView)
        {
            try
            {
                if (bindableReceiptDetailView != null)
                {
                    foreach (var item in PaymentHistoryList.ToList())
                    {
                        if (item != null)
                        {
                            if (item.OrderId == bindableReceiptDetailView.OrderId)
                            {
                                item.DetailsVisibility = !item.DetailsVisibility;
                                item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                            }
                            else
                            {
                                item.DetailsVisibility = false;
                                item.ArrowImageSource = "dropdown_gray.png";
                            }
                        }
                        
                    }
                    MessagingCenter.Send("", "OnlinePaymentExpandCollapse");
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        public async override void GetStudentData()
        {
            try
            {
                await GetHistoryList();
                base.GetStudentData();
                await GetDetails();
                this.Amount = Convert.ToString(decimal.MinValue);
                this.ReasonForPayment = string.Empty;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        void SetBeamAppViews()
        {
            try
            {
                string clientGroupCode = (!string.IsNullOrEmpty(App.ClientGroupCode)) ? App.ClientGroupCode : string.Empty;
                if (StudentList != null && StudentList.Count > 0)
                {
                    AppSettings.Current.SelectedStudentFromAllStudentList = AppSettings.Current.StudentList.FirstOrDefault();
                }
                if (AppSettings.Current.IsParent || AppSettings.Current.IsTeacher)
                {
                    AppSettings.Current.IsRegisteredStudentListVisible = false;
                    AppSettings.Current.IsAllStudentListVisible = true;
                }
                else
                {
                    AppSettings.Current.IsRegisteredStudentListVisible = AppSettings.Current.IsAllStudentListVisible = false;
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async Task ClearStudentCacheData(string id)
        {
            var allKeys = await ICCacheManager.GetAllKeys();
            foreach (var key in allKeys)
            {
                if (key.StartsWith("quickpaymenthistory") && key.EndsWith($"_{id}"))
                {
                    ICCacheManager.InvalidateObject<CampusKeyView>(key);
                }
            }
        }
    }