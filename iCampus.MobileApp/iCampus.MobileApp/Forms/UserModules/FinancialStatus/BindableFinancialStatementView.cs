using System.ComponentModel;
using iCampus.Common.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.FinancialStatus;

public class BindableFinancialStatementView : INotifyPropertyChanged
{
    public BindableFinancialStatementView()
    {
        
    }
    public Guid UniqueID { get; private set; } = Guid.NewGuid();

    public DateTime DocumentDate
    {
        get;
        set;
    }

    public string FormattedFinancialStatementDate
    {
        get;
        set;
    }

    public int DocumentNumber
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public decimal DebitAmount
    {
        get;
        set;
    }

    public decimal CreditAmount
    {
        get;
        set;
    }

    public string DocumentType
    {
        get;
        set;
    }

    public IEnumerable<BindableFinancialStatementView> StatementList
    {
        get;
        set;
    }

    public CurrencyView CurrencyData
    {
        get;
        set;
    }

    public decimal BalanceAmount
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