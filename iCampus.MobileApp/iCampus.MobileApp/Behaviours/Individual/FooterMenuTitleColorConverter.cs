using System.Globalization;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Behaviours.Individual;

public class FooterMenuTitleColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            // Assuming AppSettings.Current.Settings.ThemeColor is a valid color string
            return boolValue ? Color.FromArgb(AppSettings.Current.Settings.ThemeColor) : Color.FromArgb("#a5a6a7");
        }
        return Color.FromArgb("#a5a6a7"); // Default color if value is not a boolean
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty; // Typically not used for one-way bindings
    }
}