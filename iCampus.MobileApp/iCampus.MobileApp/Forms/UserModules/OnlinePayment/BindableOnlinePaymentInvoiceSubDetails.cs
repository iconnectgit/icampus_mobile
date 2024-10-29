using System.ComponentModel;
using iCampus.Common.Helpers.Extensions;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class BindableOnlinePaymentInvoiceSubDetails : INotifyPropertyChanged
{
    public BindableOnlinePaymentInvoiceSubDetails()
    {
    }

    #region Properties

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public string GroupType { get; set; }

    public string CostCategoryGroupName { get; set; }

    public int CostCategoryGroupID { get; set; }

    private decimal _total;

    public decimal Total
    {
        get => decimal.Parse(_total.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
        set => _total = value;
    }

    public decimal Paid { get; set; }

    public int Quantity { get; set; }

    public decimal VAT { get; set; }

    public decimal PayableAmount { get; set; }

    public decimal DeductableAmount { get; set; }
    private string _totalString { get; set; }

    public string TotalString
    {
        get => _totalString;
        set => _totalString = value;
    }

    private FontAttributes _fontAttributes { get; set; }

    public FontAttributes FontAttributes
    {
        get => _fontAttributes;
        set => _fontAttributes = value;
    }

    private int _index { get; set; }

    public int Index
    {
        get => _index;
        set => _index = value;
    }

    #endregion
}