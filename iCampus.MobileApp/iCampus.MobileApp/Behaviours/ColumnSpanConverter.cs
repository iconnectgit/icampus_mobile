using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class ColumnSpanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isVisible)
            return isVisible ? 1 : 2; 
        return 2; 
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}