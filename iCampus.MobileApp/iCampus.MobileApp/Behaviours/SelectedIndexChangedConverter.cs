using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class SelectedIndexChangedConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var eventArgs = value as Picker;
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