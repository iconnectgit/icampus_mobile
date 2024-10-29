using System.Globalization;
using iCampus.Common.Helpers.Extensions;

namespace iCampus.MobileApp.Behaviours;

public class DateTimeFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return string.Empty;
        try
        {
            DateTime dateValue = value.ToDateTime();
            string dateFormat = string.IsNullOrEmpty(System.Convert.ToString(parameter)) ? TextResource.DateFormatKey : System.Convert.ToString(parameter);
            return dateValue.ToString(dateFormat);
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