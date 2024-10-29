using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;

namespace iCampus.MobileApp.Forms.UserModules.Appointment;

public class AppointmentCommentForm : ViewModelBase
    {
        #region Declarations
        public ICommand SubmitClickCommand { get; set; }
        #endregion
        #region Properties
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

        string _parentComments;
        public string ParentComments
        {
            get => _parentComments;
            set
            {
                _parentComments = value;
                OnPropertyChanged(nameof(ParentComments));
                IsCommentsErrVisible = string.IsNullOrWhiteSpace(ParentComments) ? true : false;
            }
        }

        bool _isCommentsErrVisible;
        public bool IsCommentsErrVisible
        {
            get => _isCommentsErrVisible;
            set
            {
                _isCommentsErrVisible = value;
                OnPropertyChanged(nameof(IsCommentsErrVisible));
            }
        }
        #endregion
        public AppointmentCommentForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            SubmitClickCommand = new Command(UpdateAppointComment);
        }

        private bool ValidateData()
        {
            IsCommentsErrVisible = string.IsNullOrWhiteSpace(ParentComments) ? true : false;
            return !IsCommentsErrVisible;
        }
        public async void UpdateAppointComment(object obj)
        {
            try
            {
                if (ValidateData())
                {
                    OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.UpdateTeacherAppointmentApiUrl, SelectedTeacherAppointment.AppointmentBookingId, ParentComments), AppSettings.Current.ApiUrl);
                    if (result.Success)
                    {
                        MessagingCenter.Send<OperationDetails>(result, "RefreshData");
                        await Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, this.PageTitle);
            }
        }
    }