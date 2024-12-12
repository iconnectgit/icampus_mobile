using System.Windows.Input;
using iCampus.MobileApp.DependencyService;
using Splat;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class TaxReceiptForm : ViewModelBase
{
    #region Declarations


    #endregion

    #region Properties

    private BindableReceiptDetailView _selectedPaymentHistory = new();

    public BindableReceiptDetailView SelectedPaymentHistory
    {
        get => _selectedPaymentHistory;
        set
        {
            _selectedPaymentHistory = value;
            OnPropertyChanged(nameof(Vat));
            IsVATShow = SelectedPaymentHistory.TotalVAT > 0;
            IsTotalAmountShow = SelectedPaymentHistory.ListAmount > 0;
            IsAdvFeeShow = SelectedPaymentHistory.AdvanceFee > 0;
        }
    }

    private decimal _vat;

    public decimal Vat
    {
        get => _vat;
        set
        {
            _vat = value;
            OnPropertyChanged(nameof(Vat));
        }
    }

    private string _parentName;

    public string ParentName
    {
        get => _parentName;
        set
        {
            _parentName = value;
            OnPropertyChanged(nameof(ParentName));
        }
    }

    private ImageSource _screenshot;

    public ImageSource Screenshot
    {
        get => _screenshot;
        set
        {
            _screenshot = value;
            OnPropertyChanged(nameof(Screenshot));
        }
    }

    private bool _isVATShow;

    public bool IsVATShow
    {
        get => _isVATShow;
        set
        {
            _isVATShow = value;
            OnPropertyChanged(nameof(IsVATShow));
        }
    }

    private bool _isTotalAmountShow;

    public bool IsTotalAmountShow
    {
        get => _isTotalAmountShow;
        set
        {
            _isTotalAmountShow = value;
            OnPropertyChanged(nameof(IsTotalAmountShow));
        }
    }

    private bool _isAdvFeeShow;

    public bool IsAdvFeeShow
    {
        get => _isAdvFeeShow;
        set
        {
            _isAdvFeeShow = value;
            OnPropertyChanged(nameof(IsAdvFeeShow));
        }
    }

    #endregion

    public TaxReceiptForm(INativeServices nativeServices, INavigation navigation) : base(null, null, null)
    {
        _nativeServices = nativeServices;
        Navigation = navigation;
        InitializePage();
    }

    #region Methods

    
    
    private async Task InitializePage()
    {
        MessagingCenter.Subscribe<string>("", "ScrollViewRightSwipeTaxReceiptSubscribe", async (arg) =>
        {
            MessagingCenter.Subscribe<string>("", "ScrollViewRightSwipeTaxReceipt", async (ar) =>
            {
                // await SideMenuClicked();
            });
        });
    }

    #endregion
}