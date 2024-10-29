using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class CheckedChangedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var eventArgs = value as Microsoft.Maui.Controls.CheckBox;
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