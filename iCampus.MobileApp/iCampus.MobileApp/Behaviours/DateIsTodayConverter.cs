using System.Globalization;
using iCampus.Common.Helpers.Extensions;

namespace iCampus.MobileApp.Behaviours;

public class DateIsTodayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            return value.ToDateTime().ToString("dd-MMM-yy") == DateTime.Today.ToString("dd-MMM-yy");
        }
        catch(Exception ex)
        {
            return false;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}