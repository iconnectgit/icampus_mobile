using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.UserModules;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.PopupForms;

public class AttachmentListPopupForm : ViewModelBase
    {
        #region Declarations       
        public ICommand AttachmentListTappedCommand { get; set; }
        public ICommand DownloadTappedCommand { get; set; }
        public ICommand SampleCommand { get; set; }
        #endregion

        #region Properties      
        IList<BindableAttachmentFileView> _selectedAttachmentList;
        public IList<BindableAttachmentFileView> SelectedAttachmentList
        {
            get => _selectedAttachmentList;
            set
            {
                _selectedAttachmentList = value;
                OnPropertyChanged(nameof(SelectedAttachmentList));
            }
        }
        BindableAttachmentFileView _selectedItem;
        public BindableAttachmentFileView SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        int _listViewHeight;
        public int ListViewHeight
        {
            get => _listViewHeight;
            set
            {
                _listViewHeight = value;
                OnPropertyChanged(nameof(ListViewHeight));
            }
        }
        #endregion
        public AttachmentListPopupForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            AttachmentListTappedCommand = new Command(AttachmentListClicked);
            DownloadTappedCommand = new Command(DownloadClicked);
            SampleCommand = new Command(SampleMethod);
        }

        private void SampleMethod()
        {
            
        }

        #region Methods
        private async void AttachmentListClicked(Object obj)
        {
            if (obj != null)
                try
                {
                    await AppSettings.Current.CurrentPopup?.CloseAsync();
                    var selectedAttachment = (BindableAttachmentFileView)obj;
                    if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                        await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, PageTitle);
                }
        }
        private async void DownloadClicked(object obj)
        {
            if (obj != null)
            {
                try
                {
                    var selectedAttachment = (BindableAttachmentFileView)obj;
                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await AppSettings.Current.CurrentPopup?.CloseAsync();
                        if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                            await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
                    }
                    else
                    {
                        if (selectedAttachment.FileStatus == 0)
                        {
                            SelectedAttachmentList[SelectedAttachmentList.IndexOf(selectedAttachment)].FileStatus = 1;
                            string filePath = await HelperMethods.DownloadAndReturnFilePath(selectedAttachment.FilePath, _nativeServices);
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                SelectedAttachmentList[SelectedAttachmentList.IndexOf(selectedAttachment)].FileDevicePath = filePath;
                                SelectedAttachmentList[SelectedAttachmentList.IndexOf(selectedAttachment)].FileStatus = 2;
                            }
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                }
            }
        }
        #endregion
    }