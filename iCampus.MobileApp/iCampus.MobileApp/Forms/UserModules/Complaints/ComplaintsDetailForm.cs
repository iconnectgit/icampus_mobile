using System.Windows.Input;
using AutoMapper;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Complaints;

public class ComplaintsDetailForm : ViewModelBase
    {
        #region Declarations
        public ICommand PreviewIconTappedCommand { get; set; }
        public ICommand DownloadTappedCommand { get; set; }
        #endregion
        #region Properties
        UserComplaintView _selectedComplaint = new UserComplaintView();
        public UserComplaintView SelectedComplaint
        {
            get => _selectedComplaint;
            set
            {
                _selectedComplaint = value;
                OnPropertyChanged(nameof(SelectedComplaint));
            }
        }
        IList<BindableAttachmentFileView> _attachmentList;
        public IList<BindableAttachmentFileView> AttachmentList
        {
            get => _attachmentList;
            set
            {
                _attachmentList = value;
                OnPropertyChanged(nameof(AttachmentList));
            }
        }
        #endregion
        public ComplaintsDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            this.MenuVisible = false;
            this.BackVisible = true;
            this.PreviewIconTappedCommand = new Command(PreviewIconClicked);
            this.DownloadTappedCommand = new Command(DownloadClicked);
        }
        
        private async void DownloadClicked(object obj)
        {
            if (obj != null)
            {
                try
                {
                    var selectedAttachment = (BindableAttachmentFileView)obj;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        if (!string.IsNullOrEmpty(selectedAttachment.FilePath))
                            await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
                    }
                    else
                    {
                        if (selectedAttachment.FileStatus == 0)
                        {
                            AttachmentList[AttachmentList.IndexOf(selectedAttachment)].FileStatus = 1;
                            string filePath = await HelperMethods.DownloadAndReturnFilePath(selectedAttachment.FilePath, _nativeServices);
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                AttachmentList[AttachmentList.IndexOf(selectedAttachment)].FileDevicePath = filePath;
                                AttachmentList[AttachmentList.IndexOf(selectedAttachment)].FileStatus = 2;
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

        private async void PreviewIconClicked(Object obj)
        {
            if (obj != null)
            {
                try
                {
                    var selectedAttachment = (BindableAttachmentFileView)obj;
                    if(!string.IsNullOrEmpty(selectedAttachment.FilePath))
                       await HelperMethods.OpenFileForPreview(selectedAttachment.FilePath, _nativeServices);
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                }
            }
        }
    }