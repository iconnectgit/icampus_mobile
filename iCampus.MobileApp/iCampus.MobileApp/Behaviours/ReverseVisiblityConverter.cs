using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class ReverseVisiblityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null && !string.IsNullOrWhiteSpace(value.ToString()) ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return false;
    }
}