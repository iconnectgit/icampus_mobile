using System.ComponentModel;
using iCampus.MobileApp.DependencyService;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.BooksReservation;

public class BindableAppointmentAvailableTimeView : INotifyPropertyChanged
    {
		public int AppointmentSettingsId
		{
			get;
			set;
		}

		public DateTime BookingDate
		{
			get;
			set;
		}

		public string BookingDateFormatted
		{
			get;
			set;
		}

		public int TimePerVisit
		{
			get;
			set;
		}
		public TimeSpan TimeSlot
		{
			get;
			set;
		}
		string _formattedTimeSlot;
		public string FormattedTimeSlot
		{
			get
            {
				if(TimeSlot!=null)
                {
					DateTime time = DateTime.Today.Add(TimeSlot);
					string displayTime = time.ToString("hh:mm tt");
					_formattedTimeSlot = displayTime;
				}
				return _formattedTimeSlot;
            }
			set
            {
				_formattedTimeSlot = value;
				OnPropertyChanged("FormattedTimeSlot");
			}
		}

		public AppointmentLock appointmentLockSettings
		{
			get;
			set;
		}

		public IEnumerable<AppointmentAvailableTimeView> appointmentAvailableTimesList
		{
			get;
			set;
		}
		private string _arrowImageSource = "dropdown_gray.png";
		public string ArrowImageSource
		{
			get
			{
				return _arrowImageSource;
			}
			set
			{
				_arrowImageSource = value;
				OnPropertyChanged("ArrowImageSource");
			}
		}
		private string _date;
		public string Date
		{
			get
			{
				return _date;
			}
			set
			{
				_date = value;
				OnPropertyChanged("Date");
			}
		}
		private List<BindableTimeSlotClass> _timeSlotList;
		public List<BindableTimeSlotClass> TimeSlotList
		{
			get
			{
				return _timeSlotList;
			}
			set
			{
				_timeSlotList = value;
				OnPropertyChanged("TimeSlotList");
			}
		}
		private bool _descriptionVisibility;
		public bool DescriptionVisibility
		{
			get
			{
				return _descriptionVisibility;
			}
			set
			{
				_descriptionVisibility = value;
				OnPropertyChanged("DescriptionVisibility");
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
				OnPropertyChanged("TimeViewMargin");
			}
		}
		double _timeSlotListviewHeight;
		public double TimeSlotListviewHeight
		{
			get
			{
				_timeSlotListviewHeight = TimeSlotList != null && TimeSlotList.Count > 0 ? (TimeSlotList.Count % 3 == 0 ? (TimeSlotList.Count / 3) * 70 : (TimeSlotList.Count / 3) * 70) + 70 : TimeSlotListviewHeight;
				return _timeSlotListviewHeight;
			}
			set
			{
				_timeSlotListviewHeight = value;
				OnPropertyChanged("TimeSlotListviewHeight");
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }