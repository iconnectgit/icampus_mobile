using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class DateChangedEventConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var eventArgs = value as DatePicker;
        if (eventArgs != null)
        {
            return null;
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}