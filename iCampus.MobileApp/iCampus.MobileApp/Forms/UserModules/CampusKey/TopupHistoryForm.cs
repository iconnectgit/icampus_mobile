using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Enums;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.UserModules.OnlinePayment;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.CampusKey;

public class TopupHistoryForm : ViewModelBase
    {
        public ICommand ExpandCollapseClickCommand { get; set; }
        public ICommand DownloadCommand { get; set; }

        
        private IList<ReceiptDetailView> _receiptDetailList = new List<ReceiptDetailView>();
        
        
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
        public IList<ReceiptDetailView> ReceiptDetailList
        {
            get => _receiptDetailList;
            set
            {
                _receiptDetailList = value;
                OnPropertyChanged(nameof(ReceiptDetailList));
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
        
        ObservableCollection<BindableReceiptDetailView> _paymentHistoryList = new ObservableCollection<BindableReceiptDetailView>();
        public ObservableCollection<BindableReceiptDetailView> PaymentHistoryList
        {
            get => _paymentHistoryList;
            set
            {
                _paymentHistoryList = value;
                OnPropertyChanged(nameof(PaymentHistoryList));
            }
        }
        bool _isPaymentHistoryNoDataFoundVisibility = false;
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
        public TopupHistoryForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        private async void InitializePage()
        {
            DownloadCommand = new Command<BindableReceiptDetailView>(DownloadClicked);
            ExpandCollapseClickCommand = new Command<BindableReceiptDetailView>(ExpandCollapseClicked);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        }

        private async Task<IList<ReceiptDetailView>> GetTopupHistoryList()
        {
            try
            {
                var paymentModeID = Convert.ToInt32(PaymentTypes.CanteenTopup);
                string cacheKeyPrefix = "campuskeyhistory";
                var receiptDetailList = await ApiHelper.GetObjectList<ReceiptDetailView>(string.Format(TextResource.TopupHistoryApiUrl, AppSettings.Current.SelectedStudent.ItemId, paymentModeID), cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
                
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
                            item.TransactionDateTime = item.TransactionDate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    IsPaymentHistoryNoDataFoundVisibility = _paymentHistoryList is not { Count: > 0 };
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

        public override async void GetStudentData()
        {
            try
            {
                GetTopupHistoryList();
                base.GetStudentData();
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }  
        }     
    }