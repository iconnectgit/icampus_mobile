using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.UserModules.OnlinePayment;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using iCampus.Portal.ViewModels;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.UserModules.ChequeReplacement;

public class ChequeReplacementForm : ViewModelBase
    {
        public ICommand PaymentsCommand { get; set; }
        public ICommand HistoryCommand { get; set; }
        public ICommand PayNowCommand { get; set; }
        public ICommand BounceExpandCollapseClickCommand { get; set; }
        public ICommand PendingExpandCollapseClickCommand { get; set; }
        public ICommand HistoryExpandCollapseClickCommand { get; set; }
        public ICommand CheckboxClickCommand { get; set; }



        PdcReplacementDataViewModel _pdcRepalcementDetails = new PdcReplacementDataViewModel();
        public PdcReplacementDataViewModel PdcRepalcementDetails
        {
            get => _pdcRepalcementDetails;
            set
            {
                _pdcRepalcementDetails = value;
                OnPropertyChanged(nameof(PdcRepalcementDetails));
            }
        }
        IEnumerable<PdcDataView> _pendingPdcList;
        public IEnumerable<PdcDataView> PendingPdcList
        {
            get => _pendingPdcList;
            set
            {
                _pendingPdcList = value;
                OnPropertyChanged(nameof(PendingPdcList));
            }
        }
        ObservableCollection<BindablePdcDataView> _bindablePendingPdcList;
        public ObservableCollection<BindablePdcDataView> BindablePendingPdcList
        {
            get => _bindablePendingPdcList;
            set
            {
                _bindablePendingPdcList = value;
                OnPropertyChanged(nameof(BindablePendingPdcList));
            }
        }
        ObservableCollection<BindablePdcDataView> _bindableBouncePdcList;
        public ObservableCollection<BindablePdcDataView> BindableBouncePdcList
        {
            get => _bindableBouncePdcList;
            set
            {
                _bindableBouncePdcList = value;
                OnPropertyChanged(nameof(BindableBouncePdcList));
            }
        }
        ObservableCollection<BindablePdcDataView> _pdcHistoryList;
        public ObservableCollection<BindablePdcDataView> PdcHistoryList
        {
            get => _pdcHistoryList;
            set
            {
                _pdcHistoryList = value;
                OnPropertyChanged(nameof(PdcHistoryList));
            }
        }
        ObservableCollection<BindablePdcDataView> _bindablePendingAndBouncedPdcList;
        public ObservableCollection<BindablePdcDataView> BindablePendingAndBouncedPdcList
        {
            get => _bindablePendingAndBouncedPdcList;
            set
            {
                _bindablePendingAndBouncedPdcList = value;
                OnPropertyChanged(nameof(BindablePendingAndBouncedPdcList));
            }
        }

        IEnumerable<PdcDataView> _bouncedPdcList;
        public IEnumerable<PdcDataView> BouncedPdcList
        {
            get => _bouncedPdcList;
            set
            {
                _bouncedPdcList = value;
                OnPropertyChanged(nameof(BouncedPdcList));
            }
        }
        IEnumerable<PdcDataView> _selectedPendingPdcList;
        public IEnumerable<PdcDataView> SelectedPendingPdcList
        {
            get => _selectedPendingPdcList;
            set
            {
                _selectedPendingPdcList = value;
                OnPropertyChanged(nameof(SelectedPendingPdcList));
            }
        }
        IEnumerable<PdcDataView> _selectedBouncedPdcList;
        public IEnumerable<PdcDataView> SelectedBouncedPdcList
        {
            get => _selectedBouncedPdcList;
            set
            {
                _selectedBouncedPdcList = value;
                OnPropertyChanged(nameof(SelectedBouncedPdcList));
            }
        }

        IEnumerable<PdcDataView> _selectedPdcHistoryList;
        public IEnumerable<PdcDataView> SelectedPdcHistoryList
        {
            get => _selectedPdcHistoryList;
            set
            {
                _selectedPdcHistoryList = value;
                OnPropertyChanged(nameof(SelectedPdcHistoryList));
            }
        }
        IEnumerable<PdcDataView> _pendingAndBouncedPdcList;
        public IEnumerable<PdcDataView> PendingAndBouncedPdcList
        {
            get => _pendingAndBouncedPdcList;
            set
            {
                _pendingAndBouncedPdcList = value;
                OnPropertyChanged(nameof(PendingAndBouncedPdcList));
            }
        }
        private bool _isHistory;
        public bool IsHistory
        {
            get => _isHistory;
            set
            {
                _isHistory = value;
                OnPropertyChanged(nameof(IsHistory));
            }
        }
        private bool _isPayment;
        public bool IsPayment
        {
            get => _isPayment;
            set
            {
                _isPayment = value;
                OnPropertyChanged(nameof(IsPayment));
            }
        }
        private bool _isPaymentsDetails;
        public bool IsPaymentsDetails
        {
            get => _isPaymentsDetails;
            set
            {
                _isPaymentsDetails = value;
                OnPropertyChanged(nameof(IsPaymentsDetails));
            }
        }
        private bool _isExtraAmountEnabled;
        public bool IsExtraAmountEnabled
        {
            get => _isExtraAmountEnabled;
            set
            {
                _isExtraAmountEnabled = value;
                OnPropertyChanged(nameof(IsExtraAmountEnabled));
            }
        }
        private bool _isExtraAmountVatEnabled;
        public bool IsExtraAmountVatEnabled
        {
            get => _isExtraAmountVatEnabled;
            set
            {
                _isExtraAmountVatEnabled = value;
                OnPropertyChanged(nameof(IsExtraAmountVatEnabled));
            }
        }
        private bool _canPayInSequenceOnly;
        public bool CanPayInSequenceOnly
        {
            get => _canPayInSequenceOnly;
            set
            {
                _canPayInSequenceOnly = value;
                OnPropertyChanged(nameof(CanPayInSequenceOnly));
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
        private string _grandTotal;
        public string GrandTotal
        {
            get => _grandTotal;
            set
            {
                _grandTotal = value;
                OnPropertyChanged(nameof(GrandTotal));
            }
        }
        private decimal _payAmount;
        public decimal PayAmount
        {
            get
            {
                try
                {
                    return Decimal.Parse((_payAmount).ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
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
        private decimal _payExtraCharge;
        public decimal PayExtraCharge
        {
            get
            {
                try
                {
                    return Decimal.Parse((_payExtraCharge).ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            set
            {
                _payExtraCharge = value;
                OnPropertyChanged(nameof(PayExtraCharge));
            }
        }
        private decimal _payVAT;
        public decimal PayVAT
        {
            get
            {
                try
                {
                    return Decimal.Parse((_payVAT).ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
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
        private bool _pendingPdcLabel;
        public bool PendingPdcLabel
        {
            get => _pendingPdcLabel;
            set
            {
                _pendingPdcLabel = value;
                OnPropertyChanged(nameof(PendingPdcLabel));
            }
        }
        private bool _bouncedPdcVisible;
        public bool BouncedPdcVisible
        {
            get => _bouncedPdcVisible;
            set
            {
                _bouncedPdcVisible = value;
                OnPropertyChanged(nameof(BouncedPdcVisible));
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
        private bool _noHistoryFoundVisibility = false;
        public bool NoHistoryFoundVisibility
        {
            get => _noHistoryFoundVisibility;
            set
            {
                _noHistoryFoundVisibility = value;
                OnPropertyChanged(nameof(NoHistoryFoundVisibility));
            }
        }
        private decimal _paymentButtonOpacity;
        public decimal PaymentButtonOpacity
        {
            get => _paymentButtonOpacity;
            set
            {
                _paymentButtonOpacity = value;
                OnPropertyChanged(nameof(PaymentButtonOpacity));
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
         
        public List<string> SelectedChequeNumbers = new List<string>();
        public List<int> StudentIdsList = new List<int>();
        List<PdcDataView> SelectedChequeList = new List<PdcDataView>();


        public ChequeReplacementForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        private async void InitializePage()
        {
            HelperMethods.GetSelectedStudent();
            PaymentButtonOpacity = 1.0m;
            HistoryButtonOpacity = 0.5m;
            PaymentsCommand = new Command(PaymentsCommandClicked);
            HistoryCommand = new Command(HistoryCommandClicked);
            PayNowCommand = new Command(PayNowCommandClicked);
            BounceExpandCollapseClickCommand = new Command<BindablePdcDataView>(BounceExpandCollapseClicked);
            PendingExpandCollapseClickCommand = new Command<BindablePdcDataView>(PendingExpandCollapseClicked);
            HistoryExpandCollapseClickCommand = new Command<BindablePdcDataView>(HistoryExpandCollapseClicked);
            CheckboxClickCommand = new Command(CheckboxClick);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);

            SetBeamAppViews();
            this.IsHistory = false;
            this.IsPayment = true;
            this.IsPayNowVisible = false;
            IsPayNowVisible = SelectedChequeNumbers.Count > 0;

            //GetDetails();
        }

        private async void PayNowCommandClicked(object obj)
        {
            try
            {
                StudentIds = string.Join(",", StudentIdsList);
                InvoiceIds = string.Join(",", SelectedChequeNumbers);
                SelectedList();
                ConfirmPaymentForm confirmPaymentForm = new ConfirmPaymentForm(Navigation)
                {
                    PageTitle = "Confirm Payment",
                    MenuVisible = false,
                    BackVisible = true,
                    IsPopUpPage = false,
                    AcademicYear = AcademicYear,
                    Amount = Convert.ToDecimal(GrandTotal),
                    Mode = Convert.ToInt32(PaymentTypes.PdcPayment),
                    SelectedChequeListString = SelectedChequeListString,
                    InvoiceIds = InvoiceIds,
                    StudentIds = StudentIds,
                    VatAmount = PayVAT,
                    ConfirmationMessage = ConfirmationMessage,
                    TermsAndConditions = TermsAndConditions,
                    AcademicYearForBillingDetails = Convert.ToInt32(AcademicYear)
                };
                ConfirmPaymentPage confirmPaymentPage = new ConfirmPaymentPage()
                {
                    BindingContext = confirmPaymentForm
                };
                await Navigation.PushAsync(confirmPaymentPage);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private void HistoryCommandClicked(object obj)
        {
            IsHistory = true;
            IsPayment = false;
            IsPayNowVisible = false;
            NoInvoiceFoundVisibility = false;
            PaymentButtonOpacity = 0.5m;
            HistoryButtonOpacity = 1.0m;
        }

        private void PaymentsCommandClicked(object obj)
        {
            IsHistory = false;
            IsPayment = true;
            IsPayNowVisible = SelectedChequeNumbers.Count > 0;
            NoInvoiceFoundVisibility = BindablePendingPdcList.Count <= 0 && BindableBouncePdcList.Count <= 0;
            PaymentButtonOpacity = 1.0m;
            HistoryButtonOpacity = 0.5m;
        }

        private async Task GetDetails()
        {
            try
            {
                OperationDetails result = await ApiHelper.GetObject<OperationDetails>(string.Format(TextResource.GetSubmittedChequeListUrl));
                if (result.Success)
                {
                    string json = JsonConvert.SerializeObject(result.Output);
                    var pdcRepalcementDetails = JsonConvert.DeserializeObject<BindablePdcReplacementDataViewModel>(json);
                    PdcHistoryList = new ObservableCollection<BindablePdcDataView>(pdcRepalcementDetails.PdcHistoryList);
                    BindablePendingPdcList = new ObservableCollection<BindablePdcDataView>(pdcRepalcementDetails
                        .PendingAndBouncedPdcList
                        .Where(x => x.ChequeType.Equals("P", StringComparison.CurrentCultureIgnoreCase)).ToList());
                    BindableBouncePdcList = new ObservableCollection<BindablePdcDataView>(pdcRepalcementDetails.PendingAndBouncedPdcList.Where(x => x.ChequeType.Equals("B", StringComparison.CurrentCultureIgnoreCase)).ToList());
                    BindablePendingAndBouncedPdcList = new ObservableCollection<BindablePdcDataView>(pdcRepalcementDetails.PendingAndBouncedPdcList);
                    
                    var data = JsonConvert.DeserializeObject<PdcReplacementDataViewModel>(json);
                    PendingAndBouncedPdcList = data.PendingAndBouncedPdcList;
                    AppSettings.Current.OnlinePaymentCurrencyCode = pdcRepalcementDetails.PaymentSettings.CurrencyData.CurrencyCode;
                    AppSettings.Current.OnlinePaymentCurrencyRoundingDigits = pdcRepalcementDetails.PaymentSettings.CurrencyData.RoundingDigits;
                    AcademicYear = Convert.ToString(pdcRepalcementDetails.AcademicYear);
                    ConfirmationMessage = pdcRepalcementDetails.PaymentSettings.ConfirmationMessage;
                    TermsAndConditions = pdcRepalcementDetails.PaymentSettings.TermsAndConditionMessage;

                    IsExtraAmountEnabled = pdcRepalcementDetails.ChequeReplacementSettings.IsExtraAmountEnabled;
                    IsExtraAmountVatEnabled = pdcRepalcementDetails.ChequeReplacementSettings.IsExtraAmountVatEnabled;
                    CanPayInSequenceOnly = pdcRepalcementDetails.ChequeReplacementSettings.CanPayInSequenceOnly;
                    PendingPdcLabel = BindablePendingPdcList.Count > 0;
                    BouncedPdcVisible = BindableBouncePdcList.Count > 0;
                    foreach (var detail in BindableBouncePdcList)
                    {
                        bool isBouncedPdcDataExistForBothYears = BindableBouncePdcList.Select(x => x.FinancialBeginYear).Distinct().Count() > 1;

                        if (!isBouncedPdcDataExistForBothYears || detail.IsCurrent == true)
                        {
                            detail.IsBounceCheckBoxChecked = true;
                            detail.IsBouncedCheckBoxVisible = true;
                            detail.IsBounceCheckBoxEnabled = false;
                            detail.CheckBoxOpacity = 0.5F;
                            IsPayNowVisible = true;
                            SelectedChequeNumbers.Add(detail.ReceiptNo);
                            StudentIdsList.Add(detail.StudentId);
                        }
                    }

                    if (!BindableBouncePdcList.Any())
                    {
                        bool isPendingPdcDataExistForBothYears = BindablePendingPdcList.Select(x => x.FinancialBeginYear).Distinct().Count() > 1;

                        foreach (var item in BindablePendingPdcList)
                        {
                            if (item.Remarks.Contains("Payable", StringComparison.OrdinalIgnoreCase) && (!isPendingPdcDataExistForBothYears || item.IsCurrent == true))
                            {
                                item.IsPendingCheckBoxVisible = true;
                            }
                        }
                    }
                    NoInvoiceFoundVisibility = BindablePendingPdcList.Count <= 0 && BindableBouncePdcList.Count <= 0;
                    NoHistoryFoundVisibility = PdcHistoryList.Count <= 0;
                   
                    PayNowAmountCalculation();


                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        public void ResetData()
        {
            PayAmount = 0;
            PayExtraCharge = 0;
            PayVAT = 0;
            GrandTotal = string.Empty;
        }
        public bool SelectSequenceToPay(List<PdcDataView> pendingList, string ReceiptNo, bool isChecked)
        {
            try
            {
                if(CanPayInSequenceOnly)
                {
                    var indexOfCurrentReceipt = pendingList.IndexOf(pendingList.FirstOrDefault(x => x.ReceiptNo == ReceiptNo));
                    if (isChecked)
                    {
                        if (SelectedChequeNumbers.Count == 0 && indexOfCurrentReceipt != 0)
                        {
                            App.Current.MainPage.DisplayAlert("", TextResource.PdcReplacementText, TextResource.OkText);
                            return false;
                        }
                        else if (SelectedChequeNumbers.Count != 0 && pendingList.IndexOf(pendingList.FirstOrDefault(x => x.ReceiptNo == SelectedChequeNumbers.Last())) + 1 != indexOfCurrentReceipt)
                        {
                            App.Current.MainPage.DisplayAlert("", TextResource.PdcReplacementText, TextResource.OkText);
                            return false;
                        }
                    }
                    else
                    {
                        if ((SelectedChequeNumbers.IndexOf(ReceiptNo) + 1) < SelectedChequeNumbers.Count())
                        {
                            App.Current.MainPage.DisplayAlert("", TextResource.PdcReplacementText, TextResource.OkText);
                            return false;
                        }
                    }
                }
                              
                if(!isChecked)
                {
                    SelectedChequeNumbers.Remove(ReceiptNo);
                    var studentId = PendingAndBouncedPdcList.FirstOrDefault(x => x.ReceiptNo == ReceiptNo)?.StudentId;
                    if( studentId != null)
                        StudentIdsList.Remove(studentId.Value);
                    return true;
                }
                else if (isChecked)
                {
                    SelectedChequeNumbers.Add(ReceiptNo);
                    var studentId = PendingAndBouncedPdcList.FirstOrDefault(x => x.ReceiptNo == ReceiptNo)?.StudentId;
                    if (studentId != null)
                        StudentIdsList.Add(studentId.Value);
                    return true;
                }
                return true;

            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                return false;
            }
        }
        public void PayNowAmountCalculation()
        {
            try
            {
                ResetData();
                var payAmount = PendingAndBouncedPdcList.Where(x => SelectedChequeNumbers.Contains(x.ReceiptNo));
                foreach (var item in payAmount)
                {
                    PayAmount += item.Amount;
                    if(item.ExtraCharge != null)
                        PayExtraCharge += (decimal)item.ExtraCharge;
                    PayVAT += item.VAT;
                    GrandTotal = (PayAmount + PayExtraCharge + PayVAT).ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        public void SelectedList()
        {
            try
            {
                SelectedChequeList.Clear();
                foreach (var item in SelectedChequeNumbers)
                {
                    if (PendingAndBouncedPdcList.Any(x => x.ReceiptNo == item))
                    {
                        SelectedChequeList.Add(PendingAndBouncedPdcList.FirstOrDefault(x => x.ReceiptNo == item));
                    }
                }
                SelectedChequeListString = string.Empty;
                SelectedChequeListString = JsonConvert.SerializeObject(SelectedChequeList);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        public void BounceExpandCollapseClicked(BindablePdcDataView bindablePdcDataView)
        {
            try
            {
                if (bindablePdcDataView != null)
                {
                    foreach (var item in BindableBouncePdcList.ToList())
                    {
                        if (item != null)
                        {
                            if (item.ReceiptNo == bindablePdcDataView.ReceiptNo)
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
        public void PendingExpandCollapseClicked(BindablePdcDataView bindablePdcDataView)
        {
            try
            {
                if (bindablePdcDataView != null)
                {
                    foreach (var item in BindablePendingPdcList.ToList())
                    {
                        if (item != null)
                        {
                            if (item.ReceiptNo == bindablePdcDataView.ReceiptNo)
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
        public void HistoryExpandCollapseClicked(BindablePdcDataView bindablePdcDataView)
        {
            try
            {
                if (bindablePdcDataView != null)
                {
                    foreach (var item in PdcHistoryList.ToList())
                    {
                        if (item != null)
                        {
                            if (item.ReceiptNo == bindablePdcDataView.ReceiptNo)
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
        public void CheckboxClick(object obj)
        {
            try
            {
                var data = obj as BindablePdcDataView;
                if (data != null)
                {
                    var isChecked = false;
                    if (data.ChequeStatus.Contains("Pending"))
                    {
                        isChecked = !data.IsPendingCheckBoxEnabled;
                        var pendingList = PendingAndBouncedPdcList.Where(x => x.ChequeType.Equals("P", StringComparison.CurrentCultureIgnoreCase)).ToList();
                        if (SelectSequenceToPay(pendingList,data.ReceiptNo, isChecked))
                        {
                            data.IsPendingCheckBoxEnabled = !data.IsPendingCheckBoxEnabled;
                        }
                        else
                        {
                            //data.IsPendingCheckBoxEnabled = false;
                        }
                    }
                    else if (data.ChequeStatus.Contains("Bounced") && !data.IsBounceCheckBoxChecked)
                    {
                        var bouncedList = PendingAndBouncedPdcList.Where(x => x.ChequeType.Equals("B", StringComparison.CurrentCultureIgnoreCase)).ToList();
                        if (!SelectSequenceToPay(bouncedList, data.ReceiptNo, isChecked))
                            data.IsBounceCheckBoxChecked = true;
                    }
                    else if (!SelectedChequeNumbers.Contains(data.ReceiptNo))
                    {
                        SelectedChequeNumbers.Add(data.ReceiptNo);
                        StudentIdsList.Add(data.StudentId);
                        
                    }
                    PayNowAmountCalculation();
                    IsPayNowVisible = SelectedChequeNumbers.Count > 0;
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
                await GetDetails();
                base.GetStudentData();
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
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

    }