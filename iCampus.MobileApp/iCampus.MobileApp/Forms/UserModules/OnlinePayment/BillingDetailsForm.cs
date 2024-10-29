using System.Web;
using System.Windows.Input;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.OnlinePayment;
using iCampus.Portal.ViewModels;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.UserModules.OnlinePayment;

public class BillingDetailsForm : ViewModelBase
    {
        #region Declarations
        public ICommand ProceedCommand { get; set; }

        #endregion
        #region Properties
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private string _streetOne;
        public string StreetOne
        {
            get => _streetOne;
            set
            {
                _streetOne = value;
                OnPropertyChanged(nameof(StreetOne));
            }
        }
        private string _streetTwo;
        public string StreetTwo
        {
            get => _streetTwo;
            set
            {
                _streetTwo = value;
                OnPropertyChanged(nameof(StreetTwo));
            }
        }
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }
        private string _area;
        public string Area
        {
            get => _area;
            set
            {
                _area = value;
                OnPropertyChanged(nameof(Area));
            }
        }
        private string _postCode;
        public string PostCode
        {
            get => _postCode;
            set
            {
                _postCode = value;
                OnPropertyChanged(nameof(PostCode));
            }
        }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _mandatoryFieldErrorMessage;
        public string MandatoryFieldErrorMessage
        {
            get => _mandatoryFieldErrorMessage;
            set
            {
                _mandatoryFieldErrorMessage = value;
                OnPropertyChanged(nameof(MandatoryFieldErrorMessage));
            }
        }
        private bool _isFirstNameErrorMessageVisible;
        public bool IsFirstNameErrorMessageVisible
        {
            get => _isFirstNameErrorMessageVisible;
            set
            {
                _isFirstNameErrorMessageVisible = value;
                OnPropertyChanged(nameof(IsFirstNameErrorMessageVisible));
            }
        }
        private decimal _actualAmount;
        public decimal ActualAmount
        {
            get => _actualAmount;
            set
            {
                _actualAmount = value;
                OnPropertyChanged(nameof(ActualAmount));
            }
        }
        private bool _isLastNameErrorMessageVisible;
        public bool IsLastNameErrorMessageVisible
        {
            get => _isLastNameErrorMessageVisible;
            set
            {
                _isLastNameErrorMessageVisible = value;
                OnPropertyChanged(nameof(IsLastNameErrorMessageVisible));
            }
        }
        private bool _isStreetOneErrorMessageVisible;
        public bool IsStreetOneErrorMessageVisible
        {
            get => _isStreetOneErrorMessageVisible;
            set
            {
                _isStreetOneErrorMessageVisible = value;
                OnPropertyChanged(nameof(IsStreetOneErrorMessageVisible));
            }
        }
        private bool _isEmailErrorMessageVisible;
        public bool IsEmailErrorMessageVisible
        {
            get => _isEmailErrorMessageVisible;
            set
            {
                _isEmailErrorMessageVisible = value;
                OnPropertyChanged(nameof(IsEmailErrorMessageVisible));
            }
        }
        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }
        private string _merchantName;
        public string MerchantName
        {
            get => _merchantName;
            set
            {
                _merchantName = value;
                OnPropertyChanged(nameof(MerchantName));
            }
        }
        private string _orderNumber;
        public string OrderNumber
        {
            get => _orderNumber;
            set
            {
                _orderNumber = value;
                OnPropertyChanged(nameof(OrderNumber));
            }
        }
        private string _paymentModeType;
        public string PaymentModeType
        {
            get => _paymentModeType;
            set
            {
                _paymentModeType = value;
                OnPropertyChanged(nameof(PaymentModeType));
            }
        }
        private int _mode;
        public int Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                OnPropertyChanged(nameof(Mode));
            }
        }
        List<OnlinePaymentInvoiceSubDetails> _invoiceSubDetailsList = new List<OnlinePaymentInvoiceSubDetails>();
        public List<OnlinePaymentInvoiceSubDetails> InvoiceSubDetailsList
        {
            get => _invoiceSubDetailsList;
            set
            {
                _invoiceSubDetailsList = value;
                OnPropertyChanged(nameof(InvoiceSubDetailsList));
            }
        }
        private int _academicYearForBillingDetails;
        public int AcademicYearForBillingDetails
        {
            get => _academicYearForBillingDetails;
            set
            {
                _academicYearForBillingDetails = value;
                OnPropertyChanged(nameof(AcademicYearForBillingDetails));
            }
        }
        BindableOnlinePaymentFilterView _billingDetails = new BindableOnlinePaymentFilterView();
        public BindableOnlinePaymentFilterView BillingDetails
        {
            get => _billingDetails;
            set
            {
                _billingDetails = value;
                OnPropertyChanged(nameof(BillingDetails));
            }
        }
        BindableReceiptDetailView _receiptDetails = new BindableReceiptDetailView();
        public BindableReceiptDetailView ReceiptDetails
        {
            get => _receiptDetails;
            set
            {
                _receiptDetails = value;
                OnPropertyChanged(nameof(ReceiptDetails));
            }
        }
        private string _invalidEmailErrorMessage;
        public string InvalidEmailErrorMessage
        {
            get => _invalidEmailErrorMessage;
            set
            {
                _invalidEmailErrorMessage = value;
                OnPropertyChanged(nameof(InvalidEmailErrorMessage));
            }
        }
        private decimal _vatAmount;
        public decimal VatAmount
        {
            get => _vatAmount;
            set
            {
                _vatAmount = value;
                OnPropertyChanged(nameof(VatAmount));
            }
        }
        private string _minimumPaymentDetailsToPaymentUrl;
        public string MinimumPaymentDetailsToPaymentUrl
        {
            get => _minimumPaymentDetailsToPaymentUrl;
            set
            {
                _minimumPaymentDetailsToPaymentUrl = value;
                OnPropertyChanged(nameof(MinimumPaymentDetailsToPaymentUrl));
            }
        }
        private bool _isInvalidEmailErrorMessageVisible;
        public bool IsInvalidEmailErrorMessageVisible
        {
            get => _isInvalidEmailErrorMessageVisible;
            set
            {
                _isInvalidEmailErrorMessageVisible = value;
                OnPropertyChanged(nameof(IsInvalidEmailErrorMessageVisible));
            }
        }
        private string _reasonForPayment;
        public string ReasonForPayment
        {
            get => _reasonForPayment;
            set
            {
                _reasonForPayment = value;
                OnPropertyChanged(nameof(ReasonForPayment));
            }
        }
        private string _cardNumberEnc;
        public string CardNumberEnc
        {
            get => _cardNumberEnc;
            set
            {
                _cardNumberEnc = value;
                OnPropertyChanged(nameof(CardNumberEnc));
            }
        }
        private string _pendingAndBouncedPdcListString;
        public string PendingAndBouncedPdcListString
        {
            get => _pendingAndBouncedPdcListString;
            set
            {
                _pendingAndBouncedPdcListString = value;
                OnPropertyChanged(nameof(PendingAndBouncedPdcListString));
            }
        }
        private string _invoiceIds;
        public string InvoiceIds
        {
            get => _invoiceIds;
            set
            {
                _invoiceIds = value;
                OnPropertyChanged(nameof(InvoiceIds));
            }
        }
        private string _studentIds;
        public string StudentIds
        {
            get => _studentIds;
            set
            {
                _studentIds = value;
                OnPropertyChanged(nameof(StudentIds));
            }
        }
        private string _proformaListString;
        public string ProformaListString
        {
            get => _proformaListString;
            set
            {
                _proformaListString = value;
                OnPropertyChanged(nameof(ProformaListString));
            }
        }
        #endregion
        public BillingDetailsForm(INavigation navigation) : base(null, null, null)
        {
            Navigation = navigation;
            InitializePage();
        }
        #region Methods
        private async void InitializePage()
        {
            ProceedCommand = new Command(ProceedClicked);
            MandatoryFieldErrorMessage = TextResource.MandatoryFieldErrorMessage;
            await GetBillingDetails();

            MessagingCenter.Subscribe<string>(this, "PaymentResult", async (arg) =>
            {
                if (AppSettings.Current.IsPaymentSuccessful)
                {
                    await SuccessfulPaymentUpdation();
                }
            });
        }
        private async void ProceedClicked()
        {
            try
            {
                if (!isValid())
                    return;
                PaymentWebViewForm paymentWebViewForm = new PaymentWebViewForm(Navigation);
                paymentWebViewForm.BackVisible = true;
                paymentWebViewForm.PageTitle = "Online Payment";
                string invoiceSubDetailsListString = (InvoiceSubDetailsList != null) ? JsonConvert.SerializeObject(InvoiceSubDetailsList) : string.Empty;
                if (!String.IsNullOrEmpty(invoiceSubDetailsListString))
                {
                    invoiceSubDetailsListString = HttpUtility.UrlEncode(invoiceSubDetailsListString);
                }
                string clientGroupCode = (!string.IsNullOrEmpty(App.ClientGroupCode)) ? App.ClientGroupCode : string.Empty;
                bool isBeam = clientGroupCode.ToLower().Equals("beam");
                string url = string.Format(String.Concat(BillingDetails.PortalPaymentPageUrl, "?SessionId={0}&OrderId={1}&Amount={2}&StudentIds={3}&PaymentModeType={4}&Mode={5}&FirstName={6}&LastName={7}&Street1={8}&Street2={9}&City={10}&Area={11}&PostCode={12}&phone={13}&emailid={14}&MerchantName={15}&InvoiceSubDetailListString={16}&AcademicYear={17}&PaymentMode={18}&VatAmount={19}&ReasonForPayment={20}&CardNumberEnc={21}&PendingAndBouncedPdcListString={22}&InvoiceIds={23}&ProformaListString={24}"), AppSettings.Current.UserSessionUid, BillingDetails.OrderId, Amount, StudentIds, BillingDetails.PaymentModeType, BillingDetails.Mode, HttpUtility.UrlEncode(FirstName), HttpUtility.UrlEncode(LastName), HttpUtility.UrlEncode(StreetOne), HttpUtility.UrlEncode(StreetTwo), HttpUtility.UrlEncode(City), HttpUtility.UrlEncode(Area), HttpUtility.UrlEncode(PostCode), PhoneNumber, Email, HttpUtility.UrlEncode(MerchantName), HttpUtility.UrlEncode(invoiceSubDetailsListString), AcademicYearForBillingDetails, PaymentModeType, VatAmount, BillingDetails.ReasonForPayment, BillingDetails.CardNumberEnc, HttpUtility.UrlEncode(BillingDetails.PendingAndBouncedPdcListString), BillingDetails.InvoiceIds, HttpUtility.UrlEncode(ProformaListString));
                if (isBeam)
                {
                    paymentWebViewForm.WebViewUrl = string.Concat(url, MinimumPaymentDetailsToPaymentUrl);
                }
                else
                {
                    paymentWebViewForm.WebViewUrl = url;
                }

                PaymentWebView paymentWebView = new PaymentWebView()
                {
                    BindingContext = paymentWebViewForm
                };
               
                var currentPage = Navigation.NavigationStack.LastOrDefault();
                await Navigation.PushAsync(paymentWebView);  
                if (currentPage != null)
                {
                    Navigation.RemovePage(currentPage);  
                }
                
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }



        }
        private bool isValid()
        {

            IsFirstNameErrorMessageVisible = string.IsNullOrEmpty(FirstName) ? true : false;
            IsLastNameErrorMessageVisible = string.IsNullOrEmpty(LastName) ? true : false;
            IsStreetOneErrorMessageVisible = string.IsNullOrEmpty(StreetOne) ? true : false;
            IsEmailErrorMessageVisible = string.IsNullOrEmpty(Email) ? true : false;
            IsInvalidEmailErrorMessageVisible = !IsEmailErrorMessageVisible;
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(StreetOne) || string.IsNullOrEmpty(Email))
            {
                return false;
            }
            else
            {
                return IsValidEmail(Email);
            }
        }
        private async Task GetBillingDetails()
        {
            try
            {
                BindableOnlinePaymentFilterView result = await ApiHelper.GetObject<BindableOnlinePaymentFilterView>(string.Format(TextResource.GetBillingDetails));
                if (result != null)
                {
                    BillingDetails = new BindableOnlinePaymentFilterView();
                    BillingDetails = result;
                    BillingDetails.Mode = Mode;
                    BillingDetails.ReasonForPayment = ReasonForPayment;
                    BillingDetails.CardNumberEnc = CardNumberEnc;
                    BillingDetails.PendingAndBouncedPdcListString = PendingAndBouncedPdcListString;
                    BillingDetails.InvoiceIds = InvoiceIds;
                    FirstName = result.FirstName;
                    LastName = result.LastName;
                    StreetOne = result.Street1;
                    StreetTwo = result.Street2;
                    City = result.City;
                    PostCode = result.PostCode;
                    PhoneNumber = result.Phone;
                    Area = result.Area;
                    Email = result.EmailId;
                    OrderNumber = result.OrderId;
                    MerchantName = result.MerchantName;
                    PaymentModeType = result.PaymentModeType;
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async Task SuccessfulPaymentUpdation()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(2000);
                    //HostScreen.Router.NavigateBack.Execute().Subscribe();
                    await GetTaxReceiptData();
                });
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async Task GetTaxReceiptData()
        {
            try
            {
                if (BillingDetails != null)
                {
                    BindableReceiptDetailView receiptDetails = await ApiHelper.PostRequest<BindableReceiptDetailView>(string.Format(TextResource.GetTaxReceiptDetailsAfterSuccessfulPayment, Convert.ToInt32(AppSettings.Current.SelectedStudentFromAllStudentList.ItemId), BillingDetails.OrderId), AppSettings.Current.ApiUrl);
                    if (receiptDetails != null)
                    {
                        ReceiptDetails = receiptDetails;
                        if (ReceiptDetails.PaymentInvoiceList != null && ReceiptDetails.PaymentInvoiceList.Count > 0)
                        {
                            ReceiptDetails.StudentName = ReceiptDetails.PaymentInvoiceList.FirstOrDefault().StudentName;
                            ReceiptDetails.GradeName = ReceiptDetails.PaymentInvoiceList.FirstOrDefault().GradeName;
                        }
                        ReceiptDetails.PrintLogo = receiptDetails.PaymentSettings.PrintLogo;
                        TaxReceiptForm taxReceiptForm = new TaxReceiptForm(_nativeServices, Navigation);
                        taxReceiptForm.SelectedPaymentHistory = ReceiptDetails;
                        taxReceiptForm.PageTitle = "Online Payment";
                        taxReceiptForm.MenuVisible = false;
                        taxReceiptForm.BackVisible = true;
                        taxReceiptForm.IsPopUpPage = false;
                        taxReceiptForm.ParentName = AppSettings.Current.UserFullName;
                        // if (HostScreen.Router.GetCurrentViewModel().GetType() != typeof(TaxReceiptForm))
                        //     HostScreen.Router.Navigate.Execute(taxReceiptForm).Subscribe();
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                IsInvalidEmailErrorMessageVisible = true;
                InvalidEmailErrorMessage = TextResource.InvalidEmailError;
                return false;
            }
        }
        #endregion
    }