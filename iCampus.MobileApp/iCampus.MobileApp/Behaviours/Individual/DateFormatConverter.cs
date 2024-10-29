using System.Globalization;
using iCampus.Common.Helpers.Extensions;

namespace iCampus.MobileApp.Behaviours.Individual;

public class DateFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            if (value != null)
            {
                string text = string.Empty;
                string dateTime = value.ToDateTime().ToString("dddd, MMMM dd, yyyy");
                return parameter?.ToString() + " " + dateTime;
            }
            else
                return DateTime.MinValue;
        }
        catch (Exception ex)
        {
            //Crashes.TrackError(ex);
            return DateTime.MinValue;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}