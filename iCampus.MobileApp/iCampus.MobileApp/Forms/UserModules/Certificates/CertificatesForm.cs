using System.Windows.Input;
using AutoMapper;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Certificates;

public class CertificatesForm : ViewModelBase
    {
        protected INativeServices _nativeServices;
        public ICommand CertificateClickCommand { get; set; }
        IList<BindableCertificateView> _certificateDetailedList = new List<BindableCertificateView>();
        public IList<BindableCertificateView> CertificateDetailedList
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

        IList<BindableCertificateView> _selectedCertificate;
        public IList<BindableCertificateView> SelectedCertificate
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

        public CertificatesForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        private async void InitializePage()
        {
            HelperMethods.GetSelectedStudent();
            CertificateClickCommand = new Command<BindableCertificateView>(CertificateViewClicked);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
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
                            ? _mapper?.Map<List<BindableCertificateView>>(data.CertificateDetailedList).Where(x => x.FileExist).ToList()
                            : _mapper?.Map<List<BindableCertificateView>>(data.CertificateDetailedList).ToList();

                ShowErrorMessage = CertificateDetailedList.Count == 0;
            }
            catch (Exception ex)
            {
                Helpers.HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }


        private void CertificateViewClicked(BindableCertificateView certificate)
        {
            try
            {
                if (certificate == null)
                    return;

                foreach (var item in CertificateDetailedList)
                {
                    item.IsSelected = item.CertificateId == certificate.CertificateId;

                    if (item.CertificateId == certificate.CertificateId)
                    {
                        if (!item.FileExist)
                        {
                            NotAvailableMessage = !string.IsNullOrEmpty(item.ErrorMessage)
                                ? item.ErrorMessage
                                : TextResource.CertificateNotAvailableMessage;
                    
                            ShowErrorMessage = true;
                        }
                        else
                        {
                            HelperMethods.OpenFileForPreview(item.CertificateFilePath, _nativeServices);
                            ShowErrorMessage = false;
                        }
                    }
                }
                MessagingCenter.Send("", "UpdateCertificateList");
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
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