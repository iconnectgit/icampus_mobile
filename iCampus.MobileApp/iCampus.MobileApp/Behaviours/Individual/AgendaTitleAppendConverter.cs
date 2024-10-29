using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

public class AgendaTitleAppendConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string title = string.Empty;
        if (value != null && !string.IsNullOrWhiteSpace(System.Convert.ToString(value)))
            title = " for " + value;

        return title;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}