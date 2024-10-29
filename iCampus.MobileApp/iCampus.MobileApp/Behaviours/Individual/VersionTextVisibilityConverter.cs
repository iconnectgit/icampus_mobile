using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

public class VersionTextVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
            return value.ToString().ToLower().Equals("about") ? true : false;
        else
            return false;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}