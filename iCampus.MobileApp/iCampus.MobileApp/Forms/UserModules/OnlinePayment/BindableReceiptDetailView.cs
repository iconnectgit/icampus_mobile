using System.ComponentModel;
using iCampus.Common.Enums;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class BindableReceiptDetailView : INotifyPropertyChanged
{
    public BindableReceiptDetailView()
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

    public int FamilyId { get; set; }

    public string StudentId { get; set; }

    public string StudentName { get; set; }

    public string GradeName { get; set; }

    public string ClassName { get; set; }

    public string ReceiptId { get; set; }

    public string OrderId { get; set; }

    private decimal _amount;

    public decimal Amount
    {
        get
        {
            try
            {
                return decimal.Parse(
                    _amount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        set => _amount = value;
    }

    private decimal _advanceFee;

    public decimal AdvanceFee
    {
        get
        {
            try
            {
                return decimal.Parse(
                    _advanceFee.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        set => _advanceFee = value;
    }

    private decimal _listAmount;

    public decimal ListAmount
    {
        get
        {
            try
            {
                return decimal.Parse(
                    _listAmount.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        set => _listAmount = value;
    }

    public decimal? ExtraAmount { get; set; }


    public string Currency { get; set; }

    public int RoundingDigits { get; set; }

    public DateTime TransactionDate { get; set; }

    public string FormattedTransactionDate { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public PaymentSettingsView PaymentSettings { get; set; }

    public bool IsBalanceAmountPayment { get; set; }

    public string PaymentStatusCode { get; set; }

    public PaymentTypes PaymentModule { get; set; }

    public List<OnlinePaymentInvoiceSubDetails> InvoiceSubDetailList { get; set; }
    public string SynchronizationError { get; set; }
    private bool _detailsVisibility;

    public bool DetailsVisibility
    {
        get => _detailsVisibility;
        set
        {
            _detailsVisibility = value;
            OnPropertyChanged("DetailsVisibility");
        }
    }

    private string _arrowImageSource = "dropdown_gray.png";

    public string ArrowImageSource
    {
        get => _arrowImageSource;
        set
        {
            _arrowImageSource = value;
            OnPropertyChanged("ArrowImageSource");
        }
    }


    private List<OnlinePaymentInvoiceSubDetails> _bindableInvoiceSubDetailListFirstIndex = new();
    private List<BindableOnlinePaymentInvoiceSubDetails> _bindableInvoiceSubDetailList = new();

    public List<BindableOnlinePaymentInvoiceSubDetails> BindableInvoiceSubDetailList
    {
        get
        {
            //string clientGroupCode = (!string.IsNullOrEmpty(App.clientGroupCode)) ? App.clientGroupCode : string.Empty;

            if (PaymentModule == PaymentTypes.Admission || PaymentModule == PaymentTypes.ReRegistrationPayment)
            {
                _bindableInvoiceSubDetailList.Add(new BindableOnlinePaymentInvoiceSubDetails()
                    { CostCategoryGroupName = "Amount paid from mobile application", Total = Amount, Index = 1 });
                AdmissionAndReRegistrationListVisibility = true;
                OtherListVisibility = false;
                if (_bindableInvoiceSubDetailList != null)
                    ListViewHeight = (_bindableInvoiceSubDetailList.Count + 1) * 60;
            }
            else
            {
                AdmissionAndReRegistrationListVisibility = false;
                OtherListVisibility = true;
                if (PaymentInvoiceList != null) ListViewHeight = PaymentInvoiceList.Count * 100;
            }
            //if (InvoiceSubDetailList != null && InvoiceSubDetailList.Count > 0)
            //{
            //    _bindableInvoiceSubDetailList = new List<BindableOnlinePaymentInvoiceSubDetails>();

            //    if (PaymentModule == PaymentTypes.Admission || PaymentModule == PaymentTypes.ReRegistrationPayment)
            //    {
            //        _bindableInvoiceSubDetailList.Add(new BindableOnlinePaymentInvoiceSubDetails() { CostCategoryGroupName = "Amount paid from mobile application", Total = Amount });
            //    }
            //    else
            //    {
            //        _bindableInvoiceSubDetailList = Mapper.Map<List<BindableOnlinePaymentInvoiceSubDetails>>(InvoiceSubDetailList);
            //    }
            //    int index = 0;
            //    foreach (var item in _bindableInvoiceSubDetailList)
            //    {
            //        if (item != null)
            //        {
            //            item.Index = index + 1;
            //            index = index + 1;
            //        }
            //    }
            //    if (_bindableInvoiceSubDetailList != null)
            //    {
            //        ListViewHeight = (_bindableInvoiceSubDetailList.Count + 1) * 60;
            //    }
            //}

            return _bindableInvoiceSubDetailList;
        }
        set => _bindableInvoiceSubDetailList = value;
    }

    public bool IsNextYearPayment { get; set; }

    public int PaymentModeId { get; set; }
    private string _paymentFullStatus { get; set; }

    public string PaymentFullStatus
    {
        get
        {
            if (PaymentStatusCode.Equals("S"))
                _paymentFullStatus = "Success";
            else if (PaymentStatusCode.Equals("F"))
                _paymentFullStatus = "Failed";
            else if (PaymentStatusCode.Equals("C"))
                _paymentFullStatus = "Cancelled";
            else if (PaymentStatusCode.Equals("P")) _paymentFullStatus = "Pending";
            return _paymentFullStatus;
        }
        set => _paymentFullStatus = value;
    }

    private bool _downloadIconVisibility { get; set; }

    public bool DownloadIconVisibility
    {
        get
        {
            _downloadIconVisibility = PaymentStatusCode.Equals("S") ? true : false;
            return _downloadIconVisibility;
        }
        set => _downloadIconVisibility = value;
    }

    private decimal _vat;

    public decimal Vat
    {
        get
        {
            if (InvoiceSubDetailList != null && InvoiceSubDetailList.Count > 0)
                return InvoiceSubDetailList.FirstOrDefault().VAT;
            return _vat;
        }
        set => _vat = value;
    }

    private string _printLogo;

    public string PrintLogo
    {
        get => _printLogo;
        set => _printLogo = value;
    }

    private string _transactionDateTime;

    public string TransactionDateTime
    {
        get => _transactionDateTime;
        set => _transactionDateTime = value;
    }

    private string _taxReceiptTransactionDate;

    public string TaxReceiptTransactionDate
    {
        get
        {
            if (TransactionDate != null) _taxReceiptTransactionDate = TransactionDate.ToString("dd MMM yyyy");
            return _taxReceiptTransactionDate;
        }
        set => _taxReceiptTransactionDate = value;
    }

    private string _amountInWords;

    public string AmountInWords
    {
        get =>
            //return ConvertAmount(double.Parse(Amount.ToString()));
            NumberToWordsExtensions.ConvertAmount(AppSettings.Current.OnlinePaymentCurrencyCode, Amount.ToDouble());
        set => _amountInWords = value;
    }

    private int _listViewHeight;

    public int ListViewHeight
    {
        get => _listViewHeight;
        set => _listViewHeight = value;
    }

    private decimal _totalVat;

    public decimal TotalVAT
    {
        get
        {
            try
            {
                return decimal.Parse(
                    _totalVat.ToFormattedAmount(AppSettings.Current.OnlinePaymentCurrencyRoundingDigits));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        set => _totalVat = value;
    }

    public List<BindablePaymentInvoiceList> PaymentInvoiceList { get; set; }
    private bool _admissionAndReRegistrationListVisibility;

    public bool AdmissionAndReRegistrationListVisibility
    {
        get => _admissionAndReRegistrationListVisibility;
        set => _admissionAndReRegistrationListVisibility = value;
    }

    private bool _otherListVisibility;

    public bool OtherListVisibility
    {
        get => _otherListVisibility;
        set => _otherListVisibility = value;
    }

    //decimal _grandTotal;
    //public decimal GrandTotal
    //{
    //	get
    //	{
    //		if (TotalVAT > 0)
    //		{
    //			_grandTotal = Amount + TotalVAT;
    //		}
    //		return _grandTotal;
    //	}
    //	set
    //	{
    //		_grandTotal = value;
    //	}
    //}

    #endregion Properties


    #region AmountInWordsConversion

    private static string[] units =
    {
        "Zero", "One", "Two", "Three",
        "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
        "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
        "Seventeen", "Eighteen", "Nineteen"
    };

    private static string[] tens =
    {
        "", "", "Twenty", "Thirty", "Forty",
        "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
    };

    public static string ConvertAmount(double amount)
    {
        try
        {
            var amount_int = (long)amount;
            var amount_dec = (long)Math.Round((amount - (double)amount_int) * 100);
            if (amount_dec == 0)
                return Convert(amount_int) + " Only.";
            else
                return Convert(amount_int) + " Point " + Convert(amount_dec) + " Only.";
        }
        catch (Exception e)
        {
            // TODO: handle exception  
        }

        return "";
    }

    public static string Convert(long i)
    {
        if (i < 20) return units[i];
        if (i < 100) return tens[i / 10] + (i % 10 > 0 ? " " + Convert(i % 10) : "");
        if (i < 1000)
            return units[i / 100] + " Hundred"
                                  + (i % 100 > 0 ? " And " + Convert(i % 100) : "");
        if (i < 100000)
            return Convert(i / 1000) + " Thousand "
                                     + (i % 1000 > 0 ? " " + Convert(i % 1000) : "");
        if (i < 10000000)
            return Convert(i / 100000) + " Lakh "
                                       + (i % 100000 > 0 ? " " + Convert(i % 100000) : "");
        if (i < 1000000000)
            return Convert(i / 10000000) + " Crore "
                                         + (i % 10000000 > 0 ? " " + Convert(i % 10000000) : "");
        return Convert(i / 1000000000) + " Arab "
                                       + (i % 1000000000 > 0 ? " " + Convert(i % 1000000000) : "");
    }
}

#endregion

public class BindablePaymentInvoiceList
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public string GradeName { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; }
    public decimal Amount { get; set; }
    public double VAT { get; set; }
    public double VatPercentage { get; set; }
    public double AmountWithVat { get; set; }
    public double TotalAmount { get; set; }
    public double TotalVAT { get; set; }
    private string _studentNameAndGrade;

    public string StudentNameAndGrade
    {
        get
        {
            _studentNameAndGrade = string.Concat(StudentName, " ", "[", GradeName, "]");
            return _studentNameAndGrade;
        }
        set => _studentNameAndGrade = value;
    }
}