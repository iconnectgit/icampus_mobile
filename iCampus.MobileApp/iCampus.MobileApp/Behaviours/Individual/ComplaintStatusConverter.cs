using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

public class ComplaintStatusConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
            return value.ToString().ToLower().Equals("open") ? "Pending" : value.ToString();
        else
            return string.Empty;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}