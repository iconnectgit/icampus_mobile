using iCampus.Common.Enums;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

[Serializable]
    public class BindableOnlinePaymentFilterView
    {
        private List<OnlinePaymentInvoiceSubDetails> _InvoiceSubDetailList;

        private List<ProformaInvoiceDetails> _ProfomaInvoiceList;

        public string FormSelectorId
        {
            get;
            set;
        }

        public string MerchantName
        {
            get;
            set;
        }

        public string ModuleType
        {
            get;
            set;
        }

        public decimal VatAmount
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Street1
        {
            get;
            set;
        }

        public string Street2
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }

        public string Area
        {
            get;
            set;
        }

        public string PostCode
        {
            get;
            set;
        }

        public string Phone
        {
            get;
            set;
        }

        public string EmailId
        {
            get;
            set;
        }

        public string InvoiceSubDetailListString
        {
            get;
            set;
        }

        public string ProformaListString
        {
            get;
            set;
        }

        public int? Mode
        {
            get;
            set;
        }

        public decimal? MinimumPaymentPercentage
        {
            get;
            set;
        }

        public decimal MinimumVatAmount
        {
            get;
            set;
        }

        public string ConfirmationMessage
        {
            get;
            set;
        }

        public string TermsAndConditionMessage
        {
            get;
            set;
        }

        public string EmailSignature
        {
            get;
            set;
        }

        public int FamilyId
        {
            get;
            set;
        }

        public string FamilyAlternateId
        {
            get;
            set;
        }

        public int? StudentId
        {
            get;
            set;
        }

        public string StudentAlternateId
        {
            get;
            set;
        }

        public short GradeId
        {
            get;
            set;
        }

        public string StudentName
        {
            get;
            set;
        }

        public string ClassName
        {
            get;
            set;
        }

        public int AcademicYear
        {
            get;
            set;
        }

        public string OrderId
        {
            get;
            set;
        }

        public string ReferenceId
        {
            get;
            set;
        }

        public string ReceiptId
        {
            get;
            set;
        }

        public DateTime TransactionDate
        {
            get;
            set;
        }

        public decimal Amount
        {
            get;
            set;
        }

        public string SessionId
        {
            get;
            set;
        }

        public string ResultIndicator
        {
            get;
            set;
        }

        public string InvoiceIds
        {
            get;
            set;
        }

        public string Currency
        {
            get;
            set;
        }

        public string PaymentModeType
        {
            get;
            set;
        }

        public bool IsOutstandingBalancePayment
        {
            get;
            set;
        }

        public PaymentModes PaymentMode
        {
            get;
            set;
        }

        public PaymentControllerEnum PaymentController
        {
            get;
            set;
        }

        public List<OnlinePaymentInvoiceSubDetails> InvoiceSubDetailList
        {
            get;
            set;
        }

        public bool IsMinimumPayment
        {
            get;
            set;
        }

        public decimal MinimumAmount
        {
            get;
            set;
        }

        public decimal OutstandingBalanceAmount
        {
            get;
            set;
        }

        public List<ProformaInvoiceDetails> ProfomaInvoiceList
        {
            get;
            set;
        }

        public string PaymentTypeDescription
        {
            get;
        }
        public string PortalPaymentPageUrl
        {
            get;
            set;
        }
        public string ReasonForPayment
        {
            get;
            set;
        }
        public string CardNumberEnc
        {
            get;
            set;
        }
        
        public string PendingAndBouncedPdcListString
        {
            get;
            set;
        }
    }