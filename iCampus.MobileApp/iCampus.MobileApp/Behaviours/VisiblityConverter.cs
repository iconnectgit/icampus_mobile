using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class VisiblityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Check if the value is not null or whitespace, and return true if it has content
        return !string.IsNullOrWhiteSpace(System.Convert.ToString(value));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true; // Typically not used for one-way bindings
    }
}