using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

class SentMessageStatusToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string iconSource = string.Empty;
        iconSource= (bool)value ? "exclamatory_mark" : "tick_green";
        return iconSource;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return false;
    }
}