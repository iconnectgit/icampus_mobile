using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class ColorCodeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string colorCode)
        {
            // Ensure the color code starts with '#'
            colorCode = colorCode.Contains("#") ? colorCode : "#" + colorCode;
            return Color.FromArgb(colorCode); // Convert to MAUI Color
        }
            
        return Color.FromArgb("#000000"); // Default to black if value is null
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return "#000000"; // Always return black as a hex string
    }
}