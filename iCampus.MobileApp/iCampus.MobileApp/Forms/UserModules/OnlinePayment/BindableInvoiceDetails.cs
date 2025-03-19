using System.ComponentModel;
using AutoMapper;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;
using Splat;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class BindableInvoiceDetails: INotifyPropertyChanged
    {
	    private IMapper _mapper;
      
        public event PropertyChangedEventHandler PropertyChanged;
        public BindableInvoiceDetails()
        {
	        _mapper = Locator.Current.GetService<IMapper>();
        }
		#region Properties
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		public long SaleId
		{
			get;
			set;
		}

		public int ItemId
		{
			get;
			set;
		}

		public string ItemDescription
		{
			get;
			set;
		}

		public int? Quantity
		{
			get;
			set;
		}

		public string Price
		{
			get;
			set;
		}

		public decimal Balance
		{
			get;
			set;
		}

		public int? NewFamilyId
		{
			get;
			set;
		}

		public int AcademicYear
		{
			get;
			set;
		}

		public string AcademicYearTitle
		{
			get;
			set;
		}

		public bool IsCurrentYear
		{
			get;
			set;
		}

		public decimal BalanceAmount
		{
			get;
			set;
		}

		

		public decimal TotalVATAmount
		{
			get;
			set;
		}
		public decimal MinimumVatAmount
		{
			get;
			set;
		}

		
		public decimal GrandTotal
		{
			get;
			set;
		}

		public List<OnlinePaymentInvoiceMainDetails> InvoiceDetailsList
		{
			get;
			set;
		}

		public List<OnlinePaymentInvoiceSubDetails> InvoiceSubDetailsList
		{
			get;
			set;
		}
		private List<BindableOnlinePaymentInvoiceSubDetails> _bindableInvoiceSubDetailsList { get; set; }
		public List<BindableOnlinePaymentInvoiceSubDetails> BindableInvoiceSubDetailsList
		{
			get
			{
				_bindableInvoiceSubDetailsList =  _mapper.Map<List<BindableOnlinePaymentInvoiceSubDetails>>(InvoiceSubDetailsList);
    
                foreach (var item in _bindableInvoiceSubDetailsList)
                {
					if(item!=null)
                    {
						item.TotalString = item.Total.ToString();
                    }
                }
				_bindableInvoiceSubDetailsList.Insert(0, new BindableOnlinePaymentInvoiceSubDetails() { CostCategoryGroupName = "Category Name", TotalString = String.Concat("Total Amount"," ","(",AppSettings.Current.OnlinePaymentCurrencyCode,")"), GroupType = "NET" });
				foreach (var item in _bindableInvoiceSubDetailsList)
				{
					if (item != null)
					{
						if (item.GroupType.ToLower().Equals("net"))
						{
							item.FontAttributes = Microsoft.Maui.Controls.FontAttributes.Bold;
						}
					}
				}
				return _bindableInvoiceSubDetailsList;
			}
			set
			{
				_bindableInvoiceSubDetailsList = value;
			}
		}

		public IList<ProformaInvoiceDetails> ProformaList
		{
			get;
			set;
		}

		public PaymentSettingsView PaymentSettings
		{
			get;
			set;
		}

		public bool IsOrderIdConfirmed
		{
			get;
			set;
		}

		public bool IsOutstandingBalancePaymentOrderIdConfirmed
		{
			get;
			set;
		}

		public bool IsNextYearRegistrationAvailable
		{
			get;
			set;
		}
		private bool _isFrameVisible { get; set; }
		public bool IsFrameVisible
		{
			get
			{
				_isFrameVisible = (InvoiceSubDetailsList != null && InvoiceSubDetailsList.Count > 0) ? true : false;
				return _isFrameVisible;
			}
			set
			{
				_isFrameVisible = value;
			}
		}
        private string _totalAmountRadioButtonImage = "unselected_radio_button.png";
        public string TotalAmountRadioButtonImage
        {
			get
			{
				return _totalAmountRadioButtonImage;
			}
			set
			{
				_totalAmountRadioButtonImage = value;
                OnPropertyChanged("TotalAmountRadioButtonImage");
            }
        }
        private string _minimumAmountRadioButtonImage = "selected_radio_button.png";
        public string MinimumAmountRadioButtonImage
        {
            get
            {
                return _minimumAmountRadioButtonImage;
            }
            set
            {
                _minimumAmountRadioButtonImage = value;
                OnPropertyChanged("MinimumAmountRadioButtonImage");
            }
        }
        private string _otherAmountRadioButtonImage = "unselected_radio_button.png";
        public string OtherAmountRadioButtonImage
        {
            get
            {
                return _otherAmountRadioButtonImage;
            }
            set
            {
                _otherAmountRadioButtonImage = value;
                OnPropertyChanged("OtherAmountRadioButtonImage");
            }
        }
        private string _headerText;
        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            set
            {
                _headerText = value;
                OnPropertyChanged("HeaderText");
            }
        }
        private bool _vatAndGrandTotalVisibility;
        public bool VatAndGrandTotalVisibility
        {
            get
			{
                return _vatAndGrandTotalVisibility;
            }
            set
			{
				_vatAndGrandTotalVisibility = value;
                OnPropertyChanged("VatAndGrandTotalVisibility");
            }
        }
        private bool _isPaymentTypeSelectionVisible;
        public bool IsPaymentTypeSelectionVisible
        {
            get
            {
                return _isPaymentTypeSelectionVisible;
            }
            set
            {
                _isPaymentTypeSelectionVisible = value;
                OnPropertyChanged("IsPaymentTypeSelectionVisible");
            }
        }
        private bool _isPayNowVisible;
        public bool IsPayNowVisible
        {
            get
            {
                return _isPayNowVisible;
            }
            set
            {
                _isPayNowVisible = value;
                OnPropertyChanged("IsPayNowVisible");
            }
        }
        private bool _isRemainingPaymentDueVisible;
        public bool IsRemainingPaymentDueVisible
        {
            get
            {
                return _isRemainingPaymentDueVisible;
            }
            set
            {
                _isRemainingPaymentDueVisible = value;
                OnPropertyChanged("IsRemainingPaymentDueVisible");
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
                OnPropertyChanged("IsTotalVisible");
            }
        }
        private decimal _minimumAmountPercentage;
        public decimal MinimumAmountPercentage
        {
            get
            {
                return _minimumAmountPercentage;
            }
            set
            {
                _minimumAmountPercentage = value;
                OnPropertyChanged("MinimumAmountPercentage");
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
                OnPropertyChanged("IsMinimumFixedAmountVisible");
            }
        }
        private decimal _payAmount;
        public decimal PayAmount
        {
            get
            {
                return _payAmount;
            }
            set
            {
                _payAmount = value;
                OnPropertyChanged("PayAmount");
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
                OnPropertyChanged("MinimumFixedAmount");
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
                _isMinimumAmountOptionVisible = value;
                OnPropertyChanged("IsMinimumAmountOptionVisible");
            }
        }
        private decimal _remainingPaymentDue;
        public decimal RemainingPaymentDue
        {
            get
            {
                return _remainingPaymentDue;
            }
            set
            {
                _remainingPaymentDue = value;
                OnPropertyChanged("RemainingPaymentDue");
            }
        }
        private string _minimumPaymentDetailsToPaymentUrl;
        public string MinimumPaymentDetailsToPaymentUrl
        {
            get
            {
                return _minimumPaymentDetailsToPaymentUrl;
            }
            set
            {
                _minimumPaymentDetailsToPaymentUrl = value;
                OnPropertyChanged("MinimumPaymentDetailsToPaymentUrl");
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
		            _otherAmountEntered = input;
	            }
	            else
	            {
		            _otherAmountEntered = 0; // Reset value to 0 if input is invalid
	            }
	            OnPropertyChanged(nameof(OtherAmountEntered)); // Correct property name
            }
        }
        private bool _isOtherAmountEntryEnabled;
        public bool IsOtherAmountEntryEnabled
        {
            get
            {
                return _isOtherAmountEntryEnabled;
            }
            set
            {
                _isOtherAmountEntryEnabled = value;
                OnPropertyChanged("IsOtherAmountEntryEnabled");
            }
        }
        private string _otherAmountErrorMessage;
        public string OtherAmountErrorMessage
        {
            get
            {
                return _otherAmountErrorMessage;
            }
            set
            {
                _otherAmountErrorMessage = value;
                OnPropertyChanged("OtherAmountErrorMessage");
            }
        }
        private bool _isOtherAmountErrorMessaheVisible;
        public bool IsOtherAmountErrorMessaheVisible
        {
            get
            {
                return _isOtherAmountErrorMessaheVisible;
            }
            set
            {
                _isOtherAmountErrorMessaheVisible = value;
                OnPropertyChanged("IsOtherAmountErrorMessaheVisible");
            }
        }
        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get
            {
                if (_totalAmount != 0)
                {
                    _totalAmount = Decimal.Parse(_totalAmount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                }
                if (IsPaymentTypeSelectionVisible)
                {
                    IsTotalVisible = false;
                }
                else
                {
                    IsTotalVisible = (_totalAmount != 0) ? true : false;
                }
                return _totalAmount;
            }
            set
            {
                _totalAmount = value;
                OnPropertyChanged("TotalAmount");
            }
        }
        private decimal _minimumAmount = 0;
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
                OnPropertyChanged("MinimumAmount");
            }
        }
        #endregion
    }