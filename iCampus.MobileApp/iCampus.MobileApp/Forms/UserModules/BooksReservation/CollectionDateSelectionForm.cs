using System.Collections.ObjectModel;
using System.Globalization;
using System.Web;
using System.Windows.Input;
using AutoMapper;
using CommunityToolkit.Maui.Views;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.BooksReservation;
using iCampus.Portal.ViewModels;
using Newtonsoft.Json;

namespace iCampus.MobileApp.Forms.UserModules.BooksReservation;

public class CollectionDateSelectionForm : ViewModelBase
    {
        #region Declarations
        public ICommand ArrowClickedCommand { get; set; }
        public ICommand SaveAndConfirmClickedCommand { get; set; }


        #endregion Declaration
        #region Properties
        ObservableCollection<BindableAppointmentAvailableTimeView> _availableTimeList = new ObservableCollection<BindableAppointmentAvailableTimeView>();
        public ObservableCollection<BindableAppointmentAvailableTimeView> AvailableTimeList
        {
            get => _availableTimeList;
            set
            {
                _availableTimeList = value;
                OnPropertyChanged(nameof(AvailableTimeList));
            }
        }
        string _selectedTimeSlot;
        public string SelectedTimeSlot
        {
            get
            {
                return _selectedTimeSlot;
            }
            set
            {
                _selectedTimeSlot = value;
                OnPropertyChanged(nameof(SelectedTimeSlot));
            }
        }
        string _selectedDateTime;
        public string SelectedDateTime
        {
            get
            {
                return _selectedDateTime;
            }
            set
            {
                _selectedDateTime = value;
                OnPropertyChanged(nameof(SelectedDateTime));
            }
        }
        bool _selectedDateTimeVisibility=false;
        public bool SelectedDateTimeVisibility
        {
            get
            {
                return _selectedDateTimeVisibility;
            }
            set
            {
                _selectedDateTimeVisibility = value;
                OnPropertyChanged(nameof(SelectedDateTimeVisibility));
            }
        }
        string _selectedDate;
        public string SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        TimeSpan _selectedTime;
        public TimeSpan SelectedTime
        {
            get
            {
                return _selectedTime;
            }
            set
            {
                _selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }
        ObservableCollection<BindableStudentBookMasterView> _allStudentBooksList = new ObservableCollection<BindableStudentBookMasterView>();
        public ObservableCollection<BindableStudentBookMasterView> AllStudentBooksList
        {
            get
            {
                return _allStudentBooksList;
            }
            set
            {
                _allStudentBooksList = value;
                OnPropertyChanged(nameof(AllStudentBooksList));
            }        }
        bool _isNoRecordMsg = true;
        public bool IsNoRecordMsg
        {
            get => _isNoRecordMsg;
            set
            {
                _isNoRecordMsg = value;
                OnPropertyChanged(nameof(IsNoRecordMsg));
            }
        }
        ObservableCollection<BindableTimeSlotClass> _timeSlots = new ObservableCollection<BindableTimeSlotClass>();
        public ObservableCollection<BindableTimeSlotClass> TimeSlots
        {
            get => _timeSlots;
            set
            {
                _timeSlots = value;
                OnPropertyChanged(nameof(TimeSlots));
            }
        }
        Thickness _timeViewMargin;
        public Thickness TimeViewMargin
        {
            get
            {
                return _timeViewMargin;
            }
            set
            {
                _timeViewMargin = value;
                OnPropertyChanged(nameof(TimeViewMargin));
            }
        }
        #endregion Properties
        public CollectionDateSelectionForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }
        #region Methods
        async void InitializePage()
        {
            ArrowClickedCommand = new Command<BindableAppointmentAvailableTimeView>(ArrowClicked);
            SaveAndConfirmClickedCommand = new Command(SaveAndConfirmClicked);
            BeamMenuClickCommand = new Command(BeamMenuClicked);
            BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
            BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
            BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);

            MessagingCenter.Subscribe<string>("", "SelectCollectionDatePageSubscribe", (arg) =>
            {
                MessagingCenter.Subscribe<string>("", "SelectCollectionDatePageSwipe", async (collectionDateSelectionFormArg) =>
                {
                    //await SideMenuClicked();
                });
            });
            MessagingCenter.Subscribe<string>("", "GetSelectedTimeSlot", (arg) =>
            {
                try
                {
                    if (AvailableTimeList != null)
                    {
                        AvailableTimeList.ToList().ForEach(x => x.TimeSlotList.ForEach(y => y.SelectedTimeSlotBackgroundColor = Colors.Transparent));
                        TimeSlots.Where(x =>x.Date!=arg).ToList().ForEach(y => y.SelectedTimeSlotBackgroundColor = Colors.Transparent); 
                        SelectedDateTime = String.Concat(AvailableTimeList.Where(x => x.DescriptionVisibility)?.FirstOrDefault()?.Date, " - ", arg);
                        SelectedDateTimeVisibility = true;
                        DateTime dateTime = DateTime.ParseExact(arg, "hh:mm tt", CultureInfo.InvariantCulture);
                        SelectedTime = dateTime.TimeOfDay;
                        DateTime selectedDate = Convert.ToDateTime(AvailableTimeList.Where(x => x.DescriptionVisibility)?.FirstOrDefault()?.Date);
                        SelectedDate = selectedDate.ToString("MM/dd/yyyy");
                        AvailableTimeList.ToList().ForEach(x => x.DescriptionVisibility = false);
                        AvailableTimeList.ToList().ForEach(x => x.ArrowImageSource = "dropdown_gray.png");
                    }
                }
                catch (Exception ex)
                {
                    HelperMethods.DisplayException(ex, this.PageTitle);
                    //Crashes.TrackError(ex);
                }
            });
            MessagingCenter.Subscribe<string>("", "TimeSlotPopupClosed", (arg) =>
            {
                AvailableTimeList.ToList().ForEach(x => x.DescriptionVisibility = false);
                AvailableTimeList.ToList().ForEach(x => x.ArrowImageSource = "dropdown_gray.png");
            });
            }
        async void ArrowClicked(BindableAppointmentAvailableTimeView obj)
        {
            foreach (var item in AvailableTimeList.ToList())
            {
                if (item != null)
                {

                    if (item.Date == obj.Date)
                    {
                        item.DescriptionVisibility = !item.DescriptionVisibility;
                        item.ArrowImageSource = item.ArrowImageSource.Equals("uparrow_gray.png") ? "dropdown_gray.png" : "uparrow_gray.png";
                        if (obj.TimeSlotList != null && obj.TimeSlotList.Count > 0)
                        {
                            TimeSlots = new ObservableCollection<BindableTimeSlotClass>();
                            foreach (var time in obj.TimeSlotList)
                            {
                                if (time != null)
                                {
                                    TimeSlots.Add(time);
                                }
                            }
                        }
                        TimeSlots.ToList().ForEach(x => x.Date = obj.Date);
                        TimeSlotListPopup timeSlotListPopup = new ()
                        {
                            BindingContext = this
                        };
                        Application.Current.MainPage.ShowPopup(timeSlotListPopup);
                    }
                }
            }
            MessagingCenter.Send("", "ExpandCollapse");
        }
        async void SaveAndConfirmClicked()
        {
            try
            {
                List<StudentBookReservationDetailView> list = new List<StudentBookReservationDetailView>();
                foreach (var item in AllStudentBooksList)
                {
                    if (item != null && item.IsChecked)
                    {
                        list.Add(new StudentBookReservationDetailView() { StudentId = item.StudentId.ToString(), StudentBookMasterId = item.StudentBookMasterId.ToString(), Title = item.Title, Quantity = item.Quantity, Price = item.Price, Amount = item.Price });
                    }
                }
                string json = JsonConvert.SerializeObject(list);
                var apiUrl = string.Format(TextResource.BooksReservationApiUrl, SelectedDate, SelectedTime, HttpUtility.UrlEncode(json));
                OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(apiUrl);
                if (result.Success)
                {
                    BackClicked(false);
                    MessagingCenter.Send("", "RefreshBooksReservationPage");
                    Application.Current.MainPage.ShowPopup(new BookingConfirmationPopup());
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
                //Crashes.TrackError(ex);
            }
        }
        #endregion Methods
    }