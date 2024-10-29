using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class SelectedItemEventArgsToSelectedItemConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Cast the value to SelectionChangedEventArgs (used in .NET MAUI)
        var eventArgs = value as SelectionChangedEventArgs;
        return eventArgs?.CurrentSelection?.FirstOrDefault(); // Return the selected item
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // Not needed for one-way binding
    }
}