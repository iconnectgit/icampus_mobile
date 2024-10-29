using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class BooleanToFontWeightConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? FontAttributes.Bold : FontAttributes.None;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}