using System.Globalization;
using System.Text.RegularExpressions;

namespace iCampus.MobileApp.Behaviours;

public class HtmlTagRemoverConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            // Remove HTML tags using Regex
            return Regex.Replace(value.ToString(), "<.*?>", string.Empty);
        }
        return string.Empty; // Return empty string if value is null
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty; // Typically not used for one-way bindings
    }
}