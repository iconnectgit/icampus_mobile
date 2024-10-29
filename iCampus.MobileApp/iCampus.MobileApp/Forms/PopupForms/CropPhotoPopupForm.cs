using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.PopupForms;

public class CropPhotoPopupForm : ViewModelBase
{
    #region Declaration

    public ICommand BrowseCommand { get; set; }
    public ICommand SaveCropedCommand { get; set; }

    public event EventHandler<PhotoSavedEventArgs> PhotoSaved;

    #endregion

    #region Properties

    private IList<AttachmentFileView> _attachmentFiles = new ObservableCollection<AttachmentFileView>();

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

    private double _cropBoxX;

    public double CropBoxX
    {
        get => _cropBoxX;
        set
        {
            _cropBoxX = value;
            OnPropertyChanged(nameof(CropBoxX));
        }
    }

    private double _cropBoxY;

    public double CropBoxY
    {
        get => _cropBoxY;
        set
        {
            _cropBoxY = value;
            OnPropertyChanged(nameof(CropBoxY));
        }
    }

    private double _cropBoxWidth;

    public double CropBoxWidth
    {
        get => _cropBoxWidth;
        set
        {
            _cropBoxWidth = value;
            OnPropertyChanged(nameof(CropBoxWidth));
        }
    }

    private double _cropBoxHeight;

    public double CropBoxHeight
    {
        get => _cropBoxHeight;
        set
        {
            _cropBoxHeight = value;
            OnPropertyChanged(nameof(CropBoxHeight));
        }
    }

    private bool _isCropBoxVisible;

    public bool IsCropBoxVisible
    {
        get => _isCropBoxVisible;
        set
        {
            _isCropBoxVisible = value;
            OnPropertyChanged(nameof(IsCropBoxVisible));
        }
    }

    #endregion

    public CropPhotoPopupForm() : base(null, null, null)
    {
        BrowseCommand = new Command(BrowseCommandMethod);
        SaveCropedCommand = new Command(SaveCropedMethod);
        IsChangeButtonVisible = true;
        //CrossMedia.Current.Initialize();
    }

    #region Methods

    private async void BrowseCommandMethod(object obj)
    {
        try
        {
            AttachmentFiles.Clear();
            var fileData = await HelperMethods.PickImageFromDevice();
            if (fileData == null) return;

            IsChangeButtonVisible = false;
            IsSaveButtonVisible = true;
            ProfilePicturePath = fileData.FilePath;
            AttachmentFiles.AddFileToList(fileData);
            IsCropBoxVisible = true;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }


    private async void SaveCropedMethod(object obj)
    {
        try
        {
            var coordinates = new
            {
                X = CropBoxX,
                Y = CropBoxY,
                Width = CropBoxWidth,
                Height = CropBoxHeight
            };

            var firstAttachment = AttachmentFiles.Count > 0 ? AttachmentFiles[0] : null;

            var imageDetails = new WebFormImageView
            {
                FolderPath = firstAttachment?.FilePath,
                ImageFileName = firstAttachment?.FileName,
                XAxisPoint = Convert.ToInt32(CropBoxX),
                YAxisPoint = Convert.ToInt32(CropBoxY),
                Width = Convert.ToInt32(CropBoxWidth),
                Height = Convert.ToInt32(CropBoxHeight)
            };

            PhotoSaved?.Invoke(this, new PhotoSavedEventArgs((ObservableCollection<AttachmentFileView>)AttachmentFiles, imageDetails));
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    #endregion
}

public class PhotoSavedEventArgs : EventArgs
{
    public IList<AttachmentFileView> AttachmentFiles { get; }
    public WebFormImageView ImageDetails { get; }

    public PhotoSavedEventArgs(ObservableCollection<AttachmentFileView> attachmentFiles, WebFormImageView imageDetails)
    {
        AttachmentFiles = attachmentFiles;
        ImageDetails = imageDetails;
    }
}