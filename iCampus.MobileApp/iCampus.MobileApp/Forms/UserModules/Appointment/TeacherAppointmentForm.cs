using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.Common.Helpers.Extensions;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Appointment;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Appointment;

public class TeacherAppointmentForm : ViewModelBase
    {
        #region Declarations
        public ICommand TimeSlotClickCommand { get; set; }
        public ICommand BookAppointmentTabbedCommand { get; set; }
        public ICommand AppointmentListTabbedCommand { get; set; }
        public ICommand CommentClickCommand { get; set; }
        public ICommand DeleteClickCommand { get; set; }
        public ICommand RequestAppointmentClickCommand { get; set; }
        #endregion

        #region Properties

        BindableAppointmentView _appointmentViewData =new BindableAppointmentView();
        public BindableAppointmentView AppointmentViewData
        {
            get => _appointmentViewData;
            set
            {
                _appointmentViewData = value;
                OnPropertyChanged(nameof(AppointmentViewData));
            }
        }

        BindableAppointmentView.TeacherAppointmentListView _selectedTeacherAppointment;
        public BindableAppointmentView.TeacherAppointmentListView SelectedTeacherAppointment
        {
            get => _selectedTeacherAppointment;
            set
            {
                _selectedTeacherAppointment = value;
                OnPropertyChanged(nameof(SelectedTeacherAppointment));
            }
        }

        ObservableCollection<BindableAppointmentView.TeacherAppointmentListView> _teacherAppointmentList = new ObservableCollection<BindableAppointmentView.TeacherAppointmentListView>();
        public ObservableCollection<BindableAppointmentView.TeacherAppointmentListView> TeacherAppointmentList
        {
            get => _teacherAppointmentList;
            set
            {
                _teacherAppointmentList = value;
                OnPropertyChanged(nameof(TeacherAppointmentList));
            }
        }
        ObservableCollection<BindableAppointmentView.TeacherCurriculumListView> _teacherCurriculumList = new ObservableCollection<BindableAppointmentView.TeacherCurriculumListView>();
        public ObservableCollection<BindableAppointmentView.TeacherCurriculumListView> TeacherCurriculumList
        {
            get => _teacherCurriculumList;
            set
            {
                _teacherCurriculumList = value;
                OnPropertyChanged(nameof(TeacherCurriculumList));
            }
        }
        BindableAppointmentView.TeacherCurriculumListView _selectedTeacherCurriculum;
        public BindableAppointmentView.TeacherCurriculumListView SelectedTeacherCurriculum
        {
            get => _selectedTeacherCurriculum;
            set
            {
                _selectedTeacherCurriculum = value;
                OnPropertyChanged(nameof(SelectedTeacherCurriculum));
            }
        }

        AppointmentAvailableTimeView _appointmentAvailableTimeData = new AppointmentAvailableTimeView();
        public AppointmentAvailableTimeView AppointmentAvailableTimeData
        {
            get => _appointmentAvailableTimeData;
            set
            {
                _appointmentAvailableTimeData = value;
                OnPropertyChanged(nameof(AppointmentAvailableTimeData));
            }
        }

        bool _noAppointmentDataExist;
        public bool NoAppointmentDataExist
        {
            get => _noAppointmentDataExist;
            set
            {
                _noAppointmentDataExist = value;
                OnPropertyChanged(nameof(NoAppointmentDataExist));
            }
        }

        bool _noTeacherAppointmentExist;
        public bool NoTeacherAppointmentExist
        {
            get => _noTeacherAppointmentExist;
            set
            {
                _noTeacherAppointmentExist = value;
                OnPropertyChanged(nameof(NoTeacherAppointmentExist));
            }
        }
        IEnumerable<AppointmentAvailableTimeView> _appointmentDateList = new List<AppointmentAvailableTimeView>();
        public IEnumerable<AppointmentAvailableTimeView> AppointmentDateList
        {
            get => _appointmentDateList;
            set
            {
                _appointmentDateList = value;
                OnPropertyChanged(nameof(AppointmentDateList));
            }
        }
        private decimal _bookAppointmentButtonOpacity;

        public decimal BookAppointmentButtonOpacity
        {
            get => _bookAppointmentButtonOpacity;
            set
            {
                _bookAppointmentButtonOpacity = value;
                OnPropertyChanged(nameof(BookAppointmentButtonOpacity));
            }
        }

        private decimal _appointmentListButtonOpacity;

        public decimal AppointmentListButtonOpacity
        {
            get => _appointmentListButtonOpacity;
            set
            {
                _appointmentListButtonOpacity = value;
                OnPropertyChanged(nameof(AppointmentListButtonOpacity));
            }
        }

        private bool _isBookAppointmentVisible;

        public bool IsBookAppointmentVisible
        {
            get => _isBookAppointmentVisible;
            set
            {
                _isBookAppointmentVisible = value;
                OnPropertyChanged(nameof(IsBookAppointmentVisible));
            }
        }

        private bool _isAppointmentListVisible;

        public bool IsAppointmentListVisible
        {
            get => _isAppointmentListVisible;
            set
            {
                _isAppointmentListVisible = value;
                OnPropertyChanged(nameof(IsAppointmentListVisible));
            }
        }
        AppointmentAvailableTimeView _selectedDate = new AppointmentAvailableTimeView();
        public AppointmentAvailableTimeView SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                AppointmentTimeList = AppointmentAvailableTimeData.appointmentAvailableTimesList.Where(x => x.BookingDateFormatted == SelectedDate.BookingDateFormatted).ToList();
            }
        }

        AppointmentAvailableTimeView _selectedTime = new AppointmentAvailableTimeView();
        public AppointmentAvailableTimeView SelectedTime
        {
            get => _selectedTime;
            set
            {
                _selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }
        IEnumerable<AppointmentAvailableTimeView> _appointmentTimeList = new List<AppointmentAvailableTimeView>();
        public IEnumerable<AppointmentAvailableTimeView> AppointmentTimeList
        {
            get => _appointmentTimeList;
            set
            {
                _appointmentTimeList = value;
                OnPropertyChanged(nameof(AppointmentTimeList));
            }
        }
        #endregion

        public TeacherAppointmentForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
        {
            IsAppointmentListVisible = false;
            IsBookAppointmentVisible = true;
            BookAppointmentButtonOpacity = 1.0m;
            AppointmentListButtonOpacity = 0.5m;
            BookAppointmentTabbedCommand = new Command(BookAppointmentClicked);
            AppointmentListTabbedCommand = new Command(AppointmentListClicked);
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            BackVisible = false;
            MenuVisible = true;
            NoAppointmentDataExist = false;
            NoTeacherAppointmentExist = false;
            CommentClickCommand = new Command(CommentClicked);
            DeleteClickCommand = new Command(DeleteClicked);
            RequestAppointmentClickCommand = new Command(RequestAppointmentClicked);
            TimeSlotClickCommand = new Command(TimeSlotSelected);
            MessagingCenter.Subscribe<OperationDetails>(this, "RefreshData", async (arg) =>
            {
                AppSettings.Current.RefreshTeacherAppointmentList = true;
                await GetTeacherAppointment();
            });
            MessagingCenter.Subscribe<BindableAppointmentView.TeacherCurriculumListView>(this, "CreateAppointment", async (selectedTeacherCurriculum) =>
            {
                if(selectedTeacherCurriculum != null)
                {
                    TeacherCurriculumList.Remove(selectedTeacherCurriculum);
                }
                AppSettings.Current.RefreshTeacherAppointmentList = true;
                await GetTeacherAppointment();
            });
        }
        public TeacherAppointmentForm(string notificationItemId) : base(null, null, null)
        {
            BookAppointmentButtonOpacity = 1.0m;
            AppointmentListButtonOpacity = 0.5m;
            BookAppointmentTabbedCommand = new Command(BookAppointmentClicked);
            AppointmentListTabbedCommand = new Command(AppointmentListClicked);
            NotificationItemId = notificationItemId;
            AppSettings.Current.RefreshTeacherAppointmentList = true;
            GetTeacherAppointment();
        }
        #region Private Methods
        private void BookAppointmentClicked(object obj)
        {
            
            IsAppointmentListVisible = false;
            IsBookAppointmentVisible = true;
            BookAppointmentButtonOpacity = 1.0m;
            AppointmentListButtonOpacity = 0.5m;
        }

        private void AppointmentListClicked(object obj)
        {

            IsAppointmentListVisible = true;
            IsBookAppointmentVisible = false;
            BookAppointmentButtonOpacity = 0.5m;
            AppointmentListButtonOpacity = 1.0m;
        }
        private async void RequestAppointmentClicked(object obj)
        {
            if (obj != null)
            {
                try
                {
                    SelectedTeacherCurriculum = (BindableAppointmentView.TeacherCurriculumListView)obj;
                    AppointmentAvailableTimeData = await ApiHelper.GetObject<AppointmentAvailableTimeView>(string.Format(TextResource.AppointmentAvailableTimeApiUrl, SelectedTeacherCurriculum.AppointmentSettingsId, SelectedTeacherCurriculum.TeacherId, AppSettings.Current.SelectedStudent.ItemId));
                    AppointmentDateList = AppointmentAvailableTimeData.appointmentAvailableTimesList.GroupBy(x => x.BookingDateFormatted)
                        .Select(dt => dt.FirstOrDefault()).ToList();

                    if (AppointmentDateList != null && AppointmentDateList.Count() > 0)
                    {
                        var requestAppointmentPopup = new RequestAppointmentPopup()
                        {
                            BindingContext = this
                        };
                        SetPopupInstance(requestAppointmentPopup);
                        Application.Current.MainPage.ShowPopup(requestAppointmentPopup);
                    }
                    else
                    {
                        await HelperMethods.ShowAlert(this.PageTitle, TextResource.NoAppointmentDateMessage);
                    }
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                }
            }
        }
        public void SetPopupInstance(Popup popup)
        {
            AppSettings.Current.CurrentPopup = popup;
        }
        private async Task<BindableAppointmentView> GetTeacherAppointment()
        {
            try
            {
                AppointmentViewData = await ApiHelper.GetObject<BindableAppointmentView>(string.Format(TextResource.TeacherAppointmentApiUrl, AppSettings.Current.SelectedStudent.ItemId),
                    cacheKeyPrefix:"teacherappointment", cacheType: AppSettings.Current.RefreshTeacherAppointmentList ? ApiHelper.CacheTypeParam.LoadFromServerAndCache : ApiHelper.CacheTypeParam.LoadFromCache);
                TeacherAppointmentList = new ObservableCollection<BindableAppointmentView.TeacherAppointmentListView>(AppointmentViewData.TeacherAppointmentList);
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    AppSettings.Current.RefreshTeacherAppointmentList = false;
                TeacherCurriculumList = new ObservableCollection<BindableAppointmentView.TeacherCurriculumListView>(AppointmentViewData.TeacherCurriculumList);
                if (TeacherAppointmentList != null && TeacherAppointmentList.Count > 0 && !string.IsNullOrEmpty(NotificationItemId))
                {
                    BindableAppointmentView.TeacherAppointmentListView appointmentView = TeacherAppointmentList.Where(x => x.AppointmentBookingId == Convert.ToInt32(NotificationItemId)).FirstOrDefault();
                    if (appointmentView != null)
                        CommentClicked(appointmentView);
                    NotificationItemId = null;
                }
                NoAppointmentDataExist = AppointmentViewData.TeacherCurriculumList.Count == 0;
                NoTeacherAppointmentExist = TeacherAppointmentList.Count == 0;
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.TeacherAppointmentTitle);
            }
            return AppointmentViewData;
        }
        private async void DeleteClicked(object obj)
        {
            if (obj != null)
            {
                try
                {
                    var selectedAppointment = (BindableAppointmentView.TeacherAppointmentListView)obj;
                    var action = await App.Current.MainPage.DisplayAlert("", TextResource.DeleteText, TextResource.YesText, TextResource.NoText);
                    if (action)
                    {
                        OperationDetails result = await ApiHelper.DeleteRequest<OperationDetails>(string.Format(TextResource.TeacherAppointmentDeleteApiUrl, selectedAppointment.AppointmentBookingId));
                        if (result.Success)
                        {
                            TeacherAppointmentList.Remove(selectedAppointment);
                            MessagingCenter.Send<OperationDetails>(result, "RefreshData");
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
        }
        private async void CommentClicked(object obj)
        {
            if (obj != null)
            {
                var selectedAppointment = (BindableAppointmentView.TeacherAppointmentListView)obj;
                AppointmentCommentForm appointmentCommentForm = new(_mapper, _nativeServices, Navigation)
                {
                    SelectedTeacherAppointment = selectedAppointment,
                    BackVisible = true,
                    MenuVisible = false,
                    ParentComments = selectedAppointment.ParentComments,
                    IsCommentsErrVisible = false,
                    PageTitle = TextResource.AppointmentComments
                };
                AppointmentComment appointmentComment = new ()
                {
                    BindingContext = appointmentCommentForm
                };
                await Navigation.PushAsync(appointmentComment);
                SelectedTeacherAppointment = null;
            }
        }
        private async void TimeSlotSelected(object obj)
        {
            try
            {
                var action = await App.Current.MainPage.DisplayAlert("", string.Format(TextResource.AppointmentConfirmationMessage, SelectedDate.BookingDateFormatted, SelectedTime.TimeSlot.ToDateTime().ToString("hh:mm tt"), SelectedTeacherCurriculum.TeacherName,Environment.NewLine), TextResource.YesText, TextResource.NoText);
                if (action)
                {
                    await BookAppointment();
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        private async Task<bool> BookAppointment()
        {
            try
            {
                OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.BookTeacherAppointmentApiUrl,
                        SelectedTeacherCurriculum.TeacherId, AppSettings.Current.SelectedStudent.ItemId, SelectedDate.BookingDate.ToPickerDateFormatted(), SelectedTime.TimeSlot,
                        SelectedTeacherCurriculum.AppointmentSettingsId, SelectedTeacherCurriculum.TeacherName),
                    AppSettings.Current.ApiUrl);
                if (result.Success)
                {
                    await HelperMethods.ShowAlert("", TextResource.AppointmentUpdatedSuccess);
                    MessagingCenter.Send<BindableAppointmentView.TeacherCurriculumListView>(SelectedTeacherCurriculum, "CreateAppointment");
                    AppSettings.Current.CurrentPopup?.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override async void GetStudentData()
        {
            base.GetStudentData();
            await GetTeacherAppointment();
        }

        #endregion
    }