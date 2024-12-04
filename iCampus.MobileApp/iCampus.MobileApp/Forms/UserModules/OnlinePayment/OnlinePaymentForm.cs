using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using iCampus.Portal.ViewModels;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class OnlinePaymentForm : ViewModelBase
    {
        #region Declarations
        public ICommand DownloadCommand { get; set; }
        public ICommand PayNowCommand { get; set; }
        public ICommand OtherAmountEntryTextChangedCommand { get; set; }
        public ICommand TotalAmountRadioButtonCommand { get; set; }
        public ICommand MinimumAmountRadioButtonCommand { get; set; }
        public ICommand OtherAmountRadioButtonCommand { get; set; }
        public ICommand ExpandCollapseClickCommand { get; set; }
        
        public ICommand PendingInvoiceCommand { get; set; }
        public ICommand PaymentHistoryCommand { get; set; }



        #endregion
        #region Properties
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
        ObservableCollection<BindableInvoiceDetails> _invoiceDetailsList = new ObservableCollection<BindableInvoiceDetails>();
        public ObservableCollection<BindableInvoiceDetails> InvoiceDetailsList
        {
            get => _invoiceDetailsList;
            set
            {
                _invoiceDetailsList = value;
                OnPropertyChanged(nameof(InvoiceDetailsList));
            }
        }
        List<OnlinePaymentInvoiceSubDetails> _invoiceSubDetailsList = new List<OnlinePaymentInvoiceSubDetails>();
        public List<OnlinePaymentInvoiceSubDetails> InvoiceSubDetailsList
        {
            get => _invoiceSubDetailsList;
            set
            {
                _invoiceSubDetailsList = value;
                OnPropertyChanged(nameof(InvoiceSubDetailsList));
            }
        }
        private bool _isPaymentTypeSelectionVisible;
        public bool IsPaymentTypeSelectionVisible
        {
            get => _isPaymentTypeSelectionVisible;
            set
            {
                _isPaymentTypeSelectionVisible = value;
                OnPropertyChanged(nameof(IsPaymentTypeSelectionVisible));
            }
        }
        private bool _isTotalVisible = true;
        public bool IsTotalVisible
        {
            get
            {
                return _isTotalVisible;
            }
            set
            {
                _isTotalVisible = value;
                OnPropertyChanged(nameof(IsTotalVisible));
            }
        }
        private bool _isMinimumFixedAmountVisible;
        public bool IsMinimumFixedAmountVisible
        {
            get
            {
                return _isMinimumFixedAmountVisible;
            }
            set
            {
                _isMinimumFixedAmountVisible = value;
                OnPropertyChanged(nameof(IsMinimumFixedAmountVisible));
            }
        }
        private bool _isMinimumAmountOptionVisible = true;
        public bool IsMinimumAmountOptionVisible
        {
            get
            {
                return _isMinimumAmountOptionVisible;
            }
            set
            {
                IsMinimumAmountOptionVisible = value;
                OnPropertyChanged(nameof(IsMinimumAmountOptionVisible));
            }
        }
        private decimal _totalBalanceAmount;
        public decimal TotalBalanceAmount
        {
            get
            {
                if (_totalBalanceAmount != 0)
                {
                    _totalBalanceAmount = Decimal.Parse(_totalBalanceAmount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                }
                if (IsPaymentTypeSelectionVisible)
                {
                    IsTotalVisible = false;
                }
                else
                {
                    IsTotalVisible = (_totalBalanceAmount != 0) ? true : false;
                }
                return _totalBalanceAmount;
            }
            set
            {
                _totalBalanceAmount = value;
                OnPropertyChanged(nameof(TotalBalanceAmount));
            }
        }
        private decimal _minimumFixedAmount;
        public decimal MinimumFixedAmount
        {
            get
            {
                if (_minimumFixedAmount != 0)
                {
                    _minimumFixedAmount = Decimal.Parse(_minimumFixedAmount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                }
                return _minimumFixedAmount;
            }
            set
            {
                _minimumFixedAmount = value;
                OnPropertyChanged(nameof(MinimumFixedAmount));
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
        private decimal _minimumAmountPercentage;
        public decimal MinimumAmountPercentage
        {
            get => _minimumAmountPercentage;
            set
            {
                _minimumAmountPercentage = value;
                OnPropertyChanged(nameof(MinimumAmountPercentage));
            }
        }
        private decimal _minimumAmount=0;
        public decimal MinimumAmount
        {
            get
            {
                if (_minimumAmount != 0)
                {
                    _minimumAmount = Decimal.Parse(_minimumAmount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                }
                return _minimumAmount;
            }
            set
            {
                _minimumAmount = value;
                OnPropertyChanged(nameof(MinimumAmount));
            }
        }
        private decimal _otherAmountEntered;
        public string OtherAmountEntered
        {
            get => _otherAmountEntered <= 0 ? string.Empty : Convert.ToString(_otherAmountEntered);
            set
            {
                if (decimal.TryParse(HelperMethods.ConvertNumerals(value), out decimal input))
                {
                    _minimumAmount = input;
                    OnPropertyChanged(nameof(OtherAmountEntered));
                }
            }
        }
        private decimal _remainingPaymentDue;
        public decimal RemainingPaymentDue
        {
            get => _remainingPaymentDue;
            set
            {
                _remainingPaymentDue = value;
                OnPropertyChanged(nameof(RemainingPaymentDue));
            }
        }
        private string _totalAmountRadioButtonImage;
        public string TotalAmountRadioButtonImage
        {
            get => _totalAmountRadioButtonImage;
            set
            {
                _totalAmountRadioButtonImage = value;
                OnPropertyChanged(nameof(TotalAmountRadioButtonImage));
            }
        }
        private string _minimumAmountRadioButtonImage;
        public string MinimumAmountRadioButtonImage
        {
            get => _minimumAmountRadioButtonImage;
            set
            {
                _minimumAmountRadioButtonImage = value;
                OnPropertyChanged(nameof(MinimumAmountRadioButtonImage));
            }
        }
        private string _otherAmountRadioButtonImage;
        public string OtherAmountRadioButtonImage
        {
            get => _otherAmountRadioButtonImage;
            set
            {
                _otherAmountRadioButtonImage = value;
                OnPropertyChanged(nameof(OtherAmountRadioButtonImage));
            }
        }
        private bool _isRemainingPaymentDueVisible;
        public bool IsRemainingPaymentDueVisible
        {
            get => _isRemainingPaymentDueVisible;
            set
            {
                _isRemainingPaymentDueVisible = value;
                OnPropertyChanged(nameof(IsRemainingPaymentDueVisible));
            }
        }
        private bool _isOtherAmountEntryEnabled;
        public bool IsOtherAmountEntryEnabled
        {
            get => _isOtherAmountEntryEnabled;
            set
            {
                _isOtherAmountEntryEnabled = value;
                OnPropertyChanged(nameof(IsOtherAmountEntryEnabled));
            }
        }
        private bool _isOtherAmountErrorMessaheVisible;
        public bool IsOtherAmountErrorMessaheVisible
        {
            get => _isOtherAmountErrorMessaheVisible;
            set
            {
                _isOtherAmountErrorMessaheVisible = value;
                OnPropertyChanged(nameof(IsOtherAmountErrorMessaheVisible));
            }
        }
        private string _otherAmountErrorMessage;
        public string OtherAmountErrorMessage
        {
            get => _otherAmountErrorMessage;
            set
            {
                _otherAmountErrorMessage = value;
                OnPropertyChanged(nameof(OtherAmountErrorMessage));
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
        ObservableCollection<BindableReceiptDetailView> _paymentHistoryList = new ObservableCollection<BindableReceiptDetailView>();
        public ObservableCollection<BindableReceiptDetailView> PaymentHistoryList
        {
            get => _paymentHistoryList;
            set
            {
                _paymentHistoryList = value;
                IsPaymentHistoryNoDataFoundVisibility = _paymentHistoryList != null && _paymentHistoryList.Count > 0 ? false : true; ;
                OnPropertyChanged(nameof(PaymentHistoryList));
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
        private decimal _vat;
        public decimal Vat
        {
            get => _vat;
            set
            {
                _vat = value;
                OnPropertyChanged(nameof(Vat));
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
        OnlinePaymentFilterView _dataForSavingBillingDetails = new OnlinePaymentFilterView();
        public OnlinePaymentFilterView DataForSavingBillingDetails
        {
            get => _dataForSavingBillingDetails;
            set
            {
                _dataForSavingBillingDetails = value;
                OnPropertyChanged(nameof(DataForSavingBillingDetails));
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
        private decimal _payAmount;
        public decimal PayAmount
        {
            get => _payAmount;
            set
            {
                _payAmount = value;
                OnPropertyChanged(nameof(PayAmount));
            }
        }
        private decimal _vatAmount;
        public decimal VatAmount
        {
            get
            {
                if(_vatAmount!=0)
                _vatAmount= Decimal.Parse(_vatAmount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                return _vatAmount;
            } 
            set
            {
                _vatAmount = value;
                OnPropertyChanged(nameof(VatAmount));
            }
        }
        private decimal _grandTotal;
        public decimal GrandTotal
        {
            get
            {
                if(_grandTotal!=0)
                _grandTotal= Decimal.Parse(_grandTotal.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                return _grandTotal;
            }
            set
            {
                _grandTotal = value;
                OnPropertyChanged(nameof(GrandTotal));
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
        private decimal _minimumAmountFromApi;
        public decimal MinimumAmountFromApi
        {
            get => _minimumAmountFromApi;
            set
            {
                _minimumAmountFromApi = value;
                OnPropertyChanged(nameof(MinimumAmountFromApi));
            }
        }
        private decimal _minimumVatAmount;
        public decimal MinimumVatAmount
        {
            get => _minimumVatAmount;
            set
            {
                _minimumVatAmount = value;
                OnPropertyChanged(nameof(MinimumVatAmount));
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
        private bool _vatAndGrandTotalVisibility;
        public bool VatAndGrandTotalVisibility
        {
            get => _vatAndGrandTotalVisibility;
            set
            {
                _vatAndGrandTotalVisibility = value;
                OnPropertyChanged(nameof(VatAndGrandTotalVisibility));
            }
        }
        private bool _noInvoiceFoundVisibility=false;
        public bool NoInvoiceFoundVisibility
        {
            get => _noInvoiceFoundVisibility;
            set
            {
                _noInvoiceFoundVisibility = value;
                OnPropertyChanged(nameof(NoInvoiceFoundVisibility));
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
        private int _listViewHeight;
        public int ListViewHeight
        {
            get => _listViewHeight;
            set
            {
                _listViewHeight = value;
                OnPropertyChanged(nameof(ListViewHeight));
            }
        }
        
        private bool _isPaymentHistoryVisible;
        public bool IsPaymentHistoryVisible
        {
            get => _isPaymentHistoryVisible;
            set
            {
                _isPaymentHistoryVisible = value;
                OnPropertyChanged(nameof(IsPaymentHistoryVisible));
            }
        }
        private bool _isPendingInvoiceVisible;
        public bool IsPendingInvoiceVisible
        {
            get => _isPendingInvoiceVisible;
            set
            {
                _isPendingInvoiceVisible = value;
                OnPropertyChanged(nameof(IsPendingInvoiceVisible));
            }
        }    
        private decimal _invoiceButtonOpacity;
        public decimal InvoiceButtonOpacity
        {
            get => _invoiceButtonOpacity;
            set
            {
                _invoiceButtonOpacity = value;
                OnPropertyChanged(nameof(InvoiceButtonOpacity));
            }
        }     
        private decimal _historyButtonOpacity;
        public decimal HistoryButtonOpacity
        {
            get => _historyButtonOpacity;
            set
            {
                _historyButtonOpacity = value;
                OnPropertyChanged(nameof(HistoryButtonOpacity));
            }
        }     
        
        
        #endregion
        public OnlinePaymentForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        #region Methods
        private async void InitializePage()
        {
            InvoiceButtonOpacity = 1.0m;
            HistoryButtonOpacity = 0.5m;
            IsPendingInvoiceVisible = true;
            IsPaymentHistoryVisible = false;
            DownloadCommand = new Command<BindableReceiptDetailView>(DownloadClicked);
            PayNowCommand = new Command<BindableInvoiceDetails>(PayNowClicked);
            OtherAmountEntryTextChangedCommand = new Command<BindableInvoiceDetails>(OtherAmountTextChanged);
            TotalAmountRadioButtonCommand = new Command<BindableInvoiceDetails>(TotalAmountRadioButtonClicked);
            MinimumAmountRadioButtonCommand = new Command<BindableInvoiceDetails>(MinimumAmountRadioButtonClicked);
            OtherAmountRadioButtonCommand = new Command<BindableInvoiceDetails>(OtherAmountRadioButtonClicked);
            ExpandCollapseClickCommand = new Command<BindableReceiptDetailView>(ExpandCollapseClicked);
            PendingInvoiceCommand = new Command(PendingInvoiceCommandMethod);
            PaymentHistoryCommand = new Command(PaymentHistoryCommandMethod);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            MinimumAmountRadioButtonImage = "selected_radio_button.png";
            TotalAmountRadioButtonImage = OtherAmountRadioButtonImage="unselected_radio_button.png";
                SetBeamAppViews();
        }
        
        private void PendingInvoiceCommandMethod(object obj)
        {
            IsPendingInvoiceVisible = true;
            IsPaymentHistoryVisible = false;
            InvoiceButtonOpacity = 1.0m;
            HistoryButtonOpacity = 0.5m;
        }
        private void PaymentHistoryCommandMethod(object obj)
        {
            //MessagingCenter.Send("", "OnlinePaymentExpandCollapse");
            IsPendingInvoiceVisible = false;
            IsPaymentHistoryVisible = true;
            InvoiceButtonOpacity = 0.5m;
            HistoryButtonOpacity = 1.0m;
        }

        
        private async void DownloadClicked(BindableReceiptDetailView bindableReceiptDetailView)
        {
            await GetTaxReceiptData(bindableReceiptDetailView);
            TaxReceiptForm taxReceiptForm = new TaxReceiptForm(_nativeServices, Navigation)
            {
                SelectedPaymentHistory = SelectedPaymentHistory,
                PageTitle = PageTitle,
                MenuVisible = false,
                BackVisible = true,
                IsPopUpPage = false,
                Vat = SelectedPaymentHistory.TotalVAT,
                ParentName = AppSettings.Current.UserFullName
            };

            TaxReceiptView taxReceiptView = new TaxReceiptView()
            {
                BindingContext = taxReceiptForm
            };
            await Navigation.PushAsync(taxReceiptView);
           
            if (!string.IsNullOrEmpty(bindableReceiptDetailView.SynchronizationError))
            {
                await App.Current.MainPage.DisplayAlert("", bindableReceiptDetailView.SynchronizationError, TextResource.OkText);

            }
        }

        private void ExpandCollapseClicked(BindableReceiptDetailView bindableReceiptDetailView)
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
        private async Task GetInvoiceDetails()
        {
            try
            {
                OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.OnlinePaymentDetails, AppSettings.Current.SelectedStudentFromAllStudentList.ItemId), AppSettings.Current.ApiUrl);
                if (result.Success)
                {
                    if(result.Output!=null)
                    {
                        string json = JsonConvert.SerializeObject(result.Output);
                        var data = JsonConvert.DeserializeObject<BindableInvoiceDetailsViewModel>(json);
                        if(data!=null&&(data.DetailList!=null&&data.DetailList.Count>0))
                        {
                            #region CurrencyCode
                            if(data.CurrencyData!=null)
                            {
                                AppSettings.Current.OnlinePaymentCurrencyCode = data.CurrencyData.CurrencyCode;
                                AppSettings.Current.OnlinePaymentCurrencyRoundingDigits = data.CurrencyData.RoundingDigits;
                            }
                            #endregion

                            #region PaymentHistory
                            PaymentHistoryList = new ObservableCollection<BindableReceiptDetailView>(_mapper.Map<List<BindableReceiptDetailView>>(data.PaymentHistory));
                            foreach (var item in PaymentHistoryList)
                            {
                                if(item!=null)
                                {
                                    item.TransactionDateTime = item.TransactionDate.ToString("dd-MMM-yyyy HH:mm:ss tt",System.Globalization.CultureInfo.InvariantCulture);
                                }
                            }
                            #endregion
                            InvoiceDetailsList = new ObservableCollection<BindableInvoiceDetails>( _mapper.Map<List<BindableInvoiceDetails>>(data.DetailList));
                            #region DataForSavingBillingDetails
                            //InvoiceSubDetailsList = InvoiceDetailsList.FirstOrDefault().InvoiceSubDetailsList;
                            //DataForSavingBillingDetails.InvoiceSubDetailListString = JsonConvert.SerializeObject(InvoiceSubDetailsList);
                            if(PaymentHistoryList!=null&&PaymentHistoryList.Count>0)
                            {
                                Mode = PaymentHistoryList.FirstOrDefault().PaymentModeId;
                            }
                            //AcademicYear = InvoiceDetailsList.FirstOrDefault().AcademicYear;//today
                            #endregion
                            if(InvoiceDetailsList!=null)
                            {
                                foreach (var item in InvoiceDetailsList)
                                {
                                    if (item!=null)
                                    {
                                        if(item.InvoiceDetailsList!=null&&item.InvoiceDetailsList.Count>0)
                                        {
                                            if (item.TotalVATAmount>0)
                                            {
                                                item.VatAndGrandTotalVisibility = true;
                                            }
                                            
                                            item.HeaderText = $"{(item.IsCurrentYear ? "Current Year Invoices " : "Next Year Invoices ")} ({item.AcademicYearTitle})";
                                        }
                                        NoInvoiceFoundVisibility = (item.InvoiceSubDetailsList == null || item.InvoiceSubDetailsList.Count <= 0) ? true : false;
                                        item.IsPayNowVisible = item.TotalAmount <= 0 ? false : true;


                                        if (data.PaymentSettings != null)
                                        {
                                            item.IsPaymentTypeSelectionVisible = item.IsRemainingPaymentDueVisible = data.PaymentSettings.AllowMinimumPayment();
                                            item.IsTotalVisible = !item.IsPaymentTypeSelectionVisible;
                                            if (data.PaymentSettings.AllowMinimumPaymentPercentage())
                                            {
                                                item.MinimumAmountPercentage = Convert.ToDecimal(data.PaymentSettings.MinimumPaymentPercentage);
                                                item.MinimumAmount = (item.TotalAmount * item.MinimumAmountPercentage) / 100;
                                                item.IsMinimumFixedAmountVisible = false;
                                                item.PayAmount = item.VatAndGrandTotalVisibility ? item.GrandTotal : item.MinimumAmount;
                                            }
                                            else if (data.PaymentSettings.AllowMinimumPaymentFixedAmount())
                                            {
                                                item.MinimumAmount = item.MinimumFixedAmount = Convert.ToDecimal(data.PaymentSettings.MinimumFixedAmount);
                                                decimal fixedAmountPercentage = item.GrandTotal > 0 ? (item.MinimumFixedAmount * 100) / item.GrandTotal : 0;
                                                item.MinimumVatAmount = (item.TotalVATAmount * fixedAmountPercentage) / 100;
                                                item.IsMinimumFixedAmountVisible = true;
                                                item.IsMinimumAmountOptionVisible = item.MinimumFixedAmount >= item.TotalAmount ? false : true;
                                                item.PayAmount = item.VatAndGrandTotalVisibility ? item.GrandTotal : item.MinimumAmount;
                                                if (!item.IsMinimumAmountOptionVisible)
                                                {
                                                    TotalAmountRadioButtonClicked(item);
                                                    item.MinimumAmount = 0;
                                                    item.PayAmount = item.VatAndGrandTotalVisibility ? item.GrandTotal : item.TotalAmount;
                                                }
                                            }
                                            item.RemainingPaymentDue = item.TotalAmount - item.MinimumAmount;
                                        }
                                        if (!item.IsPaymentTypeSelectionVisible)
                                        {
                                            item.PayAmount = item.VatAndGrandTotalVisibility ? item.GrandTotal : item.TotalAmount;
                                        }

                                        item.MinimumPaymentDetailsToPaymentUrl = string.Format("&IsMinimumPayment={0}&MinimumAmount={1}&MinimumVatAmount={2}&MinimumPaymentPercentage={3}", IsPaymentTypeSelectionVisible, MinimumAmountFromApi, MinimumVatAmount, MinimumAmountPercentage);

                                    }
                                }
                            }
                            PrintLogo = data.PaymentSettings.PrintLogoUrl;
                            
                            if (data.PaymentSettings != null)
                            {
                                ConfirmationMessage = data.PaymentSettings.ConfirmationMessage;
                                AppSettings.Current.TransactionFailedMessage = data.PaymentSettings.FailureMessage;
                                AppSettings.Current.PaymentCancelledMessage = data.PaymentSettings.CancelMessage;
                                TermsAndConditions = data.PaymentSettings.TermsAndConditionMessage;
                                NoInvoiceMessage = data.PaymentSettings.NoInvoiceMessage;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        public override async void GetStudentData()
        {
            base.GetStudentData();
            await GetInvoiceDetails();
        }
        private async void PayNowClicked(BindableInvoiceDetails bindableInvoice)
        {
            if (!IsValid(bindableInvoice))
                return;
            ConfirmPaymentForm confirmPaymentForm = new ConfirmPaymentForm(Navigation)
            {
                PageTitle = PageTitle,
                MenuVisible = false,
                BackVisible = true,
                IsPopUpPage = false,
                AcademicYear = bindableInvoice.AcademicYearTitle,
                Amount = Convert.ToDecimal(bindableInvoice.PayAmount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits)),
                ConfirmationMessage = ConfirmationMessage,
                TermsAndConditions = TermsAndConditions,
                InvoiceSubDetailsList = bindableInvoice.InvoiceSubDetailsList,
                MinimumPaymentDetailsToPaymentUrl = bindableInvoice.MinimumPaymentDetailsToPaymentUrl,
                ActualAmount = bindableInvoice.TotalAmount,
                VatAmount = bindableInvoice.TotalVATAmount,
                Mode = Convert.ToInt32(PaymentTypes.OnlineFees),
                StudentIds = AppSettings.Current.SelectedStudent.ItemId,
                AcademicYearForBillingDetails = bindableInvoice.AcademicYear
            };

            ConfirmPaymentPage confirmPaymentPage = new ConfirmPaymentPage()
            {
                BindingContext = confirmPaymentForm
            };
            await Navigation.PushAsync(confirmPaymentPage);
        }
        private async void OtherAmountTextChanged(BindableInvoiceDetails bindableInvoice)
        {
            if(!string.IsNullOrEmpty(bindableInvoice.OtherAmountEntered))
            {
                if(Convert.ToDecimal(bindableInvoice.OtherAmountEntered) < bindableInvoice.MinimumAmount || Convert.ToDecimal(bindableInvoice.OtherAmountEntered) > bindableInvoice.TotalAmount)
                {
                    bindableInvoice.RemainingPaymentDue = bindableInvoice.TotalAmount;
                    return;
                }
            }
            bindableInvoice.RemainingPaymentDue = bindableInvoice.TotalAmount - (string.IsNullOrEmpty(bindableInvoice.OtherAmountEntered) ? 0 : Convert.ToDecimal(bindableInvoice.OtherAmountEntered));
            bindableInvoice.PayAmount = !string.IsNullOrEmpty(bindableInvoice.OtherAmountEntered) ?Convert.ToDecimal(bindableInvoice.OtherAmountEntered) : 0;
            bindableInvoice.PayAmount = bindableInvoice.PayAmount + bindableInvoice.TotalVATAmount;
        }
        private async void TotalAmountRadioButtonClicked(BindableInvoiceDetails bindableInvoice)
        {
            bindableInvoice.TotalAmountRadioButtonImage = "selected_radio_button.png";
            bindableInvoice.MinimumAmountRadioButtonImage = bindableInvoice.OtherAmountRadioButtonImage = "unselected_radio_button.png";
            bindableInvoice.IsRemainingPaymentDueVisible = false;
            bindableInvoice.OtherAmountEntered = string.Empty;
            bindableInvoice.IsOtherAmountEntryEnabled = false;
            bindableInvoice.PayAmount = bindableInvoice.TotalAmount + bindableInvoice.TotalVATAmount;
        }
        private async void MinimumAmountRadioButtonClicked(BindableInvoiceDetails bindableInvoice)
        {
            bindableInvoice.MinimumAmountRadioButtonImage = "selected_radio_button.png";
            bindableInvoice.TotalAmountRadioButtonImage = bindableInvoice.OtherAmountRadioButtonImage = "unselected_radio_button.png";
            bindableInvoice.IsRemainingPaymentDueVisible = true;
            bindableInvoice.RemainingPaymentDue = bindableInvoice.TotalAmount - bindableInvoice.MinimumAmount;
            bindableInvoice.OtherAmountEntered = string.Empty;
            bindableInvoice.IsOtherAmountEntryEnabled = false;
            bindableInvoice.PayAmount = bindableInvoice.MinimumAmount + bindableInvoice.TotalVATAmount;
        }
        private async void OtherAmountRadioButtonClicked(BindableInvoiceDetails bindableInvoice)
        {
            bindableInvoice.OtherAmountRadioButtonImage = "selected_radio_button.png";
            bindableInvoice.TotalAmountRadioButtonImage = bindableInvoice.MinimumAmountRadioButtonImage = "unselected_radio_button.png";
            bindableInvoice.IsRemainingPaymentDueVisible = true;
            bindableInvoice.RemainingPaymentDue = bindableInvoice.TotalAmount;
            bindableInvoice.IsOtherAmountEntryEnabled = true;
        }
        private bool IsValid(BindableInvoiceDetails bindableInvoice)
        {
            if(!string.IsNullOrEmpty(bindableInvoice.OtherAmountEntered))
            {
                if(Convert.ToDecimal(bindableInvoice.OtherAmountEntered) < bindableInvoice.MinimumAmount)
                {
                    bindableInvoice.OtherAmountErrorMessage = String.Format(TextResource.InvalidOtherAmountErrorMessage, bindableInvoice.MinimumAmount, bindableInvoice.TotalAmount);
                    bindableInvoice.IsOtherAmountErrorMessaheVisible = true;
                    return false;
                }
                else if(Convert.ToDecimal(bindableInvoice.OtherAmountEntered) > bindableInvoice.TotalAmount)
                {
                    bindableInvoice.OtherAmountErrorMessage = String.Format(TextResource.InvalidOtherAmountErrorMessage, bindableInvoice.MinimumAmount, bindableInvoice.TotalAmount);
                    bindableInvoice.IsOtherAmountErrorMessaheVisible = true;
                    return false;
                }
            }
            else if(bindableInvoice.IsOtherAmountEntryEnabled && string.IsNullOrEmpty(bindableInvoice.OtherAmountEntered))
            {
                bindableInvoice.OtherAmountErrorMessage = TextResource.OtherAmountEmptyErrorMessage;
                bindableInvoice.IsOtherAmountErrorMessaheVisible = true;
                return false;
            }
            bindableInvoice.IsOtherAmountErrorMessaheVisible = false;
            return true;
        }
        private async Task GetTaxReceiptData(BindableReceiptDetailView bindableReceiptDetailView)
        {
            try
            {
                bindableReceiptDetailView.StudentId = null;
                BindableReceiptDetailView result = await ApiHelper.PostRequest<BindableReceiptDetailView>(string.Format(TextResource.GetTaxReceiptDetails, bindableReceiptDetailView.StudentId, bindableReceiptDetailView.PaymentModeId, bindableReceiptDetailView.IsBalanceAmountPayment, bindableReceiptDetailView.OrderId), AppSettings.Current.ApiUrl);
                if (result!=null)
                {
                    SelectedPaymentHistory = result;
                    SelectedPaymentHistory.FormattedTransactionDate = bindableReceiptDetailView.FormattedTransactionDate;
                    if (SelectedPaymentHistory.PaymentInvoiceList != null && SelectedPaymentHistory.PaymentInvoiceList.Count > 0)
                    {
                        SelectedPaymentHistory.StudentName = SelectedPaymentHistory.PaymentInvoiceList.FirstOrDefault().StudentName;
                        SelectedPaymentHistory.GradeName = SelectedPaymentHistory.PaymentInvoiceList.FirstOrDefault().GradeName;
                    }
                    SelectedPaymentHistory.PrintLogo = PrintLogo;
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

                    if (result.PaymentModule == PaymentTypes.PdcPayment)
                    {
                        if (result.ExtraAmount.HasValue && result.ExtraAmount.Value > 0)
                        {
                            SelectedPaymentHistory.AdvanceFee = Convert.ToDecimal(SelectedPaymentHistory.ExtraAmount);
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        void SetBeamAppViews()
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
        #endregion
    }