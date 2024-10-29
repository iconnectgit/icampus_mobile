using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Enums;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Appointment;

public class FamilyAppointmentDetailForm : ViewModelBase
    {
        public ICommand SubmitClickCommand { get; set; }
        public ICommand AppointmentStatusClickCommand { get; set; }
        AppointmentBookingView _familyAppointmentObject = new AppointmentBookingView();
        public AppointmentBookingView FamilyAppointmentObject
        {
            get => _familyAppointmentObject;
            set
            {
                _familyAppointmentObject = value;
                OnPropertyChanged(nameof(FamilyAppointmentObject));
            }
        }
        public FamilyAppointmentDetailForm(IMapper mapper, INativeServices nativeServices, INavigation navigation) : base(mapper, null, null)
        {
            _mapper = mapper;
            _nativeServices = nativeServices;
            Navigation = navigation;
            InitializePage();
        }

        private void InitializePage()
        {
            BackVisible = true;
            BackTitle = TextResource.BackTitle;
            PageTitle = TextResource.FamilyAppointmentPageTitle;
            SubmitClickCommand = new Command<AppointmentBookingView>(SubmitAppointmentComments);
            AppointmentStatusClickCommand = new Command<AppointmentBookingView>(SaveAppointmentStatus);
            MessagingCenter.Subscribe<string>("", "ScrollViewRightSwipeFamilyAppointmentDetailsSubscribe", (arg) =>
            {
                MessagingCenter.Subscribe<string>("", "ScrollViewRightSwipeFamilyAppointmentDetail", async (argFamilyppointment) =>
                {
                    //await SideMenuClicked();
                });

            });
        }

        private async void SaveAppointmentStatus(AppointmentBookingView obj)
        {
            try
            {
                short statusId = FamilyAppointmentObject.IsApproved ? (short)AppointmentStatus.Completed : (short)AppointmentStatus.Approved;
                OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.UpdateFamilyAppointmentStatusUrl, FamilyAppointmentObject.AppointmentBookingId, statusId), AppSettings.Current.ApiUrl);
                if (result.Success)
                {
                    FamilyAppointmentObject = await GetUpdatedAppointment();
                    await Navigation.PopAsync();
                    MessagingCenter.Send(FamilyAppointmentObject, "FamilyAppointmentStatusUpdated");
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.FamilyAppointmentPageTitle);
            }
        }

        private async Task<AppointmentBookingView> GetUpdatedAppointment()
        {
            var appointmentList = await ApiHelper.GetObjectList<AppointmentBookingView>(TextResource.FamilyAppointmentUrl + "?status=null");
            return appointmentList.Where(e => e.AppointmentBookingId == FamilyAppointmentObject.AppointmentBookingId).FirstOrDefault();
        }

        private async void SubmitAppointmentComments(AppointmentBookingView obj)
        {
            try
            {
                OperationDetails result = await ApiHelper.PostRequest<OperationDetails>(string.Format(TextResource.UpdateFamilyAppointmentCommentUrl, FamilyAppointmentObject.AppointmentBookingId, FamilyAppointmentObject.TeacherComments), AppSettings.Current.ApiUrl);
                if (result.Success)
                {
                    MessagingCenter.Send(FamilyAppointmentObject, "FamilyAppointmentStatusUpdated");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                HelperMethods.DisplayException(ex, TextResource.FamilyAppointmentPageTitle);
            }

        }
    }