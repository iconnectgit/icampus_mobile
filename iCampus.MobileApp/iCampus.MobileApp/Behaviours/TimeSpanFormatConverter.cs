using System.Globalization;
using iCampus.Common.Helpers.Extensions;

namespace iCampus.MobileApp.Behaviours;

public class TimeSpanFormatConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            var val = (TimeSpan)value;
            return val.ToDateTime().ToString("hh:mm tt");
        }
        catch(Exception ex)
        {
            return string.Empty;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}