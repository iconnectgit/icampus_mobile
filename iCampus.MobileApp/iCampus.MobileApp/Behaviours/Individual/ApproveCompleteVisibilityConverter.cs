using System.Globalization;
using iCampus.Portal.ViewModels;

namespace iCampus.MobileApp.Behaviours.Individual;

public class ApproveCompleteVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        AppointmentBookingView appointmentBookingView = new AppointmentBookingView();
        if (value!=null)
            appointmentBookingView = (AppointmentBookingView)value;

        if (parameter !=null && System.Convert.ToInt32(parameter) == 1)
        {
            return appointmentBookingView.IsActionButtonVisible && !appointmentBookingView.IsApproved;
        }
        else if (parameter != null && System.Convert.ToInt32(parameter) == 2)
        {
            return appointmentBookingView.IsActionButtonVisible && appointmentBookingView.IsApproved && !appointmentBookingView.IsCompleted;
        }
        return false;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return false;
    }
}