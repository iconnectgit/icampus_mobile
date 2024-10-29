using iCampus.Common.Enums;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class BindablePaymentSettingsView
{
    public BindablePaymentSettingsView()
    {
    }

    public int PaymentId { get; set; }

    public string PaymentModule { get; set; }

    public string DescriptionMessage { get; set; }

    public string OverDueMessage { get; set; }

    public string PaidSchoolMessage { get; set; }

    public string ConfirmationMessage { get; set; }

    public string PrintMessage { get; set; }

    public string PrintLogo { get; set; }

    public string PrintReceiptMessage { get; set; }

    public string PaidMessage { get; set; }

    public string SuccessMessage { get; set; }

    public string FailureMessage { get; set; }

    public string CancelMessage { get; set; }

    public string CompleteMessage { get; set; }

    public string NoInvoiceMessage { get; set; }

    public string BlockContent { get; set; }

    public string TermsAndConditionMessage { get; set; }

    public string PortalTitle { get; set; }

    public string GeneralEmailId { get; set; }

    public string EmailSignature { get; set; }

    public decimal? MinimumPaymentPercentage { get; set; }

    public bool IsMinimumPaymentEnabled { get; set; }

    public string PaymentDisagreeMessage { get; set; }

    public string ContactSchoolMessage { get; set; }

    public string PaymentAttemptMessage { get; set; }

    public string MinimumPaymentMessage { get; set; }

    public string DNRStudentsMessage { get; set; }

    public string FamilyOutstandingBalanceMessage { get; set; }

    public PaymentTypes PaymentType { get; set; }
}