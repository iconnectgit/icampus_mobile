using System.Globalization;
using iCampus.Common.Helpers.Extensions;

namespace iCampus.MobileApp.Behaviours;

public class TitleFontWeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToBoolean() ? FontAttributes.None : FontAttributes.Bold;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return true;
    }
}