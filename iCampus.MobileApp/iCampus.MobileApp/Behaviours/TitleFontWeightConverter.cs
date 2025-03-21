using System.Globalization;
using iCampus.Common.Helpers.Extensions;

namespace iCampus.MobileApp.Behaviours;

public class TitleFontWeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToBoolean() ? Colors.Gray : Colors.Black; // Red for unread messages
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Colors.Black;
    }
}