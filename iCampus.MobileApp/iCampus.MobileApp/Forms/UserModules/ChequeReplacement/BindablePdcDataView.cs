using System.ComponentModel;
using iCampus.Common.Helpers.Extensions;

namespace iCampus.MobileApp.Forms.UserModules.ChequeReplacement;

public class BindablePdcDataView : INotifyPropertyChanged
    {
        public decimal ExtraChargesVatPercentage 
        { 
            get; 
            set; 
        }
        private decimal _vAT;
        public decimal VAT 
        {
            get
            {
                try
                {
                    return Decimal.Parse((_vAT).ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            set
            {
                _vAT = value;
            }
        }

        public bool IsCurrent 
        { 
            get; 
            set; 
        }
        public int FinancialBeginYear 
        { 
            get; 
            set; 
        }
        public string FinancialYearTitle 
        { 
            get; 
            set; 
        }
        public bool IsSynchronizedWithAccountsDb 
        { 
            get; 
            set; 
        }
        public string SyncStatus 
        { 
            get; 
            set; 
        }
        public string BankName 
        { 
            get; 
            set; 
        }
        public string Remarks 
        { 
            get; 
            set; 
        }
        public int ChequeTypeId 
        { 
            get; 
        }
        public decimal? ExtraCharge 
        { 
            get; 
            set; 
        }
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
        public string ReceiptNo 
        { 
            get; 
            set; 
        }
        public string ChequeStatus 
        { 
            get; 
            set; 
        }
        public string ChequeNo 
        { 
            get; 
            set; 
        }
        public DateTime DueDate 
        { 
            get; 
            set; 
        }
        public string ChequeType 
        { 
            get; 
            set; 
        }
        public int StudentId 
        { 
            get; 
            set; 
        }
        public decimal ExtraChargesIncludingVAT 
        { 
            get; 
        }

        //public bool IsCheckboxEnabled(bool isBouncedPdc, bool isBouncedPdcDataExist, bool isDataExistForBothYears);

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
        private bool _isBounceCheckBoxEnabled;
        public bool IsBounceCheckBoxEnabled
        {
            get
            {
                return _isBounceCheckBoxEnabled;
            }
            set
            {
                _isBounceCheckBoxEnabled = value;
                OnPropertyChanged("IsBounceCheckBoxEnabled");
            }
        }
        private bool _isPendingCheckBoxEnabled;
        public bool IsPendingCheckBoxEnabled
        {
            get
            {
                return _isPendingCheckBoxEnabled;
            }
            set
            {
                _isPendingCheckBoxEnabled = value;
                OnPropertyChanged("IsPendingCheckBoxEnabled");
            }
        }
        private bool _isPendingCheckBoxVisible;
        public bool IsPendingCheckBoxVisible
        {
            get
            {
                return _isPendingCheckBoxVisible;
            }
            set
            {
                _isPendingCheckBoxVisible = value;
                OnPropertyChanged("IsPendingCheckBoxVisible");
            }
        }
        private bool _isBouncedCheckBoxVisible;
        public bool IsBouncedCheckBoxVisible
        {
            get
            {
                return _isBouncedCheckBoxVisible;
            }
            set
            {
                _isBouncedCheckBoxVisible = value;
                OnPropertyChanged("IsBouncedCheckBoxVisible");
            }
        }
        private bool _isBounceCheckBoxChecked;
        public bool IsBounceCheckBoxChecked
        {
            get
            {
                return _isBounceCheckBoxChecked;
            }
            set
            {
                _isBounceCheckBoxChecked = value;
                OnPropertyChanged("IsBouncedCheckBoxVisible");
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
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }