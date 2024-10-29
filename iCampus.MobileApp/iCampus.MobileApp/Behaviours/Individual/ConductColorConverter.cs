using System.Globalization;

namespace iCampus.MobileApp.Behaviours.Individual;

public class ConductColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null && value.ToString().ToLower().Contains("demerits"))
        {
            return "#ed7a75";
        }
        else if (value != null && value.ToString().ToLower().Contains("merits") && !value.ToString().ToLower().Contains("demerits"))
        {
            return "#48a28d";
        }
        else
            return "#61b9e0";
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}