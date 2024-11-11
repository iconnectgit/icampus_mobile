using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.CampusKey;

public class CampusKeyDetailsForm : ViewModelBase
    {
        #region Declarations
        TransactionDetails _campusKeyObject = new TransactionDetails();
        IEnumerable<CampusKeyInvoiceDetails> _invoiceDetailsObject = new List<CampusKeyInvoiceDetails>();
        public CampusKeyView _campusKeyViewModel = new CampusKeyView();
        private decimal _totalBalance;
        private int _totalQuantity;
        #endregion

        #region Properties
        bool _isInvoiceDetailsAvailable;
        public bool IsInvoiceDetailsAvailable
        {
            get => _isInvoiceDetailsAvailable;
            set
            {
                _isInvoiceDetailsAvailable = value;
                OnPropertyChanged(nameof(IsInvoiceDetailsAvailable));
            }
        }

        public TransactionDetails CampusKeyObject
        {
            get => _campusKeyObject;
            set
            {
                _campusKeyObject = value;
                OnPropertyChanged(nameof(CampusKeyObject));
            }
        }
        public IEnumerable<CampusKeyInvoiceDetails> InVoiceDetailsObject
        {
            get => _invoiceDetailsObject;
            set
            {
                _invoiceDetailsObject = value;
                OnPropertyChanged(nameof(InVoiceDetailsObject));
            }
        }
        public CampusKeyView CampusKeyViewModel
        {
            get => _campusKeyViewModel;
            set
            {
                _campusKeyViewModel = value;
                OnPropertyChanged(nameof(CampusKeyViewModel));
            }
        }
        public decimal TotalBalance
        {
            get => _totalBalance;
            set
            {
                _totalBalance = value;
                OnPropertyChanged(nameof(TotalBalance));
            }
        }
        public int TotalQuantity
        {
            get => _totalQuantity;
            set
            {
                _totalQuantity = value;
                OnPropertyChanged(nameof(TotalQuantity));
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
        #endregion
        public CampusKeyDetailsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
        }
        #region Methods

        public async Task<CampusKeyView> GetCampusKeyInvoiceDetails(int saleId)
        {
            try
            {
                string cacheKeyPrefix = "campuskey_invoice_" + saleId;
                CampusKeyViewModel = await ApiHelper.GetObject<CampusKeyView>(string.Format(TextResource.CampusKeyInvoiceDetailsApiUrl, saleId), cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
                IsInvoiceDetailsAvailable =  CampusKeyViewModel.InvoiceDetails.ToList().Count > 0 && !string.IsNullOrEmpty(CampusKeyViewModel.InvoiceDetails.FirstOrDefault().ItemDescription);
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
            }
            return _campusKeyViewModel;
        }
        #endregion
    }