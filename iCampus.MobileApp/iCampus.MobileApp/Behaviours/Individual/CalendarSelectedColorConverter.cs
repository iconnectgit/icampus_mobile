using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

public class CalendarSelectedColorConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value != null && (bool)value) ? Colors.White : Color.FromArgb("#707070");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}