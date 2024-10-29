using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class ReverseBoolValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            // Reverse the boolean value
            return !boolValue;
        }
        return false; // Return false if value is not a boolean
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Typically not used for one-way bindings
        return true;
    }
}