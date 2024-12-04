using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using iCampus.Common.Helpers;
using iCampus.MobileApp.DependencyService;
using iCampus.MobileApp.Helpers;
using iCampus.MobileApp.Views.UserModules.Appointment;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Forms.UserModules.Appointment;

public class FamilyAppointmentForm : ViewModelBase
{
    #region Declaration

    public ICommand ApproveClickCommand { get; set; }
    public ICommand CompleteClickCommand { get; set; }
    public ICommand FamilyAppointmentListTappedCommand { get; set; }

    #endregion

    #region Properties

    private IList<AppointmentBookingView> _familyAppointmentList = new List<AppointmentBookingView>();

    public IList<AppointmentBookingView> FamilyAppointmentList
    {
        get => _familyAppointmentList;
        set
        {
            _familyAppointmentList = value;
            OnPropertyChanged(nameof(FamilyAppointmentList));
        }
    }

    private ObservableCollection<AppointmentBookingView> _bindableFamilyAppointmentList = new();

    public ObservableCollection<AppointmentBookingView> BindableFamilyAppointmentList
    {
        get => _bindableFamilyAppointmentList;
        set
        {
            _bindableFamilyAppointmentList = value;
            OnPropertyChanged(nameof(BindableFamilyAppointmentList));
        }
    }

    private AppointmentBookingView _selectedAppointment = new();

    public AppointmentBookingView SelectedAppointment
    {
        get => _selectedAppointment;
        set
        {
            _selectedAppointment = value;
            OnPropertyChanged(nameof(SelectedAppointment));
        }
    }

    private bool _isAppointmentAvailable = false;

    public bool IsAppointmentsAvailable
    {
        get => _isAppointmentAvailable;
        set
        {
            _isAppointmentAvailable = value;
            OnPropertyChanged(nameof(IsAppointmentsAvailable));
        }
    }

    #endregion

    public FamilyAppointmentForm(IMapper mapper, INativeServices nativeServices, INavigation navigation, string notificationItemId = null) : base(mapper, null, null)
    {
        _mapper = mapper;
        _nativeServices = nativeServices;
        Navigation = navigation;
        NotificationItemId = notificationItemId;
        if(!string.IsNullOrEmpty(NotificationItemId))
            AppSettings.Current.RefreshFamilyAppointmentList = true;
        InitializePage();
    }

    public FamilyAppointmentForm(string notificationItemId) : base(null, null, null)
    {
        NotificationItemId = notificationItemId;
        AppSettings.Current.RefreshFamilyAppointmentList = true;
        GetFamilyAppointmentList();
    }

    private async void InitializePage()
    {
        MenuVisible = true;
        FamilyAppointmentListTappedCommand = new Command<AppointmentBookingView>(AppointmentListTappedCommand);
        ApproveClickCommand = new Command<AppointmentBookingView>(ApproveClicked);
        CompleteClickCommand = new Command<AppointmentBookingView>(CompleteClicked);
        BeamMenuClickCommand = new Command(BeamMenuClicked);
        BeamHeaderMessageIconClickCommand = new Command(BeamHeaderMessageIconClicked);
        BeamHeaderNotificationIconClickCommand = new Command(BeamHeaderNotificationIconClicked);
        BeamHeaderStudentImageClickCommand = new Command(StudentViewTapClicked);
        await GetFamilyAppointmentList();
        MessagingCenter.Subscribe<AppointmentBookingView>(this, "FamilyAppointmentStatusUpdated",
            async (updatedAppointment) =>
            {
                if (updatedAppointment != null)
                {
                    var obj = FamilyAppointmentList
                        .Where(i => i.AppointmentBookingId == updatedAppointment.AppointmentBookingId).FirstOrDefault();
                    var index = FamilyAppointmentList.IndexOf(obj);
                    FamilyAppointmentList[index] = updatedAppointment;
                    BindableFamilyAppointmentList =
                        new ObservableCollection<AppointmentBookingView>(FamilyAppointmentList);
                    AppSettings.Current.RefreshFamilyAppointmentList = true;
                    await GetFamilyAppointmentList();
                }
            });
    }

    #region Private methods

    private async void AppointmentListTappedCommand(AppointmentBookingView obj)
    {
        if (obj != null)
        {
            FamilyAppointmentDetailForm appointmentDetailForm = new(_mapper, _nativeServices, Navigation)
            {
                FamilyAppointmentObject = obj
            };
            FamilyAppointmentDetail familyAppointmentDetail = new()
            {
                BindingContext = appointmentDetailForm
            };
            await Navigation.PushAsync(familyAppointmentDetail);
            SelectedAppointment = null;
        }
    }

    private async Task<IList<AppointmentBookingView>> GetFamilyAppointmentList()
    {
        try
        {
            FamilyAppointmentList = await ApiHelper.GetObjectList<AppointmentBookingView>(
                TextResource.FamilyAppointmentUrl + "?status=null", cacheKeyPrefix: "teacherappointment",
                cacheType: AppSettings.Current.RefreshFamilyAppointmentList
                    ? ApiHelper.CacheTypeParam.LoadFromServerAndCache
                    : ApiHelper.CacheTypeParam.LoadFromCache);
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                AppSettings.Current.RefreshFamilyAppointmentList = false;
            BindableFamilyAppointmentList = new ObservableCollection<AppointmentBookingView>(FamilyAppointmentList);
            if (BindableFamilyAppointmentList != null && BindableFamilyAppointmentList.Count > 0 && !string.IsNullOrEmpty(NotificationItemId))
            {
                var appointmentView = BindableFamilyAppointmentList
                    .Where(x => x.AppointmentBookingId == Convert.ToInt32(NotificationItemId)).FirstOrDefault();
                if (appointmentView != null)
                    AppointmentListTappedCommand(appointmentView);
                NotificationItemId = null;
            }

            IsAppointmentsAvailable = !FamilyAppointmentList.Any();
            return FamilyAppointmentList;
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
            return new List<AppointmentBookingView>();
        }
    }

    private async void ApproveClicked(AppointmentBookingView obj)
    {
        try
        {
            var action = await Application.Current.MainPage.DisplayAlert("",
                string.Format(TextResource.ApproveConfirmationMessage, obj.BookingDateTimeFormatted, obj.StudentName),
                TextResource.YesText, TextResource.NoText);
            if (action)
            {
                var operationDetails = await ApiHelper.PostRequest<OperationDetails>(
                    string.Format(TextResource.FamilyAppointmentStatusUpdateApiUrl, obj.AppointmentBookingId, 1));
                if (operationDetails.Success)
                {
                    await HelperMethods.ShowAlert("", TextResource.AppointmentApproveMessage);
                    AppSettings.Current.RefreshFamilyAppointmentList = true;
                    await GetFamilyAppointmentList();
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    private async void CompleteClicked(AppointmentBookingView obj)
    {
        try
        {
            var action = await Application.Current.MainPage.DisplayAlert("", TextResource.CompleteConfirmationMessage,
                TextResource.YesText, TextResource.NoText);
            if (action)
            {
                var operationDetails = await ApiHelper.PostRequest<OperationDetails>(
                    string.Format(TextResource.FamilyAppointmentStatusUpdateApiUrl, obj.AppointmentBookingId, 2));
                if (operationDetails.Success)
                {
                    await HelperMethods.ShowAlert("", TextResource.AppointmentCompleteMessage);
                    AppSettings.Current.RefreshFamilyAppointmentList = true;
                    await GetFamilyAppointmentList();
                }
            }
        }
        catch (Exception ex)
        {
            HelperMethods.DisplayException(ex, PageTitle);
        }
    }

    #endregion
}