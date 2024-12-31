using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Complaints;

public class RaiseComplaintsForm : ViewModelBase
    {
        #region Declarations
        public ICommand SaveEditorCommand { get; set; }
        private Popup _currentPopup;
        private string _selectedHtmlContentKey;
        public ICommand AddAttachmentClickCommand { get; set; }
        public ICommand SendComplaintClickCommand { get; set; }
        public ICommand DeleteComplaintClickCommand { get; set; }
        public ICommand DeleteAttachmentClickCommand { get; set; }
        public ICommand EditClickCommand { get; set; }
        #endregion
        #region Properties
        IList<ExtPickListItem> _categoryList = new List<ExtPickListItem>();
        public IList<ExtPickListItem> CategoryList
        {
            get => _categoryList;
            set
            {
                _categoryList = value;
                OnPropertyChanged(nameof(CategoryList));
            }
        }

        bool _isCategoryErrVisible;
        public bool IsCategoryErrVisible
        {
            get => _isCategoryErrVisible;
            set
            {
                _isCategoryErrVisible = value;
                OnPropertyChanged(nameof(IsCategoryErrVisible));
            }
        }

        bool _isDescriptionErrVisible;
        public bool IsDescriptionErrVisible
        {
            get => _isDescriptionErrVisible;
            set
            {
                _isDescriptionErrVisible = value;
                OnPropertyChanged(nameof(IsDescriptionErrVisible));
            }
        }

        bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set
            {
                _isEdit = value;
                OnPropertyChanged(nameof(IsEdit));
            }
        }

        string _complaintDescription;
        public string ComplaintDescription
        {
            get => _complaintDescription;
            set
            {
                _complaintDescription =
                    "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>"
                    + value
                    + "</body></html>";
                OnPropertyChanged(nameof(ComplaintDescription));
                IsDescriptionErrVisible = string.IsNullOrWhiteSpace(ComplaintDescription) ? true : false;
            }
        }

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

        ExtPickListItem _selectedCategory = new ExtPickListItem();
        public ExtPickListItem SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                if (!string.IsNullOrEmpty(this.SelectedCategory?.ItemName))
                    IsCategoryErrVisible = false;
            }
        }

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

        List<int> _deletedAttachmentFileID = new List<int>();
        public List<int> DeletedAttachmentFileID
        {
            get => _deletedAttachmentFileID;
            set
            {
                _deletedAttachmentFileID = value;
                OnPropertyChanged(nameof(DeletedAttachmentFileID));
            }
        }

        List<string> _deletedAttachmentFileName = new List<string>();
        public List<string> DeletedAttachmentFileName
        {
            get => _deletedAttachmentFileName;
            set
            {
                _deletedAttachmentFileName = value;
                OnPropertyChanged(nameof(DeletedAttachmentFileName));
            }
        }

        bool _isNavigationBarVisible;
        public bool IsNavigationBarVisible
        {
            get => _isNavigationBarVisible;
            set
            {
                _isNavigationBarVisible = value;
                OnPropertyChanged(nameof(IsNavigationBarVisible));
            }
        }
        bool _isResolved = true;
        public bool IsResolved
        {
            get => _isResolved;
            set
            {
                _isResolved = value;
                OnPropertyChanged(nameof(IsResolved));
            }
        }
        IEnumerable<BindableStudentPickListItem> _studentList;
        public IEnumerable<BindableStudentPickListItem> StudentList
        {
            get
            {
                return _studentList;
            }
            set
            {
                _studentList = value;
                OnPropertyChanged(nameof(StudentList));
            }
        }
        private bool _isFromErrorLabelVisible;
        public bool IsFromErrorLabelVisible
        {
            get => _isFromErrorLabelVisible;
            set
            {
                _isFromErrorLabelVisible = value;
                OnPropertyChanged(nameof(IsFromErrorLabelVisible));
            }
        }
        BindableStudentPickListItem _selectedStudent;
        public BindableStudentPickListItem SelectedStudent
        {
            get
            {
                return _selectedStudent;
            }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }
        bool _isStudentListVisible = true;
        public bool IsStudentListVisible
        {
            get => _isStudentListVisible;
            set
            {
                _isStudentListVisible = value;
                OnPropertyChanged(nameof(IsStudentListVisible));
            }
        }
        private string _editedHtmlContent;
        public string EditedHtmlContent
        {
            get => _editedHtmlContent;
            set
            {
                _editedHtmlContent = value;
                OnPropertyChanged(nameof(EditedHtmlContent));
            }
        }
        private string _htmlEditorTitle;
        public string HtmlEditorTitle
        {
            get => _htmlEditorTitle;
            set
            {
                _htmlEditorTitle = value;
                OnPropertyChanged(nameof(HtmlEditorTitle));
            }
        }
        #endregion
        public RaiseComplaintsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            AddAttachmentClickCommand = new Command(AddAttachmentClicked);
            SendComplaintClickCommand = new Command(SendComplaintClicked);
            DeleteComplaintClickCommand = new Command(DeleteComplaintClicked);
            DeleteAttachmentClickCommand = new Command(DeleteAttachmentClicked);
            EditClickCommand = new Command(EditClicked);
            SaveEditorCommand = new Command(SaveEditorClickedMethod);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
            if(AppSettings.Current.IsTeacher)
            {
                IsStudentListVisible = false;
            }
        }

        #region Methods
        private async void EditClicked(object obj)
        {
            try
            {
                OpenHtmlEditPopup(ComplaintDescription, "remarktext");
            }
            catch(Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
            
        }
        private async void AddAttachmentClicked(object obj)
        {
            AttachmentFileView fileData = await HelperMethods.PickFileFromDevice();
            if (AttachmentFiles.Any(x => x.FileName.Equals(fileData.FileName, StringComparison.OrdinalIgnoreCase)))
            {
                await HelperMethods.ShowAlert("", "This file has already been added.");
                return;
            }
            AttachmentFiles.AddFileToList(fileData);
        }
        private bool ValidateData()
        {
            IsCategoryErrVisible = SelectedCategory != null && SelectedCategory.ItemName != null ? false : true;
            IsDescriptionErrVisible = string.IsNullOrWhiteSpace(ComplaintDescription) ? true : false;
            return !IsCategoryErrVisible && !IsDescriptionErrVisible ? true : false;
        }
        private async void SendComplaintClicked(object obj)
        {
            try
            {
                if (ValidateData())
                {
                    var list = new List<AttachmentFileView>();
                    SelectedComplaint.CategoryId = Convert.ToInt32(this.SelectedCategory.ItemId);
                    SelectedComplaint.DescriptionMessage = this.ComplaintDescription;
                    if(SelectedStudent!= null)
                    {
                        SelectedComplaint.StudentId = Convert.ToInt32(SelectedStudent.ItemId);
                    }
                    if (this.AttachmentFiles != null && this.AttachmentFiles.Count > 0)
                        list = new List<AttachmentFileView>(this.AttachmentFiles);
                    if (DeletedAttachmentFileID != null && DeletedAttachmentFileID.Count > 0)
                    {
                        SelectedComplaint.DeletedAttachmentsArray = new[] { string.Join(",", DeletedAttachmentFileName.Select(x => x)) };
                    }
                    SelectedComplaint.AttachmentsArray = new[] { string.Join(",", list.Select(x => x.FileName)) };
                    SelectedComplaint.ExistingAttachmentIds = string.Join(",", SelectedComplaint.AttachmentList.Select(x => x.AttachmentId));

                    bool result = await ApiHelper.PostMultiDataRequestAsync<bool>(TextResource.SendComplaintApiUrl, AppSettings.Current.ApiUrl, SelectedComplaint, list);
                    if (result)
                    {
                        MessagingCenter.Send<RaiseComplaintsForm>(this, "SendComplaint");
                        await Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void DeleteAttachmentClicked(object obj)
        {
            try
            {
                if (obj != null)
                {
                    var action = await App.Current.MainPage.DisplayAlert("", TextResource.DeleteText, TextResource.YesText, TextResource.NoText);
                    if (action)
                    {
                        AttachmentFileView attachmentFile = (AttachmentFileView)obj;
                        if (attachmentFile.FileData == null)
                        {
                            var fileid = SelectedComplaint.AttachmentList.Where(x => x.AttachmentFile.Equals(attachmentFile.FileName)).FirstOrDefault().AttachmentId;
                            DeletedAttachmentFileID.Add(fileid);
                            DeletedAttachmentFileName.Add(attachmentFile.FileName);
                        }
                        AttachmentFiles.Remove(attachmentFile);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void DeleteComplaintClicked(object obj)
        {
            try
            {
                var deleteTapAction = await App.Current.MainPage.DisplayAlert(TextResource.DeleteConfirmationTitle,
                    TextResource.DeleteText, TextResource.YesText, TextResource.NoText);
                if (deleteTapAction)
                {
                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(
                        string.Format(TextResource.DeleteComplaintApiUrl, SelectedComplaint.ComplaintId),
                        AppSettings.Current.ApiUrl);
                    if (result.Success)
                    {
                        MessagingCenter.Send(SelectedComplaint, "DeleteComplaint");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        HelperMethods.DisplayException(new Exception(TextResource.ExceptionMessage), this.PageTitle);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private void SaveEditorClickedMethod()
        {
            string wrappedHtmlContent = "<html><head><meta name='viewport' content='width=device-width; height=device-height; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;'/></head><body>" 
                                        + EditedHtmlContent 
                                        + "</body></html>";
            ComplaintDescription = wrappedHtmlContent;
            _currentPopup?.Close();
        }
        private void OpenHtmlEditPopup(string htmlText, string key)
        {
            EditedHtmlContent = ExtractBodyContent(htmlText);
            _selectedHtmlContentKey = key;
            HtmlEditorTitle = "Message";
            EditHtmlContentPopup htmlContentPopup = new ()
            {
                BindingContext = this
            };
            SetPopupInstance(htmlContentPopup);
            Application.Current.MainPage.ShowPopup(htmlContentPopup);
        }
        public void SetPopupInstance(Popup popup)
        {
            _currentPopup = popup;
        }
        private string ExtractBodyContent(string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                return string.Empty; 
            }

            int bodyStart = htmlText.IndexOf("<body>") + 6;
            int bodyEnd = htmlText.IndexOf("</body>");

            if (bodyStart != -1 && bodyEnd != -1)
            {
                string bodyContent = htmlText.Substring(bodyStart, bodyEnd - bodyStart).Trim();
        
                string strippedContent = Regex.Replace(bodyContent, "<.*?>", string.Empty).Trim();
        
                return strippedContent;
            }
            else
            {
                string strippedContent = Regex.Replace(htmlText, "<.*?>", string.Empty).Trim();
                
                return strippedContent;
            }
        }

        #endregion
    }