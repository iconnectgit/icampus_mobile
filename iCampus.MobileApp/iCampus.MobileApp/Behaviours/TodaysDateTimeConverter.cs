using System.Globalization;
using iCampus.Common.Helpers.Extensions;

namespace iCampus.MobileApp.Behaviours;

public class TodaysDateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            if (value != null)
            {
                string dateTime = value.ToDateTime().ToString("dd-MMM-yy") == DateTime.Today.ToString("dd-MMM-yy") ? TextResource.TodaysDateTimeFormatKey : TextResource.DateFormatKey;
                return value.ToDateTime().ToString(dateTime);
            }
            else
                return DateTime.MinValue;
        }
        catch(Exception ex)
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