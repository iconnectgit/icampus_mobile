using System.Windows.Input;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Certificates;

public class CertificatesForm : ViewModelBase
    {
        protected INativeServices _nativeServices;
        public ICommand CertificateClickCommand { get; set; }
        IList<CertificateView> _certificateDetailedList = new List<CertificateView>();
        public IList<CertificateView> CertificateDetailedList
        {
            get => _certificateDetailedList;
            set
            {
                _certificateDetailedList = value;
                OnPropertyChanged(nameof(CertificateDetailedList));
            }
        }
        IList<PickListItem> _certificateList;
        public IList<PickListItem> CertificateList
        {
            get => _certificateList;
            set
            {
                _certificateList = value;
                OnPropertyChanged(nameof(CertificateList));
            }        }

        IList<CertificateView> _selectedCertificate;
        public IList<CertificateView> SelectedCertificate
        {
            get => _selectedCertificate;
            set
            {
                _selectedCertificate = value;
                OnPropertyChanged(nameof(SelectedCertificate));
            }           
        }
        private bool _showCertificateList;
        public bool ShowCertificateList
        {
            get => _showCertificateList;
            set
            {
                _showCertificateList = value;
                OnPropertyChanged(nameof(ShowCertificateList));
            }   
        }
        private bool _showErrorMessage;
        public bool ShowErrorMessage
        {
            get => _showErrorMessage;
            set
            {
                _showErrorMessage = value;
                OnPropertyChanged(nameof(ShowErrorMessage));
            }   
        }
        private bool _showCertificate;
        public bool ShowCertificate
        {
            get => _showCertificate;
            set
            {
                _showCertificate = value;
                OnPropertyChanged(nameof(ShowCertificate));
            }   
        }
        private string _notAvailableMessage;
        public string NotAvailableMessage
        {
            get => _notAvailableMessage;
            set
            {
                _notAvailableMessage = value;
                OnPropertyChanged(nameof(NotAvailableMessage));
            }  
        }
        
        public List<int> CertificateIdList = new List<int>();

        public CertificatesForm(INativeServices nativeServices) : base(null, null, null)
        {
            _nativeServices = nativeServices;
            InitializePage();
        }

        private async void InitializePage()
        {
            CertificateClickCommand = new Command<CertificateView>(CertificateViewClicked);
        }

        public async Task GetCertificates()
        {
            try
            {
                string cacheKeyPrefix = "certificate";
                var data = await ApiHelper.GetObject<CertificateViewModel>(string.Format(TextResource.GetCertificateDataUrl, AppSettings.Current.SelectedStudent.ItemId), cacheKeyPrefix: cacheKeyPrefix, cacheType: ApiHelper.CacheTypeParam.LoadFromCache);
                CertificateList = data.CertificateList;
                NotAvailableMessage = string.IsNullOrEmpty(data.CertificateSettings.NotAvailableMessage)
                            ? TextResource.CertificateNotAvailableMessage
                            : data.CertificateSettings.NotAvailableMessage;

                CertificateDetailedList = data.CertificateSettings.HideCertificateIfNoFileUploaded
                            ? data.CertificateDetailedList.Where(x => x.FileExist).ToList()
                            : data.CertificateDetailedList.ToList();

                ShowErrorMessage = CertificateDetailedList.Count == 0;
            }
            catch (Exception ex)
            {
                Helpers.HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }


        private void CertificateViewClicked(CertificateView certificate)
        {
            try
            {
                if (certificate != null)
                {
                    foreach (var item in CertificateDetailedList)
                    {
                        if (item != null)
                        {
                            if (item.CertificateId == certificate.CertificateId)
                            {
                                if (!item.FileExist)
                                {
                                    if (!string.IsNullOrEmpty(certificate.ErrorMessage))
                                    {
                                        NotAvailableMessage = certificate.ErrorMessage;
                                    }
                                    ShowErrorMessage = true;
                                }
                                else
                                {
                                    HelperMethods.OpenFileForPreview(certificate.CertificateFilePath, _nativeServices);
                                    ShowErrorMessage = false;

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }

        public async override void GetStudentData()
        {
            try
            {
                GetCertificates();
                base.GetStudentData();
                ShowErrorMessage = false;
            }
            catch (Exception ex)
            {
                Helpers.HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
    }