using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Forms.PopupForms;
using iCampus.MobileApp.Forms.UserModules.Appointment;
using iCampus.MobileApp.Forms.UserModules.Calendar;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.PopUpViews;
using iCampus.MobileApp.Views.UserModules.Appointment;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.NotificationCenter;

public class NotificationCenterForm : ViewModelBase
    {
        #region Declarations
        private Popup _currentPopup;
        public ICommand ListTappedCommand { get; set; }
        public ICommand AttachmentClickCommand { get; set; }
        public ICommand LinksClickCommand { get; set; }
        public ICommand ArrowClickedCommand { get; set; }
        public ICommand AppointmentListTappedCommand { get; set; }


        #endregion Declarations

        #region Properties
        private string _agendaDate;
        public string AgendaDate
        {
            get => _agendaDate;
            set
            {
                _agendaDate = value;
                OnPropertyChanged(nameof(AgendaDate));
            }
        }
        ObservableCollection<BindableAgendaView> _agendaList = new ObservableCollection<BindableAgendaView>();
        public ObservableCollection<BindableAgendaView> AgendaList
        {
            get => _agendaList;
            set
            {
                _agendaList = value;
                OnPropertyChanged(nameof(AgendaList));
            }
        }
        ObservableCollection<Grouping<string, BindableAgendaView>> _groupedAgendaList;
        public ObservableCollection<Grouping<string, BindableAgendaView>> GroupedAgendaList
        {
            get => _groupedAgendaList;
            set
            {
                _groupedAgendaList = value;
                OnPropertyChanged(nameof(GroupedAgendaList));
            }
        }
        BindableAgendaView _weeklySelectedAgenda = new BindableAgendaView();
        public BindableAgendaView WeeklySelectedAgenda
        {
            get => _weeklySelectedAgenda;
            set
            {
                _weeklySelectedAgenda = value;
                OnPropertyChanged(nameof(WeeklySelectedAgenda));
            }
        }
        AgendaView _selectedAgendaType = new AgendaView();
        public AgendaView SelectedAgendaType
        {
            get => _selectedAgendaType;
            set
            {
                _selectedAgendaType = value;
                OnPropertyChanged(nameof(SelectedAgendaType));
            }
        }
        DashboardView _notificationCenterData = new DashboardView();
        public DashboardView NotificationCenterData
        {
            get => _notificationCenterData;
            set
            {
                _notificationCenterData = value;
                OnPropertyChanged(nameof(NotificationCenterData));
            }
        }
        List<AppointmentBookingView> _appointmentList = new List<AppointmentBookingView>();
        public List<AppointmentBookingView> AppointmentList
        {
            get => _appointmentList;
            set
            {
                _appointmentList = value;
                OnPropertyChanged(nameof(AppointmentList));
            }
        }
        AppointmentBookingView _selectedAppointment = new AppointmentBookingView();
        public AppointmentBookingView SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {
                _selectedAppointment = value;
                OnPropertyChanged(nameof(SelectedAppointment));
            }
        }
        bool _noDataExist;
        public bool NoDataExist
        {
            get => _noDataExist;
            set
            {
                _noDataExist = value;
                OnPropertyChanged(nameof(NoDataExist));
            }
        }
        private int _appointmentListViewHeight;
        public int AppointmentListViewHeight
        {
            get => _appointmentListViewHeight;
            set
            {
                _appointmentListViewHeight = value;
                OnPropertyChanged(nameof(AppointmentListViewHeight));
            }
        }
        bool _appintmentText;
        public bool AppointmentText
        {
            get => _appintmentText;
            set
            {
                _appintmentText = value;
                OnPropertyChanged(nameof(AppointmentText));
            }
        }
        private IList<BindableAttachmentFileView> _selectedAttachmentList;
        public IList<BindableAttachmentFileView> SelectedAttachmentList
        {
            get => _selectedAttachmentList;
            set
            {
                _selectedAttachmentList = value;
                OnPropertyChanged(nameof(SelectedAttachmentList));
            }
        }
        #endregion Properties
        public NotificationCenterForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
             MessagingCenter.Subscribe<string>("", "ListViewRightSwipeNotificationCenter", async (argumentNotificationCenter) =>
                {
                    //await SideMenuClicked();
                });
            MessagingCenter.Subscribe<AppointmentBookingView>(this, "FamilyAppointmentStatusUpdated", async (updatedAppointment) =>
            {
                if (updatedAppointment != null)
                {
                    AppSettings.Current.RefreshNotificationCenterData = true;
                    GetNotificationData();
                }
            });
            ListTappedCommand = new Command<BindableAgendaView>(ListViewTapped);
            AttachmentClickCommand = new Command<BindableAgendaView>(AttachmentClicked);
            LinksClickCommand = new Command<BindableAgendaView>(LinksClicked);
            ArrowClickedCommand = new Command<BindableAgendaView>(ArrowClicked);
            AppointmentListTappedCommand = new Command<AppointmentBookingView>(AppointmentListTappedMethod);
        }
        #region Methods

        public async void GetNotificationData()
        {
            try
            {
                string userID = AppSettings.Current.IsParent ? AppSettings.Current.SelectedStudent.ItemId : AppSettings.Current.UserRefId.ToString();
                NotificationCenterData = await Helpers.ApiHelper.GetObject<DashboardView>(TextResource.AssignmentDashboardDataAPIUrl,
                    cacheKeyPrefix: "notificationcenter", cacheType:AppSettings.Current.RefreshNotificationCenterData ? ApiHelper.CacheTypeParam.LoadFromServerAndCache : ApiHelper.CacheTypeParam.LoadFromCache);
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    AppSettings.Current.RefreshNotificationCenterData = false;
                if (NotificationCenterData != null)
                {
                    if (NotificationCenterData.AgendaUsers != null && NotificationCenterData.AgendaUsers.FirstOrDefault()?.AgendaList != null)
                    {
                        AgendaDate = NotificationCenterData.AgendaUsers.FirstOrDefault().AgendaList.FirstOrDefault().AgendaDate != null ? ((DateTime)NotificationCenterData.AgendaUsers.FirstOrDefault()?.AgendaList.FirstOrDefault()?.AgendaDate).ToString("dddd, MMMM dd, yyyy") : String.Empty;
                        if(AppSettings.Current.IsParent)
                        {
                            var list = NotificationCenterData.AgendaUsers.FirstOrDefault(x => x.UserRefId.ToString().Equals(userID))?.AgendaList;
                            if(list != null)
                            {
                                AgendaList = _mapper.Map<ObservableCollection<BindableAgendaView>>(list);
                            }
                            else
                            {
                                AgendaList = new ObservableCollection<BindableAgendaView>();
                                GroupedAgendaList = new ObservableCollection<Grouping<string, BindableAgendaView>>();
                            }
                        }
                        else
                            AgendaList = _mapper.Map<ObservableCollection<BindableAgendaView>>(NotificationCenterData.AgendaUsers.FirstOrDefault()?.AgendaList);
                    }
                    AppointmentList = NotificationCenterData.FamilyAppointmentList.ToList();
                    AppointmentListViewHeight = 80 * AppointmentList.Count;
                    AppointmentText = AppointmentList.Count > 0;
                }

                if (AgendaList != null && AgendaList.Count > 0)
                {
                    var grpList = from agenda in AgendaList
                                  group agenda by agenda.TypeTitle into agendaGroup
                                  select new Grouping<string, BindableAgendaView>(agendaGroup.Key, agendaGroup.Where(x => x.TypeTitle == agendaGroup.Key).FirstOrDefault(), agendaGroup);



                    GroupedAgendaList = new ObservableCollection<Grouping<string, BindableAgendaView>>(grpList);
                    if (GroupedAgendaList != null)
                    {
                        foreach (var item in GroupedAgendaList)
                        {
                            if (item != null)
                            {
                                foreach (var listItem in item)
                                {
                                    if (listItem != null)
                                    {
                                        if (string.IsNullOrEmpty(item.FirstOrDefault().CurriculumName))
                                        {
                                            listItem.AgendaDetailsVisibility = true;
                                        }
                                        else
                                        {
                                            listItem.AgendaDetailsVisibility = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                NoDataExist = ((GroupedAgendaList != null && GroupedAgendaList.Count > 0) || (AppointmentList != null && AppointmentList.Count > 0)) ? false : true;
                MenuVisible = false;
                BackVisible = true;
                IsPopUpPage = false;
               

            }
            catch (Exception ex)
            {
                await Helpers.ApiHelper.HideProcessingIndicatorPopup();
                //Crashes.TrackError(ex);
            }
        }
        
        void ListViewTapped(BindableAgendaView obj)
        {
            if (obj != null)
            {
                CalendarForm calendarForm = new (_mapper, _nativeServices, Navigation);
                calendarForm.ListViewTapped(obj);
                WeeklySelectedAgenda = null;
            }
        }
        private async void AppointmentListTappedMethod(AppointmentBookingView obj)
        {
            if (obj != null)
            {
                FamilyAppointmentDetailForm familyAppointmentDetailForm = new (_mapper, _nativeServices, Navigation)
                {
                    FamilyAppointmentObject = obj
                };
                SelectedAppointment = null;
                FamilyAppointmentDetail familyAppointmentDetail = new FamilyAppointmentDetail()
                {
                    BindingContext = familyAppointmentDetailForm
                };
                await Navigation.PushAsync(familyAppointmentDetail);
            }
        }
        private async void AttachmentClicked(BindableAgendaView sender)
        {
            AttachmentListPopupForm attachmentListPopupForm = new(_mapper, _nativeServices, Navigation)
            {
                SelectedAttachmentList = _mapper.Map<List<BindableAttachmentFileView>>(sender.AttachmentList)
            };
            var attachmentListPopup = new AttachmentListPopup()
            {
                BindingContext = attachmentListPopupForm
            };
            SetPopupInstance(attachmentListPopup);
            await Application.Current.MainPage.ShowPopupAsync(attachmentListPopup);
        }
        public void SetPopupInstance(Popup popup)
        {
            AppSettings.Current.CurrentPopup = popup;
        }

        private async void LinksClicked(BindableAgendaView sender)
        {
            // WebsiteLinksPopupForm popupForm = new WebsiteLinksPopupForm();
            // popupForm.SelectedWebsiteLinks = sender.WebsiteLinks;
            // await PopupNavigation.Instance.PushAsync(new WebsiteLinksPopup(popupForm), true);
        }
        async void ArrowClicked(BindableAgendaView bindableAgendaView)
        {
            if (bindableAgendaView != null)
            {
                foreach (var item in GroupedAgendaList.ToList())
                {
                    if (item != null)
                    {
                        foreach (var listItem in item)
                        {
                            if (listItem != null)
                            {
                                if (listItem.AgendaId == bindableAgendaView.AgendaId)
                                {
                                    listItem.AgendaDetailsVisibility = !listItem.AgendaDetailsVisibility;
                                    listItem.ArrowImageSource = listItem.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                                }
                            }
                            else
                            {
                                listItem.AgendaDetailsVisibility = false;
                                listItem.ArrowImageSource = "dropdown_gray.png";
                            }
                        }
                    }
                }
            }
            MessagingCenter.Send("", "ExpandCollapse");
        }

        public override async void GetStudentData()
        {
            base.GetStudentData();
            GetNotificationData();
        }
        #endregion
    }