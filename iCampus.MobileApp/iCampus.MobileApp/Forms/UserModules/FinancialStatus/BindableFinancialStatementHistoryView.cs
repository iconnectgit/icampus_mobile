using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.FinancialStatus;

public class BindableFinancialStatementHistoryView : INotifyPropertyChanged
{
    public Guid UniqueID { get; private set; } = Guid.NewGuid();

    public int ReceiptId
    {
        get;
        set;
    }

    public DateTime ReceiptDate
    {
        get;
        set;
    }

    public string FormattedReceiptDate
    {
        get;
        set;
    }

    public string ChequeDate
    {
        get;
        set;
    }

    public string ChequeNumber
    {
        get;
        set;
    }

    public int FamilyId
    {
        get;
        set;
    }

    public string BankName
    {
        get;
        set;
    }

    public string Payer
    {
        get;
        set;
    }

    public int AcademicYear
    {
        get;
        set;
    }

    public string PaymentMethod
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public string DebitAmount
    {
        get;
        set;
    }

    public string Amount
    {
        get;
        set;
    }

    public string InventoryStatusCode
    {
        get;
        set;
    }

    public string InventoryStatus
    {
        get;
        set;
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

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged == null)
            return;

        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}