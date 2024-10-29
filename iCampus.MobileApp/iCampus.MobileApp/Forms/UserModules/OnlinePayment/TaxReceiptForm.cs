using System.Windows.Input;
using iCampus.MobileApp.DependencyService;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class TaxReceiptForm : ViewModelBase
{
    #region Declarations

    public ICommand PrintCommand { get; set; }

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

    private async void PrintClicked()
    {
        try
        {
            // Already commented

            //var screenshotBytes = Xamarin.Forms.DependencyService.Get<IScreenshotManager>().CaptureAsync();
            //string directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            //string file = Path.Combine(directory, "temp.pdf");
            //System.IO.File.WriteAllBytes(file, screenshotBytes);

            //Image image = new Image();
            //Stream stream = new MemoryStream(screenshotBytes);
            //Screenshot = ImageSource.FromStream(() => { return stream; });

            //string filepath = Xamarin.Forms.DependencyService.Get<ISaveFiles>().SaveFiles("kpokale", screenshotBytes);

            //string myFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "myFile.pdf");
            //System.IO.File.WriteAllBytes(myFilePath, screenshotBytes);
            //PdfDocument document = new PdfDocument();
            //MemoryStream stream = new MemoryStream();

            //PdfPage page = document.Pages.Add();

            //MemoryStream imageStream = new MemoryStream(screenshotBytes);
            //PdfGraphics graphics = page.Graphics;


            ////Load the image
            //PdfBitmap image = new PdfBitmap(imageStream);


            ////Draw the image
            //graphics.DrawImage(image, 0, 0, page.GetClientSize().Width, page.GetClientSize().Height);

            ////Save the document into memory stream
            //document.Save(stream);
            //stream.Position = 0;


            //    Xamarin.Forms.DependencyService.Get<ISaveFiles>().Save("XAMLtoPDF.pdf", "application/pdf", stream);

            //MemoryStream memoryStream = new MemoryStream();
            //memoryStream = new MemoryStream(screenshotBytes);
            //Xamarin.Forms.DependencyService.Get<ISaveFiles>().PrintImage(memoryStream);


            
            //Important
            //_nativeServices.PrintImage(AppSettings.Current.TaxReceiptStream);
        }
        catch (Exception ex)
        {
        }
    }

    private async Task InitializePage()
    {
        PrintCommand = new Command(PrintClicked);
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