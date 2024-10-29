using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.ChequeReplacement;

public class BindablePaymentSettingsView : INotifyPropertyChanged
    {
        public string PortalTitle { get; set; }
        public string GeneralEmailId { get; set; }
        public string EmailSignature { get; set; }
        public decimal? MinimumPaymentPercentage { get; set; }
        public decimal? MinimumFixedAmount { get; set; }
        public bool IsMinimumPaymentEnabled { get; set; }
        public string PaymentDisagreeMessage { get; set; }
        public string TermsAndConditionMessage { get; set; }
        public string ContactSchoolMessage { get; set; }
        public string MinimumPaymentMessage { get; set; }
        public string DNRStudentsMessage { get; set; }
        public string FamilyOutstandingBalanceMessage { get; set; }
        public BindablePaymentTypes PaymentType { get; set; }
        public string FrontMessage { get; set; }
        public string ApplicationModeMessage { get; set; }
        public string RegistrationModeMessage { get; set; }
        public string PaymentAttemptMessage { get; set; }
        public string BlockContent { get; set; }
        public string NoInvoiceMessage { get; set; }
        public string CompleteMessage { get; set; }
        public int PaymentId { get; set; }
        public string PaymentModule { get; set; }
        public string DescriptionMessage { get; set; }
        public string OverDueMessage { get; set; }
        public string PaidSchoolMessage { get; set; }
        public BindableCurrencyView CurrencyData { get; set; }
        public string PrintMessage { get; set; }
        public string ConfirmationMessage { get; set; }
        public string PrintLogoUrl { get; set; }
        public string PrintReceiptMessage { get; set; }
        public string PaidMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string FailureMessage { get; set; }
        public string CancelMessage { get; set; }
        public string PrintLogo { get; set; }
        public int FinancialYear { get; set; }

        //public bool AllowMinimumPayment();
        //public bool AllowMinimumPaymentFixedAmount();
        //public bool AllowMinimumPaymentPercentage();
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum BindablePaymentTypes
    {
        OnlineFees = 1,
        Admission = 2,
        RegistrationPayment = 3,
        MiscellaneousPayment = 4,
        ReRegistrationPayment = 5,
        PdcPayment = 6,
        CanteenTopup = 7,
        QuickPayment = 8
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