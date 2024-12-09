using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Complaints;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Complaints;

public class ComplaintsForm : ViewModelBase
    {
        #region Declarations
        private Popup _currentPopup;
        public ICommand AttachmentClickCommand { get; set; }
        public ICommand ListTappedCommand { get; set; }
        public ICommand RaiseComplaintClickCommand { get; set; }
        public ICommand EditClickCommand { get; set; }
        public ICommand DeleteClickCommand { get; set; }
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

        ObservableCollection<UserComplaintView> _complaintsList = new ObservableCollection<UserComplaintView>();
        public ObservableCollection<UserComplaintView> ComplaintList
        {
            get => _complaintsList;
            set
            {
                _complaintsList = value;
                OnPropertyChanged(nameof(ComplaintList));
            }
        }
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

        bool _isNoRecordMsg;
        public bool IsNoRecordMsg
        {
            get => _isNoRecordMsg;
            set
            {
                _isNoRecordMsg = value;
                OnPropertyChanged(nameof(IsNoRecordMsg));
            }
        }
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
        #endregion

        public ComplaintsForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        #region Private methods
        private async void InitializePage()
        {
            try
            {
                MenuVisible = true;
                IsNoRecordMsg = false;
                AttachmentClickCommand = new Command<UserComplaintView>(AttachmentClicked);
                ListTappedCommand = new Command<UserComplaintView>(ListViewTapped);
                EditClickCommand = new Command(EditClicked);
                DeleteClickCommand = new Command(DeleteClicked);
                RaiseComplaintClickCommand = new Command(RaiseComplaintClicked);
                BeamMenuClickCommand = new Command(BeamMenuClicked);
                BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
                BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
                BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
                //await GetComplaintsList();
                MessagingCenter.Subscribe<RaiseComplaintsForm>(this, "SendComplaint", (arg) =>
                {
                    AppSettings.Current.RefreshComplaintsList = true;
                    GetComplaintsList();
                });
                MessagingCenter.Subscribe<UserComplaintView>(this, "DeleteComplaint", (deletedComplaint) =>
                {
                    if(deletedComplaint != null)
                    {
                        this.ComplaintList.Remove(deletedComplaint);
                        AppSettings.Current.RefreshComplaintsList = true;
                        GetComplaintsList();
                    }
                });
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.ComplaintsPageTitle);
            }
        }

        private async void EditClicked()
        {
            
        }
        private async void ListViewTapped(UserComplaintView obj)
        {
            if (obj != null)
            {
                RaiseComplaintsForm raiseComplaintsForm = new RaiseComplaintsForm(_mapper, _nativeServices, Navigation)
                {
                    PageTitle = TextResource.EditComplaintTitle,
                    BackVisible = true,
                    MenuVisible = false,
                    CategoryList = CategoryList,
                    StudentList = AppSettings.Current.StudentList,
                    SelectedComplaint= obj,
                    IsEdit = true,
                    ComplaintDescription = obj.DescriptionMessage,
                    IsResolved = !string.Equals(obj.StatusName, "resolved", StringComparison.OrdinalIgnoreCase),
                    AttachmentFiles =new ObservableCollection<AttachmentFileView>(obj.Attachments),
                    SelectedCategory = CategoryList.Where(x => x.ItemId == obj.CategoryId.ToString()).FirstOrDefault(),
                    SelectedStudent = AppSettings.Current.StudentList.Where(x => x.ItemId == obj.StudentId.ToString()).FirstOrDefault()
                };
                RaiseComplaints raiseComplaints = new RaiseComplaints()
                {
                    BindingContext = raiseComplaintsForm
                };
                await Navigation.PushAsync(raiseComplaints);
            }
            SelectedComplaint = null;
        }

        private async void DeleteClicked(object obj)
        {
            try
            {
                if (obj != null)
                {
                    var selectedComplaint = (UserComplaintView)obj;
                    var deleteTapAction = await App.Current.MainPage.DisplayAlert(TextResource.DeleteConfirmationTitle, TextResource.DeleteText, TextResource.YesText, TextResource.NoText);
                    if (deleteTapAction)
                    {
                        OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.DeleteComplaintApiUrl, selectedComplaint.ComplaintId), AppSettings.Current.ApiUrl);
                        if (result.Success)
                        {
                            ComplaintList.Remove(selectedComplaint);
                            AppSettings.Current.RefreshComplaintsList = true;
                            GetComplaintsList();
                        }
                        else
                        {
                            HelperMethods.DisplayException(new Exception(TextResource.ExceptionMessage), this.PageTitle);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async void RaiseComplaintClicked(object obj)
        {
            RaiseComplaintsForm raiseComplaintsForm = new RaiseComplaintsForm(_mapper, _nativeServices, Navigation)
            {
                PageTitle = TextResource.RaiseComplaintsTitle,
                BackVisible = true,
                MenuVisible = false,
                CategoryList = CategoryList,
                StudentList = AppSettings.Current.StudentList
            };
            RaiseComplaints raiseComplaints = new RaiseComplaints()
            {
                BindingContext = raiseComplaintsForm
            };
            await Navigation.PushAsync(raiseComplaints);
        }

        public async Task<IEnumerable<UserComplaintView>> GetComplaintsList()
        {
            try
            {
                bool loadFilterPanelLists = true;
                var complaintData = await ApiHelper.GetObject<UserComplaintViewModel>(string.Format(TextResource.ComplaintsListApiUrl, loadFilterPanelLists), cacheKeyPrefix: "complaints", cacheType: AppSettings.Current.RefreshComplaintsList ? ApiHelper.CacheTypeParam.LoadFromServerAndCache : ApiHelper.CacheTypeParam.LoadFromCache);
                this.ComplaintList = new ObservableCollection<UserComplaintView>(complaintData.UserComplaintList);
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    AppSettings.Current.RefreshComplaintsList = false;
                CategoryList = complaintData.ComplaintCategories;
                IsNoRecordMsg = this.ComplaintList.ToList().Count > 0 ? false : true;

                return ComplaintList;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.ComplaintsPageTitle);
                return new List<UserComplaintView>();
            }
        }
        
        private async void AttachmentClicked(UserComplaintView sender)
        {
            try
            {
                AttachmentListPopupForm attachmentListPopupForm = new(_mapper, _nativeServices, Navigation)
                {
                    SelectedAttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(sender.Attachments)
                };
                var attachmentListPopup = new AttachmentListPopup()
                {
                    BindingContext = attachmentListPopupForm
                };
                SetPopupInstance(attachmentListPopup);
                await Application.Current.MainPage.ShowPopupAsync(attachmentListPopup);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void SetPopupInstance(Popup popup)
        {
            AppSettings.Current.CurrentPopup = popup;
        }
        public override async void GetStudentData()
        {
            try
            {
                await GetComplaintsList();
                base.GetStudentData();
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.CampusKeyPageTitle);
            }
        }
        #endregion
    }