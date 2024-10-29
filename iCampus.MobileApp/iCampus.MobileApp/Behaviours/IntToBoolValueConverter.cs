using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class IntToBoolValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int intValue) 
        {
            return intValue > 0;
        }
        return false; 
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool boolValue && boolValue ? 1 : 0; 
    }
}