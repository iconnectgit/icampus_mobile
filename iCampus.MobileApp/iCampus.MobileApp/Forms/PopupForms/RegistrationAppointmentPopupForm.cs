using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.Common.ViewModels;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels.Enums;

namespace iCampus.MobileApp.Forms.PopupForms;

public class RegistrationAppointmentPopupForm : ViewModelBase
	{
        #region Declaration
        public ICommand TimeSlotClickCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        #endregion
        #region Properties
        List<RegistrationAppointmentAvailableTimeView> _appointmentAvailableTimeData = new List<RegistrationAppointmentAvailableTimeView>();
        public List<RegistrationAppointmentAvailableTimeView> AppointmentAvailableTimeData
        {
            get => _appointmentAvailableTimeData;
            set
            {
                _appointmentAvailableTimeData = value;
                OnPropertyChanged(nameof(AppointmentAvailableTimeData));
            }
        }

        IEnumerable<RegistrationAppointmentAvailableTimeView> _appointmentDateList = new List<RegistrationAppointmentAvailableTimeView>();
        public IEnumerable<RegistrationAppointmentAvailableTimeView> AppointmentDateList
        {
            get => _appointmentDateList;
            set
            {
                _appointmentDateList = value;
                OnPropertyChanged(nameof(AppointmentDateList));
            }
        }

        IEnumerable<RegistrationAppointmentAvailableTimeView> _appointmentTimeList = new List<RegistrationAppointmentAvailableTimeView>();
        public IEnumerable<RegistrationAppointmentAvailableTimeView> AppointmentTimeList
        {
            get => _appointmentTimeList;
            set
            {
                _appointmentTimeList = value;
                OnPropertyChanged(nameof(AppointmentTimeList));
            }
        }

        RegistrationAppointmentAvailableTimeView _selectedDate = new RegistrationAppointmentAvailableTimeView();
        public RegistrationAppointmentAvailableTimeView SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                AppointmentTimeList = AppointmentAvailableTimeData.Where(x => x.BookingDateFormatted == SelectedDate.BookingDateFormatted).ToList();
            }
        }

        RegistrationAppointmentAvailableTimeView _selectedTime = new RegistrationAppointmentAvailableTimeView();
        public RegistrationAppointmentAvailableTimeView SelectedTime
        {
            get => _selectedTime;
            set
            {
                _selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }
        string _selectedDateTimeText;
        public string SelectedDateTimeText
        {
            get => _selectedDateTimeText;
            set
            {
                _selectedDateTimeText = value;
                OnPropertyChanged(nameof(SelectedDateTimeText));
            }
        }
        bool _selectedDateTimeTextVisible = false;
        public bool SelectedDateTimeTextVisible
        {
            get => _selectedDateTimeTextVisible;
            set
            {
                _selectedDateTimeTextVisible = value;
                OnPropertyChanged(nameof(SelectedDateTimeTextVisible));
            }
        }
        private long? _familyId;
        public long? FamilyId
        {
            get => _familyId;
            set
            {
                _familyId = value;
                OnPropertyChanged(nameof(FamilyId));
            }
        }
        private int _registrationId;
        public int RegistrationId
        {
            get => _registrationId;
            set
            {
                _registrationId = value;
                OnPropertyChanged(nameof(RegistrationId));
            }
        }
        #endregion
        public RegistrationAppointmentPopupForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(null, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            TimeSlotClickCommand = new Command(TimeSlotClickMethod);
            SaveCommand = new Command(SaveCommandMethod);
        }
        #region Methods
        private async void TimeSlotClickMethod(object obj)
        {
            try
            {
                string[] dateFormats = { "d-MMM-yyyy", "dd-MMM-yyyy" };
                if (DateTime.TryParseExact(SelectedDate.BookingDateFormatted, dateFormats, null, System.Globalization.DateTimeStyles.None, out DateTime bookingDate))
                {
                    string formattedBookingDate = bookingDate.ToString("dddd, MMMM dd, yyyy");
                    string formattedTimeSlot = new DateTime(SelectedTime.TimeSlot.Ticks).ToString("hh:mm tt");
                    SelectedDateTimeText = $"{formattedBookingDate} - {formattedTimeSlot}";
                    SelectedDateTimeTextVisible = true;
                }
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
                if (!string.IsNullOrEmpty(SelectedDate.BookingDateFormatted) && SelectedTime.TimeSlot != TimeSpan.Zero)
                {
                    var action = await App.Current.MainPage.DisplayAlert("", string.Format(TextResource.RegistrationAppointmentConfirmationMessage, SelectedDateTimeText), TextResource.YesText, TextResource.NoText);
                    if (action)
                    {
                        await BookAppointment();
                    }
                }
                else
                {
                    string alertMessage = string.IsNullOrEmpty(SelectedDate.BookingDateFormatted)
                                        ? "Please choose date and time for the appointment"
                                        : "Please choose time for the appointment";
                    await HelperMethods.ShowAlert("Alert!", alertMessage);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        
        private async Task BookAppointment()
        {
            try
            {
                OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.BookStudentRegistrationAppointmentApi,
                        FamilyId, "", RegistrationId, SelectedDate.RegistrationDateId, SelectedDate.BookingDate, SelectedTime.TimeSlot, "", BookingActions.New), AppSettings.Current.ApiUrl);
                
                await HelperMethods.ShowAlertWithAction("", result.Message, async () =>
                {
                    MessagingCenter.Send<string>("", "UpdateReregistration");
                    AppSettings.Current.CurrentPopup.Close();
                });
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
        #endregion
    }