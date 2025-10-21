using System;
using System.Globalization;
using Microsoft.Maui.Controls;
namespace iCampus.MobileApp.Behaviours;

public class UrlDecodeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string encodedFileName)
        {
            string decodedUrl = Uri.UnescapeDataString(encodedFileName);

            decodedUrl = decodedUrl.Replace("\\", "/");

            string fileName = Path.GetFileName(decodedUrl);
            
            return fileName;
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}