using System.Globalization;

namespace iCampus.MobileApp.Behaviours;

public class ImageUrlToSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            var data = (byte[])value;
            return ImageSource.FromStream(() =>
            {
                return new MemoryStream(data);
            });
        }
        else
        {
            return null; // Return null if the value is null
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // This method is generally not needed for one-way bindings
        return null;
    }
}