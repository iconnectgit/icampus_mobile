using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.PopupForms;

public class UpdatePhotoPopupForm : ViewModelBase
	{
        #region Declaration
        public ICommand ChangeCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        #endregion

        #region Properties
        IList<AttachmentFileView> _attachmentFiles = new ObservableCollection<AttachmentFileView>();
        public IList<AttachmentFileView> AttachmentFiles
        {
            get => _attachmentFiles;
            set
            {
                _attachmentFiles = value;
                OnPropertyChanged(nameof(AttachmentFiles));
            }
        }
        private string _profilePicturePath;
        public string ProfilePicturePath
        {
            get => _profilePicturePath;
            set
            {
                _profilePicturePath = value;
                OnPropertyChanged(nameof(ProfilePicturePath));
            }
        }
        private int _studentId;
        public int StudentId
        {
            get => _studentId;
            set
            {
                _studentId = value;
                OnPropertyChanged(nameof(StudentId));
            }
        }
        private bool _isChangeButtonVisible;
        public bool IsChangeButtonVisible
        {
            get => _isChangeButtonVisible;
            set
            {
                _isChangeButtonVisible = value;
                OnPropertyChanged(nameof(IsChangeButtonVisible));
            }
        }
        private bool _isSaveButtonVisible;
        public bool IsSaveButtonVisible
        {
            get => _isSaveButtonVisible;
            set
            {
                _isSaveButtonVisible = value;
                OnPropertyChanged(nameof(IsSaveButtonVisible));
            }
        }
        private string _imageDataJsonString;
        public string ImageDataJsonString
        {
            get => _imageDataJsonString;
            set
            {
                _imageDataJsonString = value;
                OnPropertyChanged(nameof(ImageDataJsonString));
            }
        }
        #endregion
        public UpdatePhotoPopupForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            ChangeCommand = new Command(ChangeCommandMethod);
            SaveCommand = new Command(SaveCommandMethod);
            IsChangeButtonVisible = true;
        }

        #region Methods
        private async void ChangeCommandMethod(object obj)
        {
            try
            {
                AttachmentFiles.Clear();
                AttachmentFileView fileData = await HelperMethods.PickImageFromDevice();
                if (fileData == null)
                {
                    return;
                }
                IsChangeButtonVisible = false;
                IsSaveButtonVisible = true;
                ProfilePicturePath = fileData.FilePath;
                AttachmentFiles.AddFileToList(fileData);
                WebFormImageView imageDetails = new WebFormImageView();
                imageDetails.Id = StudentId;
                imageDetails.ImageFileName = fileData.FileName;
                ImageDataJsonString = JsonConvert.SerializeObject(new List<WebFormImageView>() { imageDetails });

            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void SaveCommandMethod(object obj)
        {
            try
            {
                var attachments = new List<AttachmentFileView>(AttachmentFiles);
                OperationDetails result = await ApiHelper.PostMultiDataRequestAsync<OperationDetails>(string.Format(TextResource.UploadStudentImageApi, ImageDataJsonString), AppSettings.Current.ApiUrl,files:attachments);
                if(result.Success)
                {
                    AppSettings.Current.CurrentPopup.Close();
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        #endregion
    }