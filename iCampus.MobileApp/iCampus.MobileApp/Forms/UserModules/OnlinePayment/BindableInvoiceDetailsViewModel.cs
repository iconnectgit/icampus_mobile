using iCampus.Common.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class BindableInvoiceDetailsViewModel
{
    public BindableInvoiceDetailsViewModel()
    {
    }
    public bool IsOrderIdConfirmed
    {
        get;
        set;
    }

    public string JavascriptSessionUrl
    {
        get;
        set;
    }

    public List<BindableInvoiceDetails> DetailList
    {
        get;
        set;
    }

    public PaymentSettingsView PaymentSettings
    {
        get;
        set;
    }
    public IEnumerable<BindableReceiptDetailView> PaymentHistory { get; set; }
    public CurrencyView CurrencyData { get; set; }
}