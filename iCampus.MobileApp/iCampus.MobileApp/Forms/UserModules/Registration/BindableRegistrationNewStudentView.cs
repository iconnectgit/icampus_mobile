using System.ComponentModel;
using iCampus.Common.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Registration;

	public class BindableRegistrationNewStudentView : RegistrationNewStudentView, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isEditDeleteButtonVisible;
        public bool IsEditDeleteButtonVisible
        {
            get
            {
                return _isEditDeleteButtonVisible;
            }
            set
            {
                _isEditDeleteButtonVisible = value;
                OnPropertyChanged("IsEditDeleteButtonVisible");
            }
        }
        private bool _isKGInformationButtonVisible;
        public bool IsKGInformationButtonVisible
        {
            get
            {
                return _isKGInformationButtonVisible;
            }
            set
            {
                _isKGInformationButtonVisible = value;
                OnPropertyChanged("IsKGInformationButtonVisible");
            }
        }
        private string _kgInformationButtonCaption;
        public string KgInformationButtonCaption
        {
            get
            {
                return _kgInformationButtonCaption;
            }
            set
            {
                _kgInformationButtonCaption = value;
                OnPropertyChanged("KgInformationButtonCaption");
            }
        }
        
        private bool _detailsVisibility;
        public bool DetailsVisibility
        {
            get
            {
                return _detailsVisibility;
            }
            set
            {
                _detailsVisibility = value;
                OnPropertyChanged("DetailsVisibility");
            }
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
        private bool _isPerStudentAppointmentButtonVisible;
        public bool IsPerStudentAppointmentButtonVisible
        {
            get
            {
                return _isPerStudentAppointmentButtonVisible;
            }
            set
            {
                _isPerStudentAppointmentButtonVisible = value;
                OnPropertyChanged("IsPerStudentAppointmentButtonVisible");
            }
        }
        private bool _isBookingDateButtonVisible;
        public bool IsBookingDateButtonVisible
        {
            get
            {
                return _isBookingDateButtonVisible;
            }
            set
            {
                _isBookingDateButtonVisible = value;
                OnPropertyChanged("IsBookingDateButtonVisible");
            }
        }
        private bool _isCancelButtonVisible;
        public bool IsCancelButtonVisible
        {
            get
            {
                return _isCancelButtonVisible;
            }
            set
            {
                _isCancelButtonVisible = value;
                OnPropertyChanged("IsCancelButtonVisible");
            }
        }
        public BindableRegistrationNewStudentView()
		{
		}
	}
