using System.Globalization;
using iCampus.MobileApp.Forms;

namespace iCampus.MobileApp.Behaviours.Individual;

public class CalendarTodayHighlightColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value != null && (bool)value) ? Color.FromArgb(AppSettings.Current.Settings.ThemeColor) : Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}