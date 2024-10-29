using System.ComponentModel;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.MiscellaneousPayment;

public class BindableInvoiceDetailsView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public long SaleId { get; set; }

        public int ItemId { get; set; }

        public string ItemDescription { get; set; }

        public int? Quantity { get; set; }

        public string Price { get; set; }

        public decimal Balance { get; set; }

        public int? NewFamilyId { get; set; }

        public int AcademicYear { get; set; }

        public string AcademicYearTitle { get; set; }

        public bool IsCurrentYear { get; set; }

        public decimal BalanceAmount { get; set; }

        public decimal MinimumAmount { get; set; }

        public decimal MinimumVatAmount { get; set; }

        public decimal TotalVATAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal GrandTotal => TotalAmount + TotalVATAmount;

        public List<OnlinePaymentInvoiceMainDetails> InvoiceDetailsList { get; set; }

        public List<OnlinePaymentInvoiceSubDetails> InvoiceSubDetailsList { get; set; }

        public IList<BindableProformaInvoiceDetails> ProformaList { get; set; }

        public PaymentSettingsView PaymentSettings { get; set; }

        public bool IsOrderIdConfirmed { get; set; }

        public bool IsOutstandingBalancePending => BalanceAmount > 0m;

        public bool IsOutstandingBalanceAboveMaxLimitPending => BalanceAmount > MaxAllowedOutstandingBalanceForReregistration;

        public bool IsOutstandingBalancePaymentOrderIdConfirmed { get; set; }

        public bool IsNextYearRegistrationAvailable { get; set; }

        public bool IsAllChildrenSelected { get; set; }

        public List<int> SelectedStudentIdList { get; set; }

        public int StudentId { get; set; }

        public string SchedulePaymentMessage { get; set; }

        public string FullPaymentMessage { get; set; }

        public string ReregisterStudentIds { get; set; }

        public bool AllowDirectRegistrationPayment { get; set; }

        public decimal MaxAllowedOutstandingBalanceForReregistration { get; set; }

        public bool IsPaymentFooterPanelVisibile => TotalAmount > 0m && (!IsOutstandingBalancePending || !IsOutstandingBalanceAboveMaxLimitPending || IsAllChildrenSelected || AllowDirectRegistrationPayment);

        
        public BindableInvoiceDetailsView()
        {
            ProformaList = new List<BindableProformaInvoiceDetails>();
            InvoiceDetailsList = new List<OnlinePaymentInvoiceMainDetails>();
            InvoiceSubDetailsList = new List<OnlinePaymentInvoiceSubDetails>();
            SelectedStudentIdList = new List<int>();
        }
    }

    public class BindableCurrencyView
    {
        public int SchoolICampusId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyBankNoteName { get; set; }
        public string CurrencyCoinName { get; set; }
        public int SubUnitMaxLimit { get; set; }
        public int? RoundingDigits { get; set; }
    }

    public class BindableProformaInvoiceDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public int ProFormaID { get; set; }

        public string StudentID { get; set; }

        public int FamilyID { get; set; }

        public string Description { get; set; }

        private decimal _amount;
        public decimal Amount
        {
            get
            {
                try
                {
                    return Decimal.Parse((_amount).ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            set
            {
                _amount = value;
            }
        }
        public decimal DrAmount { get; set; }

        public decimal CrAmount { get; set; }


        private decimal _vatAmount;
        public decimal VatAmount
        {
            get
            {
                try
                {
                    return Decimal.Parse((_vatAmount).ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            set
            {
                _vatAmount = value;
            }
        }

        public decimal VatPercentage { get; set; }

        public string FirstName_1 { get; set; }

        public string StudentName { get; set; }

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get
            {
                try
                {
                    return Decimal.Parse((_totalAmount).ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            set
            {
                _totalAmount = value;
            }
        }

        public bool IsPaid { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string OrderId { get; set; }

        public bool IsOrderConfirmed { get; set; }
        private string _arrowImageSource = "dropdown_gray.png";
        public string ArrowImageSource
        {
            get
            {
                return _arrowImageSource;
            }
            set
            {
                _arrowImageSource = value;
                OnPropertyChanged("ArrowImageSource");
            }
        }
        private bool _detailsVisibility;
        public bool DetailsVisibility
        {
            get
            {
                return _detailsVisibility;
            }
            set
            {
                _detailsVisibility = value;
                OnPropertyChanged("DetailsVisibility");
            }
        }
        private bool _isCheckBoxChecked;
        public bool IsCheckBoxChecked
        {
            get
            {
                return _isCheckBoxChecked;
            }
            set
            {
                _isCheckBoxChecked = value;
                OnPropertyChanged("IsCheckBoxChecked");
            }
        }
        private float _checkBoxOpacity;
        public float CheckBoxOpacity
        {
            get
            {
                return _checkBoxOpacity;
            }
            set
            {
                _checkBoxOpacity = value;
                OnPropertyChanged("CheckBoxOpacity");
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        private bool _isCheckBoxEnabled;
        public bool IsCheckBoxEnabled
        {
            get
            {
                return _isCheckBoxEnabled;
            }
            set
            {
                _isCheckBoxEnabled = value;
                OnPropertyChanged("IsCheckBoxEnabled");
            }
        }
        private bool _downloadIconVisibility;
        public bool DownloadIconVisibility
        {
            get
            {
                return _downloadIconVisibility;
            }
            set
            {
                _downloadIconVisibility = value;
                OnPropertyChanged("DownloadIconVisibility");

            }
        }
        private bool _isCheckBoxVisible = true;
        public bool IsCheckBoxVisible
        {
            get
            {
                return _isCheckBoxVisible;
            }
            set
            {
                _isCheckBoxVisible = value;
                OnPropertyChanged("IsCheckBoxVisible");
            }
        }
    }